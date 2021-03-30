using Logitude.Accounting.Data.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL.ExternalPages
{
    public class DateBetweenPagesClosedBalanceCalculator : ExternalPageIntervalClosedBalanceCalculator
    {
        public DateBetweenPagesClosedBalanceCalculator(DateTime date, List<ReconcileExternalPage> externalPages, int tenant)
            : base(date, externalPages, tenant)
        { }

        public override decimal CalculateClosingBalance()
        {
            ReconcileExternalPage mostRecentPageBeforeTheDate = externalPages.OrderByDescending(d => d.ToDate).Where(d => d.ToDate <= date).FirstOrDefault();
            return mostRecentPageBeforeTheDate.CloseBalance;
        }
    }
}
