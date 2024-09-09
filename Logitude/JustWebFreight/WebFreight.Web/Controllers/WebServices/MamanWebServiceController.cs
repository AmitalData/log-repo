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
        [TokenAutherize]
        [HttpGet]
        [HttpPut]
        [HttpDelete]
        [HttpPost]
        public HttpResponseMessage Handler()
        {
            RelatedLogEntity entity = new RelatedLogEntity() { EntityID = "mamanApi", EntityType = "API" };

            try
            {
                int tenant = HeaderHelper.Authenticate().Tenant;
                string path = Request.RequestUri.AbsolutePath.Replace("api/maman/", "");
                string body = Request.Content.ReadAsStringAsync().Result;
                HttpMethod method = Request.Method;

                DevLog.Instance.WriteDebug($"mamanApi in, path: " + path + ", tenant: " + tenant + ", body: " + body + ", method: " + method, entity);

                HttpResponseMessage res = MamanService.Send(path, body, tenant, method);
                return res;
            }
            catch (Exception e)
            {
                DevLog.Instance.WriteFatal(e, "mamanApi error: " + e.Message, entity);

                var res = new { Status = (int)HttpStatusCode.InternalServerError, Error = "Internal error", Success = false };
                return Request.CreateResponse(HttpStatusCode.OK, res);
            }
        }
    }
}