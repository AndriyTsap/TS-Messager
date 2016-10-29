using System;
using ConsoleApp1.Contracts.Enities;
using ConsoleApp1.Contracts.Services;
using Microsoft.EntityFrameworkCore;
using PhotoGallery.Entities;
using PhotoGallery.Infrastructure;
using PhotoGallery.Infrastructure.Repositories;

namespace ConsoleApp1.Services
{
    public class Logger: ILogger
    {
        private ILoggingRepository _loggingRepository;

        public Logger(ILoggingRepository loggingRepository)
        {
            _loggingRepository = loggingRepository;
        }

        public void Log(LogEntry entry)
        {
            if (entry.Exception != null)
                _loggingRepository.Add(new Error {Severity = entry.Severity.ToString(), DateCreated = DateTime.Now, Message = entry.Message, StackTrace = entry.Exception.StackTrace});
            else
                _loggingRepository.Add(new Error { Severity = entry.Severity.ToString(), DateCreated = DateTime.Now, Message = entry.Message, StackTrace = "" });
            _loggingRepository.Commit();
        }
    }
}