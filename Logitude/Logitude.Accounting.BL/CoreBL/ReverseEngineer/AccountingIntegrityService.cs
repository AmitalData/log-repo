using Logitude.Accounting.BL.CoreBL.Reports.Aging;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.EntityUpdateServices;
using Logitude.Infrastructure.Data;
using Logitude.Server.Tools.QueueService;
using Logitude.Server.Tools.Utils;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Logitude.Accounting.BL.CoreBL.ReverseEngineer
{
    public class AccountingIntegrityService
    {
        public string CheckParams(AccountingIntegrityInParam accountingIntegrityInParam)
        {
            if (accountingIntegrityInParam == null)
            {
                return "accountingIntegrityInParam is null";
            }
            var fullAccountingSettingPM = FullAccountingSettingQueryService.Get(accountingIntegrityInParam.Tenant);
            if (fullAccountingSettingPM == null)
            {
                return "Full Accounting Setting is null for tenant " + accountingIntegrityInParam.Tenant;
            }
            if (accountingIntegrityInParam.FromMonthInclusive.Year != accountingIntegrityInParam.ToMonthInclusive.Year)
            {
                return "From date and To date must be same year";
            }
            if (accountingIntegrityInParam.FromMonthInclusive == null)
            {
                return "From Month not selected";
            }
            if (accountingIntegrityInParam.ToMonthInclusive == null)
            {
                return "To Month not selected";
            }
            if (accountingIntegrityInParam.FromMonthInclusive.Month > accountingIntegrityInParam.ToMonthInclusive.Month)
            {
                return "To month is greater than from month";
            }

            if (accountingIntegrityInParam.ToMonthInclusive.Subtract(accountingIntegrityInParam.FromMonthInclusive) > TimeSpan.FromDays(365))
            {
                return "day  Subtract  > 365 ";
            }
            return "";


        }
        public AccountingIntegrityResult CheckIntegrity(AccountingIntegrityInParam accountingIntegrityInParam)
        {

            string errorMessage = CheckParams(accountingIntegrityInParam);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                throw new Exception(errorMessage);
            }

            var myAccountingIntegrityResult = new AccountingIntegrityResult();
            try
            {
                JournalLineToLedgerCheck(accountingIntegrityInParam, myAccountingIntegrityResult);
                LedgerToMonthTotalCheck(accountingIntegrityInParam, myAccountingIntegrityResult);

                GLAccountBalanceCheck(accountingIntegrityInParam, myAccountingIntegrityResult);

                NetCommonHelper.Logger.DevLog.Instance.WriteDebug(String.Format("AccountingIntegrityService, Point 1, tenant {0}", accountingIntegrityInParam.Tenant));
                var myDueLocalBalanceService = new DueLocalBalanceService();
                myDueLocalBalanceService.ReBuild(accountingIntegrityInParam.Tenant, null, false); // fastRun=false 
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug(String.Format("AccountingIntegrityService, Point 2, tenant {0}", accountingIntegrityInParam.Tenant));


                DueLocalBalanceCheck(accountingIntegrityInParam, myAccountingIntegrityResult);
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug(String.Format("AccountingIntegrityService, Point 3, tenant {0}", accountingIntegrityInParam.Tenant));


                LedgerOpenAmountDiffCheck(accountingIntegrityInParam, myAccountingIntegrityResult);
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug(String.Format("AccountingIntegrityService, Point 4, tenant {0}", accountingIntegrityInParam.Tenant));

                InterestReportDiffCheck(accountingIntegrityInParam, myAccountingIntegrityResult);
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug(String.Format("AccountingIntegrityService, Point 5, tenant {0}", accountingIntegrityInParam.Tenant));

                RebuildAgingDataByType(accountingIntegrityInParam, myAccountingIntegrityResult, "Customer2");
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug(String.Format("AccountingIntegrityService, Point 6, tenant {0}", accountingIntegrityInParam.Tenant));

                RebuildAgingDataByType(accountingIntegrityInParam, myAccountingIntegrityResult, "Vendor3");
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug(String.Format("AccountingIntegrityService, Point 7, tenant {0}", accountingIntegrityInParam.Tenant));


            }
            catch
            {
                myAccountingIntegrityResult.HasException = true;
            }
            finally
            {
                if (!myAccountingIntegrityResult.HasException)
                {
                    if (myAccountingIntegrityResult.MyAccountingIntegrityStep != null)
                    {
                        myAccountingIntegrityResult.HasException = myAccountingIntegrityResult.MyAccountingIntegrityStep
                            .Any(r => !String.IsNullOrWhiteSpace(r.ExceptionMessage));


                        myAccountingIntegrityResult.ShouldFix
                            = myAccountingIntegrityResult.MyAccountingIntegrityStep
                            .Any(r => r.ShouldFix);
                    }

                }
            }
            return myAccountingIntegrityResult;
        }

        private static void RebuildAgingData(AccountingIntegrityInParam accountingIntegrityInParam, AccountingIntegrityResult myAccountingIntegrityResult)
        {
            var sw = Stopwatch.StartNew();
            string ExceptionMessage = "";
            int badRows = 0;

            try
            {
                var dailyRebuildAgingService = new DailyRebuildAgingService();
                dailyRebuildAgingService.ReBuild(accountingIntegrityInParam.Tenant);
            }
            catch (Exception ee)
            {
                ExceptionMessage = ee.ToString();
                //throw;
            }
            finally
            {

                myAccountingIntegrityResult.MyAccountingIntegrityStep = myAccountingIntegrityResult.MyAccountingIntegrityStep ?? new List<AccountingIntegrityStep>();
                myAccountingIntegrityResult.MyAccountingIntegrityStep.Add(new AccountingIntegrityStep()
                {
                    Name = System.Reflection.MethodBase.GetCurrentMethod().Name,
                    //Month = currentMonth,
                    ExceptionMessage = ExceptionMessage,
                    BadRows = badRows,
                    ShouldFix = (badRows > 0 && String.IsNullOrWhiteSpace(ExceptionMessage)),
                    ElapsedMilliseconds = sw.ElapsedMilliseconds,
                });
            }



        }


        private static void RebuildAgingDataByType(AccountingIntegrityInParam accountingIntegrityInParam, AccountingIntegrityResult myAccountingIntegrityResult, string type)
        {
            var sw = Stopwatch.StartNew();
            string ExceptionMessage = "";
            int badRows = 0;

            try
            {
                var dailyRebuildAgingService = new DailyRebuildAgingService();
                dailyRebuildAgingService.ReBuildByType(accountingIntegrityInParam.Tenant, type);
            }
            catch (Exception ee)
            {
                ExceptionMessage = ee.ToString();
                //throw;
            }
            finally
            {

                myAccountingIntegrityResult.MyAccountingIntegrityStep = myAccountingIntegrityResult.MyAccountingIntegrityStep ?? new List<AccountingIntegrityStep>();
                myAccountingIntegrityResult.MyAccountingIntegrityStep.Add(new AccountingIntegrityStep()
                {
                    Name = System.Reflection.MethodBase.GetCurrentMethod().Name,
                    //Month = currentMonth,
                    ExceptionMessage = ExceptionMessage,
                    BadRows = badRows,
                    ShouldFix = (badRows > 0 && String.IsNullOrWhiteSpace(ExceptionMessage)),
                    ElapsedMilliseconds = sw.ElapsedMilliseconds,
                });
            }



        }


        public void FixDBIntegrity(int tenant, List<AccountingIntegrityStep> MyAccountingIntegrityStep)
        {
            MyAccountingIntegrityStep.ForEach(s => s.ExceptionMessage = null);
            var sb = new StringBuilder();
            try
            {

                sb.Append($"FixDBIntegrity({tenant}) start at").AppendLine(DateTime.Now.ToString());


                var JournalLineToLedgerStepList = MyAccountingIntegrityStep.Where(r => r.Name == "JournalLineToLedgerCheck");
                var JournalLineToLedgerStep = JournalLineToLedgerStepList.Where(r => r.BadRows > 0).FirstOrDefault();
                if (JournalLineToLedgerStepList != null)
                {
                    throw new Exception("no abilty to fix JournalLineToLedger !!!");
                }

                var ledgerToMonthTotalCheckSteps = MyAccountingIntegrityStep
                .Where(r => r.Name == "LedgerToMonthTotalCheck" && r.BadRows > 0)
                .OrderBy(r => r.Month)
                .ToList();
                ledgerToMonthTotalCheckSteps.ForEach(r =>
                {
                    try
                    {
                        sb.Append($"ReverseEngineerTotalByMonthService({r.Month.GetValueOrDefault().ToString()})").AppendLine(DateTime.Now.ToString());
                        var reverseEngineerTotalByMonthService = new ReverseEngineerTotalByMonthService(r.Month.Value, tenant, null);
                        reverseEngineerTotalByMonthService.FixDbIntegrityFromLedgeToTotal();

                    }
                    catch (Exception ee)
                    {
                        r.ExceptionMessage = ee.ToString();
                        sb.AppendLine(ee.ToString());

                    }
                });


                var myGLAccountBalanceCheck = MyAccountingIntegrityStep
                    .FirstOrDefault(r => r.Name == "GLAccountBalanceCheck" && r.BadRows > 0);
                if (myGLAccountBalanceCheck != null)
                {
                    try
                    {
                        sb.AppendLine($"ReverseEngineerGLAccountBalance()").AppendLine(DateTime.Now.ToString());
                        var reverseEngineerGLAccountBalance = new ReverseEngineerGLAccountBalance(tenant);
                        reverseEngineerGLAccountBalance.FIXCheckDbIntegrity();

                    }
                    catch (Exception ee)
                    {
                        myGLAccountBalanceCheck.ExceptionMessage = ee.ToString();
                        sb.AppendLine(ee.ToString());
                    }
                }

                var myDueLocalBalanceCheck = MyAccountingIntegrityStep
                   .FirstOrDefault(r => r.Name == "DueLocalBalanceCheck" && r.BadRows > 0);
                if (myDueLocalBalanceCheck != null)
                {
                    try
                    {
                        sb.AppendLine($"DueLocalBalanceService()").AppendLine(DateTime.Now.ToString());
                        var myDueLocalBalanceService = new DueLocalBalanceService();
                        myDueLocalBalanceService.ReBuild(tenant, null);

                    }
                    catch (Exception ee)
                    {
                        myDueLocalBalanceCheck.ExceptionMessage = ee.ToString();
                        sb.AppendLine(ee.ToString());
                    }
                }
            }
            catch (Exception eee)
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteFatal(eee, "AccountingIntegrityService");
                sb.AppendLine(eee.ToString());
                throw new Exception(eee.ToString(), eee);// Itzik the exceptions is not thrown so i threw them
            }
            finally
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteInfo("AccountingIntegrityService:"+sb.ToString() );
            }



        }


        private void LedgerOpenAmountDiffCheck(AccountingIntegrityInParam accountingIntegrityInParam, AccountingIntegrityResult myAccountingIntegrityResult)
        {


            int year = accountingIntegrityInParam.FromMonthInclusive.Year;
            var icurrentMonth = accountingIntegrityInParam.FromMonthInclusive.Month;



            var sw = Stopwatch.StartNew();
            string ExceptionMessage = "";
            int badRows = 0;

            try
            {
                var myReconcileOpenAmountService = new ReconcileOpenAmountService();
                var listDiff = myReconcileOpenAmountService.GetLedgerOpenAmountDiff(accountingIntegrityInParam.Tenant, year);

                myAccountingIntegrityResult.LedgerOpenAmount = myAccountingIntegrityResult.LedgerOpenAmount ?? new List<LedgerOpenAmountRecoDiffM>();
                myAccountingIntegrityResult.LedgerOpenAmount.AddRange(listDiff);
                badRows = listDiff.Count();
            }
            catch (Exception ee)
            {
                ExceptionMessage = ee.ToString();
                //throw;
            }
            finally
            {

                myAccountingIntegrityResult.MyAccountingIntegrityStep = myAccountingIntegrityResult.MyAccountingIntegrityStep ?? new List<AccountingIntegrityStep>();
                myAccountingIntegrityResult.MyAccountingIntegrityStep.Add(new AccountingIntegrityStep()
                {
                    Name = System.Reflection.MethodBase.GetCurrentMethod().Name,
                    //Month = currentMonth,
                    ExceptionMessage = ExceptionMessage,
                    BadRows = badRows,
                    ShouldFix = (badRows > 0 && String.IsNullOrWhiteSpace(ExceptionMessage)),
                    ElapsedMilliseconds = sw.ElapsedMilliseconds,
                });
            }



        }




        private void InterestReportDiffCheck(AccountingIntegrityInParam accountingIntegrityInParam, AccountingIntegrityResult myAccountingIntegrityResult)
        {



            try
            {


                var sw = Stopwatch.StartNew();
                string ExceptionMessage = "";
                int badRows = 0;

                try
                {
                    var gLAccountBalanceInterestReportCheck = new GLAccountBalanceInterestReportCheck(accountingIntegrityInParam.Tenant);
                    gLAccountBalanceInterestReportCheck.CheckDbIntegrity();


                    myAccountingIntegrityResult.InterestReportResult = myAccountingIntegrityResult.InterestReportResult ?? new List<InterestReportDiff>();
                    myAccountingIntegrityResult.InterestReportResult.AddRange(gLAccountBalanceInterestReportCheck.CompareReport.InterestReportDiffList);
                    badRows = gLAccountBalanceInterestReportCheck.CompareReport.InterestReportDiffList.Count();

                    IAccountingContext context = AccountingContext.GetContext(accountingIntegrityInParam.Tenant);
                    GLAccountQueryService gLAccountQueryService = new GLAccountQueryService(context);
                    if (myAccountingIntegrityResult?.TotalOpenReconciliationResult?.Count() > 0)
                    {
                        myAccountingIntegrityResult.TotalOpenReconciliationResult.ForEach(r =>
                        {
                            if (!String.IsNullOrEmpty(r.AccountId))
                            {
                                GLAccountPM gLAccountPM = gLAccountQueryService.GetSingle(r.AccountId, false, true);
                                if (gLAccountPM != null)
                                {
                                    r.DisplayNumber = gLAccountPM.DisplayNumber;
                                    r.LocalName = gLAccountPM.LocalName;
                                }
                            }
                        });
                    }

                }
                catch (Exception ee)
                {
                    ExceptionMessage = ee.ToString();
                    //throw;
                }
                finally
                {

                    myAccountingIntegrityResult.MyAccountingIntegrityStep = myAccountingIntegrityResult.MyAccountingIntegrityStep ?? new List<AccountingIntegrityStep>();
                    myAccountingIntegrityResult.MyAccountingIntegrityStep.Add(new AccountingIntegrityStep()
                    {
                        Name = System.Reflection.MethodBase.GetCurrentMethod().Name,
                        //Month = currentMonth,
                        ExceptionMessage = ExceptionMessage,
                        BadRows = badRows,
                        ShouldFix = (badRows > 0 && String.IsNullOrWhiteSpace(ExceptionMessage)),
                        ElapsedMilliseconds = sw.ElapsedMilliseconds,
                    });
                }



            }


            catch (Exception e)
            {

                //throw;
            }

        }



        private void DueLocalBalanceCheck(AccountingIntegrityInParam accountingIntegrityInParam, AccountingIntegrityResult myAccountingIntegrityResult)
        {



            int year = accountingIntegrityInParam.FromMonthInclusive.Year;
            var icurrentMonth = accountingIntegrityInParam.FromMonthInclusive.Month;



            var sw = Stopwatch.StartNew();
            string ExceptionMessage = "";
            int badRows = 0;

            try
            {
                var myDueLocalBalanceService = new DueLocalBalanceService();
                var listDiff = myDueLocalBalanceService.ReverseEngineer(accountingIntegrityInParam.Tenant, null);

                myAccountingIntegrityResult.DueLocalBalance = myAccountingIntegrityResult.DueLocalBalance ?? new List<DueLocalBalanceDiffM>();
                myAccountingIntegrityResult.DueLocalBalance.AddRange(listDiff);
                IAccountingContext context = AccountingContext.GetContext(accountingIntegrityInParam.Tenant);
                GLAccountQueryService gLAccountQueryService = new GLAccountQueryService(context);
                if (myAccountingIntegrityResult.DueLocalBalance.Count > 0)
                {
                    myAccountingIntegrityResult.DueLocalBalance.ForEach(r =>
                    {
                        if (!String.IsNullOrEmpty(r.AccountId))
                        {
                            GLAccountPM gLAccountPM = gLAccountQueryService.GetSingle(r.AccountId, false, true);
                            if (gLAccountPM != null)
                            {
                                r.DisplayNumber = gLAccountPM.DisplayNumber;
                                r.LocalName = gLAccountPM.LocalName;
                            }
                        }
                    });
                }
                badRows = listDiff.Count();
            }
            catch (Exception ee)
            {
                ExceptionMessage = ee.ToString();
                //throw;
            }
            finally
            {

                myAccountingIntegrityResult.MyAccountingIntegrityStep = myAccountingIntegrityResult.MyAccountingIntegrityStep ?? new List<AccountingIntegrityStep>();
                myAccountingIntegrityResult.MyAccountingIntegrityStep.Add(new AccountingIntegrityStep()
                {
                    Name = System.Reflection.MethodBase.GetCurrentMethod().Name,
                    //Month = currentMonth,
                    ExceptionMessage = ExceptionMessage,
                    BadRows = badRows,
                    ShouldFix = (badRows > 0 && String.IsNullOrWhiteSpace(ExceptionMessage)),
                    ElapsedMilliseconds = sw.ElapsedMilliseconds,
                });
            }




        }

        public void GLAccountBalanceCheck(AccountingIntegrityInParam accountingIntegrityInParam, AccountingIntegrityResult myAccountingIntegrityResult)
        {


            try
            {


                var sw = Stopwatch.StartNew();
                string ExceptionMessage = "";
                int badRows = 0;

                try
                {
                    var ReverseEngineerGLAccountBalance = new ReverseEngineerGLAccountBalance(/*currentMonth, */accountingIntegrityInParam.Tenant);
                    ReverseEngineerGLAccountBalance.CheckDbIntegrity();


                    myAccountingIntegrityResult.BalanceInLocalCurrencyResult = myAccountingIntegrityResult.BalanceInLocalCurrencyResult ?? new List<GLAccountBalanceDTO>();
                    myAccountingIntegrityResult.BalanceInLocalCurrencyResult.AddRange(ReverseEngineerGLAccountBalance.CompareReport.GLAccountBalanceList);
                    badRows = ReverseEngineerGLAccountBalance.CompareReport.GLAccountBalanceList.Count();
                    myAccountingIntegrityResult.TotalOpenReconciliationResult = new List<GLAccountBalanceDTO>();
                    myAccountingIntegrityResult.TotalOpenReconciliationResult.AddRange(ReverseEngineerGLAccountBalance.CompareReport.TotalOpenReconciliation);

                    var reverseEngineerControlAccountAccumulateChild = new ReverseEngineerControlAccountAccumulateChild(accountingIntegrityInParam.Tenant);
                    reverseEngineerControlAccountAccumulateChild.CheckDbIntegrity();
                    myAccountingIntegrityResult.TotalOpenReconciliationResult.AddRange(reverseEngineerControlAccountAccumulateChild.CompareReport.GLAccountBalanceList);


                    var ReverseEngineerCashBook = new ReverseEngineerCashBook(accountingIntegrityInParam.Tenant);
                    ReverseEngineerCashBook.CheckDbIntegrity();
                    myAccountingIntegrityResult.TotalOpenReconciliationResult.AddRange(ReverseEngineerCashBook.CompareReport.GLAccountBalanceList);
                    IAccountingContext context = AccountingContext.GetContext(accountingIntegrityInParam.Tenant);
                    GLAccountQueryService gLAccountQueryService = new GLAccountQueryService(context);
                    if (myAccountingIntegrityResult.TotalOpenReconciliationResult.Count > 0)
                    {
                        myAccountingIntegrityResult.TotalOpenReconciliationResult.ForEach(r =>
                        {
                            if (!String.IsNullOrEmpty(r.AccountId))
                            {
                                GLAccountPM gLAccountPM = gLAccountQueryService.GetSingle(r.AccountId, false, true);
                                if (gLAccountPM != null)
                                {
                                    r.DisplayNumber = gLAccountPM.DisplayNumber;
                                    r.LocalName = gLAccountPM.LocalName;
                                }
                            }
                        });
                    }

                }
                catch (Exception ee)
                {
                    ExceptionMessage = ee.ToString();
                    //throw;
                }
                finally
                {

                    myAccountingIntegrityResult.MyAccountingIntegrityStep = myAccountingIntegrityResult.MyAccountingIntegrityStep ?? new List<AccountingIntegrityStep>();
                    myAccountingIntegrityResult.MyAccountingIntegrityStep.Add(new AccountingIntegrityStep()
                    {
                        Name = System.Reflection.MethodBase.GetCurrentMethod().Name,
                        //Month = currentMonth,
                        ExceptionMessage = ExceptionMessage,
                        BadRows = badRows,
                        ShouldFix = (badRows > 0 && String.IsNullOrWhiteSpace(ExceptionMessage)),
                        ElapsedMilliseconds = sw.ElapsedMilliseconds,
                    });
                }



            }


            catch (Exception e)
            {

                //throw;
            }
        }

        private void LedgerToMonthTotalCheck(AccountingIntegrityInParam accountingIntegrityInParam, AccountingIntegrityResult myAccountingIntegrityResult)
        {
            try
            {
                int year = accountingIntegrityInParam.FromMonthInclusive.Year;
                var icurrentMonth = accountingIntegrityInParam.FromMonthInclusive.Month;
                do
                {

                    var sw = Stopwatch.StartNew();
                    string ExceptionMessage = "";
                    int badRows = 0;
                    DateTime currentMonth = new DateTime(accountingIntegrityInParam.FromMonthInclusive.Year, icurrentMonth, 1);
                    try
                    {
                        var reverseEngineerTotalByMonthService = new ReverseEngineerTotalByMonthService(currentMonth, accountingIntegrityInParam.Tenant, null);
                        reverseEngineerTotalByMonthService.CheckDbIntegrity();
                        myAccountingIntegrityResult.LedgerToMounthTotalResult = myAccountingIntegrityResult.LedgerToMounthTotalResult ?? new List<GLAccountTotalByMonthsDTO>();
                        var res = reverseEngineerTotalByMonthService.CompareReport.GLAccountTotalByMonthsList;
                        if (res.Any(r => r.ForeignAmountCredit != 0)
                   || res.Any(r => r.ForeignAmountDebit != 0)
                   || res.Any(r => r.LocalAmountCredit != 0)
                   || res.Any(r => r.LocalAmountDebit != 0)
                   )
                        {
                            myAccountingIntegrityResult.LedgerToMounthTotalResult.AddRange(reverseEngineerTotalByMonthService.CompareReport.GLAccountTotalByMonthsList);
                        }
                            
                            
                        
                        IAccountingContext context = AccountingContext.GetContext(accountingIntegrityInParam.Tenant);
                        GLAccountQueryService gLAccountQueryService = new GLAccountQueryService(context);
                        if (myAccountingIntegrityResult.LedgerToMounthTotalResult.Count > 0)
                        {
                            myAccountingIntegrityResult.LedgerToMounthTotalResult.ForEach(r =>
                            {
                                if (!String.IsNullOrEmpty(r.AccountId))
                                {
                                    GLAccountPM gLAccountPM = gLAccountQueryService.GetSingle(r.AccountId, false, true);
                                    if (gLAccountPM != null)
                                    {
                                        r.DisplayNumber = gLAccountPM.DisplayNumber; 
                                        r.LocalName = gLAccountPM.LocalName;
                                    }
                                }
                            });
                        }
                        badRows = //reverseEngineerTotalByMonthService.CompareReport.GLAccountTotalByMonthsList.Count();
                            myAccountingIntegrityResult.LedgerToMounthTotalResult.Count();
                    }
                    catch (Exception ee)
                    {
                        ExceptionMessage = ee.ToString();
                        //throw;
                    }
                    finally
                    {

                        myAccountingIntegrityResult.MyAccountingIntegrityStep = myAccountingIntegrityResult.MyAccountingIntegrityStep ?? new List<AccountingIntegrityStep>();
                        myAccountingIntegrityResult.MyAccountingIntegrityStep.Add(new AccountingIntegrityStep()
                        {
                            Name = System.Reflection.MethodBase.GetCurrentMethod().Name,
                            Month = currentMonth,
                            ExceptionMessage = ExceptionMessage,
                            BadRows = badRows,
                            ShouldFix = (badRows > 0 && String.IsNullOrWhiteSpace(ExceptionMessage)),
                            ElapsedMilliseconds = sw.ElapsedMilliseconds,
                        });
                    }


                    icurrentMonth++;

                } while (icurrentMonth <= accountingIntegrityInParam.ToMonthInclusive.Month);
            }


            catch (Exception e)
            {

                //throw;
            }
        }

        private static void JournalLineToLedgerCheck(AccountingIntegrityInParam accountingIntegrityInParam, AccountingIntegrityResult myAccountingIntegrityResult)
        {
            try
            {
                int year = accountingIntegrityInParam.FromMonthInclusive.Year;
                var icurrentMonth = accountingIntegrityInParam.FromMonthInclusive.Month;
                do
                {

                    var sw = Stopwatch.StartNew();
                    string ExceptionMessage = "";
                    int badRows = 0;
                    DateTime currentMonth = new DateTime(accountingIntegrityInParam.FromMonthInclusive.Year, icurrentMonth, 1);
                    try
                    {
                        var reverseEngineerLedgerTransactionService = new ReverseEngineerLedgerTransactionService(currentMonth, accountingIntegrityInParam.Tenant);
                        reverseEngineerLedgerTransactionService.CheckDbIntegrity();
                        myAccountingIntegrityResult.JournalLineToLedgerResult = myAccountingIntegrityResult.JournalLineToLedgerResult ?? new List<JournalLineLedgerDTO>();
                        myAccountingIntegrityResult.JournalLineToLedgerResult.AddRange(reverseEngineerLedgerTransactionService.CompareReport.rows);
                        IAccountingContext context = AccountingContext.GetContext(accountingIntegrityInParam.Tenant);
                        JournalQueryService journalQueryService = new JournalQueryService(context);
                        if (myAccountingIntegrityResult.JournalLineToLedgerResult.Count > 0)
                        {
                            myAccountingIntegrityResult.JournalLineToLedgerResult.ForEach(r => 
                            { 
                                if (!String.IsNullOrEmpty(r.JournalId))
                                {
                                    r.JournalNumber = journalQueryService.GetSingle(r.JournalId, false, true).JournalNumber;
                                }
                            });
                        }
                        badRows = reverseEngineerLedgerTransactionService.CompareReport.rows.Count();
                    }
                    catch (Exception ee)
                    {
                        ExceptionMessage = ee.ToString();
                        //throw;
                    }
                    finally
                    {

                        myAccountingIntegrityResult.MyAccountingIntegrityStep = myAccountingIntegrityResult.MyAccountingIntegrityStep ?? new List<AccountingIntegrityStep>();
                        myAccountingIntegrityResult.MyAccountingIntegrityStep.Add(new AccountingIntegrityStep()
                        {
                            Name = System.Reflection.MethodBase.GetCurrentMethod().Name,
                            Month = currentMonth,
                            ExceptionMessage = ExceptionMessage,
                            BadRows = badRows,
                            ShouldFix = false /*JournalLineToLedgerCheck can not fix */,   //(badRows > 0 && String.IsNullOrWhiteSpace(ExceptionMessage)),
                            ElapsedMilliseconds = sw.ElapsedMilliseconds,
                        });
                    }


                    icurrentMonth++;

                } while (icurrentMonth <= accountingIntegrityInParam.ToMonthInclusive.Month);
            }


            catch (Exception e)
            {

                //throw;
            }
        }


        public BatchTaskExecutionPM FixEntegrityCheckErrorInBatch(string id, int tenant)
        {
            string xmlParameters = SerializeXMLParameters(id, tenant);

            CreateBatchTaskExecution(xmlParameters, tenant);
            BatchTaskExecutionPM taskExecution = CreateBatchTaskExecution(xmlParameters, tenant);
            SendBatchTaskToQueue(taskExecution);
            return taskExecution;
        }

        public string SerializeXMLParameters(string id, int tenant)
        {
            IntegrityCheckArgs args = new IntegrityCheckArgs() { EntityId = id, Tenant = tenant };
            var stringwriter = new System.IO.StringWriter();
            var serializer = new XmlSerializer(typeof(IntegrityCheckArgs));
            serializer.Serialize(stringwriter, args);
            string xmlParameters = stringwriter.ToString();
            return xmlParameters;
        }

        public BatchTaskExecutionPM CreateBatchTaskExecution(string xmlParameter, int tenant)
        {
            BatchTaskExecutionPM taskExe = null;
            taskExe = new BatchTaskExecutionPM()
            {
                Subject = "Fix Integrity Check Errors",
                Tenant = tenant,
                ChangeSetOp = ChangeSetOperation.Insert,
                ClassName = "Logitude.Accounting.BL.CoreBL.Batch.BatchFixIntegrityCheckErrorsService,Logitude.Accounting.BL",
                CreateDate = DateTime.Now,
                PrametersXml = xmlParameter,
                StatusCode = "C",

            };

            IInfrastructureContext MyContext = InfrastructureContext.GetContext(tenant);
            BatchTaskExecutionUpdateService bteUpdateService = new BatchTaskExecutionUpdateService(MyContext, new Dictionary<string, IContext>(), tenant);
            bteUpdateService.Update(taskExe, true);
            return taskExe;
        }

        public void SendBatchTaskToQueue(BatchTaskExecutionPM taskExecution)
        {
            IQueueService queueservice = new DbQueueService();
            queueservice.InitializeQueue("batchtaskexecutionqueue", 0);
            queueservice.Send(new Dictionary<string, string>()
                {
                    { "BatchTaskExecutionId", taskExecution.Id },
                    { "Tenant", taskExecution.Tenant.ToString() }
                }, taskExecution.Tenant);

        }

    }
    public class AccountingIntegrityInParam
    {
        public int Tenant { get; set; }
        public DateTime FromMonthInclusive { get; set; }
        public DateTime ToMonthInclusive { get; set; }
    }
    public class AccountingIntegrityResult
    {
        public bool HasException { get; set; }

        public List<AccountingIntegrityStep> MyAccountingIntegrityStep { get; set; }

        public List<JournalLineLedgerDTO> JournalLineToLedgerResult { get; set; }
        public List<GLAccountTotalByMonthsDTO> LedgerToMounthTotalResult { get; set; }
        public List<GLAccountBalanceDTO> BalanceInLocalCurrencyResult { get; set; }
        public List<DueLocalBalanceDiffM> DueLocalBalance { get; set; }
        public bool ShouldFix { get; set; }
        public List<LedgerOpenAmountRecoDiffM> LedgerOpenAmount { get; set; }
        public List<GLAccountBalanceDTO> TotalOpenReconciliationResult { get; set; }
        public List<InterestReportDiff> InterestReportResult { get; set; }
    }

    public class AccountingIntegrityStep
    {
        public string Name { get; set; }
        public DateTime? Month { get; set; }
        public string ExceptionMessage { get; set; }
        public bool ShouldFix { get; set; }
        public int BadRows { get; set; }
        public long ElapsedMilliseconds { get; set; }
    }
}
