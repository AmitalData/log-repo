using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL.InterestReport
{
    public class InterestReportCalculationPreparations:IInterestReportCalculateionPreparations
    {
        public InterestReportCalculationPreparations()
        {
            
        }
        public virtual List<InterestTransactionsGroupedByDate> GetInterestTransactionsGroupedByDate(List<InterestTransactionPM> interestTransactionPMs)
        {
            List<InterestTransactionsGroupedByDate> interestTransactionsGroupedByDates = (from interestTransaction in interestTransactionPMs
                                                                                          group interestTransaction by interestTransaction.InterestValueDate.Date into groupByDate
                                                                                          select new InterestTransactionsGroupedByDate()
                                                                                          {
                                                                                              GroupInterestValueDate = groupByDate.Key,
                                                                                              TotalLocalAmount = groupByDate.Sum(d => d.LocalAmount),

                                                                                          }).OrderBy(d=>d.GroupInterestValueDate).ToList();
            return interestTransactionsGroupedByDates;
        }

        public virtual List<InterestTransactionPM> GetInterestTransactionsForGlAccountAndInterestValueDate(string glaccountId, DateTime InterestReportCalculationDate,int tenant)
        {
            InterestTransactionQueryService interestTransactionQueryService = new InterestTransactionQueryService(tenant);
            return interestTransactionQueryService.GetInterestTransactionsForGlAccountAndInterestValueDate(glaccountId, InterestReportCalculationDate, tenant);
        }

        public virtual InterestReportPM GetInterestReportPM(string interestReportId,int tenant)
        {
            InterestReportQueryService interestReportQueryService = new InterestReportQueryService(tenant);
            InterestReportPM interestReportPM = interestReportQueryService.GetSingle(interestReportId, false, false);
            return interestReportPM;
        }

        public virtual GLAccountInterestPeriodPM GetGLAccountInterestPeriodPMWithinStartInterestDate(InterestReportPM interestReportPM,DateTime startInterestDate)
        {
            List<GLAccountInterestPeriodPM> gLAccountInterestPeriodPMs = GetGLAccountInterestPeriodsForInterestCalculationDateOrderedByDateDescending(interestReportPM,startInterestDate);
            GLAccountInterestPeriodPM gLAccountInterestPeriodPM = gLAccountInterestPeriodPMs.FirstOrDefault();
            if (gLAccountInterestPeriodPM == null)
            {
                throw new ApplicationException("the glaccount doesn't have any interest periods");
            }
            return gLAccountInterestPeriodPM;
        }

        public virtual decimal CalculateInterestBasesTypePercentage(string InterestRateBaseId, DateTime startInterestDate,int tenant)
        {
            InterestBasesPeriodQueryService interestBasesPeriodQueryService = new InterestBasesPeriodQueryService(tenant);
            InterestBasesPeriodPM standardInterestBasesPeriodPM = interestBasesPeriodQueryService.GetInterestBasesPeriodPMByBaseTypeIdAndStartDate(InterestRateBaseId, startInterestDate, tenant);
            if (standardInterestBasesPeriodPM == null) {
                throw new ApplicationException("there is no interest bases periods");
            }
            decimal percentage = standardInterestBasesPeriodPM.InterestRate;
            return percentage;
        }

        public virtual List<GLAccountInterestPeriodPM> GetGLAccountInterestPeriodsForInterestCalculationDateOrderedByDateDescending(InterestReportPM interestReportPM,DateTime interestCalculationDate)
        {
            int tenant = interestReportPM.Tenant;
            GLAccountInterestPeriodQueryService gLAccountInterestPeriodQueryService = new GLAccountInterestPeriodQueryService(tenant);
            List<GLAccountInterestPeriodPM> gLAccountInterestPeriodPMs = gLAccountInterestPeriodQueryService.GetLAccountInterestPeriodPMsByInterestDate(interestReportPM.GLAccountId, interestCalculationDate, tenant);
            return gLAccountInterestPeriodPMs.OrderByDescending(d => d.PeriodStartDate).ToList();
        }
    }

    public interface IInterestReportCalculateionPreparations
    {
        List<InterestTransactionsGroupedByDate> GetInterestTransactionsGroupedByDate(List<InterestTransactionPM> interestTransactionPMs);
        List<InterestTransactionPM> GetInterestTransactionsForGlAccountAndInterestValueDate(string glaccountId, DateTime InterestReportCalculationDate, int tenant);
        InterestReportPM GetInterestReportPM(string interestReportId, int tenant);
        GLAccountInterestPeriodPM GetGLAccountInterestPeriodPMWithinStartInterestDate(InterestReportPM interestReportPM, DateTime startInterestDate);
        decimal CalculateInterestBasesTypePercentage(string InterestRateBaseId, DateTime startInterestDate, int tenant);
        List<GLAccountInterestPeriodPM> GetGLAccountInterestPeriodsForInterestCalculationDateOrderedByDateDescending(InterestReportPM interestReportPM, DateTime interestCalculationDate);


    }
}
