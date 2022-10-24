using NewTextreader;
using WebApplication2._0.Interfaces;

namespace WebApplication2._0.Extensions
{
    public static class FileLogicExtensions
    {
        public static IServiceCollection AddFilelogic(this IServiceCollection services)
        {
            services.AddTransient<IRandomize, Randomize>();
            return services;
        }
    }
}
