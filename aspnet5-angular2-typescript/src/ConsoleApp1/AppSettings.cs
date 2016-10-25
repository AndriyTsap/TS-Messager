using System.IO;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.Extensions.Configuration;

namespace ConsoleApp1
{
    public class AppSettings
    {
        private IConfigurationRoot Configuration { get; set; }

        private AppSettings()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(@"appsettings.json");

            string path = Directory.GetCurrentDirectory();
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
            get { return "Data:XmlFiles:UserFilePath"; }
        }

        public string MessagesFilePath
        {
            get { return Configuration["Data:XmlFiles:MessageFilePath"]; }
        }

        public string GroupsFilePath
        {
            get { return Configuration["Data:XmlFiles:GroupFilePath"]; }
        }

        public string UserGroupFilePath
        {
            get { return Configuration["Data:XmlFiles:UserGroupFilePath"]; }
        }

        public string ConnectionString
        {
            get { return Configuration["Data:DBConnection:ConnectionString"]; }
        }
    }
}