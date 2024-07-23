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
    public class AutoSignUpHybridController : ApiController
    {

        [System.Web.Http.HttpPost]

        public Response Insert([FromBody] object[] t)//(DataContracts.AutoSignUpData entity, bool batch)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;


            DataContracts.AutoSignUpData entity = JsonConvert.DeserializeObject<DataContracts.AutoSignUpData>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            bool batch = JsonConvert.DeserializeObject<bool>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);


            AutoSignUpWcfService AutoSignUpWcfService = new AutoSignUpWcfService();
            Response response = AutoSignUpWcfService.Insert(entity, batch);
            return response;
        }
    }
}