using Logitude.Server.Tools.Counters;
using Logitude.SystemLogs;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure.ServiceRuntime;
using Simplog.Data.Helpers;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure;
using WebFreight.Web.Helpers;
using Logitude.Server.Tools.QueueService;
using System.Web;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.Server.Tools.Helpers;

namespace CommunicationWorkerRole
{
    public class AddDropBoxFileToAnalyzeQueueWR : WorkerEntryPoint
    {
        IQueueService queue;
        DateTime StartDate = DateTime.Now;
        string path = "/ToLogitude";
        public override async void AsyncRun()
        {
            StartDate = DateTime.Now;
            while (IsRunning)
            {
                if (!General.IsUpdating())
                {
                    queue = new DbQueueService();
                    queue.InitializeQueue("DrobBoxQueue", 0);
                    var response = queue.Receive(new TimeSpan(0, 0, 10));
                    LastActivity = DateTime.UtcNow;
                    int Tenant = 0;
                    if (response != null && response.MessageId != null)
                    {
                        int.TryParse(response.MessageValues["Tenant"], out Tenant);
                        try
                        {
                            ProcessComingQueues(Tenant);
                            queue.Complete();
                            LogDoneItemInMemory();
                            //if (subscriptionClient != null)
                            //{

                            //    message = subscriptionClient.Receive(new TimeSpan(0, 0, 30));
                            //    LastActivity = DateTime.UtcNow;
                            //    if (message != null)
                            //    {
                            //        string messageData = message.Properties["MessageData"].ToString();
                            //        SaveMessageToAnalyzeQueue(messageData);
                            //        message.Complete();
                            //        
                            //    }
                            //}


                        }
                        catch (Exception ex)
                        {
                            #region HandleException
                            ExceptionHandler.HandleException(ex, DateTime.Now, 0, "", "AddDropBoxFileToAnalyzeQueueWR", "", null);
                            if (response.RetryNumber <= 1)
                            {
                                queue.Delay(new TimeSpan(0, 0, 0, 5));
                            }

                            if (response.RetryNumber > 1 && response.RetryNumber <= 2)
                            {
                                queue.Delay(new TimeSpan(0, 0, 0, 10));
                            }
                            if (response.RetryNumber >= 3)
                            {
                                queue.CompleteAsFailed();
                            }

                            #endregion
                        }
                    }
                    TimeSpan span = (DateTime.Now).Subtract(StartDate);
                    if (span.Minutes >= 10)
                    {
                        queue.InitializeQueue("DrobBoxQueue", 0);
                        queue.Send(new Dictionary<string, string>() { { "Tenant", "0" } }, null, null);
                        StartDate = DateTime.Now;
                    }
                }
                else
                {
                    Thread.Sleep(60000);
                }
            }

        }

        private void SaveMessageToAnalyzeQueue(byte[] messageData,int tenant)
        {
            AnalyzeQueueRepository analyzeQueueReposiory = new AnalyzeQueueRepository();
           // byte[] messageBytes = Encoding.ASCII.GetBytes(messageData);

            AnalyzeQueue analyzeQueue = new AnalyzeQueue()
            {
                CreateDate = TenantServerConfigration.GetCurrentDateTime(0),
                From = "DropBox",
                Id = IdCounter.GetNumber("AnalyzeQueue", 0),
                MessageBody = messageData,
                Status = "W",
                Retries = 0,
                ConnectedToEntity = false,
                ConnectedToTenant = true,
                Tenant = tenant,
                FileSize = messageData.Length,
            };

            analyzeQueue.SearchFields = analyzeQueue.From + ',' + analyzeQueue.Status;
            analyzeQueueReposiory.Add(analyzeQueue);
            analyzeQueueReposiory.SubmitChanges();
        }

        private async void ProcessComingQueues(int Tenant)
        {
            TenantAdditionalDataRepository Repo = new TenantAdditionalDataRepository(0);
            if (Tenant == 0)
            {
                var Tenants = Repo.GetTenantAdditionalDatas().Where(a => !string.IsNullOrEmpty(a.DropBoxAccessToken));
                foreach (var tenant in Tenants)
                {
                    DropBoxActionsHelper helper = new DropBoxActionsHelper(tenant.Tenant);
                    var list = helper.ListFolder(path);
                    if (list != null)
                    {
                        try
                        {
                            if (list != null && list.Result != null && list.Result.Entries != null)
                            {
                                var firstFile = list.Result.Entries.FirstOrDefault(i => i.IsFile);
                                if (firstFile != null)
                                {
                                    var ss = firstFile.PathDisplay;
                                    //firstFile1.AsFile.Id
                                    var temp = await helper.Download(path, firstFile.AsFile);
                                    SaveMessageToAnalyzeQueue(temp, tenant.Tenant);
                                    var tempdel = helper.DeleteFile(ss);
                                }
                            }
                           
                        }
                        catch (Exception ex)
                        {

                            
                        } 
                    }
                   
                }
            }
            else
            {
                
                DropBoxActionsHelper helper = new DropBoxActionsHelper(Tenant);
                var list = helper.ListFolder(path);

                var firstFile = list.Result.Entries.FirstOrDefault(i => i.IsFile);
                if (firstFile != null)
                {
                    var ss = firstFile.PathDisplay;
                    //firstFile.AsFile.Id
                    var temp = await helper.Download(path, firstFile.AsFile);
                    SaveMessageToAnalyzeQueue(temp, Tenant);
                    var tempdel = helper.DeleteFile(ss);
                }
               
            }

        }

        public override bool OnStart()
        {

            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "DropBoxFileToAnalyzeQueue";
            DoneItemsInRange = new Dictionary<DateTime, int>();
            try
            {
                queue = new DbQueueService();
                queue.InitializeQueue("DrobBoxQueue", 0);

            }

            catch (Exception ex)
            {

                string ip = "";
                if (HttpContext.Current != null && HttpContext.Current.Request != null)
                {
                    ip = HttpContext.Current.Request.UserHostAddress;
                }
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "DropBoxFileToAnalyzeQueue Role", null, ip);


            }
            return base.OnStart();
        }
    }
}