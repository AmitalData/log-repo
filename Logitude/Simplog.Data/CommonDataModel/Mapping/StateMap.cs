using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class StateMap : EntityTypeConfiguration<State>
    {
        public StateMap()
        {
            this.HasKey(t => t.Id);

            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Code).IsRequired().HasMaxLength(10).IsUnicode(false);
            this.Property(t => t.EnglishName).IsRequired().HasMaxLength(40).IsUnicode(false);
            this.Property(t => t.LocalName).HasMaxLength(40).IsUnicode(true);
            this.Property(t => t.Notes).HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.CountryId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.SearchFields).HasMaxLength(1000).IsUnicode(true);
            this.Property(t => t.QBOTransactionLocationCode).HasMaxLength(3).IsUnicode(false);
            // Table & Column Mappings
            this.ToTable("States");
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
            this.Property(t => t.QBOTransactionLocationCode).HasColumnName("QBOTransactionLocationCode");
            this.Property(t => t.AutomaticLastUpdateDate).HasColumnName("AutomaticLastUpdateDate"); 

            // Relationships
            this.HasRequired(t => t.Country).WithMany().HasForeignKey(d => d.CountryId);
        }
    }
}
