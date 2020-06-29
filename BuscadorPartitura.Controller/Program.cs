using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuscadorPartitura.Core.Interfaces;
using BuscadorPartitura.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BuscadorPartitura.Controller
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
                });
    }
}
