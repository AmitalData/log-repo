using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using WebFreight.Web.CustomWebServices.SignChunks.Common;

namespace WebFreight.Web.WcfApi
{
    [ServiceContract]
    public interface IExternalTasksQueueExportSignWcfService
    {

        //[OperationContract]
        //void ExportSignServers(int tenant, List<SignServer> signServers);
        
        [OperationContract]
        ResponseExportSignTask GetExportSignTaskFromQueue(ExportReqSignData exportReqSignData);
        [OperationContract]
        Logitude.Server.Tools.Response MarkExportSignTaskAsFail(int tenant, String queueId, String customsRequestsSheetId, string interfaceTypeCode, string ErrorMessage);

        [OperationContract]
        Logitude.Server.Tools.Response MarkExportSignTaskAsDone(int tenant, String queueId, String customsRequestsSheetId, string interfaceTypeCode, Byte[] signBytes);

    }

   
}
