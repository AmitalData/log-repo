using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WebFreight.Web.WcfApi
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IOceanInsightsWcfService" in both code and config file together.
    [ServiceContract]
    public interface IOceanInsightsV2WcfService
    {
        [OperationContract]
        Response Insert(int Tenant, string ScacCode, string ContainerNo,string Type,string System = null);

        [OperationContract]
        Response GetStatus(string RequestId, string Type);
        
    }
}
