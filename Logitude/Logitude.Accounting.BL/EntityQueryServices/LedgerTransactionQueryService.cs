using Logitude.Accounting.BL.CoreBL.Mapping;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityKeys;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Logitude.Server.Tools;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Accounting.BL.DataContract;
using Logitude.Accounting.BL.CloseTables;

namespace Logitude.Accounting.BL.EntityQueryServices
{
    public partial class LedgerTransactionQueryService : EntityQueryService<LedgerTransaction, LedgerTransactionKeys, LedgerTransactionPM, object, LedgerTransactionKeys>
    {
        public bool CheckIfLedgerTransactionOtherCurrencyExist(string gLAccointId, string currencyId, int tenant)
        {
            return this.repository.CheckIfLedgerTransactionOtherCurrencyExist(gLAccointId, currencyId, tenant);
        }

        public string GetAnyLedgerTransactionCurrency(string gLAccointId, int tenant)
        {
            return this.repository.GetAnyLedgerTransactionCurrency(gLAccointId, tenant);
        }

        public string GetAnyLedgerTransactionByJournalId(string journalId, int tenant)
        {
            return this.repository.GetAnyLedgerTransactionByJournalId(journalId, tenant);
        }
        public string GetCurrencyWhenMultiOff(string gLAccointId, int tenant)
        {
            string anyCurrencyId = this.repository.GetAnyLedgerTransactionCurrency(gLAccointId, tenant);
            if (!String.IsNullOrEmpty(anyCurrencyId))
            {
                bool otherCurrencyExists = this.repository.CheckIfLedgerTransactionOtherCurrencyExist(gLAccointId, anyCurrencyId, tenant);
                if (otherCurrencyExists)
                {
                    anyCurrencyId = "NonMultiError";
                }
            }
            return anyCurrencyId;
        }

