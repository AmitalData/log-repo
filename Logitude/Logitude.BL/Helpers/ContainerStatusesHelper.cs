using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Security;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.Server.Tools;
using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.Web;

namespace Logitude.BL.Helpers
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
        private LogitudeOceanInsightsRequestRepository logitudeOceanInsightsRequestRepository;
        private ShippingLinePM tenantZeroShippingLine;
        private ShippingLinePM shippingLine;
        ShippingLineRepository shippingLineRepository;
        ShippingLineQuery shippingLineQuery;

        public ContainerStatusesHelper(string shipmentId, string containerId, bool isContainer, int tenant)
        {
            this.tenant = tenant;
            this.shipmentId = shipmentId;
            this.containerId = containerId;
            this.isContainer = isContainer;
            this.commonContext = CommonDataContext.GetContext(this.tenant);
            this.logitudeOceanInsightsRequestRepository = new LogitudeOceanInsightsRequestRepository(this.tenant);
            this.shippingLineRepository = new ShippingLineRepository(tenant);
            this.shippingLineQuery = new ShippingLineQuery(shippingLineRepository);
            if (!isContainer)
                this.GetSingleShipmentById();
            else
            {
                this.GetSingleContainerById();
            }
            this.GetCommuniactionLogObjectTableId();
            this.GetLoggedContactId();
            this.GetShipmentScacCode();
            this.GetTenantZeroShippingLine();
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
            string mainCarriageCarrierId = isContainer ? container?.MainCarriageCarrierId : this.shipment?.MainCarriageCarrierId;
            shippingLine = shippingLineQuery.GetSinglePM(mainCarriageCarrierId, tenant);
            if (shippingLine != null)
                this.scacCode = shippingLine != null ? shippingLine.SCACCode : "";
        }
        private void GetTenantZeroShippingLine()
        {
            int tenantZero = 0;
            if (this.shippingLine != null)
                tenantZeroShippingLine = shippingLineQuery.GetSinglePMByCode(this.shippingLine.Code, tenantZero);
        }

        private void GetentityReference()
        {
            this.entityReference = this.isContainer ? container?.ContainerNumber : shipment?.Master;
        }

        private void BuildCommunicationLogAdditionalFields()
        {
            this.communicationLogAdditionalFields = "";
            var oceanInsightType = "m_bl"; //Shipment
            if (this.isContainer)
            {
                oceanInsightType = "c_id"; // Container
            }
            this.communicationLogAdditionalFields = scacCode + "," + oceanInsightType + "," + this.shipmentId + "," + (container != null ? container.ContainerNumber : null);
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
                LoggingEntityId = isContainer ? container?.Id : shipment?.Id,
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
            if (string.IsNullOrEmpty(this.scacCode) || string.IsNullOrEmpty(this.entityReference) || !this.IsValidShippingLine())
            {
                isValid = false;
            }
            return isValid;
        }

        private bool IsValidShippingLine()
        {
            if (this.tenantZeroShippingLine == null)
            {
                return false;
            }
            if (this.isContainer && !this.tenantZeroShippingLine.IsSendingByContainer)
            {
                return false;
            }
            if (!this.isContainer && !this.tenantZeroShippingLine.IsSendingByBillOfLading)
            {
                return false;
            }
            return true;
        }

        public bool IsLogitudeOceanInsightsRequestExistForShipment()
        {
            LogitudeOceanInsightsRequest logitudeOceanInsightsRequest = logitudeOceanInsightsRequestRepository.GetSingleLogitudeOceanInsightsRequestByOBLNumberAndScac(shipment?.Master, scacCode, shipmentId);

            if (logitudeOceanInsightsRequest == null)
            {
                return true;
            }

            return false;
        }
        public bool IsLogitudeOceanInsightsRequestExistForConatiner()
        {
            LogitudeOceanInsightsRequest logitudeOceanInsightsRequest = logitudeOceanInsightsRequestRepository.GetSingleLogitudeOceanInsightsRequestByContainerAndScac(container?.ContainerNumber, scacCode, shipmentId);

            if (logitudeOceanInsightsRequest == null)
            {
                return true;
            }

            return false;
        }
    }
}