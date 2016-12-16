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

namespace APITests
{
    [TestFixture]
    public class FriendsSearcherTests
    {
        [Test]
        public async Task ShouldBeOk_IfIt_CanCheckUsers_OnFrienship()
        {
            var userRepo = ServiceLocator.Instance.Resolve<IUserRepository>();
            var chatRepo = ServiceLocator.Instance.Resolve<IChatRepository>();
            var chatUserRepo = ServiceLocator.Instance.Resolve<IChatUserRepository>();

            var friendsSearcher = new FriendsSearcher(userRepo, chatRepo, chatUserRepo);

            var val = await friendsSearcher.ValidateFriend(1, 2);

            Assert.That(val);

        }

        [Test]
        public async Task ShouldBeOk_ifIt_CanReturn_Friends()
        {
            var userRepo = ServiceLocator.Instance.Resolve<IUserRepository>();
            var loggingRepo = ServiceLocator.Instance.Resolve<ILoggingRepository>();
            var chatRepo = ServiceLocator.Instance.Resolve<IChatRepository>();
            var chatUserRepo = ServiceLocator.Instance.Resolve<IChatUserRepository>();

            var friendsSearcher = new FriendsSearcher(userRepo, chatRepo, chatUserRepo);
            var controller = new UsersController(userRepo, loggingRepo,
                chatRepo, chatUserRepo, friendsSearcher);
            
            var friends = await controller.GetFriends();

            foreach(var f in friends)
            {
                Console.WriteLine(f.Username);
            }

            Assert.That(friends.Count() != 0);
        }
    }
}