using System;
using System.Collections.Generic;
using System.Text;

namespace BuscadorPartitura.Core.Model
{
    public class MetricStatus
    {
        public float CpuUsage { get; set; }
        public float MemoryUsage { get; set; }
        public string MachineName { get; set; }
    }
}
