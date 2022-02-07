using ScoreFinder.Crawler;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ScoreFinder.Tests
{
    [TestClass]
    public class EndToEndTestes
    {
#warning TODO: Implement this test!
        [TestMethod]
        public void FAZER_ESSE_METODO()
        {
            var args = new string[]
            {
                "--termo","sem você a vida é tão","--tipo","0"
            };

            var factory = new CrawlerFactory(args);

            Assert.AreEqual(factory.GetCrawler().GetImagesAsync().GetAwaiter().GetResult().Count, 1);
        }
    }
}