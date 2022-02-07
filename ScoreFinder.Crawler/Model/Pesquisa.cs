using System;
using System.Collections.Generic;
using System.Text;

namespace ScoreFinder.Crawler.Model
{
    public class Search
    {
        public string Term { get; set; }
        public List<string> ResultUrls { get; set; }

        public Search()
        {
            ResultUrls = new List<string>();
        }
    }
}
