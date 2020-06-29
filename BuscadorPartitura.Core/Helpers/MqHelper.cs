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
        public static string CreateQueueName() =>$"Orchestrator_{Environment.MachineName}";
    }
}
