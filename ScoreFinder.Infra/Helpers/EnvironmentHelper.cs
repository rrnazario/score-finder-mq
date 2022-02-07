using System;
using System.Linq;

namespace ScoreFinder.Infra.Helpers
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
            return _config?[name] ?? Environment.GetEnvironmentVariable(name) ?? string.Empty; 
#endif
        }
    }
}
