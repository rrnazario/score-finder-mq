using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuscadorPartitura.Core.Interfaces
{
    public interface IMessageQueueConnection
    {
        bool CreateQueue(string queueName);
        public bool WriteMessage(string message, string queueName);
    }
}
