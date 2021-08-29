using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System.Data.Entity.ModelConfiguration;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data;
 
namespace Logitude.Accounting.Data.EntityMapping
{
 
    public class FullAccountingSettingMap : EntityTypeConfiguration<FullAccountingSetting>
    {
	    string dbms;
        public FullAccountingSettingMap()
        { 
				this.ToTable("FullAccountingSettings");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.DeductionFileNumber).HasColumnName("DeductionFileNumber").HasMaxLength(30).IsUnicode(false);

            this.Property(t => t.ConsolidationVAT).HasColumnName("ConsolidationVAT").HasMaxLength(30).IsUnicode(false);

            this.Property(t => t.DefaultVATTypeId).HasColumnName("DefaultVATTypeId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.VATInputsGLAccountId).HasColumnName("VATInputsGLAccountId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.AutomaticReconcileMethodId).HasColumnName("AutomaticReconcileMethodId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ExchangeRateDiffGLAccountId).HasColumnName("ExchangeRateDiffGLAccountId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.RevenueExpenseGLAccountId).HasColumnName("RevenueExpenseGLAccountId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Id).HasColumnName("Id").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.VATOutputGLAccountId).HasColumnName("VATOutputGLAccountId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.CustomerControlAccountId).HasColumnName("CustomerControlAccountId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.VendorControlAccountId).HasColumnName("VendorControlAccountId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.FileControlAccountId).HasColumnName("FileControlAccountId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.OceanExportJobControlAccountId).HasColumnName("OceanExportJobControlAccountId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.AirExportJobControlAccountId).HasColumnName("AirExportJobControlAccountId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.OceanImportJobControlAccountId).HasColumnName("OceanImportJobControlAccountId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.AirImportJobControlAccountId).HasColumnName("AirImportJobControlAccountId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ExternalReconciliationDefault).HasColumnName("ExternalReconciliationDefault").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.TaxWithholdingGLAccountId).HasColumnName("TaxWithholdingGLAccountId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.DefaultTaxWithholdPercentage).HasColumnName("DefaultTaxWithholdPercentage").HasPrecision(16, 2);

            this.Property(t => t.CustomsGLAccountId).HasColumnName("CustomsGLAccountId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.DefaultDifferencesGLAccountId).HasColumnName("DefaultDifferencesGLAccountId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.DefaultExternalDiffGLAccountId).HasColumnName("DefaultExternalDiffGLAccountId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.SoftwareVersion).HasColumnName("SoftwareVersion").HasMaxLength(100).IsUnicode(false);

            this.Property(t => t.IsPaymentChequesActivated).HasColumnName("IsPaymentChequesActivated");

            this.Property(t => t.GLAccounterCounterLength).HasColumnName("GLAccounterCounterLength");

            this.Property(t => t.PaymentChequesLogoId).HasColumnName("PaymentChequesLogoId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.NumberOfAgingMonths).HasColumnName("NumberOfAgingMonths");

            this.Property(t => t.AllowMultiRatesInInvoiceLines).HasColumnName("AllowMultiRatesInInvoiceLines");

            this.Property(t => t.NumberofPeriods).HasColumnName("NumberofPeriods");

            this.Property(t => t.FirstPeriodsMonths).HasColumnName("FirstPeriodsMonths").HasMaxLength(100).IsUnicode(false);

            this.Property(t => t.SecondPeriodsMonths).HasColumnName("SecondPeriodsMonths").HasMaxLength(100).IsUnicode(false);

            this.Property(t => t.ThirdsPeriodsMonths).HasColumnName("ThirdsPeriodsMonths").HasMaxLength(100).IsUnicode(false);

            this.Property(t => t.ActiveSecurityLevel).HasColumnName("ActiveSecurityLevel");
        }
    }
}
	 