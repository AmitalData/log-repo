using Logitude.Server.Tools;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Logitude.BL.CommonDataModel.EntityPMs;
using WebFreight.Web.DataContracts;
using Logitude.Server.Tools;

namespace WebFreight.Web.WcfApi
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IDocumentInWcfService" in both code and config file together.
    [ServiceContract]
    public interface IDocumentInWcfService
    {
        [OperationContract]
        Response Upsert(DocumentsFilingPM documentDataPM, bool batch);

        [OperationContract]
        DocumentsFilingPM GetDocumentDataByExternalId(string externalId, int tenant, ref Response response);

        [OperationContract]
        Response UpsertDocumentData(DocumentDataPM documentDataPM, bool batch);

        [OperationContract]
        Response UploadDocumentFileData(int tenant, string blobname, string DocumentId);
        
        [OperationContract]
        Response GetStorageContainerConnectionString(int tenant);

        [OperationContract]
        Response UploadDocumentFileData(byte[] buffer, long fileSize, long sentBytes, string[] blockIdsList, int bufferNumber, int tenant, string FileNameWithExtention, string DocumentId);
        
        [OperationContract]
        DocumentsFilingPM GetDocumentDataByExternalIdWithoutBinaray(string externalId, int tenant, ref Response response);
    }
}
