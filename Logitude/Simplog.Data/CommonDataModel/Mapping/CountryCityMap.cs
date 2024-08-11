using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class CountryCityMap : EntityTypeConfiguration<CountryCity>
    {
        public CountryCityMap()
        {
            this.HasKey(t => t.Id);

            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Code).IsRequired().HasMaxLength(15).IsUnicode(true);
            this.Property(t => t.EnglishName).IsRequired().HasMaxLength(80).IsUnicode(true);
            this.Property(t => t.LocalName).HasMaxLength(40).IsUnicode(true);
            this.Property(t => t.Notes).HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.CountryId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.SearchFields).HasMaxLength(1000).IsUnicode(true);
            this.Property(t => t.StateId).HasMaxLength(15).IsUnicode(false);

            this.ToTable("CountryCities");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.EnglishName).HasColumnName("EnglishName");
            this.Property(t => t.LocalName).HasColumnName("LocalName");
            this.Property(t => t.InActive).HasColumnName("InActive");
            this.Property(t => t.Notes).HasColumnName("Notes");
            this.Property(t => t.CountryId).HasColumnName("CountryId");
            this.Property(t => t.AddedManually).HasColumnName("AddedManually");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
            this.Property(t => t.StateId).HasColumnName("StateId");

            // Relationships
            this.HasRequired(t => t.Country).WithMany().HasForeignKey(d => d.CountryId);
            this.HasOptional(t => t.State).WithMany().HasForeignKey(d => d.StateId);
        }
    }
}
