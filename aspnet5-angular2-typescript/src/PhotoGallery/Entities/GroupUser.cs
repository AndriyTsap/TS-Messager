using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoGallery.Entities
{
    public class GroupUser : IEntityBase
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int GroupId { get; set; }

        public virtual User User { get; set; }
        public virtual Group Group { get; set; }

        public override bool Equals(object obj)
        {
            var groupUser = obj as GroupUser;
            return groupUser != null && Id == groupUser.Id && GroupId == groupUser.GroupId && UserId == groupUser.UserId;
        }
    }
}
