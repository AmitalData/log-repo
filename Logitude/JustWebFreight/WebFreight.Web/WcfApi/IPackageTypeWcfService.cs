using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using WebFreight.Web.DataContracts;

namespace WebFreight.Web.WcfApi
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IPackageTypeWcfService" in both code and config file together.
    [ServiceContract]
    public interface IPackageTypeWcfService
    {
        [OperationContract]
        Response Upsert(PackageTypePM entityPM, bool batch);

        [OperationContract]
        List<PackageTypeList> GetPackageTypeList(PackageTypeApiFilters filters, int tenant, ref Response response);
    }
}
