﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PhotoGallery.Entities;
using PhotoGallery.Infrastructure.Repositories.Abstract;

namespace PhotoGallery.Infrastructure.Repositories
{
    public class GroupRepository :EntityBaseRepository<Group>, IGroupRepository
    {
        public GroupRepository(PhotoGalleryContext context)
            : base(context)
        { }
    }
}
