using Logitude.Accounting.BL.CoreBL;
using Logitude.SystemLogs;
using System;
using System.Text;

namespace CommunicationWorkerRole.Tasks
{
    public class RecalculateCashbooksTotalsTask: TaskManagerBase
    {

        private StringBuilder stringBuilder;
        int tenant;

        public RecalculateCashbooksTotalsTask(string Id, int tenant) : base(Id, tenant)
        {
            stringBuilder = new StringBuilder();
            this.tenant = tenant;
        }

        public override void StartTask()
        {
            bool failed = false;
            try
            {


                stringBuilder.Append(DateTime.Now.ToString()).AppendLine("RecalculateCashbooksTotalsTask:Start");
                try
                {
                    var cashbookService = new CashbookService();
                    var updatedCount = cashbookService.RecalculateCashbooksTotals(tenant);
                    string responseText = updatedCount;
                    stringBuilder.Append(DateTime.Now.ToString()).Append("responseText:").Append(responseText).AppendLine();
                }
                catch (Exception ex)
                {
                    failed = true;
                    stringBuilder.Append(DateTime.Now.ToString()).Append("Exception:").Append(ex.Message).AppendLine();
                    ExceptionHandler.HandleException(ex, DateTime.Now, 0, "", "WorkerRole", $"RecalculateCashbooksTotalsTask()", null);
                }
            }
            finally
            {
                if (failed)
                {
                    throw new Exception(stringBuilder.ToString());
                }
            }
        }

    }
}
