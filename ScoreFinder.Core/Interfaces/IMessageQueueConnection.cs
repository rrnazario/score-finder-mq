using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScoreFinder.Core.Interfaces
{
    public interface IMessageQueueConnection
    {
        bool CreateQueue(string queueName);
        bool WriteMessage(string message, string queueName);
        void ConfigureConsumeQueueListener(string queueName, bool alwaysRedeliver, Action<object, object> eventHandler = null);
        void DeleteQueue(string queueName);
    }
}
