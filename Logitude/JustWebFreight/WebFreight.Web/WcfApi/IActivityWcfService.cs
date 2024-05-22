using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Logitude.CRM.BL.EntityPMs;
using Logitude.Server.Tools;

namespace WebFreight.Web.WcfApi
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IActivityWcfService" in both code and config file together.
    [ServiceContract]
    public interface IActivityWcfService
    {

        [OperationContract]
        Response isOnline();

        [OperationContract]
        Response Upsert(ActivityPM entityPM, string email);

        [OperationContract]
        List<ActivityPM> GetActivities(string email, int tenant, ref Response response);

        [OperationContract]
        Response UpdateOutlookID(string activityId, string outlookId,int tenant);

        [OperationContract]
        Response SetAsSynchronized(string activityId,string email,int tenant);

        [OperationContract]
        Response Delete(string activityId, int tenant);

        [OperationContract]
        ActivityPM GetActivityPM(string id, int tenant, ref Response response);

        [OperationContract]
        Response UploadDocumentFileData(int tenant, string blobname, string DocumentId);

        [OperationContract]
        Response GetStorageContainerConnectionString(int tenant);

        [OperationContract]
        Response UploadDocumentFileData(byte[] buffer, long fileSize, long sentBytes, string[] blockIdsList, int bufferNumber, int tenant, string FileNameWithExtention, string DocumentId);
    }
}
