using System.Collections.Generic;
using System.IO;
using System.Linq;
using ConsoleApp1.Contracts.Services;
using PhotoGallery.Entities;

namespace ConsoleApp1.Services
{
    public class StorageSystem<T>: IStorageSystem<T> where T: IEntityBase
    {
        private readonly ISerializer _serializer;
        private string _filePath;

        public StorageSystem(ISerializer serializer)
        {
            _serializer = serializer;
        }

        public void SetFilePath(string filePath)
        {
            _filePath = filePath;
            Initialize();
        }

        public void Add(T entity)
        {
            var entities = GetAll().ToList();
            entity.Id = entities.Select(t => t.Id).Max() + 1;
            entities.Add(entity);
            SaveChanges(entities);
        }

        public void Edit(T entity)
        {
            var entities = GetAll().ToList();
            var removeTarget = entities.FirstOrDefault(t => t.Id == entity.Id);
            if (removeTarget == null)
            {
                return;
            }
            entities.Remove(removeTarget);
            entities.Add(entity);
            SaveChanges(entities);
        }

        public void DeleteById(int id)
        {
            var entities = GetAll().ToList();
            var removeTarget = entities.FirstOrDefault(t => t.Id == id);
            if (removeTarget == null)
            {
                return;
            }
            entities.Remove(removeTarget);
            SaveChanges(entities);
        }

        public T Get(int id)
        {
            var tasks = GetAll();
            var task = tasks.FirstOrDefault(t => t.Id == id);
            return task;
        }

        public IEnumerable<T> GetAll()
        {
            var entities = _serializer.Deserialize<List<T>>(_filePath);
            return entities;
        }

        private void Initialize()
        {
            if (File.Exists(_filePath))
            {
                return;
            }
            var entities = new List<T>();
            
            SaveChanges(entities);
        }

        private void SaveChanges(IEnumerable<T> entities)
        {
            var serialized = _serializer.Serialize(entities.ToList());
            File.WriteAllText(_filePath, serialized);
        }
    }
}