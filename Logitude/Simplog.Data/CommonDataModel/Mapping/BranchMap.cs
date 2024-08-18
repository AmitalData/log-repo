using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class BranchMap : EntityTypeConfiguration<Branch>
    {
        public BranchMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.EnglishName).IsRequired().HasMaxLength(40).IsUnicode(false);
            this.Property(t => t.LocalName).HasMaxLength(40).IsUnicode(true);
            this.Property(t => t.Notes).HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.SearchFields).HasMaxLength(1000).IsUnicode(true);
            this.Property(t => t.Code).HasMaxLength(10).IsUnicode(false);
            this.Property(t => t.ExternalId).HasMaxLength(25).IsUnicode(true);
            this.Property(t => t.AddressId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Signature).HasMaxLength(60).IsUnicode(false);
            this.Property(t => t.INTTRAId).HasMaxLength(35).IsUnicode(false);
            this.Property(t => t.INTTRAContactId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.INTTRAAlias).HasMaxLength(35).IsUnicode(false);
			this.Property(t => t.CounterCode).HasMaxLength(5).IsUnicode(false);

			// Table & Column Mappings
			this.ToTable("Branches");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.EnglishName).HasColumnName("EnglishName");
            this.Property(t => t.LocalName).HasColumnName("LocalName");
            this.Property(t => t.Notes).HasColumnName("Notes");
            this.Property(t => t.InActive).HasColumnName("InActive");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.ExternalId).HasColumnName("ExternalId");
            this.Property(t => t.AddressId).HasColumnName("AddressId");
            this.Property(t => t.Signature).HasColumnName("Signature");
            this.Property(t => t.INTTRAId).HasColumnName("INTTRAId");
            this.Property(t => t.INTTRAContactId).HasColumnName("INTTRAContactId");
            this.Property(t => t.INTTRAAlias).HasColumnName("INTTRAAlias"); 
            this.Property(t => t.CounterCode).HasColumnName("CounterCode");
            this.Property(t => t.AutomaticLastUpdateDate).HasColumnName("AutomaticLastUpdateDate");


            this.HasOptional(t => t.Address).WithMany().HasForeignKey(d => d.AddressId);
            this.HasOptional(t => t.INTTRAContact).WithMany().HasForeignKey(d => d.INTTRAContactId);
        }
    }
}
