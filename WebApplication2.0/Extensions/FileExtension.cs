using WebApplication2._0.DataBaseWorker;
using WebApplication2._0.Interfaces;
using WebApplication2._0.Managers;
using WebApplication2._0.Managers.FilesWorker;

namespace WebApplication2._0.Extensions
{
    public static class FileExtension
    {
        public static IServiceCollection AddFileServices(this IServiceCollection services)
        {
            services.AddTransient<IRecordManager, FileRecordManager>();
            services.AddTransient<ICollectionManager, FileCollectionManager>();
            services.AddTransient<IGenerateManager, GenerateManager>();
            return services;
        }
    }
}