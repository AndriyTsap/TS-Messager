using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PhotoGallery.Infrastructure;
using PhotoGallery.Infrastructure.Repositories;
using PhotoGallery.Infrastructure.Repositories.Abstract;
using PhotoGallery.Infrastructure.Services;
using PhotoGallery.Infrastructure.Services.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace APITests
{
    public class ServiceLocator
    {
        private IConfigurationRoot _configuration;

        private static ServiceLocator _instance;
        public static ServiceLocator Instance => _instance ?? (_instance = new ServiceLocator());

        private IServiceCollection _services;
        private IServiceProvider _provider;

        public ServiceLocator()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            Console.WriteLine(currentDirectory);
            var builder = new ConfigurationBuilder()
                .SetBasePath(currentDirectory)
                .AddJsonFile(@"appsettings.json");

            builder.AddEnvironmentVariables();
            _configuration = builder.Build();
            _services = new ServiceCollection();
            Load();
        }

        public void Load()
        {
            _services.AddDbContext<PhotoGalleryContext>(options =>
                    options.UseSqlServer(_configuration["Data:DBConnection:ConnectionString"]));

            //Repositories
            _services.AddScoped<IUserRepository, UserRepository>();
            _services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            _services.AddScoped<IRoleRepository, RoleRepository>();
            _services.AddScoped<IMessageRepository, MessageRepository>();
            _services.AddScoped<IRoleRepository, RoleRepository>();
            _services.AddScoped<ILoggingRepository, LoggingRepository>();


            //Services
            _services.AddScoped<IEncryptionService, EncryptionService>();
            _services.AddScoped<IEncryptionService, EncryptionService>();

            _provider = _services.BuildServiceProvider();
        }

        public T Resolve<T>()
        {
            return _provider.GetService<T>();
        }
    }
}

