using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Simplog.Data.CommonDataModel.Repositories;
using System.Web;
using System.Web.Http;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.DigitalPortal
{
    public class DigitalTotangoController : ApiController
    {
        [HttpPost]
        [Route("DigitalTotangoController/Post")]
        public void Post(TotangoActivityInfo activityInfo)
        {
            var authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(HttpContext.Current.Request.Headers["Token"]);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            SecurityUtility.CheckDigitalUserAuthentication(authToken.Tenant, activityInfo.CardId);

            var myTenant = GetTenant(authToken.Tenant);
            activityInfo.IsSharedLogisticsContact = true;
            activityInfo.OrganizationId = authToken.Tenant.ToString();
            activityInfo.OrgDisplayName = myTenant.Company + (myTenant.CountryName != null ? ("-" + myTenant.CountryName.Trim()) : "");
            activityInfo.Tenant = authToken.Tenant;
            ActivityLog.AddContactActivityWithTotango(activityInfo.OrganizationId, activityInfo.OrgDisplayName, activityInfo.UserName, activityInfo.Module,
               activityInfo.Activity, activityInfo.ContactId, activityInfo.Tenant, activityInfo.IsSharedLogisticsContact, activityInfo.CardId, activityInfo.PartnerTypeId, null);
        }

        private TenantPM GetTenant(int tenant)
        {
            var tenantQuery = new TenantQuery(tenant);
            var tenantPM = tenantQuery.GetSinglePM(tenant);

            return tenantPM;
        }
    }
}