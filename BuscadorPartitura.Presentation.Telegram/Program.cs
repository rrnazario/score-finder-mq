using BuscadorPartitura.Core.Interfaces;
using BuscadorPartitura.Core.Misc.Constants;
using BuscadorPartitura.Core.Services;
using BuscadorPartitura.Infra.Constants;
using BuscadorPartitura.Infra.Helpers;
using BuscadorPartitura.Presentation.Telegram.Model;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;

namespace BuscadorPartitura.Presentation.Telegram
{
    class Program
    {
        static IMessageQueueConnection mq;
        static TelegramBotClient bot = new TelegramBotClient(EnvironmentHelper.GetValue("telegramBotToken"));
        static List<ChatMq> ActiveChats = new List<ChatMq>(); //Buscar do banco, caso morra a aplicação
        static void Main(string[] args)
        {
            //DI
            var serviceProvider = new ServiceCollection()
            .AddSingleton<IMessageQueueConnection, RabbitConnectionService>()
            .BuildServiceProvider();

            mq = serviceProvider.GetService<IMessageQueueConnection>();

            bot.OnMessage += Bot_OnMessage;

            bot.StartReceiving();

            while (true) { }
        }

        private static void Bot_OnMessage(object sender, global::Telegram.Bot.Args.MessageEventArgs e)
        {
            //Chamar API pra cadastrar o pedido
            using (var client = new HttpClient())
            {
                string queueName;
                var chat = ActiveChats.FirstOrDefault(a => a.ChatId == e.Message.Chat.Id);

                if (chat != null)
                    queueName = chat.QueueName;
                else
                {
                    queueName = $"TelegramBot{e.Message.Chat.FirstName}{DateTime.Now:ddMMyyyyHHmmssfff}";

                    chat = new ChatMq() { ChatId = e.Message.Chat.Id, QueueName = queueName };
                    ActiveChats.Add(chat);
                }

                Console.WriteLine($"Getting sender information:\n\tChatId: {chat.ChatId}\n\tLastUsedQueue: {queueName}\n\tLastAppUse: {chat.LastQuery}");

                chat.LastQuery = DateTime.Now;

                //Criando o conteudo para a Azure function
                var str = new StringContent(JsonConvert.SerializeObject(new { name = e.Message.Text, queue = queueName }), Encoding.Default, "application/json");

                //Cadastrar uma fila pra escutar até voltar as imagens
                mq.CreateQueue(queueName);
                mq.ConfigureConsumeQueueListener(queueName, false, sendResultSheets);

                //Chamando a Azure Function
                client.PostAsync(TelegramConstants.OrquestradorGetSheetUrl, str).GetAwaiter().GetResult();
            }
        }

        private static void sendResultSheets(object obj, object eventArgs)
        {
            var body = (eventArgs as BasicDeliverEventArgs).Body;
            var image = Encoding.UTF8.GetString(body.ToArray());

            //((EventingBasicConsumer)obj).Model.BasicAck(body.res)

            var chat = ActiveChats.First(f => f.QueueName == (eventArgs as BasicDeliverEventArgs).RoutingKey);
            if (image == ControllerConstants.NoDataMessage)
            {
                Console.WriteLine($"[{chat.ChatId}] Sending no data message'...");
                bot.SendTextMessageAsync(new ChatId(chat.ChatId), "Nada encontrado, desculpe!");
            }
            else
            {
                Console.WriteLine($"[{chat.ChatId}] Sending picture '{image}'...");
                bot.SendPhotoAsync(new ChatId(chat.ChatId), new InputOnlineFile(new Uri(image)));                
            }

            Console.WriteLine($"[{chat.ChatId}] Sent!");
        }
    }
}