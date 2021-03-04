using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityAMs;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Logitude.Server.Tools.StorageService;
using Logitude.SystemLogs;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.EntityOtherServices
{
    public class ExternalTasksQueueService
    {
        private int tenant;
        private string subject;
        private ICommonDataContext commonContext;
        public ExternalTasksQueueService(int tenant, string subject)
        {
            this.tenant = tenant;
            this.subject = subject;
            commonContext = CommonDataContext.GetContext(tenant);
        }
        public StatusUpdateExternalTasksQueueResult AddStatusUpdateExternalTaskQueue(ShipmentAdditionalCloudDataAM Data)
        {
            byte[] queueTasks = GetSerializeQueueTasks(Data);
            Document document = AddDocumentToRepo(queueTasks);
            ObjectTable objectTable = GetShipmentObjectTable();
            CommunicationLog commLog = AddCommunicationLogToRepo(document, objectTable, Data.ShipmentNumber);
            WriteDocumentToStorage(document, queueTasks);
            StatusUpdateExternalTasksQueueResult externalTasksQueueResult = TryAddingMessageToQueue(Data, commLog);

            return externalTasksQueueResult;
        }

        private StatusUpdateExternalTasksQueueResult TryAddingMessageToQueue(ShipmentAdditionalCloudDataAM Data, CommunicationLog commLog)
        {
            StatusUpdateExternalTasksQueueResult externalTasksQueueResult = new StatusUpdateExternalTasksQueueResult();
            if (!string.IsNullOrEmpty(commLog.QueueName))
            {
                try
                {
                    AddingMessageToTargetQueue(commLog);
                }
                catch (Exception ex)
                {
                    string errorMessage = BuildErrorMessage(ex);
                    Communications.UpdateCommunicationLogStatus(commLog.Id, Data.Tenant, null, commLog.CommunicationStatusTypeCode, "Exception occured while adding message to queue " + commLog.QueueName + " " + DateTime.Now.ToString(), errorMessage);
                    externalTasksQueueResult.Response.HasError = true;
                    externalTasksQueueResult.Response.ErrorMessage = errorMessage;
                    externalTasksQueueResult.Exception = ex;
                }
            }
            return externalTasksQueueResult;
        }

        private byte[] GetSerializeQueueTasks(ShipmentAdditionalCloudDataAM Data)
        {
            List<QueueTask> tasks = new List<QueueTask>
                {
                    new QueueTask()
                    {
                        Action = "StatusUpdate",
                        Parameters = new List<Logitude.Server.Tools.Parameter>() {
                            new Logitude.Server.Tools.Parameter { Name = "ShipmentNumber", Value = Data.ShipmentNumber},
                            new Logitude.Server.Tools.Parameter { Name = "Code", Value = Data.Code},
                            new Logitude.Server.Tools.Parameter { Name = "Date", Value = Data.Date},
                            new Logitude.Server.Tools.Parameter { Name = "Time", Value = Data.Time},
                            new Logitude.Server.Tools.Parameter { Name = "Remarks", Value = Data.Remarks},
                            new Logitude.Server.Tools.Parameter { Name = "Direction", Value = Data.Direction}
                        }
                    }
                };
            var ByteData = LogitudeXmlSerializer.SerializeObject(tasks);
            return ByteData;
        }

        private Document AddDocumentToRepo(byte[] queueTasks)
        {
            DocumentRepository documentrepository = new DocumentRepository(commonContext);
            Document document = new Document()
            {
                CreateDate = DateTime.Now,
                Extension = "xml",
                FileSize = queueTasks.Length,
                Tenant = Convert.ToInt32(tenant),
                Id = IdCounter.GetNumber("Document", tenant),
                HasFile = true,
                Folder = "ExternalTasksQueue",
            };
            documentrepository.Add(document);
            documentrepository.SubmitChanges();
            return document;
        }

        private ObjectTable GetShipmentObjectTable()
        {
            ObjectTableRepository objecttableRep = new ObjectTableRepository(tenant);
            ObjectTable objectTable = objecttableRep.GetObjectTableByName("Shipment", 0, true);
            return objectTable;
        }

        private CommunicationLog AddCommunicationLogToRepo(Document document, ObjectTable objectTable, string shipmentNumber)
        {

            CommunicationLogRepository communicationLogRepository = new CommunicationLogRepository(commonContext);
            CommunicationLog communicationLog = new CommunicationLog()
            {
                Id = IdCounter.GetNumber("CommunicationLog", tenant),
                LastStatusDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                InOut = "O",
                ObjectTableId = (objectTable != null && !string.IsNullOrEmpty(objectTable.Id)) ? objectTable.Id : null,
                Subject = subject,
                Tenant = tenant,
                CommunicationLogTypeCode = "Q",
                CommunicationStatusTypeCode = "W",
                CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                DocumentId = document.Id,
                CreateDateUTC = DateTime.UtcNow,
                LastStatusDateUTC = DateTime.UtcNow,
                QueueName = "externaltasksqueue" + tenant + 1,
                Priority = 1,
                EntityReference = shipmentNumber,
            };

            communicationLogRepository.Add(communicationLog);
            communicationLogRepository.SubmitChanges();
            return communicationLog;
        }

        private void WriteDocumentToStorage(Document document, byte[] queueTasks)
        {
            IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
            BlobFileInfo fileInfo = new BlobFileInfo()
            {
                FileName = document.Id,
                FolderName = document.Folder,
                Extension = document.Extension,
                Tenant = tenant,
                FileSize = queueTasks.Length,
            };

            storageservice.Write(queueTasks, fileInfo);
        }

        private void AddingMessageToTargetQueue(CommunicationLog communicationLog)
        {
            Communications.UpdateCommunicationLogStatus(communicationLog.Id, tenant, null, communicationLog.CommunicationStatusTypeCode, "Before adding message to queue " + communicationLog.QueueName + " " + DateTime.Now.ToString(), null);
            SendCommunicationLogMessageToQueue(communicationLog.QueueName, communicationLog.Id, tenant);
            Communications.UpdateCommunicationLogStatus(communicationLog.Id, tenant, null, communicationLog.CommunicationStatusTypeCode, "after adding message to queue " + communicationLog.QueueName + " " + DateTime.Now.ToString(), null);
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

        private string BuildErrorMessage(Exception ex)
        {
            string errorMessage = ex.Message;

            if (!string.IsNullOrEmpty(ex.StackTrace))
                errorMessage += Environment.NewLine + ex.StackTrace;

            return errorMessage;
        }

        public StatusUpdateExternalTasksQueueResult AddVIRExternalTaskQueue(ShipmentPM shipmentPM)
        {
            StatusUpdateExternalTasksQueueResult tasksQueueResult = new StatusUpdateExternalTasksQueueResult();
            if (IsMatchVIRStatusConditions(shipmentPM))
            {
                ShipmentAdditionalCloudDataAM shipmentAdditionalCloudDataAM = new ShipmentAdditionalCloudDataAM()
                {
                    ShipmentNumber = shipmentPM.ShipmentNumber,
                    Tenant = shipmentPM.Tenant,
                    Code = "VIR",
                    Remarks = "",
                    Direction = shipmentPM.DirectionId
                };
                tasksQueueResult = AddStatusUpdateExternalTaskQueue(shipmentAdditionalCloudDataAM);
            }

            return tasksQueueResult;
        }

        private bool IsMatchVIRStatusConditions(ShipmentPM shipmentPM)
        {
            bool isMatch = shipmentPM != null && shipmentPM.IsUserIDNumberRequired;
            return isMatch;
        }
    }

    public class StatusUpdateExternalTasksQueueResult
    {
        public Response Response { get; set; }
        public Exception Exception { get; set; }
    }
}
