using Simplog.Data.InvoiceModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InvoiceModel.Mapping
{
    public class ARInvoiceStockLineMap : EntityTypeConfiguration<ARInvoiceStockLine>
    {
        public ARInvoiceStockLineMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ARInvoiceStockId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Number).IsRequired().HasMaxLength(20).IsUnicode(false);
            this.Property(t => t.CreatedByUserId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.UpdatedByUserId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ARInvoiceId).HasMaxLength(15).IsUnicode(false);

            this.ToTable("ARInvoiceStockLines");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.ARInvoiceStockId).HasColumnName("ARInvoiceStockId");
            this.Property(t => t.Number).HasColumnName("Number");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId");
            this.Property(t => t.IsUsed).HasColumnName("IsUsed");
            this.Property(t => t.ARInvoiceId).HasColumnName("ARInvoiceId");

            this.HasRequired(t => t.ARInvoiceStock).WithMany().HasForeignKey(d => d.ARInvoiceStockId);
            this.HasRequired(t => t.CreatedByUser).WithMany().HasForeignKey(d => d.CreatedByUserId);
            this.HasRequired(t => t.UpdatedByUser).WithMany().HasForeignKey(d => d.UpdatedByUserId);
            this.HasOptional(t => t.ARInvoice).WithMany().HasForeignKey(d => d.ARInvoiceId);
        }
    }
}
