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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ICustomerHypredService" in both code and config file together.
    [ServiceContract]
    public interface ICustomerWcfService
    {
        [OperationContract]
        Response Upsert(CustomerPM entityPM,bool batch);

        [OperationContract]
        List<CustomerList> GetCustomerList(string searchText, string email, bool myCustomer, int tenant, int skip, int take, ref Response response);

        [OperationContract]
        List<CustomerList> GetCustomerListByEmail(string email, int tenant, ref Response response);

        [OperationContract]
        CustomerList GetCustomerListById(string id, int tenant, ref Response response);

        [OperationContract]
        CustomerPM GetCustomerPM(CustomerApiFilters filters, int tenant, ref Response response);

        [OperationContract]
        List<AddressPM> GetCustomerAddresses(CustomerApiFilters filters, int tenant, ref Response response);

        [OperationContract]
        List<ContactPM> GetCustomerContacts(CustomerApiFilters filters, int tenant, ref Response response);

        [OperationContract]
        CustomerPM GetReadyForActivationCustomer(int tenant, ref Response response);

        [OperationContract]
        Response RemoveFromCustomersQueue(Guid queueMessageLockToken, int tenant);

        //[OperationContract]
        //byte[] GetQuestionnaireAnswers(string questionnaireId, int tenant, string tableId, string entityId, ref Response response);

         

    }
}
