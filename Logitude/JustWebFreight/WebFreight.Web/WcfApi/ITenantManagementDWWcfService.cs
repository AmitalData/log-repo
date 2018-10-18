using Logitude.BL.GlobalModel.EntityDws;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WebFreight.Web.WcfApi
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ICustomerAdditionalServiceDWWcfService" in both code and config file together.
    [ServiceContract]
    public interface ITenantManagementDWWcfService
    {
        [OperationContract]
        List<TenantManagementDW> GetTenantManagements(int tenant, int skip, int take , ref Response response);

    }
}
