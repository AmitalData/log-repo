using Logitude.BL.Security;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.Server.Tools;
using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
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
        private CommunicationsParams communicationLogParams;
        private ICommonDataContext commonContext;
        private ShipmentQuery shipmentQuery;
        private ShipmentPM shipment;
        private ContainerQuery containerQuery;
        private ContainerPM container;
        private string communicationLogObjectTableId;
        private string loggedContactId;
        private string communicationLogTo = "OceanInsightStatusRequest";
        private string communicationLogSubject = "Shipment Containers Statuses";
        private string objectTableName = "Shipment";
        private bool isContainer = false;
        private string communicationLogAdditionalFields;
        private string scacCode;
        private string entityReference;
        private string communicationLogId; 

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
            this.communicationLogAdditionalFields = scacCode + "," + oceanInsightType + "," + this.shipmentId + "," + container.ContainerNumber;
        }

        public void SendContainerStatusRequest()
        {
            this.BuildCommunicationLog();
            this.SendDBQueueForContainerStatuses();
        }
       
        private void BuildCommunicationLog()
        {
            communicationLogParams = new CommunicationsParams()
            {
                Tenant = tenant,
                From = "Amital",
                To = communicationLogTo,
                CommunicationLogTypeCode = "Q",
                Priority = 1,
                InOut = "O",
                Status = "W",
                LoggingUserId = this.loggedContactId,
                LoggingObjectTableId = communicationLogObjectTableId,
                LoggingEntityId = isContainer ? container.Id : shipment.Id,
                LoggingEntityReference = entityReference,
                Subject = communicationLogSubject,
                FolderName = communicationLogTo.ToLower(),
                QueueName = "ContainerStatusesCommunicationLogQueue",
                AdditionalFields = communicationLogAdditionalFields,
                ByteData = new byte[] { }

            };
            communicationLogId = Communications.AddCommunicationLog(communicationLogParams);
        }
        private void SendDBQueueForContainerStatuses()
        {
            try
            {
                IQueueService queueservice = new DbQueueService();
                queueservice.InitializeQueue(communicationLogParams.QueueName, 0);
                queueservice.Send(new Dictionary<string, string>() { { "CommunicationLogId", communicationLogId }, { "Tenant", tenant.ToString() } }, tenant);
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