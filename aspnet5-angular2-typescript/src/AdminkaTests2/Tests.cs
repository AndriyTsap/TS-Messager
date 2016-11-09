﻿using System;
using System.Collections.Generic;
using System.IO;
using ConsoleApp1.Services;
using NUnit;
using NUnit.Framework;
using NUnit.Framework.Internal;
using PhotoGallery.Entities;
using System.Linq;
using System.Security.Cryptography;
using ConsoleApp1;
using ConsoleApp1.Contracts.Enities;
using ConsoleApp1.Contracts.Services;
using Moq;
using PhotoGallery.Infrastructure.Repositories;
using PhotoGallery.Infrastructure.Repositories.Abstract;
using PhotoGallery.ViewModels;
using PhotoGallery.Infrastructure.Services;
using PhotoGallery.Infrastructure.Services.Abstract;

namespace AdminkaTests2
{   
    [TestFixture]
    public class Tests
    {
        [Test]
        public void SomeMethod()
        {
            Assert.That(true);
        }

        [Test]
        public void Check_IfXMLSerializer_CanDeserialize_SerializedBefore_ListOfObjects()
        {
            //Arrange
            var serializer = new Serializer();
            var data = new MockData();
            var mockUsers = data.Users;
            var pathToMockFile = Directory.GetCurrentDirectory()+"\\Data\\MockFile.xml";

            //Act
            var dataString = serializer.Serialize(mockUsers);
            File.WriteAllText(pathToMockFile, dataString);

            var deserializedUsersList = serializer.Deserialize<List<User>>(pathToMockFile);

            //Assert
            Assert.That(mockUsers.SequenceEqual(deserializedUsersList));
        }

        [Test]
        public void Check_IfSerilizer_Serialize_AllSpectrumOfObjects()
        {
            //Arrange
            var serializer = new Serializer();
            var data = new MockData();
            var mockUsers = data.Users;
            var mockGroups = data.Grops;
            var mockMessages = data.Messages;
            var mockGroupUsers = data.GroupUsers;

            //Act
            var usersString = serializer.Serialize(mockUsers);
            var groupsString = serializer.Serialize(mockGroups);
            var messagesString = serializer.Serialize(mockMessages);
            var groupUsersString = serializer.Serialize(mockGroupUsers);

            //Assert
            Assert.That(!usersString.Equals("") && !groupsString.Equals("") && !groupUsersString.Equals("") && !messagesString.Equals(""));
        }

        [Test]
        public void Must_Be_Success_IfStorageSystemClass_WriteObjectsToFIles()
        {
            //Arrange
            var data = new MockData();
            var mockUsers = data.Users;
            var pathToMockFile = Directory.GetCurrentDirectory() + "\\Data\\MockFile.xml";

            var serializerMock = new Mock<ISerializer>();
            serializerMock.Setup(s => s.Serialize(It.IsAny<object>())).Returns("some string");
            serializerMock.Setup(s => s.Deserialize<List<User>>(It.IsAny<string>())).Returns(mockUsers);
            var storageSystem = new StorageSystem<User>(serializerMock.Object);
            storageSystem.SetFilePath(pathToMockFile);
            var mockUser = new User {Id = 3, Username = "Taras"};

            //act
            File.WriteAllText(pathToMockFile, String.Empty);

            storageSystem.Add(mockUser);
            var storagedData = storageSystem.GetAll().ToList();

            //assert
            Assert.That(storagedData.Count!=0);
        }

        [Test]
        public void Must_Be_Success_IdDIContainer_Resolve_Generic_Services()
        {
            //arrange
            var storageSystem = ServiceLocator.Instance.Resolve<IStorageSystem<User>>();
            var pathToMockFile = Directory.GetCurrentDirectory() + "\\Data\\MockFile.xml";
            storageSystem.SetFilePath(pathToMockFile);

            // act
            var users = storageSystem.GetAll().ToList();

            //assert
            Assert.That(users.Count!=0);
        }

        [Test]
        public void Must_Be_Success_If_LogerLogs_Messages()
        {
            //arrange
            var data = new MockData();
            var mockErrors = data.Errors;

            var mockErrorRepository = new Mock<ILoggingRepository>();
            Action<Error> addingAction = (error) => { mockErrors.Add(error); };
            Action commitingAction = () => { };

            mockErrorRepository.Setup(mer => mer.GetAll()).Returns(mockErrors);
            mockErrorRepository.Setup(mer => mer.Add(It.IsAny<Error>())).Callback(addingAction);
            mockErrorRepository.Setup(mer => mer.Commit()).Callback(commitingAction);

            var logger = new ConsoleApp1.Services.Logger(mockErrorRepository.Object);

            //act 
            logger.Log(new LogEntry(LoggingEventType.Information, mockErrors[0].Message));
            var errors = mockErrorRepository.Object.GetAll().ToList();

            //assert
            Assert.That(errors.LastOrDefault().Message == mockErrors.FirstOrDefault().Message);
        }

        [Test]
        public void MustBeSuccess_IfAcountService_LoginAndRegisterUsers()
        {
            //arrange
            var data = new MockData();

            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(repo => repo.GetSingleByUsername(It.IsAny<string>()))
                .Returns(new User()
                {
                    HashedPassword = "VfBM7WAgmlJhA25K7CMOqOcME8H/fXgI3bD9OSUB+00=",
                    Salt = "/E3qadielrSMtyT7YEpb2w=="
                });

            var mockEncriptionService = new Mock<IEncryptionService>();
            mockEncriptionService.Setup(encryptionService => encryptionService.EncryptPassword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns("VfBM7WAgmlJhA25K7CMOqOcME8H/fXgI3bD9OSUB+00=");
            mockEncriptionService.Setup(encryptionService => encryptionService.CreateSalt())
                .Returns("/E3qadielrSMtyT7YEpb2w==");

            int id = 0;
            var mockRoleRepository = new Mock<IRoleRepository>();
            mockRoleRepository.Setup(repo => repo.GetAll()).Returns(data.Roles);
            mockRoleRepository.Setup(repo => repo.GetSingle(id))
                .Returns(data.Roles.SingleOrDefault(role => role.Id == id));

            var mockUserRoleRepository = new Mock<IUserRoleRepository>();
            mockUserRoleRepository.Setup(repo => repo.GetAll()).Returns(data.UserRoles);
            mockUserRoleRepository.Setup(repo => repo.GetSingle(id))
                .Returns(data.UserRoles.SingleOrDefault(userRole => userRole.Id == id));

            AccountService service = new AccountService(mockUserRepository.Object, mockRoleRepository.Object,
                mockUserRoleRepository.Object, mockEncriptionService.Object);
            
            //act
            var loginResult =
                service.Login(new LoginViewModel() {Password = "kazantip", Username = "Andriy", RememberMe = true});

            var registerResult =
                service.Register(new RegistrationViewModel() {Username = "Misha", Email = "choliy.misha2gmail.com", Password = "mishacholiy"}, new []{0,1});
            
            //assert
            Assert.That(loginResult.Succeeded && registerResult.Succeeded);
            
        }

    }
}