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
using PhotoGallery.Infrastructure.Repositories.Abstract;
using ServiceStack.Text;

namespace ConsoleApp1.Services
{
    public class CsvManager : ICsvManager
    {
        private readonly FileManager _fileManager;
        private readonly Logger _loger = new Logger(ServiceLocator.Instance.Resolve<ILoggingRepository>());

        public CsvManager()
        {
            _fileManager = new FileManager();
        }
        public IEnumerable<User> ImportUsers(string path)
        {
            string strUsers=_fileManager.ReadFromFile(path);
            IEnumerable<User> listUsers = CsvSerializer.DeserializeFromString<IEnumerable<User>>(strUsers);
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
