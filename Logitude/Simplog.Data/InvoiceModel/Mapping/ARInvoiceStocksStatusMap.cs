using Simplog.Data.InvoiceModel.EntityPOCOs;
using System;
using System.Data.Entity.ModelConfiguration;

namespace Simplog.Data.InvoiceModel.Mapping
{
    public class ARInvoiceStocksStatusMap : EntityTypeConfiguration<ARInvoiceStocksStatus>
    {
        public ARInvoiceStocksStatusMap()
        {
            // Primary Key
            this.HasKey(t => t.Code);

            // Properties
            this.Property(t => t.Code)
                .IsRequired()
                .HasMaxLength(2)
                .IsUnicode(false);

            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(true);

            this.Property(t => t.SearchFields)
                .HasMaxLength(1000)
                .IsUnicode(true);

            // Table & Column Mappings
            this.ToTable("ARInvoiceStocksStatus");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
        }
    }
}
