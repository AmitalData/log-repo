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
    public class BatchCancelClosingJournalTask : BatchTaskExecutionsService
    {
        public BatchCancelClosingJournalTask(BatchTaskExecutionPM batchTaskExecution) : base(batchTaskExecution) { }

        public override void RunCode()
        {
            try
            {
                BatchCancelClosingJournalTaskArgs args = GetArgs<BatchCancelClosingJournalTaskArgs>();

                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    TaxReportClosingService closingService = new TaxReportClosingService(args.Tenant, args.TaxReportId);
                    closingService.CancelClosingJournal();
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteFatal(ex);
                throw;
            }
        }
    }

    public class BatchCancelClosingJournalTaskArgs
    {
        public int Tenant { get; set; }
        public string TaxReportId { get; set; }
    }
}
