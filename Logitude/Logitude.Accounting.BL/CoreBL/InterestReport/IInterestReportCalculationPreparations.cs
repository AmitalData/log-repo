using Logitude.Accounting.Def.EntityPMs;
using System;
using System.Collections.Generic;

namespace Logitude.Accounting.BL.CoreBL.InterestReport
{
    public interface IInterestReportCalculationPreparations
    {
        List<InterestTransactionPM> GetInterestTransactionsForGlAccountAndInterestValueDate(string glaccountId, DateTime InterestReportCalculationDate, int tenant);
        InterestReportPM GetInterestReportPM(string interestReportId, int tenant);
        List<GLAccountInterestPeriodPM> GetGlaccountInterestPeriods(InterestReportPM interestReportPM);
        List<InterestBasesPeriodPM> GetAllInterestBasesPeriodPMs(int tenant);
    }
}