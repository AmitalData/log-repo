using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools;
using Newtonsoft.Json;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebFreight.Web.WcfApi;

namespace WebFreight.Web.Controllers.HybridModel
{
    public class DocumentInHybridController : ApiController
    {
        [System.Web.Http.HttpPost]
        public Response Upsert([FromBody] object[] t)//(DocumentsFilingPM documentDataPM, bool batch)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;


            DocumentsFilingPM documentsFilingPM = JsonConvert.DeserializeObject<DocumentsFilingPM>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            bool batch = JsonConvert.DeserializeObject<bool>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);


            DocumentInWcfService DocumentInWcfService = new DocumentInWcfService();
            Response response = DocumentInWcfService.Upsert(documentsFilingPM, batch);
            return response;
        }

        //ב - DocumentInWcfService הפונקצייה UpsertDocumentData היתה כפולה, ולכן תוקן השם כאן 
        //השם החדש: UpsertDocumentDataFilingPM
        //todo:orit- ברגע שממרים את הפיתוח של WCF CLIENT יש לפנות ל2 פונקציות נפרדות
        [System.Web.Http.HttpPost]
        public Response UpsertDocumentDataFilingPM([FromBody] object[] t)//(DocumentsFilingPM entityPM)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;

            DocumentsFilingPM documentsFilingPM = JsonConvert.DeserializeObject<DocumentsFilingPM>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);

            DocumentInWcfService DocumentInWcfService = new DocumentInWcfService();
            Response Response = DocumentInWcfService.UpsertDocumentData(documentsFilingPM);
            return Response;
        }

        [System.Web.Http.HttpPost]
        public DocumentsFilingPM GetDocumentDataByExternalId([FromBody] object[] t)//(string externalId, int tenant, ref Response response)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;


            string externalId = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            int tenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);
            Response response = JsonConvert.DeserializeObject<Response>(JsonConvert.SerializeObject(t[2]), jsonSerializerSettings);

            DocumentInWcfService DocumentInWcfService = new DocumentInWcfService();
            DocumentsFilingPM Response = DocumentInWcfService.GetDocumentDataByExternalId(externalId, tenant, ref response);
            return Response;
        }


        [System.Web.Http.HttpPost]
        public Response UpsertDocumentData([FromBody] object[] t)//(DocumentDataPM documentDataPM, bool batch)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;


            DocumentDataPM documentDataPM = JsonConvert.DeserializeObject<DocumentDataPM>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            bool batch = JsonConvert.DeserializeObject<bool>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);


            DocumentInWcfService DocumentInWcfService = new DocumentInWcfService();
            Response response = DocumentInWcfService.UpsertDocumentData(documentDataPM, batch);
            return response;
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


            DocumentInWcfService DocumentInWcfService = new DocumentInWcfService();
            Response response = DocumentInWcfService.UploadDocumentFileData(buffer, fileSize, sentBytes, blockIdsList, bufferNumber, tenant, FileNameWithExtention, DocumentId);
            return response;
        }

        [System.Web.Http.HttpPost]
        public DocumentsFilingPM GetDocumentDataByExternalIdWithoutBinaray([FromBody] object[] t)//(string externalId, int tenant, ref Response response)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;

            string externalId = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            int tenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);
            Response response = JsonConvert.DeserializeObject<Response>(JsonConvert.SerializeObject(t[2]), jsonSerializerSettings);

            DocumentInWcfService DocumentInWcfService = new DocumentInWcfService();
            DocumentsFilingPM Response = DocumentInWcfService.GetDocumentDataByExternalId(externalId, tenant, ref response);
            return Response;
        }
    }
}
