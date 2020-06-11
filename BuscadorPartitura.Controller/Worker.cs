using System;
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuscadorPartitura.Core.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client.Events;
using BuscadorPartitura.Controller.Model;
using BuscadorPartitura.Infra.Constants;

namespace BuscadorPartitura.Controller
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IMessageQueueConnection _mq;

        private readonly List<RunningCrawlers> runningCrawlers;

        public Worker(ILogger<Worker> logger, IMessageQueueConnection mq)
        {
            _logger = logger;
            _mq = mq;

            _mq.CreateQueue(FunctionsConstants.MetricsQueueName);
            _mq.ConfigureConsumeQueueListener(FunctionsConstants.OrchestratorQueueName, CreateCrawler);            

            runningCrawlers = new List<RunningCrawlers>();
        }

        private void CreateCrawler(object obj, object eventArgs)
        {
            var body = (eventArgs as BasicDeliverEventArgs).Body;
            var arguments = Encoding.UTF8.GetString(body.ToArray()); //--termo blabla --tipo 0

            var crawler = new RunningCrawlers();

            //var process = Process.Start(SystemConstants.CrawlerExeName, arguments);
            crawler.RunningProcess = new Process();
            crawler.RunningProcess.StartInfo.FileName = ControllerConstants.CrawlerExeName;
            crawler.RunningProcess.StartInfo.Arguments = arguments;
            crawler.RunningProcess.StartInfo.UseShellExecute = false;
            crawler.RunningProcess.StartInfo.RedirectStandardError = true;
            crawler.RunningProcess.StartInfo.RedirectStandardOutput = true;

            var images = new List<string>();
            crawler.RunningProcess.OutputDataReceived += (s, e) =>
            {
                crawler.Images.AddRange(e.Data.Split("\n"));
            };

            crawler.RunningProcess.Start();
            crawler.RunningProcess.BeginErrorReadLine();
            crawler.RunningProcess.BeginOutputReadLine();

            runningCrawlers.Add(crawler);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                //Update statistics about processes
                if (runningCrawlers.Count > 0)
                {
                    foreach (var crawler in runningCrawlers)
                    {
                        var memory = crawler.RunningProcess.PrivateMemorySize64;

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
