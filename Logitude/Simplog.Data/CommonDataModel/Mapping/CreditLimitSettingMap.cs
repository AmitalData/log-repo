using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class CreditLimitSettingMap : EntityTypeConfiguration<CreditLimitSetting>
    {
        public CreditLimitSettingMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);

            this.ToTable("CreditLimitSettings");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.IsCreditLimitEnabled).HasColumnName("IsCreditLimitEnabled");
            this.Property(t => t.InvoiceCreationWarning).HasColumnName("InvoiceCreationWarning");
            this.Property(t => t.InvoiceCreationBlock).HasColumnName("InvoiceCreationBlock");
            this.Property(t => t.ShipmentCreationBlock).HasColumnName("ShipmentCreationBlock");
            this.Property(t => t.CustomersShipmentsBlock).HasColumnName("CustomersShipmentsBlock");
            this.Property(t => t.AgentsShipmentsBlock).HasColumnName("AgentsShipmentsBlock");
            this.Property(t => t.ShipperConsigneeShipmentBlock).HasColumnName("ShipperConsigneeShipmentBlock");
            this.Property(t => t.CustomsAgentsShipmentsBlock).HasColumnName("CustomsAgentsShipmentsBlock");
            this.Property(t => t.ShippingAgentsShipmentsBlock).HasColumnName("ShippingAgentsShipmentsBlock");
            this.Property(t => t.AirlinesShipmentsBlock).HasColumnName("AirlinesShipmentsBlock");
            this.Property(t => t.ShippingLinesShipmentsBlock).HasColumnName("ShippingLinesShipmentsBlock");
            this.Property(t => t.TruckersShipmentsBlock).HasColumnName("TruckersShipmentsBlock");
            this.Property(t => t.VendorsShipmentsBlock).HasColumnName("VendorsShipmentsBlock");
            this.Property(t => t.WarehousesShipmentsBlock).HasColumnName("WarehousesShipmentsBlock");
            this.Property(t => t.CustomersInvoicesBlock).HasColumnName("CustomersInvoicesBlock");
            this.Property(t => t.AgentsInvoicesBlock).HasColumnName("AgentsInvoicesBlock");
            this.Property(t => t.ShipperConsigneeInvoiceBlock).HasColumnName("ShipperConsigneeInvoiceBlock");
            this.Property(t => t.CustomsAgentsInvoicesBlock).HasColumnName("CustomsAgentsInvoicesBlock");
            this.Property(t => t.ShippingAgentsInvoicesBlock).HasColumnName("ShippingAgentsInvoicesBlock");
            this.Property(t => t.AirlinesInvoicesBlock).HasColumnName("AirlinesInvoicesBlock");
            this.Property(t => t.ShippingLinesInvoicesBlock).HasColumnName("ShippingLinesInvoicesBlock");
            this.Property(t => t.TruckersInvoicesBlock).HasColumnName("TruckersInvoicesBlock");
            this.Property(t => t.VendorsInvoicesBlock).HasColumnName("VendorsInvoicesBlock");
            this.Property(t => t.WarehousesInvoicesBlock).HasColumnName("WarehousesInvoicesBlock");
        }
    }
}