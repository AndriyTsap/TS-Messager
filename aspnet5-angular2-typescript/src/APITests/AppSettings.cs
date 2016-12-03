using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace APITests
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
                if (_instance == null)
                    _instance = new AppSettings();
                return _instance;
            }
        }

        public string ConnectionString
        {
            get { return Configuration["Data:DBConnection:ConnectionString"]; }
        }
    }
}
