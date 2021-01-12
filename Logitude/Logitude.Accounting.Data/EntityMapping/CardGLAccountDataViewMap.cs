using Logitude.Accounting.Data.EntityPOCOs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class CardGLAccountDataViewMap : EntityTypeConfiguration<CardGLAccountDataView>
    {
        public CardGLAccountDataViewMap()
        {
            this.ToTable("CardGLAccountDataView");
            this.HasKey(t => new { t.Id });

            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Tenant).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
         
            // Table & Column Mappings

            // GLAccount
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.InternalNumber).HasColumnName("InternalNumber");
            this.Property(t => t.AccountTypeCode).HasColumnName("AccountTypeCode");
            this.Property(t => t.DisplayNumber).HasColumnName("DisplayNumber");
            this.Property(t => t.GLAccountLocalName).HasColumnName("GLAccountLocalName");
            this.Property(t => t.GLAccountEnglishName).HasColumnName("GLAccountEnglishName");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
            this.Property(t => t.IsMultiCurrency).HasColumnName("IsMultiCurrency");
            this.Property(t => t.CurrencyId).HasColumnName("CurrencyId");
            this.Property(t => t.RevenueExpenseType).HasColumnName("RevenueExpenseType");
            this.Property(t => t.IsControlAccount).HasColumnName("IsControlAccount");
            this.Property(t => t.ChartOfAccountsId).HasColumnName("ChartOfAccountsId");
            this.Property(t => t.Inactive).HasColumnName("Inactive");
            this.Property(t => t.ChartOfAccountsTypeCode).HasColumnName("ChartOfAccountsTypeCode");
            this.Property(t => t.ReconcileMethodCode).HasColumnName("ReconcileMethodCode");
            this.Property(t => t.ControlAccountId).HasColumnName("ControlAccountId");
            this.Property(t => t.AutomaticReconcileId).HasColumnName("AutomaticReconcileId");
            this.Property(t => t.PreviousEnglishName).HasColumnName("PreviousEnglishName");
            this.Property(t => t.PreviousEnglishNameChangeDate).HasColumnName("PreviousEnglishNameChangeDate");
            this.Property(t => t.PreviousLocalName).HasColumnName("PreviousLocalName");
            this.Property(t => t.PreviousLocalNameChangeDate).HasColumnName("PreviousLocalNameChangeDate");
            this.Property(t => t.PreviousNumber).HasColumnName("PreviousNumber");
            this.Property(t => t.PreviousNumberChangeDate).HasColumnName("PreviousNumberChangeDate");
            this.Property(t => t.PreviousChartOfAccountsId).HasColumnName("PreviousChartOfAccountsId");
            this.Property(t => t.PreviousChartOfAccountsChangeDate).HasColumnName("PreviousChartOfAccountsChangeDate");
            this.Property(t => t.CustomerGLAccountId).HasColumnName("CustomerGLAccountId");
            this.Property(t => t.ConsolidationVat).HasColumnName("ConsolidationVat ");
        //    this.Property(t => t.BalanceInLocalCurrency).HasColumnName("BalanceInLocalCurrency");
        //    this.Property(t => t.LocalBalanceInDue).HasColumnName("LocalBalanceInDue");
        //    this.Property(t => t.NextDueDate).HasColumnName("NextDueDate");

            this.Property(t => t.RevaluationEnabled).HasColumnName("RevaluationEnabled");
            this.Property(t => t.ParentAccountId).HasColumnName("ParentAccountId");
            this.Property(t => t.Category1Id).HasColumnName("Category1Id");
            this.Property(t => t.Category2Id).HasColumnName("Category2Id");
            this.Property(t => t.Category3Id).HasColumnName("Category3Id");
            this.Property(t => t.Category4Id).HasColumnName("Category4Id");
            this.Property(t => t.Category5Id).HasColumnName("Category5Id");
            this.Property(t => t.IsVATExempt).HasColumnName("IsVATExempt");
            this.Property(t => t.DeductionFileNumber).HasColumnName("DeductionFileNumber");

            // Card
            this.Property(t => t.SalesmanUserId).HasColumnName("SalesmanUserId");
            this.Property(t => t.CollectorId).HasColumnName("CollectorId");
            this.Property(t => t.CardEnglishName).HasColumnName("CardEnglishName");
            this.Property(t => t.CardLocalName).HasColumnName("CardLocalName");
            this.Property(t => t.PaymentTermId).HasColumnName("PaymentTermId");
            this.Property(t => t.SalesmanUserId).HasColumnName("SalesmanUserId");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
            this.Property(t => t.VatNumber).HasColumnName("VatNumber");
            this.Property(t => t.VatTypeId).HasColumnName("VatTypeId");
            this.Property(t => t.CityName).HasColumnName("CityName");
            this.Property(t => t.CountryId).HasColumnName("CountryId");
            this.Property(t => t.CountryCode).HasColumnName("CountryCode");

            // SalesMan
            this.Property(t => t.SalesManEnglishName).HasColumnName("SalesManEnglishName");
            this.Property(t => t.SalesManLocalName).HasColumnName("SalesManLocalName");

            // Collector
            this.Property(t => t.CollectorEnglishName).HasColumnName("CollectorEnglishName");
            this.Property(t => t.CollectorLocalName).HasColumnName("CollectorLocalName");

        }
    }
}
