using Microsoft.EntityFrameworkCore;
using PhotoGallery.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace PhotoGallery.Infrastructure
{
    public class PhotoGalleryContext : DbContext
    {
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupUser> GroupUsers { get; set; }
        public DbSet<Error> Errors { get; set; }

        public PhotoGalleryContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                entity.Relational().TableName = entity.DisplayName();
            }

            // Photos
            modelBuilder.Entity<Photo>().Property(p => p.Title).HasMaxLength(100);
            modelBuilder.Entity<Photo>().Property(p => p.AlbumId).IsRequired();

            // Album
            modelBuilder.Entity<Album>().Property(a => a.Title).HasMaxLength(100);
            modelBuilder.Entity<Album>().Property(a => a.Description).HasMaxLength(500);
            modelBuilder.Entity<Album>().HasMany(a => a.Photos).WithOne(p => p.Album);

            // User
            modelBuilder.Entity<User>().Property(u => u.Username).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<User>().Property(u => u.Email).IsRequired().HasMaxLength(200);
            modelBuilder.Entity<User>().Property(u => u.HashedPassword).IsRequired().HasMaxLength(200);
            modelBuilder.Entity<User>().Property(u => u.Salt).IsRequired().HasMaxLength(200);
            modelBuilder.Entity<User>().HasMany(u => u.GroupUsers).WithOne(gu => gu.User);
            modelBuilder.Entity<User>().HasMany(u => u.Messages).WithOne(m => m.User);

            //Message
            modelBuilder.Entity<Message>().Property(m => m.Text).IsRequired().HasMaxLength(500);
            modelBuilder.Entity<Message>().Property(m => m.GroupId).IsRequired();
            modelBuilder.Entity<Message>().Property(m => m.SenderId).IsRequired();

            //Group
            modelBuilder.Entity<Group>().Property(g => g.Name).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Group>().HasMany(g => g.Messages).WithOne(m => m.Group);
            modelBuilder.Entity<Group>().HasMany(g => g.GroupUsers).WithOne(gu => gu.Group);

            //GroupUser
            modelBuilder.Entity<GroupUser>().Property(gu => gu.GroupId).IsRequired();
            modelBuilder.Entity<GroupUser>().Property(gu => gu.UserId).IsRequired();

            // UserRole
            modelBuilder.Entity<UserRole>().Property(ur => ur.UserId).IsRequired();
            modelBuilder.Entity<UserRole>().Property(ur => ur.RoleId).IsRequired();

            // Role
            modelBuilder.Entity<Role>().Property(r => r.Name).IsRequired().HasMaxLength(50);
        }
    }
}
