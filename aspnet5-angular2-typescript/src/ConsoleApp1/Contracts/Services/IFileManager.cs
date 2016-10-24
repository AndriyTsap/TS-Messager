using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApp1.Contracts.Services
{
    interface IFileManager
    {
        void WriteToFile(string str, string path);

        string ReadFromFile(string path);
    }
}
