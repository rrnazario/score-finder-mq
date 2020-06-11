using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace BuscadorPartitura.Controller.Model
{
    public class RunningCrawlers
    {
        public Process RunningProcess { get; set; }
        public List<string> Images { get; set; }

        public RunningCrawlers()
        {
            Images = new List<string>();
        }
    }
}
