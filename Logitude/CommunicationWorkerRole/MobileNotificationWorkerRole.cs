using Logitude.SystemLogs;
using Microsoft.Azure.NotificationHubs;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure.ServiceRuntime;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace CommunicationWorkerRole
{
    public class MobileNotificationWorkerRole : WorkerEntryPoint
    {

        QueueDescription queueDescription;
        QueueClient client;
        private bool _OnStartDone;
        string mobilenotificationlogQueueName;

        string tenantName = "";
        public override void Run()
        {
            int? tenant = null;
            string notificationId = null;
            while (IsRunning)
            {
                try
                {
                    int sleeptime = 2000;
                    var message = client.Receive(new TimeSpan(0, 0, 10));
                    LastActivity = DateTime.UtcNow;
                    if (message != null)
                    {
                        try
                        {
                            if (message.Properties.Keys.Contains("NotificationId"))  notificationId = message.Properties["NotificationId"].ToString();
                            if (message.Properties.Keys.Contains("Tenant"))  tenant = (int)message.Properties["Tenant"];
                            if (message.Properties.Keys.Contains("TenantName")) tenantName = message.Properties["TenantName"].ToString();

                            if (notificationId == null || tenant == null)
                            {
                                message.Complete();
                                
                                continue;
                            }

                            MobileNotificationLogRepository mobileNotificationLogRepository = new MobileNotificationLogRepository();
                            MobileNotificationLog mobileNotificationLog = mobileNotificationLogRepository.GetSingleMobileNotificationLogBuIdAndTenant(notificationId, tenant);//GetFirstNotCompletedMobileNotificationLog();

                            if (mobileNotificationLog == null) { message.Complete(); continue; }
                           
                            ContactMobileDeviceRepository contactMobileDeviceRepository = new ContactMobileDeviceRepository();

                            List<ContactMobileDevice> contactMobileDeviceLists = contactMobileDeviceRepository.GetAllDeviceByEmail(mobileNotificationLog.Email);

                            if (contactMobileDeviceLists.Count == 0)
                            {
                                mobileNotificationLog.AndroidStatus = "D";
                                mobileNotificationLog.IOSStatus = "D";
                                mobileNotificationLogRepository.Update(mobileNotificationLog);
                                mobileNotificationLogRepository.SubmitChanges();
                                message.Complete();
                                continue;
                            }

                            List<ContactMobileDevice> iosDeviceList = contactMobileDeviceLists.Where(d => d.Platform == "IOS").ToList();
                            List<ContactMobileDevice> androidDeviceList = contactMobileDeviceLists.Where(d => d.Platform == "Android").ToList();


                            if (androidDeviceList.Count == 0 && mobileNotificationLog.AndroidStatus == "W") mobileNotificationLog.AndroidStatus = "D";
                            if (iosDeviceList.Count == 0 && mobileNotificationLog.IOSStatus == "W") mobileNotificationLog.IOSStatus = "D";


                            #region Send to  IOS Device

                            int badgeNumber = mobileNotificationLogRepository.GetNotificationCountByEmail(mobileNotificationLog.Email);
                            if (iosDeviceList.Count > 0 && mobileNotificationLog.IOSStatus == "W")
                            {
                                List<List<string>> Tags = GetTagsMobileDevice(iosDeviceList);
                                int temp = 0;
                                int Count = Tags.Count;
                                foreach (List<string> tag in Tags)
                                {
                                    temp++;
                                   bool isEnd = false;
                                    while (!isEnd)
                                    {
                                        if (temp!= Count)isEnd = true;
                                        Task[] tasks = { SendNotificationForIOSAsync(tag, mobileNotificationLog, badgeNumber, tenantName) };
                                        isEnd = ComplateTask(tasks, mobileNotificationLog, temp, Count, isEnd, "Before sending notification to ios registered devices", "ios");
                                    }
                                }
                            }

                            #endregion

                            #region Send to  Android Device

                            if (androidDeviceList.Count > 0 && mobileNotificationLog.AndroidStatus == "W")
                            {
                                List<List<string>> Tags = GetTagsMobileDevice(androidDeviceList);

                                int temp = 0;
                                int Count = Tags.Count;//androidDeviceList.Count / 20;
                                foreach (List<string> tag in Tags)
                                {
                                    temp++;
                                    bool isEnd = false;
                                    while (!isEnd)
                                    {
                                        if (temp != Count) isEnd = true;
                                        Task[] tasks = { SendNotificationForAndroidAsync(tag, mobileNotificationLog, badgeNumber, tenantName) };
                                        isEnd = ComplateTask(tasks, mobileNotificationLog, temp, Count, isEnd, "Before sending notification to android registered devices", "android");
                                    }
                                }

                            }


                            #endregion


                            mobileNotificationLogRepository.Update(mobileNotificationLog);
                            mobileNotificationLogRepository.SubmitChanges();

                            message.Complete();
                            LogDoneItemInMemory();
                        }

                        catch (Exception ex)
                        {
                            ExceptionHandler.HandleException(ex, DateTime.Now, (int)tenant, "", "MobileNotificationLogWorkerRole", "", null);
                            DelayQueueMessage(message);
                        }


                    }



                }

                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "mobile notification worker role start", null, null);
                    Thread.Sleep(10000);
                }
            }

        }

        private static Task<NotificationOutcome> SendNotificationForAndroidAsync(List<string> tagsList, MobileNotificationLog mobileNotificationLog, int badgeNumber ,string tenantName)
        {
         
            string gcmMessage = "{\"data\":{\"msg\":\"" + mobileNotificationLog.NotificationMessageAndroid + "\",  \"EntitiyId\":\"" + mobileNotificationLog.EntityId + "\"  , \"NotifiyId\":\"" + mobileNotificationLog.Id + "\"  , \"Tenant\":\"" + mobileNotificationLog.Tenant.ToString() + "\" , \"badge\":\"" + badgeNumber.ToString() + "\"  , \"title\":\"" + tenantName + "\" }}";
            NotificationHubClient hub = NotificationHubClient.CreateClientFromConnectionString(LogitudeSettings.NotificationHubConnectionString, LogitudeSettings.NotificationHubName);
            Task<NotificationOutcome> result = hub.SendGcmNativeNotificationAsync(gcmMessage, tagsList);

            return result;

        }

        private static Task<NotificationOutcome> SendNotificationForIOSAsync( List<string> tagsList ,  MobileNotificationLog mobileNotificationLog , int badgeNumber,string tenantName)
        {

            string NotificationMsg = mobileNotificationLog.NotificationMessageIOS.Replace("\n", "\\n");
            string apnsMessage = "{\"aps\":{\"alert\":\"" + NotificationMsg + "\"  , \"sound\":\"" + "default" + "\" ,  \"badge\":" + badgeNumber + " ,   \"EntitiyId\":\"" + mobileNotificationLog.EntityId + "\",  \"NotifiyId\":\"" + mobileNotificationLog.Id + "\" ,  \"Tenant\":\"" + mobileNotificationLog.Tenant.ToString() + "\"  }}";
            NotificationHubClient hub = NotificationHubClient.CreateClientFromConnectionString(LogitudeSettings.NotificationHubConnectionString, LogitudeSettings.NotificationHubName);
            Task<NotificationOutcome> result = hub.SendAppleNativeNotificationAsync(apnsMessage, tagsList);
           
            return result;
        }

        public override bool OnStart()
        {
            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "MobileNotification";
            DoneItemsInRange = new Dictionary<DateTime, int>();

            try
            {
                mobilenotificationlogQueueName = ThreadedRoleEntryPoint.GetQueueByEnviroment("mobilenotificationlogqueue");
                if (!StorageAcountDetails.NameSpaceManager.QueueExists(mobilenotificationlogQueueName))
                {
                    queueDescription = new QueueDescription(mobilenotificationlogQueueName);
                    queueDescription.MaxSizeInMegabytes = 5120;
                    queueDescription.EnableDeadLetteringOnMessageExpiration = false;
                    StorageAcountDetails.NameSpaceManager.CreateQueue(queueDescription);
                }

                client = StorageAcountDetails.CreateServiceBusQueueClient(mobilenotificationlogQueueName);
            }
            catch (Exception ex)
            {
                //  ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "email worker role start", null, null);
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

        private static void DelayQueueMessage(BrokeredMessage message)
        {
            if (message.DeliveryCount < 11)
            {
                if (message.DeliveryCount >= 3 && message.DeliveryCount <= 5)
                {
                    //    Thread.Sleep(new TimeSpan(0, 0, 10));
                    message.ScheduledEnqueueTimeUtc = DateTime.UtcNow.AddSeconds(5);
                }

                if (message.DeliveryCount > 5 && message.DeliveryCount <= 10)
                {
                    //Thread.Sleep(new TimeSpan(0, 0, 30));
                    message.ScheduledEnqueueTimeUtc = DateTime.UtcNow.AddSeconds(10);
                }
                if (message.DeliveryCount == 11)
                {
                    message.ScheduledEnqueueTimeUtc = DateTime.UtcNow.AddSeconds(60);
                }
                message.Abandon();
            }
            else
            {
                // add error log
                message.Complete();
            }
        }

        private string GetExceptionMessage(Exception exc)
        {
            string exception = "";
            if (exc != null)
            {
                exception = exc.Message;
                if (exc.InnerException != null)
                {
                    exception = exception + Environment.NewLine + exc.InnerException;
                }
                if (exc.StackTrace != null)
                {
                    exception = exception + Environment.NewLine + "Stack trace: " + exc.StackTrace;
                }
            }

            return exception;
        }


        private List<List<string>> GetTagsMobileDevice(List<ContactMobileDevice> deviceList)
        {
            List<List<string>> Tags = new List<List<string>>();

            if (deviceList != null)
            {
                int i = 0; int j = 20;

                if (deviceList.Count < 20) j = deviceList.Count;

                List<string> mobileTags = new List<string>();
                foreach (ContactMobileDevice device in deviceList)
                {
                    if (i < j)
                    {
                        mobileTags.Add(device.NotificationUniqueKey);
                        i++;
                    }
                    else
                    {

                        Tags.Add(mobileTags);
                        mobileTags = new List<string>();
                        mobileTags.Add(device.NotificationUniqueKey);
                        i = 1;
                        if (deviceList.Count - 20 >= 20) j = 20;
                        else j = deviceList.Count - 20;
                    }
                }

                if (mobileTags.Count > 0) Tags.Add(mobileTags);
            }
            return Tags;
        }


        private bool  ComplateTask(Task[] tasks , MobileNotificationLog mobileNotificationLog , int temp, int count ,bool isEnd,string logstring,string type)
        {
            bool result = isEnd;
            Task.WaitAll(tasks);
            Task task = tasks.FirstOrDefault();
            var isFaulted = task.IsFaulted;
            if (temp == count)
            {
                if (isFaulted)
                {
                    if (type == "ios") mobileNotificationLog.NumberOfRetriesIOS++;
                    else mobileNotificationLog.NumberOfRetriesAndroid++;

                    if ((mobileNotificationLog.NumberOfRetriesIOS >= 5 && type == "ios") || (mobileNotificationLog.NumberOfRetriesAndroid >= 5 && type == "android"))
                    {
                        if (type == "ios") mobileNotificationLog.IOSStatus = "F";
                        else mobileNotificationLog.AndroidStatus = "F";

                        if (task.Exception != null)
                        {
                            string exceptionMessage = task.Exception.ToString(); 

                            if (task.Exception.InnerException != null)
                            {
                                exceptionMessage = exceptionMessage + Environment.NewLine + task.Exception.InnerException.ToString();
                            }
                            if (task.Exception.StackTrace != null)
                            {
                                exceptionMessage = exceptionMessage + Environment.NewLine + "Stack trace: " + task.Exception.StackTrace.ToString();
                            }

                            mobileNotificationLog.Exception = exceptionMessage;
                        }
                        result = true;
                    }
                }
                else
                {
                    if(type == "ios") mobileNotificationLog.IOSStatus = "D";
                    else mobileNotificationLog.AndroidStatus = "D";

                    result = true;
                }

                if (result)
                {
                    logstring += Environment.NewLine + "After sending notification to " + type +" registered devices";
                    mobileNotificationLog.DoneDate = DateTime.UtcNow;
                    mobileNotificationLog.Log = logstring;

                }
            }

            return result;
        }




    }
}
