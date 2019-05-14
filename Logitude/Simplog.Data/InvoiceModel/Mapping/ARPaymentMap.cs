using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.InvoiceModel.EntityPOCOs;

namespace Simplog.Data.InvoiceModel.Mapping
{
    public class ARPaymentMap : EntityTypeConfiguration<ARPayment>
    {
        public ARPaymentMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.PaymentNo).IsRequired().HasMaxLength(20).IsUnicode(false);
            this.Property(t => t.PaidBy).HasMaxLength(50).IsUnicode(true);
            this.Property(t => t.PrintNotes).HasMaxLength(500).IsUnicode(true);
            this.Property(t => t.InternalNotes).HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.CreatedByUserId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.PrintByUserId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.LocalCurrencyId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.BranchId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ARAccountId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.StatusCode).IsRequired().HasMaxLength(2).IsUnicode(false);
            this.Property(t => t.PaymentCurrencyId).IsRequired().HasMaxLength(15).IsUnicode(false);            
            this.Property(t => t.AccountingPaymentMethodId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.DebitAccountId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.BillToAddressId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.BillToId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.SearchFields).IsUnicode(true);
            this.Property(t => t.ChequeOrPaymentRef).HasMaxLength(30).IsUnicode(true);
            this.Property(t => t.Bank).HasMaxLength(30).IsUnicode(true);
            this.Property(t => t.BankBranch).HasMaxLength(30).IsUnicode(true);
            this.Property(t => t.Account).HasMaxLength(20).IsUnicode(true);
            this.Property(t => t.UpdatedByUserId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.CreditCardTypeId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.BankAccountId).HasMaxLength(15).IsUnicode(true);
            this.Property(t => t.CashbookId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.TransferStatusCode).HasMaxLength(2).IsUnicode(false);
            this.Property(t => t.TransferError).HasMaxLength(8000).IsOptional().IsUnicode(true);
            this.Property(t => t.ExternalAccountingEntityId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.InvoiceNumber).HasMaxLength(20).IsUnicode(false);
            this.Property(t => t.ShipmentNumber).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ApprovedByUserId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.SATXML).IsMaxLength().IsUnicode(true);
            this.Property(t => t.SATPaymentMethodCode).HasMaxLength(2).IsUnicode(false);
            this.Property(t => t.SATTransferStatusCode).IsRequired().HasMaxLength(2).IsUnicode(false);
            this.Property(t => t.BankAccountLiteId).HasMaxLength(15).IsUnicode(true);
            this.Property(t => t.TransmissionError)
     .HasMaxLength(8000)
     .IsUnicode(true);

            this.Property(t => t.SATAdditionalFieldsXML).IsMaxLength().IsUnicode(true);
            this.Property(t => t.MetodoPagoCode).HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.TipoCadenaPago).HasMaxLength(2).IsUnicode(false);
            this.Property(t => t.CadPago).HasMaxLength(200).IsUnicode(false);
            this.Property(t => t.CertPago).IsMaxLength().IsUnicode(false);
            this.Property(t => t.SelloPago).IsMaxLength().IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("ARPayments");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.PaymentNo).HasColumnName("PaymentNo");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.PrintDate).HasColumnName("PrintDate");
            this.Property(t => t.AmountInLocalCurrency).HasColumnName("AmountInLocalCurrency");
            this.Property(t => t.IsClosed).HasColumnName("IsClosed");
            this.Property(t => t.AmountInPaymentCurrency).HasColumnName("AmountInPaymentCurrency");
            this.Property(t => t.PaidBy).HasColumnName("PaidBy");
            this.Property(t => t.PrintNotes).HasColumnName("PrintNotes");
            this.Property(t => t.InternalNotes).HasColumnName("InternalNotes");
            this.Property(t => t.PaymentCurrencyExchangeRate).HasColumnName("PaymentCurrencyExchangeRate");
            this.Property(t => t.ExchangeRateDate).HasColumnName("ExchangeRateDate");
            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId");
            this.Property(t => t.PrintByUserId).HasColumnName("PrintByUserId");
            this.Property(t => t.LocalCurrencyId).HasColumnName("LocalCurrencyId");
            this.Property(t => t.BranchId).HasColumnName("BranchId");
            this.Property(t => t.ARAccountId).HasColumnName("ARAccountId");
            this.Property(t => t.StatusCode).HasColumnName("StatusCode");
            this.Property(t => t.PaymentCurrencyId).HasColumnName("PaymentCurrencyId");
            this.Property(t => t.AccountingPaymentMethodId).HasColumnName("AccountingPaymentMethodId");
            this.Property(t => t.DebitAccountId).HasColumnName("DebitAccountId");
            this.Property(t => t.BillToAddressId).HasColumnName("BillToAddressId");
            this.Property(t => t.BillToId).HasColumnName("BillToId");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
            this.Property(t => t.OpenAmount).HasColumnName("OpenAmount");
            this.Property(t => t.ChequeOrPaymentRef).HasColumnName("ChequeOrPaymentRef");
            this.Property(t => t.Bank).HasColumnName("Bank");
            this.Property(t => t.BankBranch).HasColumnName("BankBranch");
            this.Property(t => t.Account).HasColumnName("Account");
            this.Property(t => t.ValueDate).HasColumnName("ValueDate");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId");
            this.Property(t => t.ProfitCurrencyExchangeRate).HasColumnName("ProfitCurrencyExchangeRate");
            this.Property(t => t.AmountInProfitCurrency).HasColumnName("AmountInProfitCurrency");
            this.Property(t => t.RegisterDate).HasColumnName("RegisterDate");
            this.Property(t => t.CreditCardTypeId).HasColumnName("CreditCardTypeId");
            this.Property(t => t.BankAccountId).HasColumnName("BankAccountId");
            this.Property(t => t.CashbookId).HasColumnName("CashbookId");
            this.Property(t => t.TransferTries).HasColumnName("TransferTries");
            this.Property(t => t.TransferError).HasColumnName("TransferError");
            this.Property(t => t.IsTransferStarted).HasColumnName("IsTransferStarted");
            this.Property(t => t.TransferStatusCode).HasColumnName("TransferStatusCode");
            this.Property(t => t.ExternalAccountingEntityId).HasColumnName("ExternalAccountingEntityId");
            this.Property(t => t.SATPaymentMethodCode).HasColumnName("SATPaymentMethodCode");
            this.Property(t => t.SATTransferStatusCode).HasColumnName("SATTransferStatusCode");
            this.Property(t => t.TransmissionError).HasColumnName("TransmissionError");
            this.Property(t => t.BankAccountLiteId).HasColumnName("BankAccountLiteId");
            this.Property(t => t.MetodoPagoCode).HasColumnName("MetodoPagoCode");
            this.Property(t => t.IsFullAccounting).HasColumnName("IsFullAccounting");
            this.Property(t => t.TipoCadenaPago).HasColumnName("TipoCadenaPago");
            this.Property(t => t.CadPago).HasColumnName("CadPago");
            this.Property(t => t.CertPago).HasColumnName("CertPago");
            this.Property(t => t.SelloPago).HasColumnName("SelloPago");
            this.Property(t => t.SATApprovalDate).HasColumnName("SATApprovalDate");
            this.Property(t => t.ApprovedDate).HasColumnName("ApprovedDate");
            this.Property(t => t.ApprovedByUserId).HasColumnName("ApprovedByUserId");
            this.Property(t => t.FirstApproveDate).HasColumnName("FirstApproveDate");
            this.Property(t => t.IsExternalEntity).HasColumnName("IsExternalEntity");

            // Relationships
            this.HasOptional(t => t.ARAccount).WithMany().HasForeignKey(d => d.ARAccountId);
            this.HasOptional(t => t.DebitAccount).WithMany().HasForeignKey(d => d.DebitAccountId);
            this.HasRequired(t => t.BillToAddress).WithMany().HasForeignKey(d => d.BillToAddressId);
            this.HasRequired(t => t.AccountingPaymentMethod).WithMany().HasForeignKey(d => d.AccountingPaymentMethodId);
            this.HasRequired(t => t.Status).WithMany().HasForeignKey(d => d.StatusCode);
            this.HasRequired(t => t.Branch).WithMany().HasForeignKey(d => d.BranchId).WillCascadeOnDelete(false);
            this.HasRequired(t => t.BillToCard).WithMany().HasForeignKey(d => d.BillToId);
            this.HasRequired(t => t.CreatedByUser).WithMany().HasForeignKey(d => d.CreatedByUserId).WillCascadeOnDelete(false);
            this.HasOptional(t => t.CreditCardType).WithMany().HasForeignKey(d => d.CreditCardTypeId);
            this.HasRequired(t => t.LocalCurrency).WithMany().HasForeignKey(d => d.LocalCurrencyId).WillCascadeOnDelete(false);
            this.HasRequired(t => t.PaymentCurrency).WithMany().HasForeignKey(d => d.PaymentCurrencyId).WillCascadeOnDelete(false);
            this.HasOptional(t => t.PrintByUser).WithMany().HasForeignKey(d => d.PrintByUserId).WillCascadeOnDelete(false);
            this.HasRequired(t => t.UpdatedByUser).WithMany().HasForeignKey(d => d.UpdatedByUserId).WillCascadeOnDelete(false);
            this.HasRequired(t => t.TransferStatus).WithMany().HasForeignKey(d => d.TransferStatusCode);
            this.HasOptional(t => t.SATPaymentMethod).WithMany().HasForeignKey(d => d.SATPaymentMethodCode);
            this.HasRequired(t => t.SATTransferStatus).WithMany().HasForeignKey(d => d.SATTransferStatusCode);
            this.HasOptional(t => t.BankAccountLite).WithMany().HasForeignKey(d => d.BankAccountLiteId);
            this.HasOptional(t => t.MetodoPago).WithMany().HasForeignKey(d => d.MetodoPagoCode);

            this.Property(t => t.SATAdditionalFieldsXML).HasColumnName("SATAdditionalFieldsXML");
            this.HasOptional(t => t.ApprovedByUser).WithMany().HasForeignKey(d => d.ApprovedByUserId);


        }
    }
}
