using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.EntityLists;

namespace Logitude.Accounting.Data.EntityListQueryServices
{ 

    public partial class ReconciliationLineListQueryService
    {
	    private IQueryable<ReconciliationLineList> GetIqueryableList(IQueryable<ReconciliationLine> iQueryable)
        {
            IQueryable<ReconciliationLineList> query = (from a in iQueryable.Include("LedgerTransaction").Include("LedgerTransaction.Journal")
                                                        select new ReconciliationLineList()
                                                        {
                                                            ReconciliationId = a.ReconciliationId,
                                                            TransactionId = a.TransactionId,
                                                            CurrencyId = a.CurrencyId,
                                                            CurrencyName = a.Currency != null ? a.Currency.EnglishName : null,
                                                            CurrencyCode = a.Currency != null ? a.Currency.Code : null,
                                                            Line = a.Line,
                                                            IsPartial = a.IsPartial,
                                                            Tenant = a.Tenant,
                                                            ReconciliationAmount = a.ReconciliationAmount,
                                                            // ledger transaction fields
                                                            CreateDate = a.LedgerTransaction != null ? a.LedgerTransaction.CreateDate : DateTime.Now,
                                                            DueDate = (a.LedgerTransaction != null ? a.LedgerTransaction.DueDate : DateTime.Now),
                                                            AmountCredit = (a.LedgerTransaction != null ? a.LedgerTransaction.Account.ReconcileMethodCode=="0"? a.LedgerTransaction.LocalAmountCredit: a.LedgerTransaction.ForeignAmountCredit : 0),
                                                            AmountDebit = (a.LedgerTransaction != null ? a.LedgerTransaction.Account.ReconcileMethodCode == "0" ? a.LedgerTransaction.LocalAmountDebit : a.LedgerTransaction.ForeignAmountDebit : 0),
                                                            IsAmountDebitNegative = (a.LedgerTransaction != null ? a.LedgerTransaction.Account.ReconcileMethodCode == "0" ? a.LedgerTransaction.LocalAmountDebit : a.LedgerTransaction.ForeignAmountDebit : 0) > 0,
                                                            TransactionAmount = (a.LedgerTransaction != null ? a.LedgerTransaction.Account.ReconcileMethodCode == "0" ? a.LedgerTransaction.LocalAmountCredit : a.LedgerTransaction.ForeignAmountCredit : 0) == 0?
                                                                                -1*(a.LedgerTransaction != null ? a.LedgerTransaction.Account.ReconcileMethodCode == "0" ? a.LedgerTransaction.LocalAmountDebit : a.LedgerTransaction.ForeignAmountDebit : 0) :
                                                                                (a.LedgerTransaction != null ? a.LedgerTransaction.Account.ReconcileMethodCode == "0" ? a.LedgerTransaction.LocalAmountCredit : a.LedgerTransaction.ForeignAmountCredit : 0),
                                                            Reference1 = (a.LedgerTransaction != null ? a.LedgerTransaction.Reference1 : null),
                                                            Reference2 = (a.LedgerTransaction != null ? a.LedgerTransaction.Reference2 : null),
                                                            Reference3 = (a.LedgerTransaction != null ? a.LedgerTransaction.Reference3 : null),
                                                            Notes = (a.LedgerTransaction != null ? a.LedgerTransaction.Notes : null),
                                                            JournalNumber = (a.LedgerTransaction != null ? a.LedgerTransaction.JournalLine.Journal.JournalNumber : null),
                                                            JournalId = (a.LedgerTransaction != null ? a.LedgerTransaction.JournalId : null),
                                                            OpenAmountCurrencySign = (a.Currency != null ? a.Currency.Sign : null),
                                                            SearchFields = (a.LedgerTransaction != null ? a.LedgerTransaction.SearchFields : null),
                                                            CurrencySign = (a.Currency != null ? a.Currency.Sign : null),
                                                            AccountingDate = a.LedgerTransaction != null ? a.LedgerTransaction.AccountingDate : DateTime.Now,
                                                            ReconciliationAmountWithSign = a.ReconciliationAmount +" "+ (a.Currency != null ? a.Currency.Sign : null),

                                                        });;
            return query;
        }


		private IQueryable<ReconciliationLine> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<ReconciliationLine> iQueryable,int tenant)
        {
            return iQueryable;
		}

		private IQueryable<ReconciliationLine> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<ReconciliationLine> iQueryable,int tenant)
        {
			return iQueryable;
		}


	}


}
	