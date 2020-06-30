using RabbitMQ.Client.Events;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BuscadorPartitura.Controller.Model
{
    public class RunningCrawler
    {
        //public Process RunningProcess { get; set; }
        public int ProcessId { get; set; }
        public List<string> Images { get; set; }
        public string QueueReturnName { get; set; }
        public string ArgumentsToRun { get; set; }
        public bool ToErase { get; set; }
        public bool Redelivered { get; set; }

        public RunningCrawler()
        {
            Images = new List<string>();
            ToErase = false;
        }

        public RunningCrawler(object eventArgs) : this()
        {
            var body = (eventArgs as BasicDeliverEventArgs).Body;
            var arguments = Encoding.UTF8.GetString(body.ToArray()); //--termo TERMO DE BUSCA PODENDO TER ESPACO --tipo 0|queueName            

            Redelivered = (eventArgs as BasicDeliverEventArgs).Redelivered;
            QueueReturnName = arguments.Split('|').Last();
            ArgumentsToRun = arguments.Split('|').First();
        }

        public override string ToString() => $"'{ArgumentsToRun}' Queue: '{QueueReturnName}'";
    }
}
