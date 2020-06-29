using BuscadorPartitura.Crawler;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuscadorPartitura.Tests
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