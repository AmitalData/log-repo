using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class CardMap : EntityTypeConfiguration<Card>
    {
        public CardMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.EnglishName).IsRequired().HasMaxLength(70).IsUnicode(false);
            this.Property(t => t.VatNumber).HasMaxLength(20).IsUnicode(false);
            this.Property(t => t.LocalName).HasMaxLength(100).IsUnicode(true);
            this.Property(t => t.PaymentTermId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ReceivablesAccountingCard).HasMaxLength(25).IsUnicode(false);
            this.Property(t => t.PayablesAccountingCard).HasMaxLength(25).IsUnicode(false);
            this.Property(t => t.PartnerTypeId).IsRequired().IsFixedLength().HasMaxLength(2).IsUnicode(false);
            this.Property(t => t.Code).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Notes).HasMaxLength(2000).IsUnicode(true);
            this.Property(t => t.Website).HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.InvoiceCurrencyId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.VatTypeId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.SearchFields).HasMaxLength(1000).IsUnicode(true);
            this.Property(t => t.CityName).HasMaxLength(25).IsUnicode(true);
            this.Property(t => t.ImageDetailId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.BankName).HasMaxLength(40).IsUnicode(true);
            this.Property(t => t.BankAddress).HasMaxLength(100).IsUnicode(true);
            this.Property(t => t.Swift).HasMaxLength(20).IsUnicode(false);
            this.Property(t => t.AccountNumber).HasMaxLength(25).IsUnicode(false);
            this.Property(t => t.IBANNumber).HasMaxLength(30).IsUnicode(true);
            this.Property(t => t.InvitationDate);
            this.Property(t => t.SharedLogisticsInvitationStatusCode);
            this.Property(t => t.LastLoginDate);
            this.Ignore(t => t.PartnerTypeName);
            this.Property(t => t.ClassifierId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.CollectorId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.PrimaryContactId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.CreatedByUserId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.UpdatedByUserId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.CountryId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.CountryCode).HasMaxLength(2).IsUnicode(false);
            this.Property(t => t.CountryName).HasMaxLength(120).IsUnicode(false);
            this.Property(t => t.SalesmanUserId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.IRSPlace).HasMaxLength(60).IsUnicode(false);
            this.Property(t => t.IRSNumber).HasMaxLength(60).IsUnicode(false);
            this.Property(t => t.SupportNotes).HasMaxLength(2000).IsUnicode(true);
            this.Property(t => t.GLAccountId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ExternalAccountingBusinessArea).HasMaxLength(25).IsUnicode(false);
            this.Property(t => t.SATPaymentMethodCode).HasMaxLength(2).IsUnicode(false);
            this.Property(t => t.ExternalId2).HasMaxLength(25).IsUnicode(false);
            this.Property(t => t.SATForeignRFC).HasMaxLength(20).IsUnicode(false);
            this.Property(t => t.MetodoPagoCode).HasMaxLength(3).IsUnicode(false);
            this.Property(t => t.UsoCFDICode).HasMaxLength(3).IsUnicode(false);
            this.Property(t => t.Address1).HasMaxLength(65).IsUnicode(true);
            this.Property(t => t.Address2).HasMaxLength(65).IsUnicode(true);
            this.Property(t => t.ZipCode).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Phone).HasMaxLength(40).IsUnicode(false);
            this.Property(t => t.StateName).HasMaxLength(40).IsUnicode(false);
            this.Property(t => t.CreatedByPartner).HasMaxLength(25).IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("Cards");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.EnglishName).HasColumnName("EnglishName");
            this.Property(t => t.VatNumber).HasColumnName("VatNumber");
            this.Property(t => t.LocalName).HasColumnName("LocalName");
            this.Property(t => t.InActive).HasColumnName("InActive");
            this.Property(t => t.PaymentTermId).HasColumnName("PaymentTermId");
            this.Property(t => t.ReceivablesAccountingCard).HasColumnName("ReceivablesAccountingCard");
            this.Property(t => t.PayablesAccountingCard).HasColumnName("PayablesAccountingCard");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.PartnerTypeId).HasColumnName("PartnerTypeId");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.Notes).HasColumnName("Notes");
            this.Property(t => t.Website).HasColumnName("Website");
            this.Property(t => t.InvoiceCurrencyId).HasColumnName("InvoiceCurrencyId");
            this.Property(t => t.VatTypeId).HasColumnName("VatTypeId");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
            this.Property(t => t.ImageDetailId).HasColumnName("ImageDetailId");
            this.Property(t => t.BankName).HasColumnName("BankName");
            this.Property(t => t.BankAddress).HasColumnName("BankAddress");
            this.Property(t => t.Swift).HasColumnName("Swift");
            this.Property(t => t.AccountNumber).HasColumnName("AccountNumber");
            this.Property(t => t.IBANNumber).HasColumnName("IBANNumber");
            this.Property(t => t.InvitationDate).HasColumnName("InvitationDate");
            this.Property(t => t.LastLoginDate).HasColumnName("LastLoginDate");
            this.Property(t => t.ClassifierId).HasColumnName("ClassifierId");
            this.Property(t => t.CollectorId).HasColumnName("CollectorId");
            this.Property(t => t.PrimaryContactId).HasColumnName("PrimaryContactId");
            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId");
            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId");
            this.Property(t => t.IsCustomer).HasColumnName("IsCustomer");
            this.Property(t => t.EnableConsolidationInvoices).HasColumnName("EnableConsolidationInvoices");
            this.Property(t => t.CityName).HasColumnName("CityName");
            this.Property(t => t.CountryId).HasColumnName("CountryId");
            this.Property(t => t.CountryCode).HasColumnName("CountryCode");
            this.Property(t => t.CountryName).HasColumnName("CountryName");
            this.Property(t => t.SalesmanUserId).HasColumnName("SalesmanUserId");
            this.Property(t => t.IsActiveForMobile).HasColumnName("IsActiveForMobile");
            this.Property(t => t.IRSPlace).HasColumnName("IRSPlace");
            this.Property(t => t.IRSNumber).HasColumnName("IRSNumber");
            this.Property(t => t.SupportNotes).HasColumnName("SupportNotes");
            this.Property(t => t.GLAccountId).HasColumnName("GLAccountId");
            this.Property(t => t.ExternalAccountingBusinessArea).HasColumnName("ExternalAccountingBusinessArea");
            this.Property(t => t.SATPaymentMethodCode).HasColumnName("SATPaymentMethodCode");
            this.Property(t => t.ExternalId2).HasColumnName("ExternalId2");
            this.Property(t => t.SATForeignRFC).HasColumnName("SATForeignRFC");         
            this.Property(t => t.UsoCFDICode).HasColumnName("UsoCFDICode");
            this.Property(t => t.Address1).HasColumnName("Address1");
            this.Property(t => t.Address2).HasColumnName("Address2");
            this.Property(t => t.Phone).HasColumnName("Phone");
            this.Property(t => t.ZipCode).HasColumnName("ZipCode");
            this.Property(t => t.StateName).HasColumnName("StateName");
            this.Property(t => t.IsInternationalPartner).HasColumnName("IsInternationalPartner");
            this.Property(t => t.IsAutonomy).HasColumnName("IsAutonomy");
            this.Property(t => t.CreatedByPartner).HasColumnName("CreatedByPartner");

            //#if ORACLE_DB
            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
         if (dbms == "oracle")
         {
             this.Property(t => t.SharedLogisticsInvitationStatusCode).HasColumnName("SharedLogInvitationStatCode");
         }
         //#else
         else
         {
             this.Property(t => t.SharedLogisticsInvitationStatusCode).HasColumnName("SharedLogisticsInvitationStatusCode");
         }
            
