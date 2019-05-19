using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Net.Http;
using Logitude.Accounting.BL.Utils;
using System.Net;
using WebFreight.Web.Helpers;

namespace WebFreight.Web.Controllers.AccountingModel
{
    //[RoutePrefix("api/ReconciliationAfterConversion")]
    public class ReconciliationAfterConversionController : ApiController
    {
        public ReconciliationAfterConversionController()
        {

        }

        public HttpResponseMessage GetReconciliationAfterConversion(int tenant)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                //AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                //SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                //SecurityUtility.CheckContactFeature("TaxDeductionReport", "NEW", authToken.Tenant);

                ReconciliationAfterConversionBatch reconciliationAfterConversionBatch = new ReconciliationAfterConversionBatch();
                reconciliationAfterConversionBatch.RunReconciliationAfterConversion(tenant);
                string responseText = reconciliationAfterConversionBatch.ResponseText();
                HttpStatusCode StatusCode = reconciliationAfterConversionBatch.StatusCode();
                var res1 = new { Success = true, Message = responseText };

                return Request.CreateResponse(StatusCode, res1);

            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

    }
}