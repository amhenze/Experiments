using Serilog;
using WebApplication2._0.Extensions;

namespace WebApplication2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    config.ConfigureAppConfiguration(hostContext.HostingEnvironment.ContentRootPath, hostContext.HostingEnvironment.EnvironmentName);
                })
                .ConfigureLogging(logging => logging.ClearProviders())
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseUrls("http://*:5266", "http://*:7266");
                webBuilder.ConfigureKestrel(options => options.AddServerHeader = false);
                webBuilder.UseStartup<Startup>();
            })
            .UseSerilog((context, services, configuration) =>
            {
                configuration
                    .ReadFrom.Configuration(context.Configuration)
                    .ReadFrom.Services(services)
                    .Enrich.FromLogContext();
            });
    }
}

