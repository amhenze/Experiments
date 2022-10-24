using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebApplication2._0.Entities;
using WebApplication2._0.Interfaces;
using WebApplication2._0.Models;
using WebApplication2._0.Options;

namespace WebApplication2._0.Managers.FilesWorker
{
    public class FileCollectionManager : ICollectionManager
    {
        private readonly RootFolderOptions _options;
        private readonly ILogger<FileCollectionManager> _logger;


        public FileCollectionManager(IOptions<RootFolderOptions> options,
            ILogger<FileCollectionManager> logger)
        {
            _options = options.Value;
            _logger = logger;
        }

        public void Create(CollectionModel model)
        {
            string fileName = "file" + model.CollectionId;
            string filePath = Path.Combine(_options.Path, fileName);
            File.Create(filePath);
        }
        public async Task<List<CollectionModel>> Read(int id = default)
        {
            var allFiles = Directory.GetFiles(_options.Path);
            List<CollectionModel>? dirList = new List<CollectionModel>();
            if (id == default)
            {

                int i = 1;
                foreach (var file in allFiles)
                {
                    CollectionModel model = new CollectionModel();
                    model.CollectionId = i++;
                    model.CollectionName = file;
                    dirList.Add(model);
                }
            }
            else
            {
                CollectionModel model = new CollectionModel();
                model.CollectionId = id;
                model.CollectionName = allFiles[id];
                dirList.Add(model);
            }
            return dirList;
        }
        public void Update(CollectionModel model)
        {

        }
        public void Delete(int id)
        {
            var allFiles = Directory.GetFiles(_options.Path);
            File.Delete(allFiles[id - 1]);
        }
    }
}


