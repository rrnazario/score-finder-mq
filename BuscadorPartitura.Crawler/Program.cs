using BuscadorPartitura.Infra.Helpers;
using System;
using System.Linq;

namespace BuscadorPartitura.Crawler
{
    class Program
    {
        static void Main(string[] args)
        {
            var crawlerFactory = new CrawlerFactory(args);

            var crawler = crawlerFactory.GetCrawler();

            var images = crawler.GetImagesAsync().GetAwaiter().GetResult();

            foreach (var image in images)
                Console.WriteLine(image);
        }
    }
}
