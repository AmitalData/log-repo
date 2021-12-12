using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Simplog.Data.ShipmentsModel.Mapping
{
    public class ShipmentPayableMap : EntityTypeConfiguration<ShipmentPayable>
    {
        public ShipmentPayableMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Notes).HasMaxLength(1000).IsUnicode(true);
            this.Property(t => t.ShipmentId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ChargesTypeId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ShipmentPayableLineStatusCode).IsRequired().HasMaxLength(4).IsUnicode(false);
            this.Property(t => t.MeasurementId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.CurrencyId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.UpdateByUserId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.VendorId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.DueTypeCode).HasMaxLength(2).IsUnicode(false);
            this.Property(t => t.PrepaidCollectId).HasMaxLength(1).IsUnicode(false);
            this.Property(t => t.ShipmentPayableParentId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ShipmentPayableAmountTypeCode).HasMaxLength(4).IsUnicode(false);
            this.Property(t => t.CreatedByUserId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.CorrectionNote).HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.CorrectionByUserId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.IATACodeId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.QuoteChargeId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.VatTypeId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ReceivableId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.TariffId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.TariffNumber).HasMaxLength(20).IsUnicode(false);
            this.Property(t => t.TariffLineId).HasMaxLength(20).IsUnicode(false);
            // Table & Column Mappings
            this.ToTable("ShipmentPayables");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.Quantity).HasColumnName("Quantity");
            this.Property(t => t.AWBPrint).HasColumnName("AWBPrint");
            this.Property(t => t.UnitPrice).HasColumnName("UnitPrice");
            this.Property(t => t.Rate).HasColumnName("Rate");
            this.Property(t => t.ExpectedAmountLocal).HasColumnName("ExpectedAmountLocal");
            this.Property(t => t.Notes).HasColumnName("Notes");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate").IsRequired();
            this.Property(t => t.ValueDate).HasColumnName("ValueDate");
            this.Property(t => t.ShipmentId).HasColumnName("ShipmentId");
            this.Property(t => t.ChargesTypeId).HasColumnName("ChargesTypeId");
            this.Property(t => t.ShipmentPayableLineStatusCode).HasColumnName("ShipmentPayableLineStatusCode");
            this.Property(t => t.MeasurementId).HasColumnName("MeasurementId");
            this.Property(t => t.CurrencyId).HasColumnName("CurrencyId");
            this.Property(t => t.UpdateByUserId).HasColumnName("UpdateByUserId");
            this.Property(t => t.VendorId).HasColumnName("VendorId");
            this.Property(t => t.DueTypeCode).HasColumnName("DueTypeCode");
            this.Property(t => t.IsFromQuote).HasColumnName("IsFromQuote");
            this.Property(t => t.PrepaidCollectId).HasColumnName("PrepaidCollectId");
            this.Property(t => t.MaxAmount).HasColumnName("MaxAmount");
            this.Property(t => t.MinAmount).HasColumnName("MinAmount");
            this.Property(t => t.ExpectedAmountInProfitCurrency).HasColumnName("ExpectedAmountInProfitCurrency");
            this.Property(t => t.ProfitCurrencyExchangeRate).HasColumnName("ProfitCurrencyExchangeRate");
            this.Property(t => t.IsEditedByUser).HasColumnName("IsEditedByUser");
            this.Property(t => t.ShipmentPayableParentId).HasColumnName("ShipmentPayableParentId");
            this.Property(t => t.ExpectedAmount).HasColumnName("ExpectedAmount");
            this.Property(t => t.AccountedAmount).HasColumnName("AccountedAmount");
            this.Property(t => t.AccountedAmountInLocalCurrency).HasColumnName("AccountedAmountInLocalCurrency");          
            this.Property(t => t.ShipmentPayableAmountTypeCode).HasColumnName("ShipmentPayableAmountTypeCode");
            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate").IsRequired();
            this.Property(t => t.OpenAmount).HasColumnName("OpenAmount");          
            this.Property(t => t.OpenAmountInProfitCurrency).HasColumnName("OpenAmountInProfitCurrency");
            this.Property(t => t.CorrectionAmount).HasColumnName("CorrectionAmount");
            this.Property(t => t.CorrectionNote).HasColumnName("CorrectionNote");
            this.Property(t => t.CorrectionByUserId).HasColumnName("CorrectionByUserId");
            this.Property(t => t.CorrectionDate).HasColumnName("CorrectionDate");
            this.Property(t => t.IATACodeId).HasColumnName("IATACodeId");
            this.Property(t => t.QuoteCostMinAmount).HasColumnName("QuoteCostMinAmount");
            this.Property(t => t.QuoteCostMaxAmount).HasColumnName("QuoteCostMaxAmount");
            this.Property(t => t.OpenAmountInLocalCurrency).HasColumnName("OpenAmountInLocalCurrency");
            this.Property(t => t.VatTypeId).HasColumnName("VatTypeId");
            this.Property(t => t.ReceivableId).HasColumnName("ReceivableId");
            this.Property(t => t.IsBackToBack).HasColumnName("IsBackToBack");
            this.Property(t => t.TariffNumber).HasColumnName("TariffNumber");
            this.Property(t => t.TariffLineId).HasColumnName("TariffLineId");
            this.Property(t => t.TariffId).HasColumnName("TariffId");
            this.Property(t => t.TariffVersion).HasColumnName("TariffVersion");

            //#if ORACLE_DB
            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
    if (dbms == "oracle")
    {
        this.Property(t => t.AccountedAmountInProfitCurrency).HasColumnName("AccountedAmountInProfit");
    }
    //#else
    else
    {
        this.Property(t => t.AccountedAmountInProfitCurrency).HasColumnName("AccountedAmountInProfitCurrency");
    }
            
//#endif
            this.Property(t => t.QuoteChargeId).HasColumnName("QuoteChargeId");
            this.Property(t => t.IsChargeBySteps).HasColumnName("IsChargeBySteps");

            // Relationships
            this.HasOptional(t => t.VendorCard).WithMany().HasForeignKey(d => d.VendorId);
            this.HasRequired(t => t.ChargesType).WithMany().HasForeignKey(d => d.ChargesTypeId);
            this.HasRequired(t => t.Currency).WithMany().HasForeignKey(d => d.CurrencyId);
            this.HasOptional(t => t.DueType).WithMany().HasForeignKey(d => d.DueTypeCode);
            this.HasOptional(t => t.Measurement).WithMany().HasForeignKey(d => d.MeasurementId);
            this.HasOptional(t => t.ShipmentPayableAmountType).WithMany().HasForeignKey(d => d.ShipmentPayableAmountTypeCode);
            this.HasRequired(t => t.ShipmentPayableLineStatus).WithMany().HasForeignKey(d => d.ShipmentPayableLineStatusCode);
            this.HasOptional(t => t.CorrectionByUser).WithMany().HasForeignKey(d => d.CorrectionByUserId);
            this.HasRequired(t => t.Shipment).WithMany().HasForeignKey(d => d.ShipmentId).WillCascadeOnDelete(false);
            this.HasOptional(t => t.ShipmentPayableParent).WithMany(t => t.ChildShipmentPayables).HasForeignKey(d => d.ShipmentPayableParentId);
            this.HasRequired(t => t.CreatedByUser).WithMany().HasForeignKey(d => d.CreatedByUserId).WillCascadeOnDelete(false);
            this.HasRequired(t => t.UpdateByUser).WithMany().HasForeignKey(d => d.UpdateByUserId).WillCascadeOnDelete(false);
            this.HasOptional(t => t.VatType).WithMany().HasForeignKey(d => d.VatTypeId);
        }
    }
}
