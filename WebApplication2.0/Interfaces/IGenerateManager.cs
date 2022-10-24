using WebApplication2._0.Models;

namespace WebApplication2._0.Interfaces
{
    public interface IGenerateManager
    {
        void GenerateRecords(CollectionModel model);
        void GenerateCollection(CollectionModel model);
    }
}
