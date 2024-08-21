using Logitude.Server.Tools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebFreight.Web.WcfApi;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using WebFreight.Web.Controllers.CustomsModel.Extended;

namespace WebFreight.Web.Controllers.HybridModel
{
    public class DocumentTypeHybridController : ApiController
    {
        [System.Web.Http.HttpPost]
        public Response Upsert([FromBody] object[] t)//(DocumentTypePM entityPM, bool batch)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;


            DocumentTypePM documentTypePM = JsonConvert.DeserializeObject<DocumentTypePM>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            bool batch = JsonConvert.DeserializeObject<bool>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);


            DocumentTypeWcfService DocumentTypeWcfService = new DocumentTypeWcfService();
            Response response = DocumentTypeWcfService.Upsert(documentTypePM, batch);
            return response;
        }


        [System.Web.Http.HttpPost]
        public List<DocumentTypeList> GetDocumentTypes([FromBody] object[] t)//(string objectTableName, int tenant, int skip, int take, ref Response response)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;

            string objectTableName = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            int tenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);
            int skip = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[2]), jsonSerializerSettings);
            int take = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[3]), jsonSerializerSettings);
            Response response = JsonConvert.DeserializeObject<Response>(JsonConvert.SerializeObject(t[4]), jsonSerializerSettings);

            DocumentTypeWcfService DocumentTypeWcfService = new DocumentTypeWcfService();
            List<DocumentTypeList> listResponse = DocumentTypeWcfService.GetDocumentTypes(objectTableName, tenant, skip, take, ref response);
            return listResponse;
        }


        [System.Web.Http.HttpPost]
        public DocumentTypePM GetDocumentTypeByCode([FromBody] object[] t)//(string code, int tenant, ref Response response)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;

            string code = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            int tenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);
            Response response = JsonConvert.DeserializeObject<Response>(JsonConvert.SerializeObject(t[2]), jsonSerializerSettings);

            DocumentTypeWcfService DocumentTypeWcfService = new DocumentTypeWcfService();
            DocumentTypePM Response = DocumentTypeWcfService.GetDocumentTypeByCode(code, tenant, ref response);
            return Response;
        }
    }
}