        public List<JournalLineLedgerDTO> GetReportCompareToJournalLine(DateTime fromDate, DateTime toDate, int tenant
             , string JournalId = null, int? JournalLine = null
            , string glAccountId = null
            )
        {
            bool check20181209 = false;

            var qLedgerTransByAcountingDate =
                this.repository.GetAll(tenant).Where(rec =>
                    EntityFunctions.TruncateTime(rec.AccountingDate) >= fromDate.Date &&
                    EntityFunctions.TruncateTime(rec.AccountingDate) <= toDate.Date);
            if (!string.IsNullOrWhiteSpace(JournalId))
            {
                qLedgerTransByAcountingDate = qLedgerTransByAcountingDate.Where(rec => rec.JournalId == JournalId);
                if (JournalLine.HasValue)
                {
                    var JournalLineVAL = JournalLine.GetValueOrDefault();
                    qLedgerTransByAcountingDate = qLedgerTransByAcountingDate.Where(rec => rec.JournalLineNumber == JournalLineVAL);

                }
            }
            if (!string.IsNullOrWhiteSpace(glAccountId))
            {

                qLedgerTransByAcountingDate = qLedgerTransByAcountingDate.Where(rec => rec.AccountId == glAccountId);
            }

            var qsGLAccount = new GLAccountQueryService(this.MainContext as IAccountingContext);
            var qAllControlAccount = qsGLAccount.GetQAllControlAccount(tenant);


            qLedgerTransByAcountingDate = (from trans in qLedgerTransByAcountingDate
                                           let controlAccIds = (from ca in qAllControlAccount select ca.Id)
                                           where controlAccIds.Contains(trans.AccountId) == false
                                           select trans
                     );
            var qGroupByJournalLineAccCurr = qLedgerTransByAcountingDate
                .GroupBy(rec => new { rec.JournalId, jLine = rec.JournalLineNumber, rec.AccountId, rec.CurrencyId,
                    rec.AccountingDate ,
                    rec.DueDate,
                    rec.DocumentDate
                })
                .Select(g => new JournalLineLedgerDTO()
                {
                    CHANGE_TYPE = "",
                    JournalId = g.Key.JournalId,
                    JournalLineNumber = g.Key.jLine,

                    AccountId = g.Key.AccountId,
                    CurrencyId = g.Key.CurrencyId,

                    LocalAmountCredit = (double)g.Sum(x => x.LocalAmountCredit),
                    LocalAmountDebit = (double)g.Sum(x => x.LocalAmountDebit),

                    ForeignAmountCredit = (double)g.Sum(x => x.ForeignAmountCredit),
                    ForeignAmountDebit = (double)g.Sum(x => x.ForeignAmountDebit),

                    AccountingDate = g.Key.AccountingDate,
                    DueDate = g.Key.DueDate,
                    DocumentDate = g.Key.DocumentDate,

                    //DocumentDate =
                })//.AsEnumerable().Select( rec=> new 
                ;
            var qLedgerTrans = qGroupByJournalLineAccCurr;


            if (check20181209)
            {
                var qtest = qLedgerTrans.ToList();
            }


            var qsJournalLine = new JournalLineQueryService(this.MainContext as IAccountingContext);
            var qJLAll = qsJournalLine.GetJournalLineAsLedgerTransaction(fromDate, toDate, tenant
                , JournalId, JournalLine
                , glAccountId
                );

            if (check20181209)
            {
                var qtest = qJLAll.ToList();
            }

            var qNotinJournalLine = (
                from t in qLedgerTrans
                join jl in qJLAll
                on new
                {
                    t.JournalId,
                    t.JournalLineNumber,
                    t.AccountId,
                    t.CurrencyId,
                    t.AccountingDate,
                    t.DueDate,
                    t.DocumentDate
                } 
                equals new
                {
                    jl.JournalId,
                    jl.JournalLineNumber,
                    jl.AccountId,
                    jl.CurrencyId,
                    jl.AccountingDate,
                    jl.DueDate,
                    jl.DocumentDate
                }
                into joinT
                from joinr in joinT.DefaultIfEmpty()
                where joinr == null

                select new JournalLineLedgerDTO()
                {
                    CHANGE_TYPE = "Not in Journal Line",
                    JournalId = t.JournalId,
                    JournalLineNumber = t.JournalLineNumber,
                    AccountId = t.AccountId,
                    CurrencyId = t.CurrencyId,

                    LocalAmountCredit = t.LocalAmountCredit,
                    LocalAmountDebit = t.LocalAmountDebit,

                    ForeignAmountCredit = t.ForeignAmountCredit,
                    ForeignAmountDebit = t.ForeignAmountDebit,

                    //DocumentDate = t.DocumentDate
                    AccountingDate=t.AccountingDate,
                    DueDate=t.DueDate,
                    DocumentDate=t.DocumentDate


                }
                );

            if (check20181209)
            {
                var qtest = qNotinJournalLine.ToList();
            }

            

            var qNotinLedgerTrans = (
                from jl in qJLAll
                join t in qLedgerTrans
                on new
                {
                    jl.JournalId,
                    jl.JournalLineNumber,
                    jl.AccountId,
                    jl.CurrencyId,
                    jl.AccountingDate,
                    jl.DueDate,
                    jl.DocumentDate
                }
                equals new {
                    t.JournalId,
                    t.JournalLineNumber,
                    t.AccountId,
                    t.CurrencyId,
                    t.AccountingDate,
                    t.DueDate,
                    t.DocumentDate
                }
                into joinT
                from joinr in joinT.DefaultIfEmpty()
                where joinr == null
                //on t.AccountId equals jl.AccountId
                select new JournalLineLedgerDTO()
                {
                    CHANGE_TYPE = "Not in LedgerTrans ",
                    JournalId = jl.JournalId,
                    JournalLineNumber = jl.JournalLineNumber,
                    AccountId = jl.AccountId,
                    CurrencyId = jl.CurrencyId,

                    LocalAmountCredit = jl.LocalAmountCredit,
                    LocalAmountDebit = jl.LocalAmountDebit,

                    ForeignAmountCredit = jl.ForeignAmountCredit,
                    ForeignAmountDebit = jl.ForeignAmountDebit,

                    //DocumentDate = jl.DocumentDate
                    AccountingDate=jl.AccountingDate,
                    DueDate=jl.DueDate,
                    DocumentDate=jl.DocumentDate

                }
                );
            if (check20181209)
            {
                var qtest = qNotinLedgerTrans.ToList();
            }
            //ProblemWithEqualAccIDCreditDebit(qLedgerTrans, qJLAll);

            var qJLAllG = (from jl in qJLAll
                           group jl by new
                           {
                               jl.JournalId,
                               //jl.JournalLineNumber,
                               jl.AccountId,
                               jl.CurrencyId,
                               jl.AccountingDate,
                               jl.DueDate,
                               jl.DocumentDate

                           } into g
                           select new JournalLineLedgerDTO()
                           {
                               CHANGE_TYPE = "",
                               JournalId = g.Key.JournalId,
                               JournalLineNumber = 999,//How care line number?  //g.Key.JournalLineNumber,

                               AccountId = g.Key.AccountId,
                               CurrencyId = g.Key.CurrencyId,

                               LocalAmountCredit = (double)g.Sum(x => x.LocalAmountCredit),
                               LocalAmountDebit = (double)g.Sum(x => x.LocalAmountDebit),

                               ForeignAmountCredit = (double)g.Sum(x => x.ForeignAmountCredit),
                               ForeignAmountDebit = (double)g.Sum(x => x.ForeignAmountDebit),

                               AccountingDate= g.Key.AccountingDate,
                               DueDate= g.Key.DueDate,
                               DocumentDate= g.Key.DocumentDate

                           }
                );

            if (check20181209)
            {
                var qtest = qJLAllG.ToList();
            }

            

            var qLedgerTransG = (from t in qLedgerTrans
                                 group t by new
                                 {
                                     t.JournalId,
                                     //jl.JournalLineNumber,
                                     t.AccountId,
                                     t.CurrencyId,


                                     t.AccountingDate,
                                     t.DueDate,
                                     t.DocumentDate

                                 } into g
                                 select new JournalLineLedgerDTO()
                                 {
                                     CHANGE_TYPE = "",
                                     JournalId = g.Key.JournalId,
                                     JournalLineNumber = 999,//How care line number? ///g.Key.JournalLineNumber,

                                     AccountId = g.Key.AccountId,
                                     CurrencyId = g.Key.CurrencyId,

                                     LocalAmountCredit = (double)g.Sum(x => x.LocalAmountCredit),
                                     LocalAmountDebit = (double)g.Sum(x => x.LocalAmountDebit),

                                     ForeignAmountCredit = (double)g.Sum(x => x.ForeignAmountCredit),
                                     ForeignAmountDebit = (double)g.Sum(x => x.ForeignAmountDebit),


                                     AccountingDate= g.Key.AccountingDate,
                                     DueDate= g.Key.DueDate,
                                     DocumentDate= g.Key.DocumentDate

                                 }
                );
            if (check20181209)
            {
                var qtest = qLedgerTransG.ToList();
            }


            var qDiffAmount = (
               from jlG in qJLAllG
               join tG in qLedgerTransG
               on new { jlG.JournalId, jlG.JournalLineNumber, jlG.AccountId, jlG.CurrencyId ,
                   jlG.AccountingDate,
                   jlG.DueDate,
                   jlG.DocumentDate
               }
               equals new { tG.JournalId, tG.JournalLineNumber, tG.AccountId, tG.CurrencyId ,
                   tG.AccountingDate,
                   tG.DueDate,
                   tG.DocumentDate
               }
               into joinT
               from joinr in joinT
               where jlG.LocalAmountDebit != joinr.LocalAmountDebit ||
               jlG.LocalAmountCredit != joinr.LocalAmountCredit ||
               jlG.ForeignAmountCredit != joinr.ForeignAmountCredit ||
#if true
 Math.Abs(jlG.ForeignAmountDebit - joinr.ForeignAmountDebit) > 0.001 //fuck the round !!!!
#else
                //jl.ForeignAmountDebit != joinr.ForeignAmountDebit //||
                //bad Math.Round in Vat            
#endif

            
            select new JournalLineLedgerDTO()
               {
                   CHANGE_TYPE = "Different(Delta)",
                   JournalId = jlG.JournalId,
                   JournalLineNumber = jlG.JournalLineNumber,

                   AccountId = jlG.AccountId,
                   CurrencyId = jlG.CurrencyId,

                   LocalAmountCredit = (jlG.LocalAmountCredit - joinr.LocalAmountCredit),
                   LocalAmountDebit = (jlG.LocalAmountDebit - joinr.LocalAmountDebit),

                   ForeignAmountCredit = (jlG.ForeignAmountCredit - joinr.ForeignAmountCredit),
                   ForeignAmountDebit = (jlG.ForeignAmountDebit - joinr.ForeignAmountDebit),

                //DocumentDate = jl.DocumentDate
                AccountingDate=jlG.AccountingDate,
                DueDate=jlG.DueDate,
                DocumentDate=jlG.DocumentDate

            }
               );
            qDiffAmount = qDiffAmount
                .Where(r => 
                Math.Abs(r.LocalAmountCredit) > 0.001//fuck the round !!!!
                || Math.Abs(r.LocalAmountDebit) > 0.001//fuck the round !!!!
                || Math.Abs(r.ForeignAmountCredit) > 0.001//fuck the round !!!!
                || Math.Abs(r.ForeignAmountDebit) > 0.001)//fuck the round !!!!
                ;
            if (check20181209)
            {
                var qtest = qDiffAmount.ToList();
            }
            bool fast_butShowDiffDueRoundISBad = false;
            if (fast_butShowDiffDueRoundISBad)
            {
                var qReport =
                    //qNotinJournalLine.Union(qNotinLedgerTrans).Union(qDiff);
                    qNotinJournalLine.Concat(qNotinLedgerTrans).Concat(qDiffAmount);
                return qReport.ToList();
            }
            else
            {
                var rqNotinJournalLine = qNotinJournalLine.ToList();
                var rqNotinLedgerTrans = qNotinLedgerTrans.ToList();
                var rqDiff = qDiffAmount.ToList();
                return //rqNotinJournalLine.Union(rqNotinLedgerTrans).Union(rqDiff).ToList();
                rqNotinJournalLine.Concat(rqNotinLedgerTrans).Concat(rqDiff).ToList();
            }
        }

