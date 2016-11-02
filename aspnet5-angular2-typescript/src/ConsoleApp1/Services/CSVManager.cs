using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ConsoleApp1.Contracts.Enities;
using ConsoleApp1.Contracts.Services;
using ConsoleApp1.Services;
using PhotoGallery.Entities;
using PhotoGallery.Infrastructure.Repositories;
using ServiceStack.Text;

namespace ConsoleApp1.Services
{
    public class CsvManager : ICsvManager
    {
        private FileManager _fileManager;
        private Logger _loger = new Logger(ServiceLocator.Instance.Resolve<ILoggingRepository>());
        public CsvManager()
        {
            _fileManager = new FileManager();
        }
        public List<User> ImportUsers(string path)
        {
            string strUsers=_fileManager.ReadFromFile(path);
            List<User> listUsers = CsvSerializer.DeserializeFromString<List<User>>(strUsers);
            _loger.Log(new LogEntry(LoggingEventType.Information, "Users Imported from " + path));
            return listUsers;
        }

        public void ExportUsersToCSV(List<User> listUsers, string path)
        {
            var csv = CsvSerializer.SerializeToCsv(listUsers);           
            _fileManager.WriteToFile(csv,path);
            _loger.Log(new LogEntry(LoggingEventType.Information, "Users Exported to "+path));
        }
    }
}
