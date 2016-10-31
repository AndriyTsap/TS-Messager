using System;
using System.IO;
using ConsoleApp1.Contracts.Services;

namespace ConsoleApp1.Services
{
    public class FileManager : IFileManager
    {
        public string ReadFromFile(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException();
            }
            if (new FileInfo(path).Length == 0)
            {
                throw new Exception("File is empty!");
            }
            return File.ReadAllText(path);
        }

        public void WriteToFile(string str, string path)
        {
            using (var stream = File.CreateText(path))
            {
                stream.WriteLine(str);
            }
        }
    }
}
