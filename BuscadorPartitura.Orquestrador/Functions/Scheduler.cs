using System;
using BuscadorPartitura.Core.Interfaces;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace BuscadorPartitura.Orquestrador.Functions
{
    public class Scheduler
    {
        private readonly IDatabase _database;

        public Scheduler(IDatabase database)
        {
            _database = database;
        }

        [FunctionName("Scheduler")]
        public static void Run([TimerTrigger("* */1 * * * *")]TimerInfo myTimer, ILogger log) //Minuto em Minuto
        {
            //Ir de tempos em tempos no banco e ver se tem partitura pra ser consultada
            
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        }
    }
}
