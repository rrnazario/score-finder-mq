using ScoreFinder.Crawler.Interfaces;
using ScoreFinder.Crawler.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScoreFinder.Crawler.Implementations
{
    public class PersonalDropboxCrawler : BaseCrawler, ICrawler
    {
        public PersonalDropboxCrawler(Search search) : base(search) { }
        
        public override Task<List<string>> GetImagesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
