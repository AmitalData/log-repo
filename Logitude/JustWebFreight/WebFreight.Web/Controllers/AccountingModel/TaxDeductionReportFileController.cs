using Logitude.Accounting.BL.CloseTables;
using Logitude.Accounting.BL.CoreBL;
using Logitude.Accounting.BL.DataContract;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Infrastructure.BL.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
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

        [HttpGet]
        [Route("TaxDeductionReportFile/GetTaxDeductionReportData")]
        public HttpResponseMessage GetTaxDeductionReportData(string reportId)
        {
            try
            {
                if (!HttpContext.Current.Request.Headers.AllKeys.Contains("Token"))
                {
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, "Missing authentication token.");
                }
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                IAccountingContext context = AccountingContext.GetContext(authToken.Tenant);
                TaxDeductionReportQueryService taxDeductionReportQuery = new TaxDeductionReportQueryService(context);
                taxDeductionReportQuery.InitializeSettings();
                TaxDeductionReportPM taxDeductionReportPM = taxDeductionReportQuery.GetSingle(reportId, true, false);
                if (taxDeductionReportPM == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Tax Deduction Report not found.");
                }

                if (taxDeductionReportPM.StatusTypeCode == TaxDeductionReportStatusValues.Completed && String.IsNullOrEmpty(taxDeductionReportPM.ReportSavedData))
                {
                    TaxDeductionReportService.CreateReportDataForOlderReports(ref taxDeductionReportPM, tenant);
                }

                TaxDeductionReportData taxDeductionReportData = string.IsNullOrEmpty(taxDeductionReportPM.ReportSavedData)
                                ? new TaxDeductionReportData()
                                : JsonSerializer.Deserialize<TaxDeductionReportData>(taxDeductionReportPM.ReportSavedData);
                return Request.CreateResponse(HttpStatusCode.OK, taxDeductionReportData);
            }
            catch (Exception ex)
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteError(
                            $"[GetTaxDeductionReportData] Unexpected error: {ex.GetBaseException().Message} {ex}");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ApiExceptionBuilder.BuildException(ex));

            }
        }

    }
}