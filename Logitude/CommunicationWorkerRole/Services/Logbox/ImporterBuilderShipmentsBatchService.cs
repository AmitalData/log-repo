using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.Server.Tools;
using Logitude.Server.Tools.QueueService;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationWorkerRole.Services.Logbox
{
    public class ImporterBuilderShipmentsBatchService
    {
        private ShipmentQuery shipmentQuery;
        private List<Shipment> shipments;
        private ImporterShipmentsBatchService importerShipmentsBatchService;
        public ImporterBuilderShipmentsBatchService(ImporterShipmentsBatchService importerShipmentsBatchService)
        {
            this.importerShipmentsBatchService = importerShipmentsBatchService;
            shipmentQuery = new ShipmentQuery(importerShipmentsBatchService.tenant);
        }

        public List<Shipment> GetAllShipmentsFromToDate()
        {
            DateTime fromDate = (DateTime)importerShipmentsBatchService.customerTenantAccessCardsBatch.FromDatetime;
            DateTime toDate = (DateTime)importerShipmentsBatchService.customerTenantAccessCardsBatch.ToDatetime;
            string targetCustomerId = importerShipmentsBatchService.customerTenantAccessCard.CustomerId;
            IQueryable<Shipment> IQueryableShipmentsFromToDate = shipmentQuery.GetIQueryableShipmentsByTenantAndCreateDate(importerShipmentsBatchService.tenant, fromDate, toDate);
            shipments = IQueryableShipmentsFromToDate.Where(shipment => shipment.CustomerId == targetCustomerId).ToList();

            return shipments;
        }

        public List<Shipment> Run()
        {
            APILogsPM LogPM = importerShipmentsBatchService.GetNewLogPM();
            List<Shipment> allowedShipments = GetAllowedShipmentsForLogBox();
            importerShipmentsBatchService.CreateAPILog(allowedShipments.Select(shipment => shipment.Id).ToList(), LogPM, "Shipment");

            APILogsUtility.UpdateAPILogStatus(LogPM.Id, importerShipmentsBatchService.tenant, "D", allowedShipments.Count(), DateTime.Now, DateTime.UtcNow, "Sending Schedual Shipments Done Successfully", null, null, null, "");
            
            return allowedShipments;
        }

        private List<Shipment> GetAllowedShipmentsForLogBox()
        {
            TenantPM tenantPM = TenantQuery.GetSingleTenantPM(importerShipmentsBatchService.tenant, false);
            List<Shipment> allowedShipments = new List<Shipment>();

            foreach (Shipment shipment in shipments)
            {
                allowedShipments = TryToSendImporterShipmentQueue(tenantPM, allowedShipments, shipment);
            }

            return allowedShipments;
        }

        private List<Shipment> TryToSendImporterShipmentQueue(TenantPM tenantPM, List<Shipment> shipments, Shipment shipment)
        {
            if (!IsAllowedShipmentForLogBox(tenantPM, shipment)) return shipments;

            List<Shipment> allowedShipments = shipments;
            allowedShipments.Add(shipment);
            SendImporterShipmentQueue(shipment);

            return allowedShipments;
        }

        private bool IsAllowedShipmentForLogBox(TenantPM tenantPM, Shipment shipment)
        {
            if (shipment == null) return false;
            if (shipment.IsCancelled) return false;
            if (!tenantPM.CustomerTenantShareCustomsFile) return false;
            if (importerShipmentsBatchService.customerTenantAccessCard.LastMappingDateTime != null && shipment.CreateDateTime <= importerShipmentsBatchService.customerTenantAccessCard.LastMappingDateTime) return false;

            CustomerTenantAccessQuery customerTenantAccessQuery = new CustomerTenantAccessQuery(importerShipmentsBatchService.tenant);
            CustomerTenantAccessInfo customerTenantAccess = customerTenantAccessQuery.GetCustomerTenantAccessInfo(importerShipmentsBatchService.tenant, shipment.CustomerId);

            if (customerTenantAccess == null) return false;
            if (!customerTenantAccess.HasAccess) return false;

            PrivateLabelShipmentService privateLabelShipmentService = new PrivateLabelShipmentService(tenantPM, shipment, customerTenantAccess);

            if (!privateLabelShipmentService.IsShipmentsAllowedForLogBox()) return false;

            return true;
        }

        private void SendImporterShipmentQueue(Shipment shipment)
        {
            IQueueService queueService = new DbQueueService();
            queueService.InitializeQueue("ImportersShipmentsBatchQueue", 0);
            queueService.Send(new Dictionary<string, string>() { { "ShipmentId", shipment.Id }, { "ImporterTenant", importerShipmentsBatchService.customerTenantAccessInfo.CustomerTenant.ToString() }, { "Tenant", importerShipmentsBatchService.tenant.ToString() }, { "BatchNumber", importerShipmentsBatchService.BatchNumber } }, importerShipmentsBatchService.tenant, null, importerShipmentsBatchService.CustomerId, importerShipmentsBatchService.BatchNumber);
        }
    }
}
