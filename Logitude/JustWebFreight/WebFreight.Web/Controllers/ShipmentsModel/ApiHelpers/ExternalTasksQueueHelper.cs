using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Logitude.BL.ShipmentsModel.EntityAMs;
using System.Web;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Server.Infrastructure.DataContracts;
using WebFreight.Web.Security;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.Server.Tools;
using Simplog.Server.Infrastructure;
using Logitude.BL.CommonDataModel.CodePropertiesMapping;
using Simplog.Data.InfrastructureModel;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Newtonsoft.Json;
using Logitude.Server.Tools.QueueService;
using WebFreight.Web.Helpers;
using Logitude.BL.GlobalModel.EntityQueries;
using Logitude.BL.GlobalModel.EntityPMs;
using System.Transactions;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure.Azure;
using Microsoft.Practices.Unity;
using Logitude.Server.Tools.StorageService;
using Microsoft.ServiceBus.Messaging;
using Logitude.SystemLogs;

namespace WebFreight.Web.Controllers.ShipmentsModel.ApiHelpers
{
    public class ExternalTasksQueueHelper
    {
        private int tenant;
        private ICommonDataContext commonContext;
        public ExternalTasksQueueHelper(int tenant)
        {
            this.tenant = tenant;
            commonContext = CommonDataContext.GetContext(tenant);
        }
        public StatusUpdateExternalTasksQueueResult PostStatusUpdateExternalTaskQueue(ShipmentAdditionalCloudDataAM Data)
        {
            StatusUpdateExternalTasksQueueResult externalTasksQueueResult = new StatusUpdateExternalTasksQueueResult();
            HybridPartnerPM CurrentHybridPartner = GetHybridPartner();
            if (CurrentHybridPartner != null && !CurrentHybridPartner.IsExternalPartner)
            {
                byte[] queueTasks = GetSerializeQueueTasks(Data);
                Document document = AddDocumentToRepo(queueTasks);
                ObjectTable objectTable = GetShipmentObjectTable();
                CommunicationLog commLog = AddCommunicationLogToRepo(document, objectTable, Data.ShipmentNumber);
                WriteDocumentToStorage(document, queueTasks);
                externalTasksQueueResult = TryAddingMessageToQueue(Data, commLog);
            }

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

        private HybridPartnerPM GetHybridPartner()
        {
            HybridPartnerQuery HybridPartnerQuery = new HybridPartnerQuery(tenant);
            HybridPartnerPM CurrentHybridPartner = HybridPartnerQuery.GetSinglePMByPartnerTenant(tenant);
            return CurrentHybridPartner;
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
                Subject = "Status Update",
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
    }

    public class StatusUpdateExternalTasksQueueResult
    {
        public Response Response { get; set; }
        public Exception Exception { get; set; }
    }
}