using Logitude.Accounting.BL.Utils;
using Logitude.SystemLogs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;

namespace CommunicationWorkerRole.Tasks
{
    public class ReconcileInProgressCleanupTask : TaskManagerBase
    {

        private StringBuilder _SB;

        public ReconcileInProgressCleanupTask(string Id, int tenant) : base(Id, tenant)
        {
            _SB = new StringBuilder();
        }

        public override void StartTask()
        {
                   bool failed = false;
           
                    try
                    {
                        _SB.Append(DateTime.Now.ToString()).AppendLine("ReconcileInProgressCleanupTask:Start");

                        var reconcileInProgressCleanupBatch = new ReconcileInProgressCleanupBatch();
                        int tenant = this.Task != null ? this.Task.Tenant : 0;
                         reconcileInProgressCleanupBatch.ResetInProgressTransactions();
                         string responseText = reconcileInProgressCleanupBatch.GetResponseText();
                        _SB.Append(DateTime.Now.ToString()).Append("responseText:").Append(responseText).AppendLine();
                    }
                    catch (Exception ex)
                    {
                        failed = true;
                        _SB.Append(DateTime.Now.ToString()).Append("Exception:").Append(ex.Message).AppendLine();
                    ExceptionHandler.HandleException(ex, DateTime.Now, 0, "", "WorkerRole", $"ReconcileInProgressCleanupTask()", null);
                    }

                    finally
                    {
                        if (failed)
                        {
                            throw new Exception(_SB.ToString());
                        }
                    }
         
        }

    }
}
