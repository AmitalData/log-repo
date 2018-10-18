using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class CountryMap : EntityTypeConfiguration<Country>
    {
        public CountryMap()
        {
            this.HasKey(t => t.Id);

            this.Property(t => t.Id).IsRequired().IsUnicode(false).HasMaxLength(15);
            this.Property(t => t.Code).IsRequired().IsFixedLength().IsUnicode(false).HasMaxLength(2);
            this.Property(t => t.EnglishName).IsRequired().IsUnicode(false).HasMaxLength(120);
            this.Property(t => t.LocalName).HasMaxLength(120).IsUnicode(true);
            this.Property(t => t.Notes).HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.GlobalZoneId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.SearchFields).HasMaxLength(1000).IsUnicode(true);

            // Table & Column Mappings
            this.ToTable("Countries");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.EnglishName).HasColumnName("EnglishName");
            this.Property(t => t.LocalName).HasColumnName("LocalName");
            this.Property(t => t.AddedManually).HasColumnName("AddedManually");
            this.Property(t => t.InActive).HasColumnName("InActive");
            this.Property(t => t.Notes).HasColumnName("Notes");
            this.Property(t => t.GlobalZoneId).HasColumnName("GlobalZoneId");
            this.Property(t => t.EC).HasColumnName("EC");
            this.Property(t => t.HasStates).HasColumnName("HasStates");
            this.Property(t => t.IsStateRequired).HasColumnName("IsStateRequired");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
            this.Property(t => t.HasCitiesList).HasColumnName("HasCitiesList");

            // Relationships
            this.HasRequired(t => t.GlobalZone).WithMany().HasForeignKey(d => d.GlobalZoneId);

        }
    }
}
