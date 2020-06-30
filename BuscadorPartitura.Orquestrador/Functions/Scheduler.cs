using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BuscadorPartitura.Core.Helpers;
using BuscadorPartitura.Core.Interfaces;
using BuscadorPartitura.Core.Misc.Constants;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BuscadorPartitura.Orquestrador.Functions
{
    public class Scheduler
    {
        private readonly IDatabase _database;
        private readonly IMessageQueueConnection _mq;


        public Scheduler(IDatabase database, IMessageQueueConnection mq)
        {
            _database = database;
            _mq = mq;
        }

        [FunctionName("Scheduler")]
        public void Run([TimerTrigger("0 */10 * * * *")]TimerInfo myTimer, ILogger log)
        {
            var scheduledSearches = _database.GetSheetsToSearch();

            Parallel.ForEach(scheduledSearches, search =>
            {
                //Chamar API pra cadastrar o pedido
                using (var client = new HttpClient())
                {
                    string queueName = MqHelper.GetQueueChatName(search.ChatId);

                    //Criando o conteudo para a Azure function
                    var str = new StringContent(JsonConvert.SerializeObject(new { name = search.Term, queue = queueName }), Encoding.Default, "application/json");

                    //Cadastrar uma fila pra escutar até voltar as imagens
                    _mq.CreateQueue(queueName);

                    //Chamando a Azure Function
                    client.PostAsync(TelegramConstants.OrquestradorGetSheetUrl, str).GetAwaiter().GetResult();

                    client.Dispose();
                }
            });
            
            log.LogInformation($"Scheduler runned at '{DateTime.Now}'");


        }
    }
}
