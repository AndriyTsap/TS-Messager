using System;
using System.Linq;

using System.Threading.Tasks;
using ConsoleApp1.Contracts.Services;

using Microsoft.EntityFrameworkCore;
using PhotoGallery.Infrastructure;
using PhotoGallery.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using ConsoleApp1.Services;

using ServiceStack.Text;
using ConsoleApp1.Services;

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

            CsvManager csvManager = new CsvManager();
            AppSettings appSettings = AppSettings.Instance;
            var users = userRepository.GetAll().ToList();
            try
            {
                users.AddRange(csvManager.ImportUsers(appSettings.UserFilePathForImport));
                csvManager.ExportUsersToCSV(users, appSettings.UserFilePathForExport);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "");
            }
          
            Console.WriteLine(users.Last().Username);

            Console.ReadLine();
        }
    }
}
