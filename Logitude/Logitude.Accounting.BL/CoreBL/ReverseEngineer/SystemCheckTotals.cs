using Logitude.Accounting.BL.CloseTables;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.Repositories;
using Logitude.Server.Tools.Utils;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL
{
    public class SystemCheckTotals
    {

        public void TotalSumMustBeZero(int tenant)
        {

            using (var scope = TransactionFactory.GetTransaction())
            {

                var _AccountingContext = AccountingContext.GetContext(tenant);
                var repoGLAccount = new GLAccountRepository(_AccountingContext);
                var repoGLAccountTotalByMonth = new GLAccountTotalByMonthRepository(_AccountingContext);

                var maxYear = repoGLAccountTotalByMonth.GetAll(tenant).Max(r => r.Year);
                var minYear = repoGLAccountTotalByMonth.GetAll(tenant).Min(r => r.Year);
                var fullAccountingSetting = //Hope From Cache
                FullAccountingSettingQueryService
                .Get(tenant);
                for (int i = 0; i < 2; i++)
                {
                    bool controlAccountLevel = i == 1;
                    var qAllCardsAndDetialsAccType = //Get The Account List
                         repoGLAccount.
                         GetQAllCardsAndDetailsAccType(tenant,
                         controlAccountLevel ? fullAccountingSetting.CustomerControlAccountId : "",
                         controlAccountLevel ? fullAccountingSetting.VendorControlAccountId : "",
                         ""
                         ,
                         controlAccountLevel ? fullAccountingSetting.FileControlAccountId : "");


                    for (int year = minYear; year <= maxYear; year++)
                        for (int month = 1; month < 13; month++)
                        {

                            //for (
                            //    int dateTypeCode = 1/*GLAccountTotalDateTypeValues.Accoutingdate*/;
                            //    dateTypeCode <= 3 /*GLAccountTotalDateTypeValues.DocumentDate*/ ;
                            //    dateTypeCode++)
                            {
                                var q = (from tot in repoGLAccountTotalByMonth
                                     .GetQuaryableMonthTotals(year, month, tenant, GLAccountTotalDateTypeValues.Accountingdate)
                                         join glAcc in qAllCardsAndDetialsAccType
                                         on tot.AccountId equals glAcc.Id
                                         group tot by 1 into g
                                         select g.Sum(tot => tot.LocalAmountDebit - tot.LocalAmountCredit)
                                );
                                var res = q.FirstOrDefault();
                                if (res != 0)
                                {
                                    var accIdList = qAllCardsAndDetialsAccType.Select(r => r.Id);
                                    throw new Exception($"not equal to zero for year:{year} month:{month} dateTypeCode:{GLAccountTotalDateTypeValues.Accountingdate} controlAccountLevel:{controlAccountLevel}");
                                }
                            }
                        }
                }
            }
        }

        public void LedgerTransactionSumMustBeZero(int tenant, int YYYY)
        {
            using (var scope = TransactionFactory.GetTransaction())
            {

                var _AccountingContext = AccountingContext.GetContext(tenant);
                var repoGLAccount = new GLAccountRepository(_AccountingContext);
                var repoGLAccountTotalByMonth = new GLAccountTotalByMonthRepository(_AccountingContext);
                var ledgerTransactionRepository = new LedgerTransactionRepository(_AccountingContext);
                DateTime from = new DateTime(YYYY, 1, 1);
                DateTime toDate = new DateTime(YYYY, 12, 31);
                var res = ledgerTransactionRepository.GetAll(tenant)
                    .Where(rec => EntityFunctions.TruncateTime(rec.AccountingDate) >= from.Date)
                    .Where(rec => EntityFunctions.TruncateTime(rec.AccountingDate) <= toDate.Date)
                    //.Sum(r => r.LocalAmountDebit - r.LocalAmountCredit);
                    .Select(r => r.LocalAmountDebit - r.LocalAmountCredit)
                    .DefaultIfEmpty(0)
                    .Sum();
                if (res != 0)
                {

                    throw new Exception($"LedgerTransactionSumMustBeZero:not equal to zero for year:{YYYY} ");
                }

            }

        }
        /// <summary>
        /// ניתן היה לסכום כל DATETYPE ולוודא שהינו שווה לאפס
        /// אבל
        /// יותר מדוייק לבצע לפי כרטיס כך ניתן למצוא את הכרטיטסים הלא תקינים ומכאן את פ היומן ?!!?!
        /// </summary>
        /// <param name="tenant"></param>
        /// <returns></returns>
        public string TotalSumPerAccountGroupByDateTypeDiff(int tenant)
        {
            using (var scope = TransactionFactory.GetTransaction())
            {

                var _AccountingContext = AccountingContext.GetContext(tenant);
                var repoGLAccount = new GLAccountRepository(_AccountingContext);
                var repoGLAccountTotalByMonth = new GLAccountTotalByMonthRepository(_AccountingContext);

                var gAccountingdate =
                    repoGLAccountTotalByMonth.GetAll(tenant)
                    .Where(r => r.DateTypeCode == GLAccountTotalDateTypeValues.Accountingdate)
                    .GroupBy(gBy => new { gBy.AccountId })
                    .Select(g => new TotSumPerAccount()
                    {
                        AccountId=g.Key.AccountId,
                        //g.Key.GLAccountTotalDateType,
                        TotalLocalAmountAccountingdate = g.Sum(r => r.LocalAmountDebit - r.LocalAmountCredit),
                        TotalForeignAmountAccountingdate = g.Sum(r => r.ForeignAmountDebit - r.ForeignAmountCredit),
                        TotalLocalAmountDueDate = 0,
                        TotalForeignAmountDueDate = 0,
                        TotalLocalAmountDocumentDate = 0,
                        TotalForeignAmountDocumentDate = 0
                    }
                    );

                var gDueDate =
                    repoGLAccountTotalByMonth.GetAll(tenant)
                    .Where(r => r.DateTypeCode == GLAccountTotalDateTypeValues.DueDate)
                    .GroupBy(gBy => new { gBy.AccountId })
                    .Select(g => new TotSumPerAccount()
                    {
                        AccountId=g.Key.AccountId,
                        //g.Key.GLAccountTotalDateType,
                        TotalLocalAmountAccountingdate = 0,
                        TotalForeignAmountAccountingdate = 0,
                        TotalLocalAmountDueDate = g.Sum(r => r.LocalAmountDebit - r.LocalAmountCredit),
                        TotalForeignAmountDueDate = g.Sum(r => r.ForeignAmountDebit - r.ForeignAmountCredit),
                        TotalLocalAmountDocumentDate = 0,
                        TotalForeignAmountDocumentDate = 0
                    }
                    );

                var gDocumentDate =
    repoGLAccountTotalByMonth.GetAll(tenant)
    .Where(r => r.DateTypeCode == GLAccountTotalDateTypeValues.DocumentDate)
    .GroupBy(gBy => new { gBy.AccountId })
    .Select(g => new TotSumPerAccount()
    {
        AccountId=g.Key.AccountId,
        //g.Key.GLAccountTotalDateType,
        TotalLocalAmountAccountingdate = 0,
        TotalForeignAmountAccountingdate = 0,
        TotalLocalAmountDueDate = 0,
        TotalForeignAmountDueDate = 0,
        TotalLocalAmountDocumentDate = g.Sum(r => r.LocalAmountDebit - r.LocalAmountCredit),
        TotalForeignAmountDocumentDate = g.Sum(r => r.ForeignAmountDebit - r.ForeignAmountCredit),
    }
    );


                var totalByMonthMustBeEqual = gAccountingdate.Concat(gDueDate).Concat(gDocumentDate)
                    .GroupBy(r => r.AccountId)
                    .Select(g => new TotSumPerAccount()
                    {
                        AccountId = g.Key,
                        TotalLocalAmountAccountingdate = g.Sum(r => r.TotalLocalAmountAccountingdate),
                        TotalForeignAmountAccountingdate = g.Sum(r => r.TotalForeignAmountAccountingdate),
                        TotalLocalAmountDueDate = g.Sum(r => r.TotalLocalAmountDueDate),
                        TotalForeignAmountDueDate = g.Sum(r => r.TotalForeignAmountDueDate),
                        TotalLocalAmountDocumentDate = g.Sum(r => r.TotalLocalAmountDocumentDate),
                        TotalForeignAmountDocumentDate = g.Sum(r => r.TotalForeignAmountDocumentDate),

                    })
                    .Where(r =>
                    r.TotalLocalAmountAccountingdate != r.TotalLocalAmountDueDate ||
                    r.TotalLocalAmountAccountingdate != r.TotalLocalAmountDocumentDate ||

                    r.TotalForeignAmountAccountingdate != r.TotalForeignAmountDueDate ||
                    r.TotalForeignAmountAccountingdate != r.TotalForeignAmountDocumentDate

                    ).ToList();


                var textJson=ProxyUtil.JsonConvertSerialize(totalByMonthMustBeEqual);
                return textJson;
            }
        }
    }
    class TotSumPerAccount
    {
        public string AccountId { get; set; }
        public decimal TotalLocalAmountAccountingdate { get; internal set; }
        public decimal TotalForeignAmountAccountingdate { get; internal set; }
        public decimal TotalLocalAmountDueDate { get; internal set; }
        public decimal TotalForeignAmountDueDate { get; internal set; }
        public decimal TotalLocalAmountDocumentDate { get; internal set; }
        public decimal TotalForeignAmountDocumentDate { get; internal set; }
    }
}

