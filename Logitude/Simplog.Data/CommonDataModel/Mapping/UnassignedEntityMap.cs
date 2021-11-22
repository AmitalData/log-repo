using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.CommonDataModel.EntityPOCOs;
namespace Simplog.Data.CommonDataModel.Mapping
{
    public class UnassignedEntityMap : EntityTypeConfiguration<UnassignedEntity>
    {
        public UnassignedEntityMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Tenant).IsRequired();
            this.Property(t => t.ObjectTableId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.UnassignedCode).HasMaxLength(25).IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("UnassignedEntitys");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.ObjectTableId).HasColumnName("ObjectTableId");
            this.Property(t => t.UnassignedCode).HasColumnName("UnassignedCode");

            // Relationships
            this.HasOptional(t => t.EntityObjectTable).WithMany().HasForeignKey(d => d.ObjectTableId);
        }
    }
}
