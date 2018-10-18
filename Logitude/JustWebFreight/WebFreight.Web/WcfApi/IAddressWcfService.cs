using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Logitude.BL.CommonDataModel.EntityPMs;

namespace WebFreight.Web.WcfApi
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IAddressWcfService" in both code and config file together.
    [ServiceContract]
    public interface IAddressWcfService
    {
        [OperationContract]
        Response Upsert(AddressPM entityPM, bool batch);

        [OperationContract]
        AddressPM GetAddressByExternalId(string externalId, int tenant, ref Response response);
    }
}
