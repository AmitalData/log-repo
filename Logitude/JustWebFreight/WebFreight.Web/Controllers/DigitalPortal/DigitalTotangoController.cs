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

            var isSharedLogisticsContact = true;
            ActivityLog.AddContactActivityLog(activityInfo.CardId, activityInfo.PartnerTypeId, activityInfo.ContactId,
                                              activityInfo.Module, activityInfo.Activity, authToken.Tenant,
                                              isSharedLogisticsContact, null);
        }
    }
}