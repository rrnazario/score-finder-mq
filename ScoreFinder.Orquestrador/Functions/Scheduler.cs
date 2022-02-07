using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ScoreFinder.Core.Helpers;
using ScoreFinder.Core.Interfaces;
using ScoreFinder.Infra.Constants;
using ScoreFinder.Infra.Helpers;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ScoreFinder.Orquestrador.Functions
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

                    //Cadastrar uma fila pra escutar at� voltar as imagens
                    _mq.CreateQueue(queueName);

                    //Chamando a Azure Function
                    client.PostAsync(EnvironmentHelper.GetValue(DictionaryConstants.OrquestradorGetSheetUrl), str).GetAwaiter().GetResult();
                }
            });
            
            log.LogInformation($"Scheduler runned at '{DateTime.Now}'");


        }
    }
}
