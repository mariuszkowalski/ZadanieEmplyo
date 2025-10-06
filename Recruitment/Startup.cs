using DataAccess.DataTransfer;
using DataAccess.DataTransfer.Interfaces;
using DataAccess.DataTransfer.Reports;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Services.Core.DataOperation;
using Services.Core.DataOperation.Interfaces;
using Services.Core.DataOperation.InvalidationHandlers;
using Services.Core.DataOperation.InvalidationHandlers.Interfaces;
using Services.Questions;
using Services.Structure;
using Services.VacationCalculation;
using System.Data;


public static class Startup
{
    public static IServiceProvider ConfigureServices()
    {
        // JSON dump not required.
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
                .WriteTo.File(Path.Combine(AppContext.BaseDirectory, "Logs/Log_.txt"),
                    encoding: System.Text.Encoding.UTF8,
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 22)
            .CreateLogger();

        var services = new ServiceCollection();
        
        services.AddMemoryCache();

        services.AddLogging(builder =>
        {
            builder.ClearProviders();
            builder.AddSerilog();
        });

        services.AddScoped<IDbConnection>(sp =>
        {
            var connection = new SqliteConnection("Data Source=app.db");
            return connection;
        });

        // Add daos
        services.AddScoped<IEmployeeDao, EmployeeDao>();
        services.AddScoped<ITeamDao, TeamDao>();
        services.AddScoped<IVacationPackageDao, VacationPackageDao>();
        services.AddScoped<IVacationDao, VacationDao>();
        
        services.AddScoped<IVacationReportRepository, VacationReportRepository>();
        services.AddScoped<ITeamReportRepository, TeamReportRepository>();
        
        services.AddScoped<IVacationReportService, VacationReportService>();
        services.AddScoped<ITeamReportService, TeamReportService>();

        // Add Services
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<ITeamService, TeamService>();
        services.AddScoped<IVacationPackageService, VacationPackageService>();
        services.AddScoped<IVacationService, VacationService>();

        services.AddScoped<IVacationCalculationService, VacationCalculationService>();
        services.AddScoped<IQuestionsService, QuestionsService>();

        // Events.        
        services.AddSingleton<IEventBus, InMemoryEventBus>();
        services.AddSingleton<CacheInvalidationHandler>();

        services.AddScoped<EmployeeStructureService>();
        services.AddScoped<DatabaseInitializerService>();
        
        
        services.AddSingleton<App>();

        return services.BuildServiceProvider();
    }
}