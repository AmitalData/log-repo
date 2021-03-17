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

            decimal closedBalance = 0;
            var pageContainsTheDate = externalPages.FirstOrDefault(page => page.FromDate <= date && date <= page.ToDate);
            var lastPage = externalPages.OrderByDescending(d => d.ToDate).FirstOrDefault();
            var firstPage = externalPages.OrderBy(d => d.ToDate).FirstOrDefault();
            var mostRecentPageBeforeTheDate = externalPages.OrderByDescending(d => d.ToDate).Where(d=>d.ToDate <= date).FirstOrDefault();

            if (pageContainsTheDate != null)
                closedBalance =  CalculateClosedBalanceFromPage(pageContainsTheDate, date);
            else if(date > lastPage.ToDate)
                closedBalance = lastPage.CloseBalance;
            else if (date < firstPage.FromDate)
                closedBalance = firstPage.StartBalance;
            else if (mostRecentPageBeforeTheDate != null)
                closedBalance = mostRecentPageBeforeTheDate.CloseBalance;


            return closedBalance;
        }

        private decimal CalculateClosedBalanceFromPage(ReconcileExternalPage page, DateTime date)
        {
            List<ReconcileExternalPageLine> lines = GetPageLines(page);

            decimal linesSummationUpToDate = GetLinesSummationUpToDate(date, lines);
            decimal closedBalance = page.StartBalance + linesSummationUpToDate;

            return closedBalance;
        }

        private static decimal GetLinesSummationUpToDate(DateTime date, List<ReconcileExternalPageLine> lines)
        {
            return lines.Where(line => line.ReferenceDate <= date)
                                                        .Sum(line => line.CreditAmount - line.DebitAmount);
        }

        private List<ReconcileExternalPageLine> GetPageLines(ReconcileExternalPage page)
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
}
