using CommunicationWorkerRole.Services;
using Logitude.SystemLogs;
using Simplog.Data.CommonDataModel;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using WebFreight.Web.QuoteModel;

namespace CommunicationWorkerRole.Tasks
{
    public class QuoteAutomaticallyClosingTask : TaskManagerBase
    {
        public QuoteAutomaticallyClosingTask(string Id, int tenant)
            : base(Id, tenant)
        {

        }

        public override void StartTask()
        {
            try
            {
                QuoteSchedulerTaskService quoteSchedulerTaskService = new QuoteSchedulerTaskService(this);
                quoteSchedulerTaskService.ExecuteDailyAutomaticallyClosing(Tenant);
            }
            catch (Exception ex)
            {
                string errorMessage = new StringBuilder().Append("Exception Message: ").AppendLine().Append(ex.Message).AppendLine().ToString();
                errorMessage += new StringBuilder().Append("Stack Trace:").AppendLine().Append(ex.StackTrace).AppendLine().ToString();
                throw new Exception(errorMessage);
            }
        }
    }
}
