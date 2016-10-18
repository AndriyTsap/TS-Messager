using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PhotoGallery.Entities;
using PhotoGallery.Infrastructure;
using PhotoGallery.Infrastructure.Repositories;

namespace ConsoleApp1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PhotoGalleryContext>();
            optionsBuilder.UseSqlServer(
                "Server = (localdb)\\mssqllocaldb; Database = PhotoGalery; Trusted_Connection = True; MultipleActiveResultSets = true");
            var context = new PhotoGalleryContext(optionsBuilder.Options);
            var roleRepository = new RoleRepository(context);
            var userRepository = new UserRepository(context, roleRepository);

            

            //userRepository.Add(new User {Username = "Rostik", DateCreated = DateTime.Now, Email = "email", HashedPassword = "13abe32211", Salt = "234234234"});
            //userRepository.Commit();

            var users = userRepository.GetAll().ToList();

            Console.WriteLine(users.Last().Username);
            Console.ReadLine();
        }
    }
}
