using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Simplog.Data.ShipmentsModel.Mapping
{
    public class ShipmentReceivableMap : EntityTypeConfiguration<ShipmentReceivable>
    {
        public ShipmentReceivableMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ShipmentId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ShipmentReceivableLineStatusCode).IsRequired().IsFixedLength().HasMaxLength(4).IsUnicode(false);
            this.Property(t => t.CurrencyId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Notes).HasMaxLength(1000).IsUnicode(true);
            this.Property(t => t.ChargesTypeId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.UpdateByUserId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.PrepaidCollectId).HasMaxLength(1).IsUnicode(false);
            this.Property(t => t.DueTypeCode).HasMaxLength(2).IsUnicode(false);
            this.Property(t => t.ARInvoiceLineId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.MeasurementId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ARInvoiceId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.CreatedByUserId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.IATACodeId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.QuoteChargeId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.VatTypeId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ShipmentReceivableParentId).HasMaxLength(15).IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("ShipmentReceivables");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.ShipmentId).HasColumnName("ShipmentId");         
            this.Property(t => t.Quantity).HasColumnName("Quantity");
            this.Property(t => t.CurrencyId).HasColumnName("CurrencyId");
            this.Property(t => t.UnitPrice).HasColumnName("UnitPrice");
            this.Property(t => t.TotalAmount).HasColumnName("TotalAmount");
            this.Property(t => t.TotalAmountLocal).HasColumnName("TotalAmountLocal");
            this.Property(t => t.Notes).HasColumnName("Notes");
            this.Property(t => t.ChargesTypeId).HasColumnName("ChargesTypeId");
            this.Property(t => t.Rate).HasColumnName("Rate");
            this.Property(t => t.PayableLocal).HasColumnName("PayableLocal");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate").IsRequired();
            this.Property(t => t.UpdateByUserId).HasColumnName("UpdateByUserId");
            this.Property(t => t.PrepaidCollectId).HasColumnName("PrepaidCollectId");
            this.Property(t => t.AWBPrint).HasColumnName("AWBPrint");
            this.Property(t => t.DueTypeCode).HasColumnName("DueTypeCode");
            this.Property(t => t.ARInvoiceLineId).HasColumnName("ARInvoiceLineId");
            this.Property(t => t.MeasurementId).HasColumnName("MeasurementId");
            this.Property(t => t.IsExchangeRateFixed).HasColumnName("IsExchangeRateFixed");
            this.Property(t => t.IsFromQuote).HasColumnName("IsFromQuote");
            this.Property(t => t.IsFixedPrice).HasColumnName("IsFixedPrice");
            this.Property(t => t.AmountInProfitCurrency).HasColumnName("AmountInProfitCurrency");
            this.Property(t => t.ProfitCurrencyExchangeRate).HasColumnName("ProfitCurrencyExchangeRate");
            this.Property(t => t.ARInvoiceId).HasColumnName("ARInvoiceId");
            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate").IsRequired();
            this.Property(t => t.IATACodeId).HasColumnName("IATACodeId");
            this.Property(t => t.VatTypeId).HasColumnName("VatTypeId");
            this.Property(t => t.IsBackToBack).HasColumnName("IsBackToBack");
            this.Property(t => t.IsExpense).HasColumnName("IsExpense");
            this.Property(t => t.ShipmentReceivableParentId).HasColumnName("ShipmentReceivableParentId");
            this.Property(t => t.QuoteSaleMinAmount).HasColumnName("QuoteSaleMinAmount");
            this.Property(t => t.QuoteSaleMaxAmount).HasColumnName("QuoteSaleMaxAmount");

            //#if ORACLE_DB
            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms == "oracle")
            {
                this.Property(t => t.ShipmentReceivableLineStatusCode).HasColumnName("StatusCode");
            }
            //#else
            else
            {
                this.Property(t => t.ShipmentReceivableLineStatusCode).HasColumnName("ShipmentReceivableLineStatusCode");
            }
            
//#endif
            this.Property(t => t.QuoteChargeId).HasColumnName("QuoteChargeId");
            this.Property(t => t.IsChargeBySteps).HasColumnName("IsChargeBySteps");

            // Relationships
            this.HasOptional(t => t.ARInvoiceLine).WithMany().HasForeignKey(d => d.ARInvoiceLineId);
            this.HasOptional(t => t.ARInvoice).WithMany().HasForeignKey(d => d.ARInvoiceId);
            this.HasRequired(t => t.ChargesType).WithMany().HasForeignKey(d => d.ChargesTypeId);
            this.HasRequired(t => t.Currency).WithMany().HasForeignKey(d => d.CurrencyId);
            this.HasOptional(t => t.DueType).WithMany().HasForeignKey(d => d.DueTypeCode);
            this.HasRequired(t => t.Measurement).WithMany().HasForeignKey(d => d.MeasurementId);
            this.HasRequired(t => t.ShipmentReceivableLineStatus).WithMany().HasForeignKey(d => d.ShipmentReceivableLineStatusCode);
            this.HasRequired(t => t.Shipment).WithMany().HasForeignKey(d => d.ShipmentId).WillCascadeOnDelete(false);
            this.HasRequired(t => t.UpdateByUser).WithMany().HasForeignKey(d => d.UpdateByUserId).WillCascadeOnDelete(false);
            this.HasRequired(t => t.CreatedByUser).WithMany().HasForeignKey(d => d.CreatedByUserId).WillCascadeOnDelete(false);
            this.HasOptional(t => t.VatType).WithMany().HasForeignKey(d => d.VatTypeId);
            this.HasOptional(t => t.ShipmentReceivableParent).WithMany(t => t.ChildShipmentReceivables).HasForeignKey(d => d.ShipmentReceivableParentId);
        }
    }
}
