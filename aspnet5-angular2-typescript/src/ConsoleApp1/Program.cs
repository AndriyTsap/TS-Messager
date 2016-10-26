using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsoleApp1.Contracts.Services;
using Microsoft.EntityFrameworkCore;
using PhotoGallery.Entities;
using PhotoGallery.Infrastructure;
using PhotoGallery.Infrastructure.Repositories;
using ServiceStack.Text;
using ConsoleApp1.CSVManager;

namespace ConsoleApp1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var userRepository = ServiceLocator.Instance.Resolve<IUserRepository>();
            
            var users = userRepository.GetAll().ToList();
            
            Console.WriteLine(users.Last().Username);
            
            Console.ReadLine();
        }
    }
}
