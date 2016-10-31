using System.Collections.Generic;
using PhotoGallery.Entities;

namespace ConsoleApp1.Contracts.Services
{
    public interface IStorageSystem<T> where T: IEntityBase
    {
        void SetFilePath(string filePath);
        IEnumerable<T> GetAll();
        T Get(int id);
        void Add(T entity);
        void Edit(T entity);
        void DeleteById(int id);
    }
}