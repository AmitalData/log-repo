
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.EntityKeys;
using Simplog.Server.Infrastructure;
using System.Data.Entity.Core.Objects;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Reflection.Emit;
using System.Data.Entity;
using Logitude.Accounting.Def.EntityPMs;
using System.Diagnostics;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.Security;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.BL.EntityQueryServices;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Logitude.Accounting.BL.EntityUpdateServices;
using System.Web;
using Simplog.Data.Helpers;
using Logitude.BL.Interfaces;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;
using Logitude.BL.Helpers;
using Logitude.BL.Resolvers;

namespace Logitude.Accounting.BL.CoreBL
{
    public class AutomaticExternalReconcileService
    {
        public const int RESULT_LIMIT = 100;
        public const bool AllowOnlySingleBankPageLineOnEachReconcileGroup = true;
        public int tenant;
        private List<MatchingLine> matchedLines = new List<MatchingLine>();
        List<MyPageLine> externalPageLines;
        List<MyLedgerTransaction> ledgerTransactions;

        public AutomaticExternalReconcileService(int _tenant)
        {
            tenant = _tenant;
        }

        public MatchedReconciliationLines GetMatchedLines(AutoExternalReconcileArgs args)
        {
            ValidateParameters(args);

            externalPageLines = GetFilteredPageLines(args);
            ledgerTransactions = GetFilteredLedgerTransactions(args);

            if (externalPageLines.Count > 0 && ledgerTransactions.Count > 0)
            {
                if (args.AmountReconcile && !args.ReferenceReconcile && !args.RefDateReconcile && !args.AccoutingDateReconcile)
                    SetMatchedLinesByAmount();
                else if (args.AmountReconcile && args.ReferenceReconcile && !args.RefDateReconcile && !args.AccoutingDateReconcile)
                    SetMatchedLinesByAmountAndReferences();
                else if (args.AmountReconcile && !args.ReferenceReconcile && (args.RefDateReconcile || args.AccoutingDateReconcile))
					SetMatchedLinesByAmountAndReferenceDateOrAccoutingDateRouting(args.RefDateReconcile, args.AccoutingDateReconcile);
                else if (args.AmountReconcile && args.ReferenceReconcile && (args.RefDateReconcile || args.AccoutingDateReconcile))
					SetMatchedLinesByAmountAndReferenceDateOrAccoutingDateAndReferencesRouting(args.RefDateReconcile, args.AccoutingDateReconcile);
                else if (!args.AmountReconcile && !args.ReferenceReconcile && (args.RefDateReconcile || args.AccoutingDateReconcile))
                    throw new ApplicationException(TextCodesTranslator.TranslateText("ExternalReconciliation.O.CantAutoRecoByRefDate",tenant,LoggedContactResolver.GetLoggedContactShowLocal(tenant)));
                //SetMatchedLinesByReferenceDate();
                else if (!args.AmountReconcile && args.ReferenceReconcile && !args.RefDateReconcile && !args.AccoutingDateReconcile)
                    SetMatchedLinesByReferences();
                else if (!args.AmountReconcile && args.ReferenceReconcile && (args.RefDateReconcile || args.AccoutingDateReconcile))
					SetMatchedLinesByReferenceDateOrAccoutingDateAndReferencesRouting(args.RefDateReconcile, args.AccoutingDateReconcile);

			}

            return BuildMathcedReconcileLines();
        }

        private MatchedReconciliationLines BuildMathcedReconcileLines()
        {
            MatchedReconciliationLines matchedReconcileLines = new MatchedReconciliationLines();
            matchedReconcileLines.transactionLines = GetLedgerTransactionsFromMatchedLines();
            matchedReconcileLines.pageLines = GetPageLinesFromMatchedLines();
            matchedReconcileLines.Count = matchedLines.Count;
            return matchedReconcileLines;
        }

        // ----------------------------------------------------------------------------------------------
        //   [Abdullah]: process logic
        //   1- get lines filtered by accounts
        //   2- set dictionary of (KEY => list of transactions) , KEY will be (amount, ref, ref date)
        //   3- loop on page lines, and select matched transaction from dictionary
        //   4- add it to matched result
        // ----------------------------------------------------------------------------------------------
        private void SetMatchedLinesByAmountAndReferenceDateOrAccoutingDateRouting(bool refDate, bool accountDate)
        {

			if (refDate && accountDate)			
			   SetMatchedLinesByAmountAndReferenceDateAndAccoutingDate();			
			else if (refDate)			
				SetMatchedLinesByAmountAndReferenceDate();			
			else if (accountDate)			
				SetMatchedLinesByAmountAndAccoutingDate();	
		}
		private void SetMatchedLinesByAmountAndReferenceDateOrAccoutingDateAndReferencesRouting(bool refDate, bool accountDate)
		{

			if (refDate && accountDate)
				SetMatchedLinesByAmountAndReferenceDateAndAccoutingDateAndReferences();
			else if (refDate)
				SetMatchedLinesByAmountAndReferenceDateAndReferences();
			else if (accountDate)
				SetMatchedLinesByAmountAndAccoutingDateAndReferences();
		}
		private void SetMatchedLinesByReferenceDateOrAccoutingDateAndReferencesRouting(bool refDate, bool accountDate)
		{

			if (refDate && accountDate)
				SetMatchedLinesByReferenceDateAndAccoutingDateAndReferences();
			else if (refDate)
				SetMatchedLinesByReferenceDateAndReferences();
			else if (accountDate)
				SetMatchedLinesByAccoutingDateAndReferences();
		}
		private void SetMatchedLinesByAmount()
        {
            int groupNumberCounter = 1;
            Dictionary<decimal, List<MyLedgerTransaction>> transactionsByCreditAmountDictionary = GetTransactionsByCreditAmountDictionary(ledgerTransactions);
            Dictionary<decimal, List<MyLedgerTransaction>> transactionsByDebitAmountDictionary = GetTransactionsByDebitAmountDictionary(ledgerTransactions);

            foreach (var pageLine in externalPageLines)
            {
                MyLedgerTransaction matchedTransaction = null;
                if (pageLine.CreditAmount != 0)
                    matchedTransaction = GetNotUsedMatchedTransaction(transactionsByDebitAmountDictionary, pageLine.CreditAmount);
				else if (pageLine.DebitAmount != 0)
					matchedTransaction = GetNotUsedMatchedTransaction(transactionsByCreditAmountDictionary, pageLine.DebitAmount);

                if (matchedTransaction != null)
                {
                    AddMatchedPageLine(groupNumberCounter, pageLine);
                    AddMatchedTransaction(groupNumberCounter, matchedTransaction);
                    groupNumberCounter++;

                    if (matchedLines.Count >= RESULT_LIMIT)
                        break;
                }
            }
        }
		private void SetMatchedLinesByAmountAndReferenceDate()
		{
			int groupNumberCounter = 1;			
				Dictionary<AmountRefDateKey, List<MyLedgerTransaction>> transactionsByCreditAmountAndReferenceDateDictionary = GetTransactionByCreditAmountAndReferenceDateDictionary(ledgerTransactions);
				Dictionary<AmountRefDateKey, List<MyLedgerTransaction>> transactionsByDebitAmountAndReferenceDateDictionary = GetTransactionByDebitAmountAndReferenceDateDictionary(ledgerTransactions);
			
			foreach (var pageLine in externalPageLines)
			{
				MyLedgerTransaction matchedTransaction = null;
				if (pageLine.CreditAmount != 0)
					matchedTransaction = GetNotUsedMatchedTransaction(transactionsByDebitAmountAndReferenceDateDictionary, new AmountRefDateKey(pageLine.CreditAmount, pageLine.ReferenceDate));
				else if (pageLine.DebitAmount != 0)
					matchedTransaction = GetNotUsedMatchedTransaction(transactionsByCreditAmountAndReferenceDateDictionary, new AmountRefDateKey(pageLine.DebitAmount, pageLine.ReferenceDate));

				if (matchedTransaction != null)
				{
					AddMatchedPageLine(groupNumberCounter, pageLine);
					AddMatchedTransaction(groupNumberCounter, matchedTransaction);
					groupNumberCounter++;

					if (matchedLines.Count >= RESULT_LIMIT)
						break;
				}


			}
		}
		private void SetMatchedLinesByAmountAndAccoutingDate()
		{
			int groupNumberCounter = 1;


			Dictionary<AmountRefDateKey, List<MyLedgerTransaction>> transactionsByCreditAmountAndAccountDateDictionary = GetTransactionByCreditAmountAndAccountDateDictionary(ledgerTransactions);
			Dictionary<AmountRefDateKey, List<MyLedgerTransaction>> transactionsByDebitAmountAndAccountDateDictionary = GetTransactionByDebitAmountAndAccountDateDictionary(ledgerTransactions);

			foreach (var pageLine in externalPageLines)
			{
				MyLedgerTransaction matchedTransaction = null;
				if (pageLine.CreditAmount != 0)
					matchedTransaction = GetNotUsedMatchedTransaction(transactionsByDebitAmountAndAccountDateDictionary, new AmountRefDateKey(pageLine.CreditAmount, pageLine.ReferenceDate));
				else if (pageLine.DebitAmount != 0)
					matchedTransaction = GetNotUsedMatchedTransaction(transactionsByCreditAmountAndAccountDateDictionary, new AmountRefDateKey(pageLine.DebitAmount, pageLine.ReferenceDate));

				if (matchedTransaction != null)
				{
					AddMatchedPageLine(groupNumberCounter, pageLine);
					AddMatchedTransaction(groupNumberCounter, matchedTransaction);
					groupNumberCounter++;

					if (matchedLines.Count >= RESULT_LIMIT)
						break;
				}


			}
		}
		private void SetMatchedLinesByAmountAndReferenceDateAndAccoutingDate()
		{
			int groupNumberCounter = 1;
			
				Dictionary<AmountRefDatAccountDateKey, List<MyLedgerTransaction>> transactionsByCreditAmountAndReferenceDateAndAccountDateDictionary = GetTransactionByCreditAmountAndReferenceDateAndAccountDateDictionary(ledgerTransactions);
				Dictionary<AmountRefDatAccountDateKey, List<MyLedgerTransaction>> transactionsByDebitAmountAndReferenceDateAndAccountDateDictionary = GetTransactionByDebitAmountAndReferenceDateAndAccountDateDictionary(ledgerTransactions);

			foreach (var pageLine in externalPageLines)
			{
				MyLedgerTransaction matchedTransaction = null;
				if (pageLine.CreditAmount != 0)
					matchedTransaction = GetNotUsedMatchedTransaction(transactionsByDebitAmountAndReferenceDateAndAccountDateDictionary, new AmountRefDatAccountDateKey(pageLine.CreditAmount, pageLine.ReferenceDate, pageLine.ReferenceDate));
				else if (pageLine.DebitAmount != 0)
					matchedTransaction = GetNotUsedMatchedTransaction(transactionsByCreditAmountAndReferenceDateAndAccountDateDictionary, new AmountRefDatAccountDateKey(pageLine.DebitAmount, pageLine.ReferenceDate, pageLine.ReferenceDate));

				if (matchedTransaction != null)
				{
					AddMatchedPageLine(groupNumberCounter, pageLine);
					AddMatchedTransaction(groupNumberCounter, matchedTransaction);
					groupNumberCounter++;

					if (matchedLines.Count >= RESULT_LIMIT)
						break;
				}


			}
		}

