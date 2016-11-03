using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ConsoleApp1;
using ConsoleApp1.Services;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace AdminkaTests2
{
    [TestFixture]
    public class FileManagerTests
    {
        private FileManager _fileManager = new FileManager();

        [Test]
        public void IfFileForReadingNotExist_ShouldThrowFileNotFoundException()
        {
            var pathToFile = "..//ConsoleApp1//Data//notExistFile.csv";

            Assert.Throws<FileNotFoundException>(()=>_fileManager.ReadFromFile(pathToFile));
        }

        [Test]
        public void IfFileForReadingIsEmpty_ShouldThrowException()
        {

            var pathToFile = "..//ConsoleApp1//Data//EmptyFile.csv";
           
            File.Create(pathToFile);

            Assert.Throws<Exception>(() => _fileManager.ReadFromFile(pathToFile));
        }

        [Test]
        public void Check_DataForWritingAnd_DataForReading_TheSameFile_ShouldBeEqual()
        {
            var pathToFile = "..//ConsoleApp1//Data//FileForTesting.csv";
            string dataForWriting = "1,Igor,29/10/1995\r\n";

            _fileManager.WriteToFile(dataForWriting, pathToFile);
            string dataFromFile = _fileManager.ReadFromFile(pathToFile);

            Assert.AreEqual(dataForWriting+"\r\n",dataFromFile);//add \r\n, because FileWriter add them by default
        }

    }
}
