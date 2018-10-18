using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.Mapping
{
    public class EntityLastUpdateMap : EntityTypeConfiguration<EntityLastUpdate>
    {
        public EntityLastUpdateMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.EntityId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.ObjectTableId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.UpdatedByUserId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.EntityGUID)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("EntityLastUpdates");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.EntityId).HasColumnName("EntityId");
            this.Property(t => t.ObjectTableId).HasColumnName("ObjectTableId");
            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            this.Property(t => t.EntityGUID).HasColumnName("EntityGUID");

            // Relationships
            //this.HasRequired(t => t.ObjectTable)
            //    .WithMany(t => t.EntityLastUpdates)
            //    .HasForeignKey(d => d.ObjectTableId);
            this.HasRequired(t => t.UpdatedByUser)
                .WithMany()
                .HasForeignKey(d => d.UpdatedByUserId);

        }
    }
}
