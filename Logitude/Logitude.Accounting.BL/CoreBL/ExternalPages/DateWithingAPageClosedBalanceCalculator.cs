using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL.ExternalPages
{
    public class DateWithingAPageClosedBalanceCalculator : ExternalPageIntervalClosedBalanceCalculator
    {
        public DateWithingAPageClosedBalanceCalculator(DateTime date, List<ReconcileExternalPage> externalPages, int tenant)
            : base(date, externalPages, tenant)
        { }

        
        public override decimal CalculateClosingBalance()
        {
            ReconcileExternalPage pageThatContainsTheDate = GetThePageContainsTheDate();

            List<ReconcileExternalPageLine> lines = GetPageLinesOrderedByReferenceDate(pageThatContainsTheDate);

            decimal linesSummationUpToDate = GetLinesSummationUpToDate(date, lines);
            return pageThatContainsTheDate.StartBalance + linesSummationUpToDate;
        }

        private ReconcileExternalPage GetThePageContainsTheDate()
        {
            return externalPages.FirstOrDefault(page => page.FromDate <= date && date <= page.ToDate);
        }

        private decimal GetLinesSummationUpToDate(DateTime date, List<ReconcileExternalPageLine> lines)
        {
            return lines.Where(line => line.ReferenceDate <= date)
                                                        .Sum(line => line.CreditAmount - line.DebitAmount);
        }
        private List<ReconcileExternalPageLine> GetPageLinesOrderedByReferenceDate(ReconcileExternalPage page)
        {
            var accountingContext = AccountingContext.GetContext(tenant);
            ReconcileExternalPageLineListQueryService linesQueryService = new ReconcileExternalPageLineListQueryService(accountingContext);
            var lines = linesQueryService.GetPageLines(page.Id, tenant);
            lines = lines.OrderBy(line => line.ReferenceDate).ToList();
            return lines;
        }
    }
}
