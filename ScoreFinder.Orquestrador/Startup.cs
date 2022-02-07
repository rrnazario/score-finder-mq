using ScoreFinder.Core.Interfaces;
using ScoreFinder.Core.Services;
using ScoreFinder.Orquestrador;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Startup))]
namespace ScoreFinder.Orquestrador
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<IDatabase, SQLiteDatabaseService>();
            builder.Services.AddSingleton<IMessageQueueConnection, RabbitConnectionService>();

            builder.Services.AddLogging();
        }
    }
}
