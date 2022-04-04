using Logitude.Accounting.BL.CloseTables;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL.ReverseEngineer
{
    public class ReverseEngineerControlAccountAccumulateChild
    {
        private int _Tenant;
        private IAccountingContext _AccountingContext;
        private bool _TestIt;

        public ReverseEngineerControlAccountAccumulateChild(int myTenant)
        {
            this._Tenant = myTenant;
        }

        public CompareReportM CompareReport { get; private set; }

        public void CheckDbIntegrity(DateTime? dateMonth=null)
        {
            var sw = Stopwatch.StartNew();
            using (var scope = TransactionFactory.GetNewTransaction(TimeSpan.FromMinutes(10)))
            {
                _AccountingContext = AccountingContext.GetContext(_Tenant);

                var myGLAccountRepo = new GLAccountRepository(_AccountingContext);
                

                var myGLAccountTotalByMonthRepo = new GLAccountTotalByMonthRepository(_AccountingContext);

                


                var qChildsAsContol =
                    (from acc in myGLAccountRepo.GetAll(_Tenant).Where(r => r.ControlAccountId != null)
                     join tot in myGLAccountTotalByMonthRepo.GetAll(_Tenant).Where(r => r.DateTypeCode == GLAccountTotalDateTypeValues.Accountingdate)
                     on acc.Id equals tot.AccountId
                     select new { acc.ControlAccountId,tot.Year,tot.Month, tot.LocalAmountDebit,tot.LocalAmountCredit }
                     
                     );
                if (dateMonth!=null)
                {
                    int year = dateMonth.GetValueOrDefault().Year;
                    int Month = dateMonth.GetValueOrDefault().Month;
                    qChildsAsContol = qChildsAsContol
                        .Where(r => r.Year == year)
                        .Where(r => r.Month == Month);
                        

                }
                var qChildGroupTotal= (from a in qChildsAsContol
                        group a by a.ControlAccountId into g
                        select new GLAccountBalanceDTO
                        {
                            AccountId = g.Key,
                            BalanceInLocalCurrency = g.Sum(r => r.LocalAmountDebit - r.LocalAmountCredit),
                            CHANGE_TYPE = ""

                        }
                 );

                _TestIt = false;
                if (_TestIt)
                {
                    var calcTotalfromChild = qChildGroupTotal.ToList();
                    var contolAccountIDs = calcTotalfromChild.Select(r => r.AccountId);//max 10
                }



                var qControlIds = (from acc in myGLAccountRepo.GetAll(_Tenant).Where(r => r.ControlAccountId != null)
                          select acc.ControlAccountId
                        );
                var qContolTotal =
                (
                //from acc in myGLAccountRepo.GetAll(_Tenant).Where(r => r.ControlAccountId != null)
                from tot in myGLAccountTotalByMonthRepo
                .GetAll(_Tenant).Where(r => r.DateTypeCode == GLAccountTotalDateTypeValues.Accountingdate)
                 //on acc.ControlAccountId equals tot.AccountId
                 .Where(r=> qControlIds.Contains(r.AccountId))
                 select new { tot.AccountId, tot.Year, tot.Month, tot.LocalAmountDebit, tot.LocalAmountCredit }

                 );
                if (dateMonth != null)
                {
                    int year = dateMonth.GetValueOrDefault().Year;
                    int Month = dateMonth.GetValueOrDefault().Month;
                    qContolTotal = qContolTotal
                        .Where(r => r.Year == year)
                        .Where(r => r.Month == Month);
                }
                var qControlGroupTotal = (from a in qContolTotal
                                          group a by a.AccountId into g
                                        select new GLAccountBalanceDTO
                                        {
                                            AccountId = g.Key,
                                            BalanceInLocalCurrency = g.Sum(r => r.LocalAmountDebit - r.LocalAmountCredit),
                                            CHANGE_TYPE = ""

                                        }
                );

                if (_TestIt)
                {
                    var calcTotalControls = qControlGroupTotal.ToList();
                    var contolAccountIDs = calcTotalControls.Select(r => r.AccountId);//max 10
                }

                var qDiff = (
           from childsTotal in qChildGroupTotal
           join controlTotal in qControlGroupTotal
           on childsTotal.AccountId equals controlTotal.AccountId
           where (
           childsTotal.BalanceInLocalCurrency - controlTotal.BalanceInLocalCurrency >= 0.001m ||
           childsTotal.BalanceInLocalCurrency - controlTotal.BalanceInLocalCurrency <= -0.001m)
           select new GLAccountBalanceDTO
           {
               AccountId = childsTotal.AccountId,

               BalanceInLocalCurrency = childsTotal.BalanceInLocalCurrency - controlTotal.BalanceInLocalCurrency,
               CHANGE_TYPE = "the diff childsTotal-controlTotal "
           }

           );
                var l = qDiff.Take(30).ToList();
                CompareReport = new CompareReportM()
                {
                    CompareReportName = "totalControl=totalChilds",
                    //rows = res,
                    GLAccountBalanceList = l,
                    ///TotalOpenReconciliation = myTotalOpenReconciliation,
                    Took = sw.Elapsed
                };
                Convert2DisplayNumber(CompareReport.GLAccountBalanceList, _Tenant);


            }
        }

        private void Convert2DisplayNumber(List<GLAccountBalanceDTO> rows, int tenant)
        {
            if (rows == null)
            {
                return;
            }
            try
            {
                var AccountIdList = rows.Where(r => !string.IsNullOrWhiteSpace(r.AccountId)).Select(x => x.AccountId).Distinct().ToList();
                var repo = new GLAccountRepository(tenant);
                var res = repo.GetDisplayNumberList(AccountIdList.ToHashSet(), tenant);
                foreach (var item in rows)
                {
                    var display = res.FirstOrDefault(r => r.Key == item.AccountId);
                    if (string.IsNullOrEmpty(display.Value))
                    {
                        continue;
                    }
                    item.AccountDisplayNumber= display.Value;
                }
            }
            catch (Exception)
            {


            }
        }
    }
}
