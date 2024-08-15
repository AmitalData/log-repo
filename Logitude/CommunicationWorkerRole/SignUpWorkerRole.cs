using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure;

using System.Threading;

using System.Net;
using Microsoft.WindowsAzure.ServiceRuntime;
using System.IO;
using System.Xml.Serialization;

using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Azure;

using WebFreight.Web.InfrastructureModel;
using Simplog.Server.Infrastructure.Helpers;
using Microsoft.WindowsAzure.Storage.Queue;
using Logitude.SystemLogs;
using System.Diagnostics;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.Server.Tools;
using CommunicationWorkerRole.Services.Logbox;
using System.Net.Http;
using Newtonsoft.Json;
using CommunicationWorkerRole.Services.SignUp;
using System.Drawing;

namespace CommunicationWorkerRole
{
    public class SignUpWorkerRole : WorkerEntryPoint
    {
        CloudStorageAccount storageAccount;
        CloudQueueClient queueclient;
        CloudQueue queue;
        private bool _OnStartDone;
        //just for test
        public int MillisecondsTimeout = 10000;
        public override void Run()
        {
            // testing check out operation

            while (IsRunning)
            {

                if (!General.IsUpdating() && LogitudeSettings.WorkerRoleName.ToLower() != "staging")
                {
                    WorkOnce();
                    Thread.Sleep(MillisecondsTimeout);//600000
                }

            }

        }

        public void WorkOnce()
        {
            if (!General.IsUpdating() && LogitudeSettings.WorkerRoleName.ToLower() != "staging")
            {


                OnStart();
                if (General.IsUpdating())
                {
                    Thread.Sleep(300000);
                    return;
                }
                if (!queue.Exists()) //??? onstart built it >>
                {
                    return;
                }
                var msg = queue.GetMessage(TimeSpan.FromSeconds(5));
                LastActivity = DateTime.UtcNow;
                if (msg == null)
                {
                    return;
                }
                try
                {
                    MemoryStream memorystream = new MemoryStream(msg.AsBytes);

                    XmlSerializer serializer = new XmlSerializer(typeof(SignUpInfoClass));

                    SignUpInfoClass signUpInfo = (SignUpInfoClass)serializer.Deserialize(memorystream);
                    string message = msg.AsString;

                    queue.DeleteMessage(msg);

                    //string email = signUpInfo.Email;
                    //string name = signUpInfo.Name;
                    //string company = signUpInfo.Company;
                    //string phone = signUpInfo.phone;

                    if (signUpInfo.IsCreateLogboxTenantFromCloud && !signUpInfo.IsCreateLogboxTenantFromCloudPassed)
                    {
                        QueueMessageCloudToLogboxSender.Send(signUpInfo);
                    }
                    else if (signUpInfo.IsCreateLogboxTenantFromCloud)
                    {
                        CreateTenant(signUpInfo);
                    }
                    else if (signUpInfo.IsCrmTenant)
                    {
                        CardRepository cardRepository = new CardRepository(signUpInfo.Tenant);
                        string accountingCard = cardRepository.GetAccountingCardFromCard(signUpInfo.CustomerId, signUpInfo.Tenant);
                        if (string.IsNullOrEmpty(accountingCard)) CreateTenant(signUpInfo);
                        else return;

                    }
                    else CreateTenant(signUpInfo);
                    LogDoneItemInMemory();
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.ToString());
                    ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole", "SignUpWorkerRole : Run() Method", null);
                    Thread.Sleep(10000);
                }
            }
        }

