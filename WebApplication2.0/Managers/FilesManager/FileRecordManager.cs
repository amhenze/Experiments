using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebApplication2._0.Extensions;
using WebApplication2._0.Interfaces;
using WebApplication2._0.Models;
using WebApplication2._0.Options;

namespace WebApplication2._0.Managers.FilesWorker
{
    public class FileRecordManager : IRecordManager
    {
        private readonly RootFolderOptions _options;
        private readonly ILogger<FileRecordManager> _logger;
        private readonly IRandomize _randomize;
        //private IValidator<FileRecordManager> _folderValidator;
        private string fileName;

        public FileRecordManager(IOptions<RootFolderOptions> options,
            ILogger<FileRecordManager> logger,
            IRandomize randomize
            //IValidator<FileRecordManager> folderValidator
            )
        {
            _options = options.Value;
            _logger = logger;
            _randomize = randomize;
            //_folderValidator = folderValidator;
            fileName = "newgen" + _randomize.GetRandomInt(1, 100, 999) + ".txt";
        }

        public void Create(RecordModel model)
        {
            CollectionModel collection = new CollectionModel();
            var allFiles = Directory.GetFiles(_options.Path);
            collection.CollectionName = allFiles[model.CollectionId - 1];
            using StreamWriter file = new(Path.Combine(collection.CollectionName, fileName));
            file.WriteLine(Convert.ToString(model.Number) +";"+ model.Letter);
            _logger.LogInformation("File create");
        }

        public async Task<List<RecordModel>> Read(int collectionId)
        {
            List<RecordModel> records = new List<RecordModel>();
            int fileCount = 1;
            var allDirectories = Directory.GetDirectories(_options.Path);
            CollectionModel collection = new CollectionModel();
            collection.CollectionName = allDirectories[collectionId - 1];
            collection.CollectionName = Path.Combine(_options.Path, collection.CollectionName);
            try
            {
                if (File.Exists(collection.CollectionName))
                {
                    string[] file = await File.ReadAllLinesAsync(collection.CollectionName);
                    foreach (var line in file)
                    {
                        var tokens = line.Split(';');
                        var linestruct = new RecordModel { Number = Int32.Parse(tokens[0]), Letter = tokens[1] ,CollectionId = collectionId ,RecordId = fileCount++};
                        if (linestruct.Remain())
                        {
                            records.Add(linestruct);
                        }
                    }
                    records = records
                        .OrderBy(l => l.Number)
                        .ThenByDescending(l => l.Letter)
                        .ToList();
                    return records;
                }
                else
                {
                    throw new Exception("dir not exist");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "папка не существует");
                return null;
            }
        }
        public void Update(RecordModel model)
        {

        }
        public void Delete(RecordModel model)
        {
            var allDirectories = Directory.GetDirectories(_options.Path);
            Directory.Delete(allDirectories[model.CollectionId - 1], true);
        }
    }
}
