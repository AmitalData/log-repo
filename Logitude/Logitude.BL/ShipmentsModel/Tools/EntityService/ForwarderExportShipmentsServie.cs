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

namespace Logitude.BL.ShipmentsModel.Tools.EntityService
{
    public class ForwarderExportShipmentsServie
    {  
        private NewAExporterShipmentAM shipment;
        private ICommonDataContext commonContext;
        string action = "NewAExporterShipment";

        public ForwarderExportShipmentsServie(NewAExporterShipmentAM newAExporterShipmentAM)
        {
            shipment = newAExporterShipmentAM;
            commonContext = CommonDataContext.GetContext(shipment.Tenant);
            SetShipmentCustomerCode();
            //action = GetActionName();  
        }

        public CommunicationLog HandelCommunicationLogData(List<QueueTask> tasks)
        {
            CommunicationLog communicationLog;
            var ByteData = LogitudeXmlSerializer.SerializeObject(tasks);
            Document document = CreateDocument(ByteData);
            communicationLog = CreateCommunicationLog(document);
            WriteFileOnStorage(ByteData, document);
            return communicationLog;
        }

        public List<QueueTask> AddExternalTaskQueue()
        {
            List<QueueTask> tasks = new List<QueueTask>();
            var data = LogitudeXmlSerializer.SerializeObjectToUTF8XmlString(shipment);
            AddQueueTask(tasks, data);
            return tasks;
        }

        public byte[] GetExternalTasksByteData()
        {
            List<QueueTask> tasks = new List<QueueTask>();
            var data = LogitudeXmlSerializer.SerializeObjectToUTF8XmlString(shipment);
            AddQueueTask(tasks, data);
            var ByteData = LogitudeXmlSerializer.SerializeObject(tasks);
            return ByteData;
        }
          
