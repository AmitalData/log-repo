using AmitalCloud.Infrastructure.Application.EntityQueryServices;
using AmitalCloud.Infrastructure.Data;
using AmitalCloud.Infrastructure.Web.Helpers;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace AmitalCloud.Infrastructure.Web.Controllers
{
    public class AmitalCloudController : ApiController
    {
        public HttpResponseMessage GetCheckIsupgradingSystem()
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, GetIsBlockingFromDB());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, AmitalCloudApiExceptionBuilder.BuildException(ex));
            }

        }
        private bool GetIsBlockingFromDB()
        {

            var res = new TenantManagementQueryService(0).GetMulti(a => true, "RecurringPeriod,PaymentMethod,PaymentChannel");


            var result = new GlobalDBQueryService(0).GetMulti(a => true); // a.IsBlocking == true); //,a=>new GlobalDBPM() {Id = a.Id });
            bool isBlocking = result.Count > 0;
            string[] authenticatedIPs = AmitalCloudSettings.CustomerCareIP?.Split(',');
            string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
            if (string.IsNullOrEmpty(currentIP))
            {
                currentIP = HttpContext.Current.Request.UserHostAddress;
            }
            bool isIpAuthenticated = authenticatedIPs.Contains(currentIP);
            if (isIpAuthenticated)
            {
                isBlocking = false;
            }
            return isBlocking;
        }



    }
}