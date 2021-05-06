using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OutlookConnection.Common.Contracts
{
    interface ILoginRepo
    {
        LoginWcfServiceReference.Response Login(string email, string password);
        LoginWcfServiceReference.TenantInfo[] GetUserTenants(string email, ref OutlookConnection.Common.LoginWcfServiceReference.Response response);
    }
}
