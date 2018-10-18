using Microsoft.WindowsAzure.ServiceRuntime;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebFreight.Web.GlobalModel;

namespace CommunicationWorkerRole
{
    public class GeneralWorkerRole : WorkerEntryPoint
    {

        public override void Run()
        {
            //while (IsRunning)
            //{
            //    if (!General.IsUpdating())
            //    {
            //        IGlobalContext globalObjectContext = GlobalContext.GetContext();
            //        System.Collections.Generic.List<ContactPassword> lockedUsers = (from a in globalObjectContext.ContactPasswords
            //                                                                        where a.IsLocked == true
            //                                                                        select a).ToList();

            //        foreach (ContactPassword contact in lockedUsers)
            //        {
            //            if (contact.LockDateTime != null)
            //            {
            //                TimeSpan timeElapsed = (DateTime.Now - contact.LockDateTime.Value);
            //                if (timeElapsed.TotalMinutes > 30)
            //                {
            //                    contact.IsLocked = false;
            //                    contact.LockDateTime = null;
            //                    contact.NumberOfRetries = 0;

            //                }
            //            }
            //            else
            //            {
            //                contact.IsLocked = false;
            //                contact.LockDateTime = null;
            //            }
            //        }

            //        globalObjectContext.SaveChanges();

            //        Thread.Sleep(10000);
            //    }
            //    else
            //    {
            //        Thread.Sleep(120000);
            //    }
            //}
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;


            //DiagnosticMonitor.Start("DiagnosticsConnectionString");

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.
            RoleEnvironment.Changing += RoleEnvironmentChanging;

            return base.OnStart();
        }

        private void RoleEnvironmentChanging(object sender, RoleEnvironmentChangingEventArgs e)
        {
            // If a configuration setting is changing
            if (e.Changes.Any(change => change is RoleEnvironmentConfigurationSettingChange))
            {
                // Set e.Cancel to true to restart this role instance
                e.Cancel = true;
            }
        }
    }
}
