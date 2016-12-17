using System;
using Microsoft.EntityFrameworkCore;
using NUnit;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework.Internal;
using Moq;
using PhotoGallery.Entities;
using PhotoGallery.Infrastructure;
using PhotoGallery.Infrastructure.Repositories;
using PhotoGallery.Infrastructure.Repositories.Abstract;
using PhotoGallery.Infrastructure.Services;
using PhotoGallery.Controllers;
using PhotoGallery.Infrastructure.Services.Abstract;
using Microsoft.AspNetCore.SignalR.Infrastructure;

namespace APITests
{
    [TestFixture]
    public class MessageControllerTests
    {
        [Test]
        public async Task ShoildByOk_IfGetMessagesByChatId_ReturnsShortSenderInfo()
        {
            var userRepo = ServiceLocator.Instance.Resolve<IUserRepository>();
            var loggingRepo = ServiceLocator.Instance.Resolve<ILoggingRepository>();
            var chatRepo = ServiceLocator.Instance.Resolve<IChatRepository>();
            var chatUserRepo = ServiceLocator.Instance.Resolve<IChatUserRepository>();
            var messageRepo = ServiceLocator.Instance.Resolve<IMessageRepository>();
            var jwtFormater = ServiceLocator.Instance.Resolve<IJwtFormater>();
            var signalRConnectionManager = ServiceLocator.Instance.Resolve<IConnectionManager>();

            //var messageController = new MessagesController(loggingRepo, messageRepo, chatRepo, chatUserRepo, userRepo, jwtFormater, signalRConnectionManager);
            var user = userRepo.GetSingleByUsername("Andriy");
            var chatUsers = await chatUserRepo.FindByAsync(cu => cu.UserId == user.Id);
            var chatIds = chatUsers.ToList().Select(cu => cu.ChatId);
            var chats = await chatRepo.FindByAsync(c => chatIds.Contains(c.Id));


            var res = chats.Skip(0).Take(20);

            foreach(var r in chats)
            {
                Console.WriteLine(r.Name);
            }

            Assert.That(res.Count()>0);
        }

        [Test]
        public async Task ShoildByOk_IfController_SentMessages()
        {
            var userRepo = ServiceLocator.Instance.Resolve<IUserRepository>();
            var loggingRepo = ServiceLocator.Instance.Resolve<ILoggingRepository>();
            var chatRepo = ServiceLocator.Instance.Resolve<IChatRepository>();
            var chatUserRepo = ServiceLocator.Instance.Resolve<IChatUserRepository>();
            var messageRepo = ServiceLocator.Instance.Resolve<IMessageRepository>();
            var jwtFormater = ServiceLocator.Instance.Resolve<IJwtFormater>();
            var signalRConnectionManager = ServiceLocator.Instance.Resolve<IConnectionManager>();

            //var messageController = new MessagesController(loggingRepo, messageRepo, chatRepo, chatUserRepo, userRepo, jwtFormater, signalRConnectionManager);
            var user = userRepo.GetSingleByUsername("Andriy");
            var chatUsers = await chatUserRepo.FindByAsync(cu => cu.UserId == user.Id);
            var chatIds = chatUsers.ToList().Select(cu => cu.ChatId);
            var chats = await chatRepo.FindByAsync(c => chatIds.Contains(c.Id));


            var res = chats.Skip(0).Take(20);

            foreach (var r in chats)
            {
                Console.WriteLine(r.Name);
            }

            Assert.That(res.Count() > 0);
        }
    }
}
