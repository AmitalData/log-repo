using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data.Utilities;
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
        public virtual List<InterestTransactionPM> GetInterestTransactionsForGlAccountAndInterestValueDate(InterestTransactionGetParameters interestTransactionGetParameters)
        {
            InterestTransactionQueryService interestTransactionQueryService = new InterestTransactionQueryService(interestTransactionGetParameters.Tenant);
            return interestTransactionQueryService.GetInterestTransactionsForGlAccountAndInterestValueDate(interestTransactionGetParameters);
        }
        public virtual InterestReportPM GetInterestReportPM(string interestReportId,int tenant)
        {
            InterestReportQueryService interestReportQueryService = new InterestReportQueryService(tenant);
            InterestReportPM interestReportPM = interestReportQueryService.GetSingle(interestReportId, false, false);
            return interestReportPM;
        }
        public virtual decimal? GetCreditLimitFromGLAccount(string GLAccountId, int tenant)
        {
            InterestReportQueryService interestReportQueryService = new InterestReportQueryService(tenant);
            decimal? CreditLimit = interestReportQueryService.GetCreditLimitFromGLAccount(GLAccountId, tenant);
            return CreditLimit;
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

        public GLAccountPM GetGLAccount(string GLAccountId, int tenant)
        {
            throw new NotImplementedException();
        }
    }

   
}
