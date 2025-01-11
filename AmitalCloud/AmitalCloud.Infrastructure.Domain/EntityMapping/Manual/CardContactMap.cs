using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;

namespace AmitalCloud.Infrastructure.Domain.EntityMapping
{
    public class CardContactMap : EntityTypeConfiguration<CardContact>
    {
        public CardContactMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.ContactId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.CardId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.InternetAccess)
               .IsRequired();

            this.Property(t => t.LastLoginDate);

            // Table & Column Mappings
            this.ToTable("CardContacts");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ContactId).HasColumnName("ContactId");
            this.Property(t => t.CardId).HasColumnName("CardId");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.InternetAccess).HasColumnName("InternetAccess");
            this.Property(t => t.LastLoginDate).HasColumnName("LastLoginDate");
            this.Property(t => t.IsAirExport).HasColumnName("IsAirExport");
            this.Property(t => t.IsAirImport).HasColumnName("IsAirImport");
            this.Property(t => t.IsInlandExport).HasColumnName("IsInlandExport");
            this.Property(t => t.IsInlandImport).HasColumnName("IsInlandImport");
            this.Property(t => t.IsOceanExport).HasColumnName("IsOceanExport");
            this.Property(t => t.IsOceanImport).HasColumnName("IsOceanImport");
            this.Property(t => t.IsAll).HasColumnName("IsAll");
            this.Property(t => t.IsCustomsImport).HasColumnName("IsCustomsImport");
            this.Property(t => t.IsInlandDomestic).HasColumnName("IsInlandDomestic");

            // Relationships
            this.HasRequired(t => t.Card)
                .WithMany()
                .HasForeignKey(d => d.CardId);
            this.HasRequired(t => t.Contact)
                .WithMany()
                .HasForeignKey(d => d.ContactId);
        }
    }
}
