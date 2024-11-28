using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Simplog.Data.ShipmentsModel.Mapping
{
    public class ShipmentPickUpDeliveryMap : EntityTypeConfiguration<ShipmentPickUpDelivery>
    {
        public ShipmentPickUpDeliveryMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.CarrierNumber).HasMaxLength(30).IsUnicode(false);
            this.Property(t => t.Notes).HasMaxLength(2000).IsUnicode(true);
            this.Property(t => t.ShipmentId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.PickUpDeliveryNumber).IsRequired().HasMaxLength(25).IsUnicode(false);
            this.Property(t => t.FromPartnerCardId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.FromPortId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Driver).HasMaxLength(40).IsUnicode(false);
            this.Property(t => t.ToPartnerCardId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ToPortId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.CarrierId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.TruckNumber).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.TrailerNumber).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.PickUpDeliveryTypeCode).IsRequired().HasMaxLength(4).IsUnicode(false);
            this.Property(t => t.PickUpDeliveryFromTypeCode).IsRequired().HasMaxLength(4).IsUnicode(false);
            this.Property(t => t.PickUpDeliveryToTypeCode).IsRequired().HasMaxLength(4).IsUnicode(false);
            this.Property(t => t.ToAddress).HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.FromAddress).HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.EmptyPickupContainerPartnerId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.EmptyPickupDepotReference).HasMaxLength(50).IsUnicode(false);
            this.Property(t => t.EmptyDeliveryContainerPartnerId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.EmptyDeliveryDepotReference).HasMaxLength(50).IsUnicode(false);
            this.Property(t => t.FromAddressId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ToAddressId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.FromAddressCity).HasMaxLength(25).IsUnicode(true);
            this.Property(t => t.FromAddressZipCode).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.FromAddressCountryId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ToAddressCity).HasMaxLength(25).IsUnicode(true);
            this.Property(t => t.ToAddressZipCode).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ToAddressCountryId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.TransportModeCode).HasMaxLength(4).IsUnicode(false);
            this.Property(t => t.ParentPickUpDeliveryId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.StandaloneShipmentId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.StandaloneShipmentNumber).HasMaxLength(20).IsUnicode(false);
            this.Property(t => t.DeliveryContact).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.PackageTypeCode).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.DescriptionOfGoods).HasMaxLength(2000).IsUnicode(false);
            this.Property(t => t.Commodity).HasMaxLength(5).IsUnicode(false);
            this.Property(t => t.ToAddressCityId).HasMaxLength(5).IsUnicode(false);
            this.Property(t => t.FromAddressCityId).HasMaxLength(5).IsUnicode(false);
            this.Property(t => t.ResponsibilityCode).HasMaxLength(1).IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("ShipmentPickUpDeliveries");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.ATD).HasColumnName("ATD");
            this.Property(t => t.ATA).HasColumnName("ATA");
            this.Property(t => t.ETD).HasColumnName("ETD");
            this.Property(t => t.ETA).HasColumnName("ETA");
            this.Property(t => t.CarrierNumber).HasColumnName("CarrierNumber");
            this.Property(t => t.Notes).HasColumnName("Notes");
            this.Property(t => t.ShipmentId).HasColumnName("ShipmentId");
            this.Property(t => t.PickUpDeliveryNumber).HasColumnName("PickUpDeliveryNumber");
            this.Property(t => t.FromPartnerCardId).HasColumnName("FromPartnerCardId");
            this.Property(t => t.FromPortId).HasColumnName("FromPortId");
            this.Property(t => t.Driver).HasColumnName("Driver");
            this.Property(t => t.ToPartnerCardId).HasColumnName("ToPartnerCardId");
            this.Property(t => t.ToPortId).HasColumnName("ToPortId");
            this.Property(t => t.FullResponsibility).HasColumnName("FullResponsibility");
            this.Property(t => t.CarrierId).HasColumnName("CarrierId");
            this.Property(t => t.TruckNumber).HasColumnName("TruckNumber");
            this.Property(t => t.TrailerNumber).HasColumnName("TrailerNumber");
            this.Property(t => t.PickUpDeliveryTypeCode).HasColumnName("PickUpDeliveryTypeCode");
            this.Property(t => t.PickUpDeliveryFromTypeCode).HasColumnName("PickUpDeliveryFromTypeCode");
            this.Property(t => t.PickUpDeliveryToTypeCode).HasColumnName("PickUpDeliveryToTypeCode");
            this.Property(t => t.ToAddress).HasColumnName("ToAddress");
            this.Property(t => t.FromAddress).HasColumnName("FromAddress");
            this.Property(t => t.EmptyPickupContainerPartnerId).HasColumnName("EmptyPickupContainerPartnerId");
            this.Property(t => t.EmptyPickupDepotReference).HasColumnName("EmptyPickupDepotReference");            
            this.Property(t => t.EmptyDeliveryDepotReference).HasColumnName("EmptyDeliveryDepotReference");
            this.Property(t => t.FromAddressId).HasColumnName("FromAddressId");
            this.Property(t => t.ToAddressId).HasColumnName("ToAddressId");
            this.Property(t => t.FromAddressCity).HasColumnName("FromAddressCity");
            this.Property(t => t.FromAddressZipCode).HasColumnName("FromAddressZipCode");
            this.Property(t => t.FromAddressCountryId).HasColumnName("FromAddressCountryId");
            this.Property(t => t.ToAddressCity).HasColumnName("ToAddressCity");
            this.Property(t => t.ToAddressZipCode).HasColumnName("ToAddressZipCode");
            this.Property(t => t.ToAddressCountryId).HasColumnName("ToAddressCountryId");
            this.Property(t => t.TransportModeCode).HasColumnName("TransportModeCode");
            this.Property(t => t.ParentPickUpDeliveryId).HasColumnName("ParentPickUpDeliveryId");
            this.Property(t => t.ChildPickUpIndex).HasColumnName("ChildPickUpIndex");
            this.Property(t => t.ChildDeliveryIndex).HasColumnName("ChildDeliveryIndex");
            this.Property(t => t.StandaloneShipmentId).HasColumnName("StandaloneShipmentId");
            this.Property(t => t.StandaloneShipmentNumber).HasColumnName("StandaloneShipmentNumber");
            this.Property(t => t.DeliveryContact).HasColumnName("DeliveryContact");
            this.Property(t => t.PackageTypeCode).HasColumnName("PackageTypeCode");
            this.Property(t => t.DeliveryContact).HasColumnName("Quantity");
            this.Property(t => t.DeliveryContact).HasColumnName("GrossWeight");
            this.Property(t => t.DeliveryContact).HasColumnName("Volume");
            this.Property(t => t.DeliveryContact).HasColumnName("CustomerChargeableWeight");
            this.Property(t => t.DeliveryContact).HasColumnName("TruckerChargeableWeight");
            this.Property(t => t.DescriptionOfGoods).HasColumnName("DescriptionOfGoods");
            this.Property(t => t.Commodity).HasColumnName("Commodity");
            this.Property(t => t.DeliveryContact).HasColumnName("CreateDate");
            this.Property(t => t.ToAddressCityId).HasColumnName("ToAddressCityId");
            this.Property(t => t.FromAddressCityId).HasColumnName("FromAddressCityId");
            this.Property(t => t.ResponsibilityCode).HasColumnName("ResponsibilityCode");

            //#if ORACLE_DB
            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms == "oracle")
            {
                this.Property(t => t.EmptyDeliveryContainerPartnerId).HasColumnName("EmptyDelContainerPartnerId");

            }
            //#elseelse
            else
            {
                this.Property(t => t.EmptyDeliveryContainerPartnerId).HasColumnName("EmptyDeliveryContainerPartnerId");
            }

            // Relationships
            this.HasOptional(t => t.FromAddressObj).WithMany().HasForeignKey(d => d.FromAddressId);
            this.HasOptional(t => t.ToAddressObj).WithMany().HasForeignKey(d => d.ToAddressId);
            this.HasOptional(t => t.CarrierCard).WithMany().HasForeignKey(d => d.CarrierId);
            this.HasOptional(t => t.FromPartnerCard).WithMany().HasForeignKey(d => d.FromPartnerCardId);
            this.HasOptional(t => t.ToPartnerCard).WithMany().HasForeignKey(d => d.ToPartnerCardId);
            this.HasRequired(t => t.PickUpDeliveryToType).WithMany().HasForeignKey(d => d.PickUpDeliveryFromTypeCode).WillCascadeOnDelete(false);
            this.HasRequired(t => t.PickUpDeliveryFromType).WithMany().HasForeignKey(d => d.PickUpDeliveryToTypeCode).WillCascadeOnDelete(false);
            this.HasRequired(t => t.PickUpDeliveryType).WithMany().HasForeignKey(d => d.PickUpDeliveryTypeCode);
            this.HasOptional(t => t.FromPort).WithMany().HasForeignKey(d => d.FromPortId);
            this.HasOptional(t => t.ToPort).WithMany().HasForeignKey(d => d.ToPortId);
            this.HasRequired(t => t.Shipment).WithMany().HasForeignKey(d => d.ShipmentId).WillCascadeOnDelete(false);
            this.HasOptional(t => t.FromAddressCountry).WithMany().HasForeignKey(d => d.FromAddressCountryId);
            this.HasOptional(t => t.ToAddressCountry).WithMany().HasForeignKey(d => d.ToAddressCountryId);
            this.HasOptional(t => t.EmptyPickupContainerPartner).WithMany().HasForeignKey(d => d.EmptyPickupContainerPartnerId);
            this.HasOptional(t => t.EmptyDeliveryContainerPartner).WithMany().HasForeignKey(d => d.EmptyDeliveryContainerPartnerId);
            this.HasOptional(t => t.TransportMode).WithMany().HasForeignKey(d => d.TransportModeCode);
            this.HasOptional(t => t.ParentPickUpDelivery).WithMany().HasForeignKey(d => d.ParentPickUpDeliveryId);
            this.HasOptional(t => t.StandaloneShipment).WithMany().HasForeignKey(d => d.StandaloneShipmentId);
            this.HasOptional(t => t.ShipmentDeliveryContact).WithMany().HasForeignKey(d => d.DeliveryContact);
            this.HasOptional(t => t.PackageType).WithMany().HasForeignKey(d => d.PackageTypeCode);
            this.HasOptional(t => t.Responsibility).WithMany().HasForeignKey(d => d.ResponsibilityCode);
        }
    }
}
