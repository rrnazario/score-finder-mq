using ScoreFinder.Crawler.Interfaces;
using ScoreFinder.Crawler.Model;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ScoreFinder.Crawler.Implementations
{
    public class ChoraMeuCavacoCrawler : BaseCrawler, ICrawler
    {
        private readonly HttpClient _httpClient;
        public ChoraMeuCavacoCrawler(Search pesquisa, HttpClient httpClient) : base(pesquisa)
        {
            _httpClient = httpClient;
        }
        public override async Task<List<string>> GetImagesAsync()
        {
            if (string.IsNullOrEmpty(_pesquisa.Term))
                throw new ArgumentException("termo");


            var result = await _httpClient.GetAsync(new Uri(Path.Combine(@$"https://www.chorameucavaco.com.br/pesquisa/", _pesquisa.Term)));

            var html = new HtmlDocument();
            html.LoadHtml(await result.Content.ReadAsStringAsync());
            var nosPesquisaPartitura = html.DocumentNode.SelectNodes("//a[@class='button button-green radius block']").Where(w => w.InnerHtml.Contains("Ver Partitura"));

            var tasks = new List<Task<IEnumerable<string>>>();

            foreach (var href in nosPesquisaPartitura)
            {
                tasks.Add(Task.Run(async () =>
                {
                    var sheetLink = href.Attributes["href"].Value;

                    result = await _httpClient.GetAsync(sheetLink);

                    html.LoadHtml(await result.Content.ReadAsStringAsync());

                    return html.DocumentNode.SelectNodes("//img[@class='jwc_target']").Select(s => s.Attributes["src"].Value);
                }));
            }

            Task.WaitAll(tasks.ToArray());
            _pesquisa.ResultUrls.AddRange(tasks.SelectMany(s => s.Result));

            return _pesquisa.ResultUrls.Distinct().ToList();
        }
    }
}
