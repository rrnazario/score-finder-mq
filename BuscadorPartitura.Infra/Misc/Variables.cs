using System;

namespace BuscadorPartitura.Infra.Misc
{
    public class Variables
    {
        /// <summary>
        /// Get value from Azure Function application settings.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetValue(string name) => Environment.GetEnvironmentVariable(name);
    }
}
