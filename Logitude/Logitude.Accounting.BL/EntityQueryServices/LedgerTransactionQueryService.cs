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
                .GroupBy(rec => new { rec.JournalId, jLine = rec.JournalLineNumber, rec.AccountId, rec.CurrencyId })
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

                    //DocumentDate =
                })//.AsEnumerable().Select( rec=> new 
                ;
            var qLedgerTrans = qGroupByJournalLineAccCurr;




            var qsJournalLine = new JournalLineQueryService(this.MainContext as IAccountingContext);
            var qJLAll = qsJournalLine.GetJournalLineAsLedgerTransaction(fromDate, toDate, tenant
                , JournalId, JournalLine
                , glAccountId
                );


            var qNotinJournalLine = (
                from t in qLedgerTrans
                join jl in qJLAll
                on new { t.JournalId, t.JournalLineNumber, t.AccountId, t.CurrencyId } equals new { jl.JournalId, jl.JournalLineNumber, jl.AccountId, jl.CurrencyId }
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

                }
                );

            var qNotinLedgerTrans = (
                from jl in qJLAll
                join t in qLedgerTrans
                on new { jl.JournalId, jl.JournalLineNumber, jl.AccountId, jl.CurrencyId }
                equals new { t.JournalId, t.JournalLineNumber, t.AccountId, t.CurrencyId }
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
                }
                );

            //ProblemWithEqualAccIDCreditDebit(qLedgerTrans, qJLAll);


            var qJLAllG = (from jl in qJLAll
                           group jl by new
                           {
                               jl.JournalId,
                               //jl.JournalLineNumber,
                               jl.AccountId,
                               jl.CurrencyId
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
                           }
                );

            var qLedgerTransG = (from t in qLedgerTrans
                                 group t by new
                                 {
                                     t.JournalId,
                                     //jl.JournalLineNumber,
                                     t.AccountId,
                                     t.CurrencyId
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
                                 }
                );

            var qDiff = (
               from jlG in qJLAllG
               join tG in qLedgerTransG
               on new { jlG.JournalId, jlG.JournalLineNumber, jlG.AccountId, jlG.CurrencyId }
               equals new { tG.JournalId, tG.JournalLineNumber, tG.AccountId, tG.CurrencyId }
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
               }
               );


            bool fast_butShowDiffDueRoundISBad = false;
            if (fast_butShowDiffDueRoundISBad)
            {
                var qReport =
                    //qNotinJournalLine.Union(qNotinLedgerTrans).Union(qDiff);
                    qNotinJournalLine.Concat(qNotinLedgerTrans).Concat(qDiff);
                return qReport.ToList();
            }
            else
            {
                var rqNotinJournalLine = qNotinJournalLine.ToList();
                var rqNotinLedgerTrans = qNotinLedgerTrans.ToList();
                var rqDiff = qDiff.ToList();
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


        public IQueryable<LedgerTransaction> GetClosedPeriodTransactions(string accountId, int year, int openMonth, int closedMonth, int tenant)
        {
            return repository.GetClosedPeriodTransactions(accountId, year, openMonth, closedMonth, tenant);
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
