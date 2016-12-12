using NUnit.Framework;
using PhotoGallery.Infrastructure.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APITests
{
    public class UserRepositoryTests
    {

        [TestFixture]
        public class FriendsSearcherTests
        {
            [Test]
            public async Task ShouldChangePhone_AfterEditMethod()
            {
                var data = new MockData();
                var userRepo = ServiceLocator.Instance.Resolve<IUserRepository>();

                userRepo.Add(data.Users[0]); 
                userRepo.Commit();

                data.Users[0].Phone = "newPhone";
                userRepo.Edit(data.Users[0]);
                userRepo.Commit();
                userRepo.GetSingle(data.Users[0].Id);

                Assert.That(userRepo.GetSingle(data.Users[0].Id).Phone== "newPhone");
            }
        }
    }
}
