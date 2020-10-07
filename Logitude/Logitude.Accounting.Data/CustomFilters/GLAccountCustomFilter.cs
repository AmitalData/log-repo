using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Logitude.Server.Tools.Helpers;
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



        public IQueryable<GLAccount> GetFilteredQuery(QueryOperations operations, IQueryable<GLAccount> queryableData, IAccountingContext context)
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

                    queryableData = FilterByGLAccountCurrency(queryableData, item);

                    if (item.FieldName == "ConnectedToSalesmanId")
                    {

                        string salesManId = item.FieldValue as string;

                        queryableData = (from a in queryableData
                                         join md in context.GLAccountMoreDatas on a.Id equals md.AccountId
                                         
                                         join card in context.Cards on a.Id equals card.GLAccountId
                                         into cardjoin from card in cardjoin.DefaultIfEmpty()

                                         where card.SalesmanUserId == salesManId

                                         select a);


                        List<string> salesmanParentAccounts = queryableData.Select(s => s.Id).ToList();

                        var splittedAccounts = (from a in context.GLAccounts
                                                join c in context.GLAccountCurrencies on a.Id equals c.GLAccountId

                                                where salesmanParentAccounts.Contains(c.MainGLAccountId)
                                                select c.GLAccount);

                        List<string> splittedAccountsIds =  splittedAccounts.Select(s => s.Id).ToList();

                        queryableData = (from a in context.GLAccounts where (salesmanParentAccounts.Contains(a.Id) || splittedAccountsIds.Contains(a.Id)) select a);



                    }

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
        private IQueryable<GLAccount> FilterByGLAccountCurrency(IQueryable<GLAccount> queryableData, QueryFilterItem queryFilterItem)
        {
            if (queryFilterItem.FieldName == "GLAccountCurrencyFilter")
            {
                GLAccountCurrencyRepository gLAccountCurrencyRepository = new GLAccountCurrencyRepository(tenant);
                List<String> GLAccountCurrencyIds = gLAccountCurrencyRepository.GetAll(tenant).Select(s=>s.GLAccountId).ToList();
                queryableData = queryableData.Where(c => GLAccountCurrencyIds.Contains(c.Id) == false  && c.IsMultiCurrency ==false );

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
