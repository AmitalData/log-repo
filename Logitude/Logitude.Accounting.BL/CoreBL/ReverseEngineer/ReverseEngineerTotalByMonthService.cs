using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Accounting.BL.CloseTables;

namespace Logitude.Accounting.BL.CoreBL
{
    public class ReverseEngineerTotalByMonthService
    {
        private DateTime _SeedDate;
        private int _Tenant;
        private IAccountingContext _AccountingContext;
        private string _GLAccountId;

        public ReverseEngineerTotalByMonthService(DateTime seedDate, int currTenant,string glAccountId)
        {
            // TODO: Complete member initialization
            this._SeedDate = seedDate;
            this._Tenant = currTenant;
            this._GLAccountId = glAccountId;
        }

        public static void CheckLastMonth(int subtractMonths)
        {
            if (subtractMonths > 0)
            {
                subtractMonths = -1 * subtractMonths;
            }
            var tenantList = new List<int>() { 1 };
            //DateTime.Now.Subtract
            var seedDate = DateTime.Now.AddMonths(subtractMonths);
            while (seedDate < DateTime.Now)
            {
                foreach (var currTenant in tenantList)
                {
                    var s = new ReverseEngineerTotalByMonthService(seedDate, currTenant,null);
                    s.CheckDbIntegrity();
                }

                seedDate = seedDate.AddMonths(1);
            }
        }
        public void CheckDbIntegrity()
        {

            var sw = Stopwatch.StartNew();
            string debugIt = "";
            try
            {


                bool stopJournalApproval = true;
                if (stopJournalApproval)
                {
                    TODO_StopJournalApproval();
                }
                var startDayOfMonth = new DateTime(_SeedDate.Date.Year, _SeedDate.Date.Month, 1);
                var endDayOfMonth = //start.AddMonths(1).AddMinutes(-1);
                    new DateTime(_SeedDate.Date.Year, _SeedDate.Date.Month, DateTime.DaysInMonth(_SeedDate.Date.Year, _SeedDate.Date.Month));

                var listOfDateTypeValues = new List<string>() {
                    GLAccountTotalDateTypeValues.Accountingdate,
                    GLAccountTotalDateTypeValues.DueDate,
                    GLAccountTotalDateTypeValues.DocumentDate,
                };
                var myGLAccountTotalByMonthsList = new List<GLAccountTotalByMonthsDTO>();
                foreach (string dateTypeValue in listOfDateTypeValues)
                {
                    using (var scope = TransactionFactory.GetNewTransaction(TimeSpan.FromMinutes(25)))
                    {
                        _AccountingContext = AccountingContext.GetContext(_Tenant);

                        if ((this._AccountingContext as System.Data.Entity.DbContext).Database.CommandTimeout < 1200)//wrokerrole mode !!!
                        {
                            (this._AccountingContext as System.Data.Entity.DbContext).Database.CommandTimeout = 1200;
                        }
                        var myGLAccountRepo = new GLAccountRepository(_AccountingContext);
                        var quaryAllControlAccount = myGLAccountRepo.GetQuaryAllControlAccount(_Tenant);
                        var myGLAccountTotalByMonthRepo = new GLAccountTotalByMonthRepository(_AccountingContext);
                        var quaryablMonthTotals1 = myGLAccountTotalByMonthRepo.GetQuaryableMonthTotals(_SeedDate.Date.Year, _SeedDate.Date.Month, _Tenant,
                            dateTypeValue/*GLAccountTotalDateTypeValues.Accountingdate*/);


                        var quaryablMonthTotalsWithoutControl = (
                            from t in quaryablMonthTotals1
                            where !(from controlAcc in quaryAllControlAccount select controlAcc.Id)
                                    .Contains(t.AccountId)
                            select t);

                        var quaryablMonthTotalDTO = quaryablMonthTotalsWithoutControl
                            .Select(totalByMonth =>
                            new GLAccountTotalByMonthsDTO()
                            {

                                Tenant = totalByMonth.Tenant,
                                AccountId = totalByMonth.AccountId,
                                CurrencyId = totalByMonth.CurrencyId,
                                Year = totalByMonth.Year,
                                Month = totalByMonth.Month,
                                LocalAmountCredit = totalByMonth.LocalAmountCredit,
                                LocalAmountDebit = totalByMonth.LocalAmountDebit,
                                ForeignAmountCredit = (decimal)totalByMonth.ForeignAmountCredit,
                                ForeignAmountDebit = (decimal)totalByMonth.ForeignAmountDebit,
                                CHANGE_TYPE = ""
                            });


                        List<string> listOfAccId = null;
                        if (!String.IsNullOrWhiteSpace(this._GLAccountId))
                        {
                            quaryablMonthTotalDTO = quaryablMonthTotalDTO.Where(r => r.AccountId == this._GLAccountId);
                            listOfAccId = new List<string>() { this._GLAccountId };
                        }

                        var ledgerTransactionRepository = new LedgerTransactionRepository(_AccountingContext);
                        var qLedgerAsGLAccountTotalByMonthByAccountingDate = ledgerTransactionRepository.GetQueryableGLAccountTotalByMonthByDateTypeCode(
                            dateTypeValue, startDayOfMonth, endDayOfMonth, _Tenant, listOfAccId);




                        var qNotinLedgerTransaction = (from totalByMonth in quaryablMonthTotalDTO
                                                       join ledgerTransaction in qLedgerAsGLAccountTotalByMonthByAccountingDate
                                                       on
                                                       new { totalByMonth.AccountId, totalByMonth.CurrencyId, totalByMonth.Year, totalByMonth.Month } equals
                                                       new { ledgerTransaction.AccountId, ledgerTransaction.CurrencyId, ledgerTransaction.Year, ledgerTransaction.Month }
                                                       into joinT
                                                       from joinr in joinT.DefaultIfEmpty()
                                                       where joinr == null

                                                       select new GLAccountTotalByMonthsDTO()
                                                       {
                                                           Tenant = totalByMonth.Tenant,
                                                           AccountId = totalByMonth.AccountId,
                                                           CurrencyId = totalByMonth.CurrencyId,
                                                           Year = totalByMonth.Year,
                                                           Month = totalByMonth.Month,
                                                           LocalAmountCredit = totalByMonth.LocalAmountCredit,
                                                           LocalAmountDebit = totalByMonth.LocalAmountDebit,
                                                           ForeignAmountCredit = totalByMonth.ForeignAmountCredit,
                                                           ForeignAmountDebit = totalByMonth.ForeignAmountDebit,
                                                           CHANGE_TYPE = const_qNotinLedgerTransaction
                                                       }
                                                       );

                        var qNotinTotalByMonth = (from ledgerTransaction in qLedgerAsGLAccountTotalByMonthByAccountingDate
                                                  join totalByMonth in quaryablMonthTotalDTO
                                                  on
                                                  new { ledgerTransaction.AccountId, ledgerTransaction.CurrencyId, ledgerTransaction.Year, ledgerTransaction.Month }
                                                  equals
                                                  new { totalByMonth.AccountId, totalByMonth.CurrencyId, totalByMonth.Year, totalByMonth.Month }
                                                  into joinT
                                                  from joinr in joinT.DefaultIfEmpty()
                                                  where joinr == null
                                                  select new GLAccountTotalByMonthsDTO()
                                                  {
                                                      Tenant = ledgerTransaction.Tenant,
                                                      AccountId = ledgerTransaction.AccountId,
                                                      CurrencyId = ledgerTransaction.CurrencyId,
                                                      Year = ledgerTransaction.Year,
                                                      Month = ledgerTransaction.Month,
                                                      LocalAmountCredit = ledgerTransaction.LocalAmountCredit,
                                                      LocalAmountDebit = ledgerTransaction.LocalAmountDebit,
                                                      ForeignAmountCredit = ledgerTransaction.ForeignAmountCredit,
                                                      ForeignAmountDebit = ledgerTransaction.ForeignAmountDebit,
                                                      CHANGE_TYPE = Const_qNotinTotalByMonth
                                                  });


                        var qDiff = (from totalByMonth in quaryablMonthTotalDTO
                                     join ledgerTransaction in qLedgerAsGLAccountTotalByMonthByAccountingDate
                                     on
                                     new { totalByMonth.AccountId, totalByMonth.CurrencyId, totalByMonth.Year, totalByMonth.Month } equals
                                     new { ledgerTransaction.AccountId, ledgerTransaction.CurrencyId, ledgerTransaction.Year, ledgerTransaction.Month }
                                     into joinT
                                     from joinr in joinT
                                     where
                        !totalByMonth.ForeignAmountCredit.Equals(joinr.ForeignAmountCredit) ||
                        !totalByMonth.ForeignAmountDebit.Equals(joinr.ForeignAmountDebit) ||



                        !totalByMonth.LocalAmountCredit.Equals(joinr.LocalAmountCredit) ||
                        !totalByMonth.LocalAmountDebit.Equals(joinr.LocalAmountDebit)
                                     select new GLAccountTotalByMonthsDTO()
                                     {

                                         Tenant = totalByMonth.Tenant,
                                         AccountId = totalByMonth.AccountId,
                                         CurrencyId = totalByMonth.CurrencyId,
                                         Year = totalByMonth.Year,
                                         Month = totalByMonth.Month,
                                         LocalAmountCredit = totalByMonth.LocalAmountCredit - joinr.LocalAmountCredit,
                                         LocalAmountDebit = totalByMonth.LocalAmountDebit - joinr.LocalAmountDebit,
                                         ForeignAmountCredit = totalByMonth.ForeignAmountCredit - joinr.ForeignAmountCredit,
                                         ForeignAmountDebit = totalByMonth.ForeignAmountDebit - joinr.ForeignAmountDebit,
                                         CHANGE_TYPE = Const_qDiff
                                     });
                        var GLAccountTotalByMonthsList =
                            //qNotinLedgerTransaction.Union(qNotinTotalByMonth).Union(qDiff).ToList();
                            qNotinLedgerTransaction.Concat(qNotinTotalByMonth).Concat(qDiff).ToList();
                        ;
                        GLAccountTotalByMonthsList.ForEach(
                            r =>
                        {

                            r.DateTypeValue = dateTypeValue;
                            myGLAccountTotalByMonthsList.Add(r);
                        });


                    }

                    CompareReport = new CompareReportM()
                    {
                        CompareReportName = "ReverseEngineerTotalByMonthService",
                        Year = _SeedDate.Date.Year,
                        Month = _SeedDate.Date.Month,
                        //rows = res,
                        GLAccountTotalByMonthsList = myGLAccountTotalByMonthsList,
                        Took = sw.Elapsed
                    };
                }

            }
            finally
            {
                Debug.WriteLine(debugIt);
            }


        }

