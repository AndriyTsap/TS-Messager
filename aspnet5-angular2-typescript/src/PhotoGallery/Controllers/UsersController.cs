using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhotoGallery.Entities;
using PhotoGallery.Infrastructure.Core;
using PhotoGallery.Infrastructure.Repositories.Abstract;
using PhotoGallery.Infrastructure.Services.Abstract;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PhotoGallery.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IChatRepository _chatRepository;
        private readonly IChatUserRepository _chatUserRepository;
        private readonly ILoggingRepository _loggingRepository;
        private readonly IFriendsSearcher _friendsSearcher;

        public UsersController(IUserRepository userRepository, ILoggingRepository loggingRepository,
            IChatRepository chatRepository, IChatUserRepository chatUserRepository, IFriendsSearcher friendsSearcher)
        {
            _chatUserRepository = chatUserRepository;
            _chatRepository = chatRepository;
            _userRepository = userRepository;
            _loggingRepository = loggingRepository;
            _friendsSearcher = friendsSearcher;
        }

        // GET: api/users?offset=20
        [HttpGet]
        public async Task<dynamic> GetAll(int offset = 0)
        {
            var users = await _userRepository.GetRangeWithOffsetAsync(offset);
            var dataForView = users.Select(u => new {u.Username, u.FirstName, u.Email, u.LastName, u.BirthDate, u.Phone, u.Photo, u.About});
            return dataForView;
        }

        // GET api/users/getById?id=1
        [HttpGet("getById")]
        public async Task<dynamic> Get(int id)
        {
            var repoUser = await _userRepository.FindByAsync(u => u.Id == id);
            var user = repoUser.FirstOrDefault();
            var dataForView = new {user.Id, user.Username, user.Email, user.FirstName, user.LastName, user.BirthDate, user.Phone, user.Photo, user.About};
            return dataForView;
        }

        // GET api/users/getByToken
        [Authorize]
        [HttpGet("getByToken")]
        public async Task<dynamic> GetByToken()
        {
            var authenticationHeader = Request?.Headers["Authorization"];
            var token = authenticationHeader?.FirstOrDefault().Split(' ')[1];
            var jwt = new JwtSecurityToken(token);
            var subject = jwt.Subject;
            var repoUser = await _userRepository.FindByAsync(u => u.Username == subject);
            var user = repoUser.FirstOrDefault();
            var dataForView = new {user.Id, user.Username, user.Email, user.FirstName, user.LastName, user.BirthDate, user.Phone, user.Photo, user.About};
            return dataForView;
        }

        // Get api/users/checkOnFriendship?id=1
        [Authorize]
        [HttpGet("checkOnFriendship")]
        public async Task<bool> CheckOnFriendship(int id)
        {
            var isFriend = false;

            var authenticationHeader = Request.Headers["Authorization"];
            var token = authenticationHeader.FirstOrDefault().Split(' ')[1];
            var jwt = new JwtSecurityToken(token);
            var subject = jwt.Subject;
            var subjectId = _userRepository.GetSingleByUsername(subject).Id;
            isFriend = await _friendsSearcher.ValidateFriend(subjectId, id);

            return isFriend;
        }

        // Get api/users/search?username=Andriy
        [HttpGet("search")]
        public async Task<IEnumerable<dynamic>> SearchUsers(string username = "")
        {
            var repoUsers = await _userRepository.FindByAsync(u => u.Username.StartsWith(username));
            var users = repoUsers.Select(u => new {u.Username, u.Phone, u.Photo, u.BirthDate});
            return users;
        }

        [Authorize]
        [HttpPut("editPersonalData")]
        public IActionResult Post([FromBody] User user)
        {
            IActionResult result = new ObjectResult(false);
            GenericResult editResult = null;
            
            var authenticationHeader = Request?.Headers["Authorization"];
            var token = authenticationHeader?.FirstOrDefault().Split(' ')[1];
            var jwt = new JwtSecurityToken(token);
            var subject = jwt.Subject;
            var dbUser = _userRepository.GetSingleByUsername(subject);
           
            user.HashedPassword = dbUser.HashedPassword;
            user.Salt = dbUser.Salt;
            
            try
            {
                _userRepository.Edit(user);
                _userRepository.Commit();

                editResult = new GenericResult()
                {
                    Succeeded = true,
                    Message = "User updated."
                };
            }
            catch (Exception ex)
            {
                editResult = new GenericResult()
                {
                    Succeeded = false,
                    Message = ex.Message
                };

                _loggingRepository.Add(new Error()
                {
                    Message = ex.Message,
                    StackTrace = ex.StackTrace,
                    DateCreated = DateTime.Now
                });
                _loggingRepository.Commit();

            }

            result = new ObjectResult(editResult);
            return result; 
        }
        
        [HttpDelete("delete")]
        [Authorize]
        public IActionResult Delete()
        {
            IActionResult result = new ObjectResult(false);
            GenericResult removeResult = null;

            var authenticationHeader = Request?.Headers["Authorization"];
            var token = authenticationHeader?.FirstOrDefault().Split(' ')[1];
            var jwt = new JwtSecurityToken(token);
            var subject = jwt.Subject;

            var user = _userRepository.GetSingleByUsername(subject);

            try
            {
                _userRepository.Delete(user);
                _userRepository.Commit();

                removeResult = new GenericResult()
                {
                    Succeeded = true,
                    Message = "User removed."
                };
            }
            catch (Exception ex)
            {
                removeResult = new GenericResult()
                {
                    Succeeded = false,
                    Message = ex.Message
                };

                _loggingRepository.Add(new Error()
                {
                    Message = ex.Message,
                    StackTrace = ex.StackTrace,
                    DateCreated = DateTime.Now
                });
                _loggingRepository.Commit();

            }

            result = new ObjectResult(removeResult);
            return result;
        }
    }
}
