using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.Extensions.Configuration;

namespace ConsoleApp1
{
    public class AppSettings
    {
        private IConfigurationRoot Configuration { get; set; }
        private readonly string _currentDirectory;

        private AppSettings()
        {
            //_currentDirectory = Path.GetFullPath(Path.GetDirectoryName(Directory.GetCurrentDirectory()))+@"\ConsoleApp1";
            _currentDirectory = Directory.GetCurrentDirectory();
            Console.WriteLine(_currentDirectory);
            var builder = new ConfigurationBuilder()
                .SetBasePath(_currentDirectory)
                .AddJsonFile(@"appsettings.json");

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        private static AppSettings _instance;

        public static AppSettings Instance
        {
            get
            {
                if(_instance == null)
                    _instance = new AppSettings();
                return _instance;
            }
        }

        public string UsersFilePath
        {
            get { return _currentDirectory+Configuration["Data:XmlFiles:UserFilePath"]; }
        }

        public string MessagesFilePath
        {
            get { return _currentDirectory + Configuration["Data:XmlFiles:MessageFilePath"]; }
        }

        public string GroupsFilePath
        {
            get { return _currentDirectory + Configuration["Data:XmlFiles:GroupFilePath"]; }
        }

        public string UserGroupFilePath
        {
            get { return _currentDirectory + Configuration["Data:XmlFiles:UserGroupFilePath"]; }
        }

        public string ConnectionString
        {
            get { return Configuration["Data:DBConnection:ConnectionString"]; }
        }
    }
}