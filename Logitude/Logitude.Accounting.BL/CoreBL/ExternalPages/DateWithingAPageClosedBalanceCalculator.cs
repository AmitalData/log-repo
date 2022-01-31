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
            List<ReconcileExternalPage> pagesThatContainsTheDate = GetPagesContainsTheDate();

            if (pagesThatContainsTheDate.Count() == 2 && CheckIfPagesHasCrossDate(pagesThatContainsTheDate))
            {
                var latestPage = pagesThatContainsTheDate.Last();
                var firstPage = pagesThatContainsTheDate.First();
                bool pagesHasCrossDates = latestPage.FromDate == firstPage.ToDate;
                if(pagesHasCrossDates)
                    return CalculateBalanceByPage(latestPage);
                return 0;

            }
            else
            {
                return CalculateBalanceByPage(pagesThatContainsTheDate.FirstOrDefault());
            }
        }

        private decimal CalculateBalanceByPage(ReconcileExternalPage pageThatContainsTheDate)
        {
            List<ReconcileExternalPageLine> lines = GetPageLinesOrderedByReferenceDate(pageThatContainsTheDate);

            decimal linesSummationUpToDate = GetLinesSummationUpToDate(date, lines);
            return pageThatContainsTheDate.StartBalance + linesSummationUpToDate;
        }

        private List<ReconcileExternalPage> GetPagesContainsTheDate()
        {
            return externalPages.Where(page => page.FromDate <= date && date <= page.ToDate).OrderBy(page=>page.FromDate).ToList();
        }

        private bool CheckIfPagesHasCrossDate(List<ReconcileExternalPage> pagesThatContainsTheDate)
        {
            var latestPage = pagesThatContainsTheDate.Last();
            var firstPage = pagesThatContainsTheDate.First();
            bool pagesHasCrossDates = latestPage.FromDate == firstPage.ToDate;
            return pagesHasCrossDates;
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
