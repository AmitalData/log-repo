using Logitude.Server.Tools.QueueService;
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

        IQueueService queueservice;

        public override void Run()
        {
            int? tenant = null;
            string notificationId = null;
            string tenantName = "";

            while (IsRunning)
            {
                if (!General.IsUpdating())
                {
                    try
                    {
                        queueservice = new DbQueueService();
                        queueservice.InitializeQueue("mobilenotificationlogqueue", 0);
                        var response = queueservice.Receive(new TimeSpan(0, 0, 10));
                        
                        if (response != null && response.MessageId != null)
                        {
                            try
                            {
                                notificationId = response.MessageValues.Keys.Contains("NotificationId") ? response.MessageValues["NotificationId"].ToString() : "";
                                tenantName = response.MessageValues.Keys.Contains("TenantName") ? response.MessageValues["TenantName"].ToString() : "";
                                tenant = response.MessageValues.Keys.Contains("Tenant") && !string.IsNullOrEmpty(response.MessageValues["Tenant"].ToString()) ? (int?)int.Parse(response.MessageValues["Tenant"].ToString()) : null;


                                if (notificationId == null || tenant == null)
                                {
                                    queueservice.Complete();

                                    continue;
                                }

                                MobileNotificationLogRepository mobileNotificationLogRepository = new MobileNotificationLogRepository();
                                MobileNotificationLog mobileNotificationLog = mobileNotificationLogRepository.GetSingleMobileNotificationLogBuIdAndTenant(notificationId, tenant);//GetFirstNotCompletedMobileNotificationLog();

                                if (mobileNotificationLog == null) { queueservice.Complete(); continue; }

                                ContactMobileDeviceRepository contactMobileDeviceRepository = new ContactMobileDeviceRepository();

                                List<ContactMobileDevice> contactMobileDeviceLists = contactMobileDeviceRepository.GetAllDeviceByEmail(mobileNotificationLog.Email);

                                if (contactMobileDeviceLists.Count == 0)
                                {
                                    mobileNotificationLog.AndroidStatus = "D";
                                    mobileNotificationLog.IOSStatus = "D";
                                    mobileNotificationLogRepository.Update(mobileNotificationLog);
                                    mobileNotificationLogRepository.SubmitChanges();
                                    queueservice.Complete();
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
                                            if (temp != Count) isEnd = true;
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


                                queueservice.Complete();
                                LogDoneItemInMemory();
                            }

                            catch (Exception ex)
                            {
                                ExceptionHandler.HandleException(ex, DateTime.Now, (int)tenant, "", "MobileNotificationWorkerRole", "", null);
                                #region HandleException
                                if (response.MessageValues.Keys.Contains("NotificationId"))
                                {
                                    if (response.RetryNumber <= 1)
                                    {
                                        queueservice.Delay(new TimeSpan(0, 0, 0, 5));
                                    }

                                    if (response.RetryNumber > 1 && response.RetryNumber <= 2)
                                    {
                                        queueservice.Delay(new TimeSpan(0, 0, 0, 10));
                                    }
                                    if (response.RetryNumber >= 3)
                                    {
                                        queueservice.CompleteAsFailed();
                                    }
                                }
                                else
                                {
                                    queueservice.CompleteAsFailed();
                                }
                                #endregion

                            }

                        }
                        else
                        {
                            Thread.Sleep(10000);
                        }

                    }
                    catch (Exception ex)
                    {
                        ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "mobile notification worker role start", null, null);
                        Thread.Sleep(10000);

                    }
                }
                else
                {
                    Thread.Sleep(60000);
                }
            }
        }

        private static Task<NotificationOutcome> SendNotificationForAndroidAsync(List<string> tagsList, MobileNotificationLog mobileNotificationLog, int badgeNumber ,string tenantName)
        {
         
            string gcmMessage = "{\"data\":{\"msg\":\"" + mobileNotificationLog.NotificationMessageAndroid + "\",  \"EntitiyId\":\"" + mobileNotificationLog.EntityId + "\"  , \"NotifiyId\":\"" + mobileNotificationLog.Id + "\"  , \"Tenant\":\"" + mobileNotificationLog.Tenant.ToString() + "\" , \"badge\":\"" + badgeNumber.ToString() + "\"  , \"title\":\"" + tenantName + "\" }}";
            NotificationHubClient hub = NotificationHubClient.CreateClientFromConnectionString(LogitudeSettings.NotificationHubConnectionString, LogitudeSettings.NotificationHubName);
            Task<NotificationOutcome> result = hub.SendFcmNativeNotificationAsync(gcmMessage, tagsList);

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


        public override bool OnStart()
        {
            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "MobileNotification";
            DoneItemsInRange = new Dictionary<DateTime, int>();
            ConnectClient();
            return base.OnStart();
        }

        private void ConnectClient()
        {
            try
            {
                queueservice = new DbQueueService();
                queueservice.InitializeQueue("mobilenotificationlogqueue", 0);
            }

            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Mobile Notification worker role start", null, null);
            }
        }

    }
}
