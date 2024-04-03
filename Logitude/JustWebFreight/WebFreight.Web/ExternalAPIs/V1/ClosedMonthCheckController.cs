using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Security;

namespace WebFreight.Web.ExternalAPIs.V1
{
    public class ClosedMonthCheckController : ApiController
    {
        private const string OpenPeriod = "Open";
        private const string ClosedPeriod = "Closed";
        public HttpResponseMessage Get(int? year ,int? month,string periodTypeCode)
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            try
            {
                ValidateInputParamaters(year, month, periodTypeCode);
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticateAPICall(authToken.Tenant);
                SecurityUtility.AuthenticateAccessibleAPI("ClosedMonthCheck", authToken.Tenant);

                IAccountingContext accountingContext = AccountingContext.GetContext(tenant);
                AccountingPeriodQueryService accountingPeriodQueryService = new AccountingPeriodQueryService(accountingContext);
                List<AccountingPeriodPM> accountingPeriodsByTypeRegular = accountingPeriodQueryService.GetAccountingPeriodsByTenantAndType(periodTypeCode, tenant);
                var currentAccountingPeriodPM = accountingPeriodsByTypeRegular.FirstOrDefault(periods => periods.Year == year);
                ValidateAccountingPeriod(currentAccountingPeriodPM, periodTypeCode);
                string result = GetAccountingPeriodStatus(currentAccountingPeriodPM, month);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        private string GetAccountingPeriodStatus(AccountingPeriodPM currentAccountingPeriodPM, int? month)
        {
            if (currentAccountingPeriodPM != null && month > currentAccountingPeriodPM.ClosedMonth.GetValueOrDefault() && month <= currentAccountingPeriodPM.OpenMonth)
            {
                return OpenPeriod;
            }
            else
            {
                return ClosedPeriod;
            }
        }

        private void ValidateAccountingPeriod(AccountingPeriodPM currentAccountingPeriodPM,string periodTypeCode)
        {
            if (currentAccountingPeriodPM == null)
            {
                throw new Exception("Accounting period with code " + periodTypeCode + " doesn't exist");
            }
        }

        private void ValidateInputParamaters(int? year, int? month, string periodTypeCode) {
            if (year is null)
            {
                throw new Exception("Year is required");
            }
            if (month is null)
            {
                throw new Exception("Month is required");
            }
            if (string.IsNullOrWhiteSpace(periodTypeCode))
            {
                throw new Exception("Accounting period is required");
            }
            String DateString = String.Format("{0}/{1}/{2}", month, 1, year);
            DateTime dateTime;
            if (!DateTime.TryParse(DateString, out dateTime))
            {
                throw new Exception("Please enter a valid year and month");
            }
        }
    }
}
