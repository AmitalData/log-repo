using Logitude.Accounting.BL.Validators;
using Logitude.Server.Tools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebFreight.Web.DataContracts;
using WebFreight.Web.WcfApi;

namespace WebFreight.Web.Controllers.HybridModel
{
    public class FeatureHybridController : ApiController
    {
        [System.Web.Http.HttpPost]
        public List<FeatureAccessInfo> GetActiveFeaturesForUser([FromBody] object[] t)//(List<FeatureAccessInfo> featuresList, int tenant, ref Response response)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;

            List<FeatureAccessInfo> featuresList = JsonConvert.DeserializeObject<List<FeatureAccessInfo>>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            int tenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);
            Response response = JsonConvert.DeserializeObject<Response>(JsonConvert.SerializeObject(t[2]), jsonSerializerSettings);

            FeatureWcfService FeatureWcfService = new FeatureWcfService();
            List<FeatureAccessInfo> listResponse = FeatureWcfService.GetActiveFeaturesForUser(featuresList, tenant, ref response);
            return listResponse;
        }

        [System.Web.Http.HttpPost]
        public bool CheckOutlookVersion([FromBody] object[] t)//(string Version)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;

            string Version = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            
            FeatureWcfService FeatureWcfService = new FeatureWcfService();
            bool Response = FeatureWcfService.CheckOutlookVersion(Version);
            return Response;
        }
    }
}
