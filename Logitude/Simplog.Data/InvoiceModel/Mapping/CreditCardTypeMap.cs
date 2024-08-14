using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.InvoiceModel.EntityPOCOs;

namespace Simplog.Data.InvoiceModel.Mapping
{
    public class CreditCardTypeMap : EntityTypeConfiguration<CreditCardType>
    {
        public CreditCardTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Code)
                .IsRequired()
                .HasMaxLength(2)
                .IsUnicode(false);

            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);

            this.Property(t => t.SearchFields)
                .HasMaxLength(1000)
                .IsUnicode(true);

            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.BankAccountId)
               .HasMaxLength(15)
               .IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("CreditCardTypes");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.InActive).HasColumnName("InActive");
            this.Property(t => t.BankAccountId).HasColumnName("BankAccountId");

        }
    }
}
