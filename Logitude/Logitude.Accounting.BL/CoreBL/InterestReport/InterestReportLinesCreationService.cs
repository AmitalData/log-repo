using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL.InterestReport
{
    public class InterestReportLinesCreationService
    {
        public void CreateInterestReportLines(List<InterestTransactionPM> interestTransactionPMs,string interestReportId,int tenant)
        {
            IAccountingContext accountingContext = AccountingContext.GetContext(tenant);
            InterestReportLineUpdateService interestReportLineUpdateService = new InterestReportLineUpdateService(accountingContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), tenant);

            for (int i = 0; i < interestTransactionPMs.Count; i++)
            {
                InterestReportLinePM interestReportLinePM = new InterestReportLinePM()
                {
                    InterestReportId = interestReportId,
                    InterestTransactionId = interestTransactionPMs[i].Id,
                    Tenant = tenant,
                    ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                };
                interestReportLineUpdateService.Update(interestReportLinePM, true);
            }
        }
    }
}
