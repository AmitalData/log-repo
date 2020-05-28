using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.Mapping
{
    public class ScreenModificationMap : EntityTypeConfiguration<ScreenModification>
    {
        public ScreenModificationMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.ScreenId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.ScreenCode)
               .HasMaxLength(100)
               .IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("ScreenModifications");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.NumberOfColumns).HasColumnName("NumberOfColumns");
            this.Property(t => t.NumberOfRows).HasColumnName("NumberOfRows");
            this.Property(t => t.ScreenId).HasColumnName("ScreenId");
            this.Property(t => t.ScreenCode).HasColumnName("ScreenCode");

            // Relationships
            //this.HasRequired(t => t.Screen)
            //    .WithMany(t => t.ScreenModifications)
            //    .HasForeignKey(d => d.ScreenId);

        }
    }
}