        public bool IsExternalHybridPartner()
        {
            HybridPartnerQuery HybridPartnerQuery = new HybridPartnerQuery(shipment.Tenant);
            HybridPartnerPM CurrentHybridPartner = HybridPartnerQuery.GetSinglePMByPartnerTenant(shipment.Tenant); 
            return CurrentHybridPartner != null && !CurrentHybridPartner.IsExternalPartner;
        } 
        public BlobFileInfo CreateNewBlobFile( byte[] byteData, Document document)
        {
            return new BlobFileInfo()
            {
                FileName = document.Id,
                FolderName = document.Folder,
                Extension = document.Extension,
                Tenant = this.shipment.Tenant,
                FileSize = byteData.Length, 
            };
        } 
        public void AddQueueTask(List<QueueTask> tasks, string data)
        {
            tasks.Add(new QueueTask() { Action = action, Parameters = new List<Parameter>() { new Parameter { Name = "ExporterShipment", Value = data } } });
        } 
        public CommunicationLog CreateCommunicationLog( Document document)
        {
            ObjectTable objectTable = GetObjectTableName();
            CommunicationLog communicationLog = GetNewCommunicationLogInstance(document, objectTable);
            CommunicationLogRepository communicationLogRepository = new CommunicationLogRepository(commonContext); 
            communicationLogRepository.Add(communicationLog);
            communicationLogRepository.SubmitChanges(); 
            return communicationLog; 
        } 
        public CommunicationLog GetNewCommunicationLogInstance(Document document, ObjectTable objectTable)
        { 
            return new CommunicationLog()
            {
                Id = IdCounter.GetNumber("CommunicationLog", shipment.Tenant),
                LastStatusDate = TenantServerConfigration.GetCurrentDateTime(shipment.Tenant),
                InOut = "O",
                ObjectTableId = (objectTable != null && !string.IsNullOrEmpty(objectTable.Id)) ? objectTable.Id : null,
                Subject = action,
                Tenant = shipment.Tenant,
                CommunicationLogTypeCode = "Q",
                CommunicationStatusTypeCode = "W",
                CreateDate = TenantServerConfigration.GetCurrentDateTime(shipment.Tenant),
                DocumentId = document.Id,
                CreateDateUTC = DateTime.UtcNow,
                LastStatusDateUTC = DateTime.UtcNow,
                QueueName = "externaltasksqueue" + shipment.Tenant + 1,
                Priority = 1,
            };
        } 
        public void SetShipmentCustomerCode( )
        { 
            CustomerTenantAccessQuery customerTenantAccessQuery = new CustomerTenantAccessQuery(shipment.Tenant);

            CustomerTenantAccessCardQuery customerTenantAccessCardQuery = new CustomerTenantAccessCardQuery(shipment.Tenant);
            CustomerTenantAccessPM customerTenantAccessPM = customerTenantAccessQuery.GetCustomerTenantAccessPMsByTenantCustomerTenant(shipment.Tenant, shipment.ExporterTenant);
            CustomerTenantAccessCardPM card = customerTenantAccessCardQuery.GetCustomerTenantAccessCardPMByCustomerTenantAccessId(customerTenantAccessPM.Id, shipment.Tenant).Where(a => a.StatusTypeCode.ToUpper() != "IA").FirstOrDefault();
             
            if (card != null)
            {
                shipment.Customer = new CodeProperties() { Code = card.CustomerCode };
            }
        } 
        private ObjectTable GetObjectTableName()
        {
            ObjectTableRepository objectTableRepository = new ObjectTableRepository(shipment.Tenant);
            var objectTable = objectTableRepository.GetObjectTableByName("Shipment", 0, true);
            return objectTable;
        } 
        public Document CreateDocument( byte[] byteData)
        { 
            DocumentRepository documentrepository = new DocumentRepository(commonContext); 
            Document document = GetNewDocumentInstance(byteData);
            documentrepository.Add(document);
            documentrepository.SubmitChanges();

            return document;
        } 
        public Document GetNewDocumentInstance( byte[] byteData)
        {
            return new Document()
            {
                CreateDate = DateTime.Now,
                Extension = "xml",
                FileSize = byteData.Length,
                Tenant = Convert.ToInt32(shipment.Tenant),
                Id = IdCounter.GetNumber("Document", shipment.Tenant),
                HasFile = true,
                Folder = "ExternalTasksQueue",
            };
        } 
        public static void SendCommunicationLogMessageToQueue(string queueName, string communicationLogId, int tenant)
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
        public void AddQueueMessages( CommunicationLog communicationLog)
        {
            Communications.UpdateCommunicationLogStatus(communicationLog.Id, shipment.Tenant, null, communicationLog.CommunicationStatusTypeCode, "Before adding message to queue Forwarder Shipment " + DateTime.Now.ToString(), null); 
            SendCommunicationLogMessageToQueue(communicationLog.QueueName, communicationLog.Id, shipment.Tenant);
            Communications.UpdateCommunicationLogStatus(communicationLog.Id, shipment.Tenant, null, communicationLog.CommunicationStatusTypeCode, "after adding message to queue  Forwarder Shipment " + DateTime.Now.ToString(), null);
        }

        public void AddExceptionQueueMessages(CommunicationLog communicationLog, Exception ex)
        {
            string errorMessage = ex.Message;

            if (!string.IsNullOrEmpty(ex.StackTrace))
            {
                errorMessage += Environment.NewLine + ex.StackTrace;
            }

            Communications.UpdateCommunicationLogStatus(communicationLog.Id, shipment.Tenant, null, communicationLog.CommunicationStatusTypeCode, "Exception occured while adding message to queue Forwarder Shipment " + DateTime.Now.ToString(), errorMessage);
        } 
        public void WriteFileOnStorage(byte[] byteData, Document document)
        { 
            IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
            BlobFileInfo fileInfo = CreateNewBlobFile(byteData, document);
            storageservice.Write(byteData, fileInfo);
        }
        private string GetActionName()
        {
            string action = "NewAExporterShipment";
            // if (Shipment.SendUpdatesToAgentEnabled)
            // {
            //     myAction = "UpdateAExporterShipment";
            // }
            return action;
        }
    }
}