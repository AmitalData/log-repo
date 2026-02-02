using Newtonsoft.Json.Linq;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using WebFreight.Web.Helpers.AmitalAPI;
using WebFreight.Web.Helpers.AmitalAPI.Structs;

namespace WebFreight.Web.Controllers.WebServices
{
    public class AmitalAPISchemaController : AmitalAPIControllerBase<AmitalApiSchemaApi, AmitalApiSchema>
    {
        AmitalApiSchemaApi schemaApi = new AmitalApiSchemaApi();

        [HttpGet]
        public HttpResponseMessage GetSettings()
        {
            string token = AutorizeAndGetToken();
            var res = schemaApi.GetSettings(token);
            return Request.CreateResponse(HttpStatusCode.OK, res);
        }

        [HttpPost]
        public HttpResponseMessage PostRequestQuery([FromBody] JObject parameters)
        {
            string token = AutorizeAndGetToken();
            object res = schemaApi.GetRequestQuery(token, parameters);

            return res == null ?
                Request.CreateResponse(HttpStatusCode.BadRequest) :
                Request.CreateResponse(HttpStatusCode.OK, res);
        }

        [HttpPost]
        public HttpResponseMessage PostRequeue([FromBody] JObject request)
        {
            string token = AutorizeAndGetToken();

            string[] ids = request["Ids"]?.ToObject<string[]>();

            if (ids == null || ids.Length == 0)
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { success = false, message = "No IDs provided" });

            bool success = schemaApi.Requeue(token, ids);

            return success ?
                Request.CreateResponse(HttpStatusCode.OK, new { success = true }) :
                Request.CreateResponse(HttpStatusCode.BadRequest, new { success = false, message = "Failed to requeue items" });
        }

        [HttpGet]
        public HttpResponseMessage GetDownloadRequest(string id)
        {
            string token = AutorizeAndGetToken();
            Stream stream = schemaApi.DownloadRequest(token, id);

            if (stream == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StreamContent(stream);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            return response;
        }
    }
}