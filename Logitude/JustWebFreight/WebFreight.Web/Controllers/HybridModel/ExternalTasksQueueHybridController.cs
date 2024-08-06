using Logitude.Server.Tools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebFreight.Web.WcfApi;

namespace WebFreight.Web.Controllers.HybridModel
{
    public class ExternalTasksQueueHybridController : ApiController
    {
        [System.Web.Http.HttpPost]
        public Response GetDataCFIRDEC([FromBody] object[] t)//(Dictionary<string, string> queryParams, int tenant)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;


            Dictionary<string, string> queryParams = JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            int tenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);


            ExternalTasksQueueWcfService ExternalTasksQueueWcfService = new ExternalTasksQueueWcfService();
            Response response = ExternalTasksQueueWcfService.GetDataCFIRDEC(queryParams, tenant);
            return response;
        }


        [System.Web.Http.HttpPost]
        public Response LGTQuery([FromBody] object[] t)//(string queryId, Dictionary<string, string> queryParams, int tenant)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;

            string queryId = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            Dictionary<string, string> queryParams = JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);
            int tenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[2]), jsonSerializerSettings);

            ExternalTasksQueueWcfService ExternalTasksQueueWcfService = new ExternalTasksQueueWcfService();
            Response response = ExternalTasksQueueWcfService.LGTQuery(queryId, queryParams, tenant);
            return response;

        }


        [System.Web.Http.HttpPost]
        public Response LGTQueryExample([FromBody] object[] t)//(string queryId, Dictionary<string, string> queryParams, int tenant)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;


            string queryId = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            Dictionary<string, string> queryParams = JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);
            int tenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[2]), jsonSerializerSettings);


            ExternalTasksQueueWcfService ExternalTasksQueueWcfService = new ExternalTasksQueueWcfService();
            Response response = ExternalTasksQueueWcfService.LGTQueryExample(queryId, queryParams, tenant);
            return response;
        }


        [System.Web.Http.HttpPost]
        public Response LGTQueryOld([FromBody] object[] t)//(string queryId, Dictionary<string, string> queryParams, int tenant)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;


            string queryId = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            Dictionary<string, string> queryParams = JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);
            int tenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[2]), jsonSerializerSettings);


            ExternalTasksQueueWcfService ExternalTasksQueueWcfService = new ExternalTasksQueueWcfService();
            Response response = ExternalTasksQueueWcfService.LGTQueryOld(queryId, queryParams, tenant);
            return response;
        }


        [System.Web.Http.HttpPost]
        public string GetTaskFromQueue([FromBody] object[] t)//(int tenant, int priority)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;


            int tenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            int priority = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);


            ExternalTasksQueueWcfService ExternalTasksQueueWcfService = new ExternalTasksQueueWcfService();
            Response response = ExternalTasksQueueWcfService.GetTaskFromQueue(tenant, priority);
            return response.ToString();
        }


        [System.Web.Http.HttpPost]
        public Response MarkTaskAsDone([FromBody] object[] t)//(string communicationLogId, int tenant, int priority)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;

            string communicationLogId = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            int tenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);
            int priority = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[2]), jsonSerializerSettings);

            ExternalTasksQueueWcfService ExternalTasksQueueWcfService = new ExternalTasksQueueWcfService();
            Response Response = ExternalTasksQueueWcfService.MarkTaskAsDone(communicationLogId, tenant, priority);
            return Response;
        }
    }
}
