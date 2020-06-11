using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BuscadorPartitura.Crawler.Interfaces
{
    public interface ICrawler
    {
        Task<List<string>> GetImagesAsync();
    }
}
