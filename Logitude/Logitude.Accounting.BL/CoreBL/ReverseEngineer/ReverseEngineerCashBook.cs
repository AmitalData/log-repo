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
    public class ReverseEngineerCashBook
    {
        private int _Tenant;
        private IAccountingContext _AccountingContext;
        private bool _TestIt;
        

        public ReverseEngineerCashBook(int myTenant)
        {
            this._Tenant = myTenant;
        }

        public CompareReportM CompareReport { get; private set; }

        public void CheckDbIntegrity()
        {
            var sw = Stopwatch.StartNew();
            using (var scope = TransactionFactory.GetNewTransaction(TimeSpan.FromMinutes(10)))
            {
                _AccountingContext = AccountingContext.GetContext(_Tenant);

                var myGLAccountMoreDataRepository = new GLAccountMoreDataRepository(_AccountingContext);


                var myCashBookRepository = new CashBookRepository(_AccountingContext);



                var q = (from c in myCashBookRepository.GetAll(_Tenant)
                         join a in myGLAccountMoreDataRepository.GetAll(_Tenant)
                         on c.AccountId equals a.AccountId
                         where a.LocalBalanceInDue!= c.TotalAmount


                         select new GLAccountBalanceDTO
                         {
                             AccountId = c.AccountId,
                             BalanceInLocalCurrency = c.TotalAmount.GetValueOrDefault() - a.BalanceInLocalCurrency,
                             CHANGE_TYPE = c.LocalName + "  היתרה בקופה שונה מהיתרה בכרטיס הנחש"

                         }
                         );


          



                

           
                var l = q.ToList();
                CompareReport = new CompareReportM()
                {
                    CompareReportName = "ReverseEngineerCashBook",
                    //rows = res,
                    GLAccountBalanceList = l,
                    ///TotalOpenReconciliation = myTotalOpenReconciliation,
                    Took = sw.Elapsed
                };
            }

        }
    }
}
