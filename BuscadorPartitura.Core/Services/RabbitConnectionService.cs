using BuscadorPartitura.Core.Interfaces;
using BuscadorPartitura.Infra.Helpers;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuscadorPartitura.Core.Services
{
    public class RabbitConnectionService : IMessageQueueConnection
    {
        private static readonly IConnection _connection = CreateConnection(GetConnectionFactory());
        private static readonly IModel _channel = _connection.CreateModel();
        public IConnection Connection => _connection;
        public RabbitConnectionService() { }

        protected static ConnectionFactory GetConnectionFactory()
        {
            var connectionFactory = new ConnectionFactory
            {
                HostName = EnvironmentHelper.GetValue("RabbitMQHostName"),
                UserName = EnvironmentHelper.GetValue("RabbitMQUserName"),
                Password = EnvironmentHelper.GetValue("RabbitMQPassword")
            };

            return connectionFactory;
        }

        protected static IConnection CreateConnection(ConnectionFactory connectionFactory) => connectionFactory.CreateConnection();

        public bool CreateQueue(string queueName)
        {
            try
            {
                using (var channel = _connection.CreateModel())                
                    channel.QueueDeclare(queueName, false, false, true, null);                

                return true;
            }
            catch (Exception)
            {
                return false;
            }
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

        public void ConfigureConsumeQueueListener(string queueName, bool alwaysRedeliver, Action<object, object> eventHandler)
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (model, ea) => eventHandler(model, ea);

            _channel.BasicConsume(queue: queueName,
                                 autoAck: !alwaysRedeliver,
                                 consumer: consumer);

            //DeleteQueue(queueName);
        }
        public void DeleteQueue(string queueName) => _channel.QueueDelete(queueName, true, false);
    }
}
