using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL
{
    public class ExternalPagesBalanceService
    {
        private int tenant;
        public ExternalPagesBalanceService(int tenant)
        {
            this.tenant = tenant;
        }


        /// <summary>
        /// Calculates the closed balance of external entity (Bank Page or GL Account) on a specific date
        /// </summary>
        /// <param name="objectTableName">external entity object table name</param>
        /// <param name="EntityId">external entity id</param>
        /// <param name="date">date to get closed balance by</param>
        public decimal GetClosingBalanceByDate(string objectTableName,string EntityId,DateTime date)
        {
            List<ReconcileExternalPage> externalPages = GetApprovedExternalPages(objectTableName, EntityId);
            if (externalPages != null && externalPages.Count() > 0)
                return CalculateClosedBalanceFromExternalPages(date, externalPages);
            return 0;
        }

        private decimal CalculateClosedBalanceFromExternalPages(DateTime date, List<ReconcileExternalPage> externalPages)
        {
            decimal closedBalance = 0;

            ExternalPagesDescriptor pagesDescription = new ExternalPagesDescriptor(date, externalPages);
            pagesDescription.BuildDescriptor();

            if (pagesDescription.HasPageThatContainsTheDate)
                closedBalance = CalculateClosedBalanceFromPage(pagesDescription.PageThatContainsTheDate, date);

            else if (pagesDescription.IsDateAfterThanAllPages)
                closedBalance = pagesDescription.LastPage.CloseBalance;

            else if (pagesDescription.IsDateEarlierThanAllPages)
                closedBalance = pagesDescription.FirstPage.StartBalance;

            else if (pagesDescription.IsDateBetweenThePages)
                closedBalance = pagesDescription.MostRecentPageBeforeTheDate.CloseBalance;

            return closedBalance;
        }

        private decimal CalculateClosedBalanceFromPage(ReconcileExternalPage page, DateTime date)
        {
            List<ReconcileExternalPageLine> lines = GetPageLinesOrderedByReferenceDate(page);

            decimal linesSummationUpToDate = GetLinesSummationUpToDate(date, lines);
            decimal closedBalance = page.StartBalance + linesSummationUpToDate;

            return closedBalance;
        }

        private static decimal GetLinesSummationUpToDate(DateTime date, List<ReconcileExternalPageLine> lines)
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

        private List<ReconcileExternalPage> GetApprovedExternalPages(string objectTableName, string EntityId)
        {
            string objectTableId = GetObjectTableId(objectTableName);
            var accountingContext = AccountingContext.GetContext(tenant);
            ReconcileExternalPageListQueryService pageListQueryService = new ReconcileExternalPageListQueryService(accountingContext);

            var externalPages = pageListQueryService.GetApprovedExternalPages(objectTableId, EntityId, tenant);

            return externalPages;
        }

        private string GetObjectTableId(string objectTableName)
        {
            ObjectTableQuery objectTableQuery = new ObjectTableQuery(tenant);
            ObjectTablePM objectTable = objectTableQuery.GetObjectTableByName(objectTableName, tenant);
            string objectTableId = objectTable.Id;
            return objectTableId;
        }
    }



    public class ExternalPagesDescriptor
    {
        private List<ReconcileExternalPage> externalPages;
        private DateTime date;
        public ExternalPagesDescriptor(DateTime date, List<ReconcileExternalPage> externalPages)
        {
            this.date = date;
            this.externalPages = externalPages;
        }

        public void BuildDescriptor()
        {
            BuildPagesFacts();
            SetNamedPages();
        }
        private void BuildPagesFacts()
        {
            HasPageThatContainsTheDate = PageThatContainsTheDate != null;
            IsDateAfterThanAllPages = date > LastPage.ToDate;
            IsDateEarlierThanAllPages = date < FirstPage.FromDate;
            IsDateBetweenThePages = MostRecentPageBeforeTheDate != null;
        }

        private void SetNamedPages()
        {
            PageThatContainsTheDate = externalPages.FirstOrDefault(page => page.FromDate <= date && date <= page.ToDate);
            LastPage = externalPages.OrderByDescending(d => d.ToDate).FirstOrDefault();
            FirstPage = externalPages.OrderBy(d => d.ToDate).FirstOrDefault();
            MostRecentPageBeforeTheDate = externalPages.OrderByDescending(d => d.ToDate).Where(d => d.ToDate <= date).FirstOrDefault();
        }

        public bool HasPageThatContainsTheDate { get; set; }
        public bool IsDateAfterThanAllPages { get; set; }
        public bool IsDateEarlierThanAllPages { get; set; }
        public bool IsDateBetweenThePages { get; set; }
        public ReconcileExternalPage PageThatContainsTheDate { get; set; }
        public ReconcileExternalPage LastPage { get; set; }
        public ReconcileExternalPage FirstPage { get; set; }
        public ReconcileExternalPage MostRecentPageBeforeTheDate { get; set; }

    }
}