        private static void ProblemWithEqualAccIDCreditDebit(IQueryable<JournalLineLedgerDTO> qLedgerTrans, IQueryable<JournalLineLedgerDTO> qJLAll)
        {
            var qDiffProblemWithEqualAccIDCreditDebit = (
                from jl in qJLAll
                join t in qLedgerTrans
                on new { jl.JournalId, jl.JournalLineNumber, jl.AccountId, jl.CurrencyId }
                equals new { t.JournalId, t.JournalLineNumber, t.AccountId, t.CurrencyId }
                into joinT
                from joinr in joinT
                where jl.LocalAmountDebit != joinr.LocalAmountDebit ||
                jl.LocalAmountCredit != joinr.LocalAmountCredit ||
                jl.ForeignAmountCredit != joinr.ForeignAmountCredit ||
#if true
 Math.Abs(jl.ForeignAmountDebit - joinr.ForeignAmountDebit) > 0.001 //fuck the round !!!!
#else           
                //jl.ForeignAmountDebit != joinr.ForeignAmountDebit //||
                //bad Math.Round in Vat            
#endif


                select new JournalLineLedgerDTO()
                {
                    CHANGE_TYPE = "Different(Delta)",
                    JournalId = jl.JournalId,
                    JournalLineNumber = jl.JournalLineNumber,

                    AccountId = jl.AccountId,
                    CurrencyId = jl.CurrencyId,

                    LocalAmountCredit = (jl.LocalAmountCredit - joinr.LocalAmountCredit),
                    LocalAmountDebit = (jl.LocalAmountDebit - joinr.LocalAmountDebit),

                    ForeignAmountCredit = (jl.ForeignAmountCredit - joinr.ForeignAmountCredit),
                    ForeignAmountDebit = (jl.ForeignAmountDebit - joinr.ForeignAmountDebit),

                    //DocumentDate = jl.DocumentDate
                }
                );
        }


