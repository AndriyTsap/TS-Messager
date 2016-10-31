using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsoleApp1.Services;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace AdminkaTests2
{
    [TestFixture]
    public class CsvManagerTests
    {   
        [Test]
        public void Check_IfExportedUsers_Equal_Importd_UsingOneFile()
        {
            var data = new MockData();
            var mockUsers = data.Users;
            var pathToCSVFile = "..//ConsoleApp1//Data//usersForImporting.csv";

            CsvManager csvManager=new CsvManager();
            csvManager.ExportUsersToCSV(mockUsers, pathToCSVFile);

            var importedUsersList=csvManager.ImportUsers(pathToCSVFile);
            
            Assert.That(mockUsers.SequenceEqual(importedUsersList));
        }
    }   
}
