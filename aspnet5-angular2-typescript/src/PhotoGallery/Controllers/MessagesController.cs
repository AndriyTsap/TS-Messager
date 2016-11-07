using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PhotoGallery.Entities;
using PhotoGallery.Infrastructure.Core;
using PhotoGallery.Infrastructure.Repositories;
using PhotoGallery.Infrastructure.Repositories.Abstract;
using PhotoGallery.ViewModels;

namespace PhotoGallery.Controllers
{
    [Route("api/[controller]")]
    public class MessagesController : Controller
    {
        private readonly IMessageRepository _messageRepository;
        private readonly ILoggingRepository _loggingRepository;
        private readonly IGroupRepository _groupRepository;

        public MessagesController(ILoggingRepository loggingRepository, IMessageRepository messageRepository,
            IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
            _loggingRepository = loggingRepository;
            _messageRepository = messageRepository;
        }

        [HttpGet("chats")]
        public IEnumerable<Group> GetAllDialogs()
        {
            return _groupRepository.GetAll();
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
                    .Where(m => m.GroupId == chatId);
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
                GroupId = vMMessage.GroupId
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