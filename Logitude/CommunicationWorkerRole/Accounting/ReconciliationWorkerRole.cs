using Logitude.SystemLogs;
using System;
using System.Collections.Generic;
using System.Threading;
using Logitude.Server.Tools.QueueService;
using System.Web;
using Logitude.Accounting.BL.CoreBL;
using Logitude.Server.Tools.Helpers;
using Newtonsoft.Json;
using Logitude.Accounting.Def.EntityPMs;
using System.Transactions;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.Helpers;
using Microsoft.Practices.Unity;
using Logitude.Server.Tools;

namespace CommunicationWorkerRole
{
    public class ReconciliationWorkerRole : WorkerEntryPoint
    {
        IQueueService queue;
        public override void Run()
        {
            try
            {
                while (IsRunning)
                {
                    if (!General.IsUpdating())
                    {
                        Do();
                    }
                    else
                    {
                        Thread.Sleep(2000);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "ReconciliationWorkerRole  worker role start", null, null);
                Thread.Sleep(2000);
            }
        }

        public override bool OnStart()
        {
            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "ReconciliationWorkerRole";
            DoneItemsInRange = new Dictionary<DateTime, int>();

            try
            {
                ConnectClient();
            }
            catch (Exception ex)
            {
                string ip = "";
                if (HttpContext.Current != null && HttpContext.Current.Request != null)
                {
                    ip = HttpContext.Current.Request.UserHostAddress;
                }
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "CargoTrackingImporterApprovalReceivedQueue Role", null, ip);
            }
            return base.OnStart();
        }

        private void Do()
        {
            ConnectClient();
            var response = queue.Receive();
            LastActivity = DateTime.UtcNow;
            int tenant = 0;
            if (response == null || response.MessageId == null)
            {
                Thread.Sleep(2000);
                return;
            }
            string communicationLogId = response.MessageValues["communicationLogId"].ToString();
            int.TryParse(response.MessageValues["tenant"].ToString(), out tenant);
            ICommonDataContext context = CommonDataContext.GetContext(tenant);
            CommunicationLogRepository communicationLogRep = new CommunicationLogRepository(context);
            CommunicationLog commLog = communicationLogRep.GetSingleCommunicationLog(communicationLogId, tenant);
            if (commLog != null)
            {
                try
                {
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        var recoCallback = CreateReconciliation(response, tenant, commLog);
                        var recoCallbackJson = JsonConvert.SerializeObject(recoCallback);
                        commLog.CommunicationStatusTypeCode = "D";
                        commLog.DoneDate = TenantServerConfigration.GetCurrentDateTime(commLog.Tenant);
                        commLog.DoneDateUTC = DateTime.UtcNow;
                        commLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(commLog.Tenant);
                        commLog.LastStatusDateUTC = DateTime.UtcNow;
                        commLog.AdditionalFields = recoCallbackJson;
                        communicationLogRep.Update(commLog);
                        communicationLogRep.SubmitChanges();
                        scope.Complete();
                    }
                    queue.Complete();
                    LogDoneItemInMemory();
                }
                catch (Exception ex)
                {
                    Communications.UpdateCommunicationLogStatus(commLog.Id, tenant, null, "F", null + DateTime.Now.ToString(), ex.Message);
                    queue.Complete();
                }
            }
        }

        private RecoCallback CreateReconciliation(QueueResponse response, int tenant, CommunicationLog commLog)
        {
            BlobFileInfo fileInfo = new BlobFileInfo()
            {
                FileName = commLog.Document.Id,
                FolderName = commLog.Document.Folder,
                Extension = commLog.Document.Extension,
                Tenant = commLog.Document.Tenant,
                FileSize = commLog.Document.FileSize,
            };
            Logitude.Server.Tools.StorageService.IBlobService storageservice = Logitude.Server.Tools.ContainerAccessor.Container.Resolve(typeof(Logitude.Server.Tools.StorageService.IBlobService), "StorageService", new ParameterOverride("", 1)) as Logitude.Server.Tools.StorageService.IBlobService;
            byte[] objectData = storageservice.Read(fileInfo);
            var jsonObject = System.Text.Encoding.Default.GetString(objectData);
            var reconciliationPM = JsonConvert.DeserializeObject<ReconciliationPM>(jsonObject);
            CreateReconciliationService recoService = new CreateReconciliationService();
            RecoCallback recoCallback = recoService.CreateReconciliation(reconciliationPM);
            return recoCallback;
        }

        private void ConnectClient()
        {
            try
            {
                queue = new DbQueueService();
                queue.InitializeQueue("ReconciliationWorkerRole", 0);

            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "ReconciliationWorkerRole worker role start", null, null);
            }
        }

    }
}
