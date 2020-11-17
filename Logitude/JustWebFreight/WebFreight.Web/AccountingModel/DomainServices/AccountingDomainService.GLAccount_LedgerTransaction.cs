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
        private LedgerTransactionBalanceService ledgerTransactionBalanceService;
        public List<LedgerTransactionList> GetGLAccount_LedgerTransactionFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            if (ledgerTransactionBalanceService==null)
            {
                var accountingContext = AccountingContext.GetContext(tenant);
                QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
                LedgerTransactionBalanceFilterCreateLTBFilter ledgerTransactionBalanceFilterCreateLTBFilter = new LedgerTransactionBalanceFilterCreateLTBFilter();
                LedgerTransactionBalanceFilter LTBFilter = ledgerTransactionBalanceFilterCreateLTBFilter.CreateLTBFilter(null, tenant, queryOperations);
                ledgerTransactionBalanceService = new LedgerTransactionBalanceService(accountingContext, LTBFilter);
                ledgerTransactionBalanceService.Run();

            }
           
            return ledgerTransactionBalanceService.Response.MyLedgerTransactionList;

        }

        public int GetGLAccount_LedgerTransactionFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            if (ledgerTransactionBalanceService == null)
            {
                var accountingContext = AccountingContext.GetContext(tenant);
                QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
                LedgerTransactionBalanceFilterCreateLTBFilter ledgerTransactionBalanceFilterCreateLTBFilter = new LedgerTransactionBalanceFilterCreateLTBFilter();
                LedgerTransactionBalanceFilter LTBFilter = ledgerTransactionBalanceFilterCreateLTBFilter.CreateLTBFilter(null, tenant, queryOperations);
                ledgerTransactionBalanceService = new LedgerTransactionBalanceService(accountingContext, LTBFilter);
                ledgerTransactionBalanceService.Run();

            }
            return (int)ledgerTransactionBalanceService.Response.TotalRowCount;

        }
    }
}