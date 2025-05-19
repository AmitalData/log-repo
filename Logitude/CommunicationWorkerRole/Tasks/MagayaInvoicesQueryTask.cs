using Logitude.Accounting.BL.Utils;
using Logitude.SystemLogs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;

namespace CommunicationWorkerRole.Tasks
{
    public class MagayaInvoicesQueryTask : TaskManagerBase
    {

        private StringBuilder stringBuilder;

        public MagayaInvoicesQueryTask(string Id, int tenant) : base(Id, tenant)
        {
            stringBuilder = new StringBuilder();
        }

        public override void StartTask()
        {
            bool failed = false;
            try
            {


                Log("MagayaInvoicesQueryTask:Start", "Start");
                try
                   {
                        MagayaInvoicesQueryBatch magayaInvoicesQueryBatch = new MagayaInvoicesQueryBatch();
                        int tenant = this.Task != null ? this.Task.Tenant : 0 ;
                        magayaInvoicesQueryBatch.RunMagayaInvoicesQuery(this.Task.StartDateTimeUTC.ToString(), DateTime.Now.ToString(), tenant);
                        string responseText = magayaInvoicesQueryBatch.ResponseText();

                        Log(responseText, "responseText:");
                    }
                    catch (Exception ex)
                    {
                        failed = true;
                       Log(ex.Message, "Exception:");
                       ExceptionHandler.HandleException(ex, DateTime.Now, 0, "", "WorkerRole", $"MagayaInvoicesQueryTask()", null);
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


        private void Log(string message,string type)
        {
            stringBuilder.Append(DateTime.Now.ToString()).Append(type).Append(message).AppendLine();
        }

    }
}
