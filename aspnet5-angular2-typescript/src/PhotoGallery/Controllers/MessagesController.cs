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
using PhotoGallery.ViewModels;

namespace PhotoGallery.Controllers
{
    [Route("api/[controller]")]
    public class MessagesController: Controller
    {
        IMessageRepository _messageRepository;
        ILoggingRepository _loggingRepository;

        public MessagesController(ILoggingRepository loggingRepository, IMessageRepository messageRepository)
        {
            _loggingRepository = loggingRepository;
            _messageRepository = messageRepository;
        }

        [HttpGet("dialogId:int")]
        public IEnumerable<Message> GetAll(int? dialogId)
        {
            List<Message> _messages = null;

            try
            {
                
                int _totalMessages = new int();

                _totalMessages = _messageRepository.GetAll().Count();

                _messages = _messageRepository
                    .AllIncluding(m => m)
                    .OrderBy(m => m.Id)
                    .Skip(_totalMessages - 20).ToList();
            }
            catch (Exception ex)
            {
                _loggingRepository.Add(new Error() { Message = ex.Message, StackTrace = ex.StackTrace, DateCreated = DateTime.Now });
                _loggingRepository.Commit();
            }

            return _messages;
        }

        [HttpPost]
        public IActionResult SendMessage(string text, int groupToId, int senderId)
        {
            Message message = new Message
            {
                Text = text,
                GroupId = groupToId,
                SenderId = senderId
            };

            try
            {
                int messageId = _messageRepository
                    .AllIncluding(m => m)
                    .Last().Id+1;

                message.Id = messageId;

                _messageRepository.Add(message);
                return Ok();
            }
            catch (Exception ex)
            {
                _loggingRepository.Add(new Error() { Message = ex.Message, StackTrace = ex.StackTrace, DateCreated = DateTime.Now });
                _loggingRepository.Commit();
            }

            return NotFound();
        }
    }
}