		private void SetMatchedLinesByAmountAndReferences()
        {
            int groupNumberCounter = 1;
            Dictionary<AmountRefKey, List<MyLedgerTransaction>> groupedTransactionsDictionaryCredit_ref1 = GetTransactionsByAmountAndReference(ledgerTransactions, true, 1);
            Dictionary<AmountRefKey, List<MyLedgerTransaction>> groupedTransactionsDictionaryDebit_ref1 = GetTransactionsByAmountAndReference(ledgerTransactions, false, 1);
            Dictionary<AmountRefKey, List<MyLedgerTransaction>> groupedTransactionsDictionaryCredit_ref2 = GetTransactionsByAmountAndReference(ledgerTransactions, true, 2);
            Dictionary<AmountRefKey, List<MyLedgerTransaction>> groupedTransactionsDictionaryDebit_ref2 = GetTransactionsByAmountAndReference(ledgerTransactions, false, 2);
            Dictionary<AmountRefKey, List<MyLedgerTransaction>> groupedTransactionsDictionaryCredit_ref3 = GetTransactionsByAmountAndReference(ledgerTransactions, true, 3);
            Dictionary<AmountRefKey, List<MyLedgerTransaction>> groupedTransactionsDictionaryDebit_ref3 = GetTransactionsByAmountAndReference(ledgerTransactions, false, 3);

            foreach (var pageLine in externalPageLines.Where(d => d.Reference != null))
            {
                MyLedgerTransaction matchedTransaction = null;
                if (pageLine.CreditAmount != 0)
                {
                    matchedTransaction = GetNotUsedMatchedTransaction(groupedTransactionsDictionaryDebit_ref1, new AmountRefKey(pageLine.CreditAmount, pageLine.Reference.TrimStart('0')));
                    if (matchedTransaction == null)
                        matchedTransaction = GetNotUsedMatchedTransaction(groupedTransactionsDictionaryDebit_ref2, new AmountRefKey(pageLine.CreditAmount, pageLine.Reference.TrimStart('0')));
                    if (matchedTransaction == null)
                        matchedTransaction = GetNotUsedMatchedTransaction(groupedTransactionsDictionaryDebit_ref3, new AmountRefKey(pageLine.CreditAmount, pageLine.Reference.TrimStart('0')));
                }
                else if( pageLine.DebitAmount != 0)
				{
                    matchedTransaction = GetNotUsedMatchedTransaction(groupedTransactionsDictionaryCredit_ref1, new AmountRefKey(pageLine.DebitAmount, pageLine.Reference.TrimStart('0')));
                    if (matchedTransaction == null)
                        matchedTransaction = GetNotUsedMatchedTransaction(groupedTransactionsDictionaryCredit_ref2, new AmountRefKey(pageLine.DebitAmount, pageLine.Reference.TrimStart('0')));
                    if (matchedTransaction == null)
                        matchedTransaction = GetNotUsedMatchedTransaction(groupedTransactionsDictionaryCredit_ref3, new AmountRefKey(pageLine.DebitAmount, pageLine.Reference.TrimStart('0')));
                }

                if (matchedTransaction != null)
                {
                    AddMatchedPageLine(groupNumberCounter, pageLine);
                    AddMatchedTransaction(groupNumberCounter, matchedTransaction);
                    groupNumberCounter++;

                    if (matchedLines.Count >= RESULT_LIMIT) break;
                }
            }
        }


        private bool AddSameReferencePageLines<T>(int groupNumberCounter, List<IGrouping<T, MyPageLine>> groupedPageLinesByReference, T pageLineReference, bool haveNewItems)
        {
            List<MyPageLine> matchedPageLinesForMyRef = GetMatchedPageLines(groupedPageLinesByReference, pageLineReference);
            if (matchedPageLinesForMyRef != null && matchedPageLinesForMyRef.Count > 0)
            {
                AddMatchedPageLines(groupNumberCounter, matchedPageLinesForMyRef);
                haveNewItems = true;
            }

            return haveNewItems;
        }

        private bool AddMatchedLedger<T>(int groupNumberCounter, Dictionary<T, List<MyLedgerTransaction>> groupedTransactionsDictionary_ref1, Dictionary<T, List<MyLedgerTransaction>> groupedTransactionsDictionary_ref2, Dictionary<T, List<MyLedgerTransaction>> groupedTransactionsDictionary_ref3, MyPageLine pageLine, T pageLineReference, bool haveNewItems)
        {
            List<MyLedgerTransaction> matchedTransactions = GetMatchedLedgerTransactions(groupedTransactionsDictionary_ref1, groupedTransactionsDictionary_ref2, groupedTransactionsDictionary_ref3, pageLineReference);

            if (matchedTransactions != null && matchedTransactions.Count > 0)
            {
                AddMatchedPageLine(groupNumberCounter, pageLine); // original matched page line
                AddMatchedTransactions(groupNumberCounter, matchedTransactions);
                haveNewItems = true;
            }

            return haveNewItems;
        }

        private List<MyPageLine> GetMatchedPageLines<T>(List<IGrouping<T, MyPageLine>> groupedPageLinesByReference, T pageLineReference)
        {
            var matchedPageLinesForMyRef = groupedPageLinesByReference.Where(d => d.Key.Equals(pageLineReference)).SelectMany(d => d).ToList();
            matchedPageLinesForMyRef = matchedPageLinesForMyRef.Where(d => !matchedLines.Where(w => w.BankPageLineId != null).Select(a => a.BankPageLineId).Contains(d.Id)).ToList();
            return matchedPageLinesForMyRef;
        }

        private List<MyLedgerTransaction> GetMatchedLedgerTransactions<T>(Dictionary<T, List<MyLedgerTransaction>> groupedTransactionsDictionary_ref1, Dictionary<T, List<MyLedgerTransaction>> groupedTransactionsDictionary_ref2, Dictionary<T, List<MyLedgerTransaction>> groupedTransactionsDictionary_ref3, T pageLineReference)
        {
            List<MyLedgerTransaction> matchedTransactions = new List<MyLedgerTransaction>();
            matchedTransactions = GetNotUsedMatchedTransactions<T>(groupedTransactionsDictionary_ref1, pageLineReference);

            if (groupedTransactionsDictionary_ref2.Count > 0)
            {
                var transactions = GetNotUsedMatchedTransactions(groupedTransactionsDictionary_ref2, pageLineReference);
                if (transactions != null && transactions.Count > 0)
                    matchedTransactions = matchedTransactions.Concat(transactions).ToList();
            }
            if (groupedTransactionsDictionary_ref3.Count > 0)
            {
                var transactions = GetNotUsedMatchedTransactions(groupedTransactionsDictionary_ref3, pageLineReference);
                if (transactions != null && transactions.Count > 0)
                    matchedTransactions = matchedTransactions.Concat(transactions).ToList();
            }

            return matchedTransactions;
        }

        private List<IGrouping<string, MyPageLine>> GetGroupedPageLinesByReference()
        {
            return externalPageLines
                .Where(d=>d.Reference != null)
                .GroupBy(d => d.Reference.Trim(' '))
                .Where(d => d.Count() > 1 && d.Key != null).ToList();
        }

        private List<IGrouping<RefRefDateKey, MyPageLine>> GetGroupedPageLinesByReferenceAndRefDate()
        {
            return externalPageLines
                .GroupBy(d => new RefRefDateKey(d.Reference,d.ReferenceDate))
                .Where(d => d.Count() > 1).ToList();
        }
		private List<IGrouping<RefRefDateAccDateKey, MyPageLine>> GetGroupedPageLinesByReferenceAndRefDateAndAccDate()
		{
			return externalPageLines
				.GroupBy(d => new RefRefDateAccDateKey(d.Reference, d.ReferenceDate, d.ReferenceDate))
				.Where(d => d.Count() > 1).ToList();
		}


