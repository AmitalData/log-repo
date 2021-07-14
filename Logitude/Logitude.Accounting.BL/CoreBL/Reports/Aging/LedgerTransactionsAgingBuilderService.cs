using Logitude.Accounting.Data;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL.Reports.Aging
{
    public class LedgerTransactionsAgingBuilderService
    {
        private IAccountingContext _AccountingContext;

        public LedgerTransactionsAgingBuilderService(IAccountingContext accountingContext)
        {
            this._AccountingContext = accountingContext;
        }

        internal List<GLAccountAgingDataPM> GetAgingPMs(List<LedgerTransactionPM> myLedgerTransactionsWithCounters)
        {
            int tenant = myLedgerTransactionsWithCounters.First().Tenant;

            var supplier_CustomerOnly = new List<string>() {
"2",//AgingReportParam.Aging4AccountTypeCodeEnum.Customer2.ToString(),
"3"//AgingReportParam.Aging4AccountTypeCodeEnum.Vendor3.ToString(),
            };
            var gLAccountRepository = new GLAccountRepository(this._AccountingContext as IAccountingContext);
            var onlyCustomer2Vendor3Ledgers =
                (
                from l in myLedgerTransactionsWithCounters
                join a in

                gLAccountRepository
                .GetAll(tenant)
                .Where(a => supplier_CustomerOnly.Contains(a.AccountTypeCode))
                on l.AccountId equals a.Id

                select l
                ).ToList();

            var accountCustomer2Vendor3Ids = onlyCustomer2Vendor3Ledgers.Select(a => a.AccountId).ToList();

            if (accountCustomer2Vendor3Ids.Count == 0)
            {
                return new List<GLAccountAgingDataPM>();
            }


            List<DateTime> listPeriods;
            DateTime lessThan, graterThen_OpenTransactionsFutureDueDate;
            var listPeriodService = new ListPeriodService(new AgingReportRebulidParam()
            {
                Tenant = tenant,

                NumberOfmonthsbackwards = 6,
                AgingForDate = DateTime.Now.Date,
                ///VendorCustomerId = MyGLAccId,
                ///Aging4AccountTypeCode = aging4AccountTypeCode == "Vendor3" ? AgingReportParam.Aging4AccountTypeCodeEnum.Vendor3 : AgingReportParam.Aging4AccountTypeCodeEnum.Customer2,

            });
            listPeriodService.GetListPeriods(out listPeriods, out lessThan, out graterThen_OpenTransactionsFutureDueDate);





            var qGLAccountTotalByMonthsDTOAging =
                (
                from ledger in onlyCustomer2Vendor3Ledgers
                group ledger by new
                {
                    ledger.DueDate.Year,
                    ledger.DueDate.Month,
                    ledger.AccountId
                }
                into gbDateMonth
                select new ReconciliationUpdateAgingM()
                {
                    AccountId = gbDateMonth.Key.AccountId,
                    Year = gbDateMonth.Key.Year,
                    Month = gbDateMonth.Key.Month,
                    Total = gbDateMonth.Sum(r => r.OpenAmount),
                    TotalOpenTransactions = gbDateMonth.Count(r => !r.IsReconciled)
                });

            var onlyCustomer2Vendor3DTOAging = qGLAccountTotalByMonthsDTOAging.ToList();

            var myPeriodList = new List<PeriodM>();
            foreach (string curAccId in accountCustomer2Vendor3Ids)
            {
                var currAccountAgingDtoS = onlyCustomer2Vendor3DTOAging.Where(r => r.AccountId == curAccId).ToList();

                var less = (from rec in currAccountAgingDtoS
                            where
                              (rec.Year == lessThan.Year && rec.Month < lessThan.Month)
                              ||
                              rec.Year < lessThan.Year
                            group rec by new { rec.AccountId } into groupByAccCurrr

                            select new PeriodM()
                            {

                                OrderDate = lessThan,
                                OrderDateB4 = true,
                                AccountId = groupByAccCurrr.Key.AccountId,

                                Total = groupByAccCurrr.Sum(rec => rec.Total),
                                TotalOpenTransactions = groupByAccCurrr.Sum(r => r.TotalOpenTransactions)
                            }).FirstOrDefault()
                 ?? new PeriodM()
                 {

                     OrderDate = lessThan,
                     OrderDateB4 = true,
                     AccountId = curAccId,
                 };
                myPeriodList.Add(less);


                var myPeriodMs = (
                           from period in listPeriods

                           join totalByMonth in currAccountAgingDtoS
                           on new { period.Year, period.Month }
                           equals
                           new { totalByMonth.Year, totalByMonth.Month }
                           into JointotalByMonth
                           from totalByMonth in JointotalByMonth.DefaultIfEmpty()


                           select new PeriodM()
                           {
                               OrderDate = period,
                               AccountId = curAccId /*totalByMonth.AccountId*/,

                               Total = totalByMonth == null ? 0 : totalByMonth.Total,
                               TotalOpenTransactions = totalByMonth == null ? 0 : totalByMonth.TotalOpenTransactions,
                           })
                            .ToList();
                myPeriodList.AddRange(myPeriodMs);


                var futurePeriod =
                    (from rec in currAccountAgingDtoS
                     where
                     (rec.Year == graterThen_OpenTransactionsFutureDueDate.Year && rec.Month >= graterThen_OpenTransactionsFutureDueDate.Month)
                           ||
                     rec.Year > graterThen_OpenTransactionsFutureDueDate.Year
                     group rec by new { rec.AccountId } into groupByAccCurrr

                     select new PeriodM()
                     {

                         OrderDate = graterThen_OpenTransactionsFutureDueDate,
                         OrderAfterOpenRecordDueDate = true,
                         AccountId = groupByAccCurrr.Key.AccountId,

                         Total = groupByAccCurrr.Sum(rec => rec.Total),
                         TotalOpenTransactions = groupByAccCurrr.Sum(r => r.TotalOpenTransactions)

                     })
             .FirstOrDefault() ?? new PeriodM()
             {
                 OrderDate = graterThen_OpenTransactionsFutureDueDate,
                 OrderAfterOpenRecordDueDate = true,
                 AccountId = curAccId

             };
                myPeriodList.Add(futurePeriod);

            }

            ///List<PeriodM> myPeriodList = Bad(listPeriods, lessThan, graterThen_OpenTransactionsFutureDueDate, accountCustomer2Vendor3Ids, onlyCustomer2Vendor3DTOAging);
            myPeriodList = myPeriodList.OrderBy(rec => rec.AccountId).ThenBy(rec => rec.OrderDate).ThenBy(rec => rec.CurrencyId)
                .ToList();
            ///myPeriodList = myPeriodList.OrderBy(rec => rec.AccountId).ThenBy(rec => rec.OrderDate).ThenBy(rec => rec.CurrencyId).ToList();

            List<GLAccountAgingDataPM> gLAccountAgingDataPMs = AgingReportRebulidService.MapPeriod2AgingData(tenant, myPeriodList);
            return gLAccountAgingDataPMs;
        }

        private static List<PeriodM> Bad(List<DateTime> listPeriods, DateTime lessThan, DateTime graterThen_OpenTransactionsFutureDueDate, List<string> accountCustomer2Vendor3Ids, List<ReconciliationUpdateAgingM> onlyCustomer2Vendor3DTOAging)
        {
            var lessThanPeriods =
                   (
                   from accId in accountCustomer2Vendor3Ids

                       //from rec in onlyCustomer2Vendor3DTOAging
                   join rec in onlyCustomer2Vendor3DTOAging
                   on accId equals rec.AccountId into joinonlyCustomer2Vendor3DTOAging
                   from rec in joinonlyCustomer2Vendor3DTOAging.DefaultIfEmpty()
                   where
                    (rec.Year == lessThan.Year && rec.Month < lessThan.Month)
                    ||
                    rec.Year < lessThan.Year
                   group rec by new { rec.AccountId } into groupByAccCurrr

                   select new PeriodM()
                   {

                       OrderDate = lessThan,
                       OrderDateB4 = true,
                       AccountId = groupByAccCurrr.Key.AccountId,

                       Total = groupByAccCurrr.Sum(rec => rec.Total),
                       TotalOpenTransactions = groupByAccCurrr.Sum(r => r.TotalOpenTransactions)
                   })
                    .ToList();

            var cartesianAccIdPeriods = (from period in listPeriods
                                         from accId in accountCustomer2Vendor3Ids
                                         select new { AccountId = accId, period }
                              );

            var mainPeriods =
                           (
                           //from period in listPeriods
                           from AccIdPeriod in cartesianAccIdPeriods
                           join totalByMonth in onlyCustomer2Vendor3DTOAging
                           on new { AccIdPeriod.AccountId, AccIdPeriod.period.Year, AccIdPeriod.period.Month }
                           equals
                           new { totalByMonth.AccountId, totalByMonth.Year, totalByMonth.Month }
                           into JointotalByMonth
                           from totalByMonth in JointotalByMonth.DefaultIfEmpty()


                           select new PeriodM()
                           {
                               OrderDate = AccIdPeriod.period,
                               AccountId = totalByMonth.AccountId,

                               Total = totalByMonth == null ? 0 : totalByMonth.Total,
                               TotalOpenTransactions = totalByMonth == null ? 0 : totalByMonth.TotalOpenTransactions,
                           })
                            .ToList();

            var futurePeriods = (
                //from rec in qGLAccountTotalByMonthsDTOAging
                from accId in accountCustomer2Vendor3Ids


                join rec in onlyCustomer2Vendor3DTOAging
                on accId equals rec.AccountId into joinonlyCustomer2Vendor3DTOAging
                from rec in joinonlyCustomer2Vendor3DTOAging.DefaultIfEmpty()

                where
                           (rec.Year == graterThen_OpenTransactionsFutureDueDate.Year && rec.Month > graterThen_OpenTransactionsFutureDueDate.Month)
                           ||
                           rec.Year > graterThen_OpenTransactionsFutureDueDate.Year
                group rec by new { rec.AccountId } into groupByAccCurrr

                select new PeriodM()
                {

                    OrderDate = graterThen_OpenTransactionsFutureDueDate,
                    OrderAfterOpenRecordDueDate = true,
                    AccountId = groupByAccCurrr.Key.AccountId,

                    Total = groupByAccCurrr.Sum(rec => rec.Total),
                    TotalOpenTransactions = groupByAccCurrr.Sum(r => r.TotalOpenTransactions)

                })
             .ToList();
            var myPeriodList =
                    lessThanPeriods
                    .Union(mainPeriods)
                    .Union(futurePeriods)
                    .ToList();
            return myPeriodList;
        }

        private static PeriodM GetLessPeriodM(IEnumerable<ReconciliationUpdateAgingM> qGLAccountTotalByMonthsDTOAging, DateTime lessThan)
        {
            var qLessThanExclusiveBasicInLocal =
                   (from rec in qGLAccountTotalByMonthsDTOAging
                    where
                    (rec.Year == lessThan.Year && rec.Month < lessThan.Month)
                    ||
                    rec.Year < lessThan.Year
                    group rec by new { rec.AccountId } into groupByAccCurrr

                    select new PeriodM()
                    {

                        OrderDate = lessThan,
                        OrderDateB4 = true,
                        AccountId = groupByAccCurrr.Key.AccountId,

                        Total = groupByAccCurrr.Sum(rec => rec.Total),
                    })
                    .ToList();
            if (qLessThanExclusiveBasicInLocal.Count > 1)
            {
                throw new Exception("if (qLessThanExclusiveBasicInLocal.Count > 1)");
            }
            var myLess = qLessThanExclusiveBasicInLocal.FirstOrDefault();
            return myLess;
        }
    }
}
