using Logitude.Accounting.BL.APIDataContract;
using Logitude.Accounting.BL.APIDataContract.ApiV1;
using Logitude.Accounting.BL.CoreBL.Reports;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.Server.Tools;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers.ExternalAPIHelpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.ExternalAPIs.V1
{
    public class GLAccountMoreDataController : ApiController
    {
        public HttpResponseMessage GetSingleGLAccountMoreData(string number )
        {
            try
            {
                AuthenticationToken authToken = GetAuthenticationToken();
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticateAPICall(authToken.Tenant);
                GLAccount account= GetGLAccountByNumber(number, tenant);
                GLAccountMoreData gLAccountMoreData = GetSingleGLAccountMoreDataByGLAccountId(account);
                return Request.CreateResponse(HttpStatusCode.OK, gLAccountMoreData);
            }
            catch (Exception ex)
            {
                return CreateResponse(ex, null);
            }
        }



        private AuthenticationToken GetAuthenticationToken()
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            return authToken;
        }

        private HttpResponseMessage CreateResponse(Exception exception, string message)
        {
            if (exception == null && message != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, message);
            }
            else
            {
                var apiExceptionResult = ApiExceptionHandler.HandleException(exception);
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }
        private GLAccount GetGLAccountByNumber(string internalNumber, int tenant)
        {
            GLAccountQueryService Service = new GLAccountQueryService(tenant);
            GLAccount account= Service.GetGLAccountByInternalNumber(internalNumber, tenant);
            if(account == null)
            {
                throw new Exception("There is no GLAccount with number" + internalNumber);
            }
            else { return account; }
        }
        private GLAccountMoreData GetSingleGLAccountMoreDataByGLAccountId(GLAccount account)
        {
            GLAccountMoreDataQueryService accountMoreDataQueryService = new GLAccountMoreDataQueryService(account.Tenant);
            bool exist= accountMoreDataQueryService.CheckIfGLAccountHasMoreData(account.Id, account.Tenant);
            if (!exist)
            {
                throw new Exception("GLAccount with number " + account.InternalNumber + " has no GLAccount more data record");
            }
            else
            {
                GLAccountMoreData accountMoreData = accountMoreDataQueryService.GetGLAccountMoreDataByAccountId(account.Id, account.Tenant);
                accountMoreData.AccountId = null;
                accountMoreData.GLAccountTotalsByCurrencies = GetTotalByCurrencies(account);

                //accountMoreData.TotFutureOpenChequesInLocalCur += GetAccountExternalTransactionTotal(account.Tenant, account.Id);

                return accountMoreData;
            }

        }

        private decimal GetAccountExternalTransactionTotal(int tenant, string accountId)
        {
            LedgerTransactionListQueryService ledgerQuery = InitLedgerQuery(tenant);

            var externalTransactions = ledgerQuery.GetExternalTransactionsForAccount(accountId, tenant).ToList();

            decimal externalTransactionsTotal = 0;
            if (externalTransactions != null && externalTransactions.Count > 0)
                externalTransactionsTotal = externalTransactions.Sum(d => d.LocalAmountCredit);

            return externalTransactionsTotal;
        }

        private static LedgerTransactionListQueryService InitLedgerQuery(int tenant)
        {
            IAccountingContext accountingContext = AccountingContext.GetContext(tenant);
            LedgerTransactionListQueryService ledgerQuery = new LedgerTransactionListQueryService(accountingContext);
            return ledgerQuery;
        }

        private List<GLAccountTotalsByCurrency> GetTotalByCurrencies(GLAccount account)
        {
            LedgerTransactionBalanceFilter LTBFilter = SetLTBFilters(account);              
            var accountingContext = AccountingContext.GetContext(LTBFilter.Tenant);
            var ledgerTransactionBalanceService = new LedgerTransactionBalanceService(accountingContext, LTBFilter);
            ledgerTransactionBalanceService.Run();
            List<GLAccountTotalsByCurrency> totalsByCurrencies = new List<GLAccountTotalsByCurrency>();
            foreach(var item in ledgerTransactionBalanceService.Response.EndBalanceForeignList)
            {
                CurrencyQueryService currencyQueryService = new CurrencyQueryService(account.Tenant);
                Logitude.BL.CommonDataModel.APIDataContract.ApiV1.Currency currency = currencyQueryService.GetCurrencyById(item.CurrencyId, account.Tenant);
                totalsByCurrencies.Add(new GLAccountTotalsByCurrency()
                {
                    CurrencyId = currency != null? currency.Code :null,
                    BalanceForeign = item.BalanceForeign,
                    BalanceLocal = item.BalanceLocal,
                });
            }
            return totalsByCurrencies;
        }
        private LedgerTransactionBalanceFilter SetLTBFilters(GLAccount account)
        {
            var toDate = DateTime.Now;
            return new LedgerTransactionBalanceFilter()
            {
                PageSize = 30,
                PageStartAtRecordIndex = 0,
                Tenant = account.Tenant,
                GLAccountId = account.Id,
                IncludeRelatedCurrenciesAccount = false,
                IncludeChildAccounts = false,
                DateTypeCode = "1",
                From = DateTime.Now.AddMonths(-1),
                To = new DateTime(toDate.Year, toDate.Month, toDate.Day, 23, 59, 59),
            };
        }
    }
}