        private const string const_qNotinLedgerTransaction ="qNotinLedgerTransaction";
        private readonly string Const_qNotinTotalByMonth= "qNotinTotalByMonth";
        private readonly string Const_qDiff = "qDiff";

        public void FixDbIntegrityFromLedgeToTotal()
        {
            if (string.IsNullOrWhiteSpace(this._GLAccountId))
            {
                //throw new Exception("BETA- this._GLAccountId s must !!!!");

            }
            CheckDbIntegrity();
            if (this.CompareReport.GLAccountTotalByMonthsList == null || this.CompareReport.GLAccountTotalByMonthsList.Count == 0)
            {
                throw new Exception("is ok - nothing done  !!!!");
            }
            if (this.CompareReport.GLAccountTotalByMonthsList.Any( r=>r.CHANGE_TYPE == const_qNotinLedgerTransaction))
            {
                throw new Exception("contains qNotinLedgerTransaction FIX - the problem there is Total but any LedgerTransaction" +
                    "Deleting GLAccountTotalByMonths Requires A deeper examination - U do That not me!!!!");
            }
            using (var scope = TransactionFactory.GetNewSerializableTransaction())
            {
                _AccountingContext = AccountingContext.GetContext(_Tenant);
                var myGLAccountTotalByMonthRepository = new GLAccountTotalByMonthRepository(_AccountingContext);
                var tInsert=this.CompareReport.GLAccountTotalByMonthsList.Where(r => r.CHANGE_TYPE == Const_qNotinTotalByMonth).ToList();
                var tDeltaUpdate = this.CompareReport.GLAccountTotalByMonthsList.Where(r => r.CHANGE_TYPE == Const_qDiff).ToList();
                tInsert.ForEach(r =>
                {
                    myGLAccountTotalByMonthRepository.Add(new GLAccountTotalByMonth()
                    {
                        Tenant = this._Tenant,
                        AccountId = r.AccountId,
                        CurrencyId = r.CurrencyId,
                        DateTypeCode = r.DateTypeValue,// "1",// BETA-TO DO ...
                        Year = r.Year,
                        Month = r.Month,
                        ForeignAmountDebit = r.ForeignAmountDebit,
                        LocalAmountCredit = r.LocalAmountCredit,
                        ForeignAmountCredit = r.ForeignAmountCredit,
                        LocalAmountDebit = r.LocalAmountDebit


                    });
                });
                tDeltaUpdate.ForEach(r =>
                {
                    var poco = myGLAccountTotalByMonthRepository.GetSingle(r.AccountId, r.DateTypeValue, r.Year, r.Month, r.CurrencyId, _Tenant);
                    poco.ForeignAmountCredit -= r.ForeignAmountCredit;

                    poco.ForeignAmountDebit -= r.ForeignAmountDebit;
                    poco.LocalAmountCredit -= r.LocalAmountCredit;

                    poco.LocalAmountDebit -= r.LocalAmountDebit;
                    myGLAccountTotalByMonthRepository.Update(poco);

                });
                myGLAccountTotalByMonthRepository.SubmitChanges();
                scope.Complete();


            }

            
        }

