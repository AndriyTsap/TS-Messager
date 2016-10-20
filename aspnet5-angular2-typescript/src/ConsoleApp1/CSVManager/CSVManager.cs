using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using PhotoGallery.Entities;

namespace ConsoleApp1.CSVManager
{
    public class CSVManager
    {
        public List<User> ImportUsers(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException();
            }
            if (new FileInfo(path).Length == 0)
            {
                throw new Exception("Файл пустий");
            }
            string[] allLines = File.ReadAllLines(path);

            var list = from line in allLines
                       let data = line.Split(';')
                       select new User()
                       {
                           Id = Int32.Parse(data[0]),
                           Username = data[1],
                           Email = data[2],
                           Phone = data[3],
                           BirthDate = data[4],
                           HashedPassword = data[5],
                           Salt = data[6],
                           IsLocked = (data[7] == "true") ? true : false,
                           DateCreated = DateTime.Parse(data[8])
                       };
            return list.ToList();
        }

        public void ExportUsersToCSV(List<User> listUsers, string path)
        {
            using (var stream = File.CreateText(path))
            {
                foreach (var user in listUsers)
                {
                    stream.WriteLine($"{user.Id};{user.Username};{user.Email};{user.Phone};{user.BirthDate};{user.HashedPassword};{user.Salt};{user.IsLocked};{user.DateCreated}");
                }
            }
        }
    }
}
