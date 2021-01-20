using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class AccountingSettingMap : EntityTypeConfiguration<AccountingSetting>
    {
        public AccountingSettingMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).HasDatabaseGeneratedOption(null);
            this.Property(t => t.AccountingSystemCode).HasMaxLength(4).IsUnicode(false);
            this.Property(t => t.ReceivableVATableTempCard).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ReceivableVATExemptTempCard).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.PayableVATableTempCard).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.PayableVATExemptTempCard).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.QBOrealMeID).HasMaxLength(200);
            this.Property(t => t.QBOAccessToken).HasMaxLength(4096);
            this.Property(t => t.QBOAccessTokenSecret).HasMaxLength(4096);
            this.Property(t => t.RegistryDateTypeCode).HasMaxLength(4).IsUnicode(false);
            this.Property(t => t.ReceivableVATCard).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.PayableVATCard).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.RefreshToken).HasMaxLength(2000);
            this.Property(t => t.TransferFTPDetailId).HasMaxLength(15).IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("AccountingSettings");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.AccountingSystemCode).HasColumnName("AccountingSystemCode");
            this.Property(t => t.AllowVoidARI).HasColumnName("AllowVoidARI");
            this.Property(t => t.AllowVoidARP).HasColumnName("AllowVoidARP");
            this.Property(t => t.AllowVoidAPI).HasColumnName("AllowVoidAPI");
            this.Property(t => t.AllowVoidAPP).HasColumnName("AllowVoidAPP");
            this.Property(t => t.AllowManualInvoiceNumber).HasColumnName("AllowManualInvoiceNumber");
            this.Property(t => t.IsVatNumberMandatoryInAR).HasColumnName("IsVatNumberMandatoryInAR");
            this.Property(t => t.IsVatNumberMandatoryInAP).HasColumnName("IsVatNumberMandatoryInAP");
            this.Property(t => t.IsARInvoiceChronologicalDates).HasColumnName("IsARInvoiceChronologicalDates");
            this.Property(t => t.IsARPaymentChronologicalDates).HasColumnName("IsARPaymentChronologicalDates");
            this.Property(t => t.ReceivableVATableTempCard).HasColumnName("ReceivableVATableTempCard");
            this.Property(t => t.ReceivableVATExemptTempCard).HasColumnName("ReceivableVATExemptTempCard");
            this.Property(t => t.PayableVATableTempCard).HasColumnName("PayableVATableTempCard");
            this.Property(t => t.PayableVATExemptTempCard).HasColumnName("PayableVATExemptTempCard");
            this.Property(t => t.AllowClosureWithoutPayables).HasColumnName("AllowClosureWithoutPayables");
            this.Property(t => t.IsSingleTaxPerInvoice).HasColumnName("IsSingleTaxPerInvoice");
            this.Property(t => t.IsARInvoicesTransferEnabled).HasColumnName("IsARInvoicesTransferEnabled");
            this.Property(t => t.IsAPInvoicesTransferEnabled).HasColumnName("IsAPInvoicesTransferEnabled");
            this.Property(t => t.IsARPaymentsTransferEnabled).HasColumnName("IsARPaymentsTransferEnabled");
            this.Property(t => t.ARInvoiceTransferStartDate).HasColumnName("ARInvoiceTransferStartDate");
            this.Property(t => t.APInvoiceTransferStartDate).HasColumnName("APInvoiceTransferStartDate");
            this.Property(t => t.ARPaymentTransferStartDate).HasColumnName("ARPaymentTransferStartDate");
            this.Property(t => t.QBOrealMeID).HasColumnName("QBOrealMeID");
            this.Property(t => t.QBOAccessToken).HasColumnName("QBOAccessToken");
            this.Property(t => t.QBOAccessTokenSecret).HasColumnName("QBOAccessTokenSecret");
            this.Property(t => t.AllowMinusInvoicelines).HasColumnName("AllowMinusInvoicelines");
            this.Property(t => t.TransferToDropboxActivated).HasColumnName("TransferToDropboxActivated");
            this.Property(t => t.EnableMultiPercentageVATTypes).HasColumnName("EnableMultiPercentageVATTypes");
            this.Property(t => t.NotifyPastDateOnInvoiceEdit).HasColumnName("NotifyPastDateOnInvoiceEdit");
            this.Property(t => t.EnableMultiRateAPInvoices).HasColumnName("EnableMultiRateAPInvoices");
            this.Property(t => t.RegistryDateTypeCode).HasColumnName("RegistryDateTypeCode");
            this.Property(t => t.ReceivableVATCard).HasColumnName("ReceivableVATCard");
            this.Property(t => t.PayableVATCard).HasColumnName("PayableVATCard");
            this.Property(t => t.EnableMultiCurrencyARPayments).HasColumnName("EnableMultiCurrencyARPayments");
            this.Property(t => t.EnableMultiCurrencyAPPayments).HasColumnName("EnableMultiCurrencyAPPayments");
            this.Property(t => t.IsAPPaymentsTransferEnabled).HasColumnName("IsAPPaymentsTransferEnabled");
            this.Property(t => t.EnableNegativeOffsetARPayments).HasColumnName("EnableNegativeOffsetARPayments");
            this.Property(t => t.EnableNegativeOffsetAPPayments).HasColumnName("EnableNegativeOffsetAPPayments");
            this.Property(t => t.EnableInvoiceStocksManagement).HasColumnName("EnableInvoiceStocksManagement");
            this.Property(t => t.QBOOAuth).HasColumnName("QBOOAuth");
            this.Property(t => t.RefreshToken).HasColumnName("RefreshToken");
            this.Property(t => t.AllowManualARPaymentNumber).HasColumnName("AllowManualARPaymentNumber");
            this.Property(t => t.AllowRegionalTaxManagement).HasColumnName("AllowRegionalTaxManagement");
            this.Property(t => t.EnableAPPaymentExternalPayment).HasColumnName("EnableAPPaymentExternalPayment");
            this.Property(t => t.TransferToFTPActivated).HasColumnName("TransferToFTPActivated");
            this.Property(t => t.TransferFTPDetailId).HasColumnName("TransferFTPDetailId");
            this.Property(t => t.EnableEnteringTotalVAT).HasColumnName("EnableEnteringTotalVAT");
            this.Property(t => t.BlockSendInvoiceOriginalCopy).HasColumnName("BlockSendInvoiceOriginalCopy");

            //#if ORACLE_DB
            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms == "oracle")
            {
                this.Property(t => t.AllowPositiveAmountsInTheCreditNote).HasColumnName("AllowPstvAmountsInCrdtNote");
            }
            //#elseelse
            else
            {
                this.Property(t => t.AllowPositiveAmountsInTheCreditNote).HasColumnName("AllowPositiveAmountsInTheCreditNote");
            }
            //#endif


            // Relationships
            this.HasOptional(t => t.AccountingSystem).WithMany().HasForeignKey(d => d.AccountingSystemCode);
            this.HasRequired(t => t.Tenant).WithOptional(t => t.AccountingSetting);
            this.HasOptional(t => t.RegistryDateType).WithMany().HasForeignKey(d => d.RegistryDateTypeCode);
            this.HasOptional(t => t.TransferFTPDetail).WithMany().HasForeignKey(d => d.TransferFTPDetailId);
        }
    }
}
