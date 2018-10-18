using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.Mapping
{
    public class ObjectFieldModificationMap : EntityTypeConfiguration<ObjectFieldModification>
    {
        public ObjectFieldModificationMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.ObjectFieldId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("ObjectFieldModifications");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.IsRequired).HasColumnName("IsRequired");
            this.Property(t => t.MaxLength).HasColumnName("MaxLength");
            this.Property(t => t.MinLength).HasColumnName("MinLength");
            this.Property(t => t.ObjectFieldId).HasColumnName("ObjectFieldId");
            this.Property(t => t.UpdateDateGMT).HasColumnName("UpdateDateGMT");
            // Relationships
            //this.HasRequired(t => t.ObjectField)
            //    .WithMany(t => t.ObjectFieldModifications)
            //    .HasForeignKey(d => d.ObjectFieldId);

        }
    }
}
