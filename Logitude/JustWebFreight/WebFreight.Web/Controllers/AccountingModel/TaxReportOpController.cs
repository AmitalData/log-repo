using System;
using WebFreight.Web.Security;
using WebFreight.Web.Helpers;
using System.Web;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Net;
using System.Net.Http;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Accounting.BL.CoreBL;
using System.Web.Http;
using Logitude.Infrastructure.BL.EntityPMs;

namespace WebFreight.Web.Controllers.AccountingModel
{


    public class TaxReportOpController : ApiController
    {
        public HttpResponseMessage PostDownloadPNC874File(TaxReportPM entityPM)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("TaxReport", "NEW", authToken.Tenant);
                int tenant = authToken.Tenant;

                DocumentsFilingPM docOut = TaxReportService.CreatePNC874File(entityPM.Id, tenant);


                return Request.CreateResponse(HttpStatusCode.OK, docOut);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage PostDownloadPNC874FileInBatch(TaxReportPM entityPM)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("TaxReport", "NEW", authToken.Tenant);
                int tenant = authToken.Tenant;

                BatchTaskExecutionPM btePM = TaxReportService.CreatePNCFileInBatch(entityPM.Id, tenant);


                return Request.CreateResponse(HttpStatusCode.OK, btePM);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
    }
}
	 