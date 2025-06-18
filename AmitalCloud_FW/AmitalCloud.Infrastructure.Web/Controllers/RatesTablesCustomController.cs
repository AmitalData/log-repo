using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AmitalCloud.Infrastructure.Application.Helpers;
using AmitalCloud.Infrastructure.Web.Helpers;
using AmitalCloud.Infrastructure.Data.Queries;

namespace AmitalCloud.Infrastructure.Web.Controllers
{
    public class RatesTablesCustomController : ApiController
    {
        public HttpResponseMessage GetLastUpdateByCurrencyCode(string foreignCurrency, string tenantCurrencyId)
        {
            try
            {
                int tenant = AmitalCloudSecurityUtility.AuthenticateTenant();
                var ratesTable = new RatesTableQuery(tenant).GetLastUpdateByCurrencyCode(foreignCurrency, tenantCurrencyId);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, AmitalCloudApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}