        public decimal? GetLedgerTransactionSumFromTo(string gLAccointId, DateTime fromDate, DateTime toDate, int tenant)
        {
            return this.repository.GetLedgerTransactionSumFromTo(gLAccointId, fromDate, toDate, tenant);
        }

        public decimal? GetLedgerTransactionSumFromTo(string gLAccointId, DateTime fromDate, DateTime toDate, int tenant, string currencyId)
        {
            return this.repository.GetLedgerTransactionSumFromTo(gLAccointId, fromDate, toDate, tenant, currencyId);
        }

        public List<GLAccountTotalByMonth> CalcGLAccountTotalByMonthByDateType(string DateTypeCode, DateTime fromDate, DateTime accoutingDateUntillNotInclude, int tenant, IQueryable<string> listOfAccId = null)
        {
            return this.repository.CalcGLAccountTotalByMonthByDateType(DateTypeCode,fromDate, accoutingDateUntillNotInclude, tenant, listOfAccId);
        }

        public List<GLAccountTotalByMonth> CalcGLAccountTotalByMonthByAccountingDate(DateTime fromDate, DateTime accoutingDateUntillNotInclude, int tenant, IQueryable<string> listOfAccId = null)
        {
            return this.repository.CalcGLAccountTotalByMonthByAccountingDate(fromDate, accoutingDateUntillNotInclude, tenant, listOfAccId);
        }
        public List<CurrencySum> GetLedgerTransactionTotalLocalAmountFromTo(string gLAccointId, DateTime fromDate, DateTime toDate, int tenant)
        {
            return this.repository.GetLedgerTransactionTotalLocalAmountFromTo(gLAccointId, fromDate, toDate, tenant);
        }
        public List<LedgerTransactionPM> GetByJournalId(string journalId, int tenant)
        {
            List<LedgerTransaction> ledgerTransactionPOCOs = null;
            ledgerTransactionPOCOs = repository.GetByJournalId(journalId, tenant);
            List<LedgerTransactionPM> pms = ledgerTransactionPOCOs.Select(poco => this.GetEntityPM(poco)).ToList();
            return pms;
        }
        
