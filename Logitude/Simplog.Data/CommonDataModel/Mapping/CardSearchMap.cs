using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class CardSearchMap : EntityTypeConfiguration<CardSearch>
    {
        public CardSearchMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Keyword).IsRequired().HasMaxLength(100).IsUnicode(true);
            this.Property(t => t.CardId).HasMaxLength(15).IsRequired().IsUnicode(false);
            this.Property(t => t.PartnerTypeId).IsRequired().IsFixedLength().HasMaxLength(2).IsUnicode(false);


            this.ToTable("CardSearches");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Keyword).HasColumnName("Keyword");
            this.Property(t => t.CardId).HasColumnName("CardId");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.RecordDate).HasColumnName("RecordDate");
            this.Property(t => t.Weight).HasColumnName("Weight");
            this.Property(t => t.PartnerTypeId).HasColumnName("PartnerTypeId");
            this.Property(t => t.InActive).HasColumnName("InActive");
            this.Property(t => t.IsCustomer).HasColumnName("IsCustomer");

            

            this.HasRequired(t => t.Card).WithMany().HasForeignKey(d => d.CardId);


        }
    }
}
