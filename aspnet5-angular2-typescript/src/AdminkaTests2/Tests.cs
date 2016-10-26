using System;
using System.Collections.Generic;
using System.IO;
using ConsoleApp1.Services;
using NUnit;
using NUnit.Framework;
using NUnit.Framework.Internal;
using PhotoGallery.Entities;
using System.Linq;

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
    }
}