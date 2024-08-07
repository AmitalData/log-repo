using Logitude.BL.InfrastructureModel.EntityPMs;
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
    public class BusinessHoursHolidaHybridController : ApiController
    {
        [System.Web.Http.HttpPost]
        public Response Upsert([FromBody] object[] t)//(BusinessHoursHolidayPM entityPM, bool batch)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;


            BusinessHoursHolidayPM businessHoursHolidayPM = JsonConvert.DeserializeObject<BusinessHoursHolidayPM>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            bool batch = JsonConvert.DeserializeObject<bool>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);


            BusinessHoursHolidayWcfService BusinessHoursHolidayWcfService = new BusinessHoursHolidayWcfService();
            Response response = BusinessHoursHolidayWcfService.Upsert(businessHoursHolidayPM, batch);
            return response;
        }
    }
}
