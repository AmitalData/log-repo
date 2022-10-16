using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.QueueService;
using Logitude.ShipmentOrderModule.BL.APIDataContract.ApiV1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationWorkerRole.Services.Logbox
{
    class ImporterBuilderShipmentsOrderBatchService
    {
        private List<Logitude.ShipmentOrderModule.Data.EntityPOCOs.ShipmentOrder> shipmentsOrders;
        private ImporterShipmentsBatchService importerShipmentsBatchService;
        public ImporterBuilderShipmentsOrderBatchService(ImporterShipmentsBatchService importerShipmentsBatchService)
        {
            this.importerShipmentsBatchService = importerShipmentsBatchService;
        }

        public List<Logitude.ShipmentOrderModule.Data.EntityPOCOs.ShipmentOrder> GetAllShipmentsOrderFromToDate()
        {
            ShipmentOrderQueryService shipmentOrderQueryService = new ShipmentOrderQueryService(importerShipmentsBatchService.tenant);
            DateTime fromDate = (DateTime)importerShipmentsBatchService.customerTenantAccessCardsBatch.FromDatetime;
            DateTime toDate = (DateTime)importerShipmentsBatchService.customerTenantAccessCardsBatch.ToDatetime;
            string targetCustomerId = importerShipmentsBatchService.customerTenantAccessCard.CustomerId;
            IQueryable<Logitude.ShipmentOrderModule.Data.EntityPOCOs.ShipmentOrder> IQueryableShipmentsOrderFromToDate = shipmentOrderQueryService.GetIQueryableShipmentsOrderByTenantAndCreateDate(importerShipmentsBatchService.tenant, fromDate, toDate);
            shipmentsOrders = IQueryableShipmentsOrderFromToDate.Where(shipmentOrder => shipmentOrder.CustomerId == importerShipmentsBatchService.customerTenantAccessCard.CustomerId).ToList();
            
            return shipmentsOrders;
        }

        public List<Logitude.ShipmentOrderModule.Data.EntityPOCOs.ShipmentOrder> Run()
        {
            APILogsPM LogPM = importerShipmentsBatchService.GetNewLogPM();
            List<Logitude.ShipmentOrderModule.Data.EntityPOCOs.ShipmentOrder> allowedShipmentsOrder = GetAllowedShipmentsOrderForLogBox();
            importerShipmentsBatchService.CreateAPILog(allowedShipmentsOrder.Select(shipmentOrder => shipmentOrder.Id).ToList(), LogPM, "ShipmentOrder");

            APILogsUtility.UpdateAPILogStatus(LogPM.Id, importerShipmentsBatchService.tenant, "D", allowedShipmentsOrder.Count(), DateTime.Now, DateTime.UtcNow, "Sending Schedual Shipments Orders Done Successfully", null, null, null, "");

            return allowedShipmentsOrder;
        }

        private List<Logitude.ShipmentOrderModule.Data.EntityPOCOs.ShipmentOrder> GetAllowedShipmentsOrderForLogBox()
        {
            List<Logitude.ShipmentOrderModule.Data.EntityPOCOs.ShipmentOrder> allowedShipmentsOrder = new List<Logitude.ShipmentOrderModule.Data.EntityPOCOs.ShipmentOrder>();
            foreach (var ShipmentOrder in shipmentsOrders)
            {
                allowedShipmentsOrder = TryToSendImporterShipmentOrderQueue(allowedShipmentsOrder, ShipmentOrder);
            }

            return allowedShipmentsOrder;
        }

        private List<Logitude.ShipmentOrderModule.Data.EntityPOCOs.ShipmentOrder> TryToSendImporterShipmentOrderQueue(List<Logitude.ShipmentOrderModule.Data.EntityPOCOs.ShipmentOrder> shipmentsOrder, Logitude.ShipmentOrderModule.Data.EntityPOCOs.ShipmentOrder ShipmentOrder)
        {
            if (!IsAllowedShipmentOrderForLogBox(ShipmentOrder)) return shipmentsOrder;

            List<Logitude.ShipmentOrderModule.Data.EntityPOCOs.ShipmentOrder> allowedShipmentsOrder = shipmentsOrder;
            allowedShipmentsOrder.Add(ShipmentOrder);
            SendImporterShipmentOrderQueue(ShipmentOrder);

            return allowedShipmentsOrder;
        }

        private bool IsAllowedShipmentOrderForLogBox(Logitude.ShipmentOrderModule.Data.EntityPOCOs.ShipmentOrder shipmentOrder)
        {
            if (importerShipmentsBatchService.customerTenantAccessCard.LastMappingDateTime != null && shipmentOrder.CreateDate <= importerShipmentsBatchService.customerTenantAccessCard.LastMappingDateTime) return false;
            CustomerTenantAccessQuery customerTenantAccessQuery = new CustomerTenantAccessQuery(shipmentOrder.Tenant);
            CustomerTenantAccessInfo customerTenantAccessInfo = customerTenantAccessQuery.GetCustomerTenantAccessInfo(shipmentOrder.Tenant, shipmentOrder.ShipperId);

            if (customerTenantAccessInfo == null) return false;
            if (!customerTenantAccessInfo.HasAccess) return false;
            if (!customerTenantAccessInfo.IsExportActivated) return false;
            if (customerTenantAccessInfo.CustomerTenant == 0) return false;
            if (string.IsNullOrEmpty(shipmentOrder.CustomerTenantNumber?.ToString())) return false;
            if (shipmentOrder.CustomerTenantNumber != customerTenantAccessInfo.CustomerTenant) return false;
            if (string.IsNullOrEmpty(shipmentOrder.CustomerShipmentNumber)) return false;

            const string oceanTransportModeId = "O";
            if (shipmentOrder.TransportModeId != oceanTransportModeId) return false;

            const string exportDirectionId = "E";
            if (shipmentOrder.DirectionId != exportDirectionId) return false;

            return true;
        }

        private void SendImporterShipmentOrderQueue(Logitude.ShipmentOrderModule.Data.EntityPOCOs.ShipmentOrder ShipmentOrder)
        {
            IQueueService queueService = new DbQueueService();
            queueService.InitializeQueue("ImporterShipmentOrderQueue", 0);
            queueService.Send(
                new Dictionary<string, string>() { { "ShipmentOrderId", ShipmentOrder.Id }, { "Tenant", ShipmentOrder.Tenant.ToString() }, { "BatchNumber", importerShipmentsBatchService.BatchNumber }, { "CustomerId", importerShipmentsBatchService.CustomerId } }, ShipmentOrder.Tenant, null, importerShipmentsBatchService.CustomerId, importerShipmentsBatchService.BatchNumber);
        }
    }
}
