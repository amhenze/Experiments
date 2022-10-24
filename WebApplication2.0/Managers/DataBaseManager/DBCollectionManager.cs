using AutoMapper;
using Microsoft.Extensions.Options;
using Npgsql;
using WebApplication2._0.Entities;
using WebApplication2._0.Interfaces;
using WebApplication2._0.Models;
using WebApplication2._0.Options;

namespace WebApplication2._0.DataBaseWorker
{
    public class DBCollectionManager : ICollectionManager
    {
        private readonly ILogger<DBCollectionManager> _logger;
        private readonly IDBExecuter _dbExecuter;
        private readonly IMapper _mapper;
        public DBCollectionManager(ILogger<DBCollectionManager> logger,
            IDBExecuter dbExecuter,
            IMapper mapper)
        {
            _logger = logger;
            _dbExecuter = dbExecuter;
            _mapper = mapper;
        }
        public void Create(CollectionModel model)
        {
            var collectionEntity = _mapper.Map<CollectionEntity>(model);
            var sql = $@"INSERT INTO myschema.collections(collection_id, collection_name) 
                VALUES(default,@param1)";
            object[] parameters = { collectionEntity.CollectionName };
            _dbExecuter.ExecuteNonQuery(sql, parameters);
        }
        public async Task<List<CollectionModel>> Read(int id = default)
        {
            var sql = $@"SELECT * FROM myschema.collections
                ORDER BY folder_id ASC ";
            var collections = await _dbExecuter.ExecuteReader<CollectionEntity>(sql, null);
            return collections.Select(c => _mapper.Map<CollectionModel>(c)).ToList();
        }
        public void Update(CollectionModel model)
        {
            var collectionEntity = _mapper.Map<CollectionEntity>(model);
            var sql = $@"Update myschema.collections
                SET collection_name = (@param2)
                Where collection_id = (@param1)";
            object[] parameters = { collectionEntity.CollectionId, collectionEntity.CollectionName};
            _dbExecuter.ExecuteNonQuery(sql, parameters);
        }
        public void Delete(int id)
        {
            var sql = $@"Delete from myschema.records where (collection_id) VALUES(@param1)
                    Delete from myschema.collections where (collection_id) VALUES(@param1)";
            object[] parameters = { id };
            _dbExecuter.ExecuteNonQuery(sql, parameters);
        }
    }
}
