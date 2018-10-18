using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.Mapping
{
    public class EntityLastActivityMap : EntityTypeConfiguration<EntityLastActivity>
    {
        public EntityLastActivityMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.ObjectTableId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.UserId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.EntityId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.ActivityTypeCode)
                .IsRequired()
                .HasMaxLength(4)
                .IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("EntityLastActivities");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.ActivityDate).HasColumnName("ActivityDate");
            this.Property(t => t.ObjectTableId).HasColumnName("ObjectTableId");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.EntityId).HasColumnName("EntityId");
            this.Property(t => t.ActivityTypeCode).HasColumnName("ActivityTypeCode");

            // Relationships
            //this.HasRequired(t => t.ObjectTable)
            //    .WithMany(t => t.EntityLastAccesses)
            //    .HasForeignKey(d => d.ObjectTableId);
            this.HasRequired(t => t.User)
                .WithMany()
                .HasForeignKey(d => d.UserId);
            //this.HasRequired(t => t.ActivityType)
            //    .WithMany(t => t.EntityLastActivities)
            //    .HasForeignKey(d => d.ActivityTypeCode);

        }
    }
}
