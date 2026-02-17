using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.QuoteModel.EntityPOCOs;

namespace Simplog.Data.QuoteModel.Mapping
{
    public class QuotePackageMap : EntityTypeConfiguration<QuotePackage>
    {
        public QuotePackageMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.PackageTypeId)
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.QuoteId)
                .HasMaxLength(15)
                .IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("QuotePackages");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.PackageTypeId).HasColumnName("PackageTypeId");
            this.Property(t => t.Quantity).HasColumnName("Quantity");
            this.Property(t => t.GrossWeight).HasColumnName("GrossWeight");
            this.Property(t => t.Volume).HasColumnName("Volume");
            this.Property(t => t.Height).HasColumnName("Height");
            this.Property(t => t.Width).HasColumnName("Width");
            this.Property(t => t.Length).HasColumnName("Length");
            this.Property(t => t.QuoteId).HasColumnName("QuoteId");
            this.Property(t => t.VolumetricWeight).HasColumnName("VolumetricWeight");

            // Relationships
            this.HasOptional(t => t.PackageType)
                .WithMany()
                .HasForeignKey(d => d.PackageTypeId);

            this.HasOptional(t => t.Quote)
                .WithMany()
                .HasForeignKey(d => d.QuoteId);

        }
    }
}
