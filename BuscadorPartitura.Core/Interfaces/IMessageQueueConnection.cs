using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuscadorPartitura.Core.Interfaces
{
    public interface IMessageQueueConnection
    {
        bool CreateQueue(string queueName);
        bool WriteMessage(string message, string queueName);
        void defaultEventHandler(object obj, object eventArgs);
        void ConfigureConsumeQueueListener(string queueName, Action<object, object> eventHandler = null);
    }
}
