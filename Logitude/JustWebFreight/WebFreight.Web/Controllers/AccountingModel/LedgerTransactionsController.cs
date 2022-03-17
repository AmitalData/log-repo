using Logitude.Accounting.BL.CoreBL;
using Logitude.Accounting.BL.CoreBL.Reports;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Data.Utilities;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using WebFreight.Web.AccountingModel.LedgerTransactionService;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.App_Code.AngularJS_App_Code.Generated
{
    public partial class LedgerTransactionsController : ApiController
    {
        decimal CumulativeLocalAmount = 0;
        [HttpGet]
        public HttpResponseMessage GetLedgerTransactionsByFilters([FromUri] ApiQueryFilters filters)
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();
                int tenant = AuthinticateTenant();
                LedgerTransactionBalanceFilterCreateLTBFilter ledgerTransactionBalanceFilterCreateLTBFilter = new LedgerTransactionBalanceFilterCreateLTBFilter();
                LedgerTransactionBalanceFilter LTBFilter = ledgerTransactionBalanceFilterCreateLTBFilter.CreateLTBFilter(filters, tenant);
                LedgerTransactionHelper ledgerTransactionHelper = new LedgerTransactionHelper();
                IAccountingContext  accountingContext = AccountingContext.GetContext(LTBFilter.Tenant);
                var ledgerTransactionBalanceService = new LedgerTransactionBalanceService(accountingContext, LTBFilter);
                List<LedgerTransactionList> ledgerTransactions = new List<LedgerTransactionList>();
                
                if (LTBFilter.UseTaxreportFilter)
                {
                   
                        ledgerTransactions = GetLedgerTransactinByTaxReportId(LTBFilter);

                    ledgerTransactions.ForEach(rec =>
                    {
                            rec.CumulativeLocalAmount = SetCumulativeLocalAmount(rec);                      
                            ledgerTransactionBalanceService.MapLedgerTransactionLine(rec, ledgerTransactionHelper, false);
                    });
                }
               
               
                else
                {
                    ledgerTransactionBalanceService.Run();
                }
                 

                ServiceResponse response = new ServiceResponse();
               
                if(LTBFilter.CurrencyId != null && ledgerTransactions.Count>0)
                {
                    ledgerTransactions = ledgerTransactions.Where(d => d.CurrencyId == LTBFilter.CurrencyId).ToList();
                }
                if (filters.GetCount)
                {
                    response.Count =  LTBFilter.DateTypeCode == "4" ? LTBFilter.TaxReportTotalCount : ledgerTransactionBalanceService.Response.TotalRowCount.Value;
                }
                response.Result = LTBFilter.DateTypeCode == "4" ? ledgerTransactions : ledgerTransactionBalanceService.Response.MyLedgerTransactionList;
                response.TookMS = LTBFilter.DateTypeCode != "4" ? ledgerTransactionBalanceService.Response.TookMS: 0;
                PerformanceLogger.AddServerExecutionTimeHeader(logKey);
                HttpResponseMessage reponseMessage = Request.CreateResponse(HttpStatusCode.OK, response);

                return reponseMessage;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        private decimal SetCumulativeLocalAmount(LedgerTransactionList ledger)
        {
            decimal LocalAmountDebit = ledger.LocalAmountDebit;
            decimal LocalAmountCredit = ledger.LocalAmountCredit;
            CumulativeLocalAmount += (LocalAmountDebit - LocalAmountCredit);
            return CumulativeLocalAmount;
        }


        private List<LedgerTransactionList> GetLedgerTransactinByTaxReportId(LedgerTransactionBalanceFilter LTBFilter)
        {
            IAccountingContext accountingContext = AccountingContext.GetContext(LTBFilter.Tenant);
            LedgerTransactionListQueryService ledgerTransactionListQueryService = new LedgerTransactionListQueryService(accountingContext);
           return ledgerTransactionListQueryService.GetReportLinesLedgerTransactions(LTBFilter).OrderByDescending(d=> d.AccountingDate).ToList();                 
        }

        public HttpResponseMessage GetTransactionsCurrencies(string AccountId, bool splittedByCurrencyCheckBox, bool attachedGLAccountChanged)
        {
            try
            {
                int tenant = AuthinticateTenant();

                var accountingContext = AccountingContext.GetContext(tenant);
                var _LedgerTransactionQueryService = new LedgerTransactionQueryService(tenant);

                List<string> currenciesIds = _LedgerTransactionQueryService.GetTransactionsCurrencies(AccountId, tenant);

                currenciesIds = currenciesIds.Where(x => !x.Contains("1-581")).ToList();
                if (splittedByCurrencyCheckBox) {
                    GLAccountCurrencyQueryService accountcurrencyQueryService = new GLAccountCurrencyQueryService(tenant);

                    var currencies = accountcurrencyQueryService.GetRelatedCurrenciesAccountByCustomerGLAccount(AccountId, tenant);
                    currenciesIds.AddRange(currencies.Select(x => x.CurrencyId));
                }

                if (attachedGLAccountChanged)
                {
                    GLAccountQueryService gLAccountQueryService = new GLAccountQueryService(tenant);

                    var childAccountsCurrencies = gLAccountQueryService.GetChildAccountsCurrencies(tenant, AccountId);
                    currenciesIds.AddRange(childAccountsCurrencies);
                }

                
                ServiceResponse response = new ServiceResponse();
                response.Result = currenciesIds;
                HttpResponseMessage reponseMessage = Request.CreateResponse(HttpStatusCode.OK, response);

                return reponseMessage;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        [HttpGet]
        public HttpResponseMessage GetARPyamentChequesListAsLedgerTransactions([FromUri] ApiQueryFilters filters) {

            int tenant = AuthinticateTenant();
            try
            {
                string accountId = GetGLAccountFilterValueFromQueryOperations(filters, tenant);

                List<LedgerTransactionList> tranactions = GetAccountChequesTransactions(tenant, accountId);

                ServiceResponse response = new ServiceResponse();
                if (filters.GetCount)
                {
                    response.Count = tranactions.Count;
                }

                response.Result = tranactions;
                HttpResponseMessage reponseMessage = Request.CreateResponse(HttpStatusCode.OK, response);
                return reponseMessage;

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        private static string GetGLAccountFilterValueFromQueryOperations(ApiQueryFilters filters, int tenant)
        {
            QueryOperations queryOperations = BuildQueryOperationsForLedgerTransactions(filters, tenant);
            string accountId = GetGLAccountFilterValue(queryOperations);
            return accountId;
        }

        private static List<LedgerTransactionList> GetAccountChequesTransactions(int tenant, string accountId)
        {
            IAccountingContext MyContext = AccountingContext.GetContext(tenant);
            GLAccountChequesTransactionsRetreivingService ledgerTransactionRetreivingService = new GLAccountChequesTransactionsRetreivingService(tenant, MyContext);
            List<LedgerTransactionList> tranactions = ledgerTransactionRetreivingService.GetAccountChequesTransactions(accountId);
            return tranactions;
        }

        private static QueryOperations BuildQueryOperationsForLedgerTransactions(ApiQueryFilters filters, int tenant)
        {
            QueryOperations queryOperations = new QueryOperations()
            {
                ObjectTableName = "LedgerTransaction",
                PageIndex = filters.PageIndex,
                PageSize = filters.PageSize,
                QuerySection = "LedgerTransactions",
                SortByColumnName = filters.SortBy,
                SortDirectin = filters.SortDirection,
                GetAll = filters.GetAll,
            };

            #region filters
            List<ObjectField> LedgerTransactionObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("LedgerTransaction", tenant);
            List<PropertyInfo> filterProperties = filters.GetType().GetProperties().ToList();
       

            if (!string.IsNullOrEmpty(filters.AdditionalFilters))
            {
                JavaScriptSerializer JsonConvert = new JavaScriptSerializer();
                var filters_list = JsonConvert.Deserialize<List<QueryFilterItem>>(filters.AdditionalFilters);

                foreach (QueryFilterItem filter in filters_list)
                {
                    ObjectField field = LedgerTransactionObjectFields.FirstOrDefault(f => f.FieldName == filter.FieldName);
                    if (field != null)
                    {


                        string valuestring1 = filter.FieldValue != null ? filter.FieldValue.ToString() : null;

                        object value1;
                        if (valuestring1 == "#today")
                        {
                            var today = TenantServerConfigration.GetCurrentDateTime(tenant);
                            value1 = new DateTime(today.Year, today.Month, today.Day, 0, 0, 0, 0);
                        }
                        else
                        {
                            value1 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring1);

                        }



                        string valuestring2 = filter.FieldValue2 != null ? filter.FieldValue2.ToString() : null;
                        object value2 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring2);

                        queryOperations.SetFilter(filter.FieldName, value1, field.IsCustomFilter, filter.Operator, value2, field.DisplayInList);
                    }
                    else
                    {
                        queryOperations.SetFilter(filter.FieldName, filter.FieldValue, filter.IsCustom, filter.Operator, filter.FieldValue2, filter.DisplayInList);
                    }
                }
            }
            #endregion
            return queryOperations;
        }

        private static string GetGLAccountFilterValue(QueryOperations queryOperations)
        {
            QueryFilterItem filterItem = queryOperations.QueryFilterItems.Find(d => d.FieldName == "GLAccountId");
            return filterItem?.FieldValue.ToString();

        }

        private static int AuthinticateTenant()
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            SecurityUtility.CheckContactFeature("LedgerTransaction", "READ", authToken.Tenant);

            int tenant = authToken.Tenant;
            return tenant;
        }
 

        public HttpResponseMessage GetTransactionsBalanceByFilters([FromUri] ApiQueryFilters filters)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("LedgerTransaction", "READ", authToken.Tenant);

                int tenant = authToken.Tenant;
                TransactionsBalanceByFiltersService transactionsBalanceByFiltersService = new TransactionsBalanceByFiltersService();
                TransactionsBalanceByFiltersResult transactionsBalanceByFiltersResult = transactionsBalanceByFiltersService.GetTransactionsBalanceByFilters(tenant, filters);
               

                ServiceResponse response = new ServiceResponse();
                if (filters.GetCount)
                {
                    int count = transactionsBalanceByFiltersResult.ledgerTransactionBalanceService.Response.TotalRowCount.Value;
                    response.Count = count;
                }

                response.Result = transactionsBalanceByFiltersResult.CallBack;
                HttpResponseMessage reponseMessage = Request.CreateResponse(HttpStatusCode.OK, response);

                return reponseMessage;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }


        public HttpResponseMessage GetTransactionsCardIndexByFilters([FromUri] ApiQueryFilters filters)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("LedgerTransaction", "READ", authToken.Tenant);

                int tenant = authToken.Tenant;


                LedgerTransactionCardIndexFilter LTCIFilter = new LedgerTransactionCardIndexFilter();

                List<ObjectField> LedgerTransactionObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("LedgerTransaction", tenant);
                LTCIFilter.PageSize = filters.PageSize;
                LTCIFilter.PageStartAtRecordIndex = filters.PageIndex;
                LTCIFilter.Tenant = tenant;

                if (!string.IsNullOrEmpty(filters.AdditionalFilters))
                {
                    JavaScriptSerializer JsonConvert = new JavaScriptSerializer();
                    var filters_list = JsonConvert.Deserialize<List<QueryFilterItem>>(filters.AdditionalFilters);
                    var glAccountId = filters_list.Where(d => d.FieldName == "GLAccountId").FirstOrDefault().FieldValue.ToString();
                    var cat1 = filters_list.Where(d => d.FieldName == "Category1Id").FirstOrDefault().FieldValue.ToString();
                    var cat2 = filters_list.Where(d => d.FieldName == "Category2Id").FirstOrDefault().FieldValue.ToString();
                    var cat3 = filters_list.Where(d => d.FieldName == "Category3Id").FirstOrDefault().FieldValue.ToString();
                    var cat4 = filters_list.Where(d => d.FieldName == "Category4Id").FirstOrDefault().FieldValue.ToString();
                    var cat5 = filters_list.Where(d => d.FieldName == "Category5Id").FirstOrDefault().FieldValue.ToString();
                    var gLAccountType = filters_list.Where(d => d.FieldName == "AccountTypeCode").FirstOrDefault().FieldValue.ToString();
                    var chartOfAccountsId = filters_list.Where(d => d.FieldName == "ChartOfAccountsId").FirstOrDefault().FieldValue.ToString();
                    var dateType = filters_list.Where(d => d.FieldName == "DateTypeCode").FirstOrDefault().FieldValue.ToString();
                    object from = null;
                    object to = null;
                    switch (dateType)
                    {
                        case "1"://AccountingDate:
                            from = filters_list.Where(d => d.FieldName == "AccountingDate").FirstOrDefault().FieldValue;
                            to = filters_list.Where(d => d.FieldName == "AccountingDate").FirstOrDefault().FieldValue2;
                            break;
                        case "2"://DueDate:
                            from = filters_list.Where(d => d.FieldName == "DueDate").FirstOrDefault().FieldValue;
                            to = filters_list.Where(d => d.FieldName == "DueDate").FirstOrDefault().FieldValue2;
                            break;
                        case "3"://GLAccountTotalDateTypeValues.DueDate:
                            from = filters_list.Where(d => d.FieldName == "DocumentDate").FirstOrDefault().FieldValue;
                            to = filters_list.Where(d => d.FieldName == "DocumentDate").FirstOrDefault().FieldValue2;
                            break;
                    }

                    var isReconciled = filters_list.Where(d => d.FieldName == "IsReconciled").FirstOrDefault().FieldValue;
                    var includeChildAccounts = filters_list.Where(d => d.FieldName == "IncludeChildAccounts").FirstOrDefault().FieldValue;

                    var currencyIdFilter = filters_list.Where(d => d.FieldName == "CurrencyId").FirstOrDefault();
                    if (currencyIdFilter != null)
                    {
                        var currencyId = filters_list.Where(d => d.FieldName == "CurrencyId").FirstOrDefault().FieldValue.ToString();
                        LTCIFilter.CurrencyId = currencyId;
                    }


                    //search
                    var searchFieldsf = filters_list.Where(d => d.FieldName == "SearchFields").FirstOrDefault();
                    if (searchFieldsf != null)
                    {
                        var searchFields = searchFieldsf.FieldValue.ToString();
                        LTCIFilter.SearchFields = searchFields;
                    }
                    LTCIFilter.GLAccountId = glAccountId;
                    LTCIFilter.From = Convert.ToDateTime(from);
                    LTCIFilter.To = Convert.ToDateTime(to);
                    LTCIFilter.IncludeChildAccounts = Convert.ToBoolean(includeChildAccounts);
                    LTCIFilter.IsReconciled = Convert.ToBoolean(isReconciled);
                    LTCIFilter.Category1Id = cat1;
                    LTCIFilter.Category2Id = cat2;
                    LTCIFilter.Category3Id = cat3;
                    LTCIFilter.Category4Id = cat4;
                    LTCIFilter.Category5Id = cat5;
                    LTCIFilter.AccountTypeCode = gLAccountType;
                    LTCIFilter.ChartOfAccountsId = chartOfAccountsId;
                }
                var accountingContext = AccountingContext.GetContext(LTCIFilter.Tenant);
                var ledgerTransactionCardIndexService = new LedgerTransactionCardIndexService(accountingContext, LTCIFilter);
                ledgerTransactionCardIndexService.Run();

                LTCIFilter.CallBack = new LedgerTransactionCardIndexFilterCallBack()
                {
                    //EndCardIndexForeign = ledgerTransactionCardIndexService.Response.EndCardIndexForeign,

                    //StartCardIndexForeign = ledgerTransactionCardIndexService.Response.StartCardIndexForeign,
                    TotalRowCount = ledgerTransactionCardIndexService.Response.TotalRowCount,

                };

                ServiceResponse response = new ServiceResponse();
                if (filters.GetCount)
                {
                    int count = ledgerTransactionCardIndexService.Response.TotalRowCount.Value;
                    response.Count = count;
                }

                response.Result = LTCIFilter.CallBack;
                HttpResponseMessage reponseMessage = Request.CreateResponse(HttpStatusCode.OK, response);

                return reponseMessage;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }



        public HttpResponseMessage GetFirstLedgerTransaction([FromUri] string AccountId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("LedgerTransaction", "READ", authToken.Tenant);

                int tenant = authToken.Tenant;


                var accountingContext = AccountingContext.GetContext(tenant);

                var LedgerTransactionRepository = new LedgerTransactionRepository(accountingContext);
                var LedgerTransactionQuery = new LedgerTransactionQueryService(LedgerTransactionRepository);
                var MyTrans = LedgerTransactionQuery.GetFirstLedgerTransaction(AccountId, tenant);
                ServiceResponse response = new ServiceResponse();


                response.Result = MyTrans;
                HttpResponseMessage reponseMessage = Request.CreateResponse(HttpStatusCode.OK, response);

                return reponseMessage;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        [HttpGet]
        public HttpResponseMessage getLedgerTransactionsByIds([FromUri] List<string> Ids)
        {
            if (Ids != null && Ids.Count > 0)
            {
                try
                {

                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                    SecurityUtility.CheckContactFeature("LedgerTransaction", "READ", authToken.Tenant);
                    int tenant = authToken.Tenant;


                    var accountingContext = AccountingContext.GetContext(tenant);

                    LedgerTransactionListQueryService query = new LedgerTransactionListQueryService(accountingContext);
                    var transactions = query.GetTransactionsByIds(Ids);

                    ServiceResponse response = new ServiceResponse();
                    response.Result = transactions.OrderBy(d => d.GroupHash).ToList();

                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, "");

            }

        }

        public HttpResponseMessage GetLast10TransactionsForAccount([FromUri] string AccountId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("LedgerTransaction", "READ", authToken.Tenant);

                int tenant = authToken.Tenant;
                var accountingContext = AccountingContext.GetContext(tenant);

                var LedgerTransactionRepository = new LedgerTransactionRepository(accountingContext);
                var LedgerTransactionQuery = new LedgerTransactionQueryService(LedgerTransactionRepository);
                var MyTrans = LedgerTransactionQuery.GetLast10TransactionsForAccount(AccountId, tenant);

                ServiceResponse response = new ServiceResponse();
                response.Result = MyTrans;

                HttpResponseMessage reponseMessage = Request.CreateResponse(HttpStatusCode.OK, response);

                return reponseMessage;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }


        [HttpGet]
        public HttpResponseMessage GetTransactionsForARPayment(string arpaymentId, string billToGLAccountId, string paymentCurrencyId)
        {
            try

            {
                int tenant = GetAuthinticatedTenant();

                if (arpaymentId == "undefined") arpaymentId = null;

                string accountId = GetGLAccountIdForReconciledTransactions(billToGLAccountId, tenant, paymentCurrencyId);

                ARPaymentInvoicesTransactionFetcher invoiceTransactionsFetcher = new ARPaymentInvoicesTransactionFetcher(arpaymentId, accountId, tenant);
                var transactions = invoiceTransactionsFetcher.FetchSorted();

                HttpResponseMessage reponseMessage = BuildResponseMessage(transactions);

                return reponseMessage;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        private HttpResponseMessage BuildResponseMessage(List<LedgerTransactionPM> transactions)
        {
            ServiceResponse response = new ServiceResponse();
            response.Result = transactions;
            HttpResponseMessage reponseMessage = Request.CreateResponse(HttpStatusCode.OK, response);
            return reponseMessage;
        }


        //var accountingContext = AccountingContext.GetContext(tenant);
        //LedgerTransactionQueryService query = new LedgerTransactionQueryService(accountingContext);



        //// get reconciled transactions
        //List<LedgerTransactionPM> reconciledTransactions = new List<LedgerTransactionPM>();
        //if (arpaymentId != null) reconciledTransactions = query.GetReconciledInvoicesTransactionsForARPayment(arpaymentId, accountId, tenant);

        //// get full opened & partailly reconciled transactions
        //List<LedgerTransactionPM> openedTransactions
        //    = query.GetOpenInvoicesTransactionsForAccount(accountId, arpaymentId, tenant);

        //// concat two list
        //IEnumerable<LedgerTransactionPM> finalTransactionsList
        //    = openedTransactions
        //        .Concat(reconciledTransactions);


        //finalTransactionsList
        //    = finalTransactionsList
        //        .OrderByDescending(d => d.IsReconciled).ThenByDescending(d => d.PaymentReconciledAmount).ToList();


       

        //var accountingContext = AccountingContext.GetContext(tenant);
        //LedgerTransactionQueryService query = new LedgerTransactionQueryService(accountingContext);



        //// get reconciled transactions
        //List<LedgerTransactionPM> reconciledTransactions = new List<LedgerTransactionPM>();
        //if (arpaymentId != null) reconciledTransactions = query.GetReconciledInvoicesTransactionsForARPayment(arpaymentId, accountId, tenant);

        //// get full opened & partailly reconciled transactions
        //List<LedgerTransactionPM> openedTransactions
        //    = query.GetOpenInvoicesTransactionsForAccount(accountId, arpaymentId, tenant);

        //// concat two list
        //IEnumerable<LedgerTransactionPM> finalTransactionsList
        //    = openedTransactions
        //        .Concat(reconciledTransactions);


        //finalTransactionsList
        //    = finalTransactionsList
        //        .OrderByDescending(d => d.IsReconciled).ThenByDescending(d => d.PaymentReconciledAmount).ToList();


        private static int GetAuthinticatedTenant()
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            int tenant = authToken.Tenant;
            SecurityUtility.CheckContactFeature("LedgerTransaction", "READ", authToken.Tenant);
            SecurityUtility.CheckContactFeature("ARPayment", "READ", authToken.Tenant);
            return tenant;
        }

        private string GetGLAccountIdForReconciledTransactions(string glAccountId, int tenant, string paymentCurrencyId)
        {
            GLAccountQueryService gLAccountQueryService = new GLAccountQueryService(tenant);
            GLAccountPM gLAccount = gLAccountQueryService.GetSinglePM(glAccountId, tenant);
            if (gLAccount != null)
            {
                if (gLAccount.IsMultiCurrency.Value)
                {
                    GLAccountCurrencyQueryService gLAccountCurrencyQuery = new GLAccountCurrencyQueryService(tenant);
                    GLAccountCurrencyPM gLAccountCurrency = gLAccountCurrencyQuery.GetEntityByCurrencyAndGLAccountId(gLAccount.Id, paymentCurrencyId, tenant);
                    if (gLAccountCurrency != null)
                    {
                        return gLAccountCurrency.GLAccountId;
                    }
                    else return gLAccount.Id;
                }
                else
                {
                    return gLAccount.Id;
                }
            }
            return null;
        }


    }

    public class BalanceCurrency
    {
        public string CurrencyId { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencySign { get; set; }
        public string ForeignAmount { get; set; }
    }
}