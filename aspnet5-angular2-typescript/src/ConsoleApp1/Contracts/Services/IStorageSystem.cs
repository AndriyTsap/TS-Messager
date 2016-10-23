using System.Collections.Generic;
using PhotoGallery.Entities;

namespace ConsoleApp1.Contracts.Services
{
    public interface IStorageSystem
    {
        IEnumerable<IEntityBase> GetAll();
        IEntityBase Get(int id);
        void Add(IEntityBase entity);
        void Edit(IEntityBase entity);
        void DeleteById(int id);
    }
}