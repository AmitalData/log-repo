using Logitude.BL.ShipmentsModel.EntityPMs;
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
    public class DeclarationApprovalRequestHybridController : ApiController
    {
        [System.Web.Http.HttpPost]
        public Response RequestDeclarationApproval([FromBody] object[] t)//(DeclarationApprovalRequestPM declarationApprovalRequestPM)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;


            DeclarationApprovalRequestPM declarationApprovalRequestPM = JsonConvert.DeserializeObject<DeclarationApprovalRequestPM>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);

            DeclarationApprovalRequestWcfService DeclarationApprovalRequestWcfService = new DeclarationApprovalRequestWcfService();
            Response response = DeclarationApprovalRequestWcfService.RequestDeclarationApproval(declarationApprovalRequestPM);
            return response;
        }
    }
}
