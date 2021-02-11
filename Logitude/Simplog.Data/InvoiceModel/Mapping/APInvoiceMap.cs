using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.InvoiceModel.EntityPOCOs;

namespace Simplog.Data.InvoiceModel.Mapping
{
    public class APInvoiceMap : EntityTypeConfiguration<APInvoice>
    {
        public APInvoiceMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.InternalNumber).IsRequired().HasMaxLength(20).IsUnicode(false);
            this.Property(t => t.VendorId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.VATNumber).HasMaxLength(30).IsUnicode(false);
            this.Property(t => t.InvoiceNumber).IsRequired().HasMaxLength(25).IsUnicode(false);
            this.Property(t => t.PaymentTermId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.InvoiceCurrencyId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.LocalCurrencyId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.InternalNotes).HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.StatusCode).IsRequired().HasMaxLength(2).IsUnicode(false);
            this.Property(t => t.CreatedByUserId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ProfitCurrencyId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.UpdatedByUserId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.MainEntityReference).HasMaxLength(20).IsUnicode(false);
            this.Property(t => t.MainEntityId).HasMaxLength(15).IsUnicode(false);
            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms == "oracle")
            {
                this.Property(t => t.SearchFields).HasMaxLength(2000).IsUnicode(true);
            }
            else
            {
                this.Property(t => t.SearchFields).IsMaxLength().IsUnicode(true);
            }

            this.Property(t => t.BranchId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.HouseNumber).HasMaxLength(20).IsUnicode(true);
            this.Property(t => t.MasterNumber).HasMaxLength(30).IsUnicode(true);
            this.Property(t => t.Description).HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.CreditAccount).HasMaxLength(40).IsOptional().IsUnicode(false);
            this.Property(t => t.TransferError).HasMaxLength(250).IsOptional().IsUnicode(true);
            this.Property(t => t.TransferStatusCode).HasMaxLength(2).IsUnicode(false);
            this.Property(t => t.AccountingExternalCode).HasMaxLength(25).IsUnicode(true);
            this.Property(t => t.PaymentTermExternalId).HasMaxLength(25).IsUnicode(false);
            this.Property(t => t.ApprovedByUserId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.VendorGLAccountId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ExternalAccountingEntityId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.CreatedByPartner).HasMaxLength(25).IsUnicode(false);
            this.Property(t => t.Field1).HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.Field2).HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.Field3).HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.Field4).HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.Field5).HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.Field6).HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.Field7).HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.Field8).HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.Field9).HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.Field10).HasMaxLength(250).IsUnicode(true);

            // Table & Column Mappings
            this.ToTable("APInvoices");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.InternalNumber).HasColumnName("InternalNumber");
            this.Property(t => t.VendorId).HasColumnName("VendorId");
            this.Property(t => t.VATNumber).HasColumnName("VATNumber");
            this.Property(t => t.InvoiceNumber).HasColumnName("InvoiceNumber");
            this.Property(t => t.InvoiceDate).HasColumnName("InvoiceDate").IsRequired();
            this.Property(t => t.PaymentTermId).HasColumnName("PaymentTermId");
            this.Property(t => t.DueDate).HasColumnName("DueDate").IsRequired();
            this.Property(t => t.InvoiceCurrencyExchangeRate).HasColumnName("InvoiceCurrencyExchangeRate").IsRequired();
            this.Property(t => t.ExchangeRateDate).HasColumnName("ExchangeRateDate");
            this.Property(t => t.InvoiceCurrencyId).HasColumnName("InvoiceCurrencyId");
            this.Property(t => t.LocalCurrencyId).HasColumnName("LocalCurrencyId");
            this.Property(t => t.InternalNotes).HasColumnName("InternalNotes");
            this.Property(t => t.SubTotalInLocalCurrency).HasColumnName("SubTotalInLocalCurrency").IsRequired();
            this.Property(t => t.SubTotalInInvoiceCurrency).HasColumnName("SubTotalInInvoiceCurrency").IsRequired();
            this.Property(t => t.AmountInInvoiceCurrency).HasColumnName("AmountInInvoiceCurrency").IsRequired();
            this.Property(t => t.AmountInLocalCurrency).HasColumnName("AmountInLocalCurrency").IsRequired();
            this.Property(t => t.StatusCode).HasColumnName("StatusCode");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate").IsRequired();
            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId");
            this.Property(t => t.IsClosed).HasColumnName("IsClosed");
            this.Property(t => t.ProfitCurrencyId).HasColumnName("ProfitCurrencyId");
            this.Property(t => t.ProfitCurrencyExchangeRate).HasColumnName("ProfitCurrencyExchangeRate").IsRequired();
            this.Property(t => t.AmountInProfitCurrency).HasColumnName("AmountInProfitCurrency").IsRequired();
            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate").IsRequired();
            this.Property(t => t.MainEntityReference).HasColumnName("MainEntityReference");
            this.Property(t => t.MainEntityId).HasColumnName("MainEntityId");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
            this.Property(t => t.AmountDue).HasColumnName("AmountDue");
            this.Property(t => t.RefundAmount).HasColumnName("RefundAmount");
            this.Property(t => t.AmountDueInLocalCurrency).HasColumnName("AmountDueInLocalCurrency");
            this.Property(t => t.AmountDueInProfitCurrency).HasColumnName("AmountDueInProfitCurrency");
            this.Property(t => t.BranchId).HasColumnName("BranchId");
            this.Property(t => t.HouseNumber).HasColumnName("HouseNumber");
            this.Property(t => t.MasterNumber).HasColumnName("MasterNumber");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.CreditAccount).HasColumnName("CreditAccount");
            this.Property(t => t.TransferTries).HasColumnName("TransferTries");
            this.Property(t => t.TransferError).HasColumnName("TransferError");
            this.Property(t => t.IsTransferStarted).HasColumnName("IsTransferStarted");
            this.Property(t => t.TransferStatusCode).HasColumnName("TransferStatusCode");
            this.Property(t => t.AccountingExternalCode).HasColumnName("AccountingExternalCode");
            this.Property(t => t.PaymentTermExternalId).HasColumnName("PaymentTermExternalId");
            this.Property(t => t.IsMultipleEntities).HasColumnName("IsMultipleEntities");
            this.Property(t => t.ApprovedDate).HasColumnName("ApprovedDate");
            this.Property(t => t.ApprovedByUserId).HasColumnName("ApprovedByUserId");
            this.Property(t => t.OperationalDate).HasColumnName("OperationalDate");
            this.Property(t => t.VendorGLAccountId).HasColumnName("VendorGLAccountId");
            this.Property(t => t.AccountingDate).HasColumnName("AccountingDate");
            this.Property(t => t.IsExternalEntity).HasColumnName("IsExternalEntity");
            this.Property(t => t.IsGeneralInvoice).HasColumnName("IsGeneralInvoice");
            this.Property(t => t.ExternalAccountingEntityId).HasColumnName("ExternalAccountingEntityId");
            this.Property(t => t.FirstApproveDate).HasColumnName("FirstApproveDate");
            this.Property(t => t.CreatedByPartner).HasColumnName("CreatedByPartner");
            this.Property(t => t.Field1).HasColumnName("Field1");
            this.Property(t => t.Field2).HasColumnName("Field2");
            this.Property(t => t.Field3).HasColumnName("Field3");
            this.Property(t => t.Field4).HasColumnName("Field4");
            this.Property(t => t.Field5).HasColumnName("Field5");
            this.Property(t => t.Field6).HasColumnName("Field6");
            this.Property(t => t.Field7).HasColumnName("Field7");
            this.Property(t => t.Field8).HasColumnName("Field8");
            this.Property(t => t.Field9).HasColumnName("Field9");
            this.Property(t => t.Field10).HasColumnName("Field10");
            this.Property(t => t.TotalVATOnly).HasColumnName("TotalVATOnly");
            this.Property(t => t.PaidDate).HasColumnName("PaidDate");


            // Relationships
            this.HasRequired(t => t.Status).WithMany().HasForeignKey(d => d.StatusCode);
            this.HasRequired(t => t.Branch).WithMany().HasForeignKey(d => d.BranchId);
            this.HasRequired(t => t.PaymentTerm).WithMany().HasForeignKey(d => d.PaymentTermId);
            this.HasRequired(t => t.CreatedByUser).WithMany().HasForeignKey(d => d.CreatedByUserId);
            this.HasRequired(t => t.InvoiceCurrency).WithMany().HasForeignKey(d => d.InvoiceCurrencyId);
            this.HasRequired(t => t.LocalCurrency).WithMany().HasForeignKey(d => d.LocalCurrencyId);
            this.HasRequired(t => t.ProfitCurrency).WithMany().HasForeignKey(d => d.ProfitCurrencyId);
            this.HasRequired(t => t.UpdatedByUser).WithMany().HasForeignKey(d => d.UpdatedByUserId);
            this.HasRequired(t => t.VendorCard).WithMany().HasForeignKey(d => d.VendorId);
            this.HasRequired(t => t.TransferStatus).WithMany().HasForeignKey(d => d.TransferStatusCode);
            this.HasOptional(t => t.ApprovedByUser).WithMany().HasForeignKey(d => d.ApprovedByUserId);
        }
    }
}
