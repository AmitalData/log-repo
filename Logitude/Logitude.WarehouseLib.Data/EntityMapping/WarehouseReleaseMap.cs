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
 
    public class WarehouseReleaseMap : EntityTypeConfiguration<WarehouseRelease>
    {
	    string dbms;
        public WarehouseReleaseMap()
        { 
				this.ToTable("WarehouseReleases");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.CreateDate).HasColumnName("CreateDate").IsRequired();

            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate").IsRequired();

            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ReleaseNumber).HasColumnName("ReleaseNumber").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.CustomerId).HasColumnName("CustomerId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ShipmentId).HasColumnName("ShipmentId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ShipmentNumber).HasColumnName("ShipmentNumber").HasMaxLength(100).IsUnicode(false);

            this.Property(t => t.WarehouseId).HasColumnName("WarehouseId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ExpectedReleaseDate).HasColumnName("ExpectedReleaseDate");

            this.Property(t => t.ActualReleaseDate).HasColumnName("ActualReleaseDate");

            this.Property(t => t.ReleaseBy).HasColumnName("ReleaseBy").HasMaxLength(40).IsUnicode(false);

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

            this.Property(t => t.ShipmentTypeId).HasColumnName("ShipmentTypeId").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.TransportModeId).HasColumnName("TransportModeId").HasMaxLength(1).IsUnicode(false);

            this.Property(t => t.ShipmentLevelCode).HasColumnName("ShipmentLevelCode").HasMaxLength(1).IsUnicode(false);

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").HasMaxLength(1000).IsUnicode(true);

            this.Property(t => t.DirectionId).HasColumnName("DirectionId").HasMaxLength(1).IsUnicode(false);

            this.Property(t => t.TotalQuantity).HasColumnName("TotalQuantity");

            this.Property(t => t.ChargeableWeightUnitCode).HasColumnName("ChargeableWeightUnitCode").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.ConnectedTo).HasColumnName("ConnectedTo").HasMaxLength(100).IsUnicode(false);

            this.Property(t => t.FromPortId).HasColumnName("FromPortId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ToPortId).HasColumnName("ToPortId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.CustomerAddressId).HasColumnName("CustomerAddressId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.TotalVolumetricWeight).HasColumnName("TotalVolumetricWeight").HasPrecision(18, 3);

            this.Property(t => t.Ratio).HasColumnName("Ratio");

            this.Property(t => t.ToTypeCode).HasColumnName("ToTypeCode").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.ToPartnerCardId).HasColumnName("ToPartnerCardId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ToAddressId).HasColumnName("ToAddressId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ToAddressZipCode).HasColumnName("ToAddressZipCode").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ToAddressCity).HasColumnName("ToAddressCity").HasMaxLength(25).IsUnicode(true);

            this.Property(t => t.ToAddressCountryId).HasColumnName("ToAddressCountryId").HasMaxLength(15).IsUnicode(false);
        }
    }
}
	 