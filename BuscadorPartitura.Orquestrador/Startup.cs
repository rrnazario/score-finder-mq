using BuscadorPartitura.Core.Interfaces;
using BuscadorPartitura.Core.Services;
using BuscadorPartitura.Orquestrador;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

[assembly: FunctionsStartup(typeof(Startup))]
namespace BuscadorPartitura.Orquestrador
{    
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddScoped<IDatabase, Database>();

            builder.Services.AddSingleton<IMessageQueueConnection, RabbitConnection>();

            builder.Services.AddLogging();
        }
    }
}
