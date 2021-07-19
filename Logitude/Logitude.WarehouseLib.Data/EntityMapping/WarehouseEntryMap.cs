using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System.Data.Entity.ModelConfiguration;
using Logitude.WarehouseLib.Data.EntityPOCOs;
using Logitude.WarehouseLib.Data;
 
namespace Logitude.WarehouseLib.Data.EntityMapping
{
 
    public class WarehouseEntryMap : EntityTypeConfiguration<WarehouseEntry>
    {
	    string dbms;
        public WarehouseEntryMap()
        { 
				this.ToTable("WarehouseEntries");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.CreateDate).HasColumnName("CreateDate").IsRequired();

            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate").IsRequired();

            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.EntryNumber).HasColumnName("EntryNumber").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.CustomerId).HasColumnName("CustomerId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ShipmentId).HasColumnName("ShipmentId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ShipmentNumber).HasColumnName("ShipmentNumber").HasMaxLength(100).IsUnicode(false);

            this.Property(t => t.WarehouseId).HasColumnName("WarehouseId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ExpectedEntryDate).HasColumnName("ExpectedEntryDate");

            this.Property(t => t.ActualEntryDate).HasColumnName("ActualEntryDate");

            this.Property(t => t.ReceivedBy).HasColumnName("ReceivedBy").HasMaxLength(40).IsUnicode(false);

            this.Property(t => t.SpecialInstruction).HasColumnName("SpecialInstruction").HasMaxLength(250).IsUnicode(false);

            this.Property(t => t.StatusCode).HasColumnName("StatusCode").IsRequired().HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.TotalPieces).HasColumnName("TotalPieces").IsRequired();

            this.Property(t => t.TotalGrossWeight).HasColumnName("TotalGrossWeight").IsRequired().HasPrecision(18, 3);

            this.Property(t => t.GrossWeightUnitCode).HasColumnName("GrossWeightUnitCode").IsRequired().HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.TotalVolume).HasColumnName("TotalVolume").IsRequired().HasPrecision(18, 3);

            this.Property(t => t.VolumeUnitCode).HasColumnName("VolumeUnitCode").IsRequired().HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.Notes).HasColumnName("Notes").HasMaxLength(1000).IsUnicode(true);

            this.Property(t => t.CustomerRef1).HasColumnName("CustomerRef1").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.CustomerRef2).HasColumnName("CustomerRef2").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.HouseNumber).HasColumnName("HouseNumber").HasMaxLength(20).IsUnicode(false);

            this.Property(t => t.MasterNumber).HasColumnName("MasterNumber").HasMaxLength(30).IsUnicode(false);

            this.Property(t => t.DimensionsUnitCode).HasColumnName("DimensionsUnitCode").IsRequired().HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.ShipmentLevelCode).HasColumnName("ShipmentLevelCode").HasMaxLength(1).IsUnicode(false);

            this.Property(t => t.TransportModeId).HasColumnName("TransportModeId").HasMaxLength(1).IsFixedLength();

            this.Property(t => t.FromPortId).HasColumnName("FromPortId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ToPortId).HasColumnName("ToPortId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.TruckerId).HasColumnName("TruckerId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.TruckerReference).HasColumnName("TruckerReference").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ShipperId).HasColumnName("ShipperId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.DirectionId).HasColumnName("DirectionId").HasMaxLength(1).IsFixedLength();

            this.Property(t => t.ShipmentTypeId).HasColumnName("ShipmentTypeId").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.EntryReference).HasColumnName("EntryReference").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").HasMaxLength(1000).IsUnicode(true);

            this.Property(t => t.ConnectedToShipment).HasColumnName("ConnectedToShipment");

            this.Property(t => t.FromAddressId).HasColumnName("FromAddressId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ToAddressId).HasColumnName("ToAddressId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ConsigneeId).HasColumnName("ConsigneeId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ShipperReference1).HasColumnName("ShipperReference1").HasMaxLength(50).IsUnicode(false);

            this.Property(t => t.ConsigneeReference1).HasColumnName("ConsigneeReference1").HasMaxLength(50).IsUnicode(false);

            this.Property(t => t.ConsigneeReference2).HasColumnName("ConsigneeReference2").HasMaxLength(50).IsUnicode(false);

            this.Property(t => t.ShipperReference2).HasColumnName("ShipperReference2").HasMaxLength(50).IsUnicode(false);

            this.Property(t => t.ShipperName).HasColumnName("ShipperName").HasMaxLength(200).IsUnicode(true);

            this.Property(t => t.ConsigneeName).HasColumnName("ConsigneeName").HasMaxLength(200).IsUnicode(true);

            this.Property(t => t.Manufacturer).HasColumnName("Manufacturer").HasMaxLength(70).IsUnicode(false);

            this.Property(t => t.FromPartnerId).HasColumnName("FromPartnerId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ToPartnerId).HasColumnName("ToPartnerId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ChargeableWeightUnitCode).HasColumnName("ChargeableWeightUnitCode").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.TotalVolumetricWeight).HasColumnName("TotalVolumetricWeight").HasPrecision(18, 3);

            this.Property(t => t.LastStatusUpdateDate).HasColumnName("LastStatusUpdateDate");

            this.Property(t => t.ConnectedTo).HasColumnName("ConnectedTo").HasMaxLength(100).IsUnicode(false);

            this.Property(t => t.Ratio).HasColumnName("Ratio");

            this.Property(t => t.ToTypeCode).HasColumnName("ToTypeCode").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.FromTypeCode).HasColumnName("FromTypeCode").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.FromCountryId).HasColumnName("FromCountryId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ToCountryId).HasColumnName("ToCountryId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.MasterShipmentNumber).HasColumnName("MasterShipmentNumber").HasMaxLength(100).IsUnicode(false);
        }
    }
}
	 