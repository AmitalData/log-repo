using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.InvoiceModel.EntityPOCOs;

namespace Simplog.Data.InvoiceModel.Mapping
{
    public class APPaymentMap : EntityTypeConfiguration<APPayment>
    {
        public APPaymentMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.PaymentNo)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false);

            this.Property(t => t.PrintNotes)
                .HasMaxLength(500)
                .IsUnicode(true);

            this.Property(t => t.InternalNotes)
                .HasMaxLength(250)
                .IsUnicode(true);

            this.Property(t => t.ChequeOrPaymentRef)
                .HasMaxLength(20)
                .IsUnicode(false);

            this.Property(t => t.Bank)
                .HasMaxLength(40)
                .IsUnicode(false);

            this.Property(t => t.BankBranch)
                .HasMaxLength(10)
                .IsUnicode(false);

            this.Property(t => t.Account)
                .HasMaxLength(20)
                .IsUnicode(false);

            this.Property(t => t.CreatedByUserId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.PrintedByUserId)
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.LocalCurrencyId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.VendorId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.StatusCode)
                .IsRequired()
                .HasMaxLength(2)
                .IsUnicode(false);

            this.Property(t => t.PaymentCurrencyId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);
            
            this.Property(t => t.PaymentMethodId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.AccountingPaymentMethodId)
              .HasMaxLength(15)
              .IsUnicode(false);

            this.Property(t => t.VendorAddressId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.SearchFields)                
                .IsUnicode(true);

            this.Property(t => t.UpdatedByUserId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.BranchId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.CreditCardTypeId)
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.ExternalAccountingEntityId)
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.TransferStatusCode)
                .HasMaxLength(2)
                .IsUnicode(false);

            this.Property(t => t.ApprovedByUserId)
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.TaxDeductionLocalAmount)
                .HasPrecision(16, 2);

            this.Property(t => t.BankAccountId)
                .HasMaxLength(15).IsUnicode(true);

            // Table & Column Mappings
            this.ToTable("APPayments");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.PaymentNo).HasColumnName("PaymentNo");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.PrintDate).HasColumnName("PrintDate");
            this.Property(t => t.AmountInLocalCurrency).HasColumnName("AmountInLocalCurrency");
            this.Property(t => t.IsClosed).HasColumnName("IsClosed");
            this.Property(t => t.AmountInPaymentCurrency).HasColumnName("AmountInPaymentCurrency");
            this.Property(t => t.PrintNotes).HasColumnName("PrintNotes");
            this.Property(t => t.InternalNotes).HasColumnName("InternalNotes");
            this.Property(t => t.PaymentCurrencyExchangeRate).HasColumnName("PaymentCurrencyExchangeRate");
            
            this.Property(t => t.OpenAmount).HasColumnName("OpenAmount");
            this.Property(t => t.ChequeOrPaymentRef).HasColumnName("ChequeOrPaymentRef");
            this.Property(t => t.ValueDate).HasColumnName("ValueDate");
            this.Property(t => t.Bank).HasColumnName("Bank");
            this.Property(t => t.BankBranch).HasColumnName("BankBranch");
            this.Property(t => t.Account).HasColumnName("Account");
            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId");
            this.Property(t => t.PrintedByUserId).HasColumnName("PrintedByUserId");
            this.Property(t => t.LocalCurrencyId).HasColumnName("LocalCurrencyId");
            this.Property(t => t.VendorId).HasColumnName("VendorId");
            this.Property(t => t.StatusCode).HasColumnName("StatusCode");
            this.Property(t => t.PaymentCurrencyId).HasColumnName("PaymentCurrencyId");
            this.Property(t => t.PaymentMethodId).HasColumnName("PaymentMethodId");
            this.Property(t => t.AccountingPaymentMethodId).HasColumnName("AccountingPaymentMethodId");            
            this.Property(t => t.VendorAddressId).HasColumnName("VendorAddressId");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId");
            this.Property(t => t.BranchId).HasColumnName("BranchId");
            this.Property(t => t.ProfitCurrencyExchangeRate).HasColumnName("ProfitCurrencyExchangeRate");
            this.Property(t => t.AmountInProfitCurrency).HasColumnName("AmountInProfitCurrency");
            this.Property(t => t.RegisterDate).HasColumnName("RegisterDate");
            this.Property(t => t.CreditCardTypeId).HasColumnName("CreditCardTypeId");
            this.Property(t => t.ExternalAccountingEntityId).HasColumnName("ExternalAccountingEntityId");
            this.Property(t => t.TransferStatusCode).HasColumnName("TransferStatusCode");
            this.Property(t => t.TaxDeductionLocalAmount).HasColumnName("TaxDeductionLocalAmount");
            this.Property(t => t.TaxDeductionPercentage).HasColumnName("TaxDeductionPercentage");
            this.Property(t => t.ApprovedByUserId).HasColumnName("ApprovedByUserId");
            this.Property(t => t.ApprovedDateTime).HasColumnName("ApprovedDateTime");
            this.Property(t => t.BankAccountId).HasColumnName("BankAccountId");
            this.Property(t => t.FirstApproveDate).HasColumnName("FirstApproveDate");

            //#if ORACLE_DB
            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms == "oracle")
            {
                this.Property(t => t.PaymentCurrencyExchangeRateDate).HasColumnName("ExchangeRateDate");
            }
            //#else
            else
            {
                this.Property(t => t.PaymentCurrencyExchangeRateDate).HasColumnName("PaymentCurrencyExchangeRateDate");
            }
           
