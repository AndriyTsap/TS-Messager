using System;
using Microsoft.EntityFrameworkCore;
using NUnit;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Moq;
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
        public void ShouldBeOk_IfIt_CanCheckUsers_OnFrienship()
        {
            var data = new MockData();

            var userRepo = ServiceLocator.Instance.Resolve<IUserRepository>();
            var chatRepo = ServiceLocator.Instance.Resolve<IChatRepository>();
            var chatUserRepo = ServiceLocator.Instance.Resolve<IChatUserRepository>();

            var friendsSearcher = new FriendsSearcher(userRepo, chatRepo, chatUserRepo);

            var friends = friendsSearcher.GetFriends(1);

            Assert.That(true);
        }
    }
}