using System;
using ConsoleApp1.Contracts.Services;
using ConsoleApp1.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using PhotoGallery.Infrastructure;
using PhotoGallery.Infrastructure.Repositories;

namespace ConsoleApp1
{
    public class ServiceLocator
    {
        private static ServiceLocator _instance;
        public static ServiceLocator Instance => _instance ?? (_instance = new ServiceLocator());

        private IServiceCollection _services;
        private IServiceProvider _provider;

        public ServiceLocator()
        {
            _services = new ServiceCollection();
            Load();
        }

        public void Load()
        {
            _services.AddDbContext<PhotoGalleryContext>(options =>
                options.UseSqlServer(AppSettings.Instance.ConnectionString));

            //Repositories
            _services.AddScoped<IUserRepository, UserRepository>();
            _services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            _services.AddScoped<IRoleRepository, RoleRepository>();
            _services.AddScoped<IMessageRepository, MessageRepository>();
            _services.AddScoped<IRoleRepository, RoleRepository>();

            //Services
            _services.AddScoped<ISerializer, Serializer>();
            _services.AddScoped<IStorageSystem, StorageSystem>();

            _provider = _services.BuildServiceProvider();
        }

        public T Resolve<T>()
        {
            return _provider.GetService<T>();
        }
    }
}