using Logitude.Accounting.BL.CoreBL;
using Logitude.Accounting.BL.DataContract;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Infrastructure.BL.EntityPMs;
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
using System.Text.Json;

namespace WebFreight.Web.Controllers.AccountingModel
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


        public HttpResponseMessage GetTaxDeductionReportData(int tenant, string taxDeductionReport)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnTenant(tenant);

                IAccountingContext MyContext = AccountingContext.GetContext(authToken.Tenant);
                TaxDeductionReportQueryService taxDeductionReportQuery = new TaxDeductionReportQueryService(MyContext);
                taxDeductionReportQuery.InitializeSettings();
                TaxDeductionReportPM taxDeductionReportPM = taxDeductionReportQuery.GetSingle(taxDeductionReport, true, false);
                if (taxDeductionReportPM == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Tax Deduction Report not found.");
                }
                TaxDeductionReportData taxDeductionReportData = new TaxDeductionReportData();
                if (!String.IsNullOrEmpty(taxDeductionReportPM.ReportSavedData))
                {
                    taxDeductionReportData = JsonSerializer.Deserialize<TaxDeductionReportData>(taxDeductionReportPM.ReportSavedData);
                }
                return Request.CreateResponse(HttpStatusCode.OK, taxDeductionReportData);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

    }
}