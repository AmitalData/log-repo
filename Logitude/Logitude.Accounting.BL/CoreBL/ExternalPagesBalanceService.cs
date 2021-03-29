using Logitude.Accounting.BL.CoreBL.ExternalPages;
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
            ExternalPageIntervalClosedBalanceCalculatorFactory calculatorFactory = new ExternalPageIntervalClosedBalanceCalculatorFactory();

            var calculator = calculatorFactory.CreateCalculator(date, externalPages, tenant);

            return calculator.CalculateClosingBalance();
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
