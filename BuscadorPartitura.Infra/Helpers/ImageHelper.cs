using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace BuscadorPartitura.Infra.Helpers
{
    public class ImageHelper
    {
        public static void DownloadImagem(string url, string caminhoDestino) => new WebClient().DownloadFile(url, caminhoDestino);
    }
}
