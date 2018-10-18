using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.InvoiceModel.EntityPOCOs;

namespace Simplog.Data.InvoiceModel.Mapping
{
    public class Accounts1Map : EntityTypeConfiguration<Account>
    {
        public Accounts1Map()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.Code)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(true);

            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(30)
                .IsUnicode(true);

            this.Property(t => t.ExternalAccountingCard)
                .HasMaxLength(25)
                .IsUnicode(true);

            this.Property(t => t.AccountTypeCode)
                .IsRequired()
                .HasMaxLength(2)
                .IsUnicode(false);

            this.Property(t => t.SearchFields)
                .HasMaxLength(1000)
                .IsUnicode(true);

            // Table & Column Mappings
            this.ToTable("Accounts1");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.ExternalAccountingCard).HasColumnName("ExternalAccountingCard");
            this.Property(t => t.InActive).HasColumnName("InActive");
            this.Property(t => t.AddedManually).HasColumnName("AddedManually");
            this.Property(t => t.AccountTypeCode).HasColumnName("AccountTypeCode");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");

            // Relationships
            this.HasRequired(t => t.AccountType)
                .WithMany()
                .HasForeignKey(d => d.AccountTypeCode);

        }
    }
}
