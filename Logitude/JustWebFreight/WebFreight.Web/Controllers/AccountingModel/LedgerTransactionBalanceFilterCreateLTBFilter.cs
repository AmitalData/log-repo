using Logitude.Accounting.Data.EntityListQueryServices;
using Simplog.Data.InfrastructureModel.Repositories;
using System.Web.Script.Serialization;
using System;
using System.Collections.Generic;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System.Linq;
using System.Web;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using WebFreight.Web.Security;

public class LedgerTransactionBalanceFilterCreateLTBFilter
{
    public   LedgerTransactionBalanceFilter CreateLTBFilter(ApiQueryFilters filters, int tenant, QueryOperations queryOperations = null)
    {
        string token = HttpContext.Current.Request.Headers["Token"];
        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
        SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
        SecurityUtility.AuthenticationOnTenant(tenant);

        LedgerTransactionBalanceFilter LTBFilter = new LedgerTransactionBalanceFilter();

        List<ObjectField> LedgerTransactionObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("LedgerTransaction", tenant);
        LTBFilter.PageSize = filters!=null? filters.PageSize: queryOperations.PageSize;
        LTBFilter.PageStartAtRecordIndex = filters != null ? filters.PageIndex: queryOperations.PageIndex;
        LTBFilter.Tenant = tenant;

        if ((filters!=null && !string.IsNullOrEmpty(filters.AdditionalFilters)) || (queryOperations != null &&  queryOperations.QueryFilterItems!=null && queryOperations.QueryFilterItems.Count >0))
        {
            JavaScriptSerializer JsonConvert = new JavaScriptSerializer();
            var filters_list = filters!=null?JsonConvert.Deserialize<List<QueryFilterItem>>(filters.AdditionalFilters): queryOperations.QueryFilterItems;
            var glAccountId = filters_list.Where(d => d.FieldName == "GLAccountId").FirstOrDefault().FieldValue.ToString();
            //var from = filters_list.Where(d => d.FieldName == "CreateDate").FirstOrDefault().FieldValue;
            //var to = filters_list.Where(d => d.FieldName == "CreateDate").FirstOrDefault().FieldValue2;
            var includeRelatedCurrenciesAccount = filters_list.Where(d => d.FieldName == "IncludeRelatedCurrenciesAccount").FirstOrDefault().FieldValue;
            var includeChildAccounts = filters_list.Where(d => d.FieldName == "IncludeChildAccounts").FirstOrDefault().FieldValue;
            string _dateTypeCode = filters_list.Where(d => d.FieldName == "DateTypeCode").FirstOrDefault().FieldValue.ToString();
            LTBFilter.TaxreportId = filters_list.Where(d => d.FieldName == "TaxReportId").FirstOrDefault()?.FieldValue.ToString();
            LTBFilter.NotIncludedInAnyTaxReport = Convert.ToBoolean(filters_list.Where(d => d.FieldName == "NotIncludedInAnyTaxReport").FirstOrDefault().FieldValue);
            LTBFilter.UseTaxreportFilter = Convert.ToBoolean(filters_list.Where(d => d.FieldName == "UseTaxreportFilter").FirstOrDefault().FieldValue);

            // dates
            var createDateFilter = filters_list.Where(d => d.FieldName == "AccountingDate").FirstOrDefault();
            if (createDateFilter != null)
            {

                var from = createDateFilter.FieldValue.ToString();
                string[] fromDate = from.ToString().Split(';');
                LTBFilter.From = new DateTime(int.Parse(fromDate[0]), int.Parse(fromDate[1]) + 1, int.Parse(fromDate[2]), 0, 0, 0);

                var to = createDateFilter.FieldValue2.ToString();
                string[] toDate = to.ToString().Split(';');
                LTBFilter.To = new DateTime(int.Parse(toDate[0]), int.Parse(toDate[1]) + 1, int.Parse(toDate[2]), 23, 59, 59);


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
}