		private void SetMatchedLinesByReferenceDate()
        {
            int groupNumberCounter = 1;
            Dictionary<DateTime, List<MyLedgerTransaction>> transactionsByReferenceDateDictionary = GetTransactionByReferenceDateDictionary(ledgerTransactions);

            foreach (var pageLine in externalPageLines)
            {
                MyLedgerTransaction matchedTransaction;
                    matchedTransaction = GetNotUsedMatchedTransaction(transactionsByReferenceDateDictionary, pageLine.ReferenceDate);

                if (matchedTransaction != null)
                {
                    AddMatchedPageLine(groupNumberCounter, pageLine);
                    AddMatchedTransaction(groupNumberCounter, matchedTransaction);
                    groupNumberCounter++;

                    if (matchedLines.Count >= RESULT_LIMIT)
                        break;
                }


            }
        }
        private void SetMatchedLinesByAmountAndReferenceDateAndReferences()
        {
            int groupNumberCounter = 1;
            Dictionary<AmountRefRefDateKey, List<MyLedgerTransaction>> groupedTransactionsDictionaryCredit_ref1 = GetTransactionsByAmountAndReferenceDateAndReference(ledgerTransactions, true, 1);
            Dictionary<AmountRefRefDateKey, List<MyLedgerTransaction>> groupedTransactionsDictionaryDebit_ref1 = GetTransactionsByAmountAndReferenceDateAndReference(ledgerTransactions, false, 1);
            Dictionary<AmountRefRefDateKey, List<MyLedgerTransaction>> groupedTransactionsDictionaryCredit_ref2 = GetTransactionsByAmountAndReferenceDateAndReference(ledgerTransactions, true, 2);
            Dictionary<AmountRefRefDateKey, List<MyLedgerTransaction>> groupedTransactionsDictionaryDebit_ref2 = GetTransactionsByAmountAndReferenceDateAndReference(ledgerTransactions, false, 2);
            Dictionary<AmountRefRefDateKey, List<MyLedgerTransaction>> groupedTransactionsDictionaryCredit_ref3 = GetTransactionsByAmountAndReferenceDateAndReference(ledgerTransactions, true, 3);
            Dictionary<AmountRefRefDateKey, List<MyLedgerTransaction>> groupedTransactionsDictionaryDebit_ref3 = GetTransactionsByAmountAndReferenceDateAndReference(ledgerTransactions, false, 3);

            foreach (var pageLine in externalPageLines.Where(d => d.Reference != null))
            {
                MyLedgerTransaction matchedTransaction = null;
                if (pageLine.CreditAmount != 0)
                {
                    matchedTransaction = GetNotUsedMatchedTransaction(groupedTransactionsDictionaryDebit_ref1, new AmountRefRefDateKey(pageLine.CreditAmount, pageLine.Reference.TrimStart('0'), pageLine.ReferenceDate));
                    if (matchedTransaction == null)
                        matchedTransaction = GetNotUsedMatchedTransaction(groupedTransactionsDictionaryDebit_ref2, new AmountRefRefDateKey(pageLine.CreditAmount, pageLine.Reference.TrimStart('0'), pageLine.ReferenceDate));
                    if (matchedTransaction == null)
                        matchedTransaction = GetNotUsedMatchedTransaction(groupedTransactionsDictionaryDebit_ref3, new AmountRefRefDateKey(pageLine.CreditAmount, pageLine.Reference.TrimStart('0'), pageLine.ReferenceDate));
                }
                else if (pageLine.DebitAmount != 0)
				{
                    matchedTransaction = GetNotUsedMatchedTransaction(groupedTransactionsDictionaryCredit_ref1, new AmountRefRefDateKey(pageLine.DebitAmount, pageLine.Reference.TrimStart('0'), pageLine.ReferenceDate));
                    if (matchedTransaction == null)
                        matchedTransaction = GetNotUsedMatchedTransaction(groupedTransactionsDictionaryCredit_ref2, new AmountRefRefDateKey(pageLine.DebitAmount, pageLine.Reference.TrimStart('0'), pageLine.ReferenceDate));
                    if (matchedTransaction == null)
                        matchedTransaction = GetNotUsedMatchedTransaction(groupedTransactionsDictionaryCredit_ref3, new AmountRefRefDateKey(pageLine.DebitAmount, pageLine.Reference.TrimStart('0'), pageLine.ReferenceDate));
                }
                if (matchedTransaction != null)
                {
                    AddMatchedPageLine(groupNumberCounter, pageLine);
                    AddMatchedTransaction(groupNumberCounter, matchedTransaction);
                    groupNumberCounter++;
                    if (matchedLines.Count >= RESULT_LIMIT) break;
                }
            }
        }
		private void SetMatchedLinesByAmountAndAccoutingDateAndReferences()
		{
			int groupNumberCounter = 1;
			Dictionary<AmountRefRefDateKey, List<MyLedgerTransaction>> groupedTransactionsDictionaryCredit_ref1 = GetTransactionsByAmountAndAccoutingDateAndReference(ledgerTransactions, true, 1);
			Dictionary<AmountRefRefDateKey, List<MyLedgerTransaction>> groupedTransactionsDictionaryDebit_ref1 = GetTransactionsByAmountAndAccoutingDateAndReference(ledgerTransactions, false, 1);
			Dictionary<AmountRefRefDateKey, List<MyLedgerTransaction>> groupedTransactionsDictionaryCredit_ref2 = GetTransactionsByAmountAndAccoutingDateAndReference(ledgerTransactions, true, 2);
			Dictionary<AmountRefRefDateKey, List<MyLedgerTransaction>> groupedTransactionsDictionaryDebit_ref2 = GetTransactionsByAmountAndAccoutingDateAndReference(ledgerTransactions, false, 2);
			Dictionary<AmountRefRefDateKey, List<MyLedgerTransaction>> groupedTransactionsDictionaryCredit_ref3 = GetTransactionsByAmountAndAccoutingDateAndReference(ledgerTransactions, true, 3);
			Dictionary<AmountRefRefDateKey, List<MyLedgerTransaction>> groupedTransactionsDictionaryDebit_ref3 = GetTransactionsByAmountAndAccoutingDateAndReference(ledgerTransactions, false, 3);

			foreach (var pageLine in externalPageLines.Where(d => d.Reference != null))
			{
				MyLedgerTransaction matchedTransaction = null;
				if (pageLine.CreditAmount != 0)
				{
					matchedTransaction = GetNotUsedMatchedTransaction(groupedTransactionsDictionaryDebit_ref1, new AmountRefRefDateKey(pageLine.CreditAmount, pageLine.Reference.TrimStart('0'), pageLine.ReferenceDate));
					if (matchedTransaction == null)
						matchedTransaction = GetNotUsedMatchedTransaction(groupedTransactionsDictionaryDebit_ref2, new AmountRefRefDateKey(pageLine.CreditAmount, pageLine.Reference.TrimStart('0'), pageLine.ReferenceDate));
					if (matchedTransaction == null)
						matchedTransaction = GetNotUsedMatchedTransaction(groupedTransactionsDictionaryDebit_ref3, new AmountRefRefDateKey(pageLine.CreditAmount, pageLine.Reference.TrimStart('0'), pageLine.ReferenceDate));
				}
				else if (pageLine.DebitAmount != 0)
				{
					matchedTransaction = GetNotUsedMatchedTransaction(groupedTransactionsDictionaryCredit_ref1, new AmountRefRefDateKey(pageLine.DebitAmount, pageLine.Reference.TrimStart('0'), pageLine.ReferenceDate));
					if (matchedTransaction == null)
						matchedTransaction = GetNotUsedMatchedTransaction(groupedTransactionsDictionaryCredit_ref2, new AmountRefRefDateKey(pageLine.DebitAmount, pageLine.Reference.TrimStart('0'), pageLine.ReferenceDate));
					if (matchedTransaction == null)
						matchedTransaction = GetNotUsedMatchedTransaction(groupedTransactionsDictionaryCredit_ref3, new AmountRefRefDateKey(pageLine.DebitAmount, pageLine.Reference.TrimStart('0'), pageLine.ReferenceDate));
				}
				if (matchedTransaction != null)
				{
					AddMatchedPageLine(groupNumberCounter, pageLine);
					AddMatchedTransaction(groupNumberCounter, matchedTransaction);
					groupNumberCounter++;
					if (matchedLines.Count >= RESULT_LIMIT) break;
				}
			}
		}
		private void SetMatchedLinesByAmountAndReferenceDateAndAccoutingDateAndReferences()
		{
			int groupNumberCounter = 1;
			Dictionary<AmountRefRefDateAccDateKey, List<MyLedgerTransaction>> groupedTransactionsDictionaryCredit_ref1 = GetTransactionsByAmountAndReferenceDateeAndAccoutingDateAndReference(ledgerTransactions, true, 1);
			Dictionary<AmountRefRefDateAccDateKey, List<MyLedgerTransaction>> groupedTransactionsDictionaryDebit_ref1 = GetTransactionsByAmountAndReferenceDateeAndAccoutingDateAndReference(ledgerTransactions, false, 1);
			Dictionary<AmountRefRefDateAccDateKey, List<MyLedgerTransaction>> groupedTransactionsDictionaryCredit_ref2 = GetTransactionsByAmountAndReferenceDateeAndAccoutingDateAndReference(ledgerTransactions, true, 2);
			Dictionary<AmountRefRefDateAccDateKey, List<MyLedgerTransaction>> groupedTransactionsDictionaryDebit_ref2 = GetTransactionsByAmountAndReferenceDateeAndAccoutingDateAndReference(ledgerTransactions, false, 2);
			Dictionary<AmountRefRefDateAccDateKey, List<MyLedgerTransaction>> groupedTransactionsDictionaryCredit_ref3 = GetTransactionsByAmountAndReferenceDateeAndAccoutingDateAndReference(ledgerTransactions, true, 3);
			Dictionary<AmountRefRefDateAccDateKey, List<MyLedgerTransaction>> groupedTransactionsDictionaryDebit_ref3 = GetTransactionsByAmountAndReferenceDateeAndAccoutingDateAndReference(ledgerTransactions, false, 3);

			foreach (var pageLine in externalPageLines.Where(d => d.Reference != null))
			{
				MyLedgerTransaction matchedTransaction = null;
				if (pageLine.CreditAmount != 0)
				{
					matchedTransaction = GetNotUsedMatchedTransaction(groupedTransactionsDictionaryDebit_ref1, new AmountRefRefDateAccDateKey(pageLine.CreditAmount, pageLine.Reference.TrimStart('0'), pageLine.ReferenceDate, pageLine.ReferenceDate));
					if (matchedTransaction == null)
						matchedTransaction = GetNotUsedMatchedTransaction(groupedTransactionsDictionaryDebit_ref2, new AmountRefRefDateAccDateKey(pageLine.CreditAmount, pageLine.Reference.TrimStart('0'), pageLine.ReferenceDate, pageLine.ReferenceDate));
					if (matchedTransaction == null)
						matchedTransaction = GetNotUsedMatchedTransaction(groupedTransactionsDictionaryDebit_ref3, new AmountRefRefDateAccDateKey(pageLine.CreditAmount, pageLine.Reference.TrimStart('0'), pageLine.ReferenceDate, pageLine.ReferenceDate));
				}
				else if (pageLine.DebitAmount != 0)
				{
					matchedTransaction = GetNotUsedMatchedTransaction(groupedTransactionsDictionaryCredit_ref1, new AmountRefRefDateAccDateKey(pageLine.DebitAmount, pageLine.Reference.TrimStart('0'), pageLine.ReferenceDate, pageLine.ReferenceDate));
					if (matchedTransaction == null)
						matchedTransaction = GetNotUsedMatchedTransaction(groupedTransactionsDictionaryCredit_ref2, new AmountRefRefDateAccDateKey(pageLine.DebitAmount, pageLine.Reference.TrimStart('0'), pageLine.ReferenceDate, pageLine.ReferenceDate));
					if (matchedTransaction == null)
						matchedTransaction = GetNotUsedMatchedTransaction(groupedTransactionsDictionaryCredit_ref3, new AmountRefRefDateAccDateKey(pageLine.DebitAmount, pageLine.Reference.TrimStart('0'), pageLine.ReferenceDate, pageLine.ReferenceDate));
				}
				if (matchedTransaction != null)
				{
					AddMatchedPageLine(groupNumberCounter, pageLine);
					AddMatchedTransaction(groupNumberCounter, matchedTransaction);
					groupNumberCounter++;
					if (matchedLines.Count >= RESULT_LIMIT) break;
				}
			}
		}
		private void SetMatchedLinesByReferences()
        {
            int groupNumberCounter = 1;
            Dictionary<string, List<MyLedgerTransaction>> groupedTransactionsDictionary_ref1 = GetTransactionsByReference(ledgerTransactions, 1);
            Dictionary<string, List<MyLedgerTransaction>> groupedTransactionsDictionary_ref2 = GetTransactionsByReference(ledgerTransactions, 2);
            Dictionary<string, List<MyLedgerTransaction>> groupedTransactionsDictionary_ref3 = GetTransactionsByReference(ledgerTransactions, 3);

            List<IGrouping<string, MyPageLine>> groupedPageLinesByReference = GetGroupedPageLinesByReference();

            foreach (var pageLine in externalPageLines.Where(d => d.Reference != null))
            {
                string pageLineReference = pageLine.Reference.TrimStart('0').Trim(' ');
                bool haveNewItems = false;

                haveNewItems = AddMatchedLedger(groupNumberCounter, groupedTransactionsDictionary_ref1, groupedTransactionsDictionary_ref2, groupedTransactionsDictionary_ref3, pageLine, pageLineReference, haveNewItems);

                if (AllowOnlySingleBankPageLineOnEachReconcileGroup)
                {
                    if (haveNewItems)
                        AddMatchedPageLines(groupNumberCounter, new List<MyPageLine> { pageLine });
                }
                else
                    haveNewItems = AddSameReferencePageLines(groupNumberCounter, groupedPageLinesByReference, pageLineReference, haveNewItems);

                

                if (haveNewItems == true) groupNumberCounter++;
                if (matchedLines.Count >= RESULT_LIMIT) break;
            }

        }

