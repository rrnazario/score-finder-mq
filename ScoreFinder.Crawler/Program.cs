using System;

namespace ScoreFinder.Crawler
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
