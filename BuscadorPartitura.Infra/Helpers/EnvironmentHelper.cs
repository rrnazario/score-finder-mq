using System;
using System.Linq;

namespace BuscadorPartitura.Infra.Helpers
{
    public class EnvironmentHelper
    {
#warning TODO: Get these informations in a Vault, or something like that, instead environment variables (?)
        /// <summary>
        /// Get value from environment variable
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetValue(string name)
        {
#if DEBUG
            var mockFileFullPath = "C:\\temp\\mqConf.txt";

            var config = System.IO.File.ReadAllLines(mockFileFullPath).ToList();
            var result = config.FirstOrDefault(f => f.Split('|').First().Equals(name))?.Split('|')?.Last();

            return !string.IsNullOrEmpty(result) ? result : Environment.GetEnvironmentVariable(name);
#else
            return Environment.GetEnvironmentVariable(name); 
#endif
        }
    }
}
