using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoGallery.Entities
{
    public class User : IEntityBase
    {
        public User()
        {
            UserRoles = new List<UserRole>();
            GroupUsers = new List<GroupUser>();
            Messages = new List<Message>();
        }
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string BirthDate { get; set; }
        public string HashedPassword { get; set; }
        public string Salt { get; set; }
        public bool IsLocked { get; set; }
        public DateTime DateCreated { get; set; }

        public virtual List<UserRole> UserRoles { get; set; }
        public virtual List<GroupUser> GroupUsers { get; set; }
        public virtual List<Message> Messages { get; set; }

        public override bool Equals(object obj)
        {
            var user = obj as User;
            return user != null && Id == user.Id && Username == user.Username;
        }
    }
}
