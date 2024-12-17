using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class TarrifStepMap : EntityTypeConfiguration<TarrifStep>
    {
        public TarrifStepMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.TarrifHeaderId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("TarrifSteps");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.TarrifHeaderId).HasColumnName("TarrifHeaderId");
            this.Property(t => t.Step).HasColumnName("Step").HasPrecision(14, 3);
            this.Property(t => t.UnitPrice).HasColumnName("UnitPrice").HasPrecision(14, 3);
            this.Property(t => t.MinPrice).HasColumnName("MinPrice").HasPrecision(14, 3);
            this.Property(t => t.MaxPrice).HasColumnName("MaxPrice").HasPrecision(14, 3);

            // Relationships
            this.HasRequired(t => t.TarrifHeader)
                .WithMany()
                .HasForeignKey(d => d.TarrifHeaderId);

        }
    }
}
