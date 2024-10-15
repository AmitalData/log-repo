using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebFreight.Web.WcfApi;

namespace WebFreight.Web.Controllers.HybridModel
{
    public class ImporterDepositionHybridController : ApiController
    {
        [System.Web.Http.HttpPost]
        public async Task<Response> SendImporterDepositionToLogBox([FromBody] object[] t)//(ImporterDepositionPM importerDepositionPM)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;


            ImporterDepositionPM importerDepositionPM = JsonConvert.DeserializeObject<ImporterDepositionPM>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);

            ImporterDepositionWcfService ImporterDepositionWcfService = new ImporterDepositionWcfService();
            Response response = await ImporterDepositionWcfService.SendImporterDepositionToLogBox(importerDepositionPM);
            return response;
        }
    }
}
