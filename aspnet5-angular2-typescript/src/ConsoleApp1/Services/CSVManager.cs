using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ConsoleApp1.Services;
using PhotoGallery.Entities;
using ServiceStack.Text;

namespace ConsoleApp1.Services
{
    public class CSVManager
    {
        private FileManager _fileManager;
        public CSVManager(FileManager fileManager)
        {
            _fileManager = fileManager;
        }
        public List<User> ImportUsers(string path)
        {
            string strUsers=_fileManager.ReadFromFile(path);
            List<User> listUsers = CsvSerializer.DeserializeFromString<List<User>>(strUsers);
            return listUsers;
        }

        public void ExportUsersToCSV(List<User> listUsers, string path)
        {
            var csv = CsvSerializer.SerializeToCsv(listUsers);           
            FileManager fileManager=new FileManager();
            fileManager.WriteToFile(csv,path);
        }
    }
}
