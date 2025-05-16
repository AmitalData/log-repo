using System.Net.Http;
using System.Net;
using System;
using System.Web.Http;
using AmitalCloud.Infrastructure.Data.Security;
using AmitalCloud.Infrastructure.Web.Helpers;
using AmitalCloud.Infrastructure.Application.EntityQueryServices;
using System.Linq;
using AmitalCloud.Infrastructure.Application.Helpers;

namespace AmitalCloud.Infrastructure.Web.Controllers
{
    public class UserExtendedController : ApiController
    {
        public HttpResponseMessage GetCheckUserReleaseNotesToolTip(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "User ID cannot be null or empty.");
            }

            try
            {
                int tenant = AmitalCloudSecurityUtility.AuthenticateTenant();
                var usersReleaseNotesDisplayPM = new UsersReleaseNotesDisplayQueryService(tenant)
                    .GetMulti(a => a.UserId == userId && a.Tenant == tenant)
                    .FirstOrDefault();

                return Request.CreateResponse(HttpStatusCode.OK, usersReleaseNotesDisplayPM == null);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, AmitalCloudApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}
