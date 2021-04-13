using Logitude.Accounting.Data.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL.ExternalPages
{
    public class ExternalPageIntervalClosedBalanceCalculatorFactory
    {
        private DateTime date;
        private List<ReconcileExternalPage> externalPages;
        public ExternalPageIntervalClosedBalanceCalculator CreateCalculator(DateTime date, List<ReconcileExternalPage> externalPages, int tenant)
        {
            this.date = date;
            this.externalPages = externalPages;

            if (dateWithinAPage != null) return new DateWithingAPageClosedBalanceCalculator(date, externalPages, tenant);
            else if (date > LastPage.ToDate) return new DateAfterAllPagesClosedBalanceCalculator(date, externalPages, tenant);
            else if (date < FirstPage.FromDate) return new DateBeforeAllPagesClosedBalanceCalculator(date, externalPages, tenant);
            else if (MostRecentPageBeforeTheDate != null) return new DateBetweenPagesClosedBalanceCalculator(date, externalPages, tenant);

            throw new NotImplementedException("The calculator args does not have suitable implementation");
        }



        private ReconcileExternalPage dateWithinAPage
        {
            get { return externalPages.FirstOrDefault(page => page.FromDate <= date && date <= page.ToDate); }
        }
        private ReconcileExternalPage LastPage
        {
            get { return externalPages.OrderByDescending(d => d.ToDate).FirstOrDefault(); }
        }
        private ReconcileExternalPage FirstPage
        {
            get { return externalPages.OrderBy(d => d.ToDate).FirstOrDefault(); }
        }
        private ReconcileExternalPage MostRecentPageBeforeTheDate
        {
            get { return externalPages.OrderByDescending(d => d.ToDate).Where(d => d.ToDate <= date).FirstOrDefault(); }
        }
    }
}
