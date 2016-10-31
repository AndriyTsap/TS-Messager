using System;
using System.Linq;
<<<<<<< HEAD
=======
using System.Threading.Tasks;
using ConsoleApp1.Contracts.Services;
>>>>>>> a63993ebd0d3d69079347393ae9b54975c2e3665
using Microsoft.EntityFrameworkCore;
using PhotoGallery.Infrastructure;
using PhotoGallery.Infrastructure.Repositories;
<<<<<<< HEAD
using Microsoft.Extensions.DependencyInjection;

using ConsoleApp1.Services;
=======
using ServiceStack.Text;
using ConsoleApp1.CSVManager;
>>>>>>> a63993ebd0d3d69079347393ae9b54975c2e3665

namespace ConsoleApp1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var userRepository = ServiceLocator.Instance.Resolve<IUserRepository>();
            
<<<<<<< HEAD
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
=======
            var users = userRepository.GetAll().ToList();
            
            Console.WriteLine(users.Last().Username);
            
>>>>>>> a63993ebd0d3d69079347393ae9b54975c2e3665
            Console.ReadLine();
        }
    }
}
