using Devart.Common;
using Logitude.BL.CommonDataModel.EntityAMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.GlobalModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.BL.ShipmentsModel.EntityAMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
using Newtonsoft.Json;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure.Azure;
using Microsoft.Practices.Unity;
using Logitude.Server.Tools.StorageService;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.GlobalModel.EntityPMs;

namespace CommunicationWorkerRole
{
    public class CargoTrackingImporterApprovalReceivedWR : WorkerEntryPoint
    {
        IQueueService queue;
        ICommonDataContext commonContext;
        public CargoTrackingImporterApprovalReceivedWR()
        {
        }
        public override bool OnStart()
        {


            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "CargoTrackingImporterApprovalReceived";
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

        public override void Run()
        {
            try
            {
                while (IsRunning)
                {
                    if (!General.IsUpdating())
                    {
                        DoCargoTrackingImporterApprovalReceived();
                    }
                    else
                    {
                        Thread.Sleep(60000);
                    }
                }
            }
            catch (Exception ex)
            {
                ConnectClient();
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "CargoTrackingImporterApprovalReceived  worker role start", null, null);
                Thread.Sleep(10000);
            }
        }

        private void DoCargoTrackingImporterApprovalReceived()
        {
            ConnectClient();
            var response = queue.Receive();
            LastActivity = DateTime.UtcNow;
            int tenant = 0;
            if (response == null || response.MessageId == null)
            {
                Thread.Sleep(10000);
                return;
            }
            try
            {
                InitializeContext(tenant);
                AddApprovalReceivedTask(response, tenant);
                queue.Complete();
                LogDoneItemInMemory();
            }
            catch (Exception ex)
            {
                HandelException(ex, tenant, response);
            }
        }
        private void InitializeContext(int tenant)
        {
            commonContext = CommonDataContext.GetContext(tenant);
        }

        private void AddApprovalReceivedTask(QueueResponse response, int tenant)
        {
            if (!CheckIfShipmentIdExist(response))
                return;

            string ShipmentId = response.MessageValues["ShipmentId"].ToString();
            int.TryParse(response.MessageValues["Tenant"], out tenant);
            if (!IsCargoTrackingApprovalActive(tenant))
                return;
            var shipmentAdditionalCloudData = GetShipmentAdditionalCloudData(ShipmentId, tenant);
            if (shipmentAdditionalCloudData == null)
                return;

            var shipment = GetShipmentPM(ShipmentId, tenant);
            using (TransactionScope scope = TransactionFactory.GetTransaction())
            {
                var document = AddDocumentTasks(tenant, shipment);
                var commLog = AddCommunicationLog(shipment, tenant, document);
                SendCommunicationLogMessage(commLog, tenant);
                scope.Complete();
            }
        }

        private bool IsCargoTrackingApprovalActive(int tenant)
        {
            if (tenant == 0)
                return false;
            var tenantManagement = GetTenantManagement(tenant);
            if (tenantManagement == null)
                return false;
            return tenantManagement.ActivatedforDeclarationApprove;
        }

        private TenantManagementPM GetTenantManagement(int tenant)
        {
            TenantManagementQuery tenantManagementQuery = new TenantManagementQuery(tenant);
            var tenantManagement = tenantManagementQuery.GetSinglePM(tenant);
            return tenantManagement;
        }

        private bool CheckIfShipmentIdExist(QueueResponse response)
        {
            if (!response.MessageValues.Keys.Contains("ShipmentId"))
            {
                return false;
            }
            string ShipmentId = response.MessageValues["ShipmentId"].ToString();
            if (string.IsNullOrEmpty(ShipmentId))
            {
                return false;
            }
            return true;
        }
        private ShipmentPM GetShipmentPM(string shipmentId, int tenant)
        {
            ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
            return shipmentQuery.GetSinglePMWithoutComposition(shipmentId, tenant);
        }
        private Document AddDocumentTasks(int tenant, ShipmentPM shipment)
        {
            DocumentRepository documentrepository = new DocumentRepository(commonContext);
            List<QueueTask> tasks = GetTasks(tenant, shipment);
            var ByteData = LogitudeXmlSerializer.SerializeObject(tasks);
            Document document = CreateDocument(tenant, ByteData);
            documentrepository.Add(document);
            documentrepository.SubmitChanges();
            IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
            BlobFileInfo fileInfo = CreateBlobFile(document, ByteData);
            storageservice.Write(ByteData, fileInfo);
            return document;
        }



