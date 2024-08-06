using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebFreight.Web.WcfApi;

namespace WebFreight.Web.Controllers.HybridModel
{
    public class DeclarationStatusHybridController : ApiController
    {
        [System.Web.Http.HttpPost]
        public Response Upsert([FromBody] object[] t)//(List<DeclarationStatusPM> entityPM, bool batch)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;


            List<DeclarationStatusPM> declarationStatusPM = JsonConvert.DeserializeObject<List<DeclarationStatusPM>>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            bool batch = JsonConvert.DeserializeObject<bool>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);


            DeclarationStatusWcfService DeclarationStatusWcfService = new DeclarationStatusWcfService();
            Response response = DeclarationStatusWcfService.Upsert(declarationStatusPM, batch);
            return response;
        }

        [System.Web.Http.HttpPost]
        public Response BuildDeclarationStatusesList([FromBody] object[] t)//(int tenant, string customFileNo, List<DeclarationStatusPM> DeclarationStatusesList)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;

            int tenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            string customFileNo = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);
            List<DeclarationStatusPM> DeclarationStatusesList = JsonConvert.DeserializeObject<List<DeclarationStatusPM>>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);



            DeclarationStatusWcfService DeclarationStatusWcfService = new DeclarationStatusWcfService();
            Response response = DeclarationStatusWcfService.BuildDeclarationStatusesList(tenant, customFileNo, DeclarationStatusesList);
            return response;
        }

    }
}
