﻿using System;
using System.Linq;

namespace BuscadorPartitura.Infra.Helpers
{
    public class EnvironmentHelper
    {
        /// <summary>
        /// Get value from environment variable
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetValue(string name)
        {
#if DEBUG
            var config = System.IO.File.ReadAllLines("C:\\temp\\mqConf.txt").ToList();
            return config.First(f => f.Split('=').First().Equals(name)).Split('=').Last();
#else
            return Environment.GetEnvironmentVariable(name); 
#endif
        }
    }
}