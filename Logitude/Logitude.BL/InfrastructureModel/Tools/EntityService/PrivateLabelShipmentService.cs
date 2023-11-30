using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.Tools.EntityService
{
    public class PrivateLabelShipmentService
    {
        private TenantPM tenantPM; 
        private ShipmentPM shipmentPM;
        private Shipment shipment;
        private CustomerTenantAccessInfo customerTenantAccessInfo;

        public PrivateLabelShipmentService(TenantPM tenantPM, ShipmentPM shipmentPM, CustomerTenantAccessInfo customerTenantAccessInfo)
        {
            this.tenantPM = tenantPM; 
            this.shipmentPM = shipmentPM;
            this.customerTenantAccessInfo = customerTenantAccessInfo;
        }

        public PrivateLabelShipmentService(TenantPM tenantPM, Shipment shipment, CustomerTenantAccessInfo customerTenantAccessInfo)
        {
            this.tenantPM = tenantPM;
            this.shipment = shipment;
            this.customerTenantAccessInfo = customerTenantAccessInfo;
        }

        public PrivateLabelShipmentService(ShipmentPM shipmentPM, CustomerTenantAccessInfo customerTenantAccessInfo)
        {
            this.tenantPM = this.GetTenantPM(shipmentPM.Tenant); 
            this.shipmentPM = shipmentPM;
            this.customerTenantAccessInfo = customerTenantAccessInfo;
        }
        public bool IsShipmentsAllowedForLogBox()
        {
            return (IsCustomsShipmentsAllowedForLogBox() || IsExportShipmentsAllowedForLogBox());
        } 
        public bool IsCustomFileShipment(ShipmentPM forwarderShipment)
        {
            const string importDirectionId = "I";
            bool isImportShipment = forwarderShipment.DirectionId.ToUpper() == importDirectionId;
            bool hasCustomFileId = !string.IsNullOrEmpty(forwarderShipment.CustomFileId); 
            
            return isImportShipment && hasCustomFileId;
        }
        public bool IsCustomsShipmentsAllowedForLogBox()
        {
            if (shipment != null)
            {
                return (tenantPM.CustomerTenantShareCustomsFile && customerTenantAccessInfo.IsCustomsActivated && shipment.DirectionId.ToUpper() == "C");
            }

            return (tenantPM.CustomerTenantShareCustomsFile && customerTenantAccessInfo.IsCustomsActivated && shipmentPM.DirectionId.ToUpper() == "C");
        }
        public bool IsExportShipmentsAllowedForLogBox()
        {
            if (shipment != null)
            {
                return (tenantPM.CustomerTenantShareExportFile && customerTenantAccessInfo.IsExportActivated && shipment.DirectionId.ToUpper() == "E");
            }

            return (tenantPM.CustomerTenantShareExportFile && customerTenantAccessInfo.IsExportActivated && shipmentPM.DirectionId.ToUpper() == "E");
        }
        private TenantPM GetTenantPM(int tenant) { 
            TenantPM TenantPM = TenantQuery.GetSingleTenantPM(tenant, false);
            return TenantPM;
        }
     
    }
}
