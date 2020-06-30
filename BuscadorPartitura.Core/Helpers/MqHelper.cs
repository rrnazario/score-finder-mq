using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace BuscadorPartitura.Core.Helpers
{
    public class MqHelper
    {
        /// <summary>
        /// Creates queue name based on local machine information
        /// </summary>
        /// <returns></returns>
        public static string OrchestratorQueueName() => $"Orchestrator_{Environment.MachineName}";
        public static string OrchestratorQueueName(string machine) => $"Orchestrator_{machine}";
        public static string LocalMetricQueueName => $"Metric_{Environment.MachineName}";
        /// <summary>
        /// Return a telegram response queue name based on user chat id.
        /// </summary>
        /// <param name="chatId"></param>
        /// <returns></returns>
        public static string GetQueueChatName(long chatId) => $"TelegramBot{chatId}";
    }
}
