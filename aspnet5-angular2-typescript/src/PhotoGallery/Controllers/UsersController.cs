using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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

        // GET: api/users
        [HttpGet]
        public async Task<dynamic> GetAll()
        {
            var users = await _userRepository.GetAllAsync();
            var dataForView = users.Select(u => new {u.Username, u.BirthDate, u.Phone, u.Photo});
            return dataForView;
        }

        // GET api/users/2
        [HttpGet("{id}")]
        public async Task<dynamic> Get(int id)
        {
            //var isFriend = false;

            /*var authenticationHeader = Request?.Headers["Authorization"];
            
            if(authenticationHeader?.Count!=0)
            {
                var token = authenticationHeader?.FirstOrDefault().Split(' ')[1];
                var jwt = new JwtSecurityToken(token);
                var subject = jwt?.Subject;           
                var subjectId = _userRepository.GetSingleByUsername(subject).Id;
                isFriend = _friendsSearcher.ValidateFriend(subjectId, id);
            }*/
                
            var repoUser = await _userRepository.FindByAsync(u => u.Id == id);
            var user = repoUser.FirstOrDefault();
            var dataForView = new {user.Username, user.BirthDate, user.Phone, user.Photo};
            return dataForView;
        }

        // Get api/users/search?
        [HttpGet("search")]
        public async Task<IEnumerable<dynamic>> SearchUsers(string username = "")
        {
            var repoUsers = await _userRepository.FindByAsync(u => u.Username.StartsWith(username));
            var users = repoUsers.Select(u => new {u.Username, u.Phone, u.Photo, u.BirthDate});
            return users;
        }

        [Authorize]
        [HttpPost("editPersonalData")]
        public IActionResult Post([FromBody] User user)
        {
            IActionResult result = new ObjectResult(false);
            GenericResult editResult = null;

            var authenticationHeader = Request?.Headers["Authorization"];
            var token = authenticationHeader?.FirstOrDefault().Split(' ')[1];
            var jwt = new JwtSecurityToken(token);
            var subject = jwt.Subject;
            var dbUser = _userRepository.GetSingleByUsername(subject);

            user.Username = subject;
            user.HashedPassword = dbUser.HashedPassword;
            user.Salt = dbUser.Salt;

            try
            {
                _userRepository.Edit(user);
                _userRepository.Commit();

                editResult = new GenericResult()
                {
                    Succeeded = true,
                    Message = "Message removed."
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
        public IActionResult Delete([FromBody] int id)
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
                    Message = "Message removed."
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
