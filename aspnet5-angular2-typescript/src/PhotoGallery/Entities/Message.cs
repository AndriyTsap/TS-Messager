using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace PhotoGallery.Entities
{
    public class Message : IEntityBase
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Date { get; set; }
        public int SenderId { get; set; }
        public int GroupId { get; set; }

        public virtual User User { get; set; }
        public virtual Group Group { get; set; }
    }
}
