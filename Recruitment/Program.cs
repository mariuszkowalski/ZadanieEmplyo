using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Services.Core.DataOperation.InvalidationHandlers;

class Program
{
    static void Main(string[] args)
    {
        var serviceProvider = Startup.ConfigureServices();
        
        serviceProvider.GetRequiredService<CacheInvalidationHandler>();

        var app = serviceProvider.GetRequiredService<App>();

        app.Run();

        Log.CloseAndFlush();
    }
}
