using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL.InterestReport
{
    public class InterestPercentageForDateParams
    {
        public InterestPercentageForDateParams(InterestReportLinesByDateCreationParams interestReportLinesByDateCreationParams, DateTime toDate)
        {
            InterestReportLinesByDateCreationParams = interestReportLinesByDateCreationParams;
            ToDate = toDate;
        }
        public InterestReportLinesByDateCreationParams InterestReportLinesByDateCreationParams { get; private set; }
        public DateTime ToDate { get; private set; }
    }
}
