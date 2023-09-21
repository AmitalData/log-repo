using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.ExtendedServices;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Logitude.Accounting.BL.CoreBL.Batch
{
    public class BatchClosingTaxReportJournalTask : BatchTaskExecutionsService
    {
        public BatchClosingTaxReportJournalTask(BatchTaskExecutionPM batchTaskExecution) : base(batchTaskExecution) { }

        public override void RunCode()
        {
            try
            {
                BatchClosingTaxReportJournalTaskArgs args = GetArgs<BatchClosingTaxReportJournalTaskArgs>();

                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    TaxReportClosingService closingService = new TaxReportClosingService(args.Tenant, args.TaxReportId);
                    closingService.CloseTaxReport();
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {

                LogitudeSettings.HandleLogMe(ex.ToString(), true, "BatchGLAccountInterestActivationBalanceTask", new DateTime(2019, 10, 1));
                throw;
            }
        }
    }

    public class BatchClosingTaxReportJournalTaskArgs
    {
        public int Tenant { get; set; }
        public string TaxReportId { get; set; }
    }
}
