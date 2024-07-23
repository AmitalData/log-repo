using Logitude.CRM.BL.EntityDws;
using Logitude.CRM.BL.EntityPMs;
using Logitude.Server.Tools;
using Newtonsoft.Json;
using Stimulsoft.Base.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using UnifreightIIG.Common.MessageLib.Unifreight.Customs;
using WebFreight.Web.WcfApi;
using JsonConvert = Newtonsoft.Json.JsonConvert;
using JsonSerializerSettings = Newtonsoft.Json.JsonSerializerSettings;
using MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling;


namespace WebFreight.Web.Controllers.HybridModel
{
    //api/HybridModel/Activity/upsert
    public class ActivityHybridController : ApiController
    {
        [System.Web.Http.HttpPost]
        public Response Upsert([FromBody] object[] t)//(Logitude.CRM.BL.EntityPMs.ActivityPM entityPM, string email)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;


            Logitude.CRM.BL.EntityPMs.ActivityPM entityPM = JsonConvert.DeserializeObject<Logitude.CRM.BL.EntityPMs.ActivityPM>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            string email = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);

            ActivityWcfService ActivityWcfService = new ActivityWcfService();
            Response response = ActivityWcfService.Upsert(entityPM, email);
            return response;

        }

        [System.Web.Http.HttpPost]
        public List<Logitude.CRM.BL.EntityPMs.ActivityPM> GetActivities([FromBody] object[] t)//(string email, int tenant, ref Response response)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;

            string email = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            int tenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);
            Response response = JsonConvert.DeserializeObject<Response>(JsonConvert.SerializeObject(t[2]), jsonSerializerSettings);

            ActivityWcfService ActivityWcfService = new ActivityWcfService();
            List<Logitude.CRM.BL.EntityPMs.ActivityPM> listResponse = ActivityWcfService.GetActivities(email, tenant, ref response);
            return listResponse;
        }


        [System.Web.Http.HttpPost]
        public Response UpdateOutlookID([FromBody] object[] t)//(string activityId, string outlookId, int tenant)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;


            string activityId = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            string outlookId = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);
            int tenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[2]), jsonSerializerSettings);

            ActivityWcfService ActivityWcfService = new ActivityWcfService();
            Response response = ActivityWcfService.UpdateOutlookID(activityId, outlookId, tenant);
            return response;

        }


        [System.Web.Http.HttpPost]
        public Response SetAsSynchronized([FromBody] object[] t)//(string activityId, string email, int tenant)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;


            string activityId = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            string email = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);
            int tenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[2]), jsonSerializerSettings);

            ActivityWcfService ActivityWcfService = new ActivityWcfService();
            Response response = ActivityWcfService.SetAsSynchronized(activityId, email, tenant);
            return response;


        }


        [System.Web.Http.HttpPost]
        public Response Delete([FromBody] object[] t)//(string activityId, int tenant)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;


            string activityId = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            int tenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);

            ActivityWcfService ActivityWcfService = new ActivityWcfService();
            Response response = ActivityWcfService.Delete(activityId, tenant);
            return response;

        }


        [System.Web.Http.HttpPost]
        public Response isOnline()
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;

            ActivityWcfService ActivityWcfService = new ActivityWcfService();
            Response response = ActivityWcfService.isOnline();
            return response;

        }


        [System.Web.Http.HttpPost]
        public ActivityPM GetActivityPM([FromBody] object[] t)//(string id, int tenant, ref Response response)
        {

            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;


            string id = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            int tenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);
            Response response = JsonConvert.DeserializeObject<Response>(JsonConvert.SerializeObject(t[2]), jsonSerializerSettings);

            ActivityWcfService ActivityWcfService = new ActivityWcfService();
            ActivityPM Response = ActivityWcfService.GetActivityPM(id, tenant, ref response);
            return Response;
        }


        [System.Web.Http.HttpPost]
        public Response UploadDocumentFileData([FromBody] object[] t)//(byte[] buffer, long fileSize, long sentBytes, string[] blockIdsList, int bufferNumber, int tenant, string FileNameWithExtention, string DocumentId)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;


            byte[] buffer = JsonConvert.DeserializeObject<byte[]>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            long fileSize = JsonConvert.DeserializeObject<long>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);
            long sentBytes = JsonConvert.DeserializeObject<long>(JsonConvert.SerializeObject(t[2]), jsonSerializerSettings);
            string[] blockIdsList = JsonConvert.DeserializeObject<string[]>(JsonConvert.SerializeObject(t[3]), jsonSerializerSettings);
            int bufferNumber = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[4]), jsonSerializerSettings);
            int tenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[5]), jsonSerializerSettings);
            string FileNameWithExtention = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[6]), jsonSerializerSettings);
            string DocumentId = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[7]), jsonSerializerSettings);



            ActivityWcfService ActivityWcfService = new ActivityWcfService();
            Response response = ActivityWcfService.UploadDocumentFileData(buffer, fileSize, sentBytes, blockIdsList, bufferNumber, tenant, FileNameWithExtention, DocumentId);
            return response;


        }



    }
}


