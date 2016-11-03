using System;
using System.Linq;

using System.Threading.Tasks;
using ConsoleApp1.Contracts.Enities;
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
            
            var userRepository = ServiceLocator.Instance.Resolve<IUserRepository>();

            var loger = ServiceLocator.Instance.Resolve<ILogger>();

            CommandExecuter commandExecuter = new CommandExecuter();
            while (true)
            {
                try
                {
                    commandExecuter.Execute(Console.ReadLine());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + "");
                    loger.Log(new LogEntry(LoggingEventType.Error, ex.Message + "",ex));//add stack trace
                }
                
            }

        }
    }
}