        void CheckDbIntegrityOld()
        {
            var sw = Stopwatch.StartNew();
            string debugIt = "";
            try
            {


                bool stopJournalApproval = true;
                if (stopJournalApproval)
                {
                    TODO_StopJournalApproval();
                }
                var startDayOfMonth = new DateTime(_SeedDate.Date.Year, _SeedDate.Date.Month, 1);
                var endDayOfMonth = //start.AddMonths(1).AddMinutes(-1);
                    new DateTime(_SeedDate.Date.Year, _SeedDate.Date.Month, DateTime.DaysInMonth(_SeedDate.Date.Year, _SeedDate.Date.Month));

                List<GLAccountTotalByMonthPM> dbGLATotalByMonths;
                List<GLAccountTotalByMonth> calcGLATotalByMonthFromLTrans;
                using (var scope = TransactionFactory.GetNewTransaction())
                {
                    _AccountingContext = AccountingContext.GetContext(_Tenant);
                    var myGLAccountTotalByMonthQueryService = new GLAccountTotalByMonthQueryService(_AccountingContext);
                    dbGLATotalByMonths = myGLAccountTotalByMonthQueryService.GetMonthTotals(_SeedDate.Date.Year, _SeedDate.Date.Month, _Tenant);


                    var qs = new LedgerTransactionQueryService(_AccountingContext);
                    calcGLATotalByMonthFromLTrans = qs.CalcGLAccountTotalByMonthByAccountingDate(startDayOfMonth, endDayOfMonth, _Tenant);



                }
                var clacKeys = calcGLATotalByMonthFromLTrans.Select(rec => new { rec.AccountId, rec.CurrencyId });
                var dbKeys = dbGLATotalByMonths.Select(rec => new { rec.AccountId, rec.CurrencyId });

                var haveOnlyAtLedgerTransaction = clacKeys.Except(dbKeys);
                var haveOnlyAtGLAccountTotalByMonth = dbKeys.Except(clacKeys);

                var qDifferent = (
                    from db in dbGLATotalByMonths
                    join calc in calcGLATotalByMonthFromLTrans
                    on new { db.AccountId, db.CurrencyId } equals new { calc.AccountId, calc.CurrencyId }
                    //into dbJoinCalc
                    where
                    !db.ForeignAmountCredit.Equals(calc.ForeignAmountCredit) ||
                    !db.ForeignAmountDebit.Equals(calc.ForeignAmountDebit) ||
                    !db.LocalAmountCredit.Equals(calc.LocalAmountCredit) ||
                    !db.LocalAmountDebit.Equals(calc.LocalAmountDebit)
                    select new { db, calc }
                     );
                var diff = qDifferent.ToList();
                debugIt = String.Format("year{0}:Month:{1}:haveOnlyAtLedgerTransaction{2}:haveOnlyAtGLAccountTotalByMonth{3}:Different{4}",
                    _SeedDate.Date.Year, _SeedDate.Date.Month, haveOnlyAtLedgerTransaction.Count().ToString(), haveOnlyAtGLAccountTotalByMonth.Count().ToString(), qDifferent.Count().ToString());
            }
            finally
            {
                Debug.WriteLine(debugIt);
            }


        }

        private void TODO_StopJournalApproval()
        {
            //throw new NotImplementedException();
        }


        public CompareReportM CompareReport { get; set; }
    }
}
