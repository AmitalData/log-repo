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
using Simplog.Data.CommonDataModel;
using Logitude.Server.Tools;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using WebFreight.Web.WebServices;
using WebFreight.Web.DataContracts;
using Newtonsoft.Json;
using Logitude.Server.Tools.Helpers;

namespace CommunicationWorkerRole
{
    public class AddFileToDropBoxWR : WorkerEntryPoint
    {
        IQueueService queue;
        string path = "/FromLogitude";
        public override async void AsyncRun()
        {
            while (IsRunning)
            {
                if (!General.IsUpdating())
                {
                    queue = new DbQueueService();
                    queue.InitializeQueue("DropBoxCommunicationLogQueue", 0);
                    var response = queue.Receive(new TimeSpan(0, 0, 30));
                    LastActivity = DateTime.UtcNow;
                    if (response != null && response.MessageId != null)
                    {
                        int tenant = 0;
                        string CommunicationLogId = response.MessageValues["CommunicationLogId"].ToString();
                        int.TryParse(response.MessageValues["Tenant"].ToString(), out tenant);
                        try
                        {
                            ICommonDataContext commoncontext = CommonDataContext.GetContext(tenant);
                            DocumentRepository documentRepository = new DocumentRepository(commoncontext);
                            CommunicationLogRepository communicationLogRep = new CommunicationLogRepository(commoncontext);
                            var commLog = communicationLogRep.GetSingleCommunicationLog(CommunicationLogId, tenant);
                            if (commLog.CommunicationStatusTypeCode == "D")
                            {
                                Communications.UpdateCommunicationLogStatus(commLog.Id, tenant, response.MessageId, "D", "Message removed from queue (Communication log status = Done) " + DateTime.Now.ToString(), null);
                                queue.Complete();
                            }
                            else
                            {
                                Document document = documentRepository.GetSingleDocument(tenant, commLog.DocumentId);
                                Uploader uploader = new Uploader();

                                byte[] filedata = uploader.DownloadFile(document.Id, document.Extension, document.Folder, tenant);
                                string FileName = "Random.xml";
                                string newPath = "";
                                if (filedata != null)
                                {
                                    if (!string.IsNullOrEmpty(commLog.LogSettings))
                                    {
                                        var settingsData = JsonConvert.DeserializeObject<CommunicationLogSettings>(commLog.LogSettings);
                                        if (settingsData != null)
                                        {
                                            if (!string.IsNullOrEmpty(settingsData.FolderName))
                                            {
                                                newPath = path + "/" + settingsData.FolderName;
                                            }
                                            FileName = settingsData.FileName;
                                        }
                                    }
                                    DropBoxActionsHelper helper = new DropBoxActionsHelper(tenant);
                                    await helper.Upload(newPath, FileName, filedata);
                                    Communications.UpdateCommunicationLogStatus(commLog.Id, tenant, response.MessageId, "D", "Message removed from queue (Communication log status = Done) " + DateTime.Now.ToString(), null);
                                    queue.Complete();
                                }
                                else
                                {
                                    Communications.UpdateCommunicationLogStatus(commLog.Id, tenant, response.MessageId, "F", "Exception occured while proccessing the queue message " + DateTime.Now.ToString(), "File Not found");
                                    queue.CompleteAsFailed();
                                }

                            }

                            //commLog.MessageLockId = response.MessageId;
                            //communicationLogRep.Update(commLog);
                            //communicationLogRep.SubmitChanges();
                            //TenantAdditionalDataRepository Repo = new TenantAdditionalDataRepository(0);
                            //var Tenants = Repo.GetTenantAdditionalDatas().Where(a => !string.IsNullOrEmpty(a.DropBoxAccessToken));
                            //foreach (var Tenant in Tenants)
                            //{
                            //    DropBoxActionsHelper helper = new DropBoxActionsHelper(Tenant.Id);
                            //    var list = helper.ListFolder(path);

                            //    var firstFile = list.Result.Entries.FirstOrDefault(i => i.IsFile);
                            //    if (firstFile != null)
                            //    {
                            //        var ss = firstFile.PathDisplay;
                            //        //firstFile.AsFile.Id
                            //        var temp = helper.Download(path, firstFile.AsFile);
                            //        SaveMessageToAnalyzeQueue(temp.Result,Tenant.Id);
                            //        var tempdel = helper.DeleteFile(ss);
                            //    }
                            //} 
                            LogDoneItemInMemory();
                           
                        }
                        catch (Exception ex)
                        {
                            #region HandleException
                            ExceptionHandler.HandleException(ex, DateTime.Now, 0, "", "AddFileToDropBoxWR", "", null);
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
            BatchServiceCode = "AddFileToDropBoxWR";
            DoneItemsInRange = new Dictionary<DateTime, int>();
            try
            {
                queue = new DbQueueService();
                queue.InitializeQueue("DropBoxCommunicationLogQueue", 0);

            }

            catch (Exception ex)
            {

                string ip = "";
                if (HttpContext.Current != null && HttpContext.Current.Request != null)
                {
                    ip = HttpContext.Current.Request.UserHostAddress;
                }
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "AddFileToDropBoxWR Role", null, ip);


            }
            return base.OnStart();
        }
    }
}