using FluentValidation;
using WebApplication2._0.Models;
using WebApplication2._0.Options;
using WebApplication2._0.Validators;

namespace WebApplication2._0.Extensions
{
    public static class ValidatorsExtensions
    {
        public static IServiceCollection AddValidation(this IServiceCollection services)
        {
            services.AddScoped<IValidator<RootFolderOptions>, RootValidator>();
            return services;
        }
    }
}
