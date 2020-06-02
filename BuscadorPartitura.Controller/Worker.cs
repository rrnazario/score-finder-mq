using System;
using System.Diagnostics;

using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuscadorPartitura.Core.Interfaces;
using BuscadorPartitura.Infra.Misc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client.Events;

namespace BuscadorPartitura.Controller
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IMessageQueueConnection _mq;

        private readonly List<Process> runningControllers;

        public Worker(ILogger<Worker> logger, IMessageQueueConnection mq)
        {
            _logger = logger;
            _mq = mq;

            _mq.CreateQueue(FunctionsConstants.MetricsQueueName);
            _mq.ConfigureConsumeQueueListener(FunctionsConstants.OrchestratorQueueName, CreateCrawlers);            

            runningControllers = new List<Process>();
        }

        private void CreateCrawlers(object obj, object eventArgs)
        {
            var body = (eventArgs as BasicDeliverEventArgs).Body;
            var message = Encoding.UTF8.GetString(body.ToArray());

            var process = Process.Start("");

            runningControllers.Add(process);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                //Update statistics about processes
                if (runningControllers.Count > 0)
                {
                    foreach (var controller in runningControllers)
                    {
                        var memory = controller.PrivateMemorySize64;

                        //foreach (var instance in new PerformanceCounterCategory("Process").GetInstanceNames())
                        //{
                        //    if (instance.StartsWith(controller.ProcessName))
                        //    {
                        //        using var processId = new PerformanceCounter("Process", "ID Process", instance, true);

                        //        if (controller.Id == (int)processId.RawValue)
                        //        {
                        //            var name = instance;
                        //            break;
                        //        }
                        //    }
                        //}
                        

                        var metric = new
                        {

                        };
                        _mq.WriteMessage(metric.ToString(), FunctionsConstants.MetricsQueueName);
                    }
                }


                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