        private void SetMatchedLinesByReferenceDateAndReferences()
        {
            int groupNumberCounter = 1;
            Dictionary<RefRefDateKey, List<MyLedgerTransaction>> groupedTransactionsDictionary_ref1 = GetTransactionsByReferenceDateAndReference(ledgerTransactions, 1);
            Dictionary<RefRefDateKey, List<MyLedgerTransaction>> groupedTransactionsDictionary_ref2 = GetTransactionsByReferenceDateAndReference(ledgerTransactions, 2);
            Dictionary<RefRefDateKey, List<MyLedgerTransaction>> groupedTransactionsDictionary_ref3 = GetTransactionsByReferenceDateAndReference(ledgerTransactions, 3);

            List<IGrouping<RefRefDateKey, MyPageLine>> groupedPageLinesByReference = GetGroupedPageLinesByReferenceAndRefDate();

            foreach (var pageLine in externalPageLines.Where(d => d.Reference != null))
            {
                string pageLineReference = pageLine.Reference.TrimStart('0').Trim(' ');
                bool haveNewItems = false;

                haveNewItems = AddMatchedLedger(groupNumberCounter, groupedTransactionsDictionary_ref1, groupedTransactionsDictionary_ref2, groupedTransactionsDictionary_ref3, pageLine, new RefRefDateKey(pageLineReference,pageLine.ReferenceDate), haveNewItems);
                haveNewItems = AddSameReferencePageLines(groupNumberCounter, groupedPageLinesByReference, new RefRefDateKey(pageLineReference, pageLine.ReferenceDate), haveNewItems);

                if (haveNewItems == true) groupNumberCounter++;
                if (matchedLines.Count >= RESULT_LIMIT) break;
            }



            //foreach (var pageLine in externalPageLines.Where(d => d.Reference != null))
            //{
            //    string pageLineReference = pageLine.Reference.TrimStart('0');

            //    MyLedgerTransaction matchedTransaction;

            //    matchedTransaction = GetNotUsedMatchedTransaction(groupedTransactionsDictionary_ref1, new RefRefDateKey(pageLineReference, pageLine.ReferenceDate));
            //    if (matchedTransaction == null)
            //        matchedTransaction = GetNotUsedMatchedTransaction(groupedTransactionsDictionary_ref2, new RefRefDateKey(pageLineReference, pageLine.ReferenceDate));
            //    if (matchedTransaction == null)
            //        matchedTransaction = GetNotUsedMatchedTransaction(groupedTransactionsDictionary_ref3, new RefRefDateKey(pageLineReference, pageLine.ReferenceDate));


            //    if (matchedTransaction != null)
            //    {
            //        AddMatchedPageLine(groupNumberCounter, pageLine);
            //        AddMatchedTransaction(groupNumberCounter, matchedTransaction);
            //        groupNumberCounter++;
            //        if (matchedLines.Count >= RESULT_LIMIT) break;
            //    }
            //}
        }
		private void SetMatchedLinesByAccoutingDateAndReferences()
		{
			int groupNumberCounter = 1;
			Dictionary<RefRefDateKey, List<MyLedgerTransaction>> groupedTransactionsDictionary_ref1 = GetTransactionsByAccoutingDateAndReference(ledgerTransactions, 1);
			Dictionary<RefRefDateKey, List<MyLedgerTransaction>> groupedTransactionsDictionary_ref2 = GetTransactionsByAccoutingDateAndReference(ledgerTransactions, 2);
			Dictionary<RefRefDateKey, List<MyLedgerTransaction>> groupedTransactionsDictionary_ref3 = GetTransactionsByAccoutingDateAndReference(ledgerTransactions, 3);

			List<IGrouping<RefRefDateKey, MyPageLine>> groupedPageLinesByReference = GetGroupedPageLinesByReferenceAndRefDate();

			foreach (var pageLine in externalPageLines.Where(d => d.Reference != null))
			{
				string pageLineReference = pageLine.Reference.TrimStart('0').Trim(' ');
				bool haveNewItems = false;

				haveNewItems = AddMatchedLedger(groupNumberCounter, groupedTransactionsDictionary_ref1, groupedTransactionsDictionary_ref2, groupedTransactionsDictionary_ref3, pageLine, new RefRefDateKey(pageLineReference, pageLine.ReferenceDate), haveNewItems);
				haveNewItems = AddSameReferencePageLines(groupNumberCounter, groupedPageLinesByReference, new RefRefDateKey(pageLineReference, pageLine.ReferenceDate), haveNewItems);

				if (haveNewItems == true) groupNumberCounter++;
				if (matchedLines.Count >= RESULT_LIMIT) break;
			}

		}
		private void SetMatchedLinesByReferenceDateAndAccoutingDateAndReferences()
		{
			int groupNumberCounter = 1;
			Dictionary<RefRefDateAccDateKey, List<MyLedgerTransaction>> groupedTransactionsDictionary_ref1 = GetTransactionsByReferenceDateAndAccotingDateAndReference(ledgerTransactions, 1);
			Dictionary<RefRefDateAccDateKey, List<MyLedgerTransaction>> groupedTransactionsDictionary_ref2 = GetTransactionsByReferenceDateAndAccotingDateAndReference(ledgerTransactions, 2);
			Dictionary<RefRefDateAccDateKey, List<MyLedgerTransaction>> groupedTransactionsDictionary_ref3 = GetTransactionsByReferenceDateAndAccotingDateAndReference(ledgerTransactions, 3);

			List<IGrouping<RefRefDateAccDateKey, MyPageLine>> groupedPageLinesByReference = GetGroupedPageLinesByReferenceAndRefDateAndAccDate();

			foreach (var pageLine in externalPageLines.Where(d => d.Reference != null))
			{
				string pageLineReference = pageLine.Reference.TrimStart('0').Trim(' ');
				bool haveNewItems = false;

				haveNewItems = AddMatchedLedger(groupNumberCounter, groupedTransactionsDictionary_ref1, groupedTransactionsDictionary_ref2, groupedTransactionsDictionary_ref3, pageLine, new RefRefDateAccDateKey(pageLineReference, pageLine.ReferenceDate,pageLine.ReferenceDate), haveNewItems);
				haveNewItems = AddSameReferencePageLines(groupNumberCounter, groupedPageLinesByReference, new RefRefDateAccDateKey(pageLineReference, pageLine.ReferenceDate, pageLine.ReferenceDate), haveNewItems);

				if (haveNewItems == true) groupNumberCounter++;
				if (matchedLines.Count >= RESULT_LIMIT) break;
			}

		}

		private static Dictionary<AmountRefKey, List<MyLedgerTransaction>> GetTransactionsByAmountAndReference(List<MyLedgerTransaction> ledgerTransactions, bool isCreditAmount, int refNumber)
        {
            List<IGrouping<AmountRefKey, MyLedgerTransaction>> groupedTransactions_ref3 = new List<IGrouping<AmountRefKey, MyLedgerTransaction>>();
            if (refNumber == 1)
                groupedTransactions_ref3 = ledgerTransactions.GroupBy(x => new AmountRefKey(isCreditAmount ? x.ForeignAmountCredit : x.ForeignAmountDebit, x.Reference1?.TrimStart('0').Trim(' '))).ToList();
            else if (refNumber == 2)
                groupedTransactions_ref3 = ledgerTransactions.GroupBy(x => new AmountRefKey(isCreditAmount ? x.ForeignAmountCredit : x.ForeignAmountDebit, x.Reference2?.TrimStart('0').Trim(' '))).ToList();
            else if (refNumber == 3)
                groupedTransactions_ref3 = ledgerTransactions.GroupBy(x => new AmountRefKey(isCreditAmount ? x.ForeignAmountCredit : x.ForeignAmountDebit, x.Reference3?.TrimStart('0').Trim(' '))).ToList();

            var groupedTransactionsDictionary_ref3 = groupedTransactions_ref3.ToDictionary(d => d.Key, d => d.ToList()); // Key: Amount, Value: List of transaction
            return groupedTransactionsDictionary_ref3;
        }
        private static Dictionary<string, List<MyLedgerTransaction>> GetTransactionsByReference(List<MyLedgerTransaction> ledgerTransactions, int refNumber)
        {
            var groupedTransactions_ref3 = new List<IGrouping<string, MyLedgerTransaction>>();
            if (refNumber == 1)
                groupedTransactions_ref3 = ledgerTransactions.GroupBy(x => x.Reference1?.TrimStart('0').Trim(' ')).ToList();
            else if (refNumber == 2)
                groupedTransactions_ref3 = ledgerTransactions.GroupBy(x => x.Reference2?.TrimStart('0').Trim(' ')).ToList();
            else if (refNumber == 3)
                groupedTransactions_ref3 = ledgerTransactions.GroupBy(x => x.Reference3?.TrimStart('0').Trim(' ')).ToList();

            var groupedTransactionsDictionary_ref3 = groupedTransactions_ref3.Where(d=>d.Key != null).ToDictionary(d => d.Key, d => d.ToList()); // Key: Amount, Value: List of transaction
            return groupedTransactionsDictionary_ref3;
        }

