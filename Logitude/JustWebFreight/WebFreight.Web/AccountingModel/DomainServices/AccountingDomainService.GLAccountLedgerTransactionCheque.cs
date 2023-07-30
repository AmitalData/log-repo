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
            var IsFutureOpenCheques = queryOperations.QueryFilterItems.Where(d => d.FieldName == "IsFutureOpenCheques").FirstOrDefault().FieldValue.ToString();
            GLAccountChequesTransactionsRetreivingService ledgerTransactionRetreivingService = new GLAccountChequesTransactionsRetreivingService(tenant, accountingContext, IsFutureOpenCheques == "True");
            List<LedgerTransactionList> tranactions = ledgerTransactionRetreivingService.GetAccountChequesTransactions(AccountId, queryOperations.SortByColumnName, queryOperations.SortDirectin);
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
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            string accountId = GetGLAccountFilterValue(queryOperations);
            string isFutureOpenCheques = GetIsFutureOpenChequesFilterValue(queryOperations);
            GLAccountChequesTransactionsRetreivingService ledgerTransactionRetreivingService = new GLAccountChequesTransactionsRetreivingService(tenant, accountingContext, isFutureOpenCheques == "True");
            return ledgerTransactionRetreivingService.GetAccountChequesTransactionsCount(accountId);
        }

        private static string GetIsFutureOpenChequesFilterValue(QueryOperations queryOperations)
        {
            QueryFilterItem filterItem = queryOperations.QueryFilterItems.Find(d => d.FieldName == "IsFutureOpenCheques");
            return filterItem?.FieldValue.ToString();

        }

        private static string GetGLAccountFilterValue(QueryOperations queryOperations)
        {
            QueryFilterItem filterItem = queryOperations.QueryFilterItems.Find(d => d.FieldName == "GLAccountId");
            return filterItem?.FieldValue.ToString();

        }
    }
}