using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL.InterestReport
{
    public class InterestReportCalculationPreparations: IInterestReportCalculationPreparations
    {
        public InterestReportCalculationPreparations()
        {
            
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

        public List<GLAccountInterestPeriodPM> GetGlaccountInterestPeriods(InterestReportPM interestReportPM)
        {
            int tenant = interestReportPM.Tenant;
            GLAccountInterestPeriodQueryService gLAccountInterestPeriodQueryService = new GLAccountInterestPeriodQueryService(tenant);
            List<GLAccountInterestPeriodPM> gLAccountInterestPeriodPMs = gLAccountInterestPeriodQueryService.GetLAccountInterestPeriodPMsByInterestDate(interestReportPM.GLAccountId, interestReportPM.InterestCalculationDate, tenant);
            return gLAccountInterestPeriodPMs.OrderByDescending(d => d.PeriodStartDate).ToList();
        }

        public List<InterestBasesPeriodPM> GetAllInterestBasesPeriodPMs(int tenant)
        {
            InterestBasesPeriodQueryService interestBasesPeriodQueryService = new InterestBasesPeriodQueryService(tenant);
            List<InterestBasesPeriodPM> interestBasesPeriodPMs = interestBasesPeriodQueryService.GetAllInterestBasesPeriodPMs(tenant);
            return interestBasesPeriodPMs;
        }
    }

   
}
