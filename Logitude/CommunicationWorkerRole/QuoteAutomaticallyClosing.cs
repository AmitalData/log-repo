using Logitude.SystemLogs;
using Microsoft.WindowsAzure.ServiceRuntime;
using Simplog.Data.CommonDataModel;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using WebFreight.Web.QuoteModel;

namespace CommunicationWorkerRole
{
    public class QuoteAutomaticallyClosing : WorkerEntryPoint
    {
        public override void Run()
        {
            //while (IsRunning)
            //{
            //    if (!General.IsUpdating())
            //    {

            //        DateTime date1 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 30, 0);
            //        DateTime date2 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 40, 0);

            //        if (DateTime.Now >= date1 && DateTime.Now <= date2)
            //        {
            //            LastActivity = DateTime.UtcNow;
            //            ICommonDataContext iContext = CommonDataContext.GetContext(0);
            //            List<int> AllTenants = (from d in iContext.Tenants select d.Id).ToList();
            //            if (AllTenants != null)
            //            {
            //                DateTime? StartDateTime = null;
            //                DateTime? EndDateTime = null;
            //                bool iHasException = false;
            //                string iExceptionMessage = null;
            //                foreach (int iTenant in AllTenants)
            //                {
            //                    try
            //                    {
            //                        //using (TransactionScope scope = TransactionFactory.GetTransaction(new TimeSpan(3, 0, 0)))
            //                        //{
            //                        StartDateTime = DateTime.Now;

            //                        bool isExists = QuoteModelProcedureClass.IsExistsQuoteAutomaticallyClosingDataHistory(iTenant, StartDateTime);
            //                        if (!isExists)
            //                        {
            //                            QuoteModelProcedureClass.ExecuteSingleQuoteAutomaticallyClosing(iTenant);
            //                            //scope.Complete();
            //                            EndDateTime = DateTime.Now;
            //                        }
            //                        //}
            //                    }

            //                    catch (Exception ex)
            //                    {
            //                        iHasException = true;
            //                        iExceptionMessage = ex.Message;
            //                        EndDateTime = DateTime.Now;
            //                        ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Quote automatically closing worker role, Tenant: " + iTenant, null, null);
            //                        Thread.Sleep(10000);
            //                    }

            //                    finally
            //                    {
            //                        if (StartDateTime != null && EndDateTime != null)
            //                        {
            //                            QuoteModelProcedureClass.InsertQuoteAutomaticallyClosingDataHistory(iTenant, StartDateTime, EndDateTime, iHasException, iExceptionMessage);
            //                        }
            //                    }
            //                }

            //                LogDoneItemInMemory();
            //                Thread.Sleep(3600000);
            //            }

            //            else
            //            {
            //                Thread.Sleep(60000);
            //            }

            //        }

            //        else
            //        {
            //            Thread.Sleep(60000);
            //        }

            //    }

            //    else
            //    {
            //        Thread.Sleep(60000);
            //    }
            //}
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
