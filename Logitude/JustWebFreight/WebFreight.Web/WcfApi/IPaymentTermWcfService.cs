using Intuit.Ipp.Core.Configuration;
using Logitude.BL.CommonDataModel.EntityLists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WebFreight.Web.WcfApi
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IPaymentTermWcfService" in both code and config file together.
    [ServiceContract]
    public interface IPaymentTermWcfService
    {

        [OperationContract]
        List<PaymentTermList> GetIncoterms(ref Response response, int tenant);
    }
}
