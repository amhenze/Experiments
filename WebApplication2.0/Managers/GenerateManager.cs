using WebApplication2._0.Entities;
using WebApplication2._0.Interfaces;
using WebApplication2._0.Models;

namespace WebApplication2._0.Managers
{
    public class GenerateManager : IGenerateManager
    {
        private readonly ICollectionManager _collectionManager;
        private readonly IRecordManager _recordManager;
        private readonly ILogger<GenerateManager> _logger;
        private readonly IRandomize _randomize;

        public GenerateManager(ICollectionManager collectionManager, 
            IRecordManager recordManager,
            ILogger<GenerateManager> logger,
            IRandomize randomize)
        {
            _collectionManager = collectionManager;
            _recordManager = recordManager;
            _logger = logger;
            _randomize = randomize;
        }

        public void GenerateRecords(CollectionModel model)
        {
            int rowsCount = _randomize.GetRandomInt(1, 100, 1000);
            for (int i = 0; i < rowsCount; i++)
            {
                RecordModel recordModel = new RecordModel();
                recordModel.CollectionId = model.CollectionId;
                recordModel.Number = _randomize.GetRandomInt(1, 0, 100000);
                recordModel.Letter = _randomize.GetRandomString();
                _recordManager.Create(recordModel);
            }
            _logger.LogInformation($"Generate {rowsCount} records");
        }
        public void GenerateCollection(CollectionModel model)
        {
            _collectionManager.Create(model);
            GenerateRecords(model);
        }
    }
}
