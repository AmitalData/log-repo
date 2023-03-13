//#define tzuri_req
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Logitude.Server.Tools;

namespace WebFreight.Web.WcfApi
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IExternalTasksQueueWcfService" in both code and config file together.
    [ServiceContract]
    public interface IExternalTasksQueueWcfService
    {
        [OperationContract]
        string GetTaskFromQueue(int tenant, int priority);
        [OperationContract]
        Response MarkTaskAsDone(string communicationLogId, int tenant, int priority);

        [OperationContract]
        Response LGTQuery(string queryId, Dictionary<string, string> queryParams, int tenant);

#if tzuri_req

        [OperationContract]
        string GetTaskByQueueDefinitionCode(int tenant, int priority,string queueDefinitionCode);
        [OperationContract]
        Response MarkTaskAsDoneByQueueDefinitionCode(string communicationLogId, int tenant, int priority, string queueDefinitionCode);
#endif
    }
}
