﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PhotoGallery.Entities;

namespace PhotoGallery.Infrastructure.Repositories
{
    public class MessageRepository: EntityBaseRepository<Message>, IMessageRepository
    {
        public MessageRepository(PhotoGalleryContext context)
            : base(context)
        { }
    }
}
