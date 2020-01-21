using Logitude.Accounting.BL.CoreBL.Reports;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Def.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
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
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.App_Code.AngularJS_App_Code.Generated
{
    public partial class LedgerTransactionsController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage GetLedgerTransactionsByFilters([FromUri] ApiQueryFilters filters)
        {
            try
            {
                int tenant = AuthinticateTenant();

                LedgerTransactionBalanceFilter LTBFilter = CreateLTBFilter(filters, tenant);

                var accountingContext = AccountingContext.GetContext(LTBFilter.Tenant);
                var ledgerTransactionBalanceService = new LedgerTransactionBalanceService(accountingContext, LTBFilter);
                ledgerTransactionBalanceService.Run();


                ServiceResponse response = new ServiceResponse();
                if (filters.GetCount)
                {
                    int count = ledgerTransactionBalanceService.Response.TotalRowCount.Value;
                    response.Count = count;
                }

                response.Result = ledgerTransactionBalanceService.Response.MyLedgerTransactionList;
                response.TookMS= ledgerTransactionBalanceService.Response.TookMS;
                HttpResponseMessage reponseMessage = Request.CreateResponse(HttpStatusCode.OK, response);

                return reponseMessage;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
        public HttpResponseMessage GetTransactionsCurrencies(string AccountId)
        {
            try
            {
                int tenant = AuthinticateTenant();

                var accountingContext = AccountingContext.GetContext(tenant);
                var _LedgerTransactionQueryService = new LedgerTransactionQueryService(tenant);

                List<string> currenciesIds = _LedgerTransactionQueryService.GetTransactionsCurrencies(AccountId, tenant);


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
        private static int AuthinticateTenant()
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            SecurityUtility.CheckContactFeature("LedgerTransaction", "READ", authToken.Tenant);

            int tenant = authToken.Tenant;
            return tenant;
        }

        private static LedgerTransactionBalanceFilter CreateLTBFilter(ApiQueryFilters filters, int tenant)
        {
            LedgerTransactionBalanceFilter LTBFilter = new LedgerTransactionBalanceFilter();

            List<ObjectField> LedgerTransactionObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("LedgerTransaction", tenant);
            LTBFilter.PageSize = filters.PageSize;
            LTBFilter.PageStartAtRecordIndex = filters.PageIndex;
            LTBFilter.Tenant = tenant;

            if (!string.IsNullOrEmpty(filters.AdditionalFilters))
            {
                JavaScriptSerializer JsonConvert = new JavaScriptSerializer();
                var filters_list = JsonConvert.Deserialize<List<QueryFilterItem>>(filters.AdditionalFilters);
                var glAccountId = filters_list.Where(d => d.FieldName == "GLAccountId").FirstOrDefault().FieldValue.ToString();
                //var from = filters_list.Where(d => d.FieldName == "CreateDate").FirstOrDefault().FieldValue;
                //var to = filters_list.Where(d => d.FieldName == "CreateDate").FirstOrDefault().FieldValue2;
                var includeRelatedCurrenciesAccount = filters_list.Where(d => d.FieldName == "IncludeRelatedCurrenciesAccount").FirstOrDefault().FieldValue;
                var includeChildAccounts = filters_list.Where(d => d.FieldName == "IncludeChildAccounts").FirstOrDefault().FieldValue;
                string _dateTypeCode = filters_list.Where(d => d.FieldName == "DateTypeCode").FirstOrDefault().FieldValue.ToString();

                // dates
                var createDateFilter = filters_list.Where(d => d.FieldName == "AccountingDate").FirstOrDefault();
                if (createDateFilter != null)
                {
                    var from = createDateFilter.FieldValue.ToString();
                    LTBFilter.From = Convert.ToDateTime(from);

                    var to = createDateFilter.FieldValue2.ToString();
                    LTBFilter.To = Convert.ToDateTime(to);
                }

                //currency
                var currencyIdFilter = filters_list.Where(d => d.FieldName == "CurrencyId").FirstOrDefault();
                if (currencyIdFilter != null)
                {
                    var currencyId = currencyIdFilter.FieldValue.ToString();
                    LTBFilter.CurrencyId = currencyId;
                }

                //search
                var searchFieldsf = filters_list.Where(d => d.FieldName == "SearchFields").FirstOrDefault();
                if (searchFieldsf != null)
                {
                    var searchFields = searchFieldsf.FieldValue.ToString();
                    LTBFilter.SearchFields = searchFields;
                }

                LTBFilter.GLAccountId = glAccountId;
                LTBFilter.DateTypeCode = _dateTypeCode;
                //LTBFilter.From = Convert.ToDateTime(from);
                //LTBFilter.To = Convert.ToDateTime(to);
                LTBFilter.IncludeRelatedCurrenciesAccount = Convert.ToBoolean(includeRelatedCurrenciesAccount);
                LTBFilter.IncludeChildAccounts = Convert.ToBoolean(includeChildAccounts);
            }

            return LTBFilter;
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
                

                LedgerTransactionBalanceFilter LTBFilter = new LedgerTransactionBalanceFilter();

                List<ObjectField> LedgerTransactionObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("LedgerTransaction", tenant);
                LTBFilter.PageSize = filters.PageSize;
                LTBFilter.PageStartAtRecordIndex = filters.PageIndex;
                LTBFilter.Tenant = tenant;

                if (!string.IsNullOrEmpty(filters.AdditionalFilters))
                {
                    JavaScriptSerializer JsonConvert = new JavaScriptSerializer();
                    var filters_list = JsonConvert.Deserialize<List<QueryFilterItem>>(filters.AdditionalFilters);
                    var glAccountId = filters_list.Where(d => d.FieldName == "GLAccountId").FirstOrDefault().FieldValue.ToString();
                    var from = filters_list.Where(d => d.FieldName == "AccountingDate").FirstOrDefault().FieldValue;
                    var to = filters_list.Where(d => d.FieldName == "AccountingDate").FirstOrDefault().FieldValue2;
                    var includeRelatedCurrenciesAccount = filters_list.Where(d => d.FieldName == "IncludeRelatedCurrenciesAccount").FirstOrDefault().FieldValue;
                    var includeChildAccounts = filters_list.Where(d => d.FieldName == "IncludeChildAccounts").FirstOrDefault().FieldValue;
                    string _dateTypeCode = filters_list.Where(d => d.FieldName == "DateTypeCode").FirstOrDefault().FieldValue.ToString();

                    var currencyIdFilter = filters_list.Where(d => d.FieldName == "CurrencyId").FirstOrDefault();
                    if (currencyIdFilter != null)
                    {
                        var currencyId = filters_list.Where(d => d.FieldName == "CurrencyId").FirstOrDefault().FieldValue.ToString();
                        LTBFilter.CurrencyId = currencyId;
                    }
                    //search
                    var searchFieldsf = filters_list.Where(d => d.FieldName == "SearchFields").FirstOrDefault();
                    if (searchFieldsf != null)
                    {
                        var searchFields = searchFieldsf.FieldValue.ToString();
                        LTBFilter.SearchFields = searchFields;
                    }
                    LTBFilter.GLAccountId = glAccountId;
                    LTBFilter.From = Convert.ToDateTime(from);
                    LTBFilter.To = Convert.ToDateTime(to);
                    LTBFilter.IncludeRelatedCurrenciesAccount = Convert.ToBoolean(includeRelatedCurrenciesAccount);
                    LTBFilter.IncludeChildAccounts = Convert.ToBoolean(includeChildAccounts);
                    LTBFilter.DateTypeCode = _dateTypeCode;
                }
                var accountingContext = AccountingContext.GetContext(LTBFilter.Tenant);
                var ledgerTransactionBalanceService = new LedgerTransactionBalanceService(accountingContext,LTBFilter);
                ledgerTransactionBalanceService.Run();

                LTBFilter.CallBack = new LedgerTransactionBalanceFilterCallBack()
                {
                    //EndBalanceForeign = ledgerTransactionBalanceService.Response.EndBalanceForeign,
                    EndBalanceForeignList = ledgerTransactionBalanceService.Response.EndBalanceForeignList,
                    EndBalanceLocal = ledgerTransactionBalanceService.Response.EndBalanceLocal,
                    Have1CurrencyIdInPeriod = ledgerTransactionBalanceService.Response.Have1CurrencyIdInPeriod,
                    MaxCreateAt = ledgerTransactionBalanceService.Response.MaxCreateAt,

                    //StartBalanceForeign = ledgerTransactionBalanceService.Response.StartBalanceForeign,
                    StartBalanceForeignList = ledgerTransactionBalanceService.Response.StartBalanceForeignList,
                    StartBalanceLocal = ledgerTransactionBalanceService.Response.StartBalanceLocal,
                    TotalRowCount = ledgerTransactionBalanceService.Response.TotalRowCount,
                    YearTransferLedgerTransactionIds = ledgerTransactionBalanceService.Response.YearTransferLedgerTransactionIds,

                SuppressCumulativeDueMultiCurrencyInPeriod = ledgerTransactionBalanceService.Response.SuppressCumulativeDueMultiCurrencyInPeriod
                    
                };

                ServiceResponse response = new ServiceResponse();
                if (filters.GetCount)
                {
                    int count = ledgerTransactionBalanceService.Response.TotalRowCount.Value;
                    response.Count = count;
                }

                response.Result = LTBFilter.CallBack;
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
                var MyTrans = LedgerTransactionQuery.GetFirstLedgerTransaction(AccountId,tenant); 
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
        public HttpResponseMessage GetTransactionsForARPayment(string arpaymentId, string billToGLAccountId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                int tenant = authToken.Tenant;
                SecurityUtility.CheckContactFeature("LedgerTransaction", "READ", authToken.Tenant);
                SecurityUtility.CheckContactFeature("ARPayment", "READ", authToken.Tenant);

                if (arpaymentId == "undefined")
                    arpaymentId = null;

                var accountingContext = AccountingContext.GetContext(tenant);
                LedgerTransactionQueryService query = new LedgerTransactionQueryService(accountingContext);

                // get reconciled transactions
                List<LedgerTransactionPM> reconciledTransactions = new List<LedgerTransactionPM>();
                if (arpaymentId != null) reconciledTransactions = query.GetReconciledInvoicesTransactionsForARPayment(arpaymentId,billToGLAccountId, tenant);

                // get full opened & partailly reconciled transactions
                List<LedgerTransactionPM> openedTransactions 
                    = query.GetOpenInvoicesTransactionsForAccount(billToGLAccountId, arpaymentId, tenant);

                // concat two list
                IEnumerable<LedgerTransactionPM> finalTransactionsList
                    = openedTransactions
                        .Concat(reconciledTransactions);


                finalTransactionsList
                    = finalTransactionsList
                        .OrderByDescending(d => d.IsReconciled).ThenByDescending(d => d.PaymentReconciledAmount).ToList();


                ServiceResponse response = new ServiceResponse();
                response.Result = finalTransactionsList;
                HttpResponseMessage reponseMessage = Request.CreateResponse(HttpStatusCode.OK, response);

                return reponseMessage;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }


    }

    public class BalanceCurrency{
        public string CurrencyId { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencySign { get; set; }
        public string ForeignAmount { get; set; }
    }
}