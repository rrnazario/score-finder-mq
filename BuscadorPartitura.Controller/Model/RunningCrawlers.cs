using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace BuscadorPartitura.Controller.Model
{
    public class RunningCrawler
    {
        //public Process RunningProcess { get; set; }
        public int ProcessId { get; set; }
        public List<string> Images { get; set; }
        public string QueueReturnName { get; set; }
        public bool ToErase { get; set; }

        public RunningCrawler()
        {
            Images = new List<string>();
            ToErase = false;
        }
    }
}
