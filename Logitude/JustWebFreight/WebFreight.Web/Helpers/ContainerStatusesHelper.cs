using Logitude.BL.Security;
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
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.Web;

namespace WebFreight.Web.Helpers
{
    public class ContainerStatusesHelper
    {
        private int tenant;
        private string shipmentId;
        private CommunicationLog communicationLog;
        private ICommonDataContext commonContext;
        private IShipmentsContext shipmentContext;
        private ShipmentRepository shipmentRepository;
        private Shipment shipment;
        private string shipmentObjectTableId;
        private string loggedContactId;
        private Document document;
        private string communicationLogTo = "OceanInsightStatusRequest";
        private string communicationLogSubject = "Shipment Containers Statuses";

        public ContainerStatusesHelper(string shipmentId, int tenant)
        {
            this.tenant = tenant;
            this.shipmentId = shipmentId;
            this.commonContext = CommonDataContext.GetContext(this.tenant);
            this.GetSingleShipmentById();
            this.GetShipmentObjectTableId();
            this.GetLoggedContactId();
        }
        private void GetSingleShipmentById()
        {
            this.shipmentContext = ShipmentsContext.GetContext(tenant);
            this.shipmentRepository = new ShipmentRepository(shipmentContext);
            this.shipment = shipmentRepository.GetSingleShipment(shipmentId, tenant);
        }

        private void GetShipmentObjectTableId()
        {
            ObjectTableRepository myObjectTabelRepository = new ObjectTableRepository(tenant);
            ObjectTable objectTable = myObjectTabelRepository.GetObjectTableByName("Shipment", 0, true);
            if (objectTable != null)
            {
                shipmentObjectTableId = objectTable.Id;
            }
        }
        private void GetLoggedContactId()
        {
            ContactRepository contactRepository = new ContactRepository(this.commonContext);
            var loggedContact = contactRepository.GetSingleContactByEmail(SecurityUtility.GetAuthenticatedUser(), tenant);
            this.loggedContactId = loggedContact.Id;
        }
        public void SendContainerStatusRequest()
        {
            this.BuildDocument();
            this.BuildCommunicationLog();
            this.SendDBQueueForContainerStatuses();
        }
       
        private void BuildDocument()
        {
            DocumentRepository documentRepository = new DocumentRepository(this.commonContext);
            document = new Document()
            {
                CreateDate = System.DateTime.Now,
                Extension = "xml",
                Tenant = Convert.ToInt32(tenant),
                Id = IdCounter.GetNumber("Document", tenant),
                HasFile = true,
                Folder = communicationLogTo.ToLower(),
            };
            documentRepository.Add(document);
            documentRepository.SubmitChanges();
        }
        private void BuildCommunicationLog()
        {
            CommunicationLogRepository communicationLogRepository = new CommunicationLogRepository(this.commonContext);
            communicationLog = new CommunicationLog()
            {
                Id = IdCounter.GetNumber("CommunicationLog", tenant),
                LastStatusDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                LastStatusDateUTC = System.DateTime.UtcNow,
                To = communicationLogTo,
                InOut = "O",
                EntityId = shipmentId,
                ObjectTableId = shipmentObjectTableId,
                Subject = communicationLogSubject,
                Tenant = tenant,
                CommunicationLogTypeCode = "T",
                CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                CommunicationStatusTypeCode = "W",
                CreatedByUserId = this.loggedContactId,
                DocumentId = document.Id,
                EntityReference = this.shipment.ShipmentNumber,
                SearchFields = this.shipment.ShipmentNumber + "," + communicationLogTo + "," + "O" + "," + communicationLogSubject,
                CreateDateUTC = System.DateTime.UtcNow,
                QueueName = "ContainerStatusesCommunicationLogQueue",
            };
            communicationLogRepository.Add(communicationLog);
            this.commonContext.SaveChanges();
        }

        private void SendDBQueueForContainerStatuses()
        {
            try
            {
                IQueueService queueservice = new DbQueueService();
                queueservice.InitializeQueue(communicationLog.QueueName, 0);
                queueservice.Send(new Dictionary<string, string>() { { "CommunicationLogId", communicationLog.Id }, { "Tenant", tenant.ToString() } }, tenant);
            }

            catch (Exception ex)
            {
                string ip = "";
                if (HttpContext.Current != null && HttpContext.Current.Request != null)
                {
                    ip = HttpContext.Current.Request.UserHostAddress;
                }
                ExceptionHandler.HandleException(ex, System.DateTime.Now, 0, null, "ShipmentContainersWebServiceController", null, ip);
            }
        }
    }
}