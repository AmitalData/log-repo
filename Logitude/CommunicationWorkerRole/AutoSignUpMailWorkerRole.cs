using System;
using System.Linq;
using System.Net;
using Microsoft.WindowsAzure.ServiceRuntime;
using System.Threading;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;

using WebFreight.Web.GlobalModel;

using WebFreight.Web.Helpers;
using WebFreight.Web.InfrastructureModel;
using Logitude.BL.CommonDataModel.EntityPMs;
using WebFreight.Web;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure.Azure;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure;
using Logitude.Server.Tools.Counters;
using WebFreight.Web.InfrastructureModel.DomainServices;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Simplog.Data.CommonDataModel;
using Simplog.Server.Infrastructure;
using Logitude.SystemLogs;
using System.Collections.Generic;

namespace CommunicationWorkerRole
{
    public class AutoSignUpMailWorkerRole : WorkerEntryPoint
    {

        public override void Run()
        {

            if (!string.IsNullOrEmpty(LogitudeSettings.AutoSignupEmail) && !string.IsNullOrEmpty(LogitudeSettings.AutoSignupPassword))
            {
                int sleeptime = 120000;
                AutoSignupEmailRepository autosignupRepository = new AutoSignupEmailRepository();
                nsoftware.IPWorksSSL.Pops pops1 = new nsoftware.IPWorksSSL.Pops();
                pops1.MailServer = "outlook.office365.com";
                pops1.MailPort = 995;
                pops1.User = LogitudeSettings.AutoSignupEmail;
                pops1.Password = LogitudeSettings.AutoSignupPassword;
                if (string.IsNullOrEmpty(pops1.RuntimeLicense))
                    pops1.RuntimeLicense = "31534E394141315355425241315355423548433932343435000000000000000000000000000000003133534E4E5A355A000057594B314558544D5A33524E0000";

                while (IsRunning)
                {
                    if (!General.IsUpdating()) //&& LogitudeSettings.DeploymentStage != "Dev")
                    {

                        try
                        {

                            if (!pops1.Connected)
                            {
                                pops1.Connect();

                            }
                            LastActivity = DateTime.UtcNow;
                            if (pops1.MessageCount > 0)
                            {
                                pops1.MaxLines = 0;
                                pops1.MessageNumber = pops1.MessageCount;
                                pops1.Retrieve();

                                string messageText = pops1.MessageText.Replace("&amp;", "&");
                                AutoSignupEmail auotsignupEmail = new AutoSignupEmail()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    EmailBody = messageText,
                                    Status = "New",
                                    Retries = 0,
                                    EmailSubject = TruncateLongString(pops1.MessageSubject, 200),
                                    CreateDate = DateTime.Now,
                                };

                                autosignupRepository.Add(auotsignupEmail);
                                autosignupRepository.SubmitChanges();
                                LogDoneItemInMemory();

                                pops1.Delete();
                                pops1.Disconnect();
                                pops1.Connected = false;

                                Thread.Sleep(sleeptime);//(120000); // 5 minutes300000
                            }

                            else
                            {
                                pops1.Disconnect();
                                pops1.Connected = false;
                                Thread.Sleep(sleeptime);
                            }
                        }
                        catch (Exception e)
                        {

                            ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole", "AutoSignUpWorkerRole : Run() Method", null);
                            try
                            {
                                pops1.Disconnect();
                            }
                            catch (Exception exp)
                            {
                                ExceptionHandler.HandleException(exp, DateTime.Now, 0, "", "AutoSignUp WorkerRole Monitor", "AutoSignUpWorkerRole : Run() Method", System.Environment.MachineName);                       
                            } 
                            pops1.Connected = false;

                            Thread.Sleep(sleeptime);
                        }
                    }
                    else
                    {
                        Thread.Sleep(sleeptime);

                    }

                }
            }
        }


        public string TruncateLongString(string str, int maxLength)
        {
            if (!string.IsNullOrEmpty(str))

                return str.Substring(0, Math.Min(str.Length, maxLength));

            else
                return str;
        }
        public override bool OnStart()
        {

            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "AutoSignUpMail";
            DoneItemsInRange = new Dictionary<DateTime, int>();
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
