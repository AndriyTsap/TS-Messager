using ConsoleApp1.Contracts.Enities;

namespace ConsoleApp1.Contracts.Services
{
    public interface ILogger
    {
        void Log(LogEntry entry);
    }
}