        public List<LedgerTransactionPM> GetLedgerTransactionPMsByIdList(List<string> idList, int tenant)
        {
            List<LedgerTransaction> ledgerTransactionPOCOs = null;
            ledgerTransactionPOCOs = repository.GetLedgerTransactionsByIdList(idList, tenant);
            List<LedgerTransactionPM> pms = ledgerTransactionPOCOs.Select(poco => this.GetEntityPM(poco)).ToList();
            return pms;
        }

        public List<LedgerTransactionPM> DraftLedgerTransactionPMsByAccountId(string gLAccountId, int tenant)
        {
            List<LedgerTransaction> ledgerTransactionPOCOs = null;
            ledgerTransactionPOCOs = repository.GetDraftLedgerTransactionsByAccountId(gLAccountId, tenant);
            List<LedgerTransactionPM> pms = ledgerTransactionPOCOs.Select(poco => this.GetEntityPM(poco)).ToList();
            return pms;
        }

        public LedgerTransactionPM GetFirstLedgerTransaction(string AccountId, int tenant)
        {
            LedgerTransactionPM myPM = null;
            var temp = repository.GetAll(tenant).Where(a => a.AccountId == AccountId);
            if (temp != null)
            {
                LedgerTransaction MyPoco = temp.FirstOrDefault();
                myPM = this.GetEntityPM(MyPoco);
                return myPM;
            }
            else
            {
                return null;
            }
           
        }

        public List<LedgerTransactionPM> UpdateTransactionsExternalReconciled(List<string> idsList, int tenant)
        {
            List<LedgerTransaction> ledgerTransactionPOCOs = null;
            ledgerTransactionPOCOs = repository.GetLedgerTransactionsByIdList(idsList, tenant);
            List<LedgerTransactionPM> pms = ledgerTransactionPOCOs.Select(poco => this.GetEntityPM(poco)).ToList();
            return pms;
        }


        public List<LedgerTransactionPM> GetLast10TransactionsForAccount(string AccountId, int tenant)
        {
            List<LedgerTransaction> ledgerTransactionPOCOs = null;
            ledgerTransactionPOCOs = repository.GetLast10TransactionsForAccount(AccountId, tenant);
            List<LedgerTransactionPM> pms = ledgerTransactionPOCOs.Select(poco => GetEntityPM(poco)).ToList();
            return pms;
        }


        public IQueryable<LedgerTransaction> GetClosedPeriodTransactions(string accountId, DateTime closedDate, DateTime openDate, int tenant)
        {
            return repository.GetClosedPeriodTransactions(accountId, closedDate, openDate, tenant);
        }

