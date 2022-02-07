using System.Collections.Concurrent;
using System.Collections.Generic;

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
