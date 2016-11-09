using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Mvc;
using PhotoGallery.Entities;
using PhotoGallery.Infrastructure.Core;
using PhotoGallery.Infrastructure.Repositories;
using PhotoGallery.Infrastructure.Repositories.Abstract;
using PhotoGallery.Infrastructure.Services;
using PhotoGallery.ViewModels;

namespace PhotoGallery.Controllers
{
    [Route("api/[controller]")]
    public class MessagesController : Controller
    {
        private readonly IMessageRepository _messageRepository;
        private readonly ILoggingRepository _loggingRepository;
        private readonly IChatRepository _chatRepository;
        private readonly IChatUserRepository _chatUserRepository;
        private readonly IUserRepository _userRepository;
        
        public MessagesController(ILoggingRepository loggingRepository, IMessageRepository messageRepository,
            IChatRepository chatRepository, IChatUserRepository chatUserRepository, IUserRepository userRepository) 
        {
            _chatRepository = chatRepository;
            _loggingRepository = loggingRepository;
            _messageRepository = messageRepository;
            _chatUserRepository = chatUserRepository;
            _userRepository = userRepository;
        }


        [Authorize]
        [HttpGet]
        public IEnumerable<Message> GetAll()
        {
            var authenticationHeader = Request.Headers["Authorization"];
            var token = authenticationHeader.FirstOrDefault().Split(' ')[1];
            var jwtToken = new JwtSecurityToken(token);
            var subject = jwtToken.Subject;

            var user = _userRepository.GetSingleByUsername(subject);

            var chats = _chatUserRepository.FindBy(cu => cu.UserId == user.Id).Select(cu => cu.ChatId);
            var messages = _messageRepository.FindBy(message => chats.Contains(message.ChatId));
            return messages;
        }

        [Authorize]
        [HttpGet("chats")]
        public IEnumerable<Chat> GetAllDialogs()
        {
            var authenticationHeader = Request.Headers["Authorization"];
            var token = authenticationHeader.FirstOrDefault().Split(' ')[1];
            var jwtToken = new JwtSecurityToken(token);
            var subject = jwtToken.Subject;

            var user = _userRepository.GetSingleByUsername(subject);

            var chatIds = _chatUserRepository.FindBy(cu => cu.UserId == user.Id).Select(cu => cu.ChatId);
            var chats = _chatRepository.FindBy(c => chatIds.Contains(c.Id));

            return chats;
        }



        [HttpGet("{chatId:int}")]
        public IEnumerable<Message> GetMessagesByChatId(int? chatId)
        {
            IEnumerable<Message> messages = null;

            try
            {
                messages = _messageRepository
                    .AllIncluding(m => m)
                    .OrderBy(m => m.Id)
                    .Take(20)
                    .Where(m => m.ChatId == chatId);
            }
            catch (Exception ex)
            {
                _loggingRepository.Add(new Error()
                {
                    Message = ex.Message,
                    StackTrace = ex.StackTrace,
                    DateCreated = DateTime.Now
                });
                _loggingRepository.Commit();
            }

            return messages;
        }



        [HttpPost]
        public IActionResult Send(MessageViewModel vMMessage)
        {
            IActionResult result = new ObjectResult(false);
            GenericResult removeResult = null;

            var message = new Message()
            {
                Text = vMMessage.Text,
                SenderId = vMMessage.SenderId,
                ChatId = vMMessage.GroupId
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
                    Message = ex.Message,
                    StackTrace = ex.StackTrace,
                    DateCreated = DateTime.Now
                });
                _loggingRepository.Commit();
            }

            result = new ObjectResult(removeResult);
            return result;
        }

        [HttpDelete("{messageId:int}")]
        public IActionResult Delete(int messageId)
        {
            IActionResult result = new ObjectResult(false);
            GenericResult removeResult = null;

            try
            {
                _messageRepository.Delete(new Message() { Id = messageId });
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