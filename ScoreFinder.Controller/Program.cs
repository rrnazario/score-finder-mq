using ScoreFinder.Core.Interfaces;
using ScoreFinder.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ScoreFinder.Controller
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                    services.AddSingleton<IMessageQueueConnection, RabbitConnectionService>();
                    services.AddSingleton<IDatabase, SQLiteDatabaseService>();
                });
    }
}
