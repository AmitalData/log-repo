using System;
using System.Net.Http;
using System.Net;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Controllers.WebServices.Services;
using NetCommonHelper.Logger;

namespace WebFreight.Web.Controllers.WebServices
{
    public class MamanWebServiceController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage Send([FromBody] dynamic bodyDynamic, string interfacename)
        {
            int tenant = HeaderHelper.Authenticate().Tenant;

            RelatedLogEntity entity = new RelatedLogEntity() { EntityID = "mamanApi", EntityType = "API" };

            try
            {
                string body = Convert.ToString(bodyDynamic);
                DevLog.Instance.WriteDebug($"mamanApi request in, tenant: " + tenant + ", body: " + body, entity);

                HttpResponseMessage res = MamanService.Send(tenant, interfacename, body);

                DevLog.Instance.WriteDebug($"mamanApi response out, status: " + res.StatusCode + ", res: " + res.Content.ReadAsStringAsync().Result, entity);

                return res;
            }
            catch (Exception e)
            {
                DevLog.Instance.WriteFatal(e, "mamanApi error: " + e.Message, entity);

                var res = new { Status = (int)HttpStatusCode.InternalServerError, Error = "Internal error", Success = false };
                return Request.CreateResponse(HttpStatusCode.OK, res);
            }
        }

        public class MamanSendArgs
        {
            public string MamanUrl { get; set; }
            public string ServiceUrl { get; set; }
            public string Body { get; set; }
            public string Data { get; set; }
        }
    }
}