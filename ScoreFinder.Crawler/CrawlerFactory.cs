using ScoreFinder.Core.Misc.Enums;
using ScoreFinder.Crawler.Implementations;
using ScoreFinder.Crawler.Interfaces;
using ScoreFinder.Crawler.Model;
using System;
using System.Linq;
using System.Net.Http;

namespace ScoreFinder.Crawler
{
    public class CrawlerFactory
    {
        private Search search;
        private CrawlerEnums.TipoCrawler tipoCrawler;

        private static HttpClient _httpClient = new HttpClient();
        public CrawlerFactory(string[] args)
        {
            search = HandleArgs(args);
        }

        /// <summary>
        /// Returns correct crawler to extract sheets.
        /// </summary>
        /// <returns></returns>
        public ICrawler GetCrawler()
        {
            switch (tipoCrawler)
            {
                case CrawlerEnums.TipoCrawler.ChoraMeuCavaco:
                    return new ChoraMeuCavacoCrawler(search, _httpClient);
                case CrawlerEnums.TipoCrawler.Dropbox:
                    return new PersonalDropboxCrawler(search);
                default:
                    throw new NotImplementedException(tipoCrawler.ToString());
            }
        }

        /// <summary>
        /// Generate search info to crawlers based on args.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        Search HandleArgs(string[] args)
        {
            if (args.Length == 0)
                throw new ArgumentException("args");

            var pesquisa = new Search();

            for (int i = 0; i < args.Length;)
            {
                switch (args[i].Replace("--", "").ToLower())
                {
                    case "termo":
                        i++;

                        if (args[i].Contains("--"))
                            throw new ArgumentException("Term");

                        while (!args[i].StartsWith("--") || i >= args.Count() - 1)
                        {
                            pesquisa.Term = string.Join(" ", pesquisa.Term, args[i]).Trim();
                            i++;
                        }

                        if (string.IsNullOrEmpty(pesquisa.Term))
                            throw new ArgumentException("Term");
                        break;
                    case "tipo":
                        i++;
                        tipoCrawler = Enum.Parse<CrawlerEnums.TipoCrawler>(args[i]);

                        if (!Enum.IsDefined(typeof(CrawlerEnums.TipoCrawler), tipoCrawler))
                            throw new NotImplementedException(tipoCrawler.ToString());

                        break;
                    default:
                        i++;
                        break;
                }
            }

            return pesquisa;
        }
    }
}
