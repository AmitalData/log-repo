using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class CarrierAreaMap : EntityTypeConfiguration<CarrierArea>
    {
        public CarrierAreaMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Name).IsRequired().HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.CreatedByUserId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.UpdatedByUserId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.CarrierId).HasMaxLength(15).IsUnicode(false);
            
            this.ToTable("CarrierAreas");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId");
            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            this.Property(t => t.CarrierId).HasColumnName("CarrierId");
            
            this.HasOptional(t => t.CreatedByUser).WithMany().HasForeignKey(d => d.CreatedByUserId);
            this.HasOptional(t => t.UpdatedByUser).WithMany().HasForeignKey(d => d.UpdatedByUserId);
            this.HasRequired(t => t.Carrier).WithMany().HasForeignKey(d => d.CarrierId).WillCascadeOnDelete(false);
        }
    }
}
