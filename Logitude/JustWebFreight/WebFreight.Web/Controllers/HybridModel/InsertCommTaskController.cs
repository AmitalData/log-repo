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
    public class InsertCommTaskController : ApiController
    {
        [System.Web.Http.HttpPost]
        public Response InsertTask([FromBody] object[] t)//(int myTenant, int destinationTenant, int priority, string subject, List<QueueTask> queueTasks)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;


            int myTenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            int destinationTenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);
            int priority = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[2]), jsonSerializerSettings);
            string subject = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[3]), jsonSerializerSettings);
            List<QueueTask> queueTasks = JsonConvert.DeserializeObject<List<QueueTask>>(JsonConvert.SerializeObject(t[4]), jsonSerializerSettings);


            InsertCommTaskWcfService InsertCommTaskWcfService = new InsertCommTaskWcfService();
            Response response = InsertCommTaskWcfService.InsertTask(myTenant, destinationTenant, priority, subject, queueTasks);
            return response;
        }
    }
}
