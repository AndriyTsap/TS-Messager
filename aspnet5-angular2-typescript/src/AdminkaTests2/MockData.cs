using System;
using System.Collections.Generic;
using System.Globalization;
using ConsoleApp1.Contracts.Enities;
using NUnit.Framework;
using PhotoGallery.Entities;

namespace AdminkaTests2
{
    public class MockData
    {
        public List<User> Users = new List<User>
        {
            new User
            {
                Id = 0,
                Username = "Andriy",
                Email = "tsap.andriy@gmail.com",
                BirthDate = "16.10.1996",
                DateCreated = DateTime.Now,
                HashedPassword = "VfBM7WAgmlJhA25K7CMOqOcME8H/fXgI3bD9OSUB+00=",
                IsLocked = false,
                Phone = "0990920654",
                Salt = "/E3qadielrSMtyT7YEpb2w==",
                GroupUsers = new List<GroupUser>(),
                Messages = new List<Message>(),
                UserRoles = new List<UserRole>()
            },
            new User()
            {
                Id = 1,
                Username = "Ivan",
                Email = "Ivan.Kogut@gmail.com",
                BirthDate = "12.10.1993",
                DateCreated = DateTime.Now,
                HashedPassword = "9wsmLgYM5Gu4zA/BSpxK2GIBEWzqMPKs8wl2WDBzH/4=",
                IsLocked = false,
                Phone = "0666777333",
                Salt = "GTtKxJA6xJuj3ifJtTXn9Q==",
                GroupUsers = new List<GroupUser>(),
                Messages = new List<Message>(),
                UserRoles = new List<UserRole>(),
            }
        };

        public List<Group> Grops = new List<Group>
        {
            new Group
            {
                Id = 0,
                Name = "PIt-15-03 Dialog",
                DateCreated = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                Messages = new List<Message>(),
                GroupUsers = new List<GroupUser>()
            },
            new Group
            {
                Id = 1,
                Name = "Facken bursa",
                DateCreated = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                Messages = new List<Message>(),
                GroupUsers = new List<GroupUser>()
            }
        };

        public List<Message> Messages = new List<Message>
        {
            new Message()
            {
                Id = 0,
                Text = "Hi!",
                Date = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                GroupId = 0,
                SenderId = 0,
                Group = new Group(),
                User = new User()
            },
            new Message
            {
                Id = 1,
                Text = "Hi! How are you?",
                Date = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                GroupId = 0,
                SenderId = 1,
                Group = new Group(),
                User = new User()
            }
        };

        public List<GroupUser> GroupUsers = new List<GroupUser>
        {
            new GroupUser
            {
                Id = 0,
                UserId = 0,
                GroupId = 0,
                User = new User(),
                Group = new Group()
            },
            new GroupUser
            {
                Id = 1,
                UserId =1,
                GroupId = 0,
                User = new User(),
                Group = new Group()
            }
        };

        public List<Error> Errors = new List<Error>
        {
            new Error
            {
                Id = 0,
                DateCreated = DateTime.Now,
                Message = "Information message 1",
                Severity = LoggingEventType.Information.ToString(),
                StackTrace = ""
            },
            new Error
            {
                Id = 1,
                DateCreated = DateTime.Now,
                Message = "Error message 1",
                Severity = LoggingEventType.Error.ToString(),
                StackTrace = ""
            }
        };
    }
}