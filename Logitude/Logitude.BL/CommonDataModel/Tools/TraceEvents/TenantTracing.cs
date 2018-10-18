using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Security;

namespace Logitude.BL.CommonDataModel.Tools.TraceEvents
{
    public class TenantTracing
    {
        public static void Trace(TenantPM entityPM, Tenant poco, bool isNewEntity)
        {
            //WebFreightDomainService domainService = new WebFreightDomainService();
            //ContactPM loggedContact = new ContactQuery(entityPM.Id).GetContactByEmailOnly(SecurityUtility.GetAuthenticatedUser(), entityPM.Id);

            //if (isNewEntity)
            //{

            //}

            //else
            //{

            //}
        }
    }
}