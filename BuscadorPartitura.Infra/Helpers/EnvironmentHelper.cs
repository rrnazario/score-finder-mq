using BuscadorPartitura.Infra.Constants;
using Microsoft.Extensions.Configuration;
using System;
using System.Configuration;
using System.Linq;

namespace BuscadorPartitura.Infra.Helpers
{
    public static class EnvironmentHelper
    {
#if !DEBUG
        private static IConfigurationRoot _config;
        static EnvironmentHelper()
        {
            var builder = new ConfigurationBuilder();
            builder.AddAzureAppConfiguration(GetValue(DictionaryConstants.AppConfigurationConnectionString));

            _config = builder.Build();
        }
#endif

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
            var result = config.FirstOrDefault(f => f.Split('|').First().ToLower().Equals(name.ToLower()))?.Split('|')?.Last();

            return !string.IsNullOrEmpty(result) ? result : Environment.GetEnvironmentVariable(name);
#else
            return Environment.GetEnvironmentVariable(name) ?? _config[name] ?? string.empty; 
#endif
        }
    }
}