        private static Dictionary<AmountRefRefDateKey, List<MyLedgerTransaction>> GetTransactionsByAmountAndReferenceDateAndReference(List<MyLedgerTransaction> ledgerTransactions, bool isCreditAmount, int refNumber)
        {
            List<IGrouping<AmountRefRefDateKey, MyLedgerTransaction>> groupedTransactions_ref3 = new List<IGrouping<AmountRefRefDateKey, MyLedgerTransaction>>();
            if (refNumber == 1)
                groupedTransactions_ref3 = ledgerTransactions.GroupBy(x => new AmountRefRefDateKey(isCreditAmount ? x.ForeignAmountCredit : x.ForeignAmountDebit, x.Reference1?.TrimStart('0').Trim(' '), x.DocumentDate)).ToList();
            else if (refNumber == 2)
                groupedTransactions_ref3 = ledgerTransactions.GroupBy(x => new AmountRefRefDateKey(isCreditAmount ? x.ForeignAmountCredit : x.ForeignAmountDebit, x.Reference2?.TrimStart('0').Trim(' '), x.DocumentDate)).ToList();
            else if (refNumber == 3)
                groupedTransactions_ref3 = ledgerTransactions.GroupBy(x => new AmountRefRefDateKey(isCreditAmount ? x.ForeignAmountCredit : x.ForeignAmountDebit, x.Reference3?.TrimStart('0').Trim(' '), x.DocumentDate)).ToList();

            var groupedTransactionsDictionary_ref3 = groupedTransactions_ref3.ToDictionary(d => d.Key, d => d.ToList()); // Key: Amount, Value: List of transaction
            return groupedTransactionsDictionary_ref3;
        }
		private static Dictionary<AmountRefRefDateKey, List<MyLedgerTransaction>> GetTransactionsByAmountAndAccoutingDateAndReference(List<MyLedgerTransaction> ledgerTransactions, bool isCreditAmount, int refNumber)
		{
			List<IGrouping<AmountRefRefDateKey, MyLedgerTransaction>> groupedTransactions_ref3 = new List<IGrouping<AmountRefRefDateKey, MyLedgerTransaction>>();
			if (refNumber == 1)
				groupedTransactions_ref3 = ledgerTransactions.GroupBy(x => new AmountRefRefDateKey(isCreditAmount ? x.ForeignAmountCredit : x.ForeignAmountDebit, x.Reference1?.TrimStart('0').Trim(' '), x.AccountingDate)).ToList();
			else if (refNumber == 2)
				groupedTransactions_ref3 = ledgerTransactions.GroupBy(x => new AmountRefRefDateKey(isCreditAmount ? x.ForeignAmountCredit : x.ForeignAmountDebit, x.Reference2?.TrimStart('0').Trim(' '), x.AccountingDate)).ToList();
			else if (refNumber == 3)
				groupedTransactions_ref3 = ledgerTransactions.GroupBy(x => new AmountRefRefDateKey(isCreditAmount ? x.ForeignAmountCredit : x.ForeignAmountDebit, x.Reference3?.TrimStart('0').Trim(' '), x.AccountingDate)).ToList();

			var groupedTransactionsDictionary_ref3 = groupedTransactions_ref3.ToDictionary(d => d.Key, d => d.ToList()); // Key: Amount, Value: List of transaction
			return groupedTransactionsDictionary_ref3;
		}
		private static Dictionary<AmountRefRefDateAccDateKey, List<MyLedgerTransaction>> GetTransactionsByAmountAndReferenceDateeAndAccoutingDateAndReference(List<MyLedgerTransaction> ledgerTransactions, bool isCreditAmount, int refNumber)
		{
			List<IGrouping<AmountRefRefDateAccDateKey, MyLedgerTransaction>> groupedTransactions_ref3 = new List<IGrouping<AmountRefRefDateAccDateKey, MyLedgerTransaction>>();
			if (refNumber == 1)
				groupedTransactions_ref3 = ledgerTransactions.GroupBy(x => new AmountRefRefDateAccDateKey(isCreditAmount ? x.ForeignAmountCredit : x.ForeignAmountDebit, x.Reference1?.TrimStart('0').Trim(' '), x.DocumentDate, x.AccountingDate)).ToList();
			else if (refNumber == 2)
				groupedTransactions_ref3 = ledgerTransactions.GroupBy(x => new AmountRefRefDateAccDateKey(isCreditAmount ? x.ForeignAmountCredit : x.ForeignAmountDebit, x.Reference2?.TrimStart('0').Trim(' '), x.DocumentDate, x.AccountingDate)).ToList();
			else if (refNumber == 3)
				groupedTransactions_ref3 = ledgerTransactions.GroupBy(x => new AmountRefRefDateAccDateKey(isCreditAmount ? x.ForeignAmountCredit : x.ForeignAmountDebit, x.Reference3?.TrimStart('0').Trim(' '), x.DocumentDate, x.AccountingDate)).ToList();

			var groupedTransactionsDictionary_ref3 = groupedTransactions_ref3.ToDictionary(d => d.Key, d => d.ToList()); // Key: Amount, Value: List of transaction
			return groupedTransactionsDictionary_ref3;
		}
		private static Dictionary<RefRefDateKey, List<MyLedgerTransaction>> GetTransactionsByReferenceDateAndReference(List<MyLedgerTransaction> ledgerTransactions, int refNumber)
        {
            List<IGrouping<RefRefDateKey, MyLedgerTransaction>> groupedTransactions_ref3 = new List<IGrouping<RefRefDateKey, MyLedgerTransaction>>();
            if (refNumber == 1)
                groupedTransactions_ref3 = ledgerTransactions.GroupBy(x => new RefRefDateKey(x.Reference1?.TrimStart('0').Trim(' '), x.DocumentDate)).ToList();
            else if (refNumber == 2)
                groupedTransactions_ref3 = ledgerTransactions.GroupBy(x => new RefRefDateKey(x.Reference2?.TrimStart('0').Trim(' '), x.DocumentDate)).ToList();
            else if (refNumber == 3)
                groupedTransactions_ref3 = ledgerTransactions.GroupBy(x => new RefRefDateKey(x.Reference3?.TrimStart('0').Trim(' '), x.DocumentDate)).ToList();

            var groupedTransactionsDictionary_ref3 = groupedTransactions_ref3.ToDictionary(d => d.Key, d => d.ToList()); // Key: Amount, Value: List of transaction
            return groupedTransactionsDictionary_ref3;
        }
		private static Dictionary<RefRefDateKey, List<MyLedgerTransaction>> GetTransactionsByAccoutingDateAndReference(List<MyLedgerTransaction> ledgerTransactions, int refNumber)
		{
			List<IGrouping<RefRefDateKey, MyLedgerTransaction>> groupedTransactions_ref3 = new List<IGrouping<RefRefDateKey, MyLedgerTransaction>>();
			if (refNumber == 1)
				groupedTransactions_ref3 = ledgerTransactions.GroupBy(x => new RefRefDateKey(x.Reference1?.TrimStart('0').Trim(' '), x.AccountingDate)).ToList();
			else if (refNumber == 2)
				groupedTransactions_ref3 = ledgerTransactions.GroupBy(x => new RefRefDateKey(x.Reference2?.TrimStart('0').Trim(' '), x.AccountingDate)).ToList();
			else if (refNumber == 3)
				groupedTransactions_ref3 = ledgerTransactions.GroupBy(x => new RefRefDateKey(x.Reference3?.TrimStart('0').Trim(' '), x.AccountingDate)).ToList();

			var groupedTransactionsDictionary_ref3 = groupedTransactions_ref3.ToDictionary(d => d.Key, d => d.ToList()); // Key: Amount, Value: List of transaction
			return groupedTransactionsDictionary_ref3;
		}
		private static Dictionary<RefRefDateAccDateKey, List<MyLedgerTransaction>> GetTransactionsByReferenceDateAndAccotingDateAndReference(List<MyLedgerTransaction> ledgerTransactions, int refNumber)
		{
			List<IGrouping<RefRefDateAccDateKey, MyLedgerTransaction>> groupedTransactions_ref3 = new List<IGrouping<RefRefDateAccDateKey, MyLedgerTransaction>>();
			if (refNumber == 1)
				groupedTransactions_ref3 = ledgerTransactions.GroupBy(x => new RefRefDateAccDateKey(x.Reference1?.TrimStart('0').Trim(' '), x.DocumentDate, x.AccountingDate)).ToList();
			else if (refNumber == 2)
				groupedTransactions_ref3 = ledgerTransactions.GroupBy(x => new RefRefDateAccDateKey(x.Reference2?.TrimStart('0').Trim(' '), x.DocumentDate, x.AccountingDate)).ToList();
			else if (refNumber == 3)
				groupedTransactions_ref3 = ledgerTransactions.GroupBy(x => new RefRefDateAccDateKey(x.Reference3?.TrimStart('0').Trim(' '), x.DocumentDate, x.AccountingDate)).ToList();

			var groupedTransactionsDictionary_ref3 = groupedTransactions_ref3.ToDictionary(d => d.Key, d => d.ToList()); // Key: Amount, Value: List of transaction
			return groupedTransactionsDictionary_ref3;
		}
		private static Dictionary<AmountRefDateKey, List<MyLedgerTransaction>> GetTransactionByCreditAmountAndReferenceDateDictionary(List<MyLedgerTransaction> ledgerTransactions)
        {
            var groupedTransactions = ledgerTransactions.GroupBy(x => new AmountRefDateKey(x.ForeignAmountCredit, x.DocumentDate));
            var groupedTransactionsList = groupedTransactions.ToList();
            var groupedTransactionsDictionary = groupedTransactionsList.ToDictionary(d => d.Key, d => d.ToList()); // Key: Amount, Value: List of transaction
            return groupedTransactionsDictionary;
        }
		private static Dictionary<AmountRefDateKey, List<MyLedgerTransaction>> GetTransactionByCreditAmountAndAccountDateDictionary(List<MyLedgerTransaction> ledgerTransactions)
		{
			var groupedTransactions = ledgerTransactions.GroupBy(x => new AmountRefDateKey(x.ForeignAmountCredit, x.AccountingDate));
			var groupedTransactionsList = groupedTransactions.ToList();
			var groupedTransactionsDictionary = groupedTransactionsList.ToDictionary(d => d.Key, d => d.ToList()); // Key: Amount, Value: List of transaction
			return groupedTransactionsDictionary;
		}
		private static Dictionary<AmountRefDatAccountDateKey, List<MyLedgerTransaction>> GetTransactionByCreditAmountAndReferenceDateAndAccountDateDictionary(List<MyLedgerTransaction> ledgerTransactions)
		{
			var groupedTransactions = ledgerTransactions.GroupBy(x => new AmountRefDatAccountDateKey(x.ForeignAmountCredit,x.DocumentDate, x.AccountingDate));
			var groupedTransactionsList = groupedTransactions.ToList();
			var groupedTransactionsDictionary = groupedTransactionsList.ToDictionary(d => d.Key, d => d.ToList()); // Key: Amount, Value: List of transaction
			return groupedTransactionsDictionary;
		}
		private static Dictionary<DateTime, List<MyLedgerTransaction>> GetTransactionByReferenceDateDictionary(List<MyLedgerTransaction> ledgerTransactions)
        {
            var groupedTransactions = ledgerTransactions.GroupBy(x => x.DocumentDate);
            var groupedTransactionsList = groupedTransactions.ToList();
            var groupedTransactionsDictionary = groupedTransactionsList.ToDictionary(d => d.Key, d => d.ToList()); // Key: ref date, Value: List of transaction
            return groupedTransactionsDictionary;
        }
        private static Dictionary<AmountRefDateKey, List<MyLedgerTransaction>> GetTransactionByDebitAmountAndReferenceDateDictionary(List<MyLedgerTransaction> ledgerTransactions)
        {
            var groupedTransactions = ledgerTransactions.GroupBy(x => new AmountRefDateKey(x.ForeignAmountDebit, x.DocumentDate));
            var groupedTransactionsList = groupedTransactions.ToList();
            var groupedTransactionsDictionary = groupedTransactionsList.ToDictionary(d => d.Key, d => d.ToList()); // Key: Amount, Value: List of transaction
            return groupedTransactionsDictionary;
        }
		private static Dictionary<AmountRefDateKey, List<MyLedgerTransaction>> GetTransactionByDebitAmountAndAccountDateDictionary(List<MyLedgerTransaction> ledgerTransactions)
		{
			var groupedTransactions = ledgerTransactions.GroupBy(x => new AmountRefDateKey(x.ForeignAmountDebit, x.AccountingDate));
			var groupedTransactionsList = groupedTransactions.ToList();
			var groupedTransactionsDictionary = groupedTransactionsList.ToDictionary(d => d.Key, d => d.ToList()); // Key: Amount, Value: List of transaction
			return groupedTransactionsDictionary;
		}
		private static Dictionary<AmountRefDatAccountDateKey, List<MyLedgerTransaction>> GetTransactionByDebitAmountAndReferenceDateAndAccountDateDictionary(List<MyLedgerTransaction> ledgerTransactions)
		{
			var groupedTransactions = ledgerTransactions.GroupBy(x => new AmountRefDatAccountDateKey(x.ForeignAmountDebit,x.DocumentDate, x.AccountingDate));
			var groupedTransactionsList = groupedTransactions.ToList();
			var groupedTransactionsDictionary = groupedTransactionsList.ToDictionary(d => d.Key, d => d.ToList()); // Key: Amount, Value: List of transaction
			return groupedTransactionsDictionary;
		}
		private List<ReconcileExternalPageLineList> GetPageLinesFromMatchedLines()
        {
            var accountingContext = AccountingContext.GetContext(tenant);
            ReconcileExternalPageLineListQueryService pageQuery = new ReconcileExternalPageLineListQueryService(accountingContext);
            var pageLinesIds2 = matchedLines.Where(d => d.BankPageLineId != null).Select(d => d.BankPageLineId).ToList();
            var externalPageLines = pageQuery.GetPageLinesByIds(pageLinesIds2);
            externalPageLines.ForEach(line => { line.GroupHash = matchedLines.Find(d => d.BankPageLineId == line.Id).GroupNumber; });

            return externalPageLines.OrderBy(d => d.GroupHash).ToList();
        }

