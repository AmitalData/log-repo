using Logitude.BL.CommonDataModel.EntityDws;
using Logitude.CRM.BL.EntityDws;
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
    public class ActivityDWHybridController : ApiController
    {
        [System.Web.Http.HttpPost]
        public List<ActivitiyDW> GetActivitiesByDates([FromBody] object[] t)//(int tenant, DateTime fromDate, DateTime toDate, int skip, int take, ref Response response)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;

            int tenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            DateTime fromDate = JsonConvert.DeserializeObject<DateTime>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);
            DateTime toDate = JsonConvert.DeserializeObject<DateTime>(JsonConvert.SerializeObject(t[2]), jsonSerializerSettings);
            int skip = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[3]), jsonSerializerSettings);
            int take = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[4]), jsonSerializerSettings);
            Response response = JsonConvert.DeserializeObject<Response>(JsonConvert.SerializeObject(t[5]), jsonSerializerSettings);

            ActivityDWWcfService ActivityDWWcfService = new ActivityDWWcfService();
            List<ActivitiyDW> listResponse = ActivityDWWcfService.GetActivitiesByDates(tenant, fromDate, toDate, skip, take, ref response);
            return listResponse;
        }


        [System.Web.Http.HttpPost]
        public int GetActivitiesCountByDates([FromBody] object[] t)//(int tenant, DateTime fromDate, DateTime toDate, ref Response response)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;

            int tenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            DateTime fromDate = JsonConvert.DeserializeObject<DateTime>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);
            DateTime toDate = JsonConvert.DeserializeObject<DateTime>(JsonConvert.SerializeObject(t[2]), jsonSerializerSettings);
            Response response = JsonConvert.DeserializeObject<Response>(JsonConvert.SerializeObject(t[3]), jsonSerializerSettings);

            ActivityDWWcfService ActivityDWWcfService = new ActivityDWWcfService();
            int Response = ActivityDWWcfService.GetActivitiesCountByDates(tenant, fromDate, toDate, ref response);
            return Response;
        }


        [System.Web.Http.HttpPost]
        public List<ActivitiyDW> GetActivitiesByUpdateDate([FromBody] object[] t)//(int tenant, DateTime updateDate, int skip, int take, ref Response response)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;

            int tenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            DateTime updateDate = JsonConvert.DeserializeObject<DateTime>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);
            int skip = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[2]), jsonSerializerSettings);
            int take = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[3]), jsonSerializerSettings);
            Response response = JsonConvert.DeserializeObject<Response>(JsonConvert.SerializeObject(t[4]), jsonSerializerSettings);

            ActivityDWWcfService ActivityDWWcfService = new ActivityDWWcfService();
            List<ActivitiyDW> listResponse = ActivityDWWcfService.GetActivitiesByUpdateDate(tenant, updateDate, skip, take, ref response);
            return listResponse;
        }


        [System.Web.Http.HttpPost]
        public int GetActivitiesCountByUpdateDate([FromBody] object[] t)//(int tenant, DateTime updateDate, ref Response response)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;

            int tenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            DateTime updateDate = JsonConvert.DeserializeObject<DateTime>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);
            Response response = JsonConvert.DeserializeObject<Response>(JsonConvert.SerializeObject(t[2]), jsonSerializerSettings);

            ActivityDWWcfService ActivityDWWcfService = new ActivityDWWcfService();
            int Response = ActivityDWWcfService.GetActivitiesCountByUpdateDate(tenant, updateDate, ref response);
            return Response;
        }
    }
}
