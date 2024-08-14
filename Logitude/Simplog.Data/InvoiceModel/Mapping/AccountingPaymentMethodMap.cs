using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.InvoiceModel.EntityPOCOs;

namespace Simplog.Data.InvoiceModel.Mapping
{
    public class AccountingPaymentMethodMap : EntityTypeConfiguration<AccountingPaymentMethod>
    {
        public AccountingPaymentMethodMap()
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
                .HasMaxLength(2)
                .IsUnicode(false);

            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);

            this.Property(t => t.SearchFields)
                .HasMaxLength(1000)
                .IsUnicode(true);

            this.Property(t => t.ARExternalId)
             .HasMaxLength(25)
             .IsUnicode(false);

            this.Property(t => t.APExternalId)
           .HasMaxLength(25)
           .IsUnicode(false);
            this.Property(t => t.LocalName)
               .HasMaxLength(100)
               .IsUnicode(true);
            // Table & Column Mappings
            this.ToTable("AccountingPaymentMethods");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
            this.Property(t => t.AddedManually).HasColumnName("AddedManually");
            this.Property(t => t.Inactive).HasColumnName("Inactive");
            this.Property(t => t.ARExternalId).HasColumnName("ARExternalId");
            this.Property(t => t.APExternalId).HasColumnName("APExternalId");
            this.Property(t => t.IsAR).HasColumnName("IsAR");
            this.Property(t => t.IsAP).HasColumnName("IsAP");
            this.Property(t => t.LocalName).HasColumnName("LocalName");


        }
    }
}
