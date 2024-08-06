using Logitude.BL.CommonDataModel.EntityDws;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
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
    public class PackageTypeHybridController : ApiController
    {
        [System.Web.Http.HttpPost]
        public Response Upsert([FromBody] object[] t)//(PackageTypePM entityPM, bool batch)
        {

            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;


            PackageTypePM packageTypePM = JsonConvert.DeserializeObject<PackageTypePM>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            bool batch = JsonConvert.DeserializeObject<bool>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);


            PackageTypeWcfService PackageTypeWcfService = new PackageTypeWcfService();
            Response response = PackageTypeWcfService.Upsert(packageTypePM, batch);
            return response;
        }


        [System.Web.Http.HttpPost]
        public List<PackageTypeList> GetPackageTypeList([FromBody] object[] t)//(DataContracts.PackageTypeApiFilters filters, int tenant, ref Response response)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;

            DataContracts.PackageTypeApiFilters filters = JsonConvert.DeserializeObject<DataContracts.PackageTypeApiFilters>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            int tenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);
            Response response = JsonConvert.DeserializeObject<Response>(JsonConvert.SerializeObject(t[2]), jsonSerializerSettings);

            PackageTypeWcfService PackageTypeWcfService = new PackageTypeWcfService();
            List<PackageTypeList> listResponse = PackageTypeWcfService.GetPackageTypeList(filters,tenant, ref response);
            return listResponse;
        }
    }
}
