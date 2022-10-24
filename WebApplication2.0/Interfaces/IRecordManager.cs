using WebApplication2._0.Models;

namespace WebApplication2._0.Interfaces
{
    public interface IRecordManager
    {
        void Create(RecordModel model);
        Task<List<RecordModel>> Read(int folder_id);
        void Update(RecordModel model);
        void Delete(RecordModel model);
    }
}
