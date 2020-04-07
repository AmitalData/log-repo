using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityKeys;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Def.EntityPMs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.EntityQueryServices
{
    public partial class InterestReportQueryService
    {
        public decimal GetClosedBalanceOfLastInvoicedOrClosedWithoutInvoiceInterestReport(int tenant,string glaccountId)
        {
            decimal closedBalance = 0;
            InterestReportRepository interestReportRepository = new InterestReportRepository(tenant);
            closedBalance = interestReportRepository.GetClosedBalanceOfLastInvoicedOrClosedWithoutInvoiceInterestReport(tenant, glaccountId);
            return closedBalance;
        }
        public override void GetComposition(EntityKeyFields entityKeys, InterestReportPM entityPM)
        {
            IAccountingContext context = MainContext as IAccountingContext;
            InterestReportKeys activityKeys = entityKeys as InterestReportKeys;
            InterestReportLinesByDateQueryService queryService = new InterestReportLinesByDateQueryService(context);
            entityPM.InterestReportLinesByDates = queryService.GetMulti(activityKeys, true);
        }

        public bool CheckRecentCustomerReports(int tenant, DateTime interestDate, string customerId)
        {
            return (from a in context.InterestReports
                    where a.Tenant == tenant && a.InterestCalculationDate > interestDate && a.InterestReportStatusCode != "3" && a.CustomerId == customerId
                    select a).Any();
        }
    }
}
