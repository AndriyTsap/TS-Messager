using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PhotoGallery.Entities;

namespace ConsoleApp1.Contracts.Services
{
    interface ICsvManager
    {
        List<User> ImportUsers(string path);

        void ExportUsersToCSV(List<User> listUsers, string path);

    }
}