        public List<B100Data> GetTransactionsByDate( DateTime fromDate, DateTime toDate, int tenant)
        {
            List<B100Data> transactions = null;

                transactions = (from a in context.LedgerTransactions
                                               join g in context.GLAccounts on a.AccountId equals g.Id
                                                        join j in context.Journals on a.JournalId equals j.Id
                                                     
                                                        where ((a.DocumentDate >= fromDate && a.DocumentDate <= toDate) || (a.AccountingDate >= fromDate && a.AccountingDate <= toDate)) && a.Tenant == tenant
                                                        select  new B100Data()
                                                        {
                                                            AccountingDate = a.AccountingDate,
                                                            DocumentDate = a.DocumentDate,
                                                            AccountingEntityCode = j.AccountingEntityCode,
                                                            AccountingEntityReference = j.AccountingEntityReference,
                                                            ForeignAmountCredit = a.ForeignAmountCredit,
                                                            ForeignAmountDebit = a.ForeignAmountDebit,
                                                            GLAccountDisplayNumber = g.DisplayNumber,
                                                            LocalAmountCredit = a.LocalAmountCredit,
                                                            LocalAmountDebit = a.LocalAmountDebit,
                                                            CreateDate = a.CreateDate,
                                                            CurrencyId= a.CurrencyId,
                                                            CreatedByUser = j.CreatedByUserId,
                                                            JournalLineNumber = a.JournalLineNumber,
                                                            JournalNumber = j.JournalNumber,
                                                            Notes = a.Notes,
                                                            Reference2 = a.Reference2,
                                                            LedgerTransactionId = a.Id,
                                                            OppositGLAccount = a.OppositeAccount != null ? a.OppositeAccount.DisplayNumber:null,
                                                        }).ToList();

                
           


            return transactions;
        }
        public List<LedgerTransactionPM> GetTransactionBySourceId(string AccountId, int tenant)
        {
            List<LedgerTransaction> ledgerTransactionPOCOs = null;
            ledgerTransactionPOCOs = repository.GetLast10TransactionsForAccount(AccountId, tenant);
            List<LedgerTransactionPM> pms = ledgerTransactionPOCOs.Select(poco => GetEntityPM(poco)).ToList();
            return pms;
        }

        public List<LedgerTransactionPM> GetReconciledInvoicesTransactionsForARPayment(string arpaymentId, string billToGLAccountId, int tenant)
        {
            List<LedgerTransactionPM> reconciledTransactions = new List<LedgerTransactionPM>(); // result
            LedgerTransactionQueryService transactionsQuery = new LedgerTransactionQueryService(tenant);
            ReconciliationLineQueryService recoLineQuery = new ReconciliationLineQueryService(tenant);

            IQueryable<LedgerTransactionPM> transactions = GetTransactionsJoinedWithJounrals();

            // get payment transaction
            LedgerTransactionPM paymentTransaction = transactions.Where(t => t.SourceId == arpaymentId).FirstOrDefault();
            if (paymentTransaction != null)
            {
                string paymentTransactionId = paymentTransaction.Id;

                // get reconcile lines for this payment
                List<ReconciliationLinePM> recoLines = recoLineQuery.GetLinesByReconciledWithTransactionId(paymentTransactionId, tenant);

                // get transactions connected to reco lines
                List<string> recoLinesTransactionsId = recoLines.Select(d => d.TransactionId).ToList();
                reconciledTransactions = transactionsQuery.GetLedgerTransactionPMsByIdList(recoLinesTransactionsId, tenant);

                // exclude partially reconcile transactions
                reconciledTransactions = reconciledTransactions.Where(d => d.IsReconciled == true && d.SourceTypeCode == AccountingEntityValues.ARInvoice).ToList();

                reconciledTransactions = FillTransactionsReconciliationNumbers(reconciledTransactions, tenant);
                reconciledTransactions = FillReconciledPaymentTransactionAmount(reconciledTransactions.ToList(), paymentTransaction != null ? paymentTransaction.Id : null, tenant);

            }
            return reconciledTransactions;

        }
        public List<LedgerTransactionPM> GetOpenInvoicesTransactionsForAccount(string billToGLAccountId, string arpaymentId, int tenant)
        {
            IQueryable<LedgerTransactionPM> transactions = GetTransactionsJoinedWithJounrals();

            // get payment transaction
            LedgerTransactionPM paymentTransaction = transactions.Where(t => t.SourceId == arpaymentId).FirstOrDefault();

            // filter transactions by account and source type
            transactions = transactions
                    .Where(d =>
                        d.AccountId == billToGLAccountId
                        && d.Tenant == tenant
                        && d.IsReconciled == false
                        && d.SourceTypeCode == AccountingEntityValues.ARInvoice)
                    .OrderBy(b => b.AccountingDate).ThenByDescending(b => b.JournalId);

           
            List<LedgerTransactionPM> openInvoicesTransactions = FillTransactionsReconciliationNumbers(transactions.ToList(), tenant);
            openInvoicesTransactions = FillReconciledPaymentTransactionAmount(openInvoicesTransactions.ToList(), paymentTransaction != null ? paymentTransaction.Id : null, tenant);

            return openInvoicesTransactions;
        }