        private List<LedgerTransactionList> GetLedgerTransactionsFromMatchedLines()
        {
            var accountingContext = AccountingContext.GetContext(tenant);
            LedgerTransactionListQueryService query = new LedgerTransactionListQueryService(accountingContext);
            var transactionsLinesIds2 = matchedLines.Where(d => d.LedgerTransactionId != null).Select(d => d.LedgerTransactionId).ToList();
            var transactions = query.GetTransactionsByIds(transactionsLinesIds2);

            transactions.ForEach(line => { line.GroupHash = matchedLines.Find(d => d.LedgerTransactionId == line.Id).GroupNumber; });
            return transactions.OrderBy(d => d.GroupHash).ToList();
        }

        private Dictionary<decimal, List<MyLedgerTransaction>> GetTransactionsByCreditAmountDictionary(List<MyLedgerTransaction> ledgerTransactions)
        {
            var groupebByAmountTransactions = ledgerTransactions.GroupBy(x => x.ForeignAmountCredit).ToList();
            var transactionsByAmountDictionary = groupebByAmountTransactions.ToDictionary(d => d.Key, d => d.ToList()); // Key: Amount, Value: List of transaction
            return transactionsByAmountDictionary;
        }
        private Dictionary<decimal, List<MyLedgerTransaction>> GetTransactionsByDebitAmountDictionary(List<MyLedgerTransaction> ledgerTransactions)
        {
            var groupebByAmountTransactions = ledgerTransactions.GroupBy(x => x.ForeignAmountDebit).ToList();
            var transactionsByAmountDictionary = groupebByAmountTransactions.ToDictionary(d => d.Key, d => d.ToList()); // Key: Amount, Value: List of transaction
            return transactionsByAmountDictionary;
        }

		////private MyLedgerTransaction GetNotUsedMatchedTransaction(Dictionary<decimal, List<MyLedgerTransaction>> groupedTransactionsDictionary, MyPageLine pageLine)
		////{
		////    groupedTransactionsDictionary.TryGetValue(pageLine.Amount, out List<MyLedgerTransaction> pageLineMatchedTransactions);

		////    // get not used transaction matched with page line
		////    var matchedTransaction = pageLineMatchedTransactions
		////        .FirstOrDefault(trans =>
		////            !matchedLines.Select(d => d.LedgerTransactionId).Contains(trans.Id)
		////        );
		////    return matchedTransaction;
		////}
		//private MyLedgerTransaction GetNotUsedMatchedTransaction(Dictionary<decimal, List<MyLedgerTransaction>> groupedTransactionsDictionary, decimal amount)
		//{
		//    groupedTransactionsDictionary.TryGetValue(amount, out List<MyLedgerTransaction> pageLineMatchedTransactions);

		//    // get not used transaction matched with page line
		//    if(pageLineMatchedTransactions != null)
		//    return pageLineMatchedTransactions
		//        .FirstOrDefault(trans =>
		//            !matchedLines.Select(d => d.LedgerTransactionId).Contains(trans.Id)
		//        );
		//    else
		//        return null;
		//}
		private MyLedgerTransaction GetNotUsedMatchedTransaction<T>(Dictionary<T, List<MyLedgerTransaction>> groupedTransactionsDictionary, T key)
        {
            groupedTransactionsDictionary.TryGetValue(key, out List<MyLedgerTransaction> pageLineMatchedTransactions);

            // get not used transaction matched with page line
            if (pageLineMatchedTransactions != null && Convert.ToDecimal(key) > 0)
                return pageLineMatchedTransactions
                    .FirstOrDefault(trans =>
                        !matchedLines.Select(d => d.LedgerTransactionId).Contains(trans.Id)
                    );
            else
                return null;
        }
        private List<MyLedgerTransaction> GetNotUsedMatchedTransactions<T>(Dictionary<T, List<MyLedgerTransaction>> groupedTransactionsDictionary, T key)
        {
            groupedTransactionsDictionary.TryGetValue(key, out List<MyLedgerTransaction> pageLineMatchedTransactions);

            // get not used transaction matched with page line
            if (pageLineMatchedTransactions != null)
                return pageLineMatchedTransactions
                    .Where(trans =>
                        !matchedLines.Select(d => d.LedgerTransactionId).Contains(trans.Id)
                    ).ToList();
            else
                return new List<MyLedgerTransaction>();
        }
        private void AddMatchedTransaction(int groupNumberCounter, MyLedgerTransaction transaction)
        {
            matchedLines.Add(new MatchingLine() { LedgerTransactionId = transaction.Id, BankPageLineId = null, GroupNumber = groupNumberCounter });
        }
        private void AddMatchedTransactions(int groupNumberCounter, List<MyLedgerTransaction> transactions)
        {
            transactions.ForEach(transaction =>
            {
                matchedLines.Add(new MatchingLine() { LedgerTransactionId = transaction.Id, BankPageLineId = null, GroupNumber = groupNumberCounter });
            });
        }

        private void AddMatchedPageLine(int groupNumberCounter, MyPageLine pageLine)
        {
            matchedLines.Add(new MatchingLine() { LedgerTransactionId = null, BankPageLineId = pageLine.Id, GroupNumber = groupNumberCounter });
        }
        private void AddMatchedPageLines(int groupNumberCounter, List<MyPageLine> pageLines)
        {
            //matchedLines.Add(new MatchingLine() { LedgerTransactionId = null, BankPageLineId = pageLines.FirstOrDefault().Id, GroupNumber = groupNumberCounter });

            pageLines.ForEach(pageLine =>
            {
                matchedLines.Add(new MatchingLine() { LedgerTransactionId = null, BankPageLineId = pageLine.Id, GroupNumber = groupNumberCounter });
            });
        }
        private void ValidateParameters(AutoExternalReconcileArgs args)
        {
            if (args.AmountReconcile == false && args.ReferenceReconcile == false && args.RefDateReconcile == false && args.AccoutingDateReconcile == false)
            {
                var msg = TextCodesTranslator.TranslateText("ExternalReconciliation.O.SelectOneAutoRecoMethod", tenant, LoggedContactResolver.GetLoggedContactShowLocal(tenant));
                throw new ApplicationException(msg);
            }
            if (args.ObjectTableId == null) throw new ApplicationException("args.objectTableId is not provided !!!");
            if (args.GLAccountId == null) throw new ApplicationException("gl Account is not provided !!!");
        }