        public void CreateTenant(SignUpInfoClass signUpInfo)
        {
            Debug.WriteLine("CreateTenant:Email=" + signUpInfo.Email);
            string password = SignUpClass.StartSignUp(signUpInfo);
            this.Password = password;
            Debug.WriteLine("CreateTenant:password=" + password);
            //EmailsWorkerRole emailrole = new EmailsWorkerRole();


            StringBuilder HtmlTemplate = new StringBuilder();
            HtmlTemplate.Append("<div style='text-align:left;'>");
            HtmlTemplate.Append("<br /><br />");
            HtmlTemplate.Append("Name: " + signUpInfo.Name);
            HtmlTemplate.Append("<br /><br />");
            HtmlTemplate.Append("Email: " + signUpInfo.Email);
            HtmlTemplate.Append("<br /><br />");
            HtmlTemplate.Append("Company: " + signUpInfo.Company);
            HtmlTemplate.Append("<br /><br />");
            HtmlTemplate.Append("Country: " + signUpInfo.CountryName);
            HtmlTemplate.Append("<br /><br />");
            HtmlTemplate.Append("Phone: " + (!string.IsNullOrEmpty(signUpInfo.Phone) ? signUpInfo.Phone : "--"));
            HtmlTemplate.Append("<br /><br />");
            HtmlTemplate.Append("Password: " + (!string.IsNullOrEmpty(password) ? password : "your current password"));
            HtmlTemplate.Append("<br /><br />");

             string emailbody = HtmlTemplate.ToString();

            Debug.WriteLine(emailbody);
            EmailCommunicationParams emailParams = new EmailCommunicationParams();
            if (LogitudeSettings.DeploymentStage == "Simplog")
            {
                //EmailParameters parameters = new EmailParameters()
                //{
                //    From = "admin@fnarsoft.com",
                //    To = "jalal@logitudeworld.com",
                //    Cc = "Perla@logitudeworld.com;zaki@logitudeworld.com;isra@logitudeworld.com;maram@logitudeworld.com;fajr@logitudeworld.com;Liraz@logitudeworld.com;ahmada@logitudeworld.com;ahmadb@logitudeworld.com;Alex@logitudeworld.com;jinan@amital.co.il;eman@logitudeworld.com",
                //    Bcc = "",
                //    Subject = "SignUp complete successfully for " + signUpInfo.Company,
                //    Body = emailbody,
                //};
                //Debug.WriteLine(parameters.To);
                //EmailingHelper.SendEmail(parameters);


                emailParams = new EmailCommunicationParams()
                {
                    From = "admin@fnarsoft.com",
                    To = "anatl@AMITAL.CO.IL",
                    CC = "Simon@amital.co.il;ohad@AMITAL.CO.IL;chana@amital.co.il;badir@AMITAL.CO.IL;sana@AMITAL.CO.IL;elisheva@AMITAL.CO.IL",
                    BCC = "",
                    Subject = "SignUp complete successfully for " + signUpInfo.Company,
                    EmailBody = emailbody,
                    Tenant = 0,
                    IsBodySecured = true,

                };

            }
            else if (LogitudeSettings.DeploymentStage == "logboxwe1")
            {
                emailParams = GetLogboxEmailCommunicationParams(signUpInfo, emailbody);

            }
            else
            {
                //EmailParameters parameters = new EmailParameters()
                //{
                //    From = "admin@fnarsoft.com",
                //    To = "jalal@logitudeworld.com",//;itzik@amital.co.il;YaronC@AMITAL.CO.IL",
                //    Cc = "ahmada@logitudeworld.com;ahmadb@logitudeworld.com;YaronC@AMITAL.CO.IL;razan@logitudeworld.com",
                //    Bcc = "",
                //    Subject = LogitudeSettings.DeploymentStage+" - SignUp complete successfully for " + signUpInfo.Company,
                //    Body = emailbody,
                //};
                //if (LogitudeSettings.IsCostomsDeploy)
                //{
                //        parameters.To += "eldad@amital.co.il;itzik@amital.co.il;YaronC@AMITAL.CO.IL";
                //}

                //Debug.WriteLine(parameters.To);
                //EmailingHelper.SendEmail(parameters);


                emailParams = new EmailCommunicationParams()
                {
                    From = "admin@fnarsoft.com",
                    To = "anatl@AMITAL.CO.IL",//;itzik@amital.co.il;YaronC@AMITAL.CO.IL",
                    CC = "Simon@amital.co.il;ohad@AMITAL.CO.IL;chana@amital.co.il;badir@AMITAL.CO.IL;sana@AMITAL.CO.IL;elisheva@AMITAL.CO.IL",
                    BCC = "",
                    Subject = LogitudeSettings.DeploymentStage + " - SignUp complete successfully for " + signUpInfo.Company,
                    EmailBody = emailbody,
                    Tenant = 0,
                    IsBodySecured = true,
                };

                if (LogitudeSettings.IsCostomsDeploy)
                {
                    emailParams.To += "anatl@AMITAL.CO.IL";
                }
            }

            Communications.AddEmailCommunicationLogQueue(emailParams, 0);
            Debug.WriteLine("SendEmail:done!!");
        }

