using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoGallery.Entities
{
    public class Group : IEntityBase
    {
        public Group ()
        {
            GroupUsers = new List<GroupUser>();
            Messages = new List<Message>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string DateCreated { get; set; }

        public virtual List<GroupUser> GroupUsers { get; set; }
        public virtual List<Message> Messages { get; set; }

        public override bool Equals(object obj)
        {
            var group = obj as Group;
            return group != null && Id == group.Id && Name == group.Name && DateCreated == group.DateCreated;
        }
    }
}
