﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PhotoGallery.Entities;

namespace PhotoGallery.Infrastructure.Repositories
{
    public class GroupUserRepository :EntityBaseRepository<GroupUser>, IGroupUserRepository
    {
        public GroupUserRepository(PhotoGalleryContext context)
            : base(context)
        { }
    }
}
