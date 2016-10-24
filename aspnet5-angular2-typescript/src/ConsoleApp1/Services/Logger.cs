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

        public void Log(LogEntry entry)
        {

            var optionsBuilder = new DbContextOptionsBuilder<PhotoGalleryContext>();
            optionsBuilder.UseSqlServer(AppSettings.Instance.ConnectionString);
            var context = new PhotoGalleryContext(optionsBuilder.Options);
            var logginingRepository = new LoggingRepository(context);
            
            logginingRepository.Add(new Error() {Severity = entry.Severity.ToString(), DateCreated = DateTime.Now, Message = entry.Message, StackTrace = entry.Exception.StackTrace});
            logginingRepository.Commit();

        }
    }
}