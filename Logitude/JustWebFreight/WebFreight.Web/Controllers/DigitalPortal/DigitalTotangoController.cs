using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using System.Web.Http;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;

namespace WebFreight.Web.Controllers.DigitalPortal
{
    public class DigitalTotangoController : ApiController
    {
        [HttpPost]
        [Route("DigitalTotangoController/Post")]
        public void Post(TotangoActivityInfo activityInfo)
        {
            var myTenant = GetTenant(activityInfo.Tenant);
            activityInfo.IsSharedLogisticsContact = true;
            activityInfo.OrganizationId = activityInfo.Tenant.ToString();
            activityInfo.OrgDisplayName = myTenant.Company + (myTenant.CountryName != null ? ("-" + myTenant.CountryName.Trim()) : "");
            ActivityLog.AddContactActivityWithTotango(activityInfo.OrganizationId, activityInfo.OrgDisplayName, activityInfo.UserName, activityInfo.Module,
               activityInfo.Activity, activityInfo.ContactId, activityInfo.Tenant, activityInfo.IsSharedLogisticsContact, activityInfo.CardId, activityInfo.PartnerTypeId, activityInfo.Via);
        }

        private TenantPM GetTenant(int tenant)
        {
            var tenantQuery = new TenantQuery(tenant);
            var tenantPM = tenantQuery.GetSinglePM(tenant);

            return tenantPM;
        }
    }
}