using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data.Repositories;
using Logitude.Server.Tools.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                return "FullAccountingSetting is null for tenant " + accountingIntegrityInParam.Tenant;
            }
            if (accountingIntegrityInParam.FromMonthInclusive.Year != accountingIntegrityInParam.ToMonthInclusive.Year)
            {
                return "must the same year";
            }
            if (accountingIntegrityInParam.FromMonthInclusive.Month > accountingIntegrityInParam.ToMonthInclusive.Month)
            {
                return "Bad months";
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
            JournalLineToLedgerCheck(accountingIntegrityInParam, myAccountingIntegrityResult);
            LedgerToMonthTotalCheck(accountingIntegrityInParam, myAccountingIntegrityResult);

            GLAccountBalanceCheck(accountingIntegrityInParam, myAccountingIntegrityResult);

            DueLocalBalanceCheck(accountingIntegrityInParam, myAccountingIntegrityResult);
            return myAccountingIntegrityResult;
        }



        public void FixDBIntegrity(int tenant, List<AccountingIntegrityStep> MyAccountingIntegrityStep)
        {

            var sb = new StringBuilder();
            try
            {

                sb.Append($"FixDBIntegrity({tenant}) start at").AppendLine(DateTime.Now.ToString());


                var JournalLineToLedgerStepList = MyAccountingIntegrityStep.Where(r => r.Name == "JournalLineToLedgerCheck");
                var JournalLineToLedgerStep = JournalLineToLedgerStepList.First(r => r.BadRows > 0);
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

                        sb.AppendLine(ee.ToString());
                    }
                }
            }
            catch (Exception eee)
            {

                sb.AppendLine(eee.ToString());
            }
            finally
            {
                Logger.LogMe(sb.ToString(), false, "AccountingIntegrityService");
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
                    ExcetionMessage = ExceptionMessage,
                    BadRows = badRows,
                    ElapsedMilliseconds = sw.ElapsedMilliseconds,
                });
            }




        }

        private void GLAccountBalanceCheck(AccountingIntegrityInParam accountingIntegrityInParam, AccountingIntegrityResult myAccountingIntegrityResult)
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
                        ExcetionMessage = ExceptionMessage,
                        BadRows = badRows,
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
                        myAccountingIntegrityResult.LedgerToMounthTotalResult.AddRange(reverseEngineerTotalByMonthService.CompareReport.GLAccountTotalByMonthsList);
                        badRows = reverseEngineerTotalByMonthService.CompareReport.GLAccountTotalByMonthsList.Count();
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
                            ExcetionMessage = ExceptionMessage,
                            BadRows = badRows,
                            ElapsedMilliseconds = sw.ElapsedMilliseconds,
                        });
                    }


                    icurrentMonth++;

                } while (icurrentMonth < accountingIntegrityInParam.ToMonthInclusive.Month);
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
                            ExcetionMessage = ExceptionMessage,
                            BadRows = badRows,
                            ElapsedMilliseconds = sw.ElapsedMilliseconds,
                        });
                    }


                    icurrentMonth++;

                } while (icurrentMonth < accountingIntegrityInParam.ToMonthInclusive.Month);
            }


            catch (Exception e)
            {

                //throw;
            }
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
        public List<AccountingIntegrityStep> MyAccountingIntegrityStep { get; set; }

        public List<JournalLineLedgerDTO> JournalLineToLedgerResult { get; set; }
        public List<GLAccountTotalByMonthsDTO> LedgerToMounthTotalResult { get; set; }
        public List<GLAccountBalanceDTO> BalanceInLocalCurrencyResult { get; set; }
        public List<DueLocalBalanceDiffM> DueLocalBalance { get; set; }

    }

    public class AccountingIntegrityStep
    {
        public string Name { get; set; }
        public DateTime? Month { get; set; }
        public string ExcetionMessage { get; set; }
        public int BadRows { get; set; }
        public long ElapsedMilliseconds { get; internal set; }
    }
}
