using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class CreditLimitSettingList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public bool IsCreditLimitEnabled { get; set; }
        public bool InvoiceCreationWarning { get; set; }
        public bool InvoiceCreationBlock { get; set; }
        public bool ShipmentCreationBlock { get; set; }
        public bool CustomersShipmentsBlock { get; set; }
        public bool AgentsShipmentsBlock { get; set; }
        public bool ShipperConsigneeShipmentBlock { get; set; }
        public bool CustomsAgentsShipmentsBlock { get; set; }
        public bool ShippingAgentsShipmentsBlock { get; set; }
        public bool AirlinesShipmentsBlock { get; set; }
        public bool ShippingLinesShipmentsBlock { get; set; }
        public bool TruckersShipmentsBlock { get; set; }
        public bool VendorsShipmentsBlock { get; set; }
        public bool WarehousesShipmentsBlock { get; set; }
        public bool CustomersInvoicesBlock { get; set; }
        public bool AgentsInvoicesBlock { get; set; }
        public bool ShipperConsigneeInvoiceBlock { get; set; }
        public bool CustomsAgentsInvoicesBlock { get; set; }
        public bool ShippingAgentsInvoicesBlock { get; set; }
        public bool AirlinesInvoicesBlock { get; set; }
        public bool ShippingLinesInvoicesBlock { get; set; }
        public bool TruckersInvoicesBlock { get; set; }
        public bool VendorsInvoicesBlock { get; set; }
        public bool WarehousesInvoicesBlock { get; set; }
    }
}
