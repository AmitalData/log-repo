using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.Data.CustomFilters
{
    public class GLAccountCustomFilter
    {
        private int tenant;
        public GLAccountCustomFilter(int tenant)
        {
            this.Tenant = tenant;
        }

        public int Tenant
        {
            get { return tenant; }
            set { tenant = value; }
        }



        public IQueryable<GLAccount> GetFilteredQuery(QueryOperations operations, IQueryable<GLAccount> queryableData)
        {
            List<QueryFilterItem> queryFilters = operations.QueryFilterItems;

            foreach (QueryFilterItem item in queryFilters)
            {
                if (item.IsCustom)
                {

                    if (item.FieldName == "Name")
                    {
                        string tString = item.FieldValue as string;

                        if (!string.IsNullOrEmpty(tString))
                        {
                            queryableData = queryableData.Where(c => c.EnglishName.StartsWith(tString) || c.LocalName.StartsWith(tString));
                        }
                    }
                    queryableData = FilterByReceivableCredit(queryableData, item);

                    queryableData = FilterByPayableDebit(queryableData, item);

                    if (item.FieldName == "BalanceInLocalCurrencyNotNull")
                    {
                        string tString = item.FieldValue as string;
                        GLAccountMoreDataRepository MoreDataRepository = new GLAccountMoreDataRepository(tenant);
                        List<string> MoreDataIds = (from a in MoreDataRepository.GetAll(tenant)
                                                    where a.BalanceInLocalCurrency != null && a.BalanceInLocalCurrency != 0
                                                    select a.AccountId).ToList();
                        if (!string.IsNullOrEmpty(tString))
                        {
                            queryableData = queryableData.Where(c => MoreDataIds.Contains(c.Id) && c.AccountTypeCode == tString);
                        }
                    }

                    if (item.FieldName == "DisplayNumberOrName")
                    {
                        string value = item.FieldValue as string;

                        if (!string.IsNullOrEmpty(value))
                        {
                            queryableData = queryableData.Where(d => d.EnglishName.StartsWith(value) || d.DisplayNumber.StartsWith(value) || d.LocalName.StartsWith(value));
                        }
                    }
                    if (item.FieldName == "IsParent")
                    {
                        string value = item.FieldValue as string;

                        if (!string.IsNullOrEmpty(value))
                        {
                           
                            IQueryable<GLAccount> query = queryableData.Where(d => d.ParentAccountId != null);
                            List<string> ids = (from d in query
                                                   select d.ParentAccountId).ToList();
                            queryableData = queryableData.Where(d =>! ids.Contains(d.Id));
                                       
                        }
                    }
                    if (item.FieldName == "SingleAndMultiCurrencyAccount")
                    {
                        string value = item.FieldValue as string;

                        if (!string.IsNullOrEmpty(value))
                        {
                            queryableData = queryableData.Where(d => d.CurrencyId == value || d.IsMultiCurrency == true);
                        }
                    }
                }
            }

            return queryableData;
        }

        private IQueryable<GLAccount> FilterByReceivableCredit(IQueryable<GLAccount> queryableData, QueryFilterItem queryFilterItem)
        {
            if (queryFilterItem.FieldName == "ReceivableCreditFilter")
            {

                queryableData = queryableData.Where(c => (c.RevenueExpenseType == "3" && c.ChartOfAccountsTypeCode == "7") || c.RevenueExpenseType == "1");

            }
            return queryableData;

        }
        private IQueryable<GLAccount> FilterByPayableDebit(IQueryable<GLAccount> queryableData, QueryFilterItem queryFilterItem)
        {
            if (queryFilterItem.FieldName == "PayableDebitFilter")
            {

                queryableData = queryableData.Where(c => (c.RevenueExpenseType == "3" && c.ChartOfAccountsTypeCode == "7") || c.RevenueExpenseType == "2");

            }
            return queryableData;

        }
    }
}
