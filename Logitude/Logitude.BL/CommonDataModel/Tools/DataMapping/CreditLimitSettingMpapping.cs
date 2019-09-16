using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.Tools.DataMapping
{
    public class CreditLimitSettingMpapping
    {
        public static void MapEntity(CreditLimitSettingPM entityPM, CreditLimitSetting entityPOCO, bool isNewEntity)
        {
            if (isNewEntity)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
            }

            entityPOCO.InvoiceCreationBlock = entityPM.InvoiceCreationBlock;
            entityPOCO.InvoiceCreationWarning = entityPM.InvoiceCreationWarning;
            entityPOCO.IsCreditLimitEnabled = entityPM.IsCreditLimitEnabled;
            entityPOCO.ShipmentCreationBlock = entityPM.ShipmentCreationBlock;
            entityPOCO.CustomersShipmentsBlock = entityPM.CustomersShipmentsBlock;
            entityPOCO.AgentsShipmentsBlock = entityPM.AgentsShipmentsBlock;
            entityPOCO.ShipperConsigneeShipmentBlock = entityPM.ShipperConsigneeShipmentBlock;
            entityPOCO.CustomsAgentsShipmentsBlock = entityPM.CustomsAgentsShipmentsBlock;
            entityPOCO.ShippingAgentsShipmentsBlock = entityPM.ShippingAgentsShipmentsBlock;
            entityPOCO.AirlinesShipmentsBlock = entityPM.AirlinesShipmentsBlock;
            entityPOCO.ShippingLinesShipmentsBlock = entityPM.ShippingLinesShipmentsBlock;
            entityPOCO.TruckersShipmentsBlock = entityPM.TruckersShipmentsBlock;
            entityPOCO.VendorsShipmentsBlock = entityPM.VendorsShipmentsBlock;
            entityPOCO.WarehousesShipmentsBlock = entityPM.WarehousesShipmentsBlock;
            entityPOCO.CustomersInvoicesBlock = entityPM.CustomersInvoicesBlock;
            entityPOCO.AgentsInvoicesBlock = entityPM.AgentsInvoicesBlock;
            entityPOCO.ShipperConsigneeInvoiceBlock = entityPM.ShipperConsigneeInvoiceBlock;
            entityPOCO.CustomsAgentsInvoicesBlock = entityPM.CustomsAgentsInvoicesBlock;
            entityPOCO.ShippingAgentsInvoicesBlock = entityPM.ShippingAgentsInvoicesBlock;
            entityPOCO.AirlinesInvoicesBlock = entityPM.AirlinesInvoicesBlock;
            entityPOCO.ShippingLinesInvoicesBlock = entityPM.ShippingLinesInvoicesBlock;
            entityPOCO.TruckersInvoicesBlock = entityPM.TruckersInvoicesBlock;
            entityPOCO.VendorsInvoicesBlock = entityPM.VendorsInvoicesBlock;
            entityPOCO.WarehousesInvoicesBlock = entityPM.WarehousesInvoicesBlock;
        }
    }
}
