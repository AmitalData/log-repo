using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class ChargesTypeMap : EntityTypeConfiguration<ChargesType>
    {
        public ChargesTypeMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Code).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.EnglishName).IsRequired().HasMaxLength(40).IsUnicode(false);
            this.Property(t => t.LocalName).HasMaxLength(40).IsUnicode(true);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ChargesGroupCode).IsRequired().HasMaxLength(5).IsUnicode(false);
            this.Property(t => t.VatTypeId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Description).HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.IATACodeId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.DueTypeCode).HasMaxLength(2).IsUnicode(false);
            this.Property(t => t.MeasurementId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ContainerMeasurementId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.SearchFields).HasMaxLength(1000).IsUnicode(true);
            this.Property(t => t.ReceivableAccountId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.PayableAccountId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ReceivableCreditAccount).HasMaxLength(25).IsUnicode(false);
            this.Property(t => t.PayableDebitAccount).HasMaxLength(25).IsUnicode(false);
            this.Property(t => t.ReceivablesChargesTypeExternalCode).HasMaxLength(25).IsUnicode(false);
            this.Property(t => t.PayablesChargesTypeExternalCode).HasMaxLength(25).IsUnicode(false);            
            this.Property(t => t.PayableDebitGLAcountId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ReceivableCreditGLAccountId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ChargesGroupId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.SATExternalId).HasMaxLength(20).IsUnicode(false);
            this.Property(t => t.ReceivablesDefaultCurrencyId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.PayablesDefaultCurrencyId).HasMaxLength(15).IsUnicode(false);


            // Table & Column Mappings
            this.ToTable("ChargesTypes");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.EnglishName).HasColumnName("EnglishName");
            this.Property(t => t.LocalName).HasColumnName("LocalName");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.AddedManually).HasColumnName("AddedManually");
            this.Property(t => t.InActive).HasColumnName("InActive");
            this.Property(t => t.ChargesGroupCode).HasColumnName("ChargesGroupCode");
            this.Property(t => t.VatTypeId).HasColumnName("VatTypeId");
            this.Property(t => t.IsReceivable).HasColumnName("IsReceivable");
            this.Property(t => t.IsPayable).HasColumnName("IsPayable");
            this.Property(t => t.IsAir).HasColumnName("IsAir");
            this.Property(t => t.IsOcean).HasColumnName("IsOcean");
            this.Property(t => t.IsInland).HasColumnName("IsInland");
            this.Property(t => t.IsAutoDisplayInShipment).HasColumnName("IsAutoDisplayInShipment");
            this.Property(t => t.IsAutoDisplayInConsolidation).HasColumnName("IsAutoDisplayInConsolidation");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.IATACodeId).HasColumnName("IATACodeId");
            this.Property(t => t.AWBPrintDescription).HasColumnName("AWBPrintDescription");
            this.Property(t => t.DueTypeCode).HasColumnName("DueTypeCode");
            this.Property(t => t.IsAutoDisplayInQuote).HasColumnName("IsAutoDisplayInQuote");
            this.Property(t => t.MeasurementId).HasColumnName("MeasurementId");
            this.Property(t => t.ContainerMeasurementId).HasColumnName("ContainerMeasurementId");
            this.Property(t => t.ViewOrder).HasColumnName("ViewOrder");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
            this.Property(t => t.ReceivableAccountId).HasColumnName("ReceivableAccountId");
            this.Property(t => t.PayableAccountId).HasColumnName("PayableAccountId");
            this.Property(t => t.AccountingVATSplit).HasColumnName("AccountingVATSplit");
            this.Property(t => t.ReceivableCreditAccount).HasColumnName("ReceivableCreditAccount");
            this.Property(t => t.PayableDebitAccount).HasColumnName("PayableDebitAccount");
            this.Property(t => t.PayableDebitGLAcountId).HasColumnName("PayableDebitGLAcountId");
            this.Property(t => t.ReceivableCreditGLAccountId).HasColumnName("ReceivableCreditGLAccountId");
            this.Property(t => t.ChargesGroupId).HasColumnName("ChargesGroupId");
            this.Property(t => t.IsAutoDisplayInCustoms).HasColumnName("IsAutoDisplayInCustoms");
            this.Property(t => t.IsCustoms).HasColumnName("IsCustoms");
            this.Property(t => t.IsBackToBack).HasColumnName("IsBackToBack");
            this.Property(t => t.SATExternalId).HasColumnName("SATExternalId");

            this.Property(t => t.IsExpense).HasColumnName("IsExpense");

            this.Property(t => t.IsImport).HasColumnName("IsImport");
            this.Property(t => t.IsDomestic).HasColumnName("IsDomestic");
            this.Property(t => t.IsExport).HasColumnName("IsExport");
            this.Property(t => t.IsDrop).HasColumnName("IsDrop");
            this.Property(t => t.ReceivablesDefaultCurrencyId).HasColumnName("ReceivablesDefaultCurrencyId");
            this.Property(t => t.PayablesDefaultCurrencyId).HasColumnName("PayablesDefaultCurrencyId");

            //#if ORACLE_DB
            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms == "oracle")
            {
                this.Property(t => t.ReceivablesChargesTypeExternalCode).HasColumnName("ReceivablesChargesTypeExtCode");
                this.Property(t => t.PayablesChargesTypeExternalCode).HasColumnName("PayablesChargesTypeExtCode");

            }
            //#elseelse
            else
            {
                this.Property(t => t.ReceivablesChargesTypeExternalCode).HasColumnName("ReceivablesChargesTypeExternalCode");
                this.Property(t => t.PayablesChargesTypeExternalCode).HasColumnName("PayablesChargesTypeExternalCode");
            }

            
            this.HasOptional(t => t.ChargesGroup).WithMany().HasForeignKey(d => d.ChargesGroupId);
            this.HasOptional(t => t.PayableAccount).WithMany().HasForeignKey(d => d.PayableAccountId);
            this.HasOptional(t => t.ReceivableAccount).WithMany().HasForeignKey(d => d.ReceivableAccountId);
            this.HasOptional(t => t.DueType).WithMany().HasForeignKey(d => d.DueTypeCode);
            this.HasOptional(t => t.VatType).WithMany().HasForeignKey(d => d.VatTypeId);
            this.HasRequired(t => t.Measurement).WithMany().HasForeignKey(t => t.MeasurementId);
            this.HasOptional(t => t.ReceivablesDefaultCurrency).WithMany().HasForeignKey(d => d.ReceivablesDefaultCurrencyId);
            this.HasOptional(t => t.PayablesDefaultCurrency).WithMany().HasForeignKey(d => d.PayablesDefaultCurrencyId);
        }
    }
}
