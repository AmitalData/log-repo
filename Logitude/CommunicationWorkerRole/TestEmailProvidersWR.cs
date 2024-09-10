using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.IO;
using System.Net.Mail;
using System.Net;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;

using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure.Azure;

using WebFreight.Web;

using System.Text.RegularExpressions;
using System.Text;

using System.Transactions;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.ServiceBus.Messaging;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.SystemLogs;


namespace CommunicationWorkerRole
{
    public class TestEmailProvidersWR : WorkerEntryPoint
    {

      
        public override void Run()
        {

            while (IsRunning)
            {
                if (!General.IsUpdating())
                {
                    try
                    {
                        LastActivity = DateTime.UtcNow;
                        TestAllProviders();
                        LogDoneItemInMemory();
                        Thread.Sleep(new TimeSpan(0, 5, 0));
                    }
                    catch (Exception ex)
                    {
                        ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "test email providers worker role start", null, null);
                        Thread.Sleep(10000);
                    }

                }
                else
                {
                    Thread.Sleep(60000);
                }
            }
        }

        private static void TestAllProviders()
        {
            EmailProviderRepository emailProviderRepository = new EmailProviderRepository(0);
            List<EmailProvider> activeProviders = emailProviderRepository.GetActiveEmailProviders().ToList();

            foreach (EmailProvider provider in activeProviders)
            {

                DateTime currentTestDate = DateTime.UtcNow;
                EmailParameters parameters = new EmailParameters()
                {
                    From = SettingUtil.Emails.FromNoReply,
                    To = SettingUtil.Emails.AutoSignupGroup,
                    Subject = currentTestDate.ToString(),
                    Body = currentTestDate.ToString(),
                    ProviderNumber = int.Parse(provider.ProviderNumber),
                    IsProviderNumberSpecified = true,
                };

                EmailingHelper.SendEmail(parameters);
                provider.LastTestSendDate = currentTestDate;
                emailProviderRepository.Update(provider);
                emailProviderRepository.SubmitChanges();

                Thread.Sleep(new TimeSpan(0, 2, 0));


                nsoftware.IPWorksSSL.Pops pops1 = new nsoftware.IPWorksSSL.Pops();
                pops1.MailServer = "outlook.office365.com";
                pops1.MailPort = 995;
                pops1.User = "AutoSignup@logitudeworld.com";
                pops1.Password = "$Gen)(876";
                pops1.Connect();
                pops1.MaxLines = 0;
                pops1.MessageNumber = pops1.MessageCount;
                pops1.Retrieve();
                string subject = pops1.MessageSubject;
                if (subject != null)
                {
                    DateTime currentReceiveDate = DateTime.Parse(subject);
                    provider.LastTestReceivedDate = currentReceiveDate;
                }
                emailProviderRepository.Update(provider);
                emailProviderRepository.SubmitChanges();


            }

           

        }

        public override bool OnStart()
        {
            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "TestEmailProviders";
            DoneItemsInRange = new Dictionary<DateTime, int>();

            try
            {
              
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "email worker role start", null, null);
            }

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
