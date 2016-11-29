﻿using System;
using ConsoleApp1.Contracts.Services;
using ConsoleApp1.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using PhotoGallery.Entities;

using PhotoGallery.Infrastructure;
using PhotoGallery.Infrastructure.Repositories;
using PhotoGallery.Infrastructure.Repositories.Abstract;
using PhotoGallery.Infrastructure.Services;
using PhotoGallery.Infrastructure.Services.Abstract;

namespace ConsoleApp1
{
    public class ServiceLocator
    {
        private static ServiceLocator _instance;
        public static ServiceLocator Instance => _instance ?? (_instance = new ServiceLocator());

        private IServiceCollection _services;
        private IServiceProvider _provider;

        private ServiceLocator()
        {
            _services = new ServiceCollection();
            Load();
        }

        private void Load()
        {
            _services.AddDbContext<PhotoGalleryContext>(options =>
                options.UseSqlServer(AppSettings.Instance.ConnectionString));

            //Repositories
            _services.AddScoped<IUserRepository, UserRepository>();
            _services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            _services.AddScoped<IRoleRepository, RoleRepository>();
            _services.AddScoped<IMessageRepository, MessageRepository>();
            _services.AddScoped<IRoleRepository, RoleRepository>();
            _services.AddScoped<ILoggingRepository, LoggingRepository>();
           


            //Services
            _services.AddScoped<ISerializer, Serializer>();
            _services.AddTransient(typeof(IStorageSystem<>), typeof(StorageSystem<>));
            _services.AddScoped<ILogger, Logger>();
            _services.AddScoped<IEncryptionService, EncryptionService>();
            _services.AddScoped<IEncryptionService, EncryptionService>();
            _services.AddScoped<IAccountService, AccountService>();

            _provider = _services.BuildServiceProvider();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Resolve<T>()
        {
            return _provider.GetService<T>();
        }
    }
}