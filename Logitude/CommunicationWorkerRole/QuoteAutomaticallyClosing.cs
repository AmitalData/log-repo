using Logitude.SystemLogs;
using Microsoft.WindowsAzure.ServiceRuntime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebFreight.Web.QuoteModel;

namespace CommunicationWorkerRole
{
    public class QuoteAutomaticallyClosing : WorkerEntryPoint
    {
        public override void Run()
        {
            while (IsRunning)
            {
                if (!General.IsUpdating())
                {
                    try
                    {
                        DateTime date1 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 30, 0);
                        DateTime date2 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 0);

                        if (DateTime.Now >= date1 && DateTime.Now <= date2)
                        {
                            LastActivity = DateTime.UtcNow;
                            QuoteModelProcedureClass.ExecuteDailyJobAutomaticallyClosing();
                            LogDoneItemInMemory();

                            Thread.Sleep(3600000);
                        }

                        else
                        {
                            Thread.Sleep(60000);
                        }
                    }

                    catch (Exception ex)
                    {
                        ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Quote automatically closing worker role", null, null);
                        Thread.Sleep(10000);
                    }
                }

                else
                {
                    Thread.Sleep(60000);
                }
            }
        }

        public override bool OnStart()
        {
            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "QuoteAutomaticallyClosing";
            DoneItemsInRange = new Dictionary<DateTime, int>();

            ServicePointManager.DefaultConnectionLimit = 12;
            RoleEnvironment.Changing += RoleEnvironmentChanging;
            return base.OnStart();
        }

        private void RoleEnvironmentChanging(object sender, RoleEnvironmentChangingEventArgs e)
        {
            if (e.Changes.Any(change => change is RoleEnvironmentConfigurationSettingChange))
            {
                e.Cancel = true;
            }
        }
    }
}
