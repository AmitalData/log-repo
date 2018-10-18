using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;

using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.CommonDataModel.Extended
{
    public class TwoFactorAuthenticationDevicesExtendedController : ApiController
    {
        public HttpResponseMessage GetDevicesByUser(string userId)
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                SecurityUtility.CheckContactFeature("TwoFactorAuthenticationDevice", "READ", authToken.Tenant);
                TwoFactorAuthenticationDeviceQuery twoFactorAuthenticationDeviceQuery = new TwoFactorAuthenticationDeviceQuery(authToken.Tenant);
                List<TwoFactorAuthenticationDevicePM> twoFactorAuthenticationDevicePMs = twoFactorAuthenticationDeviceQuery.GetVerifiedTwoFactorAuthenticationDevicePMsByUserId(userId, authToken.Tenant).ToList();

                PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                return Request.CreateResponse(HttpStatusCode.OK, twoFactorAuthenticationDevicePMs);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

    }
}