        /// <summary>
        /// Returns any invoice transaction which is reconciled with the payment. --Abdullah
        /// </summary>
        /// <param name="arpaymentId">Payment id</param>
        /// <param name="billToGLAccountId">Payment bill to account id</param>
        /// <param name="tenant">Tenant</param>
        /// <returns>List of invoices ledger transactions</returns>
        public List<LedgerTransactionPM> GetReconciledInvoicesTransactionsForPayment(string arpaymentId, string billToGLAccountId, int tenant) // reconciled and partially reconciled
        {
            List<LedgerTransactionPM> reconciledInvoicesTransactions = GetReconciledInvoicesTransactionsForARPayment(arpaymentId, billToGLAccountId, tenant);
            List<LedgerTransactionPM> openedAndPartiallyInvoicesTransactions = GetOpenInvoicesTransactionsForAccount(billToGLAccountId, arpaymentId, tenant);

            List<LedgerTransactionPM> reconciledTransactions 
                = reconciledInvoicesTransactions.Concat(
                    openedAndPartiallyInvoicesTransactions.Where(d => d.PaymentReconciledAmount != 0) // partially invoices reconciled with this payment
                    ).ToList();

            return reconciledTransactions;
        }

        private List<LedgerTransactionPM> FillTransactionsReconciliationNumbers(List<LedgerTransactionPM> transactions, int tenant)
        {
            List<LedgerTransactionPM> trans4invoices = new List<LedgerTransactionPM>();

            trans4invoices = transactions.ToList();
            trans4invoices.ForEach(t =>
            {
                t.RecoNumber = GetReconciliationsNumbersForTransaction(t.Id, tenant);


            });

            return trans4invoices;
        }
        private List<LedgerTransactionPM> FillReconciledPaymentTransactionAmount(List<LedgerTransactionPM> transactions, string paymentTransactionId, int tenant)
        {
            List<LedgerTransactionPM> trans4invoices = new List<LedgerTransactionPM>();

            trans4invoices = transactions.ToList();
            trans4invoices.ForEach(t =>
            {
                t.PaymentReconciledAmount = GetPaymentReocnciliationAmountForTransaction(t.Id, paymentTransactionId, tenant);

            });

            return trans4invoices;
        }

        private IQueryable<LedgerTransactionPM> GetTransactionsJoinedWithJounrals()
        {

            IQueryable<LedgerTransactionPM> query =
                from _transaction in context.LedgerTransactions

                    //join _journalLine in context.JournalLines
                    //on new { x = _transaction.JournalId, y = _transaction.JournalLineNumber } equals new { x = _journalLine.JournalId, y = _journalLine.Line }

                join _journal in context.Journals
                on _transaction.JournalId equals _journal.Id

                select new LedgerTransactionPM()
                {
                    IsReconciled = _transaction.IsReconciled,

                    Id = _transaction.Id,
                    Tenant = _transaction.Tenant,
                    JournalId = _transaction.JournalId,
                    JournalLineNumber = _transaction.JournalLineNumber,
                    CreateDate = _transaction.CreateDate,
                    ControlAccountId = _transaction.ControlAccountId,
                    AccountId = _transaction.AccountId,
                    AccountingDate = _transaction.AccountingDate,
                    DocumentDate = _transaction.DocumentDate,
                    DueDate = _transaction.DueDate,
                    LocalAmountDebit = _transaction.LocalAmountDebit,
                    LocalAmountCredit = _transaction.LocalAmountCredit,
                    CurrencyId = _transaction.CurrencyId,
                    ForeignAmountDebit = _transaction.ForeignAmountDebit,
                    ForeignAmountCredit = _transaction.ForeignAmountCredit,
                    ExchangeRate = _transaction.ExchangeRate,
                    Reference1 = _transaction.Reference1,
                    Reference2 = _transaction.Reference2,
                    Reference3 = _transaction.Reference3,
                    OpenAmount = _transaction.OpenAmount,
                    OppositeAccountId = _transaction.OppositeAccountId,
                    SearchFields = _transaction.SearchFields,
                    JournalNumber = _journal.JournalNumber,
                    //CurrencyCode = _transaction.CurrencyId,
                    //Source = _transaction.Source,
                    //SourceType = _journal.accounting,
                    OpenAmountCurrencyId = _transaction.OpenAmountCurrencyId,
                    Notes = _transaction.Notes,
                    //CumulativeLocalAmount = _transaction.CumulativeLocalAmount,
                    //CumulativeForeignAmount = _transaction.CumulativeForeignAmount,
                    AmountToReconcile = _transaction.AmountToReconcile,
                    Mark = _transaction.Mark,
                    //OpenAmountCurrencyCode = _transaction.OpenAmountCurrencyCode,
                    SourceId = _journal.AccountingEntityId,
                    SourceNumber = _journal.AccountingEntityReference,
                    SourceTypeCode = _journal.AccountingEntityCode,
                    //CurrencySign = _transaction.CurrencySign,
                    //OpenAmountCurrencySign = _transaction.OpenAmountCurrencySign,
                    //GroupHash = _transaction.GroupHash,
                    IsExternalReconcile = _transaction.IsExternalReconcile,
                    InReconcileProgress = _transaction.InReconcileProgress,
                    //ForeignAmount = _journalLine.ForeignAmount,
                    ReconcileRemarks = _transaction.ReconcileRemarks,
                    //OriginalJournalId = jl.JournalId,
                    //OppositeAccountEnglishName = _transaction.OppositeAccountEnglishName,
                    //OppositeAccountLocalName = _transaction.OppositeAccountLocalName,
                    //OppositeAccountDisplayNumber = _transaction.OppositeAccountDisplayNumber,
                    //RecoNumber = _journal,
                    //ReconciliationId = _journal,
                };

            return query;
        }

