using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Net.Http;
using Logitude.Accounting.BL.Utils;
using System.Net;
using WebFreight.Web.Helpers;
using System.Text.RegularExpressions;

namespace WebFreight.Web.Controllers.AccountingModel
{
    //[RoutePrefix("api/ReconciliationStageB")]
    public class ReconciliationStageBController : ApiController
    {
        public ReconciliationStageBController()
        {

        }

        public HttpResponseMessage GetReconciliationStageB(int tenant)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];

                ReconciliationStageBBatch reconciliationStageBBatch = new ReconciliationStageBBatch();
                reconciliationStageBBatch.RunReconciliationStageB(tenant);
                string responseText = reconciliationStageBBatch.ResponseText();
                HttpStatusCode StatusCode = reconciliationStageBBatch.StatusCode();
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