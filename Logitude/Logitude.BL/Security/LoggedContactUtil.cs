using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Interfaces;
using Logitude.Server.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Logitude.BL.Security
{
    public class LoggedContactUtil : ILoggedContactUtil
    {
        public ContactPM GetLoggedContact(int tenant)
        {
            ContactPM loggedContact=null;
            try
            {
                if (HttpContext.Current != null)
                {
                    loggedContact = new ContactQuery(tenant).GetContactByEmailOnly(AuthenticationUtil.ResolveUserIdentityName(tenant), tenant);


                    // add by Abdullah & Mohammad to fix customer care contact issue, BUG 48520
                    // if (null and tenant <> 0) - customer care
                    //      get it by tenant 0 
                    if (tenant != 0 && loggedContact == null)
                    {
                        loggedContact = new ContactQuery(tenant).GetContactByEmailOnly(AuthenticationUtil.ResolveUserIdentityName(tenant), 0);
                    }
                    // note: the user will be found by method (GetContactByEmailOnly) in the cache 

                }
                else
                {
                    loggedContact = new ContactQuery(tenant).GetSingleByEmail("system@tenant" + tenant + ".com", tenant);
                }
            }
            catch { }
            

            loggedContact = loggedContact ?? new ContactPM() { DontShowLocal = true };
            return loggedContact;
        }
    }
}
