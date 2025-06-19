using Logitude.Accounting.BL.CoreBL.Batch;
using Logitude.Accounting.BL.DataContract;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.Utils;
using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
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

namespace WebFreight.Web.Controllers.AccountingModel
{
    //[RoutePrefix("api/TaxDeductionReportData")]
    public class TaxDeductionReportDataController : ApiController
    {
        public TaxDeductionReportDataController()
        {

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