using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using WebFreight.Web.WcfApi;

namespace WebFreight.Web.Controllers.HybridModel
{
    //api/HybridModel
    public class BranchHybridController : ApiController
    {
        [System.Web.Http.HttpPost]

        public Response Upsert([FromBody] object[] t)//(BranchPM entityPM, bool batch)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;


            BranchPM branchPM = JsonConvert.DeserializeObject<BranchPM>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            bool batch = JsonConvert.DeserializeObject<bool>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);


            BranchWcfService BranchWcfService = new BranchWcfService();
            Response response = BranchWcfService.Upsert(branchPM, batch);
            return response;

        }
    }
}