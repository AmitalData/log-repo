using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Data.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Accounting.BL.CloseTables;


namespace Logitude.Accounting.BL.Utils
{
    public static class GLAccountTotalByMonthsBatch
    {


        public static void CalcTotalsForLastTwoMonths(string accountId, int tenant, int monthBack, DateTime date)
        {
            try
            {
                IAccountingContext context = AccountingContext.GetContext(tenant);
                GLAccountTotalByMonthUpdateService myGLAccountTotalByMonthsUpdateServices = new GLAccountTotalByMonthUpdateService(context, new Dictionary<string, IContext>(), tenant);
                LedgerTransactionQueryService myLedgerTransactionQueryService = new LedgerTransactionQueryService(context);
                GLAccountTotalByMonthQueryService myGLAccountTotalByMonthsQueryServices = new GLAccountTotalByMonthQueryService(tenant);

                for (int i = monthBack; i > 0; i--)
                {
                    DateTime TwoMonth = date.AddMonths(-monthBack);
                    DateTime firstDayOfTwoMonth = new DateTime(TwoMonth.Year, TwoMonth.Month, 1);
                    DateTime lastDayOfTwoMonth = firstDayOfTwoMonth.AddMonths(1).AddDays(-1);

                    List<CurrencySum> myLTSum = myLedgerTransactionQueryService.GetLedgerTransactionTotalLocalAmountFromTo(accountId, firstDayOfTwoMonth, lastDayOfTwoMonth, tenant);

                    foreach (CurrencySum item in myLTSum)
                    {
                        GLAccountTotalByMonthPM myGLAccountTotal = myGLAccountTotalByMonthsQueryServices.GetSingle(accountId,GLAccountTotalDateTypeValues.Accountingdate, TwoMonth.Year, TwoMonth.Month, item.CurrencyId, false, false);

                        if (myGLAccountTotal != null)
                        {
                            myGLAccountTotal.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;

                            myGLAccountTotal.ForeignAmountCredit = item.ForeignAmountCredit;
                            myGLAccountTotal.ForeignAmountDebit = item.ForeignAmountDebit;
                            myGLAccountTotal.LocalAmountCredit = (decimal)item.LocalAmountCredit;
                            myGLAccountTotal.LocalAmountDebit = (decimal)item.LocalAmountDebit;

                        }
                        else
                        {
                            myGLAccountTotal = new GLAccountTotalByMonthPM();
                            myGLAccountTotal.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
                            myGLAccountTotal.AccountId = accountId;
                            myGLAccountTotal.Tenant = tenant;
                            myGLAccountTotal.Year = TwoMonth.Year;
                            myGLAccountTotal.Month = TwoMonth.Month;
                            myGLAccountTotal.CurrencyId = item.CurrencyId;
                            myGLAccountTotal.ForeignAmountCredit = item.ForeignAmountCredit;
                            myGLAccountTotal.ForeignAmountDebit = item.ForeignAmountDebit;
                            myGLAccountTotal.LocalAmountCredit = (decimal)item.LocalAmountCredit;
                            myGLAccountTotal.LocalAmountDebit = (decimal)item.LocalAmountDebit;

                        }

                        myGLAccountTotalByMonthsUpdateServices.Update(myGLAccountTotal, true);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }



        }

        public static void CalcTotalsForLastTwoMonthsInBatch(string accountId, int tenant,  int monthBack, DateTime date)
        {
            Task task = Task.Run(() => 
            {
                try
                {
                    CalcTotalsForLastTwoMonths(accountId, tenant, monthBack, date);
                }
                catch (Exception )
                {
                    
                    throw;
                }
               
            });
            
        }

    }
}
