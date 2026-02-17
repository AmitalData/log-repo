using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
using Microsoft.WindowsAzure.ServiceRuntime;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace CommunicationWorkerRole
{
    public class MobileSMSWorkerRole : WorkerEntryPoint
    {

        List<CommunicationLog> waitingcommlogs;

        public List<CommunicationLog> WaitingCommLogs
        {
            get
            {
                if (waitingcommlogs == null)
                {
                    waitingcommlogs = new List<CommunicationLog>();
                }
                return waitingcommlogs;
            }
            set { waitingcommlogs = value; }
        }
        public override void Run()
        {


            while (IsRunning)
            {

                if (!General.IsUpdating())
                {
                    try
                    {
                        int tenant = 0;
                        queueservice = new DbQueueService();
                        queueservice.InitializeQueue("MobileSMS", 0);

                        var response = queueservice.Receive(new TimeSpan(0, 0, 0, 10));
                        LastActivity = DateTime.UtcNow;

                        if (response.MessageId != null)
                        {

                            string communicationLogId = response.MessageValues["LogId"].ToString();
                            int.TryParse(response.MessageValues["Tenant"].ToString(), out tenant);
                            context = CommonDataContext.GetContext(tenant);
                            CommunicationLogRepository communicationLogRep = new CommunicationLogRepository(context);
                            CommunicationLog cl = communicationLogRep.GetSingleCommunicationLog(communicationLogId, tenant);

                            bool processEnebled = true;

                            if (processEnebled)
                            {
                                if (cl != null)
                                {
                                    if (cl.CommunicationStatusTypeCode == "D")
                                    {
                                        queueservice.Complete();
                                    }
                                    else
                                    {
                                        SendCommunicationLog(communicationLogId, tenant, cl, communicationLogRep);
                                        queueservice.Complete();

                                        LogDoneItemInMemory();

                                    }
                                }
                                else
                                {
                                    if (response.RetryNumber <= 11)
                                    {
                                        if (response.RetryNumber < 3)
                                        {
                                            queueservice.Delay(new TimeSpan(0, 0, 0, 1));
                                        }

                                        if (response.RetryNumber >= 3 && response.RetryNumber <= 5)
                                        {
                                            queueservice.Delay(new TimeSpan(0, 0, 0, 5));
                                        }

                                        if (response.RetryNumber > 5 && response.RetryNumber <= 10)
                                        {

                                            queueservice.Delay(new TimeSpan(0, 0, 0, 10));
                                            AzureLog.SaveLogsInStorage("couldn't find communication log: " + communicationLogId + " ,tenant:" + tenant + ",retry number(DeliveryCount):" + response.RetryNumber
                                                + ",at utc time:" + DateTime.UtcNow + ",at MobileSMS worker role.", "L", DateTime.UtcNow, "", "", 0, null, null, null);
                                            Thread.Sleep(3000);
                                        }
                                        if (response.RetryNumber == 11)
                                        {

                                            queueservice.Delay(new TimeSpan(0, 0, 2, 0));
                                            AzureLog.SaveLogsInStorage("couldn't find communication log: " + communicationLogId + " ,tenant:" + tenant + ",retry number(DeliveryCount):" + response.RetryNumber
                                            + ",at utc time:" + DateTime.UtcNow + ",at MobileSMS worker role.", "L", DateTime.UtcNow, "", "", 0, null, null, null);
                                            Thread.Sleep(10000);

                                        }
                                    }
                                    else
                                    {
                                        queueservice.Complete();
                                        AzureLog.SaveLogsInStorage("couldn't find communication log and the message is completed: " + communicationLogId + " ,tenant:" + tenant + ",retry number(DeliveryCount):" + response.RetryNumber
                                            + ",at utc time:" + DateTime.UtcNow + ",at MobileSMS worker role.", "L", DateTime.UtcNow, "", "", 0, null, null, null);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ConnectClient();
                        ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "MobileSMS worker role start", null, null);
                        Thread.Sleep(10000);
                    }

                }
                else
                {
                    Thread.Sleep(60000);
                }
            }
        }

        public bool IsCommunicationLogProcessEnabled(string communicationLogId, int tenant)
        {
            bool isEnabled = true;
            context = CommonDataContext.GetContext(tenant);
            CommunicationLogRepository communicationLogRep = new CommunicationLogRepository(context);
            CommunicationLog cl = communicationLogRep.GetSingleCommunicationLog(communicationLogId, tenant);


            DateTime requestDate = DateTime.UtcNow;
            if (requestDate < cl.NextTryDateTimeUTC)
            {
                isEnabled = false;

            }

            return isEnabled;
        }
        private void SetNextTryDateTime(CommunicationLog cl)
        {
            string newLog = null;
            DateTime date = TenantServerConfigration.GetCurrentDateTime(cl.Tenant);
            DateTime dateUtc = DateTime.UtcNow;
            switch (cl.Retries)
            {
                case 1:
                case 2:
                    {
                        cl.NextTryDateTime = date.AddSeconds(1);
                        cl.NextTryDateTimeUTC = dateUtc.AddSeconds(1);
                        queueservice.Delay(new TimeSpan(0, 0, 0, 1));

                        break;
                    }
                case 3:
                case 4:
                    {
                        cl.NextTryDateTime = date.AddSeconds(5);
                        cl.NextTryDateTimeUTC = dateUtc.AddSeconds(5);
                        queueservice.Delay(new TimeSpan(0, 0, 0, 5));
                        break;
                    }
                case 5:
                    {
                        cl.NextTryDateTime = date.AddMinutes(1);
                        cl.NextTryDateTimeUTC = dateUtc.AddMinutes(1);
                        queueservice.Delay(new TimeSpan(0, 0, 1));
                        break;
                    }
                case 6:
                case 7:
                case 8:
                    {
                        cl.NextTryDateTime = date.AddMinutes(2);
                        cl.NextTryDateTimeUTC = dateUtc.AddMinutes(2);
                        queueservice.Delay(new TimeSpan(0, 0, 2));
                        break;
                    }
                case 9:
                    {
                        cl.NextTryDateTime = date.AddMinutes(5);
                        cl.NextTryDateTimeUTC = dateUtc.AddMinutes(5);
                        queueservice.Delay(new TimeSpan(0, 0, 5));
                        break;
                    }
                case 10:
                    {
                        cl.NextTryDateTime = date.AddMinutes(10);
                        cl.NextTryDateTimeUTC = dateUtc.AddMinutes(10);
                        queueservice.Delay(new TimeSpan(0, 0, 10));
                        break;
                    }
                default:
                    {
                        cl.NextTryDateTime = date.AddMinutes(20);
                        cl.NextTryDateTimeUTC = dateUtc.AddMinutes(20);
                        queueservice.Delay(new TimeSpan(0, 0, 20));
                        break;
                    }
            }
            cl.Logs += Environment.NewLine + "Retry #" + cl.Retries + " Next Retry: " + cl.NextTryDateTimeUTC.ToString();
        }

        #region SendCommunicationLog

        ICommonDataContext context;
        private void SendCommunicationLog(string communicationLogId, int tenant, CommunicationLog cl, CommunicationLogRepository communicationLogRep)
        {


            try
            {
                if (cl.Retries < 5)
                {
                    SendWaitingCommunicationLog(cl, communicationLogRep);
                }

                else
                {
                    if (context != null)
                    {
                        cl.CommunicationStatusTypeCode = "F";
                        cl.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(cl.Tenant);
                        communicationLogRep.Update(cl);
                        communicationLogRep.SubmitChanges();
                    }

                }
            }

            catch (Exception exc)
            {
                ExceptionHandler.HandleException(exc, DateTime.Now, tenant, "", "WorkerRole", "", null);

                //Change number of retries

                cl.Retries++;
                cl.ExceptionMessage = exc.Message;
                if (exc.InnerException != null)
                {
                    cl.ExceptionMessage = cl.ExceptionMessage + Environment.NewLine + exc.InnerException;
                }
                if (exc.StackTrace != null)
                {
                    cl.ExceptionMessage = cl.ExceptionMessage + Environment.NewLine + "Stack trace: " + exc.StackTrace;
                }
                SetNextTryDateTime(cl);
                if (context != null)
                {
                    communicationLogRep.Update(cl);
                    communicationLogRep.SubmitChanges();
                }
                throw;

            }
        }

        private void SendWaitingCommunicationLog(CommunicationLog waitingCommLog, CommunicationLogRepository communicationLogRep)
        {

            TwoFactorAuthenticationDeviceRepository deviceRepository = new TwoFactorAuthenticationDeviceRepository(communicationLogRep.context);
            TwoFactorAuthenticationDevice device = deviceRepository.GetSingleTwoFactorAuthenticationDevice(waitingCommLog.EntityId, waitingCommLog.Tenant);

            // Find your Account Sid and Auth Token at twilio.com/console
            //const string accountSidTest = "AC19b9227f70b180a6014bd0d0398e90b4";
            //const string authTokenTest = "dd080709fbf4414052fc282fa6984825";

            //const string accountSIdProd = "AC40f68a3b2fbc2c260c0ad083fcb1cd5f";
            //const string authTokenProd = "892d80404b437513fe5ef47d0cba5527";

            if (!string.IsNullOrEmpty(LogitudeSettings.SMSServiceUserId) && !string.IsNullOrEmpty(LogitudeSettings.SMSServiceAuthToken))
            {
                TwilioClient.Init(LogitudeSettings.SMSServiceUserId, LogitudeSettings.SMSServiceAuthToken);

                string filename = waitingCommLog.DocumentId + "." + waitingCommLog.Document.Extension;
                Logitude.Server.Tools.BlobFileInfo fileInfo = new Logitude.Server.Tools.BlobFileInfo()
                {
                    FileName = waitingCommLog.Document.Id,
                    FolderName = waitingCommLog.Document.Folder,
                    Extension = waitingCommLog.Document.Extension,
                    Tenant = waitingCommLog.Document.Tenant,
                    FileSize = waitingCommLog.Document.FileSize,
                };
                Logitude.Server.Tools.StorageService.IBlobService storageservice = Logitude.Server.Tools.ContainerAccessor.Container.Resolve(typeof(Logitude.Server.Tools.StorageService.IBlobService), "StorageService", new Microsoft.Practices.Unity.ParameterOverride("", 1)) as Logitude.Server.Tools.StorageService.IBlobService;
                byte[] messageFileByte = storageservice.Read(fileInfo);
                string messageBody = Encoding.ASCII.GetString(messageFileByte);

                //var mediaUrl = new List<Uri>() {
                //  new Uri( "" )
                //};
                var to = new PhoneNumber(waitingCommLog.To);//("+970569393003");//(" + 970598137715"); //("+970569393003");
                var message = MessageResource.Create(
                  to,
                  from: new PhoneNumber(LogitudeSettings.SMSServicePhoneNumber),//"+16177122827"),//+15005550006
                  body: messageBody

                 );



                waitingCommLog.CommunicationStatusTypeCode = "D";
                waitingCommLog.DoneDate = TenantServerConfigration.GetCurrentDateTime(waitingCommLog.Tenant);
                waitingCommLog.DoneDateUTC = DateTime.UtcNow;
            }
            else
            {
                waitingCommLog.CommunicationStatusTypeCode = "F";
                waitingCommLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(waitingCommLog.Tenant);
                waitingCommLog.LastStatusDate = DateTime.UtcNow;
                waitingCommLog.ExceptionMessage = "Invalid SMS Service Credentials";
            }

            waitingCommLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(waitingCommLog.Tenant);
            waitingCommLog.LastStatusDateUTC = DateTime.UtcNow;

            communicationLogRep.Update(waitingCommLog);
            communicationLogRep.SubmitChanges();

        }

        #endregion

   
    
        public override bool OnStart()
        {

            ConnectClient(); // mohammad to try reconnect in case of disconnected client. 23-7-15
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;
            //ThreadId = Thread.CurrentThread.ManagedThreadId.ToString();


            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "MobileSMS";
            DoneItemsInRange = new Dictionary<DateTime, int>();

            //DiagnosticMonitor.Start("DiagnosticsConnectionString");

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.
            RoleEnvironment.Changing += RoleEnvironmentChanging;

            return base.OnStart();
        }
        IQueueService queueservice;
        public void ConnectClient()
        {
            try
            {


                queueservice = new DbQueueService();
                queueservice.InitializeQueue("MobileSMS", 0);

            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Connect client method", null, null);
            }
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
