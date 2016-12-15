using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Infrastructure;
using PhotoGallery.Entities;
using PhotoGallery.Hubs;
using PhotoGallery.Infrastructure.Core;
using PhotoGallery.Infrastructure.Repositories.Abstract;
using PhotoGallery.Infrastructure.Services.Abstract;
using PhotoGallery.ViewModels;

namespace PhotoGallery.Controllers
{
    [Route("api/[controller]")]
    public class MessagesController : ApiHubController<Broadcaster>
    {
        private readonly IMessageRepository _messageRepository;    
        private readonly ILoggingRepository _loggingRepository;
        private readonly IChatRepository _chatRepository;
        private readonly IJwtFormater _jwtFormater;
        private readonly IChatUserRepository _chatUserRepository;
        private readonly IUserRepository _userRepository;

        public MessagesController(ILoggingRepository loggingRepository, IMessageRepository messageRepository,
            IChatRepository chatRepository, IChatUserRepository chatUserRepository, IUserRepository userRepository,
            IJwtFormater jwtFormater, IConnectionManager signalRConnectionManager): base(signalRConnectionManager)
        {
            _chatRepository = chatRepository;
            _loggingRepository = loggingRepository;
            _messageRepository = messageRepository;
            _chatUserRepository = chatUserRepository;
            _userRepository = userRepository;
            _jwtFormater = jwtFormater;
        }

        // Get api/messages?offset=20
        [Authorize]
        [HttpGet]
        public async Task<IEnumerable<Message>> GetAll(int offset = 0)
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
            
            return messages.Skip(offset).Take(20);
        }

        // Get api/messages/chats?offset=20
        [Authorize]
        [HttpGet("chats")]
        public async Task<IEnumerable<Chat>> GetAllDialogs(int offset = 0)
        {
            var authenticationHeader = Request?.Headers["Authorization"];
            var token = authenticationHeader?.FirstOrDefault().Split(' ')[1];
            var subject = _jwtFormater.GetSubject(token);

            var user = _userRepository.GetSingleByUsername(subject);

            var chatUsers = await _chatUserRepository.FindByAsync(cu => cu.UserId == user.Id);
            var chatIds = chatUsers.Select(cu => cu.Id);
            var chats = await _chatRepository.FindByAsync(c => chatIds.Contains(c.Id));

            return chats.Skip(offset).Take(20);
        }
        /*
        // Get api/messages/createChat
        [Authorize]
        [HttpGet("createChat")]//don't work yet
        public async Task<IEnumerable<Chat>> CreateChat(int offset = 0)
        {
            IActionResult result = new ObjectResult(false);
            GenericResult removeResult = null;

            var authenticationHeader = Request?.Headers["Authorization"];
            var token = authenticationHeader?.FirstOrDefault().Split(' ')[1];
            var subject = _jwtFormater.GetSubject(token);

            var user = _userRepository.GetSingleByUsername(subject);
            /*
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
            this.Clients.Group(message.ChatId.ToString()).AddChatMessage(message);
            return result;
        }
        */

        // Get api/messages/getByChatId?chatId=1&offset=20
        [Authorize]
        [HttpGet("getByChatId")]
        public async Task<IEnumerable<Message>> GetMessagesByChatId(int chatId, int offset = 0)
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

            return messages.Skip(offset).Take(20);
        }


        // Post api/messages
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
                ChatId = mVMessage.ChatId,
                Date=DateTime.Now.ToString()
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
            //this.Clients.Group(message.ChatId.ToString()).AddChatMessage(message);
            return result;
        }

        // Delete api/messages
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