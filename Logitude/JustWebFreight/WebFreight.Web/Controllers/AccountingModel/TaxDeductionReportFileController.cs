using Logitude.Accounting.BL.CoreBL;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Infrastructure.BL.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
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

namespace WebFreight.Web.App_Code.AngularJS_App_Code.Generated
{
    public class TaxDeductionReportFileController: ApiController
    {


        public HttpResponseMessage PostDownloadTaxDeduction856FileInBatch(TaxDeductionReportPM entityPM)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnEntityTenant("TaxDeductionReport", entityPM.Tenant, authToken.Tenant);
                SecurityUtility.CheckContactFeature("TaxDeductionReport", "NEW", authToken.Tenant);
                int tenant = authToken.Tenant;

                BatchTaskExecutionPM btePM = TaxDeductionReportService.Create856FileInBatch(entityPM.Id, tenant);


                return Request.CreateResponse(HttpStatusCode.OK, btePM);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }



    }
}