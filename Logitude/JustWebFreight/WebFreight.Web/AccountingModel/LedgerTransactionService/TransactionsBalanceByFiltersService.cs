using Logitude.Accounting.BL.CoreBL.Reports;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityListQueryServices;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace WebFreight.Web.AccountingModel.LedgerTransactionService
{
    public class  TransactionsBalanceByFiltersService
    {

        public TransactionsBalanceByFiltersResult GetTransactionsBalanceByFilters(int tenant,ApiQueryFilters filters=null, LedgerTransactionBalanceFilter MainLTBFilter=null)
        {

            LedgerTransactionBalanceFilter LTBFilter = MainLTBFilter!=null? MainLTBFilter:new LedgerTransactionBalanceFilter();

            List<ObjectField> LedgerTransactionObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("LedgerTransaction", tenant);
            LTBFilter.PageSize = filters!=null?filters.PageSize: LTBFilter.PageSize;
            LTBFilter.PageStartAtRecordIndex = filters != null ? filters.PageIndex: LTBFilter.PageStartAtRecordIndex;
            LTBFilter.Tenant = filters != null ? tenant:LTBFilter.Tenant;

            if (filters!=null && !string.IsNullOrEmpty(filters.AdditionalFilters))
            {
                JavaScriptSerializer JsonConvert = new JavaScriptSerializer();
                var filters_list = JsonConvert.Deserialize<List<QueryFilterItem>>(filters.AdditionalFilters);
                var glAccountId = filters_list.Where(d => d.FieldName == "GLAccountId").FirstOrDefault().FieldValue.ToString();
                var from = filters_list.Where(d => d.FieldName == "AccountingDate").FirstOrDefault().FieldValue;
                var to = filters_list.Where(d => d.FieldName == "AccountingDate").FirstOrDefault().FieldValue2;


                var includeRelatedCurrenciesAccount = filters_list.Where(d => d.FieldName == "IncludeRelatedCurrenciesAccount").FirstOrDefault().FieldValue;
                var includeChildAccounts = filters_list.Where(d => d.FieldName == "IncludeChildAccounts").FirstOrDefault().FieldValue;
                string _dateTypeCode = filters_list.Where(d => d.FieldName == "DateTypeCode").FirstOrDefault()?.FieldValue.ToString();
                
                SetSecondeDateFilter(LTBFilter, filters_list);

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

                string[] fromDate = from.ToString().Split(';');
                LTBFilter.From = new DateTime(int.Parse(fromDate[0]), int.Parse(fromDate[1]) + 1, int.Parse(fromDate[2]), 0, 0, 0);

                string[] toDate = to.ToString().Split(';');
                LTBFilter.To = new DateTime(int.Parse(toDate[0]), int.Parse(toDate[1]) + 1, int.Parse(toDate[2]), 23, 59, 59);

                LTBFilter.IncludeRelatedCurrenciesAccount = Convert.ToBoolean(includeRelatedCurrenciesAccount);
                LTBFilter.IncludeChildAccounts = Convert.ToBoolean(includeChildAccounts);
                LTBFilter.DateTypeCode = _dateTypeCode;
            }
            var accountingContext = AccountingContext.GetContext(LTBFilter.Tenant);
            var ledgerTransactionBalanceService = new LedgerTransactionBalanceService(accountingContext, LTBFilter);
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

            TransactionsBalanceByFiltersResult filtersResult = new TransactionsBalanceByFiltersResult()
            {
                CallBack = LTBFilter.CallBack,
                ledgerTransactionBalanceService= ledgerTransactionBalanceService,
            };

            return filtersResult;
        }

        private void SetSecondeDateFilter(LedgerTransactionBalanceFilter LTBFilter, List<QueryFilterItem> filters_list)
        {
            string date2TypeCode = filters_list.Where(d => d.FieldName == "Date2TypeCode").FirstOrDefault()?.FieldValue.ToString();
            string fromDate2filter = filters_list.Where(d => d.FieldName == "Date2Filter").FirstOrDefault()?.FieldValue.ToString();
            string toDate2filter = filters_list.Where(d => d.FieldName == "Date2Filter").FirstOrDefault()?.FieldValue2.ToString();
            LTBFilter.Date2TypeCode = date2TypeCode;

            string[] fromDate2 = fromDate2filter.ToString().Split(';');
            LTBFilter.FromDate2 = new DateTime(int.Parse(fromDate2[0]), int.Parse(fromDate2[1]) + 1, int.Parse(fromDate2[2]), 0, 0, 0);

            string[] toDate2 = toDate2filter.ToString().Split(';');
            LTBFilter.ToDate2 = new DateTime(int.Parse(toDate2[0]), int.Parse(toDate2[1]) + 1, int.Parse(toDate2[2]), 23, 59, 59);
        }
    }

    public class TransactionsBalanceByFiltersResult
    {
        public LedgerTransactionBalanceFilterCallBack CallBack { get; set; }
        public LedgerTransactionBalanceService ledgerTransactionBalanceService { get; set; }
    }

}
