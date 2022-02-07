using ScoreFinder.Crawler.Interfaces;
using ScoreFinder.Crawler.Model;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ScoreFinder.Crawler.Implementations
{
    public class ChoraMeuCavacoCrawler : BaseCrawler, ICrawler
    {
        public ChoraMeuCavacoCrawler(Search pesquisa) : base(pesquisa) { }
        public override async Task<List<string>> GetImagesAsync()
        {
            if (string.IsNullOrEmpty(_pesquisa.Term))
                throw new ArgumentException("termo");

            using (var client = new HttpClient())
            {
                var result = await client.GetAsync(new Uri(Path.Combine(@$"https://www.chorameucavaco.com.br/pesquisa/", _pesquisa.Term)));

                var html = new HtmlDocument();
                html.LoadHtml(await result.Content.ReadAsStringAsync());
                var nosPesquisaPartitura = html.DocumentNode.SelectNodes("//a[@class='button button-green radius block']").Where(w => w.InnerHtml.Contains("Ver Partitura"));

                Parallel.ForEach(nosPesquisaPartitura, async ahref =>
                {
                    var linkPaginaPartitura = ahref.Attributes["href"].Value;

                    using (var innerClient = new HttpClient())
                    {
                        result = await innerClient.GetAsync(linkPaginaPartitura);

                        html.LoadHtml(await result.Content.ReadAsStringAsync());
                    }

                    _pesquisa.ResultUrls.AddRange(html.DocumentNode.SelectNodes("//img[@class='jwc_target']").Select(s => s.Attributes["src"].Value));
                });
            }

            return _pesquisa.ResultUrls.Distinct().ToList();
        }
    }
}