        private List<QueueTask> GetTasks(int tenant, ShipmentPM shipment)
        {
            var tasks = new List<QueueTask>();
            tasks.Add(new QueueTask()
            {
                Action = "StatusUpdate",
                Parameters = new List<Logitude.Server.Tools.Parameter>() {
                    new Logitude.Server.Tools.Parameter { Name = "ShipmentNumber", Value = shipment.ShipmentNumber},
                    new Logitude.Server.Tools.Parameter { Name = "Code", Value = "VDK"},
                    new Logitude.Server.Tools.Parameter { Name = "Date", Value = TenantServerConfigration.GetCurrentDateTime(tenant).ToShortDateString()},
                    new Logitude.Server.Tools.Parameter { Name = "Time", Value = TenantServerConfigration.GetCurrentDateTime(tenant).ToShortTimeString()},
                    new Logitude.Server.Tools.Parameter { Name = "Remarks", Value = "Approval Task Received"},
                    new Logitude.Server.Tools.Parameter { Name = "Direction", Value = shipment.DirectionId}
                }
            });
            return tasks;
        }
        private Document CreateDocument(int tenant, byte[] byteData)
        {
            return new Document()
            {
                CreateDate = DateTime.Now,
                Extension = "xml",
                FileSize = byteData.Length,
                Tenant = tenant,
                Id = IdCounter.GetNumber("Document", tenant),
                HasFile = true,
                Folder = "ExternalTasksQueue",
            };
        }
        private BlobFileInfo CreateBlobFile(Document document, byte[] byteData)
        {
            return new BlobFileInfo()
            {
                FileName = document.Id,
                FolderName = document.Folder,
                Extension = document.Extension,
                Tenant = document.Tenant,
                FileSize = byteData.Length,

            };
        }
        private CommunicationLog AddCommunicationLog(ShipmentPM shipment, int tenant, Document document)
        {
            CommunicationLogRepository communicationLogRepository = new CommunicationLogRepository(commonContext);
            ObjectTableRepository objecttableRep = new ObjectTableRepository(tenant);
            ObjectTable objectTable = objecttableRep.GetObjectTableByName("Shipment", 0, true);
            var commLog = CreateCommunicationLog(objectTable, tenant, shipment, document);
            communicationLogRepository.Add(commLog);
            communicationLogRepository.SubmitChanges();
            return commLog;
        }

        private CommunicationLog CreateCommunicationLog(ObjectTable objectTable, int tenant, ShipmentPM shipment, Document document)
        {
            return new CommunicationLog()
            {
                Id = IdCounter.GetNumber("CommunicationLog", tenant),
                LastStatusDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                InOut = "O",
                //EntityId = OceanInsightsRequest.Id,
                ObjectTableId = (objectTable != null && !string.IsNullOrEmpty(objectTable.Id)) ? objectTable.Id : null,
                Subject = "Approval Task Received",
                Tenant = tenant,
                CommunicationLogTypeCode = "Q",
                CommunicationStatusTypeCode = "W",
                CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                DocumentId = document.Id,
                CreateDateUTC = DateTime.UtcNow,
                LastStatusDateUTC = DateTime.UtcNow,
                QueueName = "externaltasksqueue" + tenant + 1,
                Priority = 1,
                EntityReference = shipment.ShipmentNumber

            };
        }

        private void SendCommunicationLogMessage(CommunicationLog commLog, int tenant)
        {
            if (!string.IsNullOrEmpty(commLog.QueueName))
            {
                try
                {
                    Communications.UpdateCommunicationLogStatus(commLog.Id, tenant, null, commLog.CommunicationStatusTypeCode, "Before adding message to queue ImporterApprovalReceived " + DateTime.Now.ToString(), null);
                    SendCommunicationLogMessageToQueue(commLog.QueueName, commLog.Id, tenant);
                    Communications.UpdateCommunicationLogStatus(commLog.Id, tenant, null, commLog.CommunicationStatusTypeCode, "after adding message to queue  ImporterApprovalReceived " + DateTime.Now.ToString(), null);
                }
                catch (Exception ex)
                {
                    string errorMessage = ex.Message;

                    if (!string.IsNullOrEmpty(ex.StackTrace))
                    {
                        errorMessage += Environment.NewLine + ex.StackTrace;
                    }

                    Communications.UpdateCommunicationLogStatus(commLog.Id, tenant, null, commLog.CommunicationStatusTypeCode, "Exception occured while adding message to queue ImporterApprovalReceived " + DateTime.Now.ToString(), errorMessage);

                }
            }
        }





        private ShipmentAdditionalCloudData GetShipmentAdditionalCloudData(string shipmentId, int tenant)
        {
            ShipmentAdditionalCloudDataRepository Repo = new ShipmentAdditionalCloudDataRepository(tenant);
            return Repo.GetSingleShipmentAdditionalCloudData(shipmentId, tenant);
        }

        private void HandelException(Exception ex, int tenant, QueueResponse response)
        {
            ExceptionHandler.HandleException(ex, DateTime.Now, tenant, "", "WorkerRole", "", null);
            if (!CheckIfShipmentIdExist(response))
            {
                queue.CompleteAsFailed();
            }
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
        }



        private void ConnectClient()
        {
            try
            {
                queue = new DbQueueService();
                queue.InitializeQueue("CargoTrackingImporterApprovalReceivedQueue", 0);

            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "CargoTrackingImporterApprovalReceived worker role start", null, null);
            }
        }
        private void SendCommunicationLogMessageToQueue(string queueName, string communicationLogId, int tenant)
        {
            try
            {
                IQueueService queueservice = new DbQueueService();
                queueservice.InitializeQueue(queueName, 0);
                queueservice.Send(new Dictionary<string, string>() { { "CommunicationLogId", communicationLogId }, { "Tenant", tenant.ToString() } }, tenant);

            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "SendCommunicationLogMessageToQueue Forwarder Shipment", null, null);
            }
        }
    }
}
