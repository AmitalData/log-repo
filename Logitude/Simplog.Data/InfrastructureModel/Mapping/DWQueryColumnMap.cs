using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.Mapping
{
    public class DWQueryColumnMap : EntityTypeConfiguration<DWQueryColumn>
    {
        public DWQueryColumnMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.DWQueryId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.DWObjectFieldId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.UserId)
                .HasMaxLength(15)
                .IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("DWQueryColumns");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.DWQueryId).HasColumnName("DWQueryId");
            this.Property(t => t.DWObjectFieldId).HasColumnName("DWObjectFieldId");
            this.Property(t => t.IndexOrder).HasColumnName("IndexOrder");
            this.Property(t => t.ColumnWidth).HasColumnName("ColumnWidth");
            this.Property(t => t.UserId).HasColumnName("UserId");


            this.HasRequired(t => t.DWObjectField).WithMany().HasForeignKey(d => d.DWObjectFieldId);
            this.HasRequired(t => t.DWQuery).WithMany().HasForeignKey(d => d.DWQueryId);
            this.HasOptional(t => t.User)
                .WithMany()
                .HasForeignKey(d => d.UserId);

        }
    }
}
