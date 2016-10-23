namespace ConsoleApp1.Contracts.Services
{
    public interface ISerializer
    {
        /// <summary>
        /// Serialize object of specific type and return serialized as string.
        /// </summary>
        string Serialize<T>(T objectForParsing) where T : class;

        /// <summary>
        /// Read data from file and return desirialized object.
        /// </summary>
        T Deserialize<T>(string filePath) where T : class;
    }
}