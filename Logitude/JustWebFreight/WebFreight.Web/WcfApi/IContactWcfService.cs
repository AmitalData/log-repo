using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;

namespace WebFreight.Web.WcfApi
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IContactWcfService" in both code and config file together.
    [ServiceContract]
    public interface IContactWcfService
    {
        [OperationContract]
        Response Upsert(ContactPM entityPM, bool batch);

        [OperationContract]
        ContactPM GetContactPMByEmail(string email, int tenant, ref Response response);

        [OperationContract]
        ContactPM GetContactByExternalId(string externalId, int tenant, ref Response response);

        [OperationContract]
        List<ContactList> GetContactList(DataContracts.ContactApiFilters filters, int tenant, ref Response response);
    }
}
