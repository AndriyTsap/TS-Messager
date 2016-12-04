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

namespace APITests
{
    [TestFixture]
    public class FriendsSearcherTests
    {
        [Test]
        public async Task ShouldBeOk_IfIt_CanCheckUsers_OnFrienship()
        {
            var data = new MockData();

            var userRepo = ServiceLocator.Instance.Resolve<IUserRepository>();
            var chatRepo = ServiceLocator.Instance.Resolve<IChatRepository>();
            var chatUserRepo = ServiceLocator.Instance.Resolve<IChatUserRepository>();

            var friendsSearcher = new FriendsSearcher(userRepo, chatRepo, chatUserRepo);

            Console.WriteLine("ssssssssssssssssssssssssssssssssss");
            var friends = await friendsSearcher.GetFriends(1);
            Console.WriteLine(friends.Count());

            Assert.That(true);
        }
    }
}