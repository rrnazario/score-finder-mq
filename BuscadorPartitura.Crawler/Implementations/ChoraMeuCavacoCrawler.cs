﻿using BuscadorPartitura.Crawler.Interfaces;
using BuscadorPartitura.Crawler.Model;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BuscadorPartitura.Crawler.Implementations
{
    public class ChoraMeuCavacoCrawler : ICrawler
    {
        private Search _pesquisa;

        public ChoraMeuCavacoCrawler(Search pesquisa)
        {
            _pesquisa = pesquisa;
        }

        public async Task<List<string>> GetImagesAsync()
        {
            if (string.IsNullOrEmpty(_pesquisa.Term))
                throw new ArgumentException("termo");

            using (var client = new HttpClient())
            {
                var result = await client.GetAsync(@$"https://www.chorameucavaco.com.br/pesquisa/{ParseArgs()}");

                var html = new HtmlDocument();
                html.LoadHtml(await result.Content.ReadAsStringAsync());
                var nosPesquisaPartitura = html.DocumentNode.SelectNodes("//a[@class='button button-green radius block']").Where(w => w.InnerHtml.Contains("Ver Partitura"));

                foreach (var ahref in nosPesquisaPartitura)
                {
                    var linkPaginaPartitura = ahref.Attributes["href"].Value;

                    result = await client.GetAsync(linkPaginaPartitura);

                    html.LoadHtml(await result.Content.ReadAsStringAsync());

                    _pesquisa.ResultUrls.AddRange(html.DocumentNode.SelectNodes("//img[@class='jwc_target']").Select(s => s.Attributes["src"].Value));                    
                }
            }

            return _pesquisa.ResultUrls.Distinct().ToList();
        }

        private string ParseArgs() => string.Join("+", _pesquisa.Term.Split(' '));
    }
}
