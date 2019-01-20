using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Interfaces;
using Logitude.Server.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.Security
{
    public class LoggedContactUtil : ILoggedContactUtil
    {
        public ContactPM GetLoggedContact(int tenant)
        {
            ContactPM loggedContact = new ContactQuery(tenant).GetContactByEmailOnly(
               //SecurityUtility.GetAuthenticatedUser()
               AuthenticationUtil.ResolveUserIdentityName(tenant)
               , tenant);
            if (loggedContact == null)
            {
                loggedContact = new ContactQuery(tenant).GetContactByEmailOnly("system@tenant" + tenant + ".com", tenant);
            }
            loggedContact = loggedContact ?? new Logitude.BL.CommonDataModel.EntityPMs.ContactPM() { DontShowLocal = true };
            return loggedContact;
        }
    }
}
