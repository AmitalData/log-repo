using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.EntityQueryServices
{
    public partial class InterestReportQueryService
    {
        public decimal GetClosedBalanceOfLastInvoicedOrClosedWithoutInvoiceInterestReport(int tenant)
        {
            decimal closedBalance = 0;
            InterestReportRepository interestReportRepository = new InterestReportRepository(tenant);
            closedBalance = interestReportRepository.GetClosedBalanceOfLastInvoicedOrClosedWithoutInvoiceInterestReport(tenant);
            return closedBalance;
        }
    }
}
