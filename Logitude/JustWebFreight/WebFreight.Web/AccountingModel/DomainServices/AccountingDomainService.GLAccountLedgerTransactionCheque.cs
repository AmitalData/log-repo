using Logitude.Accounting.BL.CoreBL;
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
        public List<LedgerTransactionList> GetGLAccountLedgerTransactionChequeFilters(byte[] xmlFilters, int tenant)
        {

            SecurityUtility.AuthenticationOnTenant(tenant);
            accountingContext = AccountingContext.GetContext(tenant);
            LedgerTransactionListQueryService listService = new LedgerTransactionListQueryService(accountingContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            queryOperations.QueryFilterItems.Where(d => d.FieldName == "GLAccountId").FirstOrDefault().FieldName = "AccountId";
            var AccountId = queryOperations.QueryFilterItems.Where(d => d.FieldName == "AccountId").FirstOrDefault().FieldValue.ToString();
            GLAccountChequesTransactionsRetreivingService ledgerTransactionRetreivingService = new GLAccountChequesTransactionsRetreivingService(tenant, accountingContext);
            List<LedgerTransactionList> tranactions = ledgerTransactionRetreivingService.GetAccountChequesTransactions(AccountId);
            MapLedgerTransactionnList(tranactions);
            return tranactions;
        }
        private void MapLedgerTransactionnList(List<LedgerTransactionList> tranactions)
        {
            LedgerTransactionHelper ledgerTransactionHelper = new LedgerTransactionHelper();
            tranactions.ForEach(rec =>
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
        public int GetGLAccountLedgerTransactionChequeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            accountingContext = AccountingContext.GetContext(tenant);
            LedgerTransactionListQueryService queryService = new LedgerTransactionListQueryService(accountingContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            queryOperations.QueryFilterItems.Where(d => d.FieldName == "GLAccountId").FirstOrDefault().FieldName = "AccountId";
            return queryService.GetListCount(queryOperations, tenant);


        }
    }
}