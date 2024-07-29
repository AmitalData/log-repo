using Simplog.Data.InvoiceModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InvoiceModel.Mapping
{
    public class ARInvoiceStockMap : EntityTypeConfiguration<ARInvoiceStock>
    {
        public ARInvoiceStockMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Name).IsRequired().HasMaxLength(80).IsUnicode(true);
            this.Property(t => t.Description).HasMaxLength(500).IsUnicode(true);
            this.Property(t => t.CreatedByUserId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.UpdatedByUserId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.StatusCode).IsRequired().HasMaxLength(2).IsUnicode(false);
            this.Property(t => t.Notes).HasMaxLength(500).IsUnicode(true);

            this.ToTable("ARInvoiceStocks");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.Inactive).HasColumnName("Inactive");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId");
            this.Property(t => t.StatusCode).HasColumnName("StatusCode");
            this.Property(t => t.StartDate).HasColumnName("StartDate");
            this.Property(t => t.EndDate).HasColumnName("EndDate");
            this.Property(t => t.Amount).HasColumnName("Amount");
            this.Property(t => t.Remaining).HasColumnName("Remaining");
            this.Property(t => t.Notes).HasColumnName("Notes");

            this.HasRequired(t => t.CreatedByUser).WithMany().HasForeignKey(d => d.CreatedByUserId);
            this.HasRequired(t => t.UpdatedByUser).WithMany().HasForeignKey(d => d.UpdatedByUserId);
            this.HasRequired(t => t.Status).WithMany().HasForeignKey(d => d.StatusCode);
        }
    }
}
