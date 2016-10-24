using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PhotoGallery.Entities;
using PhotoGallery.Infrastructure;
using PhotoGallery.Infrastructure.Repositories;
using ServiceStack.Text;
using Microsoft.Extensions.DependencyInjection;
using ConsoleApp1.CSVManager;

namespace ConsoleApp1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IServiceCollection services = new ServiceCollection();

            services.AddDbContext<PhotoGalleryContext>(options =>
                options.UseSqlServer(AppSettings.Instance.ConnectionString));

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();

            IServiceProvider provider = services.BuildServiceProvider();
            
            var userRepository = provider.GetService<IUserRepository>();
            
            //userRepository.Add(new User {Username = "Rostik", DateCreated = DateTime.Now, Email = "email", HashedPassword = "13abe32211", Salt = "234234234"});
            //userRepository.Commit();

            //CSVManager.CSVManager csvManager = new CSVManager.CSVManager();

            var users = userRepository.GetAll().ToList();
            //users.AddRange(csvManager.ImportUsers("users.csv"));

            //csvManager.ExportUsersToCSV(users,"users2.csv");
            Console.WriteLine(users.Last().Username);

            //var csv = CsvSerializer.SerializeToCsv(new[]{
            //    new User ()
            //    {
            //        Id = 0,
            //        Username = "Andriy"
            //    }
            //});

            //Console.WriteLine(csv);

            Console.ReadLine();
        }
    }
}
