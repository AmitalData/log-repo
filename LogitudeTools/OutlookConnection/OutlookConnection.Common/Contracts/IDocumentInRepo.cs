using OutlookConnection.Common.DocumentInWcfServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutlookConnection.Common.Contracts
{
    public interface IDocumentInRepo
    {

        DocumentInWcfServiceReference.Response UpsertDocumentData(OutlookConnection.Common.DocumentInWcfServiceReference.DocumentDataPM documentDataPM, bool batch);

         Task<Response> UploadDocumentDataAysnc(byte[] currentData, long fileSize, long sentBytes, string[] blockIdsList, int bufferNumber, int tenant, string FileNameWithExtention, string DocumentId);
        //DocumentInWcfServiceReference.DocumentDataPM GetDocumentDataByExternalId(string externalId, int tenant, ref OutlookConnection.Common.DocumentInWcfServiceReference.Response response);

    }
}
