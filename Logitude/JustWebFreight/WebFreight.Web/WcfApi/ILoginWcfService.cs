using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Logitude.Server.Tools;
using WebFreight.Web.Helpers;

namespace WebFreight.Web.WcfApi
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ILoginWcfService" in both code and config file together.
    [ServiceContract]
    public interface ILoginWcfService
    {
        [OperationContract]
        Response Login(string email,string password);

        [OperationContract]
        Response LoginByCredential(string email,APICredentialsParameters apiCredentialsParam);

  
        [OperationContract]
        List<TenantInfo> GetUserTenants(string email, ref Response response);

        
    }
}
