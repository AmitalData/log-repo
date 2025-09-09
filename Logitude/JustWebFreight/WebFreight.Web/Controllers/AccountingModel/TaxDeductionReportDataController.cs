using Logitude.Accounting.BL.CoreBL.Batch;
using Logitude.Accounting.BL.DataContract;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.Utils;
using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;


namespace WebFreight.Web.Controllers.AccountingModel
{
    public class TaxDeductionReportDataController : ApiController
    {
        public TaxDeductionReportDataController()
        {

        }

        [HttpGet]
        [Route("TaxDeductionReportData/GetTaxDeductionReportData")]
        public HttpResponseMessage GetTaxDeductionReportData(string reportId)
        {
            try
            {
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