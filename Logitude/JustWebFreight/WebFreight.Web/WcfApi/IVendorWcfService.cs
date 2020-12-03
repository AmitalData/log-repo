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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IVendorWcfService" in both code and config file together.
    [ServiceContract]
    public interface IVendorWcfService
    {
        [OperationContract]
        Response Upsert(VendorPM entityPM, bool batch);

        [OperationContract]
        VendorPM GetVendorPM(string code, int tenant, ref Response response);
    }
}
