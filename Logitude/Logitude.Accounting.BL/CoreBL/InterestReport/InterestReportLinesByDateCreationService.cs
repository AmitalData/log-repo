using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Server.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL.InterestReport
{
    public class InterestReportLinesByDateCreationService
    {
        private int tenant;
        public List<InterestReportLinesByDatePM> CreateInterestReportLinesByDate(InterestReportLinesByDateCreationParams interestReportLinesByDateCreationParams)
        {
            List<InterestTransactionsGroupedByDate> interestTransactionsGroupedByDates = GetInterestTransactionsGroupedByDate(interestReportLinesByDateCreationParams.InterestTransactionPMs);
            List<InterestReportLinesByDatePM> interestReportLinesByDatePMs = new List<InterestReportLinesByDatePM>();
            int sequence = 1;
            decimal accumulatedAmount = interestReportLinesByDateCreationParams.InterestReportPM.OpenBalance != null ? interestReportLinesByDateCreationParams.InterestReportPM.OpenBalance.Value : 0;
            decimal OpenBalance = accumulatedAmount;
            for (int i = 0; i < interestTransactionsGroupedByDates.Count; i++)
            {
                InterestTransactionsGroupedByDate currentInterestTransactionGroupedByDate = interestTransactionsGroupedByDates[i];
                InterestTransactionsGroupedByDate nextInterestTransactionGroupedByDate = null;
                if (i != interestTransactionsGroupedByDates.Count - 1)
                {
                    nextInterestTransactionGroupedByDate = interestTransactionsGroupedByDates[i + 1];
                }
                bool isLinehasRecent = CheckRecentCustomerReportForLine(interestReportLinesByDateCreationParams.InterestReportPM,
                                                                        currentInterestTransactionGroupedByDate.GroupInterestValueDate);
                accumulatedAmount =  accumulatedAmount + currentInterestTransactionGroupedByDate.TotalLocalAmount;
                decimal LineAccumulatedAmount = isLinehasRecent ? accumulatedAmount - OpenBalance: accumulatedAmount;
                InterestReportLinesByDateMappingParams interestReportLinesByDateMappingParams = new InterestReportLinesByDateMappingParams(
                    currentInterestTransactionGroupedByDate,
                    nextInterestTransactionGroupedByDate,
                    interestReportLinesByDateCreationParams.InterestReportPM.Id,
                    interestReportLinesByDateCreationParams.InterestReportPM.Tenant,
                    interestReportLinesByDateCreationParams,
                    LineAccumulatedAmount);
                tenant = interestReportLinesByDateCreationParams.InterestReportPM.Tenant;
                InterestReportLinesByDatePM interestReportLinesByDatePM = GetMappedInterestReportLinesByDatePM(interestReportLinesByDateMappingParams);
                interestReportLinesByDatePM.LineNumber = sequence++;
                interestReportLinesByDatePM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
                interestReportLinesByDatePMs.Add(interestReportLinesByDatePM);
            }
            return interestReportLinesByDatePMs;
        }

        private bool CheckRecentCustomerReportForLine(InterestReportPM InterestReportPM, DateTime fromDate)
        {
            InterestReportQueryService ReportQueryService = new InterestReportQueryService(tenant);
            bool hasRecent =  ReportQueryService.CheckRecentCustomerReports(fromDate, InterestReportPM);
            return hasRecent;
        }

        private InterestReportLinesByDatePM GetMappedInterestReportLinesByDatePM(InterestReportLinesByDateMappingParams interestReportLinesByDateMappingParams)
        {
            InterestReportLinesByDatePM interestReportLinesByDatePM = new InterestReportLinesByDatePM();
            InterestReportPM interestReportPM = interestReportLinesByDateMappingParams.InterestReportLinesByDateCreationParams.InterestReportPM;
            interestReportLinesByDatePM.InterestReportId = interestReportLinesByDateMappingParams.InterestReportId;
            interestReportLinesByDatePM.Tenant = interestReportLinesByDateMappingParams.Tenant;
            interestReportLinesByDatePM.FromDate = interestReportLinesByDateMappingParams.CurrentInterestTransactionGroupedByDate.GroupInterestValueDate;

            interestReportLinesByDatePM.ToDate = interestReportLinesByDateMappingParams.NextInterestTransactionGroupedByDate != null ?
                interestReportLinesByDateMappingParams.NextInterestTransactionGroupedByDate.GroupInterestValueDate :
                interestReportLinesByDateMappingParams.InterestReportLinesByDateCreationParams.InterestReportPM.InterestCalculationDate;

            double doubleTotalInterestDays = GetTotalDays(interestReportLinesByDatePM, interestReportLinesByDateMappingParams.NextInterestTransactionGroupedByDate);
            interestReportLinesByDatePM.TotalInterestDays = Convert.ToInt32(doubleTotalInterestDays);
            interestReportLinesByDatePM.TotalAmount = interestReportLinesByDateMappingParams.CurrentInterestTransactionGroupedByDate.TotalLocalAmount;
            interestReportLinesByDatePM.AccumulatedAmount = interestReportLinesByDateMappingParams.AccumulatedAmount;

            InterestPercentageForDateParams interestPercentageForDateParams = new InterestPercentageForDateParams(
                interestReportLinesByDateMappingParams.InterestReportLinesByDateCreationParams,
                interestReportLinesByDatePM.ToDate);

            interestReportLinesByDatePM.StandardInterestPercentage = GetStandardInterestPercentageForStartInterestDate(interestPercentageForDateParams);
            interestReportLinesByDatePM.ExceptionalInterestPercentage = GetExceptionalInterestPercentageForStartInterestDate(interestPercentageForDateParams);
            interestReportLinesByDatePM.CreditInterestPercentage = GetCreditInterestPercentageForStartInterestDate(interestPercentageForDateParams);

            interestReportLinesByDatePM.StandardInterestAmount = GetStandardInterestAmount(interestReportLinesByDatePM, interestReportPM);
            interestReportLinesByDatePM.ExceptionalInterestAmount = GetExceptionalInterestAmount(interestReportLinesByDatePM, interestReportPM);
            interestReportLinesByDatePM.CreditInterestAmount = GetCreditInterestAmount(interestReportLinesByDatePM);

            InterestCalculationDetails standardInterestCalculationDetails = GetStandardInterestAmountCalculationDetails(interestReportLinesByDatePM);
            InterestCalculationDetails creditInterestCalculationDetails = GetCreditInterestAmountCalculationDetails(interestReportLinesByDatePM);
            InterestCalculationDetails exceptionalInterestCalculationDetails = GetExceptionalInterestAmountCalculationDetails(interestReportLinesByDatePM);

            interestReportLinesByDatePM.CalculatedStandInterestAmount = standardInterestCalculationDetails.Value;
            interestReportLinesByDatePM.CalculatedExcepInterestAmount = exceptionalInterestCalculationDetails.Value;
            interestReportLinesByDatePM.CalculatedCreditInterestAmount = creditInterestCalculationDetails.Value;

            interestReportLinesByDatePM.IsOpenBalanceLine= interestReportLinesByDateMappingParams.CurrentInterestTransactionGroupedByDate.IsOpenBalanceLine;

            List<InterestCalculationDetails> interestCalculationDetails = new List<InterestCalculationDetails>();
            interestCalculationDetails.Add(standardInterestCalculationDetails);
            interestCalculationDetails.Add(creditInterestCalculationDetails);
            interestCalculationDetails.Add(exceptionalInterestCalculationDetails);

            interestReportLinesByDatePM.CalculationDetails = GetCalculationEquations(interestCalculationDetails);
            return interestReportLinesByDatePM;
        }

        private double GetTotalDays(InterestReportLinesByDatePM interestReportLinesByDatePM, InterestTransactionsGroupedByDate nextInterestTransactionGroupedByDate)
        {
            double totalInterestDays = (interestReportLinesByDatePM.ToDate - interestReportLinesByDatePM.FromDate).TotalDays;
            if (nextInterestTransactionGroupedByDate == null)
            {
                totalInterestDays = totalInterestDays + 1;
            }

            return totalInterestDays;
        }

        private string GetCalculationEquations(List<InterestCalculationDetails> interestCalculationDetails)
        {
            string calculationEquations = "";
            for (int i = 0; i < interestCalculationDetails.Count; i++)
            {
                if (interestCalculationDetails[i].Value != 0)
                {
                    calculationEquations = !string.IsNullOrEmpty(calculationEquations) ? calculationEquations + " + " + interestCalculationDetails[i].CalculationEquation : interestCalculationDetails[i].CalculationEquation;
                }
            }
            return calculationEquations;
        }

        private InterestCalculationDetails GetCreditInterestAmountCalculationDetails(InterestReportLinesByDatePM interestReportLinesByDatePM)
        {
            decimal calculatedCreditInterestAmount = 0;
            string calculationEquation = "";
            decimal creditInterestAmount = interestReportLinesByDatePM.CreditInterestAmount;
            decimal creditInterestPercentage = interestReportLinesByDatePM.CreditInterestPercentage / 100;
            int totalInterestDays = interestReportLinesByDatePM.TotalInterestDays;
            calculatedCreditInterestAmount = creditInterestAmount * (creditInterestPercentage / 365) * totalInterestDays;
            calculatedCreditInterestAmount = Math.Round(calculatedCreditInterestAmount, 4);
            calculationEquation = creditInterestAmount.ToString() + " * (" + creditInterestPercentage.ToString() + " / 365) * " + totalInterestDays.ToString();
            InterestCalculationDetails interestCalculationDetails = new InterestCalculationDetails() { CalculationEquation = calculationEquation, Value = calculatedCreditInterestAmount };

            return interestCalculationDetails;
        }

        private InterestCalculationDetails GetExceptionalInterestAmountCalculationDetails(InterestReportLinesByDatePM interestReportLinesByDatePM)
        {
            decimal calculatedExceptionalInterestAmount = 0;
            string calculationEquation = "";
            decimal exceptionalInterestAmount = interestReportLinesByDatePM.ExceptionalInterestAmount;
            decimal exceptionalInterestPercentage = interestReportLinesByDatePM.ExceptionalInterestPercentage / 100;
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
            decimal standardInterestPercentage = interestReportLinesByDatePM.StandardInterestPercentage / 100;
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

        private decimal GetExceptionalInterestAmount(InterestReportLinesByDatePM interestReportLinesByDatePM, InterestReportPM interestReportPM)
        {
            decimal exceptionalInterestAmount = 0;
            decimal accumulatedAmount = interestReportLinesByDatePM.AccumulatedAmount;
            decimal gLAccountInterestCreditLimit = interestReportPM.GLAccountInterestCreditLimit != null ? interestReportPM.GLAccountInterestCreditLimit.Value : 0;
            if (accumulatedAmount > 0 && accumulatedAmount > gLAccountInterestCreditLimit)
            {
                exceptionalInterestAmount = accumulatedAmount - gLAccountInterestCreditLimit;
            }
            return exceptionalInterestAmount;
        }
        private decimal GetStandardInterestAmount(InterestReportLinesByDatePM interestReportLinesByDatePM, InterestReportPM interestReportPM)
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

        private decimal GetCreditInterestPercentageForStartInterestDate(InterestPercentageForDateParams interestPercentageForDateParams)
        {
            GLAccountInterestPeriodPM gLAccountInterestPeriodPM = GetGLAccountInterestPeriodPMByPeriodToDate(interestPercentageForDateParams);
            InterestBasesPeriodPM Period = GetInterestBasesPeriodPMFromParamsPeriods(interestPercentageForDateParams, gLAccountInterestPeriodPM.CreditInterestRateBaseId, "credit");
            //InterestBasesPeriodPM Period = (from a in interestPercentageForDateParams.InterestReportLinesByDateCreationParams.InterestBasesPeriodPMs
            //                                where a.InterestBaseStartDate <= interestPercentageForDateParams.ToDate
            //                                && a.InterestBaseTypeId == gLAccountInterestPeriodPM.CreditInterestRateBaseId
            //                                && a.Tenant == interestPercentageForDateParams.InterestReportLinesByDateCreationParams.InterestReportPM.Tenant
            //                                select a).OrderByDescending(d => d.InterestBaseStartDate).FirstOrDefault();

            decimal creditAdditionalInterestPercentage = gLAccountInterestPeriodPM.CreditAddInterestPercent != null ? gLAccountInterestPeriodPM.CreditAddInterestPercent.Value : 0;
            decimal percentage = (Period.InterestRate + creditAdditionalInterestPercentage);
            return percentage;
        }

        private decimal GetExceptionalInterestPercentageForStartInterestDate(InterestPercentageForDateParams interestPercentageForDateParams)
        {
            GLAccountInterestPeriodPM gLAccountInterestPeriodPM = GetGLAccountInterestPeriodPMByPeriodToDate(interestPercentageForDateParams);

            InterestBasesPeriodPM Period = GetInterestBasesPeriodPMFromParamsPeriods(interestPercentageForDateParams, gLAccountInterestPeriodPM.ExceptionalInterestRateBaseId, "exceptional");
            //InterestBasesPeriodPM Period = (from a in interestPercentageForDateParams.InterestReportLinesByDateCreationParams.InterestBasesPeriodPMs
            //                                where a.InterestBaseStartDate <= interestPercentageForDateParams.ToDate
            //                                && a.InterestBaseTypeId == gLAccountInterestPeriodPM.ExceptionalInterestRateBaseId
            //                                && a.Tenant == interestPercentageForDateParams.InterestReportLinesByDateCreationParams.InterestReportPM.Tenant
            //                                select a).OrderByDescending(d => d.InterestBaseStartDate).FirstOrDefault();

            decimal exceptionalAdditionalInterestPercentage = gLAccountInterestPeriodPM.ExceptionalAddInterestPercent != null ? gLAccountInterestPeriodPM.ExceptionalAddInterestPercent.Value : 0;
            decimal percentage = Period != null ? (Period.InterestRate + exceptionalAdditionalInterestPercentage) : 0;
            return percentage;
        }

        private decimal GetStandardInterestPercentageForStartInterestDate(InterestPercentageForDateParams interestPercentageForDateParams)
        {
            GLAccountInterestPeriodPM gLAccountInterestPeriodPM = GetGLAccountInterestPeriodPMByPeriodToDate(interestPercentageForDateParams);

            InterestBasesPeriodPM Period = GetInterestBasesPeriodPMFromParamsPeriods(interestPercentageForDateParams, gLAccountInterestPeriodPM.StandardInterestRateBaseId,"standard");
            //InterestBasesPeriodPM Period = (from a in interestPercentageForDateParams.InterestReportLinesByDateCreationParams.InterestBasesPeriodPMs
            //                              where a.InterestBaseStartDate <= interestPercentageForDateParams.ToDate
            //                              && a.InterestBaseTypeId == gLAccountInterestPeriodPM.StandardInterestRateBaseId 
            //                              && a.Tenant == interestPercentageForDateParams.InterestReportLinesByDateCreationParams.InterestReportPM.Tenant
            //                              select a).OrderByDescending(d => d.InterestBaseStartDate).FirstOrDefault();

            decimal standardAdditionalInterestPercentage = gLAccountInterestPeriodPM.StandardAddInterestPercent != null ? gLAccountInterestPeriodPM.StandardAddInterestPercent.Value : 0;
            decimal percentage = (Period.InterestRate + standardAdditionalInterestPercentage);
            return percentage;
        }

        private InterestBasesPeriodPM GetInterestBasesPeriodPMFromParamsPeriods(InterestPercentageForDateParams interestPercentageForDateParams, string interestRateBaseId,string interestRateBaseType)
        {
            InterestBasesPeriodPM Period = (from a in interestPercentageForDateParams.InterestReportLinesByDateCreationParams.InterestBasesPeriodPMs
                                            where a.InterestBaseStartDate <= interestPercentageForDateParams.ToDate
                                            && a.InterestBaseTypeId == interestRateBaseId
                                            && a.Tenant == interestPercentageForDateParams.InterestReportLinesByDateCreationParams.InterestReportPM.Tenant
                                            select a).OrderByDescending(d => d.InterestBaseStartDate).FirstOrDefault();
            if (Period == null)
            {
                ThrowSuitableException(interestRateBaseType);
            }

            return Period;
        }

        private void ThrowSuitableException(string interestRateBaseType)
        {
            switch (interestRateBaseType) {
                case "standard": 
                    {
                        string message = TextCodesTranslator.TranslateText("InterestReport.O.NoStandardBasePeriod", tenant, true);
                        throw new ApplicationException(message);
                        
                    }
                case "exceptional":
                    {
                        string message = TextCodesTranslator.TranslateText("InterestReport.O.NoExceptionalBasePeriod&quot", tenant, true);
                        throw new ApplicationException(message);
                    }
                case "credit":
                    {
                        string message = TextCodesTranslator.TranslateText("InterestReport.O.NoCreditBasePeriod", tenant, true);
                        throw new ApplicationException(message);
                    }
            }
        }

        public virtual List<InterestTransactionsGroupedByDate> GetInterestTransactionsGroupedByDate(List<InterestTransactionPM> interestTransactionPMs)
        {
            List<InterestTransactionsGroupedByDate> interestTransactionsGroupedByDates = (from interestTransaction in interestTransactionPMs
                                                                                          group interestTransaction by interestTransaction.InterestValueDate.Date into groupByDate
                                                                                          select new InterestTransactionsGroupedByDate()
                                                                                          {
                                                                                              GroupInterestValueDate = groupByDate.Key,
                                                                                              TotalLocalAmount = groupByDate.Sum(d => d.LocalAmount),
                                                                                              IsOpenBalanceLine = groupByDate.Any(d => d.InterestEntityTypeCode == InterestEntities.OpenBalance)

                                                                                          }).OrderBy(d => d.GroupInterestValueDate).ToList();
            return interestTransactionsGroupedByDates;
        }

        private GLAccountInterestPeriodPM GetGLAccountInterestPeriodPMByPeriodToDate(InterestPercentageForDateParams interestPercentageForDateParams)
        {
            GLAccountInterestPeriodPM gLAccountInterestPeriodPM =
               interestPercentageForDateParams.InterestReportLinesByDateCreationParams.GLAccountInterestPeriodPMs
               .Where(d => d.PeriodStartDate <= interestPercentageForDateParams.ToDate)
               .OrderByDescending(d => d.PeriodStartDate).FirstOrDefault();

            if (gLAccountInterestPeriodPM == null)
            {
                string message = TextCodesTranslator.TranslateText("InterestReport.O.NoGlAccountPeriod", tenant, true);
                throw new ApplicationException(message);
            }

            return gLAccountInterestPeriodPM;
        }
    }

}
