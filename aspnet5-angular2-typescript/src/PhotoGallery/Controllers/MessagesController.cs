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
using PhotoGallery.Infrastructure.Services.Abstract;
using PhotoGallery.ViewModels;

namespace PhotoGallery.Controllers
{
    [Route("api/[controller]")]
    public class MessagesController : Controller
    {
        private readonly IMessageRepository _messageRepository;
        private readonly ILoggingRepository _loggingRepository;
        private readonly IMessageFactory _messageFactory;
        private readonly IGroupRepository _groupRepository;

        public MessagesController(ILoggingRepository loggingRepository, IMessageRepository messageRepository,
            IMessageFactory messageFactory, IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
            _messageFactory = messageFactory;
            _loggingRepository = loggingRepository;
            _messageRepository = messageRepository;
        }

        [HttpGet("chats")]
        public IEnumerable<Group> GetAllDialogs()
        {
            return _groupRepository.GetAll();
        }



        [HttpGet("chats/{dialogId:int)")]
        public IEnumerable<Group> GetTop20ChatsByChatId(int? dialogId)
        {
            IEnumerable<Group> groups = null;
            
            try
            {
                groups = _groupRepository
                    .AllIncluding(m => m)
                    .OrderBy(m => m.Id)
                    .Take(20);
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

            return groups;
        }



        [HttpPost]
        public IActionResult Send(MessageViewModel vMMessage)
        {
            IActionResult result = new ObjectResult(false);
            GenericResult removeResult = null;

            var message = _messageFactory.CreateMessage(vMMessage.Text, vMMessage.SenderId, vMMessage.GroupId);

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
                _messageRepository.Delete(new Message() {Id = messageId});
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