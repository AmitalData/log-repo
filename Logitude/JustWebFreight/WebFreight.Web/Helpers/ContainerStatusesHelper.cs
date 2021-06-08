using Logitude.BL.Security;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
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
        private string containerId;
        private CommunicationLog communicationLog;
        private ICommonDataContext commonContext;
        private ShipmentQuery shipmentQuery;
        private ShipmentPM shipment;
        private ContainerQuery containerQuery;
        private ContainerPM container;
        private string communicationLogObjectTableId;
        private string loggedContactId;
        private Document document;
        private string communicationLogTo = "OceanInsightStatusRequest";
        private string communicationLogSubject = "Shipment Containers Statuses";
        private string objectTableName = "Shipment";
        private bool isContainer = false;
        private string communicationLogAdditionalFields;
        private string scacCode;
        private string entityReference;
        public ContainerStatusesHelper(string shipmentId, string containerId, bool isContainer, int tenant)
        {
            this.tenant = tenant;
            this.shipmentId = shipmentId;
            this.containerId = containerId;
            this.isContainer = isContainer;
            this.commonContext = CommonDataContext.GetContext(this.tenant);
            if (!isContainer)
                this.GetSingleShipmentById();
            else
            {
                this.GetSingleContainerById();
            }
            this.GetCommuniactionLogObjectTableId();
            this.GetLoggedContactId();
            this.GetShipmentScacCode();
            this.GetentityReference();
            this.BuildCommunicationLogAdditionalFields();
        }
        private void GetSingleShipmentById()
        {
            this.shipmentQuery = new ShipmentQuery(tenant);
            this.shipment = shipmentQuery.GetSingleShipmentPM(shipmentId, tenant);
        }
        private void GetSingleContainerById()
        {
            this.containerQuery = new ContainerQuery(tenant);
            this.container = containerQuery.GetSinglePM(containerId, tenant);
        }
        private void GetCommuniactionLogObjectTableId()
        {
            if (this.isContainer)
            {
                objectTableName = "Container";
            }
            ObjectTableRepository myObjectTabelRepository = new ObjectTableRepository(tenant);
            ObjectTable objectTable = myObjectTabelRepository.GetObjectTableByName(objectTableName, 0, true);
            if (objectTable != null)
            {
                communicationLogObjectTableId = objectTable.Id;
            }
        }
        private void GetLoggedContactId()
        {
            ContactRepository contactRepository = new ContactRepository(this.commonContext);
            var loggedContact = contactRepository.GetSingleContactByEmail(SecurityUtility.GetAuthenticatedUser(), tenant);
            this.loggedContactId = loggedContact.Id;
        }
        private void GetShipmentScacCode()
        {
            string mainCarriageCarrierId = isContainer ? container.MainCarriageCarrierId : this.shipment.MainCarriageCarrierId;
            ShippingLineRepository shippingLineRepository = new ShippingLineRepository(tenant);
            var shippingLine = shippingLineRepository.GetSingleShippingLine(mainCarriageCarrierId, tenant);
            this.scacCode = shippingLine != null ? shippingLine.SCACCode : "";
        }
        private void GetentityReference()
        {
            this.entityReference = this.isContainer ? container.ContainerNumber : shipment.Master;
        }

        private void BuildCommunicationLogAdditionalFields()
        {
            this.communicationLogAdditionalFields = "";
            var oceanInsightType = "";
            if (this.isContainer)
            {
                oceanInsightType = "c_id";
            }
            this.communicationLogAdditionalFields = scacCode + "," + oceanInsightType;
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
                EntityId = isContainer ? container.Id : shipment.Id,
                ObjectTableId = communicationLogObjectTableId,
                Subject = communicationLogSubject,
                Tenant = tenant,
                CommunicationLogTypeCode = "T",
                CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                CommunicationStatusTypeCode = "W",
                CreatedByUserId = this.loggedContactId,
                DocumentId = document.Id,
                EntityReference = entityReference, 
                SearchFields = this.shipmentId + "," + communicationLogTo + "," + "O" + "," + communicationLogSubject,
                CreateDateUTC = System.DateTime.UtcNow,
                QueueName = "ContainerStatusesCommunicationLogQueue",
                AdditionalFields = communicationLogAdditionalFields,
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

        public bool Validate()
        {
            bool isValid = true;
            if (string.IsNullOrEmpty(this.scacCode) || string.IsNullOrEmpty(this.entityReference))
            {
                isValid = false;
            }
            return isValid;
        }
    }
}