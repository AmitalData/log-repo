using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WebFreight.Web.WcfApi
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ITotangoWcfService" in both code and config file together.
    [ServiceContract]
    public interface ITotangoWcfService
    {
        [OperationContract]
        void SendUserActivity(string Email, string orgDisplayName, string module, string activity, int tenant);
    }
}
