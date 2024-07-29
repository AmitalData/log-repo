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
 
    public class GLAccountMap : EntityTypeConfiguration<GLAccount>
    {
	    string dbms;
        public GLAccountMap()
        { 
				this.ToTable("GLAccounts");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant");

            this.Property(t => t.InternalNumber).HasColumnName("InternalNumber").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.AccountTypeCode).HasColumnName("AccountTypeCode").HasMaxLength(1).IsUnicode(false);

            this.Property(t => t.DisplayNumber).HasColumnName("DisplayNumber").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.LocalName).HasColumnName("LocalName").IsRequired().HasMaxLength(105).IsUnicode(true);

            this.Property(t => t.EnglishName).HasColumnName("EnglishName").HasMaxLength(150).IsUnicode(true);

            dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms == "oracle")
            {
              this.Property(t => t.SearchFields).HasMaxLength(2000);
			}
            else
            {
              this.Property(t => t.SearchFields).HasMaxLength(4000);
			}


            this.Property(t => t.SearchFields).HasColumnName("SearchFields").IsUnicode(true);

            this.Property(t => t.IsMultiCurrency).HasColumnName("IsMultiCurrency");

            this.Property(t => t.CurrencyId).HasColumnName("CurrencyId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.PaymentTerms).HasColumnName("PaymentTerms").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.RevenueExpenseType).HasColumnName("RevenueExpenseType").IsRequired().HasMaxLength(1).IsUnicode(false);

            this.Property(t => t.IsControlAccount).HasColumnName("IsControlAccount");

            this.Property(t => t.ChartOfAccountsId).HasColumnName("ChartOfAccountsId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Inactive).HasColumnName("Inactive");

            this.Property(t => t.ChartOfAccountsTypeCode).HasColumnName("ChartOfAccountsTypeCode").IsRequired().HasMaxLength(1).IsUnicode(false);

            this.Property(t => t.ReconcileMethodCode).HasColumnName("ReconcileMethodCode").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ControlAccountId).HasColumnName("ControlAccountId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.AutomaticReconcileId).HasColumnName("AutomaticReconcileId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.PreviousEnglishName).HasColumnName("PreviousEnglishName").HasMaxLength(150).IsUnicode(true);

            this.Property(t => t.PreviousEnglishNameChangeDate).HasColumnName("PreviousEnglishNameChangeDate");

            this.Property(t => t.PreviousLocalName).HasColumnName("PreviousLocalName").HasMaxLength(105).IsUnicode(true);

            this.Property(t => t.PreviousLocalNameChangeDate).HasColumnName("PreviousLocalNameChangeDate");

            this.Property(t => t.PreviousNumber).HasColumnName("PreviousNumber").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.PreviousNumberChangeDate).HasColumnName("PreviousNumberChangeDate");

            this.Property(t => t.PreviousChartOfAccountsId).HasColumnName("PreviousChartOfAccountsId").HasMaxLength(15).IsUnicode(false);

            dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms == "oracle")
            {
              this.Property(t => t.PreviousChartOfAccountsChangeDate).HasColumnName("PreviousChartOfAccountsChangeD");
			}
			else
			{
              this.Property(t => t.PreviousChartOfAccountsChangeDate).HasColumnName("PreviousChartOfAccountsChangeDate");
			}


            this.Property(t => t.CustomerGLAccountId).HasColumnName("CustomerGLAccountId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.RevaluationEnabled).HasColumnName("RevaluationEnabled");

            this.Property(t => t.ParentAccountId).HasColumnName("ParentAccountId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Category1Id).HasColumnName("Category1Id").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Category2Id).HasColumnName("Category2Id").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Category3Id).HasColumnName("Category3Id").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Category4Id).HasColumnName("Category4Id").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Category5Id).HasColumnName("Category5Id").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.IsVATExempt).HasColumnName("IsVATExempt");

            this.Property(t => t.DeductionFileTypeId).HasColumnName("DeductionFileTypeId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.DeductionFileNumber).HasColumnName("DeductionFileNumber").HasMaxLength(30).IsUnicode(false);

            this.Property(t => t.AssessingOfficeCode).HasColumnName("AssessingOfficeCode").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Occupation).HasColumnName("Occupation").HasMaxLength(60).IsUnicode(true);

            this.Property(t => t.DeductionTypeId).HasColumnName("DeductionTypeId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ConsolidationVat).HasColumnName("ConsolidationVat").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.IsEquipmentVendor).HasColumnName("IsEquipmentVendor");

            this.Property(t => t.ExcludeFromDeductionReport).HasColumnName("ExcludeFromDeductionReport");

            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.CreateDate).HasColumnName("CreateDate");

            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");

            this.Property(t => t.AllowEditChequePayToName).HasColumnName("AllowEditChequePayToName");

            this.Property(t => t.ActiveForInterest).HasColumnName("ActiveForInterest");

            this.Property(t => t.InterestCalculationStartDate).HasColumnName("InterestCalculationStartDate");

            this.Property(t => t.ActiveForInterestCreditInvoice).HasColumnName("ActiveForInterestCreditInvoice");

            this.Property(t => t.InterestCreditLimit).HasColumnName("InterestCreditLimit").HasPrecision(18, 2);

            this.Property(t => t.InterestOpenBalance).HasColumnName("InterestOpenBalance").HasPrecision(18, 2);

            this.Property(t => t.NameForPrintingCheques).HasColumnName("NameForPrintingCheques").HasMaxLength(1000).IsUnicode(true);

            this.Property(t => t.Smallcashbook).HasColumnName("Smallcashbook");

            this.Property(t => t.MinimumInterestInvoiceBilling).HasColumnName("MinimumInterestInvoiceBilling");

            this.Property(t => t.ReportingAsAnotherDocument).HasColumnName("ReportingAsAnotherDocument");

            this.Property(t => t.CreditAllotmentPercentage).HasColumnName("CreditAllotmentPercentage").HasPrecision(4, 2);

            this.Property(t => t.CardsDataId).HasColumnName("CardsDataId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.PostponedChequesCommission).HasColumnName("PostponedChequesCommission").HasPrecision(16, 2);

            this.Property(t => t.DateFormat).HasColumnName("DateFormat").HasMaxLength(50).IsUnicode(true);

        }
    }
}
	 