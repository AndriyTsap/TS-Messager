using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PhotoGallery.Entities;

namespace ConsoleApp1.Contracts.Services
{
    /// <summary>
    /// Manager for working with csv files
    /// </summary>
    interface ICsvManager
    {   
        /// <summary>
        /// Import users from csv file
        /// </summary>
        /// <param name="path">Path to csv file</param>
        /// <returns>List of users</returns>
        IEnumerable<User> ImportUsers(string path);

        /// <summary>
        /// Export users to csv file
        /// </summary>
        /// <param name="listUsers">List of object which will serialize and write to csv file</param>
        /// <param name="path"></param>
        void ExportUsersToCSV(List<User> listUsers, string path);
    }
}
