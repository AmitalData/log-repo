using Logitude.Accounting.BL.CoreBL.Reports;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Utilities;
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
        public List<LedgerTransactionList> GetGLAccountLedgerTransactionFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //if (ledgerTransactionBalanceService==null)
            //{
                var accountingContext = AccountingContext.GetContext(tenant);
                QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
                LedgerTransactionBalanceFilterCreateLTBFilter ledgerTransactionBalanceFilterCreateLTBFilter = new LedgerTransactionBalanceFilterCreateLTBFilter();
                LedgerTransactionBalanceFilter LTBFilter = ledgerTransactionBalanceFilterCreateLTBFilter.CreateLTBFilter(null, tenant, queryOperations);
                ledgerTransactionBalanceService = new LedgerTransactionBalanceService(accountingContext, LTBFilter);
                ledgerTransactionBalanceService.Run(true);
                
            //}
           
            return ledgerTransactionBalanceService.Response.MyLedgerTransactionList;

        }

        public int GetGLAccountLedgerTransactionFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            if (ledgerTransactionBalanceService == null)
            {
                var accountingContext = AccountingContext.GetContext(tenant);
                QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
                LedgerTransactionBalanceFilterCreateLTBFilter ledgerTransactionBalanceFilterCreateLTBFilter = new LedgerTransactionBalanceFilterCreateLTBFilter();
                LedgerTransactionBalanceFilter LTBFilter = ledgerTransactionBalanceFilterCreateLTBFilter.CreateLTBFilter(null, tenant, queryOperations);
                LTBFilter.GetCount = true;
                ledgerTransactionBalanceService = new LedgerTransactionBalanceService(accountingContext, LTBFilter);
                ledgerTransactionBalanceService.Run(true);
                MapLedgerTransactionnList();
                if (LTBFilter.TaxReportTotalCount.HasValue)
                {
                    ledgerTransactionBalanceService.Response.TotalRowCount = LTBFilter.TaxReportTotalCount.Value;
                }
            }
            return (int)ledgerTransactionBalanceService.Response.TotalRowCount;

        }
       private void MapLedgerTransactionnList()
        {
            LedgerTransactionHelper ledgerTransactionHelper = new LedgerTransactionHelper();

            ledgerTransactionBalanceService.Response.MyLedgerTransactionList.ForEach(rec =>
            {
                rec.Source = rec.IconCode + " " + rec.SourceNumber;
                rec.IsCumulativeLocalAmountPos = rec.CumulativeLocalAmount < 0;
                rec.IsForeignAmountCreditPos = rec.ForeignAmountCredit != 0;
                rec.CalculatedForeignAmount = rec.ForeignAmountCredit != 0 ? rec.ForeignAmountCredit : rec.ForeignAmountDebit;
                rec.IsCumulativeForeignAmountPos = rec.CumulativeForeignAmount < 0;
                rec.IsOriginalAmountPos = rec.OpenAmount < 0;
                rec.IsForeignAmountPos = rec.ForeignAmountCredit != 0;
                rec.ForeignAmountCreditWithSign = rec.CalculatedForeignAmount + " " + rec.CurrencySign;
                rec.CumulativeForeignAmountSign = rec.CumulativeForeignAmount + " " + rec.CurrencySign;
                ledgerTransactionHelper.MapAmountWithNegativeValue(rec);

            });
        }
    }
}