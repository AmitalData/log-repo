using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools;
namespace WebFreight.Web.WcfApi
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ICustomAgentHypredService" in both code and config file together.
    [ServiceContract]
    public interface ICustomAgentWcfService
    {
        [OperationContract]
        Response Upsert(CustomAgentPM entityPM,bool batch);
    }
}
