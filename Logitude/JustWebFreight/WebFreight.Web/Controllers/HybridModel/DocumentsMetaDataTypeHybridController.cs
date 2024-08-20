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
    public class DocumentsMetaDataTypeHybridController : ApiController
    {
        [System.Web.Http.HttpPost]
        public Response Upsert([FromBody] object[] t)//(DocumentsMetaDataTypePM entityPM, bool batch)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;


            DocumentsMetaDataTypePM documentsMetaDataTypePM = JsonConvert.DeserializeObject<DocumentsMetaDataTypePM>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            bool batch = JsonConvert.DeserializeObject<bool>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);


            DocumentsMetaDataTypeWcfService DocumentsMetaDataTypeWcfService = new DocumentsMetaDataTypeWcfService();
            Response response = DocumentsMetaDataTypeWcfService.Upsert(documentsMetaDataTypePM, batch);
            return response;
        }
    }
}
