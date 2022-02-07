﻿using ScoreFinder.Crawler.Interfaces;
using ScoreFinder.Crawler.Model;
using System;
using System.Collections.Generic;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace ScoreFinder.Crawler.Implementations
{
    public class PersonalDropboxCrawler : BaseCrawler, ICrawler
    {
        public PersonalDropboxCrawler(Search search) : base(search) { }
        
        public override Task<List<string>> GetImagesAsync()
        {
#warning TODO: Pegar do dropbox as imagens
            throw new NotImplementedException();
        }
    }
}
