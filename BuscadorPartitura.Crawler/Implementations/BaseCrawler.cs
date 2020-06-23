using BuscadorPartitura.Crawler.Interfaces;
using BuscadorPartitura.Crawler.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BuscadorPartitura.Crawler.Implementations
{
    public abstract class BaseCrawler : ICrawler
    {
        protected Search _pesquisa;        
        public BaseCrawler(Search pesquisa)
        {
            _pesquisa = pesquisa;
        }

        public abstract Task<List<string>> GetImagesAsync();
    }
}