        private static EmailCommunicationParams GetLogboxEmailCommunicationParams(SignUpInfoClass signUpInfo, string emailbody)
        {
            string additionalCCEmails = signUpInfo.IsCreateLogboxTenantFromCloud ? signUpInfo.AdditionalEmail : "";
           string additionalBCCEmails = signUpInfo.IsCreateLogboxTenantFromCloud ? "" : "boazelkana@gmail.com";
            return new EmailCommunicationParams()
            {
                From = "admin@fnarsoft.com",
                To = "anatl@AMITAL.CO.IL",
                CC = "Simon@amital.co.il;chana@amital.co.il;badir@AMITAL.CO.IL;sana@AMITAL.CO.IL;elisheva@AMITAL.CO.IL;eyal@AMITAL.CO.IL ",
                BCC = additionalBCCEmails,
                Subject = LogitudeSettings.DeploymentStage + " - SignUp complete successfully for " + signUpInfo.Company,
                EmailBody = emailbody,
                Tenant = 0,
                IsBodySecured = true,
            };
        }

        public override bool OnStart()
        {

            //if (setting.WorkEnvironment == "customs")
            //{
            //    return;
            //}
            if (_OnStartDone) return true;
            _OnStartDone = true;

            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "SignUp";
            DoneItemsInRange = new Dictionary<DateTime, int>();

            storageAccount = StorageAcountDetails.StorageAccount; //CloudStorageAccount.FromConfigurationSetting("DiagnosticsConnectionString");
            queueclient = storageAccount.CreateCloudQueueClient();
            if (!(LogitudeSettings.WorkEnvironment == "customs"))
            {

                queue = queueclient.GetQueueReference("signupqueue");
          
                queue.CreateIfNotExists();

                if (!SuppressClearQ)
                {
                    queue.Clear();
                }///itzik ask :why to .. 

         


            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            //DiagnosticMonitor.Start("DiagnosticsConnectionString");

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.
            RoleEnvironment.Changing += RoleEnvironmentChanging;
            }
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



        public bool SuppressClearQ { get; set; }

        internal void DebugCreateTenant(string email, string Company)
        {

            CreateTenant(new SignUpInfoClass() { Company = Company, Email = email, Name = Company, Phone = "24234" });

        }

        public string Password { get; set; }
    }

    public class SignUpWorkerRoleWinService : Logitude.Server.Tools.WorkerEntryPoint
    {
        SignUpWorkerRole _SignUpWorkerRole;
        public SignUpWorkerRoleWinService()
        {

            _SignUpWorkerRole = new SignUpWorkerRole();
            _SignUpWorkerRole.MillisecondsTimeout = (int)TimeSpan.FromMinutes(1).TotalMilliseconds;

        }

        public string CreatTenant(string email, string Company)
        {
            _SignUpWorkerRole.OnStart();
            _SignUpWorkerRole.DebugCreateTenant(email, Company);
            return _SignUpWorkerRole.Password;
        }

        public override void WorkOnce()
        {
            _SignUpWorkerRole.WorkOnce();
        }
        public void WorkOnceSuppressClearQ()
        {


            _SignUpWorkerRole.SuppressClearQ = true;
            _SignUpWorkerRole.WorkOnce();
        }

        public override void StartMe()
        {
            throw new NotImplementedException();
        }
    }
}
