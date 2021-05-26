using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.ShipmentsModel.Mapping
{
    public class ContainerMap : EntityTypeConfiguration<Container>
    {
        public ContainerMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ShipmentPackagesId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.MainCarriageCarrierId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.MainCarriageVesselId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.SearchFields).IsMaxLength().IsUnicode(true);
            this.Property(t => t.ContainerNumber).HasMaxLength(20).IsUnicode(false);
            this.Property(t => t.MainCarriageCarrierNumber).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Master).HasMaxLength(20).IsUnicode(false);

            this.ToTable("Containers");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.ShipmentPackagesId).HasColumnName("Code");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId");
            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId");
            this.Property(t => t.ContainerNumber).HasColumnName("ContainerNumber");
            this.Property(t => t.MainCarriageCarrierId).HasColumnName("MainCarriageCarrierId");
            this.Property(t => t.MainCarriageCarrierNumber).HasColumnName("MainCarriageCarrierNumber");
            this.Property(t => t.MainCarriageATA).HasColumnName("MainCarriageATA");
            this.Property(t => t.MainCarriageATD).HasColumnName("MainCarriageATD");
            this.Property(t => t.MainCarriageETA).HasColumnName("MainCarriageETA");
            this.Property(t => t.MainCarriageETD).HasColumnName("MainCarriageETD");
            this.Property(t => t.DischargeDate).HasColumnName("DischargeDate");
            this.Property(t => t.Master).HasColumnName("Master");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");

            this.HasOptional(t => t.CarrierCard).WithMany().HasForeignKey(d => d.MainCarriageCarrierId);
            this.HasOptional(t => t.ShipmentPackage).WithMany().HasForeignKey(d => d.ShipmentPackagesId);
            this.HasOptional(t => t.VesselCard).WithMany().HasForeignKey(d => d.MainCarriageVesselId);
        }
    }
}
