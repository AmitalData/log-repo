using Logitude.Accounting.Data;
using Logitude.Accounting.Data.Repositories;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
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
            TenantQuery tenantQuery = new TenantQuery(_Tenant);
            TenantPM tPM = tenantQuery.GetSinglePM(_Tenant);
            string accountingCurrencyId = tPM.CurrencyId;

            var sw = Stopwatch.StartNew();
            using (var scope = TransactionFactory.GetNewTransaction(TimeSpan.FromMinutes(10)))
            {
                _AccountingContext = AccountingContext.GetContext(_Tenant);

                var myGLAccountMoreDataRepository = new GLAccountMoreDataRepository(_AccountingContext);


                var myCashBookRepository = new CashBookRepository(_AccountingContext);



                var qAccountingCurrencyId = (from c in myCashBookRepository.GetAll(_Tenant).Where( r=>r.CurrencyId == accountingCurrencyId )
                         join a in myGLAccountMoreDataRepository.GetAll(_Tenant)
                         on c.AccountId equals a.AccountId
                         where a.BalanceInLocalCurrency!= c.TotalAmount 

                    

                         select new GLAccountBalanceDTO
                         {
                             AccountId = c.AccountId,
                             BalanceInLocalCurrency = c.TotalAmount ?? 0 - a.BalanceInLocalCurrency,
                             CHANGE_TYPE = c.LocalName + "  היתרה בקופה שונה מהיתרה בכרטיס הנחש"

                         }
                         );

                var qForeignCurrencyId = (from c in myCashBookRepository.GetAll(_Tenant).Where(r => r.CurrencyId != accountingCurrencyId)
                                             join a in myGLAccountMoreDataRepository.GetAll(_Tenant)
                                             on c.AccountId equals a.AccountId
                                             where a.BalanceInForeignCurrency != c.TotalAmount



                                             select new GLAccountBalanceDTO
                                             {
                                                 AccountId = c.AccountId,
                                                 BalanceInLocalCurrency = c.TotalAmount ?? 0 - a.BalanceInLocalCurrency,
                                                 CHANGE_TYPE = c.LocalName + "  היתרה מטח בקופה שונה מהיתרה בכרטיס הנחש"

                                             }
                         );








                var l = qAccountingCurrencyId.Take(30).Union(qForeignCurrencyId.Take(30)).ToList();
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
