using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.Mapping
{
    public class SharedLogisticsUpdateMap : EntityTypeConfiguration<SharedLogisticsUpdate>
    {
        public SharedLogisticsUpdateMap()
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

            this.Property(t => t.EntityId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.ReceivedFrom)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);

            this.Property(t => t.DocumentId)
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.Subject)
                .HasMaxLength(50)
                .IsUnicode(false);

            this.Property(t => t.Status)
                .IsRequired()
                .HasMaxLength(4)
                .IsUnicode(false);

            this.Property(t => t.HandledByUserId)
                .HasMaxLength(15)
                .IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("SharedLogisticsUpdates");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.ObjectTableId).HasColumnName("ObjectTableId");
            this.Property(t => t.EntityId).HasColumnName("EntityId");
            this.Property(t => t.ReceivedDate).HasColumnName("ReceivedDate");
            this.Property(t => t.ReceivedFrom).HasColumnName("ReceivedFrom");
            this.Property(t => t.DocumentId).HasColumnName("DocumentId");
            this.Property(t => t.Subject).HasColumnName("Subject");
            this.Property(t => t.Read).HasColumnName("Read");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.HandledByUserId).HasColumnName("HandledByUserId");
            this.Property(t => t.HandledDate).HasColumnName("HandledDate");

            // Relationships
            this.HasOptional(t => t.Document)
                .WithMany()
                .HasForeignKey(d => d.DocumentId);
            //this.HasRequired(t => t.ObjectTable)
            //    .WithMany(t => t.SharedLogisticsUpdates)
            //    .HasForeignKey(d => d.ObjectTableId);
            //this.HasRequired(t => t.SharedLogisticsUpdateStatus)
            //    .WithMany(t => t.SharedLogisticsUpdates)
            //    .HasForeignKey(d => d.Status);
            this.HasOptional(t => t.HandledByUser)
                .WithMany()
                .HasForeignKey(d => d.HandledByUserId);

        }
    }
}
