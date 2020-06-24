using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using BuscadorPartitura.Core.Interfaces;
using BuscadorPartitura.Infra.Constants;

namespace BuscadorPartitura.Orquestrador.Functions
{
    public class GetSheet
    {
        private readonly IMessageQueueConnection _mqConnection;
        private readonly IDatabase _database;

        public GetSheet(IMessageQueueConnection mqConnection,
                        IDatabase database)
        {
            _mqConnection = mqConnection;
            _database = database;
        }


        [FunctionName("GetSheet")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            //Get sheet name and get where should search it
            string name = req.Query["name"];
            if (string.IsNullOrEmpty(name))
            {
                using (var stream = new StreamReader(req.Body))
                {
                    string requestBody = await stream.ReadToEndAsync();
                    dynamic data = JsonConvert.DeserializeObject(requestBody);
                    name = data?.name;
                }
            }

#warning ROGIM: Go to database to get best idle machine

            //Create MQ message
            _mqConnection.CreateQueue(FunctionsConstants.OrchestratorQueueName);
            _mqConnection.WriteMessage(name, FunctionsConstants.OrchestratorQueueName);
           
            return new OkObjectResult("MQ sent");
        }
    }
}
