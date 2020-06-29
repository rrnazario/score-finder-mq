using BuscadorPartitura.Core.Misc.Enums;
using BuscadorPartitura.Crawler.Implementations;
using BuscadorPartitura.Crawler.Interfaces;
using BuscadorPartitura.Crawler.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BuscadorPartitura.Crawler
{
    public class CrawlerFactory
    {
        private Search search;
        private CrawlerEnums.TipoCrawler tipoCrawler;
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
                    return new ChoraMeuCavacoCrawler(search);
                case CrawlerEnums.TipoCrawler.Dropbox:
                    return new PersonalDropboxCrawler(search);
                default:
                    throw new NotImplementedException("Crawler ainda não implementado");
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
                throw new ArgumentException("Nenhum argumento válido");

            var pesquisa = new Search();

            for (int i = 0; i < args.Length;)
            {
                switch (args[i].Replace("--", "").ToLower())
                {
                    case "termo":
                        i++;

                        if (args[i].Contains("--"))
                            throw new ArgumentException("Termo não passado corretamente");

                        while (!args[i].StartsWith("--") || i >= args.Count() - 1)
                        {
                            pesquisa.Term = string.Join(" ", pesquisa.Term, args[i]).Trim();
                            i++;
                        }

                        if (string.IsNullOrEmpty(pesquisa.Term))
                            throw new ArgumentException("Termo não passado corretamente");
                        break;
                    case "tipo":
                        i++;
                        tipoCrawler = (CrawlerEnums.TipoCrawler)Enum.Parse(typeof(CrawlerEnums.TipoCrawler), args[i]);

                        if (!Enum.IsDefined(typeof(CrawlerEnums.TipoCrawler), tipoCrawler))
                            throw new ArgumentException("Tipo de crawler não implementado.");

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
