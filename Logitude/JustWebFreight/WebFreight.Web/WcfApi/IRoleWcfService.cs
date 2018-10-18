using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WebFreight.Web.WcfApi
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IRoleWcfService" in both code and config file together.
    [ServiceContract]
    public interface IRoleWcfService
    {
        [OperationContract]
        List<RoleList> GetRoles(int tenant, ref Response response);
    }
}
