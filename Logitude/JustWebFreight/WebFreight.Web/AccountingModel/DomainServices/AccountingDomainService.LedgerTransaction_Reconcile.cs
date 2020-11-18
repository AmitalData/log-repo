using Logitude.Accounting.BL.CoreBL.Reports;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFreight.Web.Security;

namespace WebFreight.Web.AccountingModel.DomainServices
{
	public partial class AccountingDomainService
	{
        private List<LedgerTransactionList> openTransactions;
        public List<LedgerTransactionList> GetLedgerTransaction_ReconcileFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            if (openTransactions == null)
            {
                var accountingContext = AccountingContext.GetContext(tenant);
                QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
                LedgerTransactionListQueryService transactionQuery = new LedgerTransactionListQueryService(AccountingContext.GetContext(tenant));
                string gLAccountId = (string)queryOperations.QueryFilterItems.Where(s=>s.FieldName== "AccountId").Select(s=>s.FieldValue).FirstOrDefault();
                GenericCallBack callback = transactionQuery.GetReconciliationFilterCallBack(queryOperations, gLAccountId, tenant, true);
                callback.IsFromExcelGenerator = true;
                openTransactions = transactionQuery.GetOpenReconciliationFilterList(queryOperations, callback, gLAccountId, tenant);

            }

            return openTransactions;

        }

        public int GetLedgerTransaction_ReconcileFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            if (openTransactions == null)
            {
                var accountingContext = AccountingContext.GetContext(tenant);
                QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
                LedgerTransactionListQueryService transactionQuery = new LedgerTransactionListQueryService(AccountingContext.GetContext(tenant));
                string gLAccountId = (string)queryOperations.QueryFilterItems.Where(s => s.FieldName == "AccountId").Select(s => s.FieldValue).FirstOrDefault();
                GenericCallBack callback = transactionQuery.GetReconciliationFilterCallBack(queryOperations, gLAccountId, tenant, true);
                callback.IsFromExcelGenerator = true;
                openTransactions = transactionQuery.GetOpenReconciliationFilterList(queryOperations, callback, gLAccountId, tenant);


            }
            return openTransactions.Count;

        }
    }
}