        List<MyPageLine> GetFilteredPageLines(AutoExternalReconcileArgs args)
        {
            var accountingContext = AccountingContext.GetContext(tenant);
            ReconcileExternalPageListQueryService query = new ReconcileExternalPageListQueryService(accountingContext);
            IQueryable<ReconcileExternalPageLineList> iQuerableList = query.getPageLinesByFilter(args.BankPageLineQueryOperations, args.ObjectTableId, args.EntityId, tenant);
            iQuerableList = iQuerableList.OrderByDescending(a => a.ReferenceDate);
            IQueryable<MyPageLine> pageLinesDTO = (from a in iQuerableList
                                                                      select new MyPageLine()
                                                                      {
                                                                          Id = a.Id,
                                                                          Amount = a.Amount,
                                                                          CreditAmount = a.CreditAmount,
                                                                          DebitAmount = a.DebitAmount,
                                                                          Reference = a.Reference,
                                                                          ReferenceDate = a.ReferenceDate,
                                                                      });
            return pageLinesDTO.ToList();
        }

        List<MyLedgerTransaction> GetFilteredLedgerTransactions(AutoExternalReconcileArgs args)
        {
            var accountingContext = AccountingContext.GetContext(tenant);
            LedgerTransactionListQueryService transactionQuery = new LedgerTransactionListQueryService(accountingContext);
            List<LedgerTransactionList> openReconciliation = transactionQuery.GetOpenLedgerTransactions(args.TransactionQueryOperations, args.GLAccountId, args.TransferGLAccountId, tenant);
            List<MyLedgerTransaction> linesDTO = (from a in openReconciliation
                                                        select new MyLedgerTransaction()
                                                            {
                                                                Id = a.Id,
                                                                DocumentDate = a.DocumentDate,
                                                                Reference1 = a.Reference1,
                                                                Reference2 = a.Reference2,
                                                                Reference3 = a.Reference3,
                                                                OpenAmount = a.OpenAmount,
                                                                Amount = a.ForeignAmountDebit == 0 ? a.ForeignAmountCredit : a.ForeignAmountDebit,
                                                                ForeignAmountCredit = a.ForeignAmountCredit,
                                                                ForeignAmountDebit = a.ForeignAmountDebit,
															    AccountingDate = a.AccountingDate,
														}).ToList();
            return linesDTO;
        }


        public static Func<int, ContactPM> OverrideGetLoggedContactFunc { get; set; }


        private static ContactPM GetLoggedContact(int tenant)
        {
            if (OverrideGetLoggedContactFunc != null)
            {
                return OverrideGetLoggedContactFunc(tenant);
            }

            //ILoggedContactUtil loggedContactUtil = ContainerAccessor.Container.Resolve(typeof(ILoggedContactUtil), "LoggedContactUtil", new ParameterOverride("", tenant)) as ILoggedContactUtil;
            //ContactPM loggedcontact = loggedContactUtil.GetLoggedContact(tenant);

            ContactPM loggedcontact = LoggedContactResolver.GetLoggedContact(tenant);
            return loggedcontact;
        }


      
        //Generate test records
        public void GenerateTestRecordsForExternalReco(string glAccountId, string bankAccountId, string type, int tenant)
        {

            if (glAccountId == null || bankAccountId == null) throw new ApplicationException("args.glAccountId or bankAccountId is not provided!!");

            int linesCount = 10;

            //
            // 1- Get default values

            // LoggedUser
            ContactPM loggedContact = GetLoggedContact(tenant);



            // Tenant
            TenantQuery tenantQuery = new TenantQuery(tenant);
            TenantPM tenantPM = tenantQuery.GetSinglePM(tenant);

            // BankAccount
            BankAccountQueryService bankQuery = new BankAccountQueryService(tenant);
            BankAccountPM bankAccount = bankQuery.GetSingle(bankAccountId, false, false);
            if (bankAccount != null && bankAccount.GLAccountCurrencyId == null) throw new ApplicationException("bank account does not have currency!!");

            // GLAccount
            GLAccountQueryService glaQuery = new GLAccountQueryService(tenant);
            GLAccountPM glAccount = glaQuery.GetSingle(glAccountId, false, false);

            // Account Exchange Rate
            RatesTableRepository rateRepo = new RatesTableRepository(tenant);
            RatesTable rate;
            if (glAccount.IsMultiCurrency == true)
            {
                //TAKE CURRENCY OF BANK ACCOUNT
                rate = rateRepo.GetClosestRate(tenantPM.CurrencyId, bankAccount.GLAccountCurrencyId, tenant);
            }
            else
            {
                rate = rateRepo.GetClosestRate(tenantPM.CurrencyId, glAccount.CurrencyId, tenant);
            }
            if (rate == null ) throw new ApplicationException("no exchange rate found!!");

            int lastPageNumber = Convert.ToInt32(bankAccount.LastPageNumber);
            DateTime? lastPageToDate;
            decimal? lastPageClosedAmount;
            if (bankAccount.LastPageNumber == null)
            {
                lastPageToDate = TenantServerConfigration.GetCurrentDateTime(0);
                lastPageClosedAmount = 0;
            }
            else
            {
                lastPageToDate = bankAccount.LastPageEndDate.Value.AddDays(1);
                lastPageClosedAmount = bankAccount.LastPageCloseBalance;
            }
            DateTime lastPageToDateplust10 = lastPageToDate.Value.AddDays(linesCount+1);


            // 2- Build bank page and transactions 
            JournalPM journal = new JournalPM()
            {
                ChangeSetOp = ChangeSetOperation.Insert,
                UpdateDate = DateTime.Now,
                UpdatedByUserId = loggedContact.Id,
                ApproveDate = DateTime.Now,
                ApprovedByUserId = loggedContact.Id,
                
                IsVoided = false,
                QueueId = null,

                Tenant = tenant,
                //JournalNumber = xxx,
                CreateDate = DateTime.Now,
                AccountingDate = DateTime.Now,
                TypeCode = "0", // 0- Manual
                StatusCode = "2", // 2- Approved
                CreatedByUserId = loggedContact.Id,
                AccountingEntityCode = "1", // 1- Jounral

                //AccountingEntityReference = xxxx,
                //OriginalJournalId = xxxx,

                //UpdatedByUserName = xxxx,
                //ApprovedByUserName = xxxx,
                //SearchFields = xxxx,
                //VoidedByUserId = xxxx,
                //VoidDate = xxxx,
                //OriginalJournalName = xxxx,
                //VoidedByUserName = xxxx,
                //VoidedByJournalId = xxxx,
                //ExternalSystem = xxxx,
                //StatusLocalName = xxxx,

            };
            ObjectTable bankAccountObjectTable = GetBankAccountObjectTable(tenant);

            ReconcileExternalPagePM bankPage = new ReconcileExternalPagePM()
            {
                Id = "",
                Tenant = tenant,
                ApprovedByUserId = loggedContact.Id,
                CreatedByUserId = loggedContact.Id,
                CreateDate = DateTime.Now,

                GLAccountId = glAccountId,
                EntityId = bankAccountId,
                ObjectTableId = bankAccountObjectTable.Id,
                EntryTypeCode = "1", // 1- Manual

                StartBalance = lastPageClosedAmount.Value,
                CloseBalance = lastPageClosedAmount.Value,

                PageNo = lastPageNumber + 1,

                FromDate = lastPageToDate.Value,
                ToDate = lastPageToDateplust10,

                StatusCode = "2", // 2- Approved
                //ReconcileExternalPageLines,

                ChangeSetOp = ChangeSetOperation.Insert,
            };

            decimal linesSum = 0;

            switch (type)
            {
                #region type 1
                case "1":
                    {
                        int sm = 2;
                        for (int i = 0; i < linesCount; i++)
                        {
                            DateTime _referenceDate = (i > (linesCount / 2) ? lastPageToDate.Value.AddDays(1) : lastPageToDate.Value.AddDays(i));
                            decimal _amount = ((i + 1) * 100);
                            //
                            // Declare page line
                            ReconcileExternalPageLinePM pageLine = new ReconcileExternalPageLinePM()
                            {
                                ReconcileExternalPageId = bankPage.Id,
                                LineNumber = i + 1,

                                ReferenceDate = _referenceDate,
                                Amount = _amount,
                                //Reference = "",

                                IsReconciled = false,
                                Notes = "",
                                Tenant = tenant,
                                ChangeSetOp = ChangeSetOperation.Insert,

                            };

                            // Fill reference field
                            if (i < 4)
                                pageLine.Reference = "" + (i*10+100);
                            else if (i >= 4 && i < 7)
                                pageLine.Reference = "" + (i * 10 + 100);
                            else
                                pageLine.Reference = "" + (i * 10 + 100);

                            linesSum += pageLine.Amount;




                            //
                            // Declare journal line
                            JournalLinePM journalLine = new JournalLinePM()
                            {
                                ChangeSetOp = ChangeSetOperation.Insert,
                                Tenant = tenant,
                                //JournalId = journal.Id,
                                Line = i + 1,
                                ActionCode = "3", // 2- Credit & Debit

                                DebitAccountId = glAccountId,
                                CreditAccountId = glAccountId,
                                //DebitControlAccountId = xxxx,
                                //CreditControlAccountId = xxxx,

                                DocumentDate = _referenceDate,
                                AccountingDate = DateTime.Now,
                                DueDate = _referenceDate,

                                ForeignAmount = _amount,
                                LocalAmount = _amount * Convert.ToDecimal(rate.Rate),

                                CurrencyId = glAccount.CurrencyId,
                                ExchangeRate = Convert.ToDecimal(rate.Rate),

                                //Notes = xxxx,

                                //Reference2 = xxxx,
                                //Reference3 = xxxx,
                                //ActionName = xxxx,
                                //DebitControlAccountName = xxxx,
                                //CreditAccountName = xxxx,
                                //DebitAccountName = xxxx,
                                //CreditControlAccountName = xxxx,
                                //CreditControlAccountNumber = xxxx,
                                //DebitControlAccountNumber = xxxx,
                                ////CreditAccountNumber = xxxx,
                                //DebitAccountNumber = xxxx,
                                //CurrencyName = xxxx,
                                //CurrencyCode = xxxx,
                                //ActionTypeCode = xxxx,
                                //ExternalOpenAmount = xxxx,
                                //IsCreditAccountMulti = false,
                                //IsDebitAccountMulti = xxxx,

                            };

                            // Fill reference field
                            if (sm == 0)
                                journalLine.Reference1 = "" + (i * 10 + 1000);
                            else if (sm == 1)
                                journalLine.Reference2 = "" + (i * 10 + 1000);
                            else if (sm == 2)
                                journalLine.Reference3 = "" + (i * 10 + 1000);
                            if (sm == 0) sm = 2;
                            else if (sm > 0) sm--;


                            // Push lines
                            bankPage.ReconcileExternalPageLines.Add(pageLine);
                            journal.JournalLines.Add(journalLine);



                        }

                        break;
                    }
                #endregion


                #region type 2
                case "2":
                    {
                        int sm = 2;
                        for (int i = 0; i < linesCount; i=i+2)
                        {
                            DateTime _referenceDate = (i > (linesCount / 2) ? lastPageToDate.Value.AddDays(1) : lastPageToDate.Value.AddDays(i));
                            decimal _amount = ((i + 1) * 100);
                            //
                            // Declare page line
                            ReconcileExternalPageLinePM pageLine = new ReconcileExternalPageLinePM()
                            {
                                ReconcileExternalPageId = bankPage.Id,
                                LineNumber = i + 1,

                                ReferenceDate = _referenceDate,
                                Amount = _amount/2,
                                //Reference = "",

                                IsReconciled = false,
                                Notes = "",
                                Tenant = tenant,
                                ChangeSetOp = ChangeSetOperation.Insert,

                            };

                            // Fill reference field
                            if (i < 4)
                                pageLine.Reference = "" + (i * 10 + 1000);
                            else if (i >= 4 && i < 7)
                                pageLine.Reference = "" + (i * 10 + 1000);
                            else
                                pageLine.Reference = "" + (i * 10 + 1000);

                            linesSum += pageLine.Amount;

                            //
                            // Declare page line
                            ReconcileExternalPageLinePM pageLine2 = new ReconcileExternalPageLinePM()
                            {
                                ReconcileExternalPageId = bankPage.Id,
                                LineNumber = i + 2,

                                ReferenceDate = _referenceDate,
                                Amount = _amount/2,
                                //Reference = "",

                                IsReconciled = false,
                                Notes = "",
                                Tenant = tenant,
                                ChangeSetOp = ChangeSetOperation.Insert,

                            };

                            // Fill reference field
                            if (i < 4)
                                pageLine2.Reference = ""+(i * 10 + 1000);
                            else if (i >= 4 && i < 7)
                                pageLine2.Reference = ""+(i * 10 + 1000);
                            else
                                pageLine2.Reference = ""+(i * 10 + 1000);

                            linesSum += pageLine2.Amount;




                            //
                            // Declare journal line
                            JournalLinePM journalLine = new JournalLinePM()
                            {
                                ChangeSetOp = ChangeSetOperation.Insert,
                                Tenant = tenant,
                                //JournalId = journal.Id,
                                Line = i + 1,
                                ActionCode = "3", // 2- Credit & Debit

                                DebitAccountId = glAccountId,
                                CreditAccountId = glAccountId,
                                //DebitControlAccountId = xxxx,
                                //CreditControlAccountId = xxxx,

                                DocumentDate = _referenceDate,
                                AccountingDate = DateTime.Now,
                                DueDate = _referenceDate,

                                ForeignAmount = _amount,
                                LocalAmount = _amount * Convert.ToDecimal(rate.Rate),

                                CurrencyId = glAccount.CurrencyId,
                                ExchangeRate = Convert.ToDecimal(rate.Rate),

                                //Notes = xxxx,

                                //Reference2 = xxxx,
                                //Reference3 = xxxx,
                                //ActionName = xxxx,
                                //DebitControlAccountName = xxxx,
                                //CreditAccountName = xxxx,
                                //DebitAccountName = xxxx,
                                //CreditControlAccountName = xxxx,
                                //CreditControlAccountNumber = xxxx,
                                //DebitControlAccountNumber = xxxx,
                                ////CreditAccountNumber = xxxx,
                                //DebitAccountNumber = xxxx,
                                //CurrencyName = xxxx,
                                //CurrencyCode = xxxx,
                                //ActionTypeCode = xxxx,
                                //ExternalOpenAmount = xxxx,
                                //IsCreditAccountMulti = false,
                                //IsDebitAccountMulti = xxxx,

                            };

                            // Fill reference field
                            if (sm==0)
                                journalLine.Reference1 = "" + (i * 10 + 1000);
                            else if (sm == 1)
                                journalLine.Reference2 = "" + (i * 10 + 1000);
                            else if (sm == 2)
                                journalLine.Reference3 = "" + (i * 10 + 1000);
                            if (sm == 0) sm = 2;
                            else if (sm > 0) sm--;

                            // Push lines
                            bankPage.ReconcileExternalPageLines.Add(pageLine);
                            bankPage.ReconcileExternalPageLines.Add(pageLine2);
                            journal.JournalLines.Add(journalLine);



                        }

                        break;
                    }
                    #endregion

            }



            // تهذيب
            bankPage.CloseBalance = lastPageClosedAmount.Value + linesSum;



            // 3- Update entities
            IAccountingContext accountingContext = AccountingContext.GetContext(tenant);
            JournalUpdateService journalUpdateService = new JournalUpdateService(accountingContext, new Dictionary<string, IContext>(), tenant);
            journalUpdateService.Update(journal, true);

            ReconcileExternalPageUpdateService bankPageUpdateService = new ReconcileExternalPageUpdateService(accountingContext, new Dictionary<string, IContext>(), tenant);
            bankPageUpdateService.Update(bankPage, true);


        }
        private ObjectTable GetBankAccountObjectTable(int tenant)
        {
            ObjectTableRepository objectTableRepository = new ObjectTableRepository(tenant);
            ObjectTable bankAccountObjectTable = objectTableRepository.GetObjectTableByName("BankAccount", tenant, false);
            if (bankAccountObjectTable == null)
                throw new ApplicationException("No objectfield for BankAccount!");
            return bankAccountObjectTable;
        }
    }



