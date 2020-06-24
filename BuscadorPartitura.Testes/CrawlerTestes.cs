using BuscadorPartitura.Crawler;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace BuscadorPartitura.Testes
{
    [TestClass]
    public class CrawlerTestes
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Parametro_Tipo_Errado_Buscador_Crawler()
        {
            var args = new List<string>() 
            { 
                "--termo","amor","--tipo","999"
            };

            new CrawlerFactory(args.ToArray());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Parametro_Termo_Faltando_Buscador_Crawler()
        {
            var args = new List<string>()
            {
                "--termo","--tipo","999"
            };

            new CrawlerFactory(args.ToArray());
        }

        [TestMethod]
        public void Chora_Meu_Cavaco_Uma_Partitura()
        {
            var args = new List<string>()
            {
                "--termo","sem você a vida é tão","--tipo","0"
            };

            var factory = new CrawlerFactory(args.ToArray());

            Assert.AreEqual(factory.GetCrawler().GetImagesAsync().GetAwaiter().GetResult().Count, 1);            
        }

        [TestMethod]
        public void Dropbox_Uma_Partitura()
        {
            var args = new List<string>()
            {
                "--termo","sem você a vida é tão","--tipo","1"
            };

            var factory = new CrawlerFactory(args.ToArray());

            Assert.AreEqual(factory.GetCrawler().GetImagesAsync().GetAwaiter().GetResult().Count, 1);
        }
    }
}
