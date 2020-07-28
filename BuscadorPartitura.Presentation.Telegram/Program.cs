using BuscadorPartitura.Core.Helpers;
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
        static TelegramBotClient bot = new TelegramBotClient(EnvironmentHelper.GetValue(DictionaryConstants.TelegramBotToken));
        static Dictionary<long, ChatMq> ActiveChats = new Dictionary<long, ChatMq>(); //Buscar do banco, caso morra a aplicação
        private static readonly HttpClient _client = new HttpClient();
        static void Main(string[] args)
        {            
            mq = Injection.GetService<IMessageQueueConnection>();

            bot.OnMessage += Bot_OnMessage;

            bot.StartReceiving();

            while (true) { }
        }

        private static void Bot_OnMessage(object sender, global::Telegram.Bot.Args.MessageEventArgs e)
        {
            //Chamar API pra cadastrar o pedido
            {
                ChatMq chat;

                if (ActiveChats.ContainsKey(e.Message.Chat.Id))                
                    chat = ActiveChats[e.Message.Chat.Id];                
                else
                {
                    chat = new ChatMq() { ChatId = e.Message.Chat.Id, QueueName = MqHelper.GetQueueChatName(e.Message.Chat.Id) };
                    ActiveChats.Add(e.Message.Chat.Id, chat);
                }

                Console.WriteLine($"Getting sender information:\n\tChatId: {chat.ChatId}\n\tLastUsedQueue: {chat.QueueName}\n\tLastAppUse: {chat.LastQuery}");

                chat.LastQuery = DateTime.Now;

                //Criando o conteudo para a Azure function
                var str = new StringContent(JsonConvert.SerializeObject(new { name = e.Message.Text, queue = chat.QueueName }), Encoding.Default, "application/json");

                //Cadastrar uma fila pra escutar até voltar as imagens
                mq.CreateQueue(chat.QueueName);
                mq.ConfigureConsumeQueueListener(chat.QueueName, false, sendResultSheets);

                //Chamando a Azure Function
                _client.PostAsync(TelegramConstants.OrquestradorGetSheetUrl, str).GetAwaiter().GetResult();
            }
        }

        private static void sendResultSheets(object obj, object eventArgs)
        {
            var body = (eventArgs as BasicDeliverEventArgs).Body;
            var image = Encoding.UTF8.GetString(body.ToArray());

            //((EventingBasicConsumer)obj).Model.BasicAck(body.res)

            var chat = ActiveChats.Values.First(f => f.QueueName == (eventArgs as BasicDeliverEventArgs).RoutingKey);
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