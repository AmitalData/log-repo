using Logitude.Accounting.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL.InterestReport
{
    public class InterestReportLinesByDateCreationParams
    {
        public InterestReportLinesByDateCreationParams(InterestReportPM interestReportPM,
            List<InterestTransactionPM> interestTransactionPMs,
            List<GLAccountInterestPeriodPM> gLAccountInterestPeriodPMs,
            List<InterestBasesPeriodPM> interestBasesPeriodPMs, DateTime? recentCalculationDate)
        {
            this.InterestReportPM = interestReportPM;
            this.InterestTransactionPMs = interestTransactionPMs;
            this.GLAccountInterestPeriodPMs = gLAccountInterestPeriodPMs;
            this.InterestBasesPeriodPMs = interestBasesPeriodPMs;
            this.RecentCalculationDate = recentCalculationDate;
        }
        public InterestReportPM InterestReportPM { get; private set; }
        public List<InterestTransactionPM> InterestTransactionPMs { get; private set; }
        public List<GLAccountInterestPeriodPM> GLAccountInterestPeriodPMs { get; private set; }
        public List<InterestBasesPeriodPM> InterestBasesPeriodPMs { get; private set; }
        public DateTime? RecentCalculationDate { get; private set; }


    }
}
