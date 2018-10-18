using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Logitude.BL.ShipmentsModel.EntityPMs;

namespace WebFreight.Web.WcfApi
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ISpecialServicesTypeWcfService" in both code and config file together.
    [ServiceContract]
    public interface ISpecialServicesTypeWcfService
    {
        [OperationContract]
        Response Upsert(SpecialServicesTypePM entityPM, bool batch);
    }
}
