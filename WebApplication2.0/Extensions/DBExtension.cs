using Microsoft.Extensions.Options;
using Npgsql;
using WebApplication2._0.DataBaseWorker;
using WebApplication2._0.Interfaces;
using WebApplication2._0.Managers;
using WebApplication2._0.Options;

namespace WebApplication2._0.Extensions
{
    public static class DBExtension
    {
        public static IServiceCollection AddDBServices(this IServiceCollection services)
        {
            services.AddTransient<IRecordManager, DBRecordManager>();
            services.AddTransient<ICollectionManager, DBCollectionManager>();
            services.AddTransient<IDBExecuter, DBExecuter>();
            services.AddTransient<IGenerateManager, GenerateManager>();
            services.AddTransient<NpgsqlConnection>(serviceProvider => {
                IOptions<DBOptions> options = serviceProvider.GetRequiredService<IOptions<DBOptions>>();
                var databaseOptions = options.Value;
                var connection = new NpgsqlConnection(databaseOptions.ConnectSettings);
                return connection;
            });
            return services;
        }
    }
}
