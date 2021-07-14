using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OutlookConnection.Common.Contracts
{
    public interface IContactRepo
    {
        ContactWcfServiceReference.ContactPM GetContactPMByEmail(string email);
        OutlookConnection.Common.ContactWcfServiceReference.ContactList[] GetContactList(OutlookConnection.Common.ContactWcfServiceReference.ContactApiFilters filters, int tenant, ref OutlookConnection.Common.ContactWcfServiceReference.Response response);
    }
}
