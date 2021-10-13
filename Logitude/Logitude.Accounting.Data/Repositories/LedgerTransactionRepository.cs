 
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
using Simplog.Server.Infrastructure.Helpers;

using Logitude.Accounting.Data.DataContract;

namespace Logitude.Accounting.Data.Repositories
{
    public partial class LedgerTransactionRepository : IRepository<LedgerTransaction>
    {
        
        
        

        public List<LedgerTransaction> GetMulti(EntityKeyFields entityKeys)
        {
            LedgerTransactionKeys ledgerTransactionKeys = entityKeys as LedgerTransactionKeys;

            return (from a in context.LedgerTransactions
                    where a.AccountId == ledgerTransactionKeys.Id
                    select a).ToList();
        }
        public void ResetDraftOpenReconciliation(string gLAccountId, int tenant)
        {
            using (var scope = TransactionFactory.GetTransaction())

            //using (var context = new BloggingContext())
            {
                //using (var dbContextTransaction = 
                //    (context  as DbContext).Database.BeginTransaction())
                {
                    try
                    {
                        (context as System.Data.Entity.DbContext).Database.ExecuteSqlCommand(
  String.Format(
@"UPDATE LedgerTransactions SET Mark='false',AmountToReconcile=0
WHERE Mark='true' and AccountId='{0}' and tenant={1} ", gLAccountId, tenant)
                            );



                        context.SaveChanges();

                        //dbContextTransaction.Commit();
                        scope.Complete();
                    }
                    catch (Exception)
                    {
                        //dbContextTransaction.Rollback();
                        throw;
                    }
                }
            }
        }


        public bool CheckIfLedgerTransactionOtherCurrencyExist(string gLAccountId, string currencyId, int tenant)
        {
            bool exists;
            exists = (from a in context.LedgerTransactions
                      where a.AccountId == gLAccountId && a.Tenant == tenant && a.CurrencyId != currencyId
                      select a).Any();
            return exists;
        }



        public string GetAnyLedgerTransactionCurrency(string gLAccountId, int tenant)
        {
            string currencyId = null;
            LedgerTransaction record = (from a in context.LedgerTransactions
                                        where a.AccountId == gLAccountId && a.Tenant == tenant
                                        select a).FirstOrDefault();
            if (record != null)
            {
                currencyId = record.CurrencyId;
            }
            return currencyId;
        }
        public IQueryable<LedgerTransaction> GetByJournalId(string journalId, int tenant)
        {

            return (from a in context.LedgerTransactions
                    where a.JournalId == journalId && a.Tenant == tenant
                    select a);

        }
        public IQueryable<LedgerTransaction> GetByJournalAndReference1(string journalId, string reference1, int tenant)
        {

            return (from a in context.LedgerTransactions
                    where a.JournalId == journalId && a.Reference1 == reference1 && a.Tenant == tenant
                    select a);

        }

        public List<LedgerTransaction> GetByJournalIdAndLine(string journalId, int line, int tenant)
        {

            return (from a in context.LedgerTransactions
                    where a.JournalId == journalId && a.JournalLineNumber == line && a.Tenant == tenant
                    select a).ToList();
        }

        public List<LedgerTransaction> GetByAccountId(string accountId, int tenant)
        {
            return (from a in context.LedgerTransactions
                    where a.AccountId == accountId && a.Tenant == tenant
                    select a).ToList();

        }
        public string GetAnyLedgerTransactionByJournalId(string journalId, int tenant)
        {
            string ledgerTransactionId = null;
            LedgerTransaction record = (from a in context.LedgerTransactions
                                        where a.JournalId == journalId && a.Tenant == tenant
                                        select a).FirstOrDefault();
            if (record != null)
            {
                ledgerTransactionId = record.Id;
            }
            return ledgerTransactionId;
        }

        public IQueryable<LedgerTransaction> GetQBetweenDueInclusive(
            int tenant, //List<string> listOfAccId, 
            IQueryable<string> listOfAccId,
            DateTime fromDate, DateTime toDate)
        {
            var q = (from rec in context.LedgerTransactions
                     join accId in listOfAccId on rec.AccountId equals accId
                     //where listOfAccId.Contains(rec.AccountId) //less than 1000
                     where rec.Tenant == tenant
                     where rec.DueDate >= fromDate
                     where rec.DueDate <= toDate
                     select rec
                     );
            return q;
        }
        public IQueryable<LedgerTransaction> GetQDueGreaterthan(
            int tenant, 
            //List<string> listOfAccId, 
            IQueryable<string> listOfAccId,
            DateTime myDate)
        {
            var q = (from rec in context.LedgerTransactions
                     join accId in listOfAccId on rec.AccountId equals accId
                     //where listOfAccId.Contains(rec.AccountId) //less than 1000
                     where rec.Tenant == tenant
                     where rec.DueDate > myDate
                     select rec
                     );
            return q;
        }

        
             public IQueryable<LedgerTransaction> GetQueryByDateType(int tenant, 
                 IQueryable<string> listOfAccId, 
                 string DateTypeCode ,DateTime @from, DateTime to,
            string currencyId,
            string searchByFilter,
            DateTime? maxCreateDate)
        {
            var q = (from rec in context.LedgerTransactions
                     where rec.Tenant == tenant
                     where listOfAccId.Contains(rec.AccountId) //less than 1000
                     select rec
                     );

            if (!string.IsNullOrWhiteSpace(currencyId))
            {
                q = q.Where(rec => rec.CurrencyId == currencyId);
            }
            if (!string.IsNullOrWhiteSpace(searchByFilter))
            {
                q = q.Where(rec => rec.SearchFields.Contains(searchByFilter));
            }

            if (maxCreateDate.HasValue)
            {
                DateTime maxCreateDateReal = maxCreateDate.Value;
                q = q.Where(rec => rec.CreateDate <= maxCreateDateReal);
            }

            q = QFilterByDateTruncateTimeInclusive(DateTypeCode, from, to, q);

            return q;



        }

        private IQueryable<LedgerTransaction> QFilterByDateTruncateTimeInclusive(
            string DateTypeCode, DateTime from, DateTime to, IQueryable<LedgerTransaction> q
            )
        {
            if (context.ToString().StartsWith("Fake"))
            {


                switch (DateTypeCode)
                {
                    case "2"://GLAccountTotalDateTypeValues.DueDate:
                        {
                            
                            q = (from rec in q
                                 where rec.DueDate.Date >= @from
                                 where rec.DueDate.Date <= to
                                 select rec
);

                            q = (from rec in q
                                 orderby rec.DueDate, rec.Id
                                 select rec);
                        }
                        break;
                    case "3":// GLAccountTotalDateTypeValues.DocumentDate:
                        {
                            //return null;
                            q = (from rec in q
                                 where rec.DocumentDate.Date >= @from
                                 where rec.DocumentDate.Date <= to
                                 select rec
    );

                            q = (from rec in q
                                 orderby rec.DocumentDate, rec.Id
                                 select rec);
                        }
                        break;
                    case "1": //Accountingdate = "1"
                    default:
                        {
                            q = (from rec in q
                                 where rec.AccountingDate.Date >= @from
                                 where rec.AccountingDate.Date <= to
                                 select rec
                                );
                            q = (from rec in q
                                 orderby rec.AccountingDate, rec.Id
                                 select rec);
                        }
                        break;
                }

            }
            else
            {

                switch (DateTypeCode)
                {
                    case "2"://GLAccountTotalDateTypeValues.DueDate:
                        {
                            q = (from rec in q
                                 where EntityFunctions.TruncateTime(rec.DueDate) >= @from
                                 where EntityFunctions.TruncateTime(rec.DueDate) <= to
                                 select rec);
                            q = (from rec in q
                                 orderby rec.DueDate, rec.Id
                                 select rec);
                        }
                        break;
                    case "3":// GLAccountTotalDateTypeValues.DocumentDate:
                        {
                            //return null;
                            q = (from rec in q
                                 where EntityFunctions.TruncateTime(rec.DocumentDate) >= @from
                                 where EntityFunctions.TruncateTime(rec.DocumentDate) <= to
                                 select rec
                     );
                            q = (from rec in q
                                 orderby rec.DocumentDate, rec.Id
                                 select rec);
                        }
                        break;
                    case "1": //Accountingdate = "1"
                    default:
                        {
                            q = (from rec in q
                                 where EntityFunctions.TruncateTime(rec.AccountingDate) >= @from
                                 where EntityFunctions.TruncateTime(rec.AccountingDate) <= to
                                 select rec
                     );
                            q = (from rec in q
                                 orderby rec.AccountingDate, rec.Id
                                 select rec);
                        }
                        break;
                }

            }

            return q;
        }

        private IQueryable<LedgerTransaction> QFilterByDateTruncateTimeInclusiveUntil(
         string DateTypeCode, DateTime to, IQueryable<LedgerTransaction> q
         )
        {
            if (context.ToString().StartsWith("Fake"))
            {


                switch (DateTypeCode)
                {
                    case "2"://GLAccountTotalDateTypeValues.DueDate:
                        {

                            q = (from rec in q
                              
                                 where rec.DueDate.Date <= to
                                 select rec
);

                            q = (from rec in q
                                 orderby rec.DueDate, rec.Id
                                 select rec);
                        }
                        break;
                    case "3":// GLAccountTotalDateTypeValues.DocumentDate:
                        {
                            //return null;
                            q = (from rec in q
                                 
                                 where rec.DocumentDate.Date <= to
                                 select rec
    );

                            q = (from rec in q
                                 orderby rec.DocumentDate, rec.Id
                                 select rec);
                        }
                        break;
                    case "1": //Accountingdate = "1"
                    default:
                        {
                            q = (from rec in q
                                 //where rec.AccountingDate.Date >= @from
                                 where rec.AccountingDate.Date <= to
                                 select rec
                                );
                            q = (from rec in q
                                 orderby rec.AccountingDate, rec.Id
                                 select rec);
                        }
                        break;
                }

            }
            else
            {

                switch (DateTypeCode)
                {
                    case "2"://GLAccountTotalDateTypeValues.DueDate:
                        {
                            q = (from rec in q
                                 //where EntityFunctions.TruncateTime(rec.DueDate) >= @from
                                 where EntityFunctions.TruncateTime(rec.DueDate) <= to
                                 select rec);
                            q = (from rec in q
                                 orderby rec.DueDate, rec.Id
                                 select rec);
                        }
                        break;
                    case "3":// GLAccountTotalDateTypeValues.DocumentDate:
                        {
                            //return null;
                            q = (from rec in q
                                 //where EntityFunctions.TruncateTime(rec.DocumentDate) >= @from
                                 where EntityFunctions.TruncateTime(rec.DocumentDate) <= to
                                 select rec
                     );
                            q = (from rec in q
                                 orderby rec.DocumentDate, rec.Id
                                 select rec);
                        }
                        break;
                    case "1": //Accountingdate = "1"
                    default:
                        {
                            q = (from rec in q
                                 //where EntityFunctions.TruncateTime(rec.AccountingDate) >= @from
                                 where EntityFunctions.TruncateTime(rec.AccountingDate) <= to
                                 select rec
                     );
                            q = (from rec in q
                                 orderby rec.AccountingDate, rec.Id
                                 select rec);
                        }
                        break;
                }

            }

            return q;
        }
        public IQueryable<LedgerTransaction> GetQueryOrderAccDateAndIdBy(int tenant, IQueryable<string> listOfAccId, DateTime @from, DateTime to,
            string currencyId, 
            string searchByFilter,
            DateTime? maxCreateDate)
        {
            var q = (from rec in context.LedgerTransactions
                     where rec.Tenant == tenant
                     where listOfAccId.Contains(rec.AccountId) //less than 1000
                     select rec
                     );
            //if (context.ToString().StartsWith("Fake"))
            //{
            //    q = (from rec in q
            //         where rec.AccountingDate.Date >= @from
            //         where rec.AccountingDate.Date <= to
            //         select rec
            //        );
            //}
            //else
            //{
            //    q = (from rec in q
            //         where EntityFunctions.TruncateTime(rec.AccountingDate) >= @from
            //         where EntityFunctions.TruncateTime(rec.AccountingDate) <= to
            //         select rec
            //         );
            //}
            q = QFilterByDateTruncateTimeInclusive("1", @from, to, q);

            if (!string.IsNullOrWhiteSpace(currencyId))
            {
                q = q.Where(rec => rec.CurrencyId == currencyId);
            }
            if (!string.IsNullOrWhiteSpace(searchByFilter))
            {
                q = q.Where(rec => rec.SearchFields.Contains(searchByFilter));
            }
            
            if (maxCreateDate.HasValue)
            {
                DateTime maxCreateDateReal = maxCreateDate.Value;
                q = q.Where(rec => rec.CreateDate <= maxCreateDateReal);
            }
            q = (from rec in q
                 orderby rec.AccountingDate, rec.Id
                 select rec);


            return q;



        }

        

        public IQueryable<LedgerTransaction> GetQueryOrderAccDateAndIdByRec(int tenant, List<string> listOfAccId, DateTime @from, DateTime to,
    string currencyId,
    string searchByFilter,bool? isReconciled )
        {
            var q = (from rec in context.LedgerTransactions
                     where rec.Tenant == tenant
                     where listOfAccId.Contains(rec.AccountId) //less than 1000
                     select rec
                     );
            //if (context.ToString().StartsWith("Fake"))
            //{
            //    q = (from rec in q
            //         where rec.AccountingDate.Date >= @from
            //         where rec.AccountingDate.Date <= to
            //         select rec
            //        );
            //}
            //else
            //{
            //    q = (from rec in q
            //         where EntityFunctions.TruncateTime(rec.AccountingDate) >= @from
            //         where EntityFunctions.TruncateTime(rec.AccountingDate) <= to
            //         select rec
            //         );
            //}
            q = QFilterByDateTruncateTimeInclusive("1", @from, to, q);

            if (!string.IsNullOrWhiteSpace(currencyId))
            {
                q = q.Where(rec => rec.CurrencyId == currencyId);
            }
            if (isReconciled.HasValue && isReconciled.Value == true)
            {
                q = q.Where(rec => rec.IsReconciled == true);
            }

            if (!string.IsNullOrWhiteSpace(searchByFilter))
            {
                q = q.Where(rec => rec.SearchFields.Contains(searchByFilter));
            }

            q = (from rec in q
                 orderby rec.AccountingDate, rec.Id
                 select rec);


            return q;



        }


        public IQueryable<LedgerTransaction> GetQueryOrderByDateTypeAndIdByRec(int tenant, IQueryable<string> qOfAccId, DateTime @from, DateTime to,
       string currencyId,
       string searchByFilter, bool? isReconciled, string dateType)
        {
            var q = (from rec in context.LedgerTransactions
                     where rec.Tenant == tenant
                     where qOfAccId.Contains(rec.AccountId) //less than 1000
                     select rec
                     );
            //if (context.ToString().StartsWith("Fake"))
            //{
            //    q = (from rec in q
            //         where rec.AccountingDate.Date >= @from
            //         where rec.AccountingDate.Date <= to
            //         select rec
            //        );
            //}
            //else
            //{
            //    q = (from rec in q
            //         where EntityFunctions.TruncateTime(rec.AccountingDate) >= @from
            //         where EntityFunctions.TruncateTime(rec.AccountingDate) <= to
            //         select rec
            //         );
            //}
            q = QFilterByDateTruncateTimeInclusive(dateType, @from, to, q);

            if (!string.IsNullOrWhiteSpace(currencyId))
            {
                q = q.Where(rec => rec.CurrencyId == currencyId);
            }
            if (isReconciled.HasValue )
            {
                q = q.Where(rec => rec.IsReconciled == isReconciled.Value);
            }

            if (!string.IsNullOrWhiteSpace(searchByFilter))
            {
                q = q.Where(rec => rec.SearchFields.Contains(searchByFilter));
            }

            q = (from rec in q
                 orderby rec.AccountingDate, rec.Id
                 select rec);


            return q;



        }





        public IQueryable<LedgerTransaction> GetNotReconciled(string gLAccountId, int tenant)
        {
            var q = (from record in context.LedgerTransactions
                     where record.Tenant == tenant && record.AccountId == gLAccountId && record.IsReconciled == false
                     select record);
            return q;
        }
        public IQueryable<LedgerTransaction> GetYearTransferLedgerTransaction(string gLAccountId, int year, int tenant)
        {

            var qYearTransferJournals =
                (from j in context.Journals
                 where j.Tenant == tenant
                 where j.AccountingEntityCode == "11"//yeartransfer



                 //where
                 ////string.IsNullOrWhiteSpace(j.VoidedByJournalId)
                 //(j.VoidedByJournalId == null || j.VoidedByJournalId.Trim() == string.Empty)
                 //where
                 ////string.IsNullOrWhiteSpace(j.OriginalJournalId)
                 //(j.OriginalJournalId == null || j.OriginalJournalId.Trim() == string.Empty)
                 select j
                );
            DateTime beginOfYear = new DateTime(year, 1, 1);

            IQueryable<LedgerTransaction> qLedgerTransaction = null;
            if (context.ToString().StartsWith("Fake"))
            {
                qLedgerTransaction = (from record in context.LedgerTransactions

                                      where record.Tenant == tenant 
                                      ///&& record.AccountId == gLAccountId
                                      where record.AccountingDate.Date == beginOfYear
                                      select record
                 );
            }
            else
            {
                qLedgerTransaction = (from record in context.LedgerTransactions

                 where record.Tenant == tenant 
                 //&& record.AccountId == gLAccountId
                 where EntityFunctions.TruncateTime(record.AccountingDate) == beginOfYear
                 select record
                 );

            }
            if (!string.IsNullOrWhiteSpace(gLAccountId))
            {
                qLedgerTransaction = qLedgerTransaction.Where(record => record.AccountId == gLAccountId);
            }

            var qYeartransferLedgerTransaction =
                (from record in qLedgerTransaction

                     //context.LedgerTransactions
                     //where record.Tenant == tenant && record.AccountId == gLAccountId
                     //where EntityFunctions.TruncateTime(record.AccountingDate) == beginOfYear

                 join j in qYearTransferJournals
on record.JournalId equals j.Id

                 select record);
            return qYeartransferLedgerTransaction;
        }

        public List<GLAccountTotalByMonth> CalcGLAccountTotalByMonthByDateType(string DateTypeCode, DateTime fromDate, DateTime accoutingDateUntillNotInclude, int tenant, IQueryable<string> listOfAccId = null)
        {
            var fromDateOnlyDate = fromDate.Date;
            var DateUntillNotIncludeOnlyDate = accoutingDateUntillNotInclude.Date
                .AddDays(-1);//UntillNotIncludeOnly
            var ledgerTransactionsByAccountingDate =

                (from rec in context.LedgerTransactions
                 where rec.Tenant == tenant
                 select rec);
            //if (context.ToString().StartsWith("Faked"))
            //{
            //    ledgerTransactionsByAccountingDate =
            //        (from rec in ledgerTransactionsByAccountingDate
            //         where rec.AccountingDate.Date >= fromDateOnlyDate //fromDate.Date
            //         where rec.AccountingDate.Date < accoutingDateUntillNotIncludeOnlyDate //toDate.Date
            //         select rec
            //  );
            //}
            //else
            //{
            //    ledgerTransactionsByAccountingDate =
            //    (from rec in ledgerTransactionsByAccountingDate
            //     where EntityFunctions.TruncateTime(rec.AccountingDate) >= fromDateOnlyDate //fromDate.Date
            //     where EntityFunctions.TruncateTime(rec.AccountingDate) < accoutingDateUntillNotIncludeOnlyDate //toDate.Date
            //     select rec);

            //}
            ledgerTransactionsByAccountingDate = QFilterByDateTruncateTimeInclusive(DateTypeCode, fromDateOnlyDate, DateUntillNotIncludeOnlyDate, ledgerTransactionsByAccountingDate);


            var lTransByAccountingDateFilterByListOfAccId = ledgerTransactionsByAccountingDate;
            if (listOfAccId != null)
            {
                lTransByAccountingDateFilterByListOfAccId = ledgerTransactionsByAccountingDate.Where(rec => listOfAccId.Contains(rec.AccountId));
            }

            var myQCalcGLAccountTotalByMonth = (from rec in lTransByAccountingDateFilterByListOfAccId
                                                group rec by new
                                                {
                                                    rec.AccountId,
                                                    rec.CurrencyId
                                                } into groupByAccountCurrency
                                                select new //GLAccountTotalByMonth//The entity or complex type 'Logitude.Accounting.Data.GLAccountTotalByMonth' cannot be constructed in a 
                                                {
                                                    Tenant = tenant,
                                                    //   Month = toDate.Month,
                                                    //   Year = toDate.Year,
                                                    AccountId = groupByAccountCurrency.Key.AccountId,
                                                    CurrencyId = groupByAccountCurrency.Key.CurrencyId,
                                                    LocalAmountCredit = groupByAccountCurrency.Sum(x => x.LocalAmountCredit),
                                                    LocalAmountDebit = groupByAccountCurrency.Sum(x => x.LocalAmountDebit),
                                                    ForeignAmountCredit = groupByAccountCurrency.Sum(x => x.ForeignAmountCredit),
                                                    ForeignAmountDebit = groupByAccountCurrency.Sum(x => x.ForeignAmountDebit)

                                                });
            var myCalcGLAccountTotalByMonth =
                myQCalcGLAccountTotalByMonth
                  //The entity or complex type 'Logitude.Accounting.Data.GLAccountTotalByMonth' cannot be constructed in a 
                  .AsEnumerable().Select(y => new GLAccountTotalByMonth
                  {
                      Tenant = tenant,
                      //   Month = toDate.Month,
                      //   Year = toDate.Year,
                      AccountId = y.AccountId,
                      CurrencyId = y.CurrencyId,
                      LocalAmountCredit = y.LocalAmountCredit,
                      LocalAmountDebit = y.LocalAmountDebit,
                      ForeignAmountCredit = y.ForeignAmountCredit,
                      ForeignAmountDebit = y.ForeignAmountDebit
                  }).ToList();
            return myCalcGLAccountTotalByMonth;

        }

        public List<CurrencySumOpenAmount> CalcCurrencySumOpenAmountByMonthByDateType(string DateTypeCode,  DateTime accoutingDateUntillNotInclude, int tenant, IQueryable<string> listOfAccId = null)
        {
            ///var fromDateOnlyDate = fromDate.Date;
            var DateUntillNotIncludeOnlyDate = accoutingDateUntillNotInclude.Date
                .AddDays(-1);//UntillNotIncludeOnly
            var ledgerTransactionsByAccountingDate =

                (from rec in context.LedgerTransactions
                 where rec.Tenant == tenant
                 where !rec.IsReconciled /*== false*/
                 select rec);
            
            ledgerTransactionsByAccountingDate = QFilterByDateTruncateTimeInclusiveUntil(DateTypeCode,  DateUntillNotIncludeOnlyDate, ledgerTransactionsByAccountingDate);


            var lTransByAccountingDateFilterByListOfAccId = ledgerTransactionsByAccountingDate;
            if (listOfAccId != null)
            {
                lTransByAccountingDateFilterByListOfAccId = ledgerTransactionsByAccountingDate.Where(rec => listOfAccId.Contains(rec.AccountId));
            }

            var myQCalcGLAccountTotalByMonth = (from rec in lTransByAccountingDateFilterByListOfAccId
                                                group rec by new
                                                {
                                                    rec.AccountId,
                                                    rec.OpenAmountCurrencyId
                                                } into groupByAccountCurrency
                                                select new CurrencySumOpenAmount
                                                {
                                                    
                                                    AccountId = groupByAccountCurrency.Key.AccountId,
                                                    OpenAmountCurrencyId = groupByAccountCurrency.Key.OpenAmountCurrencyId,
                                                    OpenAmount = groupByAccountCurrency.Sum(x => x.OpenAmount)

                                                });

            var l= myQCalcGLAccountTotalByMonth.ToList();
            return l;
        }

        public List<GLAccountTotalByMonth> CalcGLAccountTotalByMonthByAccountingDate(DateTime fromDate, DateTime accoutingDateUntillNotInclude, int tenant, IQueryable<string> listOfAccId = null)
        {
            var fromDateOnlyDate = fromDate.Date;
            var accoutingDateUntillNotIncludeOnlyDate = accoutingDateUntillNotInclude.Date
                .AddDays(-1);
            var ledgerTransactionsByAccountingDate =
                
                (from rec in context.LedgerTransactions
                 where rec.Tenant == tenant
                 select rec);
            //if (context.ToString().StartsWith("Faked"))
            //{
            //    ledgerTransactionsByAccountingDate =
            //        (from rec in ledgerTransactionsByAccountingDate
            //         where rec.AccountingDate.Date >= fromDateOnlyDate //fromDate.Date
            //         where rec.AccountingDate.Date < accoutingDateUntillNotIncludeOnlyDate //toDate.Date
            //         select rec
            //  );
            //}
            //else
            //{
            //    ledgerTransactionsByAccountingDate =
            //    (from rec in ledgerTransactionsByAccountingDate
            //     where EntityFunctions.TruncateTime(rec.AccountingDate) >= fromDateOnlyDate //fromDate.Date
            //     where EntityFunctions.TruncateTime(rec.AccountingDate) < accoutingDateUntillNotIncludeOnlyDate //toDate.Date
            //     select rec);

            //}
            ledgerTransactionsByAccountingDate = QFilterByDateTruncateTimeInclusive("1", fromDateOnlyDate, accoutingDateUntillNotIncludeOnlyDate, ledgerTransactionsByAccountingDate);


            var lTransByAccountingDateFilterByListOfAccId = ledgerTransactionsByAccountingDate;
            if (listOfAccId != null)
            {
                lTransByAccountingDateFilterByListOfAccId = ledgerTransactionsByAccountingDate.Where(rec => listOfAccId.Contains(rec.AccountId));
            }

            var myQCalcGLAccountTotalByMonth = (from rec in lTransByAccountingDateFilterByListOfAccId
                                               group rec by new
                                               {
                                                   rec.AccountId,
                                                   rec.CurrencyId
                                               } into groupByAccountCurrency
                                               select new //GLAccountTotalByMonth//The entity or complex type 'Logitude.Accounting.Data.GLAccountTotalByMonth' cannot be constructed in a 
                                               {
                                                   Tenant = tenant,
                                                   //   Month = toDate.Month,
                                                   //   Year = toDate.Year,
                                                   AccountId = groupByAccountCurrency.Key.AccountId,
                                                   CurrencyId = groupByAccountCurrency.Key.CurrencyId,
                                                   LocalAmountCredit = groupByAccountCurrency.Sum(x => x.LocalAmountCredit),
                                                   LocalAmountDebit = groupByAccountCurrency.Sum(x => x.LocalAmountDebit),
                                                   ForeignAmountCredit = groupByAccountCurrency.Sum(x => x.ForeignAmountCredit),
                                                   ForeignAmountDebit = groupByAccountCurrency.Sum(x => x.ForeignAmountDebit)

                                               });
            var myCalcGLAccountTotalByMonth =
                myQCalcGLAccountTotalByMonth
                  //The entity or complex type 'Logitude.Accounting.Data.GLAccountTotalByMonth' cannot be constructed in a 
                  .AsEnumerable().Select(y => new GLAccountTotalByMonth
                  {
                      Tenant = tenant,
                      //   Month = toDate.Month,
                      //   Year = toDate.Year,
                      AccountId = y.AccountId,
                      CurrencyId = y.CurrencyId,
                      LocalAmountCredit = y.LocalAmountCredit ,
                      LocalAmountDebit = y.LocalAmountDebit ,
                      ForeignAmountCredit = y.ForeignAmountCredit,
                      ForeignAmountDebit = y.ForeignAmountDebit
                  }).ToList();
            return myCalcGLAccountTotalByMonth;

        }

        public IQueryable<GLAccountTotalByMonthsDTOAging> GetQuerableReconcileOpenBalanceAsTotalByMonth(int tenant,
            IQueryable<string> accountListId,
            IQueryable<GLAccountCurrency> queryableAccountThat_ARERelatedCurrenciesAccount,
            string accountingCurrencyId, bool groupByAccountingDateDefaultByDueDate)
        {
            //IQueryable<IGrouping<Tuple<string, string, int, int>, LedgerTransaction>> myGroup;
            IQueryable<IGrouping<GLAccountTotalByMonthsKey, LedgerTransactionsT>> myGroup;

            var lTransByAccountingDateFilterByListOfAccId =
                context.LedgerTransactions
                .Where(rec => rec.Tenant == tenant)
                .Where(rec => rec.IsReconciled == false);

            IQueryable<LedgerTransactionsT> qLedgerTransactionsT;

            qLedgerTransactionsT =
                   lTransByAccountingDateFilterByListOfAccId
                   .Where(rec => accountListId.Contains(rec.AccountId))
                   .Select(LT => new LedgerTransactionsT() { LedgerTransaction = LT, MainGLAccountId = LT.AccountId });

            if (queryableAccountThat_ARERelatedCurrenciesAccount != null)
            {
                qLedgerTransactionsT =
                    qLedgerTransactionsT.Union(
                    (
                    from LT in lTransByAccountingDateFilterByListOfAccId
                    join a in queryableAccountThat_ARERelatedCurrenciesAccount on LT.AccountId equals a.GLAccountId
                    select new LedgerTransactionsT() { LedgerTransaction = LT, MainGLAccountId = a.MainGLAccountId }
                    )
                    );
            }
            
            



            

            if (groupByAccountingDateDefaultByDueDate)
            {
                myGroup = qLedgerTransactionsT
                       .GroupBy(r => new GLAccountTotalByMonthsKey///Tuple<string,string,int,int>()
                       {
                           AccountId = r.LedgerTransaction.AccountId,
                           MainGLAccountId = r.MainGLAccountId,
                           CurrencyId = r.LedgerTransaction.OpenAmountCurrencyId,
                           Year = r.LedgerTransaction.AccountingDate.Year,
                           Month = r.LedgerTransaction.AccountingDate.Month
                       });
            }
            else
            {
                myGroup = qLedgerTransactionsT
                .GroupBy(r => new GLAccountTotalByMonthsKey///Tuple<string,string,int,int>()
                {
                    AccountId = r.LedgerTransaction.AccountId,
                    MainGLAccountId = r.MainGLAccountId,
                    CurrencyId = r.LedgerTransaction.OpenAmountCurrencyId,
                    Year = r.LedgerTransaction.DueDate.Year,
                    Month = r.LedgerTransaction.DueDate.Month
                });
            }


            //// // when take from reconcile from 2018/10 the reconcile set the open amount  into FOreignAmount ALWAYS !!!
            var qTotalByMonthAcc =
                 (from groupByAccountCurrency in myGroup
                  select new GLAccountTotalByMonthsDTOAging()
                  {
                      Tenant = tenant,
                      AccountId = groupByAccountCurrency.Key.MainGLAccountId,
                      GLAccountCurrencyId = groupByAccountCurrency.Key.AccountId,
                      CurrencyId = groupByAccountCurrency.Key.CurrencyId,

                      Year = groupByAccountCurrency.Key.Year,
                      Month = groupByAccountCurrency.Key.Month,

                      LocalAmountCredit = 0,//groupByAccountCurrency.Sum(x => x.OpenAmount),
                      LocalAmountDebit = 0, //0,//groupByAccountCurrency.Sum(x => x.LocalAmountDebit),

                      ///accountingCurrencyId != groupByAccountCurrency.Key.CurrencyId ? 0 : groupByAccountCurrency.Sum(x => x.LedgerTransaction.OpenAmount),
#if supress_OpenCreditAndDebit
                      ForeignAmountCredit = 0,
                      ForeignAmountDebit = groupByAccountCurrency.Sum(x => x.LedgerTransaction.OpenAmount),
#else
                      ForeignAmountCredit = -1 * ((decimal?)(groupByAccountCurrency.Where(r => r.LedgerTransaction.LocalAmountCredit != 0).Sum(x => x.LedgerTransaction.OpenAmount)) ?? 0),
                      ForeignAmountDebit = (decimal?)(groupByAccountCurrency.Where(r => r.LedgerTransaction.LocalAmountCredit == 0).Sum(x => x.LedgerTransaction.OpenAmount)) ?? 0,
#endif

                      TotalOpenTransactions = groupByAccountCurrency.Count(),

                      CHANGE_TYPE = ""
                  }); ;

#if NotOnlyInForeign_B4_201810


            var qTotalByMonthAcc =
              (from groupByAccountCurrency in myGroup
               select new GLAccountTotalByMonthsDTOAging()
               {
                   Tenant = tenant,
                   AccountId = groupByAccountCurrency.Key.MainGLAccountId,
                   GLAccountCurrencyId = groupByAccountCurrency.Key.AccountId,
                   CurrencyId = groupByAccountCurrency.Key.CurrencyId,

                   Year = groupByAccountCurrency.Key.Year,
                   Month = groupByAccountCurrency.Key.Month,

                   LocalAmountCredit = 0,//groupByAccountCurrency.Sum(x => x.OpenAmount),
                      LocalAmountDebit =  //0,//groupByAccountCurrency.Sum(x => x.LocalAmountDebit),

                                     accountingCurrencyId != groupByAccountCurrency.Key.CurrencyId ? 0 : groupByAccountCurrency.Sum(x => x.LedgerTransaction.OpenAmount),

                   ForeignAmountCredit = 0,//groupByAccountCurrency.Sum(x => x.ForeignAmountCredit),
                      ForeignAmountDebit =

                                     accountingCurrencyId == groupByAccountCurrency.Key.CurrencyId ? 0 : groupByAccountCurrency.Sum(x => x.LedgerTransaction.OpenAmount),

                   CHANGE_TYPE = ""
               });
#endif
#if FROM2016
            
            var qTotalByMonthAcc = (from rec in lTransByAccountingDateFilterByListOfAccId
                                    group rec by new
                                    {
                                        rec.AccountId,
                                        rec.OpenAmountCurrencyId,
                                        rec.AccountingDate.Year,
                                        rec.AccountingDate.Month,
                                    } into groupByAccountCurrency
                                    select new //GLAccountTotalByMonth//The entity or complex type 'Logitude.Accounting.Data.GLAccountTotalByMonth' cannot be constructed in a 
                                    GLAccountTotalByMonthsDTO()
                                    {
                                        Tenant = tenant,
                                        AccountId = groupByAccountCurrency.Key.AccountId,
                                        CurrencyId = groupByAccountCurrency.Key.OpenAmountCurrencyId,

                                        Year = groupByAccountCurrency.Key.Year,
                                        Month = groupByAccountCurrency.Key.Month,

                                        LocalAmountCredit =  0,//groupByAccountCurrency.Sum(x => x.OpenAmount),
                                        LocalAmountDebit =  //0,//groupByAccountCurrency.Sum(x => x.LocalAmountDebit),
                                        
                                        accountingCurrencyId != groupByAccountCurrency.Key.OpenAmountCurrencyId ? 0 : groupByAccountCurrency.Sum(x => x.OpenAmount),
                                        
                                        ForeignAmountCredit = 0,//groupByAccountCurrency.Sum(x => x.ForeignAmountCredit),
                                        ForeignAmountDebit = 
                                        
                                        accountingCurrencyId == groupByAccountCurrency.Key.OpenAmountCurrencyId ? 0 : groupByAccountCurrency.Sum(x => x.OpenAmount),
                                        
                                        CHANGE_TYPE = ""
                                    });
#endif
            return qTotalByMonthAcc;
        }

        public IQueryable<GLAccountTotalByMonthsDTO> GetQueryableGLAccountTotalByMonthByDateTypeCode(
            string DateTypeCode, 
            DateTime fromDate, DateTime toDate, int tenant, List<string> listOfAccId = null)
        {
            var fromDateOnlyDate = fromDate.Date;
            var toDateOnlyDate = toDate.Date;
            var ledgerTransactionsByAccountingDate = (from rec in context.LedgerTransactions
                                                      //where EntityFunctions.TruncateTime(rec.AccountingDate) >= fromDateOnlyDate //fromDate.Date
                                                      //where EntityFunctions.TruncateTime(rec.AccountingDate) <= toDateOnlyDate //toDate.Date
                                                      where rec.Tenant == tenant
                                                      select rec);


            ledgerTransactionsByAccountingDate = QFilterByDateTruncateTimeInclusive(DateTypeCode, fromDate, toDate, ledgerTransactionsByAccountingDate);
            var lTransByAccountingDateFilterByListOfAccId = ledgerTransactionsByAccountingDate;
            if (listOfAccId != null)
            {
                lTransByAccountingDateFilterByListOfAccId = ledgerTransactionsByAccountingDate.Where(rec => listOfAccId.Contains(rec.AccountId));
            }

            bool byDateType = true;
            if (byDateType)
            {
                var myGroupBy=lTransByAccountingDateFilterByListOfAccId.GroupBy(rec => new
                {
                    rec.AccountId,
                    rec.CurrencyId,
                    rec.AccountingDate.Year,
                    rec.AccountingDate.Month,
                });
                switch (DateTypeCode)
                {
                    case "2"://GLAccountTotalDateTypeValues.DueDate:
                        {
                            myGroupBy = lTransByAccountingDateFilterByListOfAccId.GroupBy(rec => new
                            {
                                rec.AccountId,
                                rec.CurrencyId,
                                rec.DueDate.Year,
                                rec.DueDate.Month,
                            });
                        }
                        break;
                    case "3":// GLAccountTotalDateTypeValues.DocumentDate:
                        {
                            //return null;
                            myGroupBy = lTransByAccountingDateFilterByListOfAccId.GroupBy(rec => new
                            {
                                rec.AccountId,
                                rec.CurrencyId,
                                rec.DocumentDate.Year,
                                rec.DocumentDate.Month,
                            });
                            break;

                        }
                }
                var myCalcGLAccountTotalByMonth = myGroupBy.Select(groupByAccountCurrency=>
                new //GLAccountTotalByMonth//The entity or complex type 'Logitude.Accounting.Data.GLAccountTotalByMonth' cannot be constructed in a 
                                                   GLAccountTotalByMonthsDTO()
                {
                    Tenant = tenant,
                    AccountId = groupByAccountCurrency.Key.AccountId,
                    CurrencyId = groupByAccountCurrency.Key.CurrencyId,

                    Year = groupByAccountCurrency.Key.Year,
                    Month = groupByAccountCurrency.Key.Month,

                    LocalAmountCredit = groupByAccountCurrency.Sum(x => x.LocalAmountCredit),
                    LocalAmountDebit = groupByAccountCurrency.Sum(x => x.LocalAmountDebit),
                    ForeignAmountCredit = groupByAccountCurrency.Sum(x => x.ForeignAmountCredit),
                    ForeignAmountDebit = groupByAccountCurrency.Sum(x => x.ForeignAmountDebit),
                    CHANGE_TYPE = ""
                });
                return myCalcGLAccountTotalByMonth;

            }
            else
            {
                var myCalcGLAccountTotalByMonth = (from rec in lTransByAccountingDateFilterByListOfAccId
                                                   group rec by new
                                                   {
                                                       rec.AccountId,
                                                       rec.CurrencyId,
                                                       rec.AccountingDate.Year,
                                                       rec.AccountingDate.Month,
                                                   } into groupByAccountCurrency
                                                   select new //GLAccountTotalByMonth//The entity or complex type 'Logitude.Accounting.Data.GLAccountTotalByMonth' cannot be constructed in a 
                                                   GLAccountTotalByMonthsDTO()
                                                   {
                                                       Tenant = tenant,
                                                       AccountId = groupByAccountCurrency.Key.AccountId,
                                                       CurrencyId = groupByAccountCurrency.Key.CurrencyId,

                                                       Year = groupByAccountCurrency.Key.Year,
                                                       Month = groupByAccountCurrency.Key.Month,

                                                       LocalAmountCredit = groupByAccountCurrency.Sum(x => x.LocalAmountCredit),
                                                       LocalAmountDebit = groupByAccountCurrency.Sum(x => x.LocalAmountDebit),
                                                       ForeignAmountCredit = groupByAccountCurrency.Sum(x => x.ForeignAmountCredit),
                                                       ForeignAmountDebit = groupByAccountCurrency.Sum(x => x.ForeignAmountDebit),
                                                       CHANGE_TYPE = ""
                                                   });


                //.ToList();
                return myCalcGLAccountTotalByMonth;
            }

        }


        public IQueryable<GLAccountTotalByMonthsDTO> GetQueryableGLAccountTotalByMonthByDateTypeCodeFromControlAccount(
            string DateTypeCode,
            DateTime fromDate, DateTime toDate, int tenant)
        {
            var fromDateOnlyDate = fromDate.Date;
            var toDateOnlyDate = toDate.Date;
            var ledgerTransactionsByAccountingDate = (from rec in context.LedgerTransactions
                                                          //where EntityFunctions.TruncateTime(rec.AccountingDate) >= fromDateOnlyDate //fromDate.Date
                                                          //where EntityFunctions.TruncateTime(rec.AccountingDate) <= toDateOnlyDate //toDate.Date
                                                      where rec.Tenant == tenant
                                                      select rec);


            ledgerTransactionsByAccountingDate = QFilterByDateTruncateTimeInclusive(DateTypeCode, fromDate, toDate, ledgerTransactionsByAccountingDate);
            ledgerTransactionsByAccountingDate = ledgerTransactionsByAccountingDate
                .Where(r =>
                r.ControlAccountId != null && r.ControlAccountId.Trim() != string.Empty
                    );
            var lTransByAccountingDateFilterByListOfAccId = ledgerTransactionsByAccountingDate;


            
            var myGroupBy = lTransByAccountingDateFilterByListOfAccId.GroupBy(rec => new
            {
                rec.ControlAccountId,
                rec.CurrencyId,
                rec.AccountingDate.Year,
                rec.AccountingDate.Month,
            });
            switch (DateTypeCode)
            {
                case "2"://GLAccountTotalDateTypeValues.DueDate:
                    {
                        myGroupBy = lTransByAccountingDateFilterByListOfAccId.GroupBy(rec => new
                        {
                            rec.ControlAccountId,
                            rec.CurrencyId,
                            rec.DueDate.Year,
                            rec.DueDate.Month,
                        });
                    }
                    break;
                case "3":// GLAccountTotalDateTypeValues.DocumentDate:
                    {
                        //return null;
                        myGroupBy = lTransByAccountingDateFilterByListOfAccId.GroupBy(rec => new
                        {
                            rec.ControlAccountId,
                            rec.CurrencyId,
                            rec.DocumentDate.Year,
                            rec.DocumentDate.Month,
                        });
                        break;

                    }
            }
            var myCalcGLAccountTotalByMonth = myGroupBy.Select(groupByAccountCurrency =>
            new //GLAccountTotalByMonth//The entity or complex type 'Logitude.Accounting.Data.GLAccountTotalByMonth' cannot be constructed in a 
                                               GLAccountTotalByMonthsDTO()
            {
                Tenant = tenant,
                AccountId = groupByAccountCurrency.Key.ControlAccountId,
                CurrencyId = groupByAccountCurrency.Key.CurrencyId,

                Year = groupByAccountCurrency.Key.Year,
                Month = groupByAccountCurrency.Key.Month,

                LocalAmountCredit = groupByAccountCurrency.Sum(x => x.LocalAmountCredit),
                LocalAmountDebit = groupByAccountCurrency.Sum(x => x.LocalAmountDebit),
                ForeignAmountCredit = groupByAccountCurrency.Sum(x => x.ForeignAmountCredit),
                ForeignAmountDebit = groupByAccountCurrency.Sum(x => x.ForeignAmountDebit),
                CHANGE_TYPE = ""
            });
            return myCalcGLAccountTotalByMonth;




        }
        public List<CurrencySum> GetLedgerTransactionTotalLocalAmountFromTo(string accountId, DateTime fromDate, DateTime toDate, int tenant)
        {

            var mysumlist = (from r in context.LedgerTransactions
                             where r.AccountId == accountId && r.AccountingDate >= fromDate && r.AccountingDate <= toDate && r.Tenant == tenant
                             group r by new
                             {
                                 r.CurrencyId
                             } into g
                             select new CurrencySum
                             {
                                 AccountId = accountId,
                                 CurrencyId = g.Key.CurrencyId,
                                 LocalAmountCredit = g.Sum(x => x.LocalAmountCredit),
                                 LocalAmountDebit = g.Sum(x => x.LocalAmountDebit),
                                 ForeignAmountCredit = g.Sum(x => x.ForeignAmountCredit),
                                 ForeignAmountDebit = g.Sum(x => x.ForeignAmountDebit)
                             }).ToList();
            return mysumlist;
        }

        public IQueryable<CurrencySum> GetQLedgerTransactionGroupBETWEENinclusive(DateTime fromDate, DateTime toDate, int tenant)
        {
            return (from r in context.LedgerTransactions

                    //where r.AccountId == accountId && 
                    where r.AccountingDate >= fromDate
                    where r.AccountingDate <= toDate

                    where r.Tenant == tenant
                    group r by new
                    {
                        r.AccountId,
                        r.CurrencyId,
                       
                    } into g
                    select new CurrencySum
                    {
                        AccountId = g.Key.AccountId,
                        CurrencyId = g.Key.CurrencyId,
                        
                        ForeignAmountCredit = g.Sum(x => x.ForeignAmountCredit),
                        ForeignAmountDebit = g.Sum(x => x.ForeignAmountDebit),

                        LocalAmountCredit = g.Sum(x => x.LocalAmountCredit),
                        LocalAmountDebit = g.Sum(x => x.LocalAmountDebit),
                    });
        }

        


        


        public decimal? GetLedgerTransactionSumFromTo(string gLAccointId, DateTime fromDate, DateTime toDate, int tenant, string currencyId)
        {

            IQueryable<LedgerTransaction> LedgerTransactionQuery;

            LedgerTransactionQuery = context.LedgerTransactions.Where(a => a.Tenant == tenant && a.AccountId == gLAccointId && a.AccountingDate >= fromDate && a.AccountingDate <= toDate);

            if (!string.IsNullOrWhiteSpace(currencyId))
            {
                LedgerTransactionQuery = LedgerTransactionQuery.Where(a => a.CurrencyId == currencyId);
            }

            var mylist = LedgerTransactionQuery.Select(a => new { a.LocalAmountCredit, a.LocalAmountDebit }).ToList();

            var totalSum = mylist.Sum(c => c.LocalAmountCredit - c.LocalAmountDebit);


            return totalSum;

        }





        public decimal? GetLedgerTransactionSumFromTo(string gLAccointId, DateTime fromDate, DateTime toDate, int tenant)
        {
            var mylist = (from a in context.LedgerTransactions
                          where a.AccountId == gLAccointId && a.AccountingDate >= fromDate && a.AccountingDate <= toDate && a.Tenant == tenant
                          select new { a.LocalAmountCredit, a.LocalAmountDebit }).ToList();
            var totalSum = mylist.Sum(c => c.LocalAmountCredit - c.LocalAmountDebit);

            return totalSum;
        }

        public List<LedgerTransaction> GetLedgerTransactionsByIdList(List<String> idList, int tenant)
        {
            List<LedgerTransaction> ledgerTransactionPOCOs =
                (from a in context.LedgerTransactions
                 where idList.Contains(a.Id) && a.Tenant == tenant
                 select a).ToList();
            return ledgerTransactionPOCOs;
        }

        public List<LedgerTransaction> GetLedgerTransactionsByAccountIdListAndJournalId(List<String> accountIdList,string journalId, int tenant)
        {
            List<LedgerTransaction> ledgerTransactionPOCOs =
                (from a in context.LedgerTransactions
                 where accountIdList.Contains(a.AccountId) && 
                 a.Tenant == tenant &&
                 a.JournalId == journalId
                 select a).ToList();
            return ledgerTransactionPOCOs;
        }
        public List<string> GetTransactionsCurrencies(string accountId, int tenant)
        {
            List<string> transactionsCurrencies = (from lt in context.LedgerTransactions
                                                   where lt.AccountId == accountId && lt.Tenant == tenant
                                                   group lt by lt.CurrencyId into cr
                                                   select cr.Key).ToList();

            return transactionsCurrencies;
        }
        public List<LedgerTransaction> GetLast10TransactionsForAccount(string accountId, int tenant)
        {
            List<LedgerTransaction> ledgerTransactionPOCOs =
                (from a in context.LedgerTransactions
                 where a.AccountId == accountId && a.Tenant == tenant
                 select a).OrderByDescending(a=>a.AccountingDate).Take(10).ToList();
            return ledgerTransactionPOCOs;
        }
        public IQueryable<LedgerTransaction> GetQByJournalIds(List<string> listOfJournalIds, int tenant)
        {
            return
             (from a in context.LedgerTransactions
              where listOfJournalIds.Contains(a.JournalId) && a.Tenant == tenant
              select a);
            
        }
        public IQueryable<LedgerTransaction> GetLedgerTransactionsByAccount(string gLAccountId, int tenant)
        {
            if (!String.IsNullOrEmpty(gLAccountId))
            {
                return (from record in context.LedgerTransactions //.Include("PartnerType").Include("PaymentTerm")... 
                        where record.Tenant == tenant && record.AccountId == gLAccountId
                        select record);
            }
            else
            {
                return null;
            }
        }


        public List<LedgerTransaction> GetDraftLedgerTransactionsByAccountId(string gLAccountId, int tenant)
        {
            if (!String.IsNullOrEmpty(gLAccountId))
            {
                return (from record in context.LedgerTransactions
                        where record.Tenant == tenant && record.AccountId == gLAccountId && record.Mark == true
                        select record).ToList();
            }
            else
            {
                return null;
            }
        }

        public int getRecoCount(string glAccountId)
        {
            return (from a in context.LedgerTransactions
                    where 
                    a.AccountId == glAccountId 
                    && a.InReconcileProgress == false
                    && a.IsReconciled == false
                    select a).Count();
        }

       public List<TaxReportData> GetLedgerTransactionsForTaxReport(DateTime? taxReportMonth, int tenant)
        {

            int days = DateTime.DaysInMonth(taxReportMonth.Value.Year, taxReportMonth.Value.Month);
            DateTime endOfTaxReportDate = new DateTime(taxReportMonth.Value.Year, taxReportMonth.Value.Month, days,23,59,59);

            FullAccountingSettingRepository fullAccountingSettingRepository = new FullAccountingSettingRepository(tenant);
            FullAccountingSetting setting = fullAccountingSettingRepository.GetSingleFullAccountingSetting(tenant);
           
            return (from a in context.LedgerTransactions
                    join j in context.Journals on a.JournalId equals j.Id
                    join m in context.JournalAdditionalDatas on new { a.JournalId, a.JournalLineNumber } equals new { m.JournalId, m.JournalLineNumber }

                    where (m.TaxReportId == null || m.TaxReportTransmitStatusCode == "2" || m.TaxReportTransmitStatusCode==null) 
                            && a.DocumentDate <= endOfTaxReportDate
                            && a.AccountId == setting.VATInputsGLAccountId 
                            && a.Tenant == tenant 
                            && a.LocalAmountDebit != 0 
                            && a.OppositeAccountId != setting.VATOutputGLAccountId

                    select new TaxReportData()
                    {
                        Id = Guid.NewGuid().ToString(),
                        AccountingEntity = j.AccountingEntityCode,
                        Reference = a.Reference1,
                        ReferenceDate = a.DocumentDate,
                        JournalId = a.JournalId,
                        LocalAmountDebit = a.LocalAmountDebit,
                        LocalAmountCredit = a.LocalAmountCredit,
                        OppositGLAccount = a.OppositeAccountId,
                        AccountingEntityId= j.AccountingEntityId,
                        JournalLineNumber = a.JournalLineNumber,
                        AccountId = a.AccountId,
                    }
                    
                    ).ToList();


        }
        public IQueryable<LedgerTransaction> GetLedgerTransactionsByTaxReportJournalIds(DateTime? taxReportMonth, int tenant, List<string> journalIds, string accountId)
        {

            int days = DateTime.DaysInMonth(taxReportMonth.Value.Year, taxReportMonth.Value.Month);
            DateTime endOfTaxReportDate = new DateTime(taxReportMonth.Value.Year, taxReportMonth.Value.Month, days, 23, 59, 59);

            FullAccountingSettingRepository fullAccountingSettingRepository = new FullAccountingSettingRepository(tenant);
            FullAccountingSetting setting = fullAccountingSettingRepository.GetSingleFullAccountingSetting(tenant);

            return (from a in context.LedgerTransactions
                    join j in context.Journals on a.JournalId equals j.Id
                    join m in context.JournalAdditionalDatas on new { a.JournalId, a.JournalLineNumber } equals new { m.JournalId, m.JournalLineNumber }

                    where (m.TaxReportId != null )
                            && a.DocumentDate <= endOfTaxReportDate
                           
                            && a.Tenant == tenant
                            && a.LocalAmountDebit != 0
                           && a.AccountId== accountId
                            && journalIds.Contains(a.JournalId)
                   select a
                    );


        }

        public List<LedgerTransaction> GetLedgerTransactionsByJournalIds(List<string> journalIds, int tenant)
        {

          
            return (from a in context.LedgerTransactions.Include("Account")
                    where journalIds.Contains(a.JournalId) && a.Tenant==tenant

                    select a
                    ).ToList();


        }
        public IQueryable<LedgerTransaction> GetTaxReportsLedgerTransactionsByJournalIds(List<string> journalIds, int tenant, DateTime date, string accountId)
        {
            int days = DateTime.DaysInMonth(date.Year, date.Month);
            DateTime reportDate = new DateTime(date.Year, date.Month, days);
            

            return (from a in context.LedgerTransactions
                   join j in context.Journals on a.JournalId equals j.Id
                   
                    join m in context.JournalAdditionalDatas on j.Id equals m.JournalId
                    where j.AccountingEntityCode == "2" && (m.TaxReportId != null ) && a.Tenant == tenant
                    && a.DocumentDate <= reportDate
                    where journalIds.Contains(a.JournalId) && a.Tenant == tenant && a.AccountId == accountId

                    select a
                    );
        }
        public IQueryable<LedgerTransaction> GetLedgerTransactionsNotIncludedInTaxReports(int tenant, string accountId)
        {

            IQueryable<LedgerTransaction> outputLines = (from a in context.LedgerTransactions
                                                         join j in context.Journals on a.JournalId equals j.Id
                                                         join m in context.JournalAdditionalDatas on j.Id equals m.JournalId
                                                         join x in context.TaxReports on m.TaxReportId equals x.Id
                                                         where j.AccountingEntityCode == "2" &&  a.Tenant == tenant

                                                         && a.AccountId == accountId && x.StatusCode != "T"

                                                         select a
                    ).Distinct();
            List<LedgerTransaction> list = outputLines.ToList();

            FullAccountingSettingRepository fullAccountingSettingRepository = new FullAccountingSettingRepository(tenant);
            FullAccountingSetting setting = fullAccountingSettingRepository.GetSingleFullAccountingSetting(tenant);

            IQueryable<LedgerTransaction> inputLines = (from a in context.LedgerTransactions
                                                        join j in context.Journals on a.JournalId equals j.Id
                                                        join m in context.JournalAdditionalDatas on new { a.JournalId, a.JournalLineNumber } equals new { m.JournalId, m.JournalLineNumber }
                                                        join x in context.TaxReports on m.TaxReportId equals x.Id
                                                        where  (a.OppositeAccountId != setting.VATOutputGLAccountId ||  a.OppositeAccountId == null)
                                                       && a.Tenant == tenant
                                                      && a.LocalAmountDebit != 0
                                                      && a.AccountId == accountId
                                                      && x.StatusCode != "T"
                                                        select a).Distinct();
            List<LedgerTransaction> list2 = inputLines.ToList();

            return outputLines.Concat(inputLines);
        }
        public IQueryable<LedgerTransaction> GetClosedPeriodTransactions(string accountId, DateTime closedDate, DateTime openDate, int tenant)
        {
            // there is two closed periods:
            // 1- from start of year till closed date
            // 2- from last day in open month till end of the year

            DateTime yearStartDate = new DateTime(closedDate.Year, 1, 1, 0, 0, 0);
            DateTime leftClosedPeriodToDate = closedDate;

            DateTime rightClosedPeriodFromDate = new DateTime(openDate.Year, openDate.Month, DateTime.DaysInMonth(openDate.Year, openDate.Month), 23, 59, 59);
            DateTime yearEndDate = new DateTime(openDate.Year, 12, DateTime.DaysInMonth(openDate.Year, 12), 23, 59, 59);

            IQueryable<LedgerTransaction> records
                = (from a in context.LedgerTransactions
                   where a.AccountId == accountId && a.Tenant == tenant
                       && (
                               (a.AccountingDate >= yearStartDate && a.AccountingDate <= leftClosedPeriodToDate)
                               ||
                               (a.AccountingDate >= rightClosedPeriodFromDate && a.AccountingDate <= yearEndDate)
                           )
                   select a);


            return records;
        }

        public IQueryable<LedgerTransaction> GetTransactionsForMonth(int year, int month, int tenant)
        {
            DateTime monthStart = new DateTime(year, month, 1, 0, 0, 0);
            DateTime monthEnd = new DateTime(year, month, DateTime.DaysInMonth(year, month), 23, 59, 59);


            IQueryable<LedgerTransaction> pocos =
                (from a in context.LedgerTransactions
                 where a.AccountingDate >= monthStart
                    && a.AccountingDate <= monthEnd 
                    && a.Tenant == tenant
                 select a).OrderByDescending(a => a.AccountingDate);
            return pocos;
        }
    }
    public class GLAccountTotalByMonthsKey
    {
        public string AccountId { get; set; }
        public string MainGLAccountId { get; internal set; }
        public string CurrencyId { get; set; }

        public int Year { get; set; }

        public int Month { get; set; }
    }
    class LedgerTransactionsT
    {
        public LedgerTransaction LedgerTransaction { get; internal set; }
        public string MainGLAccountId { get; internal set; }
    }
}