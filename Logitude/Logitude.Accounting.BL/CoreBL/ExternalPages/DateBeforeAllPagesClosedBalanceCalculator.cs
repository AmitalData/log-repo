using Logitude.Accounting.Data.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL.ExternalPages
{
    public class DateBeforeAllPagesClosedBalanceCalculator : ExternalPageIntervalClosedBalanceCalculator
    {
        public DateBeforeAllPagesClosedBalanceCalculator(DateTime date, List<ReconcileExternalPage> externalPages, int tenant)
            : base(date, externalPages, tenant)
        { }

        public override decimal CalculateClosingBalance()
        {
            ReconcileExternalPage firstPage = externalPages.OrderBy(d => d.ToDate).FirstOrDefault();
            return firstPage.StartBalance;
        }
    }
}
