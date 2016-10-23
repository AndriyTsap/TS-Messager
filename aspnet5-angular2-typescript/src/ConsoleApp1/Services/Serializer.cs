using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using ConsoleApp1.Contracts.Services;

namespace ConsoleApp1.Services
{
    public class Serializer: ISerializer
    {
        public string Serialize<T>(T objectForParsing) where T : class
        {
            if (objectForParsing == null)
            {
                return null;
            }

            var serializer = new XmlSerializer(typeof(T));

            var settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true
            };

            using (var textWriter = new StringWriter())
            using (var xmlWriter = XmlWriter.Create(textWriter, settings))
            {
                serializer.Serialize(xmlWriter, objectForParsing);
                return textWriter.ToString();
            }
        }

        public T Deserialize<T>(string filePath) where T : class
        {
            var serializer = new XmlSerializer(typeof(T));
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                throw new ArgumentException(string.Format("File '{0}' doesn't exist.", filePath), "filePath");
            }

            using (var fileStream = File.OpenRead(filePath))
            {
                return serializer.Deserialize(fileStream) as T;
            }
        }
    }
}