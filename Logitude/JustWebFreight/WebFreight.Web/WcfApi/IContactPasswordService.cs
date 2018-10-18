using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WebFreight.Web.WcfApi
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IContactPasswordService" in both code and config file together.
    [ServiceContract]
    public interface IContactPasswordService
    {
        [OperationContract]
        Response ChangeContactPassword(string email, string oldPassword, string newPassword);
    }
}