        private string GetReconciliationsNumbersForTransaction(string transactionId, int tenant)
        {
            ReconciliationLineQueryService recoLineQuery = new ReconciliationLineQueryService(tenant);
            ReconciliationQueryService recoQuery = new ReconciliationQueryService(tenant);

            // get reconciliation lines for the transaction
            List<ReconciliationLinePM> recoLines = recoLineQuery.GetLinesByTransactionId(transactionId, tenant).ToList();

            // get reconciliaitons of reconciliation lines
            List<string> recosIds = recoLines.Select(d => d.ReconciliationId).ToList();
            List<ReconciliationPM> recos = recoQuery.GetReconciliationsByIds(recosIds, tenant);

            // join reconciliations numbers
            List<string> numbers = recos.Where(d=>d.IsCancelled == false).Select(d => d.Number).ToList();
            string numbersString = string.Join(",", numbers.ToArray());

            return numbersString;
        }

        private decimal GetPaymentReocnciliationAmountForTransaction(string transactionId, string paymentTransactionId, int tenant)
        {
            ReconciliationLineQueryService recoLineQuery = new ReconciliationLineQueryService(tenant);
            ReconciliationQueryService recoQuery = new ReconciliationQueryService(tenant);

            // get reconciliation lines for the transaction
            List<ReconciliationLinePM> recoLines = recoLineQuery.GetLinesByTransactionId(transactionId, tenant).ToList();

            decimal reconciledAmount = 0;
            recoLines.ForEach(recoLine =>
            {
                if (recoLine.ReconciledWithTransactionId == paymentTransactionId && paymentTransactionId != null && recoLine.IsRecoCancelled == false)
                    reconciledAmount += recoLine.ReconciliationAmount;
            });
            
            return reconciledAmount;
        }


    }
    public class JournalLineLedgerDTO
    {
        public string JournalId { get; set; }
        public int JournalLineNumber { get; set; }
        public string AccountId { get; set; }
        public string CurrencyId { get; set; }
        //public decimal LocalAmountCredit { get; set; }
        //public decimal LocalAmountDebit { get; set; }
        //public decimal ForeignAmountCredit { get; set; }
        //public decimal ForeignAmountDebit { get; set; }

        public double LocalAmountCredit { get; set; }
        public double LocalAmountDebit { get; set; }
        public double ForeignAmountCredit { get; set; }
        public double ForeignAmountDebit { get; set; }

        public string CHANGE_TYPE { get; set; }
        public DateTime AccountingDate { get;  set; }
        public DateTime DueDate { get;  set; }
        public DateTime DocumentDate { get;  set; }
    }


    public class VatTypePercentageDTO
    {

        public DateTime FromDate { get; set; }
        public double myVat { get; set; }
    }

    public class JournalLineLedgerPostCalcVatDTO : JournalLineLedgerDTO
    {
        public enum VatEnum
        {
            none,
            OnlyVat,
            WithoutVat
        }
        public DateTime DocumentDate { get; set; } //due vat

        public String MyVatEnum { get; set; }

        public decimal LocalAmountDebitGross { get; set; }
        public decimal ForeignAmountDebitGross { get; set; }
    }
}
