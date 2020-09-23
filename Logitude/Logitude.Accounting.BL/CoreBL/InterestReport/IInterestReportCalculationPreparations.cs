using Logitude.Accounting.Data.Utilities;
using Logitude.Accounting.Def.EntityPMs;
using System;
using System.Collections.Generic;

namespace Logitude.Accounting.BL.CoreBL.InterestReport
{
    public interface IInterestReportCalculationPreparations
    {
        List<InterestTransactionPM> GetInterestTransactionsForGlAccountAndInterestValueDate(InterestTransactionGetParameters interestTransactionGetParameters);
        InterestReportPM GetInterestReportPM(string interestReportId, int tenant);
        decimal? GetCreditLimitFromGLAccount(string GLAccount, int tenant);
        List<GLAccountInterestPeriodPM> GetGlaccountInterestPeriods(InterestReportPM interestReportPM);
        List<InterestBasesPeriodPM> GetAllInterestBasesPeriodPMs(int tenant);
        GLAccountPM GetGLAccount(string GLAccountId, int tenant);
    }
}