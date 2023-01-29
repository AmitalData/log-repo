using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.QueueService;
using Logitude.ShipmentOrderModule.Def.EntityAMs;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel;
using System;
using System.Collections.Generic;

namespace WebFreight.Web.Helpers.ImporterShipmentOrders
{
    public class ImporterShipmentOrdersService
    {
        private APILogsQuery aPILogsQuery;
        private APILogsService apiLogsService;
        private ObjectTableRepository objectTableRepository;
        private ShipmentQuery shipmentQuery;
        private IWebFreightContext webFreightContext;
        private IShipmentsContext shipmentsContext;
        private ShipmentService shipmentService;
        private ShipmentOrderAmToShipmentMapping shipmentOrderAmMap;

        private readonly int tenant;
        private APILogsPM apiLog;
        private ObjectTable objectTable;
        private ShipmentOrderAM shipmentOrder;
        private bool isNew;
        private readonly string correlationId;

        public ImporterShipmentOrdersService(int tenant, string correlationId)
        {
            this.tenant = tenant;
            this.correlationId = correlationId;

            InitiallizeServices();
            InitiallizeFields();
        }

        private void InitiallizeServices()
        {
            shipmentsContext = ShipmentsContext.GetContext(tenant);
            webFreightContext = WebFreightContext.GetContext(tenant);
            objectTableRepository = new ObjectTableRepository(webFreightContext);
            shipmentQuery = new ShipmentQuery(tenant);
            apiLogsService = new APILogsService(webFreightContext, tenant);
            aPILogsQuery = new APILogsQuery(tenant);
            shipmentOrderAmMap = new ShipmentOrderAmToShipmentMapping(tenant);
        }
        private void InitiallizeFields()
        {
            objectTable = objectTableRepository.GetObjectTableByName("Shipment", tenant, true);
        }


        public ShipmentPM UpdateShipment(ShipmentOrderAM shipmentOrder)
        {
            this.shipmentOrder = shipmentOrder;
            GetApiLog();
            APILogsUtility.UpdateAPILogStatus(apiLog.Id, apiLog.Tenant, "I", 1, DateTime.Now, DateTime.UtcNow, "Start Updating Shipment By Shipment Order " + DateTime.Now, LogitudeXmlSerializer.SerializeObjectToXmlString(shipmentOrder), null, null, "");
            try
            {
                ShipmentPM shipment = shipmentQuery.GetSingleShipmentPMByNumber(shipmentOrder.CustomerShipmentNumber, tenant);
                if (shipment == null)
                    throw new Exception("Shipment number does not exist");
                if (!shipment.IsShipmentOrder)
                    throw new Exception("The shipment order is already merged into Shipment. For update it, please update the shipment instead of the shipment order.");
                isNew = string.IsNullOrEmpty(shipment.ForwarderShipmentNumber);
                shipment = shipmentOrderAmMap.Map(shipmentOrder, shipment);
                SubmitShipmentUpdate(shipment);
                SendDocumentsFillingToForwarder(shipment);

                return shipment;
            }
            catch (Exception ex)
            {
                HandleExeption(ex);
                return null;
            }
        }

        private void SubmitShipmentUpdate(ShipmentPM shipment)
        {
            string systemEmail = "system@tenant" + tenant + ".com";
            shipmentService = new ShipmentService(shipmentsContext, shipment, systemEmail);
            shipment.DontAddToImportersQueue = true;
            shipmentService.SetChangeSet(shipment.ShipmentPackages, new List<ShipmentOrderPackagePM>(), new List<ShipmentPickUpPM>(), new List<ShipmentDeliveryPM>(), new List<ShipmentReceivablePM>(), new List<ShipmentPayablePM>(), new List<ShipmentFollowUpPM>(), new List<ShipmentAWBPrintOnlyPM>(), new List<ConsoleShipmentPM>(), new List<ShipmentCarrierStatusPM>(), new List<AWBOCIPM>(), new List<ShipmentCommodityPM>(), new List<ShipmentAssemblyPM>(), new List<ShipmentStoragePricingPM>(), new List<ShipmentProductItemPM>(), new List<ShipmentUnassignedFieldPM>());
            shipmentService.Update();

            var msg = "Shipment Updated Successfully " + DateTime.Now;
            APILogsUtility.UpdateAPILogStatus(apiLog.Id, apiLog.Tenant, "D", 1, DateTime.Now, DateTime.UtcNow, msg, null, shipment.Id, null, "");
        }

        private void SendDocumentsFillingToForwarder(ShipmentPM shipment)
        {
            if (!isNew) return;
            const string inDocumentFillingDirectionCode = "I";
            DocumentsFilingQuery documentsFilingQuery = new DocumentsFilingQuery(shipment.Tenant);
            List<string> DocumentFillingPMsIds = documentsFilingQuery.GetSharedWithAgentDocumentsFilingPMsIdsByEntityId(shipment.Id, inDocumentFillingDirectionCode, shipment.Tenant);
            if (DocumentFillingPMsIds == null || DocumentFillingPMsIds.Count == 0) return;

            foreach (string DocumentFillingPMId in DocumentFillingPMsIds)
            {
                AddForwardersShipmentDocumentsQueue(shipment, DocumentFillingPMId);
            }
        }

        private void AddForwardersShipmentDocumentsQueue(ShipmentPM shipment, string DocumentFillingPMId)
        {
            IQueueService queueservice = new DbQueueService();
            queueservice.InitializeQueue("ForwardersShipmentDocumentsQueue", 0);
            queueservice.Send(new Dictionary<string, string>() { { "ShipmentId", shipment.Id }, { "DocumentFilingId", DocumentFillingPMId }, { "Tenant", shipment.Tenant.ToString() } }, shipment.Tenant);
        }

        private void HandleExeption(Exception exception)
        {
            string errorMessage = exception.Message + Environment.NewLine;

            if (exception.InnerException != null)
            {
                errorMessage = errorMessage + " (" + (exception.InnerException.InnerException != null ? exception.InnerException.InnerException.Message : exception.InnerException.Message) + ")" + Environment.NewLine;
            }

            errorMessage = errorMessage + exception.StackTrace + Environment.NewLine;
            APILogsUtility.UpdateAPILogStatus(apiLog.Id, apiLog.Tenant, "F", 1, DateTime.Now, DateTime.UtcNow, "Update Shipment At Importer Tenant Faild " + DateTime.Now, null, null, errorMessage, (errorMessage.Length >= 250 ? errorMessage.Substring(0, 249) : errorMessage));

            throw exception;
        }

        private void GetApiLog()
        {
            apiLog = aPILogsQuery.GetSingleByCorrelationIdAndTenant(correlationId, tenant);
            if (apiLog != null) return;
            apiLog = CreateNewApiLogInstance();
            apiLogsService.Create(apiLog);
        }

        private APILogsPM CreateNewApiLogInstance()
        {
            return new APILogsPM()
            {
                Id = IdCounter.GetNumber("APILogs", tenant),
                CorrelationId = correlationId,
                CreateDate = DateTime.Now,
                CreateDateUTC = DateTime.UtcNow,
                Direction = "I",
                EntityId = shipmentOrder.Id,
                LastUpdateDate = DateTime.Now,
                LastUpdateDateUTC = DateTime.UtcNow,
                NumberOfRetries = 1,
                ObjectTableId = objectTable.Id,
                ExpirationDate = DateTime.Now.AddDays(90),
                Refrence = shipmentOrder.CustomerShipmentNumber,
                Status = "I",
                Tenant = shipmentOrder.CustomerTenantNumber,
                Subject = "Updating shipment by shipment order"
            };
        }

    }
}