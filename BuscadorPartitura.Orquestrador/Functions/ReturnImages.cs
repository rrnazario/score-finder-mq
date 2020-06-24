using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Linq;

namespace BuscadorPartitura.Orquestrador.Functions
{
    public class ReturnImages
    {
        [FunctionName("ReturnImages")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
#warning ROGIM: Will get images link and send to enduser

            string returnUrls = req.Query["returnUrls"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            returnUrls = returnUrls ?? data?.returnUrls;

            return new JsonResult(JsonConvert.SerializeObject(returnUrls.Split(',').Select(s => new { Sheet = s })));
        }
    }
}
