using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.ShipmentsModel.Mapping
{
    public class PayableProratedAmountMap : EntityTypeConfiguration<PayableProratedAmount>
    {
        public PayableProratedAmountMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ShipmentId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.PayableId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.InvoiceId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.InvoiceLineId).IsRequired().HasMaxLength(15).IsUnicode(false);

            this.ToTable("PayableProratedAmounts");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.ShipmentId).HasColumnName("ShipmentId");
            this.Property(t => t.PayableId).HasColumnName("PayableId");
            this.Property(t => t.InvoiceId).HasColumnName("InvoiceId");
            this.Property(t => t.InvoiceLineId).HasColumnName("ProratedAmountInLocalCurrency");
            this.Property(t => t.ProratedAmountInLocalCurrency).HasColumnName("ProratedAmountInLocalCurrency");
            this.Property(t => t.ProratedAmountInProfitCurrency).HasColumnName("ProratedAmountInProfitCurrency");

            this.HasRequired(t => t.Shipment).WithMany().HasForeignKey(d => d.ShipmentId);
            this.HasRequired(t => t.ShipmentPayable).WithMany().HasForeignKey(d => d.PayableId);
            this.HasRequired(t => t.APInvoice).WithMany().HasForeignKey(d => d.InvoiceId);
            this.HasRequired(t => t.APInvoiceLine).WithMany().HasForeignKey(d => d.InvoiceLineId);
        }
    }
}
