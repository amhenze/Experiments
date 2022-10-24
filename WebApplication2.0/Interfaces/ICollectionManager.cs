using WebApplication2._0.Models;

namespace WebApplication2._0.Interfaces
{
    public interface ICollectionManager
    {
        void Create(CollectionModel model);
        Task<List<CollectionModel>> Read(int id = default);
        void Update(CollectionModel model);
        void Delete(int id);
    }
}
