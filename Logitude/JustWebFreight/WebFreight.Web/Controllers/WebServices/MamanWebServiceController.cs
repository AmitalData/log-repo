using System;
using System.Net.Http;
using System.Net;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Controllers.WebServices.Services;
using NetCommonHelper.Logger;
using System.Collections.Generic;
using System.Web;

namespace WebFreight.Web.Controllers.WebServices
{
    public class MamanWebServiceController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage Send([FromBody] dynamic bodyDynamic, string interfacename, [FromUri] Dictionary<string, string> queryParams)
        {
            int tenant = HeaderHelper.Authenticate().Tenant;

            RelatedLogEntity entity = new RelatedLogEntity() { EntityID = "mamanApi", EntityType = "API" };

            try
            {
                string body = Convert.ToString(bodyDynamic);
                DevLog.Instance.WriteDebug($"mamanApi request in, tenant: " + tenant + ", body: " + body, entity);

                Dictionary<string, string> paramas = GetParams();

                HttpResponseMessage res = MamanService.Send(tenant, interfacename, body, paramas);

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


        private Dictionary<string, string> GetParams()
        {
            Dictionary<string, string> qs = new Dictionary<string, string>();
            foreach (string key in HttpContext.Current.Request.QueryString)
                qs[key] = HttpContext.Current.Request.QueryString[key];

            qs.Remove("interfacename");

            string parmeters = "";
            foreach (var item in qs)
                parmeters += item.Key + " = " + item.Value + ", ";

            DevLog.Instance.WriteDebug("mamanApi request parameters: " + parmeters);
            return qs;
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