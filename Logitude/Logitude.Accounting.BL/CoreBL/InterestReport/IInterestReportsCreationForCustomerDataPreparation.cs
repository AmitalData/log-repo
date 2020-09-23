using Logitude.Accounting.BL.DataContract;
using Logitude.Accounting.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL.InterestReport
{
    public interface IInterestReportsCreationForCustomerDataPreparation
    {
        InterestReportPM GetDraftInterestReportForCustomer(InterestReportCustomerPM interestReportCustomerPM);

        InterestReportPM GetPreviousInvoicedOrCloseWithoutInvoicedtInterestReportForCustomer(InterestReportCustomerPM interestReportCustomerPM, DateTime interestCalculationDate);

        List<InterestReportCustomerPM> GetEligibleCustomersForInterestReports(int tenant);

        InterestReportPM CreateInterestReportForCustomerGlAccount(InterestReportCustomerPM interestReportCustomerPM);
        string GetErrorMessage(Exception exception);
    }
}
