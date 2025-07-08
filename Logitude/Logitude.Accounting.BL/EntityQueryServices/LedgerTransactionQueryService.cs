using Logitude.Accounting.BL.CloseTables;
using Logitude.Accounting.BL.DataContract;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityKeys;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Data.Enums;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Server.Tools;
 using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
 using System.Text;
using System.Threading.Tasks;
using Logitude.Accounting.BL.DataContract;
using Logitude.Accounting.BL.CloseTables;
using System.Data.Entity;
using Logitude.Accounting.Data.EntityLists;
using Logitude.BL.InvoiceModel.EntityQueries;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Microsoft.Practices.ObjectBuilder2;
 
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
        public LedgerTransactionPM GetLedgerTransactionPMByJournalId(string journalId, int tenant)
        {
            LedgerTransaction MyPoco = repository.GetByJournalId(journalId, tenant).FirstOrDefault();
            return this.GetEntityPM(MyPoco);
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

        public DateTime GetFirstDateByDateType(string dateTypeValue, string gLAccointId, int tenant)
        {

            DateTime firstDate = DateTime.Today;
            if (!String.IsNullOrEmpty(gLAccointId)) gLAccointId = gLAccointId.Trim();

            var query = this.repository.GetAll(tenant);
            
            if (!String.IsNullOrEmpty(gLAccointId))
                query = query.Where(rec => rec.AccountId == gLAccointId);

            if (query.Any())
            {
                switch (dateTypeValue)
                {
                    case GLAccountTotalDateTypeValues.AccountingDate:
                        firstDate = query.Select(rec => rec.AccountingDate).Min(); // AccountingDate is not nullable
                        break;
                    case GLAccountTotalDateTypeValues.DueDate:
                        firstDate = query.Select(rec => rec.DueDate).Min();        // DueDate is not nullable
                        break;
                    case GLAccountTotalDateTypeValues.DocumentDate:
                        firstDate = query.Select(rec => rec.DocumentDate).Min();   // DocumentDate is not nullable
                        break;
                    default:
                        firstDate = DateTime.Today;
                        break;
                }
            }
            else
            {
                firstDate = DateTime.Today;
            }
            return firstDate;
        }


        public List<JournalLineLedgerDTO> GetReportCompareToJournalLine(DateTime fromDate, DateTime toDate, int tenant
             , string JournalId = null, int? JournalLine = null
            , string glAccountId = null
            )
        {
            bool check20181209 = false;
            if ((this.context as System.Data.Entity.DbContext).Database.CommandTimeout<1200)//wrokerrole mode !!!
            {
                (this.context as System.Data.Entity.DbContext).Database.CommandTimeout = 1200;
            }
            
            var qLedgerTransByAcountingDate =
                this.repository.GetAll(tenant).Where(rec =>
                    rec.AccountingDate >= fromDate.Date &&
                    rec.AccountingDate <= toDate.Date);

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
                var rqNotinJournalLine = qNotinJournalLine.Take(30).ToList();
                var rqNotinLedgerTrans = qNotinLedgerTrans.Take(30).ToList();
                var rqDiff = qDiffAmount.Take(30).ToList();
                return //rqNotinJournalLine.Union(rqNotinLedgerTrans).Union(rqDiff).ToList();
                rqNotinJournalLine.Concat(rqNotinLedgerTrans).Concat(rqDiff).ToList();
            }
        }

        internal decimal TotalOpenTransactionAmount(string accountId, int tenant)
        {
            var totalOpenTransactionAmount =
            this.repository.GetAll(tenant)
                .Where(rec => rec.AccountId == accountId)
                .Where(r => r.IsReconciled == false)
                .Sum(r => r.OpenAmount);
            return totalOpenTransactionAmount;
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
        
        public List<CurrencySumOpenAmount> CalcCurrencySumOpenAmountByMonthByDateType(string DateTypeCode,  DateTime accoutingDateUntillNotInclude, int tenant, IQueryable<string> listOfAccId = null)
        {
            return this.repository.CalcCurrencySumOpenAmountByMonthByDateType(DateTypeCode,accoutingDateUntillNotInclude, tenant, listOfAccId);
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
            ledgerTransactionPOCOs = repository.GetByJournalId(journalId, tenant).ToList();
            List<LedgerTransactionPM> pms = ledgerTransactionPOCOs.Select(poco => this.GetEntityPM(poco)).ToList();
            return pms;
        }


        public IQueryable<LedgerTransaction> GetByJournalAndAccountId(string journalId, string accountId, int tenant)
        {
            return repository.GetByJournalAndAccountId(journalId, accountId, tenant);
           
        }
        public List<LedgerTransactionPM> GetByJournalIdAndForeignAmountCreditNotEqualZero(string journalId, int tenant)
        {
            List<LedgerTransaction> ledgerTransactionPOCOs = null;
            ledgerTransactionPOCOs = repository.GetByJournalIdAndForeignAmountCreditNotEqualZero(journalId, tenant).ToList();
            List<LedgerTransactionPM> pms = ledgerTransactionPOCOs.Select(poco => this.GetEntityPM(poco)).ToList();
            return pms;
        }
        public IQueryable<LedgerTransaction> GetIQueryableLedgerTransactionsByGLAccountIdsList(List<string> GLAccountIdsList, int tenant)
        {
            IQueryable<LedgerTransaction> ledgerTransactionPOCOs = null;
            ledgerTransactionPOCOs = repository.GetAll(tenant).Where(s => GLAccountIdsList.Contains(s.AccountId));
             return ledgerTransactionPOCOs;
        }

        public LedgerTransaction GetCreditTransactionByJournalId(string journalId, int tenant)
        {
            IQueryable<LedgerTransaction> ledgerTransactionPOCOs = repository.GetByJournalId(journalId, tenant);
            LedgerTransaction poco = ledgerTransactionPOCOs.Where(d => d.LocalAmountCredit != 0).FirstOrDefault();
            return poco;
        }

        public List<LedgerTransactionPM> GetByJournalLineIdAndLine(string journalId, int line, int tenant)
        {
            List<LedgerTransaction> ledgerTransactionPOCOs = null;
            ledgerTransactionPOCOs = repository.GetByJournalIdAndLine(journalId, line, tenant);
            List<LedgerTransactionPM> pms = ledgerTransactionPOCOs.Select(poco => this.GetEntityPM(poco)).ToList();
            return pms;
        }


        public List<LedgerTransactionPM> GetLedgerTransactionPMsByIdListFast(List<string> idList, int tenant, string SourceTypeCode)
        {
            var q = GetTransactionsJoinedWithJounrals();
            //List<LedgerTransaction> ledgerTransactionPOCOs = null;
            q=(from a in q
             where idList.Contains(a.Id) && a.Tenant == tenant
               where a.SourceTypeCode == SourceTypeCode

             select a);
            //ledgerTransactionPOCOs = repository.GetLedgerTransactionsByIdList(idList, tenant);
            //List<LedgerTransactionPM> pms = ledgerTransactionPOCOs.Select(poco => this.GetEntityPM(poco)).ToList();
            var pms= q.ToList();
            return pms;
        }


        public List<LedgerTransactionPM> GetLedgerTransactionPMsByIdList(List<string> idList, int tenant)
        {
            List<LedgerTransaction> ledgerTransactionPOCOs = null;
            ledgerTransactionPOCOs = repository.GetLedgerTransactionsByIdList(idList, tenant);
            List<LedgerTransactionPM> pms = ledgerTransactionPOCOs.Select(poco => this.GetEntityPM(poco)).ToList();
            return pms;
        }


        public List<LedgerTransactionJournalLineLT> GetLedgerTransactionJournalLineLTsByIdList(List<string> idList, int tenant)
        {
            List<LedgerTransactionJournalLineLT> LTs = repository.GetLedgerTransactionJournalLineLTsByIdList(idList, tenant);
            return LTs;
        }


        public IQueryable<string> GetJournalIdsByIdList(List<string> idList, int tenant)
        {
            List<LedgerTransaction> ledgerTransactionPOCOs = null;
            ledgerTransactionPOCOs = repository.GetLedgerTransactionsByIdList(idList, tenant);
            IQueryable<string> jIds = ledgerTransactionPOCOs.Where(poco => !String.IsNullOrEmpty(poco.JournalId)).Select(poco => poco.JournalId).AsQueryable<string>();
            return jIds;
        }

     
 


        public List<LedgerTransactionJournalLineLT> GetAPInvoiceLedgerTransactionsByIdList(List<String> ledgerTransactionIds, int tenant)
        {
            List<LedgerTransactionJournalLineLT> ledgerTransactionLineLTs = repository.GetAPInvoiceLedgerTransactionsByIdList(ledgerTransactionIds, tenant);

            return ledgerTransactionLineLTs;
        }
 
        public List<LedgerTransactionPM> GetLedgerTransactionDTOByIdList(List<string> idList, int tenant)
        {
            var pocos= repository.GetLedgerTransactionsByIdList(idList, tenant);
            var pms = pocos.Select(x => new LedgerTransactionPM()
            {
                Id = x.Id,
                AccountId = x.AccountId,
                InReconcileProgress = x.InReconcileProgress,
                IsReconciled = x.IsReconciled,
                JournalId = x.JournalId,
                JournalLineNumber = x.JournalLineNumber,
                OpenAmount = x.OpenAmount,
                ForeignAmountCredit = x.ForeignAmountCredit,
                LocalAmountCredit = x.LocalAmountCredit,
                ForeignAmountDebit = x.ForeignAmountDebit,
                LocalAmountDebit = x.LocalAmountDebit,
                OpenAmountCurrencyId = x.OpenAmountCurrencyId,
            }).ToList();
            return pms;
        }


        public bool CheckAnyLedgerTransactionReconciledByIdList(List<string> idList, int tenant)
        {
            return repository.CheckAnyLedgerTransactionReconciledByIdList(idList, tenant);
        }
        public List<LedgerTransactionPM> GetLedgerTransactionsByAccountIdListAndJournalId(List<string> accountIdList,string journalId, int tenant)
        {
            List<LedgerTransaction> ledgerTransactionPOCOs = null;
            ledgerTransactionPOCOs = repository.GetLedgerTransactionsByAccountIdListAndJournalId(accountIdList, journalId, tenant);
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

        public List<string> GetTransactionsCurrencies(string accountId, int tenant)
        {
            var transactionsCurrencies = repository.GetTransactionsCurrencies(accountId, tenant);


            return transactionsCurrencies;

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
            var firstDayOfFromDate = new DateTime(fromDate.Year, 1, 1);
            IQueryable<B100Data> transactionsQuery;
            if (firstDayOfFromDate == fromDate.Date)
            {
                transactionsQuery = GettransactionsQueryWithValidateFirsDate(firstDayOfFromDate, fromDate, toDate, tenant);
            }
            else
            {
                transactionsQuery = GettransactionsQuery(fromDate, toDate, tenant);
            }
            transactions = transactionsQuery.ToList();
            
            return transactions;
        }

        private static readonly Dictionary<string, Func<LedgerTransaction, Journal, int, string, string>> referenceTypeResolvers =
           new Dictionary<string, Func<LedgerTransaction, Journal, int, string, string>>
           {
                { Logitude.Accounting.Data.Enums.AccountingEntityValues.ARInvoice, (lt, jr, tenant, fallback) => OpenFormatDocumentTypes.TaxInvoice },
                { Logitude.Accounting.Data.Enums.AccountingEntityValues.ARPayment, (lt, jr, tenant, fallback) => OpenFormatDocumentTypes.Receipt },
                { Logitude.Accounting.Data.Enums.AccountingEntityValues.Journal, (lt, jr, tenant, fallback) => GetJournalReferenceType(lt, jr, tenant, fallback) },
                { Logitude.Accounting.Data.Enums.AccountingEntityValues.ChequeDeposit, (lt, jr, tenant, fallback) => GetDepositReferenceType(jr, tenant, fallback) },
                { Logitude.Accounting.Data.Enums.AccountingEntityValues.CashDeposit, (lt, jr, tenant, fallback) => GetDepositReferenceType(jr, tenant, fallback) },
          };

        private static string GetJournalReferenceType(LedgerTransaction lt, Journal jr, int tenant, string fallback)
        {
            if (jr == null)
            {
                return fallback;
            }
            var paymentMarkers = new HashSet<string>{ "PAYMENT:", "PAY:", "CHQ:" };
            return !String.IsNullOrEmpty(jr.ExternalSystem) && !String.IsNullOrEmpty(lt.Notes) && paymentMarkers.Any(pm => lt.Notes.Contains(pm))
                        ? OpenFormatDocumentTypes.AgentInvoice
                        : fallback;
        }


        private static string GetDepositReferenceType(Journal jr, int tenant, string fallback)
        {
            if (jr == null)
            {
                return fallback;
            }

            return jr.StatusCode == JournalStatuses.Voided
                        ? OpenFormatDocumentTypes.CashOut
                        : OpenFormatDocumentTypes.BankDeposit;
        }

        public string ResolveReferenceType(LedgerTransaction lt, Journal jr, int tenant, string fallback)
        {
            if (jr != null && referenceTypeResolvers.TryGetValue(jr.AccountingEntityCode, out var dateResolver))
            {
                return dateResolver(lt, jr, tenant, fallback);
            }
            return fallback;
        }

        private IQueryable<B100Data> GettransactionsQuery(DateTime fromDate, DateTime toDate, int tenant)
        {

            var preQuery =
                (from lt in context.LedgerTransactions
                    join g in context.GLAccounts on lt.AccountId equals g.Id
                    join j in context.Journals on lt.JournalId equals j.Id
                    join cancelledj in context.Journals on j.OriginalJournalId equals cancelledj.Id into cancelledjJoin
                    from cancelledj in cancelledjJoin.DefaultIfEmpty() // Left outer join
                    where ((lt.AccountingDate >= fromDate && lt.AccountingDate <= toDate)) && lt.Tenant == tenant
                    select new
                    {
                        LedgerTransaction = lt,
                        Journal = j,
                        CancelledJournal = cancelledj,
                        GLAccount = g
                    })
                    .AsEnumerable() // materialize into memory - EF bug workaround (placing ResolveReferenceType in the LINQ-to-Entities query causes an exception)
                    .Select(x => new B100Data
                    {
                        AccountingDate = x.LedgerTransaction.AccountingDate,
                        DocumentDate = x.LedgerTransaction.DocumentDate,
                        AccountingEntityCode = x.Journal.AccountingEntityCode,
                        AccountingEntityReference = x.Journal.AccountingEntityReference,
                        AccountingEntityReferenceType = ResolveReferenceType(
                                                x.LedgerTransaction,
                                                x.CancelledJournal ?? x.Journal,
                                                tenant,
                                                OpenFormatDocumentTypes.Fallback
                                            ),
                        ForeignAmountCredit = x.LedgerTransaction.ForeignAmountCredit,
                        ForeignAmountDebit = x.LedgerTransaction.ForeignAmountDebit,
                        GLAccountDisplayNumber = x.GLAccount.DisplayNumber,
                        LocalAmountCredit = x.LedgerTransaction.LocalAmountCredit,
                        LocalAmountDebit = x.LedgerTransaction.LocalAmountDebit,
                        CreateDate = x.LedgerTransaction.CreateDate ?? DateTime.MinValue,
                        CurrencyId = x.LedgerTransaction.CurrencyId,
                        CreatedByUser = x.Journal.CreatedByUserId,
                        JournalLineNumber = x.LedgerTransaction.JournalLineNumber,
                        JournalNumber = x.Journal.JournalNumber,
                        Notes = x.LedgerTransaction.Notes,
                        Reference2 = x.LedgerTransaction.Reference2,
                        LedgerTransactionId = x.LedgerTransaction.Id,
                        OppositGLAccount = x.LedgerTransaction.OppositeAccount != null ? x.LedgerTransaction.OppositeAccount.DisplayNumber : null,
                    });
            var result = preQuery.AsQueryable();
            return result;
        }

        private IQueryable<B100Data> GettransactionsQueryWithValidateFirsDate(DateTime firstDayOfFromDate, DateTime fromDate, DateTime toDate, int tenant)
        {
            var preQuery = 
                (from lt in context.LedgerTransactions
                    join g in context.GLAccounts on lt.AccountId equals g.Id
                    join j in context.Journals on lt.JournalId equals j.Id
                    join cancelledj in context.Journals on j.OriginalJournalId equals cancelledj.Id into cancelledjJoin
                    from cancelledj in cancelledjJoin.DefaultIfEmpty() // Left outer join
                    join f in context.FullAccountingSettings on lt.Tenant equals f.Tenant
                    where ((lt.AccountingDate >= fromDate && lt.AccountingDate <= toDate))
                        && lt.Tenant == tenant
                        && (DbFunctions.TruncateTime(lt.AccountingDate) != firstDayOfFromDate || (DbFunctions.TruncateTime(lt.AccountingDate) == firstDayOfFromDate 
                                && !(j.AccountingEntityCode == Logitude.Accounting.Data.Enums.AccountingEntityValues.YearTransfer & (g.ChartOfAccountsTypeCode == "1" || g.ChartOfAccountsTypeCode == "2" || g.Id == f.RevenueExpenseGLAccountId))))

                    select new
                    {
                        LedgerTransaction = lt,
                        Journal = j,
                        CancelledJournal = cancelledj,
                        GLAccount = g
                    })
                    .ToList() // materialize into memory - EF bug workaround (placing ResolveReferenceType in the LINQ-to-Entities query causes an exception)
                    .Select(x => new B100Data
                    {
                        AccountingDate = x.LedgerTransaction.AccountingDate,
                        DocumentDate = x.LedgerTransaction.DocumentDate,
                        AccountingEntityCode = x.Journal.AccountingEntityCode,
                        AccountingEntityReference = x.Journal.AccountingEntityReference,
                        AccountingEntityReferenceType = ResolveReferenceType(
                                                x.LedgerTransaction,
                                                x.CancelledJournal ?? x.Journal,
                                                tenant,
                                                OpenFormatDocumentTypes.Fallback
                                            ),
                        ForeignAmountCredit = x.LedgerTransaction.ForeignAmountCredit,
                        ForeignAmountDebit = x.LedgerTransaction.ForeignAmountDebit,
                        GLAccountDisplayNumber = x.GLAccount.DisplayNumber,
                        LocalAmountCredit = x.LedgerTransaction.LocalAmountCredit,
                        LocalAmountDebit = x.LedgerTransaction.LocalAmountDebit,
                        CreateDate =  x.LedgerTransaction.CreateDate ?? DateTime.MinValue,
                        CurrencyId = x.LedgerTransaction.CurrencyId,
                        CreatedByUser = x.Journal.CreatedByUserId,
                        JournalLineNumber = x.LedgerTransaction.JournalLineNumber,
                        JournalNumber = x.Journal.JournalNumber,
                        Notes = x.LedgerTransaction.Notes,
                        Reference2 = x.LedgerTransaction.Reference2,
                        LedgerTransactionId = x.LedgerTransaction.Id,
                        OppositGLAccount = x.LedgerTransaction.OppositeAccount != null ? x.LedgerTransaction.OppositeAccount.DisplayNumber : null,
                    });
            var result = preQuery.AsQueryable();
            return result;
        }

        public List<LedgerTransactionPM> GetTransactionBySourceEntity(string entityId,string sourceTypeCode, int tenant)
        {
            List<LedgerTransaction> transactions = repository.GetTransactionsBySourceId(entityId, sourceTypeCode, tenant);
            return transactions.Select(poco => GetEntityPM(poco)).ToList();
        }

        public List<ReconciliationPM> GetReconciliationsByJournalId(string journalId, int tenant)
        {
            var ledgerTranasctions = GetByJournalId(journalId, tenant);
            var reconLines = new List<ReconciliationLinePM>();
            if (ledgerTranasctions.Any())
            {
                reconLines = GetReconciliationLinesForTransactions(tenant, ledgerTranasctions);
            }
            var reconciliations = new List<ReconciliationPM>();
            if (reconLines.Any())
            {
                List<string> recosIds = reconLines.Select(d => d.ReconciliationId).Distinct().ToList();
                ReconciliationQueryService recoQuery = new ReconciliationQueryService(tenant);
                reconciliations = recoQuery.GetReconciliationsByIds(recosIds, tenant);
            }

            return reconciliations;
        }

        public List<LedgerTransactionPM> GetReconciledInvoicesTransactionsForARPayment(string arpaymentId, string billToGLAccountId, int tenant)
        {
            List<LedgerTransactionPM> reconciledTransactions = new List<LedgerTransactionPM>(); // result
            LedgerTransactionQueryService transactionsQuery = new LedgerTransactionQueryService(tenant);
            ReconciliationLineQueryService recoLineQuery = new ReconciliationLineQueryService(tenant);

            LedgerTransaction paymentTransaction = GetPaymentTransaction(arpaymentId, tenant);

            if (paymentTransaction != null)
            {
                string paymentTransactionId = paymentTransaction.Id;

                List<ReconciliationLinePM> recoLines = recoLineQuery.GetLinesByReconciledWithTransactionIdWithoutMapping(paymentTransactionId, tenant);

                List<string> recoLinesTransactionsId = recoLines.Select(d => d.TransactionId).ToList();
                 reconciledTransactions = transactionsQuery.GetLedgerTransactionPMsByIdList(recoLinesTransactionsId, tenant);

                reconciledTransactions = reconciledTransactions.Where(d => d.IsReconciled == true && d.SourceTypeCode == Logitude.Accounting.Data.Enums.AccountingEntityValues.ARInvoice).ToList();

                reconciledTransactions = FillTransactionsReconciliationNumbers(reconciledTransactions, tenant);
                reconciledTransactions = FillReconciledPaymentTransactionAmount(reconciledTransactions, paymentTransaction != null ? paymentTransaction.Id : null, tenant);

            }
            return reconciledTransactions;

        }

        public List<LedgerTransactionJournalLineLT> GetReconciledInvoicesTransactionsForARPaymentLT(string arpaymentId, string billToGLAccountId, int tenant)
        {
            List<LedgerTransactionJournalLineLT> reconciledTransactions = new List<LedgerTransactionJournalLineLT>(); // result
            LedgerTransactionQueryService transactionsQuery = new LedgerTransactionQueryService(tenant);
            ReconciliationLineQueryService recoLineQuery = new ReconciliationLineQueryService(tenant);

            LedgerTransaction paymentTransaction = GetPaymentTransaction(arpaymentId, tenant);

            if (paymentTransaction != null)
            {
                string paymentTransactionId = paymentTransaction.Id;

                List<ReconciliationLinePM> recoLines = recoLineQuery.GetLinesByReconciledWithTransactionIdWithoutMapping(paymentTransactionId, tenant);

                List<string> recoLinesTransactionsId = recoLines.Select(d => d.TransactionId).ToList();
                // reconciledTransactions = transactionsQuery.GetLedgerTransactionPMsByIdList(recoLinesTransactionsId, tenant);
                reconciledTransactions = transactionsQuery.GetLedgerTransactionJournalLineLTsByIdList(recoLinesTransactionsId, tenant);

                reconciledTransactions = reconciledTransactions.Where(d => d.IsReconciled == true && d.SourceTypeCode == Logitude.Accounting.Data.Enums.AccountingEntityValues.ARInvoice).ToList();

                reconciledTransactions = FillTransactionsReconciliationNumbersLT(reconciledTransactions, tenant);
                reconciledTransactions = FillReconciledPaymentTransactionAmountLT(reconciledTransactions, paymentTransaction != null ? paymentTransaction.Id : null, tenant);

            }
            return reconciledTransactions;

        }
 

        public LedgerTransaction GetPaymentTransaction(string arpaymentId, int tenant)
        {
            JournalQueryService journalQueryService = new JournalQueryService(tenant);
            JournalPM paymentJournal = journalQueryService.GetJournalsByAccountingEntityIdAndCode(arpaymentId, AccountingEntities.ARPayment, tenant).FirstOrDefault();

            LedgerTransaction paymentCreditTransaction = GetCreditTransactionByJournalId(paymentJournal.Id, tenant);

            return paymentCreditTransaction;

        }
       
        public List<LedgerTransactionPM> GetOpenInvoicesTransactionsForAccount(string billToGLAccountId, string arpaymentId, int tenant)
       {
            IQueryable<LedgerTransactionPM> transactions = GetTransactionsJoinedWithJounrals();

            LedgerTransaction paymentTransaction = GetPaymentTransaction(arpaymentId, tenant);

            //IQueryable<LedgerTransactionPM> paymentTransactionIQ = transactions.Where(t => t.SourceId == arpaymentId && t.SourceTypeCode == "3");
            //List<LedgerTransactionPM> paymentTransactions = paymentTransactionIQ.ToList();
            //LedgerTransactionPM paymentTransaction = paymentTransactionIQ.FirstOrDefault();

            var invoicesTransactions = GetInvoicesTransactions(tenant, AccountingEntities.ARInvoice);

            // filter transactions by account and source type
            invoicesTransactions = invoicesTransactions
                    .Where(d =>
                        d.AccountId == billToGLAccountId
                        && d.Tenant == tenant
                        && d.IsReconciled == false
                        && d.SourceTypeCode == Logitude.Accounting.Data.Enums.AccountingEntityValues.ARInvoice)
                    .OrderBy(b => b.AccountingDate).ThenByDescending(b => b.JournalId);

            var transactionsList = invoicesTransactions.ToList();

            List<LedgerTransactionPM> openInvoicesTransactions = FillTransactionsReconciliationNumbers(transactionsList, tenant);
            openInvoicesTransactions = FillReconciledPaymentTransactionAmount(openInvoicesTransactions.ToList(), paymentTransaction != null ? paymentTransaction.Id : null, tenant);

            return openInvoicesTransactions;
        }

 
 

        public List<LedgerTransactionJournalLineLT> GetOpenInvoicesTransactionsForAccountLT(string billToGLAccountId, string arpaymentId, int tenant)
        {
            IQueryable<LedgerTransactionPM> transactions = GetTransactionsJoinedWithJounrals();

            LedgerTransaction paymentTransaction = GetPaymentTransaction(arpaymentId, tenant);


            var invoicesTransactions = GetInvoicesTransactions(tenant, AccountingEntities.ARInvoice);

            // filter transactions by account and source type
            invoicesTransactions = invoicesTransactions
                    .Where(d =>
                        d.AccountId == billToGLAccountId
                        && d.Tenant == tenant
                        && d.IsReconciled == false
                        && d.SourceTypeCode == Logitude.Accounting.Data.Enums.AccountingEntityValues.ARInvoice)
                    .OrderBy(b => b.AccountingDate).ThenByDescending(b => b.JournalId);

            var transactionsList = invoicesTransactions.ToList();

            List<LedgerTransactionPM> openInvoicesTransactions = FillTransactionsReconciliationNumbers(transactionsList, tenant);
            openInvoicesTransactions = FillReconciledPaymentTransactionAmount(openInvoicesTransactions.ToList(), paymentTransaction != null ? paymentTransaction.Id : null, tenant);

            List<LedgerTransactionJournalLineLT> rv = new List<LedgerTransactionJournalLineLT>();
            if (openInvoicesTransactions != null && openInvoicesTransactions.Count > 0)
            {
                foreach (var tr in openInvoicesTransactions)
                {
                    LedgerTransactionJournalLineLT lt = new LedgerTransactionJournalLineLT
                    {
                        Id = tr.Id,
                        AccountId = tr.AccountId,
                        PaymentReconciledAmount = tr.PaymentReconciledAmount,

                    };

                    rv.Add(lt);
                }
            }


            return rv;
        }
 
        /// <summary>
        /// Returns any invoice transaction which is reconciled with the payment. --Abdullah
        /// </summary>
        /// <param name="arpaymentId">Payment id</param>
        /// <param name="billToGLAccountId">Payment bill to account id</param>
        /// <param name="tenant">Tenant</param>
        /// <returns>List of invoices ledger transactions</returns>

        //public List<LedgerTransactionPM> GetReconciledInvoicesTransactionsForPayment(string arpaymentId, string billToGLAccountId, int tenant) // reconciled and partially reconciled
        //{
        //    List<LedgerTransactionPM> reconciledInvoicesTransactions = GetReconciledInvoicesTransactionsForARPayment(arpaymentId, billToGLAccountId, tenant);
        //    List<LedgerTransactionPM> openedAndPartiallyInvoicesTransactions = GetOpenInvoicesTransactionsForAccount(billToGLAccountId, arpaymentId, tenant);

        //    List<LedgerTransactionPM> reconciledTransactions
        //        = reconciledInvoicesTransactions.Concat(
        //            openedAndPartiallyInvoicesTransactions.Where(d => d.PaymentReconciledAmount != 0) // partially invoices reconciled with this payment
        //            ).ToList();

        //    return reconciledTransactions;
        //}
        public List<LedgerTransactionJournalLineLT> GetReconciledInvoicesTransactionsForPaymentLT(string arpaymentId, string billToGLAccountId, int tenant) // reconciled and partially reconciled
        {
            List<LedgerTransactionJournalLineLT> reconciledInvoicesTransactions = GetReconciledInvoicesTransactionsForARPaymentLT(arpaymentId, billToGLAccountId, tenant);
            List<LedgerTransactionJournalLineLT> openedAndPartiallyInvoicesTransactions = GetOpenInvoicesTransactionsForAccountLT(billToGLAccountId, arpaymentId, tenant);

            List<LedgerTransactionJournalLineLT> reconciledTransactions 
                = reconciledInvoicesTransactions.Concat(
                    openedAndPartiallyInvoicesTransactions.Where(d => d.PaymentReconciledAmount != 0) // partially invoices reconciled with this payment
                    ).ToList();

            return reconciledTransactions;
        }

        private List<LedgerTransactionPM> FillTransactionsReconciliationNumbers(List<LedgerTransactionPM> invoicesTransactions, int tenant)
        {
            List<ReconciliationLinePM> recoLines = GetReconciliationLinesForTransactions(tenant, invoicesTransactions);
            List<ReconciliationPM> reconciliationsOnPaymentInvoices = GetReconciliationsByReconcileLines(tenant, recoLines);

            foreach (LedgerTransactionPM invoiceTransaction in invoicesTransactions)
            {
                string recoNumbers = GetReconciliationNumbersForInvoice(recoLines, reconciliationsOnPaymentInvoices, invoiceTransaction);
                invoiceTransaction.RecoNumber = recoNumbers;
            }

            return invoicesTransactions;
        }



        public List<LedgerTransaction> GetTransactionsDeduction(string whAccountId, DateTime startDate, DateTime endDate, int tenant)
        {
             var dtos = repository.GetTransactionsDeductionDTO(whAccountId, startDate, endDate, tenant).ToList();

            return dtos.Select(dto => new LedgerTransaction
            {
                Id = dto.Id,
                AccountId = dto.AccountId,
                OppositeAccountId = dto.OppositeAccountId,
                JournalId = dto.JournalId,
                JournalLineNumber = dto.JournalLineNumber ?? 0,
                LocalAmountCredit = dto.LocalAmountCredit,
                LocalAmountDebit = dto.LocalAmountDebit,
                Reference1 = dto.Reference1,
                AccountingDate = dto.AccountingDate,
                Tenant = dto.Tenant
             }).ToList();
        }


        private List<LedgerTransactionJournalLineLT> FillTransactionsReconciliationNumbersLT(List<LedgerTransactionJournalLineLT> invoicesTransactions, int tenant)
        {
            List<ReconciliationLinePM> recoLines = GetReconciliationLinesForTransactionsLT(tenant, invoicesTransactions);
            List<ReconciliationPM> reconciliationsOnPaymentInvoices = GetReconciliationsByReconcileLines(tenant, recoLines);

            foreach (LedgerTransactionJournalLineLT invoiceTransaction in invoicesTransactions)
            {
                string recoNumbers = GetReconciliationNumbersForInvoiceLT(recoLines, reconciliationsOnPaymentInvoices, invoiceTransaction);
                invoiceTransaction.RecoNumber = recoNumbers;
            }

            return invoicesTransactions;
        }

        private static List<ReconciliationLinePM> GetRecoLinesByTransactionsIds(int tenant, List<string> transactionsIds)
        {
            ReconciliationLineQueryService recoLineQuery = new ReconciliationLineQueryService(tenant);
            List<ReconciliationLinePM> recoLines = recoLineQuery.GetLineByTransactionIds(transactionsIds, tenant);
            return recoLines;
        }

        private static string GetReconciliationNumbersForInvoice(List<ReconciliationLinePM> recoLines, List<ReconciliationPM> reconciliationsOnPaymentInvoices, LedgerTransactionPM invoiceTransaction)
        {
            List<ReconciliationLinePM> transactionRecoLines = recoLines
                                .Where(d => d.TransactionId == invoiceTransaction.Id).ToList();

            List<string> recoNumbers = transactionRecoLines.Select(a => a.ReconciliationId).ToList();
            List<ReconciliationPM> reconciliationsForInvoice = reconciliationsOnPaymentInvoices.Where(d => recoNumbers.Contains(d.Id)).ToList();

            string[] reconciliationsNumberForInvoice = reconciliationsForInvoice
                .Where(d => d.IsCancelled == false).Select(d => d.Number).ToArray();

            string numbersString = string.Join(",", reconciliationsNumberForInvoice);
            return numbersString;
        }

    

        private static string GetReconciliationNumbersForInvoiceLT(List<ReconciliationLinePM> recoLines, List<ReconciliationPM> reconciliationsOnPaymentInvoices, LedgerTransactionJournalLineLT invoiceTransaction)
        {
            List<ReconciliationLinePM> transactionRecoLines = recoLines
                                .Where(d => d.TransactionId == invoiceTransaction.Id).ToList();

            List<string> recoNumbers = transactionRecoLines.Select(a => a.ReconciliationId).ToList();
            List<ReconciliationPM> reconciliationsForInvoice = reconciliationsOnPaymentInvoices.Where(d => recoNumbers.Contains(d.Id)).ToList();

            string[] reconciliationsNumberForInvoice = reconciliationsForInvoice
                .Where(d => d.IsCancelled == false).Select(d => d.Number).ToArray();

            string numbersString = string.Join(",", reconciliationsNumberForInvoice);
            return numbersString;
        }

        private List<ReconciliationLinePM> GetReconciliationLinesForTransactions(int tenant, List<LedgerTransactionPM> invoicesTransactions)
        {
            List<string> transactionsIds = invoicesTransactions.Select(d => d.Id).ToList();

            ReconciliationLineQueryService recoLineQuery = new ReconciliationLineQueryService(tenant);
            List<ReconciliationLinePM> recoLines = recoLineQuery.GetLinesByTransactionIdsWithoutMapping(transactionsIds, tenant).ToList();
            return recoLines;
        }

        private List<ReconciliationLinePM> GetReconciliationLinesForTransactionsLT(int tenant, List<LedgerTransactionJournalLineLT> invoicesTransactions)
        {
            List<string> transactionsIds = invoicesTransactions.Select(d => d.Id).ToList();

            ReconciliationLineQueryService recoLineQuery = new ReconciliationLineQueryService(tenant);
            List<ReconciliationLinePM> recoLines = recoLineQuery.GetLinesByTransactionIdsWithoutMapping(transactionsIds, tenant).ToList();
            return recoLines;
        }

     

        private List<ReconciliationPM> GetReconciliationsByReconcileLines(int tenant, List<ReconciliationLinePM> recoLines)
        {
            List<string> recosIds = recoLines.Select(d => d.ReconciliationId).ToList();
            ReconciliationQueryService recoQuery = new ReconciliationQueryService(tenant);
            List<ReconciliationPM> recos = recoQuery.GetLightReconciliationsByIds(recosIds, tenant);

            return recos;
        }


        private List<LedgerTransactionPM> FillReconciledPaymentTransactionAmount(List<LedgerTransactionPM> transactions, string paymentTransactionId, int tenant)
        {
            List<LedgerTransactionPM> invoicesTransactions = transactions.ToList();

            List<ReconciliationLinePM> recoLines = GetReconciliationLinesForTransactions(tenant, invoicesTransactions);

            foreach (LedgerTransactionPM invoiceTransaction in invoicesTransactions)
            {
                List<ReconciliationLinePM> transactionRecoLines = recoLines.Where(d => d.TransactionId == invoiceTransaction.Id).ToList();

                decimal reconciledAmount = 0;
                transactionRecoLines.ForEach(recoLine =>
                {
                    if (recoLine.ReconciledWithTransactionId == paymentTransactionId && paymentTransactionId != null && recoLine.IsRecoCancelled == false)
                        reconciledAmount += recoLine.ReconciliationAmount;
                });

                invoiceTransaction.PaymentReconciledAmount = reconciledAmount;

            }

            return invoicesTransactions;
        }

        private List<LedgerTransactionJournalLineLT> FillReconciledPaymentTransactionAmountLT(List<LedgerTransactionJournalLineLT> transactions, string paymentTransactionId, int tenant)
        {
            List<LedgerTransactionJournalLineLT> invoicesTransactions = transactions.ToList();

            List<ReconciliationLinePM> recoLines = GetReconciliationLinesForTransactionsLT(tenant, invoicesTransactions);

            foreach (LedgerTransactionJournalLineLT invoiceTransaction in invoicesTransactions)
            {
                List<ReconciliationLinePM> transactionRecoLines = recoLines.Where(d => d.TransactionId == invoiceTransaction.Id).ToList();

                decimal reconciledAmount = 0;
                transactionRecoLines.ForEach(recoLine =>
                {
                    if (recoLine.ReconciledWithTransactionId == paymentTransactionId && paymentTransactionId != null && recoLine.IsRecoCancelled == false)
                        reconciledAmount += recoLine.ReconciliationAmount;
                });

                invoiceTransaction.PaymentReconciledAmount = reconciledAmount;

            }

            return invoicesTransactions;
        }

        public decimal? GetTotalOpenChequesLocalAmount(string accountId) {
            var journallines = from jl in context.JournalLines
                               join j in context.Journals on jl.JournalId equals j.Id
                               join arpch in context.ARPaymentCheques on j.AccountingEntityId equals arpch.PaymentId
                               where j.AccountingEntityCode == "3" && jl.ActionCode == "1" && j.IsLedgerCreated == true
                               && jl.CreditAccountId == accountId
                               select jl;
            return ((decimal?)(journallines).Distinct().Sum(s => (decimal?)s.LocalAmount)).GetValueOrDefault();
        }
        public decimal? GetTotFutureOpenChequesLocalAmount(string accountId, DateTime endOfTodayDate)
        {
            var journallines = from jl in context.JournalLines
                               join j in context.Journals on jl.JournalId equals j.Id
                               join arpch in context.ARPaymentCheques on j.AccountingEntityId equals arpch.PaymentId
                               where j.AccountingEntityCode == "3" && jl.ActionCode == "1" && j.IsLedgerCreated == true
                               && jl.CreditAccountId == accountId && jl.DueDate > endOfTodayDate
                               select jl;

            return ((decimal?)(journallines).Distinct().Sum(s => (decimal?)s.LocalAmount)).GetValueOrDefault();
        }

        public IQueryable<LedgerTransactionPM> GetTransactionsJoinedWithJounrals()
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
                    CurrencyCode = _transaction.Currency.Code,
                    //Source = _transaction.Source,
                    //SourceType = _journal.accounting,
                    OpenAmountCurrencyId = _transaction.OpenAmountCurrencyId,
                    Notes = _transaction.Notes,
                    InternalNote= _transaction.InternalNote,
                    UpdateDateTime = _transaction.UpdateDateTime,
                    UpdatedByUserName = _transaction.UpdatedByUserName,
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
        public IQueryable<LedgerTransactionPM> GetInvoicesTransactions(int tenant, string accountingEntityCode)
        {

            IQueryable<LedgerTransactionPM> query =
                from _transaction in context.LedgerTransactions

                join _journal in context.Journals
                on _transaction.JournalId equals _journal.Id

                where _journal.AccountingEntityCode == accountingEntityCode
                    && _journal.Tenant == tenant

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
                    CurrencyCode = _transaction.Currency.Code,
                    //Source = _transaction.Source,
                    //SourceType = _journal.accounting,
                    OpenAmountCurrencyId = _transaction.OpenAmountCurrencyId,
                    Notes = _transaction.Notes,
                    InternalNote=_transaction.InternalNote,
                    UpdateDateTime = _transaction.UpdateDateTime,
                    UpdatedByUserName = _transaction.UpdatedByUserName,
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

        public IQueryable<LedgerTransaction> GetLedgerTransactionsForMonth(int year, int month, int tenant)
        {
            IQueryable<LedgerTransaction> pocos = repository.GetTransactionsForMonth(year, month, tenant);
            return pocos;
        }

        public IQueryable<LedgerTransaction> GetLedgerTransactionsForMonthBySourceTypeMode(int year, int month, int tenant, string sourceTypeCode, string mode)
        {
            //IQueryable<LedgerTransaction> pocos = repository.GetTransactionsForMonthAndSourceTypeMode(year, month, tenant, sourceTypeCode, mode);
            IQueryable<LedgerTransaction> pocos = GetTransactionsForMonthAndSourceTypeMode(year, month, tenant, sourceTypeCode, mode);
            return pocos;
        }


 
     

 
    


        public IQueryable<LedgerTransaction> GetTransactionsForMonthAndSourceTypeMode(int year, int month, int tenant, string sourceTypeCode, string mode)
        {
            DateTime monthStart = new DateTime(year, month, 1, 0, 0, 0);
            DateTime monthEnd = new DateTime(year, month, DateTime.DaysInMonth(year, month), 23, 59, 59);
            IQueryable<LedgerTransaction> pocos = null;
            if (sourceTypeCode == "!=2") // Only Non-Invoice
            {
                pocos =
                    (from lt in context.LedgerTransactions
                     join journal in context.Journals on lt.JournalId equals journal.Id
                     where lt.AccountingDate >= monthStart
                        && lt.AccountingDate <= monthEnd
                        && lt.Tenant == tenant
                        && journal.Tenant == tenant
                        && journal.AccountingEntityCode != "2"
                     select lt).OrderByDescending(a => a.AccountingDate);
            }
            else
            {
                List<string> invs = null;
                ARInvoiceRepository aRInvoiceRepository = new ARInvoiceRepository(tenant);


                if (mode == "IT") // Only Interest Invoice
                {
                    invs = GetListFirstInterestInvoiceIdByMonth(aRInvoiceRepository, monthStart, monthEnd, tenant);
                    if (invs == null) invs = new List<string>();
                    pocos =
                        (from lt in context.LedgerTransactions
                         join journal in context.Journals on lt.JournalId equals journal.Id
                         join inv in invs on journal.AccountingEntityId equals inv
                         where lt.AccountingDate >= monthStart
                            && lt.AccountingDate <= monthEnd
                            && lt.Tenant == tenant
                            && journal.Tenant == tenant
                            && journal.AccountingEntityCode == "2"
                         select lt).OrderByDescending(a => a.AccountingDate);
                }
                else // Only Non-Interest Invoice
                {
                    invs = GetListFirstNonInterestInvoiceIdByMonth(aRInvoiceRepository, monthStart, monthEnd, tenant);
                    if (invs == null) invs = new List<string>();
                    pocos =
                        (from lt in context.LedgerTransactions
                         join journal in context.Journals on lt.JournalId equals journal.Id
                         join inv in invs on journal.AccountingEntityId equals inv
                         where lt.AccountingDate >= monthStart
                            && lt.AccountingDate <= monthEnd
                            && lt.Tenant == tenant
                            && journal.Tenant == tenant
                            && journal.AccountingEntityCode == "2"
                         select lt).OrderByDescending(a => a.AccountingDate);
                }

            }
            return pocos;
        }

         private List<string> GetListFirstInterestInvoiceIdByMonth(ARInvoiceRepository repository, DateTime monthStart, DateTime monthEnd, int tenant)
        {
            List<string> result = new List<string>();
            var q = (from a in repository.context.ARInvoices
                     where a.Tenant == tenant && a.ARInvoiceTypeCode == "IT"
                     && a.InvoiceDate >= monthStart
                     && a.InvoiceDate <= monthEnd
                     select a);
            if (q != null)
            {
                ARInvoice inv = q.FirstOrDefault();
                if (inv != null) result.Add(inv.Id);
            }
            return result;
        }

        private List<string> GetListFirstNonInterestInvoiceIdByMonth(ARInvoiceRepository repository, DateTime monthStart, DateTime monthEnd, int tenant)
        {
            List<string> result = new List<string>();
            var q = (from a in repository.context.ARInvoices
                     where a.Tenant == tenant && a.ARInvoiceTypeCode != "IT"
                     && a.InvoiceDate >= monthStart
                     && a.InvoiceDate <= monthEnd
                     select a);
            if (q != null)
            {
                ARInvoice inv = q.FirstOrDefault();
                if (inv != null) result.Add(inv.Id);
            }
            return result;
        }


        public List<LedgerTransactionPM> GetByJournalIdAndForeignAmountDebitNotEqualZero(string journalId, int tenant)
        {
            List<LedgerTransaction> ledgerTransactionPOCOs = null;
            ledgerTransactionPOCOs = repository.GetByJournalIdAndForeignAmountDebitNotEqualZero(journalId, tenant).ToList();
            List<LedgerTransactionPM> pms = ledgerTransactionPOCOs.Select(poco => this.GetEntityPM(poco)).ToList();
            return pms;
        }

        public bool ExistsLedgerTransactionByReferenceGLAccountId(string reference1, string gLAccountId, int tenant)
        {
            return repository.ExistsLedgerTransactionByReferenceGLAccountId(reference1, gLAccountId, tenant);
        }
    }
    public class JournalLineLedgerDTO
    {
        public string JournalId { get; set; }
        public string JournalNumber { get; set; }
        public int JournalLineNumber { get; set; }
        public string AccountId { get; set; }
        public string AccountDisplayNumber { get; set; }
        public string CurrencyId { get; set; }

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
    public class LedgerTransactionDTO
    {
        public string Id { get; internal set; }
        public string AccountId { get; internal set; }
        public bool InReconcileProgress { get; internal set; }
        public bool IsReconciled { get; internal set; }
        public string JournalId { get; internal set; }
        public decimal OpenAmount { get; internal set; }
        public decimal ForeignAmountCredit { get; internal set; }
        public decimal LocalAmountCredit { get; internal set; }
        public decimal ForeignAmountDebit { get; internal set; }
        public decimal LocalAmountDebit { get; internal set; }
        public string OpenAmountCurrencyId { get; internal set; }
    }


    public class GetNextLTArgs
    {
        public int Tenant { get; set; }
        public string AccountId { get; set; }
        public string LastCheckedId { get; set; }
        public DateTime ToAccountingDate { get; set; }
        public int ThisTimeMadeCount { get; set; }
        public bool Stop { get; set; }


    }
}
