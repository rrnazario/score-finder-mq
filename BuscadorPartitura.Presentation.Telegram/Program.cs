using BuscadorPartitura.Core.Misc.Constants;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using Telegram.Bot;

namespace BuscadorPartitura.Presentation.Telegram
{
    class Program
    {
        static void Main(string[] args)
        {
            var bot = new TelegramBotClient(Environment.GetEnvironmentVariable("telegramBotToken"));

            bot.OnMessage += Bot_OnMessage;

            bot.StartReceiving();

            while (true) { }
        }

        private static void Bot_OnMessage(object sender, global::Telegram.Bot.Args.MessageEventArgs e)
        {
            //Chamar API pra cadastrar o pedido
            using (var client = new HttpClient())
            {
                var str = new StringContent(JsonConvert.SerializeObject(new { name = e.Message.Text }), Encoding.Default, "text/json");

                client.PostAsync(TelegramConstants.OrquestradorDefaultUrl, str).ConfigureAwait(true);
            }
        }
    }
}