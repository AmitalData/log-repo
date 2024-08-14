using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class CarrierAreasPortMap : EntityTypeConfiguration<CarrierAreasPort>
    {
        public CarrierAreasPortMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Name).IsRequired().HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.AddedByUserId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.CarrierAreaId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.PortId).HasMaxLength(15).IsUnicode(false);
            
            this.ToTable("CarrierAreasPorts");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.AddedByUserId).HasColumnName("AddedByUserId");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.AddedDate).HasColumnName("AddedDate");
            this.Property(t => t.CarrierAreaId).HasColumnName("CarrierAreaId");
            this.Property(t => t.PortId).HasColumnName("PortId");
            
            this.HasOptional(t => t.AddedByUser).WithMany().HasForeignKey(d => d.AddedByUserId);
            this.HasRequired(t => t.CarrierArea).WithMany().HasForeignKey(d => d.CarrierAreaId).WillCascadeOnDelete(false);
            this.HasOptional(t => t.Port).WithMany().HasForeignKey(d => d.PortId);
        }
    }
}
