using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhotoGallery.Entities;
using PhotoGallery.Infrastructure.Core;
using PhotoGallery.Infrastructure.Repositories.Abstract;
using PhotoGallery.Infrastructure.Services.Abstract;
using PhotoGallery.ViewModels;

namespace PhotoGallery.Controllers
{
    [Route("api/[controller]")]
    public class MessagesController : Controller
    {
        private readonly IMessageRepository _messageRepository;
        private readonly ILoggingRepository _loggingRepository;
        private readonly IChatRepository _chatRepository;
        private readonly IJwtFormater _jwtFormater;
        private readonly IChatUserRepository _chatUserRepository;
        private readonly IUserRepository _userRepository;



        public MessagesController(ILoggingRepository loggingRepository, IMessageRepository messageRepository,
            IChatRepository chatRepository, IChatUserRepository chatUserRepository, IUserRepository userRepository,
            IJwtFormater jwtFormater)
        {
            _chatRepository = chatRepository;
            _loggingRepository = loggingRepository;
            _messageRepository = messageRepository;
            _chatUserRepository = chatUserRepository;
            _userRepository = userRepository;
            _jwtFormater = jwtFormater;
        }

        [Authorize]
        [HttpGet]
        public async Task<IEnumerable<Message>> GetAll()
        {
            var authenticationHeader = Request?.Headers["Authorization"];
            var token = authenticationHeader?.FirstOrDefault().Split(' ')[1];
            var subject = _jwtFormater.GetSubject(token);

            var user = _userRepository.GetSingleByUsername(subject);

            IEnumerable<Message> messages = new List<Message>();
            try
            {
                var chats = await _chatUserRepository.FindByAsync(cu => cu.UserId == user.Id);
                var chatIds = chats.Select(cu => cu.ChatId);
                messages = await _messageRepository.FindByAsync(message => chatIds.Contains(message.ChatId));
            }
            catch (Exception ex)
            {
                _loggingRepository.Add(new Error()
                {
                    Severity = "Error",
                    Message = ex.Message,
                    StackTrace = ex.StackTrace,
                    DateCreated = DateTime.Now
                });
                _loggingRepository.Commit();
            }
            
            return messages;
        }

        [Authorize]
        [HttpGet("chats")]
        public IEnumerable<Chat> GetAllDialogs()
        {
            var authenticationHeader = Request?.Headers["Authorization"];
            var token = authenticationHeader?.FirstOrDefault().Split(' ')[1];
            var subject = _jwtFormater.GetSubject(token);

            var user = _userRepository.GetSingleByUsername(subject);

            var chatIds = _chatUserRepository.FindBy(cu => cu.UserId == user.Id).Select(cu => cu.ChatId);
            var chats = _chatRepository.FindBy(c => chatIds.Contains(c.Id));

            return chats;
        }


        [Authorize]
        [HttpGet("getByChatId")]
        public async Task<IEnumerable<Message>> GetMessagesByChatId(int chatId)
        {
            IEnumerable<Message> messages = null;

            var authenticationHeader = Request?.Headers["Authorization"];
            var token = authenticationHeader?.FirstOrDefault().Split(' ')[1];
            var subject = _jwtFormater.GetSubject(token);

            var user = _userRepository.GetSingleByUsername(subject);

            var userChats = await _chatUserRepository.FindByAsync(cu => cu.UserId == user.Id);
            var chatIds = userChats.Select(uc => uc.ChatId).ToList();
            
            if (!chatIds.Contains(chatId))
            {
                _loggingRepository.Add(new Error()
                {
                    Severity = "Warning",
                    Message = "Request from "+subject+" to not his chat",
                    DateCreated = DateTime.Now
                });
                _loggingRepository.Commit();
                throw new Exception("Access denied");
            }

            try
            {
                messages =  await _messageRepository
                    .FindByAsync(m => m.ChatId == chatId);
            }
            catch (Exception ex)
            {
                _loggingRepository.Add(new Error()
                {
                    Severity = "Error",
                    Message = ex.Message,
                    StackTrace = ex.StackTrace,
                    DateCreated = DateTime.Now
                });
                _loggingRepository.Commit();
            }

            return messages;
        }



        [HttpPost]
        [Authorize]
        public IActionResult Send(MessageViewModel mVMessage)
        {
            IActionResult result = new ObjectResult(false);
            GenericResult removeResult = null;

            var authenticationHeader = Request?.Headers["Authorization"];
            var token = authenticationHeader?.FirstOrDefault().Split(' ')[1];
            var subject = _jwtFormater.GetSubject(token);

            var user = _userRepository.GetSingleByUsername(subject);

            var message = new Message()
            {
                Text = mVMessage.Text,
                SenderId = user.Id,
                ChatId = mVMessage.ChatId
            };

            try
            {
                _messageRepository.Add(message);
                _messageRepository.Commit();

                removeResult = new GenericResult()
                {
                    Succeeded = true,
                    Message = "Message sended."
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
                    Severity = "Error",
                    Message = ex.Message,
                    StackTrace = ex.StackTrace,
                    DateCreated = DateTime.Now
                });
                _loggingRepository.Commit();
            }

            result = new ObjectResult(removeResult);
            return result;
        }

        [Authorize]
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(int id)
        {
            IActionResult result = new ObjectResult(false);
            GenericResult removeResult = null;

            var authenticationHeader = Request?.Headers["Authorization"];
            var token = authenticationHeader?.FirstOrDefault().Split(' ')[1];
            var subject = _jwtFormater.GetSubject(token);

            var user = _userRepository.GetSingleByUsername(subject);

            var userChats = await _chatUserRepository.FindByAsync(uc => uc.UserId == user.Id);
            var messages = await _messageRepository.FindByAsync(m => m.Id == id);
            var message = messages.FirstOrDefault();

            var chatIds = userChats.Select(uc => uc.ChatId);

            if (!chatIds.Contains(message.ChatId))
            {
                _loggingRepository.Add(new Error()
                {
                    Severity = "Warning",
                    Message = "Request from " + subject + " to not his message",
                    DateCreated = DateTime.Now
                });

                _loggingRepository.Commit();
                removeResult = new GenericResult()
                {
                    Succeeded = false,
                    Message = "Access denied"
                };

                result = new ObjectResult(removeResult);
                return result;
            }

            try
            {
                _messageRepository.Delete(new Message() { Id = id });
                _messageRepository.Commit();

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