using BuscadorPartitura.Core.Interfaces;
using BuscadorPartitura.Infra.Misc;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuscadorPartitura.Core.Services
{
    public class RabbitConnection : IMessageQueueConnection
    {
        private static readonly IConnection _connection = CreateConnection(GetConnectionFactory());
        public IConnection Connection => _connection;
        public RabbitConnection() { }

        protected static ConnectionFactory GetConnectionFactory()
        {
            var connectionFactory = new ConnectionFactory
            {
                HostName = Variables.GetValue("RabbitMQHostName"),
                UserName = Variables.GetValue("RabbitMQUserName"),
                Password = Variables.GetValue("RabbitMQPassword")
            };

            return connectionFactory;
        }

        protected static IConnection CreateConnection(ConnectionFactory connectionFactory) => connectionFactory.CreateConnection();

        public bool CreateQueue(string queueName)
        {
            try
            {
                using (var channel = _connection.CreateModel())
                {
                    channel.QueueDeclare(queueName, false, false, false, null);
                }

                return true;
            }
            catch (Exception)
            {
            }

            return false;
        }

        public bool WriteMessage(string message, string queueName)
        {
            using (var channel = _connection.CreateModel())
            {
                channel.BasicPublish(string.Empty, queueName, null, Encoding.ASCII.GetBytes(message));
            }

            return true;
        }

        public string RetrieveSingleMessage(string queueName)
        {
            BasicGetResult data;
            using (var channel = _connection.CreateModel())
            {
                data = channel.BasicGet(queueName, true);
            }
            return data != null ? Encoding.UTF8.GetString(data.Body.ToArray()) : null;
        }
    }
}
