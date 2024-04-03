using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using System;
using System.Web.Http;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;

namespace WebFreight.Web.Controllers.DigitalPortal
{
    public class DigitalTotangoController : ApiController
    {
        [HttpPost]
        [Route("DigitalTotangoController/Post")]
        public IHttpActionResult Post(TotangoActivityInfo activityInfo)
        {
            try
            {
                var myTenant = GetTenant(activityInfo.Tenant);
                activityInfo.IsSharedLogisticsContact = true;
                activityInfo.OrganizationId = activityInfo.Tenant.ToString();
                activityInfo.OrgDisplayName = myTenant.Company + (myTenant.CountryName != null ? ("-" + myTenant.CountryName.Trim()) : "");
                ActivityLog.AddContactActivityWithTotango(activityInfo.OrganizationId, activityInfo.OrgDisplayName, activityInfo.UserName, activityInfo.Module,
                   activityInfo.Activity, activityInfo.ContactId, activityInfo.Tenant, activityInfo.IsSharedLogisticsContact, activityInfo.CardId, activityInfo.PartnerTypeId, activityInfo.Via);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ApiExceptionBuilder.BuildException(ex).ErrorMessage);
            }
        }

        private TenantPM GetTenant(int tenant)
        {
            var tenantQuery = new TenantQuery(tenant);
            var tenantPM = tenantQuery.GetSinglePM(tenant);

            return tenantPM;
        }
    }
}