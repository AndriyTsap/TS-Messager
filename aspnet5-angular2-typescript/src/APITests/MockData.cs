using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using PhotoGallery.Entities;

namespace APITests
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
                ChatUsers = new List<ChatUser>(),
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
                ChatUsers = new List<ChatUser>(),
                Messages = new List<Message>(),
                UserRoles = new List<UserRole>(),
            },
            new User()
            {
                Id = 1,
                Username = "IvanZ",
                Email = "ivan.zyola@gmail.com",
                BirthDate = "11.11.1995",
                DateCreated = DateTime.Now,
                HashedPassword = "9wsmLgYM5Gu4zA/BSpxK2GIBEWzqMPKs8wl2WDBzH/4=",
                IsLocked = false,
                Phone = "0666777333",
                Salt = "GTtKxJA6xJuj3ifJtTXn9Q==",
                ChatUsers = new List<ChatUser>(),
                Messages = new List<Message>(),
                UserRoles = new List<UserRole>(),
            }
        };

        public List<Chat> Grops = new List<Chat>
        {
            new Chat
            {
                Id = 0,
                Name = "PIt-15-03 Dialog",
                DateCreated = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                Messages = new List<Message>(),
                ChatUsers = new List<ChatUser>()
            },
            new Chat
            {
                Id = 1,
                Name = "Facken bursa",
                DateCreated = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                Messages = new List<Message>(),
                ChatUsers = new List<ChatUser>()
            }
        };

        public List<Message> Messages = new List<Message>
        {
            new Message()
            {
                Id = 0,
                Text = "Hi!",
                Date = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                ChatId = 0,
                SenderId = 0,
                Chat = new Chat(),
                User = new User()
            },
            new Message
            {
                Id = 1,
                Text = "Hi! How are you?",
                Date = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                ChatId = 0,
                SenderId = 1,
                Chat = new Chat(),
                User = new User()
            }
        };

        public List<ChatUser> GroupUsers = new List<ChatUser>
        {
            new ChatUser
            {
                Id = 0,
                UserId = 0,
                ChatId = 0,
                User = new User(),
                Chat = new Chat()
            },
            new ChatUser
            {
                Id = 1,
                UserId = 1,
                ChatId = 0,
                User = new User(),
                Chat = new Chat()
            },
            new ChatUser
            {
                Id = 2,
                UserId = 2,
                ChatId = 1,
                User = new User(),
                Chat = new Chat()
            },
            new ChatUser
            {
                Id = 3,
                UserId = 1,
                ChatId = 1,
                User = new User(),
                Chat = new Chat()
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

        public List<Role> Roles = new List<Role>()
        {
            new Role()
            {
                Id = 0,
                Name = "Admin"
            },
            new Role()
            {
                Id = 1,
                Name = "User"
            }
        };

        public List<UserRole> UserRoles = new List<UserRole>()
        {
            new UserRole()
            {
                Id = 0,
                UserId = 0,
                RoleId = 0,
                Role = new Role()
            },
            new UserRole()
            {
                Id = 1,
                UserId = 1,
                RoleId = 0,
                Role = new Role()
            },
            new UserRole()
            {
                Id = 2,
                UserId = 1,
                RoleId = 1,
                Role = new Role()
            }
        };
    }
}
