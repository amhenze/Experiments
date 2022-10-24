using AutoMapper;
using Microsoft.Extensions.Options;
using Npgsql;
using WebApplication2._0.Entities;
using WebApplication2._0.Interfaces;
using WebApplication2._0.Models;
using WebApplication2._0.Options;

namespace WebApplication2._0.DataBaseWorker
{
    public class DBRecordManager : IRecordManager
    {
        private readonly IDBExecuter _dbExecuter;
        private readonly IMapper _mapper;

        public DBRecordManager(IDBExecuter dbExecuter,IMapper mapper)
        {
            _dbExecuter = dbExecuter;
            _mapper = mapper;
        }

        //public void GenerateData(CollectionModel model)
        //{
        //    RecordModel dBDataModel = new RecordModel();
        //    int rowsCount = _randomize.GetRandomInt(1, 100, 1000);
        //    for (int i = 0; i < rowsCount; i++)
        //    {
        //        dBDataModel.FolderId = model.CollectionId;
        //        dBDataModel.Number = _randomize.GetRandomInt(1, 0, 100000);
        //        dBDataModel.Letter = _randomize.GetRandomString();
        //        AddDataRow(dBDataModel);
        //    }
        //}
        public void Create(RecordModel model)
        {
            var recordEntity = _mapper.Map<RecordEntity>(model);
            var sql = $@"INSERT INTO myschema.records(collection_id, number, letter) 
                VALUES(@param1, @param2, @param3)";
            object[] parameters = { recordEntity.CollectionId, recordEntity.Number, recordEntity.Letter };
            _dbExecuter.ExecuteNonQuery(sql, parameters);
        }

        public async Task<List<RecordModel>> Read(int folder_id)
        {
            var sql = $@"SELECT * FROM myschema.records 
                where collection_id = @param1
                ORDER BY numbers ASC, letters; ";
            object[] parameters = { folder_id };
            var record = await _dbExecuter.ExecuteReader<RecordEntity>(sql, parameters);
            return record.Select(c => _mapper.Map<RecordModel>(c)).ToList();
        }
        public void Update(RecordModel model)
        {
            var recordEntity = _mapper.Map<RecordEntity>(model);
            var sql = $@"Update myschema.records 
                SET number = (@param2), letter = (@param3)
                Where collection_id = (@param1)";
            object[] parameters = { recordEntity.CollectionId, recordEntity.Number , recordEntity.Letter};
            _dbExecuter.ExecuteNonQuery(sql, parameters);
        }
        public void Delete(RecordModel model)
        {
            var recordEntity = _mapper.Map<RecordEntity>(model);
            var sql = $@"Delete From myschema.records 
                Where record_id = (@param1)";
            object[] parameters = { recordEntity.RecordId};
            _dbExecuter.ExecuteNonQuery(sql, parameters);
        }
    }
}