//#endif

            this.HasOptional(t => t.VatType).WithMany().HasForeignKey(d => d.VatTypeId);
            this.HasOptional(t => t.InvoiceCurrency).WithMany().HasForeignKey(d => d.InvoiceCurrencyId);
            this.HasRequired(t => t.PartnerType).WithMany().HasForeignKey(d => d.PartnerTypeId);
            this.HasOptional(t => t.PaymentTerm).WithMany().HasForeignKey(d => d.PaymentTermId);
            this.HasOptional(t => t.CollectorUser).WithMany().HasForeignKey(d => d.CollectorId);
            this.HasOptional(t => t.ClassifierUser).WithMany().HasForeignKey(d => d.ClassifierId);
            this.HasOptional(t => t.CreatedByUser).WithMany().HasForeignKey(d => d.CreatedByUserId);
            this.HasOptional(t => t.UpdatedByUser).WithMany().HasForeignKey(d => d.UpdatedByUserId);
            this.HasOptional(t => t.MainAddressCountry).WithMany().HasForeignKey(d => d.CountryId);
            this.HasOptional(t => t.SalesmanUser).WithMany().HasForeignKey(d => d.SalesmanUserId);
            this.HasOptional(t => t.MetodoPago).WithMany().HasForeignKey(d => d.MetodoPagoCode);
            this.HasOptional(t => t.UsoCFDI).WithMany().HasForeignKey(d => d.UsoCFDICode);
        }
    }
}