    public class MyLedgerTransaction
    {
        public string Id { get; set; }

        //public DateTime AccountingDate { get; set; }

        public DateTime DocumentDate { get; set; }

        public decimal Amount { get; set; }
        public decimal ForeignAmountCredit { get; set; }
        public decimal ForeignAmountDebit { get; set; }

        //public string CurrencyId { get; set; }

        //public decimal ExchangeRate { get; set; }

        public string Reference1 { get; set; }

        public string Reference2 { get; set; }

        public string Reference3 { get; set; }

        public decimal OpenAmount { get; set; }
		public DateTime AccountingDate { get; set; }
		//public string OpenAmountCurrencyId { get; set; }

	}

    public class MyPageLine
    {
        public string Id { get; set; }
        public DateTime ReferenceDate { get; set; }
        public decimal Amount { get; set; }
        public decimal CreditAmount { get; set; }
        public decimal DebitAmount { get; set; }
        public string Reference { get; set; }

    }

    public class MatchingLine
    {
        public string LedgerTransactionId { get; set; }
        public string BankPageLineId { get; set; }
        public int GroupNumber { get; set; }


    }
    public class MatchedReconciliationLines
    {
        public List<LedgerTransactionList> transactionLines { get; set; }
        public List<ReconcileExternalPageLineList> pageLines { get; set; }
        public int Count { get; set; }

    }

    struct Key
    {
        public readonly string Id;
        public readonly decimal Amount;
        public readonly string Reference;
        public readonly DateTime ReferenceDate;
        public Key(string id, decimal amount, string reference, DateTime referenceDate)
        {
            Id = id;
            Amount = amount;
            Reference = reference;
            ReferenceDate = referenceDate;
        }
    }
    struct AmountRefKey
    {
        public readonly decimal Amount;
        public readonly string Reference;
        public AmountRefKey(decimal amount, string reference)
        {
            Amount = amount;
            Reference = reference;
        }
    }
    struct AmountRefRefDateKey
    {
        public readonly decimal Amount;
        public readonly string Reference;
        public readonly DateTime ReferenceDate;
        public AmountRefRefDateKey(decimal amount, string reference, DateTime referenceDate)
        {
            Amount = amount;
            Reference = reference;
            ReferenceDate = referenceDate;
        }
    }
    struct RefRefDateKey
    {
        public readonly string Reference;
        public readonly DateTime ReferenceDate;
        public RefRefDateKey(string reference, DateTime referenceDate)
        {
            Reference = reference;
            ReferenceDate = referenceDate;
        }
    }
    struct AmountRefDateKey
    {
        public readonly decimal Amount;
        public readonly DateTime ReferenceDate;
        public AmountRefDateKey(decimal amount, DateTime referenceDate)
        {
            Amount = amount;
            ReferenceDate = referenceDate;
        }
    }
	struct AmountRefDatAccountDateKey
	{
		public readonly decimal Amount;
		public readonly DateTime ReferenceDate;
		public readonly DateTime AccountingDate;

		public AmountRefDatAccountDateKey(decimal amount, DateTime referenceDate, DateTime accountingDate)
		{
			Amount = amount;
			ReferenceDate = referenceDate;
            AccountingDate = accountingDate;
		}
	}
	struct AmountRefRefDateAccDateKey
	{
		public readonly decimal Amount;
		public readonly string Reference;
		public readonly DateTime ReferenceDate;
		public readonly DateTime AccoutingDate;

		public AmountRefRefDateAccDateKey(decimal amount, string reference, DateTime referenceDate, DateTime accoutingDate)
		{
			Amount = amount;
			Reference = reference;
			ReferenceDate = referenceDate;
            AccoutingDate = accoutingDate;
		}
	}
	struct RefRefDateAccDateKey
	{
		public readonly string Reference;
		public readonly DateTime ReferenceDate;
		public readonly DateTime AccoutingDate;

		public RefRefDateAccDateKey(string reference, DateTime referenceDate,DateTime accoutingDate)
		{
			Reference = reference;
			ReferenceDate = referenceDate;
			AccoutingDate = accoutingDate;
		}
	}

	public class AutoExternalReconcileArgs
    {
        public bool AmountReconcile { get; set; }
        public bool ReferenceReconcile { get; set; }
        public bool RefDateReconcile { get; set; }
		public bool AccoutingDateReconcile { get; set; }
		public string ObjectTableId { get; set; }
        public string EntityId { get; set; }
        public string GLAccountId { get; set; }
        public string TransferGLAccountId { get; set; }
        public QueryOperations TransactionQueryOperations { get; set; }
        public QueryOperations BankPageLineQueryOperations { get; set; }
    }

}

