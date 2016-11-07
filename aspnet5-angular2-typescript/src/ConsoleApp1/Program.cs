using System;
using ConsoleApp1.Contracts.Enities;
using ConsoleApp1.Contracts.Services;
using ConsoleApp1.Services;
using PhotoGallery.Infrastructure.Repositories.Abstract;

namespace ConsoleApp1
{
    public class Program
    {
        public static void Main(string[] args)
        { 
            
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
