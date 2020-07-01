using System;
using System.Collections.Generic;
using System.Text;

namespace BuscadorPartitura.Infra.Constants
{
    /// <summary>
    /// Used to define name of configurations. It was created to centralize these values.
    /// </summary>
    public class DictionaryConstants
    {
        public const string CrawlerExePath = "CrawlerExePath";

        public const string RabbitMQHostName = "RabbitMQHostName";
        public const string RabbitMQUserName = "RabbitMQUserName";
        public const string RabbitMQPassword = "RabbitMQPassword";
                
        public const string OrquestradorGetSheetUrl = "OrquestradorGetSheetUrl";
                
        public const string DatabaseConnectionString = "SQLiteDatabaseConnection";
        
        public const string TelegramBotToken = "TelegramBotToken";

        public const string AppConfigurationConnectionString = "AppConfigurationConnectionString";
    }
}
