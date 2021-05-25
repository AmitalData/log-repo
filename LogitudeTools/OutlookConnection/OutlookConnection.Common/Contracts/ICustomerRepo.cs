using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OutlookConnection.Common.Contracts
{
    public interface ICustomerRepo
    {
        CustomerWcfServiceReference.CustomerList[] CustomersByContact(string email, string search);
        CustomerWcfServiceReference.CustomerList[] CustomersByContact(string email, string search, bool myOnly, int take);
        CustomerWcfServiceReference.CustomerList CustomersByID(string Id);
    }
}
