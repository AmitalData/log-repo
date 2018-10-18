using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.Utils
{
    public class GLAccountEndOfTheYearBatch
    {
        public static void GLAccountBalanceByYear(string accountId, int tenant, int year)
        {
            try
            {
                GLAccountTotalByMonthsBatch.CalcTotalsForLastTwoMonths(accountId, tenant, 12, new DateTime(year+1,01,01));

                IAccountingContext context = AccountingContext.GetContext(tenant);
//                GLAccountBalanceByYearUpdateService myGLAccountBalanceByYearUpdateService = new GLAccountBalanceByYearUpdateService(context, new Dictionary<string, IContext>(), tenant);
//                GLAccountBalanceByYearQueryService myGLAccountBalanceByYearQueryService = new GLAccountBalanceByYearQueryService(context);
                GLAccountTotalByMonthQueryService myGLAccountTotalByMonthsQueryServices = new GLAccountTotalByMonthQueryService(tenant);

                List<CurrencySum> myLTSum = myGLAccountTotalByMonthsQueryServices.GetSumByMonth(accountId, year, 12, tenant);

                foreach (CurrencySum item in myLTSum)
                {
//                    GLAccountBalanceByYearPM myGLAccountBalanceByYearPM = myGLAccountBalanceByYearQueryService.GetSingle(accountId, year, item.CurrencyId, false, false);

                    //if (myGLAccountBalanceByYearPM != null)
                    //{
                    //    myGLAccountBalanceByYearPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                    //    myGLAccountBalanceByYearPM.ForeignAmount = item.ForeignAmountCredit + item.ForeignAmountDebit;
                    //    myGLAccountBalanceByYearPM.LocalAmount = (decimal)item.LocalAmountCredit + (decimal)item.LocalAmountDebit;
                    //}
                    //else
                    //{
                    //    myGLAccountBalanceByYearPM = new GLAccountBalanceByYearPM();
                    //    myGLAccountBalanceByYearPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
                    //    myGLAccountBalanceByYearPM.AccountId = accountId;
                    //    myGLAccountBalanceByYearPM.Tenant = tenant;
                    //    myGLAccountBalanceByYearPM.Year = year;
                    //    myGLAccountBalanceByYearPM.CurrencyId = item.CurrencyId;
                    //    myGLAccountBalanceByYearPM.ForeignAmount = item.ForeignAmountCredit - item.ForeignAmountDebit;
                    //    myGLAccountBalanceByYearPM.LocalAmount = (decimal)item.LocalAmountCredit - (decimal)item.LocalAmountDebit;
                    //}
                    //myGLAccountBalanceByYearUpdateService.Update(myGLAccountBalanceByYearPM, true);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        public static void GLAccountRevenueExpenseTransfer(int year, int tenant)
        {
            try
            {
                IAccountingContext context = AccountingContext.GetContext(tenant);
                GLAccountTotalByMonthQueryService myGLAccountTotalByMonthsQueryService = new GLAccountTotalByMonthQueryService(tenant);
//                GLAccountBalanceByYearListQueryService myGLAccountBalanceByYearListQueryService = new GLAccountBalanceByYearListQueryService(context);
 //               GLAccountBalanceByYearQueryService myGLAccountBalanceByYearQueryService = new GLAccountBalanceByYearQueryService(context);
//                GLAccountBalanceByYearUpdateService myGLAccountBalanceByYearUpdateService = new GLAccountBalanceByYearUpdateService(context, new Dictionary<string, IContext>(), tenant);
                GLAccountListQueryService myGLAccountListQueryService = new GLAccountListQueryService(context);
                GLAccountQueryService myGLAccountQueryService = new GLAccountQueryService(context);
                FullAccountingSettingUpdateService myFullAccountingSettingUpdateService = new FullAccountingSettingUpdateService(context, new Dictionary<string, IContext>(), tenant);
                FullAccountingSettingQueryService myFullAccountingSettingQueryService = new FullAccountingSettingQueryService(context);

                List<RevenueExpenseGLAccountsEndOfTheYearSums> RevenueExpenseList = new List<RevenueExpenseGLAccountsEndOfTheYearSums>();

                List<GLAccountList> revenueExpenseGLAccountList = myGLAccountListQueryService.GetRevenueExpenseGLAccountList(tenant);

                foreach (GLAccountList item in revenueExpenseGLAccountList)
                {
                    GLAccountBalanceByYear(item.CurrencyId, tenant, year);

  //                  List<GLAccountBalanceByYearList> glaccountBalanceByYearList = myGLAccountBalanceByYearListQueryService.GetYearBalanceByAccounyId(item.Id, year);

                    //foreach (GLAccountBalanceByYearList glaccountBalanceByYearItem in glaccountBalanceByYearList)
                    //{

                    //    RevenueExpenseGLAccountsEndOfTheYearSums mylist = (from a in RevenueExpenseList
                    //                                                       where a.CurrencyId == glaccountBalanceByYearItem.CurrencyId
                    //                                                       select a).FirstOrDefault();

                    //    if (mylist != null)
                    //    {
                    //        mylist.ForeignAmount += (decimal)glaccountBalanceByYearItem.ForeignAmount;
                    //        mylist.LocalAmount += (decimal)glaccountBalanceByYearItem.LocalAmount;
                    //    }
                    //    else
                    //    {
                    //        RevenueExpenseGLAccountsEndOfTheYearSums RevenueExpenseItem = new RevenueExpenseGLAccountsEndOfTheYearSums();
                    //        RevenueExpenseItem.ForeignAmount += (decimal)glaccountBalanceByYearItem.ForeignAmount;
                    //        RevenueExpenseItem.LocalAmount += (decimal)glaccountBalanceByYearItem.LocalAmount;
                    //        RevenueExpenseItem.CurrencyId = glaccountBalanceByYearItem.CurrencyId;
                    //        RevenueExpenseList.Add(RevenueExpenseItem);
                    //    }

                    //}
                }

                FullAccountingSettingPM myFullAccountingSettingPM = myFullAccountingSettingQueryService.GetSingleFullAccountingSetting(tenant);

//                List<GLAccountBalanceByYearList> myRevenueExpenseGLAcountBalanceByYearList = myGLAccountBalanceByYearListQueryService.GetYearBalanceByAccounyId(myFullAccountingSettingPM.RevenueExpenseGLAccountId, year);

//                List<GLAccountBalanceByYearPM> myNewGLAccountBalanceByYearPMList = new List<GLAccountBalanceByYearPM>();

                //foreach (RevenueExpenseGLAccountsEndOfTheYearSums item in RevenueExpenseList)
                //{
                //    GLAccountBalanceByYearList mylist = (from a in myRevenueExpenseGLAcountBalanceByYearList
                //                                         where a.CurrencyId == item.CurrencyId
                //                                         select a).FirstOrDefault();

                //    GLAccountBalanceByYearPM myGLAccountBalanceByYearPM;

                //    if (mylist == null)
                //    {
                //        myGLAccountBalanceByYearPM = new GLAccountBalanceByYearPM();
                //        myGLAccountBalanceByYearPM.ChangeSetOp = ChangeSetOperation.Insert;
                //        myGLAccountBalanceByYearPM.AccountId = myFullAccountingSettingPM.RevenueExpenseGLAccountId;
                //        myGLAccountBalanceByYearPM.CurrencyId = item.CurrencyId;
                //        myGLAccountBalanceByYearPM.Tenant = tenant;
                //        myGLAccountBalanceByYearPM.Year = year;
                //        myGLAccountBalanceByYearPM.ForeignAmount = item.ForeignAmount;
                //        myGLAccountBalanceByYearPM.LocalAmount = item.LocalAmount;
                //    }
                //    else
                //    {
                //        myGLAccountBalanceByYearPM = myGLAccountBalanceByYearQueryService.GetSingle(myFullAccountingSettingPM.RevenueExpenseGLAccountId, year, item.CurrencyId, false, false);
                //        myGLAccountBalanceByYearPM.ChangeSetOp = ChangeSetOperation.Update;
                //        myGLAccountBalanceByYearPM.ForeignAmount = item.ForeignAmount;
                //        myGLAccountBalanceByYearPM.LocalAmount = item.LocalAmount;
                //    }
                //    myNewGLAccountBalanceByYearPMList.Add(myGLAccountBalanceByYearPM);
                //}
                //myGLAccountBalanceByYearUpdateService.UpdateMulti(myNewGLAccountBalanceByYearPMList, new List<GLAccountBalanceByYearPM>(), new GLAccountBalanceByYearPM(), true);
            }
            catch (Exception)
            {
                throw;
            }
        }


        private class RevenueExpenseGLAccountsEndOfTheYearSums
        {
            public decimal ForeignAmount { get; set; }
            public decimal LocalAmount{ get; set; }
            public string CurrencyId{ get; set; }
        }

    }

}

