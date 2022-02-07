using System.Net;

namespace ScoreFinder.Infra.Helpers
{
    public class ImageHelper
    {
        public static void DownloadImage(string url, string targetPath)
        {
            using (var client = new WebClient())
                client.DownloadFile(url, targetPath);
        }
    }
}
