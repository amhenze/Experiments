using FluentValidation;
using Microsoft.Extensions.Options;
using NewTextreader;
using WebApplication2._0.Controllers;
using WebApplication2._0.DataBaseWorker;
using WebApplication2._0.Extensions;
using WebApplication2._0.Interfaces;
using WebApplication2._0.Managers;
using WebApplication2._0.Models;
using WebApplication2._0.Options;
using WebApplication2._0.Validators;

public class Startup
{
    public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
    {
        Configuration = configuration;
        WebHostEnvironment = webHostEnvironment;
    }
    protected IWebHostEnvironment WebHostEnvironment { get; }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddFilelogic();
        string currentEnvironment = Environment.GetEnvironmentVariable("WorkerType");
        if (currentEnvironment == "db")
        {
            services.AddDBServices();
        }
        else
        {
            services.AddFileServices();
        }
        //services.AddValidation();
        services.AddLogging();
        services.AddMvc();
        services.AddAutoMapper(typeof(Startup));
        services.Configure<RootFolderOptions>(Configuration.GetSection(nameof(RootFolderOptions)));
        services.Configure<DBOptions>(Configuration.GetSection(nameof(DBOptions)));
    }
    
    public void Configure(IApplicationBuilder app)
    {
        app.UseDeveloperExceptionPage();
        app.UseStatusCodePages();



        app.UseSwagger();
        app.UseSwaggerUI();


        app.UseHttpsRedirection();

        app.UseAuthorization();
        app.UseRouting();
        app.UseEndpoints(config => config.MapControllers());

    }
}

