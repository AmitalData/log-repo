using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class TarrifFromToMap : EntityTypeConfiguration<TarrifFromTo>
    {
        public TarrifFromToMap()
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

            this.Property(t => t.PortId)
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.CountryId)
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.TarrifFromToTypeCode)
                .IsRequired()
                .HasMaxLength(4)
                .IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("TarrifFromToes");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.TarrifHeaderId).HasColumnName("TarrifHeaderId");
            this.Property(t => t.PortId).HasColumnName("PortId");
            this.Property(t => t.CountryId).HasColumnName("CountryId");
            this.Property(t => t.TarrifFromToTypeCode).HasColumnName("TarrifFromToTypeCode");

            // Relationships
            this.HasOptional(t => t.Country)
                .WithMany()
                .HasForeignKey(d => d.CountryId);
            this.HasOptional(t => t.Port)
                .WithMany()
                .HasForeignKey(d => d.PortId);
            this.HasRequired(t => t.TarrifFromToType)
                .WithMany()
                .HasForeignKey(d => d.TarrifFromToTypeCode);
            this.HasRequired(t => t.TarrifHeader)
                .WithMany()
                .HasForeignKey(d => d.TarrifHeaderId);

        }
    }
}