//#endif
            // Relationships
            this.HasRequired(t => t.VendorAddress)
                .WithMany()
                .HasForeignKey(d => d.VendorAddressId)
                .WillCascadeOnDelete(false);
            this.HasRequired(t => t.PaymentMethod)
                .WithMany()
                .HasForeignKey(d => d.PaymentMethodId);
            this.HasOptional(t => t.AccountingPaymentMethod)
               .WithMany()
               .HasForeignKey(d => d.AccountingPaymentMethodId);
            this.HasRequired(t => t.Status)
                .WithMany()
                .HasForeignKey(d => d.StatusCode);
            this.HasRequired(t => t.Branch)
                .WithMany()
                .HasForeignKey(d => d.BranchId)
                .WillCascadeOnDelete(false);
            this.HasRequired(t => t.CreatedByUser)
                .WithMany()
                .HasForeignKey(d => d.CreatedByUserId)
                .WillCascadeOnDelete(false);
            this.HasOptional(t => t.CreditCardType)
                .WithMany()
                .HasForeignKey(d => d.CreditCardTypeId);
            this.HasRequired(t => t.LocalCurrency)
                .WithMany()
                .HasForeignKey(d => d.LocalCurrencyId)
                .WillCascadeOnDelete(false);
            this.HasRequired(t => t.PaymentCurrency)
                .WithMany()
                .HasForeignKey(d => d.PaymentCurrencyId)
                .WillCascadeOnDelete(false);
            this.HasOptional(t => t.PrintedByUser)
                .WithMany()
                .HasForeignKey(d => d.PrintedByUserId);
            this.HasRequired(t => t.UpdatedByUser)
                .WithMany()
                .HasForeignKey(d => d.UpdatedByUserId)
                .WillCascadeOnDelete(false);
            this.HasRequired(t => t.VendorCard)
                .WithMany()
                .HasForeignKey(d => d.VendorId)
                .WillCascadeOnDelete(false);
            this.HasRequired(t => t.TransferStatus)
                .WithMany()
                .HasForeignKey(d => d.TransferStatusCode);

            this.HasRequired(t => t.ApprovedByUser)
              .WithMany()
              .HasForeignKey(d => d.ApprovedByUserId);
        }
    }
}
