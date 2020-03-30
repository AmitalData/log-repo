using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class AccountingSystemMap : EntityTypeConfiguration<AccountingSystem>
    {
        public AccountingSystemMap()
        {
            this.HasKey(t => t.Code);
            this.Property(t => t.Code).IsRequired().HasMaxLength(4).IsUnicode(false);
            this.Property(t => t.Name).IsRequired().HasMaxLength(40).IsUnicode(false);
            this.Property(t => t.SearchFields).HasMaxLength(1000).IsUnicode(true);

            // Table & Column Mappings
            this.ToTable("AccountingSystems");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
            this.Property(t => t.InActive).HasColumnName("InActive");
            this.Property(t => t.IsExternalCodesFromTable).HasColumnName("IsExternalCodesFromTable");
            this.Property(t => t.IsExternalCodesSyncEnabled).HasColumnName("IsExternalCodesSyncEnabled");
            this.Property(t => t.IsExternalCodesFromAPI).HasColumnName("IsExternalCodesFromAPI");
            this.Property(t => t.IsSingleTaxPerInvoice).HasColumnName("IsSingleTaxPerInvoice");
            this.Property(t => t.IsSingleCurrencyAccount).HasColumnName("IsSingleCurrencyAccount");
            this.Property(t => t.AllowManuallyDueDate).HasColumnName("AllowManuallyDueDate");
            this.Property(t => t.IsJournalMode).HasColumnName("IsJournalMode");
            this.Property(t => t.IsTaxItemManaged).HasColumnName("IsTaxItemManaged");
            this.Property(t => t.ShowDownloadScreen).HasColumnName("ShowDownloadScreen");
            this.Property(t => t.AllowMinusInvoiceLines).HasColumnName("AllowMinusInvoiceLines");
            this.Property(t => t.AllowARInvoicesTransfer).HasColumnName("AllowARInvoicesTransfer");
            this.Property(t => t.AllowAPInvoicesTransfer).HasColumnName("AllowAPInvoicesTransfer");
            this.Property(t => t.AllowARPaymentsTransfer).HasColumnName("AllowARPaymentsTransfer");
            this.Property(t => t.CanTransferToDropbox).HasColumnName("CanTransferToDropbox");
            this.Property(t => t.AllowAPPaymentsTransfer).HasColumnName("AllowAPPaymentsTransfer");
            this.Property(t => t.CanTransferToFTP).HasColumnName("CanTransferToFTP");
            
            //#if ORACLE_DB
            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms == "oracle")
            {
                this.Property(t => t.AllowPositiveAmountsInTheCreditNote).HasColumnName("AllowPstvAmountsInCrdtNote");
            }
            //#else
            else
            {
                this.Property(t => t.AllowPositiveAmountsInTheCreditNote).HasColumnName("AllowPositiveAmountsInTheCreditNote");
            }
            //#endif
        }
    }
}
