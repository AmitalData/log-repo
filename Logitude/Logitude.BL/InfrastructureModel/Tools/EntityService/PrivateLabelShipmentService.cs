using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
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
        private CustomerTenantAccessInfo customerTenantAccessInfo;

        public PrivateLabelShipmentService(TenantPM tenantPM, ShipmentPM shipmentPM, CustomerTenantAccessInfo customerTenantAccessInfo)
        {
            this.tenantPM = tenantPM; 
            this.shipmentPM = shipmentPM;
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
            return (IsCustomsShipmentsAllowedForLogBox() || IsExportShipmentsAllowedForLogBox() || IsImportShipmentsAllowedForLogBox());
        }
        public bool IsImportShipmentsAllowedForLogBox()
        {
            return (tenantPM.CustomerTenantShareImportFile && shipmentPM.DirectionId.ToUpper() == "I");
        } 
        public bool IsCustomsShipmentsAllowedForLogBox()
        {
            return (tenantPM.CustomerTenantShareCustomsFile && customerTenantAccessInfo.IsCustomsActivated && shipmentPM.DirectionId.ToUpper() == "C");
        }
        public bool IsExportShipmentsAllowedForLogBox()
        {
            return (tenantPM.CustomerTenantShareExportFile && customerTenantAccessInfo.IsExportActivated && shipmentPM.DirectionId.ToUpper() == "E");

        }
        private TenantPM GetTenantPM(int tenant) { 
            TenantPM TenantPM = TenantQuery.GetSingleTenantPM(tenant, false);
            return TenantPM;
        }
     
    }
}
