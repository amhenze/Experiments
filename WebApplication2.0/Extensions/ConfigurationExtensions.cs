namespace WebApplication2._0.Extensions
{
    public static class ConfigurationExtensions
    {
        public static IConfigurationBuilder ConfigureAppConfiguration(this IConfigurationBuilder config,
           string contentRootPath, string environmentName)
        {
            config
                .SetBasePath(contentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: false);

            return config.AddEnvironmentVariables();
        }
    }
}
