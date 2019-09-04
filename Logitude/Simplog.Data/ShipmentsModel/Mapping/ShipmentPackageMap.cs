using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Simplog.Data.ShipmentsModel.Mapping
{
    public class ShipmentPackageMap : EntityTypeConfiguration<ShipmentPackage>
    {
        public ShipmentPackageMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Description).HasMaxLength(2000).IsUnicode(false);
            this.Property(t => t.PackageTypeId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ContainerNumber).HasMaxLength(20).IsUnicode(false);
            this.Property(t => t.ShipperSeal).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.UnNumber).HasMaxLength(4).IsUnicode(false);
            this.Property(t => t.ClassNumber).HasMaxLength(10).IsUnicode(false);
            this.Property(t => t.CarrierSeal).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.MarksAndNumbers).HasMaxLength(350).IsUnicode(false);
            this.Property(t => t.PackagingGroup).HasMaxLength(10).IsUnicode(false);
            this.Property(t => t.IMDGCode).HasMaxLength(4).IsUnicode(false);
            this.Property(t => t.FlashPoint).HasMaxLength(8).IsUnicode(false);
            this.Property(t => t.Temperature).HasMaxLength(8).IsUnicode(false);
            this.Property(t => t.Harmonize).HasMaxLength(60).IsUnicode(false);
            this.Property(t => t.ShipmentId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.MaterialDescription).HasMaxLength(30).IsUnicode(false);
            this.Property(t => t.OriginalShipmentPackageId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.CommodityId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.NumberOfInsidePackages).IsRequired();
            this.Property(t => t.NumberOfInsidePackagesDetails).HasMaxLength(500).IsUnicode(false);
            this.Property(t => t.DeliveryId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.DeliveryTo).HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.DeliveryFrom).HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.EmptyContainerReturnId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.EmptyContainerReturnTo).HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.EmptyContainerReturnFrom).HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.Reference1).HasMaxLength(2000).IsUnicode(false);
            this.Property(t => t.Reference2).HasMaxLength(2000).IsUnicode(false);
            this.Property(t => t.Reference3).HasMaxLength(2000).IsUnicode(false);
            this.Property(t => t.Reference4).HasMaxLength(2000).IsUnicode(false);
            this.Property(t => t.CommodityNumber).HasMaxLength(20).IsUnicode(false);
            this.Property(t => t.CommodityName).HasMaxLength(250).IsUnicode(false);
            this.Property(t => t.CeficClass).HasMaxLength(25).IsUnicode(false);
            this.Property(t => t.KelmerCode).HasMaxLength(25).IsUnicode(false);
            this.Property(t => t.EMS).HasMaxLength(25).IsUnicode(false);
            this.Property(t => t.ProperShippingName).HasMaxLength(25).IsUnicode(false);
            this.Property(t => t.Notes).HasMaxLength(1000).IsUnicode(true);
            this.Property(t => t.TemperatureUnitCode).HasMaxLength(3).IsUnicode(false);
            this.Property(t => t.LastStatusCode).HasMaxLength(2).IsUnicode(false);
            this.Property(t => t.DeliveryTransportModeCode).HasMaxLength(4).IsUnicode(false);
            this.Property(t => t.ECRTransportModeCode).HasMaxLength(4).IsUnicode(false);
            this.Property(t => t.FlashPointTemperatureUnitCode).HasMaxLength(3).IsUnicode(false);
            this.Property(t => t.Routing).HasMaxLength(20).IsUnicode(false);
            this.Property(t => t.RoutingIds).HasMaxLength(50).IsUnicode(false);
            this.Property(t => t.VoyageTripNumber).HasMaxLength(10).IsUnicode(false);

            this.Property(t => t.Make).HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.Model).HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.Year).HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.Color).HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.ChassisNumber).HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.RegistrationNumber).HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.CountryId).HasMaxLength(15).IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("ShipmentPackages");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.PackageTypeId).HasColumnName("PackageTypeId");
            this.Property(t => t.ContainerNumber).HasColumnName("ContainerNumber");
            this.Property(t => t.ShipperSeal).HasColumnName("ShipperSeal");
            this.Property(t => t.Quantity).HasColumnName("Quantity");
            this.Property(t => t.Weight).HasColumnName("Weight");
            this.Property(t => t.Volume).HasColumnName("Volume");
            this.Property(t => t.Tare).HasColumnName("Tare");
            this.Property(t => t.Height).HasColumnName("Height");
            this.Property(t => t.Width).HasColumnName("Width");
            this.Property(t => t.Length).HasColumnName("Length");
            this.Property(t => t.UnNumber).HasColumnName("UnNumber");
            this.Property(t => t.ClassNumber).HasColumnName("ClassNumber");
            this.Property(t => t.Temperature).HasColumnName("Temperature");
            this.Property(t => t.Ventilation).HasColumnName("Ventilation");
            this.Property(t => t.CarrierSeal).HasColumnName("CarrierSeal");
            this.Property(t => t.SOC).HasColumnName("SOC");
            this.Property(t => t.MarksAndNumbers).HasColumnName("MarksAndNumbers");
            this.Property(t => t.PackagingGroup).HasColumnName("PackagingGroup");
            this.Property(t => t.IMDGCode).HasColumnName("IMDGCode");
            this.Property(t => t.FlashPoint).HasColumnName("FlashPoint");
            this.Property(t => t.Harmonize).HasColumnName("Harmonize");
            this.Property(t => t.ShipmentId).HasColumnName("ShipmentId");
            this.Property(t => t.MaterialDescription).HasColumnName("MaterialDescription");
            this.Property(t => t.IsDangerous).HasColumnName("IsDangerous");
            this.Property(t => t.OriginalShipmentPackageId).HasColumnName("OriginalShipmentPackageId");
            this.Property(t => t.VolumetricWeight).HasColumnName("VolumetricWeight");
            this.Property(t => t.CommodityId).HasColumnName("CommodityId");
            this.Property(t => t.NumberOfInsidePackages).HasColumnName("NumberOfInsidePackages");
            this.Property(t => t.NumberOfInsidePackagesDetails).HasColumnName("NumberOfInsidePackagesDetails");
            this.Property(t => t.IsDeliveryFU).HasColumnName("IsDeliveryFU");
            this.Property(t => t.DeliveryId).HasColumnName("DeliveryId");
            this.Property(t => t.DeliveryETD).HasColumnName("DeliveryETD");
            this.Property(t => t.DeliveryATD).HasColumnName("DeliveryATD");
            this.Property(t => t.DeliveryETA).HasColumnName("DeliveryETA");
            this.Property(t => t.DeliveryATA).HasColumnName("DeliveryATA");
            this.Property(t => t.DeliveryTo).HasColumnName("DeliveryTo");
            this.Property(t => t.DeliveryFrom).HasColumnName("DeliveryFrom");
            this.Property(t => t.IsEmptyContainerReturnFU).HasColumnName("IsEmptyContainerReturnFU");
            this.Property(t => t.EmptyContainerReturnId).HasColumnName("EmptyContainerReturnId");
            this.Property(t => t.EmptyContainerReturnETD).HasColumnName("EmptyContainerReturnETD");
            this.Property(t => t.EmptyContainerReturnATD).HasColumnName("EmptyContainerReturnATD");
            this.Property(t => t.EmptyContainerReturnETA).HasColumnName("EmptyContainerReturnETA");
            this.Property(t => t.EmptyContainerReturnATA).HasColumnName("EmptyContainerReturnATA");
            this.Property(t => t.EmptyContainerReturnTo).HasColumnName("EmptyContainerReturnTo");
            this.Property(t => t.EmptyContainerReturnFrom).HasColumnName("EmptyContainerReturnFrom");
            this.Property(t => t.Reference1).HasColumnName("Reference1");
            this.Property(t => t.Reference2).HasColumnName("Reference2");
            this.Property(t => t.Reference3).HasColumnName("Reference3");
            this.Property(t => t.Reference4).HasColumnName("Reference4");
            this.Property(t => t.CommodityNumber).HasColumnName("CommodityNumber");
            this.Property(t => t.CommodityName).HasColumnName("CommodityName");
            this.Property(t => t.CeficClass).HasColumnName("CeficClass");
            this.Property(t => t.KelmerCode).HasColumnName("KelmerCode");
            this.Property(t => t.EMS).HasColumnName("EMS");
            this.Property(t => t.ProperShippingName).HasColumnName("ProperShippingName");
            this.Property(t => t.MarinePollutant).HasColumnName("MarinePollutant");
            this.Property(t => t.Notes).HasColumnName("Notes");
            this.Property(t => t.TemperatureUnitCode).HasColumnName("TemperatureUnitCode");
            this.Property(t => t.NonActiveContainer).HasColumnName("NonActiveContainer");
            this.Property(t => t.OnCarriageETD).HasColumnName("OnCarriageETD");
            this.Property(t => t.OnCarriageATD).HasColumnName("OnCarriageATD");
            this.Property(t => t.OnCarriageETA).HasColumnName("OnCarriageETA");
            this.Property(t => t.OnCarriageATA).HasColumnName("OnCarriageATA");
            this.Property(t => t.LastStatusCode).HasColumnName("LastStatusCode");
            this.Property(t => t.LastStatusDate).HasColumnName("LastStatusDate");
            this.Property(t => t.DeliveryTransportModeCode).HasColumnName("DeliveryTransportModeCode");
            this.Property(t => t.ECRTransportModeCode).HasColumnName("ECRTransportModeCode");
            this.Property(t => t.FlashPointTemperatureUnitCode).HasColumnName("FlashPointTemperatureUnitCode");
            this.Property(t => t.IsMultiHarmonize).HasColumnName("IsMultiHarmonize");
            this.Property(t => t.ETD).HasColumnName("ETD");
            this.Property(t => t.ETA).HasColumnName("ETA");
            this.Property(t => t.Routing).HasColumnName("Routing");
            this.Property(t => t.RoutingIds).HasColumnName("RoutingIds");
            this.Property(t => t.VoyageTripNumber).HasColumnName("VoyageTripNumber");
            this.Property(t => t.HasContainerException).HasColumnName("HasContainerException");

            this.Property(t => t.Make).HasColumnName("Make");
            this.Property(t => t.Model).HasColumnName("Model");
            this.Property(t => t.Year).HasColumnName("Year");
            this.Property(t => t.Color).HasColumnName("Color");
            this.Property(t => t.ChassisNumber).HasColumnName("ChassisNumber");
            this.Property(t => t.RegistrationNumber).HasColumnName("RegistrationNumber");
            this.Property(t => t.CountryId).HasColumnName("CountryId");

            this.HasOptional(t => t.Shipment).WithMany().HasForeignKey(d => d.ShipmentId);
            this.HasOptional(t => t.PackageType).WithMany().HasForeignKey(d => d.PackageTypeId);
            this.HasOptional(t => t.Commodity).WithMany().HasForeignKey(d => d.CommodityId);
            this.HasOptional(t => t.Delivery).WithMany().HasForeignKey(d => d.DeliveryId);
            this.HasOptional(t => t.EmptyContainerReturn).WithMany().HasForeignKey(d => d.EmptyContainerReturnId);
            this.HasOptional(t => t.TemperatureUnit).WithMany().HasForeignKey(d => d.TemperatureUnitCode);
            this.HasOptional(t => t.LastStatus).WithMany().HasForeignKey(d => d.LastStatusCode);
            this.HasOptional(t => t.DeliveryTransportMode).WithMany().HasForeignKey(d => d.DeliveryTransportModeCode);
            this.HasOptional(t => t.ECRTransportMode).WithMany().HasForeignKey(d => d.ECRTransportModeCode);
            this.HasOptional(t => t.FlashPointTemperatureUnit).WithMany().HasForeignKey(d => d.FlashPointTemperatureUnitCode);

            this.HasOptional(t => t.Country).WithMany().HasForeignKey(d => d.CountryId);

        }
    }
}
