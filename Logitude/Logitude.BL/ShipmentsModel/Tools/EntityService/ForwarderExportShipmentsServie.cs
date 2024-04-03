using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityAMs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
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
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Logitude.BL.ShipmentsModel.Tools.EntityService
{
    public class ForwarderExportShipmentsServie
    {
        private readonly object exporterShipmentAM;
        private readonly int tenant;
        private readonly ICommonDataContext commonContext;
        private readonly string actionName;

        public ForwarderExportShipmentsServie(object exporterShipmentAM, int tenant)
        {
            this.exporterShipmentAM = exporterShipmentAM;
            this.tenant = tenant;
            commonContext = CommonDataContext.GetContext(tenant);
            actionName = GetActionName();
            SetShipmentCustomerCode();
        }

        private string GetActionName()
        {
            string ShipmentTransportModeId = (string)GetPropertyValue("TransportModeId");
            switch (ShipmentTransportModeId)
            {
                case "A": return "NewAExporterShipment";
                case "O": return "NewOExporterShipment";
                default: return "";
            }
        }

        private object GetPropertyValue(string objectFieldName)
        {
            PropertyInfo propInfo = exporterShipmentAM.GetType().GetProperty(objectFieldName);
            return propInfo?.GetValue(exporterShipmentAM);
        }

        private void SetShipmentCustomerCode()
        { 
            CustomerTenantAccessQuery customerTenantAccessQuery = new CustomerTenantAccessQuery(tenant);

            CustomerTenantAccessCardQuery customerTenantAccessCardQuery = new CustomerTenantAccessCardQuery(tenant);
            int? exporterTenant = (int?)GetPropertyValue("ExporterTenant");
            if (exporterTenant == null) return;
            CustomerTenantAccessPM customerTenantAccessPM = customerTenantAccessQuery.GetCustomerTenantAccessPMsByTenantCustomerTenant(tenant, (int)exporterTenant);
            if (customerTenantAccessPM == null) return;
            CustomerTenantAccessCardPM card = customerTenantAccessCardQuery.GetCustomerTenantAccessCardPMByCustomerTenantAccessId(customerTenantAccessPM.Id, tenant).Where(a => a.StatusTypeCode.ToUpper() != "IA").FirstOrDefault();
             
            if (card != null)
            {
                SetPropertyValue("Customer", new CodeProperties() { Code = card.CustomerCode });
            }
        }

        private void SetPropertyValue(string objectFieldName, object fieldValue)
        {
            PropertyInfo propInfo = exporterShipmentAM.GetType().GetProperty(objectFieldName);
            if (propInfo != null) propInfo.SetValue(exporterShipmentAM, fieldValue, null);
        }

        public string AddCommunicationLogQueueMessages()
        {
            if (!IsExternalHybridPartner()) return "";

            List<QueueTask> queueTasks = GetExternalQueueTasks();
            CommunicationLog communicationLog = HandleCommunicationLog(queueTasks);

            if (string.IsNullOrEmpty(communicationLog.QueueName)) return communicationLog.Id;

            try
            {
                AddQueueMessages(communicationLog);
            }
            catch (Exception ex)
            {
                AddExceptionQueueMessages(communicationLog, ex);
                throw new Exception(ex.Message, ex.InnerException);
            }

            return communicationLog.Id;
        }

        private bool IsExternalHybridPartner()
        {
            HybridPartnerQuery HybridPartnerQuery = new HybridPartnerQuery(tenant);
            HybridPartnerPM CurrentHybridPartner = HybridPartnerQuery.GetSinglePMByPartnerTenant(tenant);
            return CurrentHybridPartner != null && !CurrentHybridPartner.IsExternalPartner;
        }

        private List<QueueTask> GetExternalQueueTasks()
        {
            List<QueueTask> tasks = new List<QueueTask>();
            string data = LogitudeXmlSerializer.SerializeObjectToUTF8XmlString(exporterShipmentAM, true);
            AddQueueTask(tasks, data);
            return tasks;
        }

        private void AddQueueTask(List<QueueTask> tasks, string data)
        {
            tasks.Add(new QueueTask() { Action = actionName, Parameters = new List<Parameter>() { new Parameter { Name = "ExporterShipment", Value = data } } });
        }

        private CommunicationLog HandleCommunicationLog(List<QueueTask> queueTasks)
        {
            byte[] ByteData = LogitudeXmlSerializer.SerializeObject(queueTasks);
            Document document = CreateDocument(ByteData);
            CommunicationLog communicationLog = CreateCommunicationLog(document);
            WriteFileOnStorage(ByteData, document);
            return communicationLog;
        }

        private Document CreateDocument(byte[] fileData)
        {
            DocumentRepository documentrepository = new DocumentRepository(commonContext);
            Document document = GetNewDocument(fileData);
            documentrepository.Add(document);
            documentrepository.SubmitChanges();

            return document;
        }

        private Document GetNewDocument(byte[] fileData)
        {
            return new Document()
            {
                CreateDate = DateTime.Now,
                Extension = "xml",
                FileSize = fileData.Length,
                Tenant = Convert.ToInt32(tenant),
                Id = IdCounter.GetNumber("Document", tenant),
                HasFile = true,
                Folder = "ExternalTasksQueue",
            };
        }

        private CommunicationLog CreateCommunicationLog(Document document)
        {
            ObjectTable objectTable = GetShipmentObjectTable();
            CommunicationLog communicationLog = GetNewCommunicationLog(document, objectTable);
            CommunicationLogRepository communicationLogRepository = new CommunicationLogRepository(commonContext);
            communicationLogRepository.Add(communicationLog);
            communicationLogRepository.SubmitChanges();
            return communicationLog;
        }

        private ObjectTable GetShipmentObjectTable()
        {
            ObjectTableRepository objectTableRepository = new ObjectTableRepository(tenant);
            var objectTable = objectTableRepository.GetObjectTableByName("Shipment", 0, true);
            return objectTable;
        }

        private CommunicationLog GetNewCommunicationLog(Document document, ObjectTable objectTable)
        {
            return new CommunicationLog()
            {
                Id = IdCounter.GetNumber("CommunicationLog", tenant),
                LastStatusDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                InOut = "O",
                ObjectTableId = (objectTable != null && !string.IsNullOrEmpty(objectTable.Id)) ? objectTable.Id : null,
                Subject = actionName,
                Tenant = tenant,
                CommunicationLogTypeCode = "Q",
                CommunicationStatusTypeCode = "W",
                CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                DocumentId = document.Id,
                CreateDateUTC = DateTime.UtcNow,
                LastStatusDateUTC = DateTime.UtcNow,
                QueueName = "externaltasksqueue" + tenant + 1,
                Priority = 1,
            };
        }

        private void WriteFileOnStorage(byte[] fileData, Document document)
        {
            IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
            BlobFileInfo fileInfo = GetNewBlobFileInfo(fileData, document);
            storageservice.Write(fileData, fileInfo);
        }
        
        public BlobFileInfo GetNewBlobFileInfo(byte[] fileData, Document document)
        {
            return new BlobFileInfo()
            {
                FileName = document.Id,
                FolderName = document.Folder,
                Extension = document.Extension,
                Tenant = tenant,
                FileSize = fileData.Length,
            };
        }

        private void AddQueueMessages(CommunicationLog communicationLog)
        {
            Communications.UpdateCommunicationLogStatus(communicationLog.Id, tenant, null, communicationLog.CommunicationStatusTypeCode, "Before adding message to queue Forwarder Shipment " + DateTime.Now.ToString(), null);
            SendCommunicationLogMessageToQueue(communicationLog.QueueName, communicationLog.Id);
            Communications.UpdateCommunicationLogStatus(communicationLog.Id, tenant, null, communicationLog.CommunicationStatusTypeCode, "after adding message to queue  Forwarder Shipment " + DateTime.Now.ToString(), null);
        }

        private void SendCommunicationLogMessageToQueue(string queueName, string communicationLogId)
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

        private void AddExceptionQueueMessages(CommunicationLog communicationLog, Exception ex)
        {
            string errorMessage = ex.Message;

            if (!string.IsNullOrEmpty(ex.StackTrace))
            {
                errorMessage += Environment.NewLine + ex.StackTrace;
            }

            Communications.UpdateCommunicationLogStatus(communicationLog.Id, tenant, null, communicationLog.CommunicationStatusTypeCode, "Exception occured while adding message to queue Forwarder Shipment " + DateTime.Now.ToString(), errorMessage);
        }
    }
}