using Logitude.BL.CommonDataModel.EntityDws;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WebFreight.Web.WcfApi
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ICustomersServiceDW" in both code and config file together.
    [ServiceContract]
    public interface ICustomerDWWcfService
    {
        [OperationContract]
        List<CustomerDW> GetCustomers(int tenant, int skip, int take, ref Response response);

        [OperationContract]
        int GetCustomersCount(int tenant, ref Response response);



        [OperationContract]
        List<CustomerDW> GetCustomersByUpdateDate(int tenant, DateTime updateDate, int skip, int take, ref Response response);

        [OperationContract]
        int GetCustomersCountByUpdateDate(int tenant, DateTime updateDate, ref Response response);
    }   
}
