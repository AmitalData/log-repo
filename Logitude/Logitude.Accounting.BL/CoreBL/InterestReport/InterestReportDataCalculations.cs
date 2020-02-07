using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Logitude.Accounting.BL.CoreBL.InterestReport
{
    public class InterestReportDataCalculations
    {
        private InterestReportPM interestReportPM;
        private List<InterestTransactionPM> interestTransactionPMs;
        private List<InterestTransactionsGroupedByDate> interestTransactionsGroupedByDates;
        private string interestReportId;
        private int tenant;
        IInterestReportCalculateionPreparations interestReportCalculationPreparations;
        public InterestReportDataCalculations(InterestReportArgs interestReportArgs)
        {
            interestReportId = interestReportArgs.InterestReportId;
            tenant = interestReportArgs.Tenant;
            interestReportCalculationPreparations = interestReportArgs.CalculationPreparations!=null?interestReportArgs.CalculationPreparations: new InterestReportCalculationPreparations();
        }

        public void StartCalculations()
        {
            GetInterestReportAndInterestTransactionsForCalculations();
            using(TransactionScope scope= TransactionFactory.GetNewTransaction())
            {
                CreateInterestReportLines();
                interestReportPM.OpenBalance = GetInterestReportOpenBalance();
                List<InterestReportLinesByDatePM> interestReportLinesByDatePMs = CreateInterestReportLinesByDate();
                interestReportPM.CloseBalance = GetInterestReportCloseBalance(interestReportLinesByDatePMs);
                SubmitInterestReportLinesByDate(interestReportLinesByDatePMs);
                SubmitChangesToInterestReport();
                scope.Complete();
            }
        }

        private void SubmitChangesToInterestReport()
        {
            interestReportPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            IAccountingContext accountingContext = AccountingContext.GetContext(tenant);
            InterestReportUpdateService interestReportUpdateService = new InterestReportUpdateService(accountingContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), tenant);
            interestReportUpdateService.Update(interestReportPM, true);
        }

        private decimal? GetInterestReportCloseBalance(List<InterestReportLinesByDatePM> interestReportLinesByDatePMs)
        {
            decimal closeBalance = (from a in interestReportLinesByDatePMs
                                    orderby a.FromDate descending
                                    select a.AccumulatedAmount).FirstOrDefault();
            return closeBalance;
        }

        private decimal? GetInterestReportOpenBalance()
        {
            InterestReportQueryService interestReportQueryService = new InterestReportQueryService(tenant);
            decimal interestReportOpenBalance = interestReportQueryService.GetClosedBalanceOfLastInvoicedOrClosedWithoutInvoiceInterestReport(tenant);
            return interestReportOpenBalance;
        }

        private void SubmitInterestReportLinesByDate(List<InterestReportLinesByDatePM> interestReportLinesByDatePMs)
        {
            IAccountingContext accountingContext = AccountingContext.GetContext(tenant);
            for(int i = 0; i < interestReportLinesByDatePMs.Count; i++)
            {
                InterestReportLinesByDateUpdateService interestReportLinesByDateUpdateService = new InterestReportLinesByDateUpdateService(accountingContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), tenant);
                interestReportLinesByDateUpdateService.Update(interestReportLinesByDatePMs[i], true);
            }
        }

        private void CreateInterestReportLines()
        {
            InterestReportLinesCreationService interestReportLinesCreationService = new InterestReportLinesCreationService();
            interestReportLinesCreationService.CreateInterestReportLines(interestTransactionPMs, interestReportId, tenant);
        }

        public List<InterestReportLinesByDatePM> CreateInterestReportLinesByDate()
        {
            List<InterestReportLinesByDatePM> interestReportLinesByDatePMs = new List<InterestReportLinesByDatePM>();
            int sequence = 1;
            decimal accumulatedAmount = interestReportPM.OpenBalance??interestReportPM.OpenBalance.Value;
            for(int i = 0; i < interestTransactionsGroupedByDates.Count; i++)
            {
                
                InterestTransactionsGroupedByDate currentInterestTransactionGroupedByDate = interestTransactionsGroupedByDates[i];
                InterestTransactionsGroupedByDate nextInterestTransactionGroupedByDate = null;
               
                if (i != interestTransactionsGroupedByDates.Count - 1) {
                     nextInterestTransactionGroupedByDate = interestTransactionsGroupedByDates[i + 1];
                }
                
                InterestReportLinesByDatePM interestReportLinesByDatePM = new InterestReportLinesByDatePM();
                interestReportLinesByDatePM.LineNumber = sequence++;
                interestReportLinesByDatePM.InterestReportId = interestReportId;
                interestReportLinesByDatePM.Tenant = tenant;
                interestReportLinesByDatePM.FromDate = currentInterestTransactionGroupedByDate.GroupInterestValueDate;
                interestReportLinesByDatePM.ToDate = nextInterestTransactionGroupedByDate != null ? nextInterestTransactionGroupedByDate.GroupInterestValueDate : interestReportPM.InterestCalculationDate;
                double doubleTotalInterestDays = (interestReportLinesByDatePM.ToDate - interestReportLinesByDatePM.FromDate).TotalDays;
                interestReportLinesByDatePM.TotalInterestDays = Convert.ToInt32(doubleTotalInterestDays);
                interestReportLinesByDatePM.TotalAmount = currentInterestTransactionGroupedByDate.TotalLocalAmount;
                interestReportLinesByDatePM.AccumulatedAmount = accumulatedAmount + currentInterestTransactionGroupedByDate.TotalLocalAmount;
                accumulatedAmount = interestReportLinesByDatePM.AccumulatedAmount;
                interestReportLinesByDatePM.StandardInterestPercentage = GetStandardInterestPercentageForStartInterestDate(interestReportLinesByDatePM.FromDate);
                interestReportLinesByDatePM.ExceptionalInterestPercentage = GetExceptionalInterestPercentageForStartInterestDate(interestReportLinesByDatePM.FromDate);
                interestReportLinesByDatePM.CreditInterestPercentage = GetCreditInterestPercentageForStartInterestDate(interestReportLinesByDatePM.FromDate);
                interestReportLinesByDatePM.StandardInterestAmount = GetStandardInterestAmount(interestReportLinesByDatePM);
                interestReportLinesByDatePM.ExceptionalInterestAmount= GetExceptionalInterestAmount(interestReportLinesByDatePM);
                interestReportLinesByDatePM.CreditInterestAmount= GetCreditInterestAmount(interestReportLinesByDatePM);
                InterestCalculationDetails standardInterestCalculationDetails = GetStandardInterestAmountCalculationDetails(interestReportLinesByDatePM);
                InterestCalculationDetails creditInterestCalculationDetails=  GetCreditInterestAmountCalculationDetails(interestReportLinesByDatePM);
                InterestCalculationDetails exceptionalInterestCalculationDetails= GetExceptionalInterestAmountCalculationDetails(interestReportLinesByDatePM);
                interestReportLinesByDatePM.CalculatedStandInterestAmount = standardInterestCalculationDetails.Value;
                interestReportLinesByDatePM.CalculatedExcepInterestAmount = exceptionalInterestCalculationDetails.Value;
                interestReportLinesByDatePM.CalculatedCreditInterestAmount = creditInterestCalculationDetails.Value;
                List<InterestCalculationDetails> interestCalculationDetails = new List<InterestCalculationDetails>();
                interestCalculationDetails.Add(standardInterestCalculationDetails);
                interestCalculationDetails.Add(creditInterestCalculationDetails);
                interestCalculationDetails.Add(exceptionalInterestCalculationDetails);
                interestReportLinesByDatePM.CalculationDetails = GetCalculationEquations(interestCalculationDetails);
                interestReportLinesByDatePM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
                interestReportLinesByDatePMs.Add(interestReportLinesByDatePM);

            }
            return interestReportLinesByDatePMs;

        }

        private string GetCalculationEquations(List<InterestCalculationDetails> interestCalculationDetails)
        {
            string calculationEquations = "";
            for(int i = 0; i < interestCalculationDetails.Count; i++)
            {
                if (interestCalculationDetails[i].Value!=0)
                {
                    calculationEquations = !string.IsNullOrEmpty(calculationEquations)? calculationEquations + " + " + interestCalculationDetails[i].CalculationEquation: interestCalculationDetails[i].CalculationEquation;
                }
            }
            return calculationEquations;
        }

        private InterestCalculationDetails GetCreditInterestAmountCalculationDetails(InterestReportLinesByDatePM interestReportLinesByDatePM)
        {
            decimal calculatedCreditInterestAmount = 0;
            string calculationEquation = "";
            decimal creditInterestAmount = interestReportLinesByDatePM.CreditInterestAmount;
            decimal creditInterestPercentage = interestReportLinesByDatePM.CreditInterestPercentage;
            int totalInterestDays = interestReportLinesByDatePM.TotalInterestDays;
            calculatedCreditInterestAmount = creditInterestAmount * (creditInterestPercentage / 365) * totalInterestDays;
            calculatedCreditInterestAmount = Math.Round(calculatedCreditInterestAmount, 4);
            calculationEquation = creditInterestAmount.ToString() + " * (" + creditInterestPercentage.ToString() + " / 365) * " + totalInterestDays.ToString();
            InterestCalculationDetails interestCalculationDetails = new InterestCalculationDetails() { CalculationEquation=calculationEquation,Value= calculatedCreditInterestAmount };
            
            return interestCalculationDetails;
        }

        private InterestCalculationDetails GetExceptionalInterestAmountCalculationDetails(InterestReportLinesByDatePM interestReportLinesByDatePM)
        {
            decimal calculatedExceptionalInterestAmount = 0;
            string calculationEquation = "";
            decimal exceptionalInterestAmount = interestReportLinesByDatePM.ExceptionalInterestAmount;
            decimal exceptionalInterestPercentage = interestReportLinesByDatePM.ExceptionalInterestPercentage;
            int totalInterestDays = interestReportLinesByDatePM.TotalInterestDays;
            calculatedExceptionalInterestAmount = exceptionalInterestAmount * (exceptionalInterestPercentage / 365) * totalInterestDays;
            calculatedExceptionalInterestAmount = Math.Round(calculatedExceptionalInterestAmount, 4);
            calculationEquation = exceptionalInterestAmount.ToString() + " * (" + exceptionalInterestPercentage.ToString() + " / 365) * " + totalInterestDays.ToString();
            InterestCalculationDetails interestCalculationDetails = new InterestCalculationDetails() { CalculationEquation = calculationEquation, Value = calculatedExceptionalInterestAmount };

            return interestCalculationDetails;
        }

        private InterestCalculationDetails GetStandardInterestAmountCalculationDetails(InterestReportLinesByDatePM interestReportLinesByDatePM)
        {
            decimal calculatedStandardInterestAmount = 0;
            string calculationEquation = "";
            decimal standardInterestAmount = interestReportLinesByDatePM.StandardInterestAmount;
            decimal standardInterestPercentage = interestReportLinesByDatePM.StandardInterestPercentage;
            int totalInterestDays = interestReportLinesByDatePM.TotalInterestDays;
            calculatedStandardInterestAmount = standardInterestAmount * (standardInterestPercentage / 365) * totalInterestDays;
            calculatedStandardInterestAmount = Math.Round(calculatedStandardInterestAmount, 4);
            calculationEquation = standardInterestAmount.ToString() + " * (" + standardInterestPercentage.ToString() + " / 365) * " + totalInterestDays.ToString();
            InterestCalculationDetails interestCalculationDetails = new InterestCalculationDetails() { CalculationEquation = calculationEquation, Value = calculatedStandardInterestAmount };

            return interestCalculationDetails;
        }

        private decimal GetCreditInterestAmount(InterestReportLinesByDatePM interestReportLinesByDatePM)
        {
            decimal creditInterestAmount = 0;
            decimal accumulatedAmount = interestReportLinesByDatePM.AccumulatedAmount;
            if (accumulatedAmount < 0)
            {
                creditInterestAmount = accumulatedAmount;
            }
            return creditInterestAmount;
        }

        private decimal GetExceptionalInterestAmount(InterestReportLinesByDatePM interestReportLinesByDatePM)
        {
            decimal exceptionalInterestAmount = 0;
            decimal accumulatedAmount = interestReportLinesByDatePM.AccumulatedAmount;
            decimal gLAccountInterestCreditLimit = interestReportPM.GLAccountInterestCreditLimit != null ? interestReportPM.GLAccountInterestCreditLimit.Value : 0;
            if(accumulatedAmount>0 && accumulatedAmount > gLAccountInterestCreditLimit)
            {
                exceptionalInterestAmount = accumulatedAmount - gLAccountInterestCreditLimit;
            }
            return exceptionalInterestAmount;
        }

        private decimal GetStandardInterestAmount(InterestReportLinesByDatePM interestReportLinesByDatePM)
        {
            decimal standardInterestAmount = 0;
            decimal gLAccountInterestCreditLimit = interestReportPM.GLAccountInterestCreditLimit != null ? interestReportPM.GLAccountInterestCreditLimit.Value : 0;
            decimal accumulatedAmount = interestReportLinesByDatePM.AccumulatedAmount;
            if (accumulatedAmount > 0)
            {
                if (accumulatedAmount >= gLAccountInterestCreditLimit)
                {
                    standardInterestAmount = gLAccountInterestCreditLimit;
                }
                else
                {
                    standardInterestAmount = accumulatedAmount;
                }
            }
            return standardInterestAmount;
        }

        public void GetInterestReportAndInterestTransactionsForCalculations()
        {
            
            interestReportPM = interestReportCalculationPreparations.GetInterestReportPM(interestReportId, tenant);
            interestTransactionPMs = interestReportCalculationPreparations.GetInterestTransactionsForGlAccountAndInterestValueDate(interestReportPM.GLAccountId, interestReportPM.InterestCalculationDate,tenant);
            interestTransactionsGroupedByDates = interestReportCalculationPreparations.GetInterestTransactionsGroupedByDate(interestTransactionPMs);
        }

        private List<GLAccountInterestPeriodPM> GetGLAccountInterestPeriodsForInterestCalculationDateOrderedByDateDescending(DateTime interestCalculationDate)
        {
            GLAccountInterestPeriodQueryService gLAccountInterestPeriodQueryService = new GLAccountInterestPeriodQueryService(tenant);
            List<GLAccountInterestPeriodPM> gLAccountInterestPeriodPMs = gLAccountInterestPeriodQueryService.GetLAccountInterestPeriodPMsByInterestDate(interestReportPM.GLAccountId, interestCalculationDate, tenant);
            return gLAccountInterestPeriodPMs.OrderByDescending(d => d.PeriodStartDate).ToList();
        }

        private decimal GetStandardInterestPercentageForStartInterestDate(DateTime startInterestDate)
        {

            GLAccountInterestPeriodPM gLAccountInterestPeriodPM = interestReportCalculationPreparations.GetGLAccountInterestPeriodPMWithinStartInterestDate(interestReportPM, startInterestDate);

            decimal InterestBaseTypeRate =interestReportCalculationPreparations.CalculateInterestBasesTypePercentage(gLAccountInterestPeriodPM.StandardInterestRateBaseId, startInterestDate,tenant);
            decimal standardAdditionalInterestPercentage = gLAccountInterestPeriodPM.StandardAddInterestPercent != null ? gLAccountInterestPeriodPM.StandardAddInterestPercent.Value : 0;
            decimal percentage = InterestBaseTypeRate + standardAdditionalInterestPercentage;
            return percentage;
        }

        private decimal GetExceptionalInterestPercentageForStartInterestDate(DateTime startInterestDate)
        {
            GLAccountInterestPeriodPM gLAccountInterestPeriodPM = interestReportCalculationPreparations.GetGLAccountInterestPeriodPMWithinStartInterestDate(interestReportPM, startInterestDate);

            decimal InterestBaseTypeRate = interestReportCalculationPreparations.CalculateInterestBasesTypePercentage(gLAccountInterestPeriodPM.ExceptionalInterestRateBaseId, startInterestDate, tenant);
            decimal exceptionalAdditionalInterestPercentage = gLAccountInterestPeriodPM.ExceptionalAddInterestPercent != null ? gLAccountInterestPeriodPM.ExceptionalAddInterestPercent.Value : 0;
            decimal percentage = InterestBaseTypeRate + exceptionalAdditionalInterestPercentage;
            return percentage;
        }
        private decimal GetCreditInterestPercentageForStartInterestDate(DateTime startInterestDate)
        {
            GLAccountInterestPeriodPM gLAccountInterestPeriodPM = interestReportCalculationPreparations.GetGLAccountInterestPeriodPMWithinStartInterestDate(interestReportPM, startInterestDate);

            decimal InterestBaseTypeRate = interestReportCalculationPreparations.CalculateInterestBasesTypePercentage(gLAccountInterestPeriodPM.CreditInterestRateBaseId, startInterestDate, tenant);
            decimal creditAdditionalInterestPercentage = gLAccountInterestPeriodPM.CreditAddInterestPercent != null ? gLAccountInterestPeriodPM.CreditAddInterestPercent.Value : 0;
            decimal percentage = InterestBaseTypeRate + creditAdditionalInterestPercentage;
            return percentage;
        }

    }
}
