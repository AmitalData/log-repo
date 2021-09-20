using System;
using System.Collections.Generic;


namespace Logitude.FullAccounting.Test.Models
{
    public class VendorPM
    {
        
        public string Id { get; set; }
        
        public int Tenant { get; set; }
        
        public bool IsSecured { get; set; }

        
        
        public string TenantAddressId { get; set; }

        
        
        public string BankName { get; set; }

        
        
        public string BankAddress { get; set; }

        
        
        public string Swift { get; set; }

        
        
        public string AccountNumber { get; set; }

        
        
        public string IBANNumber { get; set; }

        
        
        public bool IsHybrid { get; set; }

        
        
        public string EnglishName { get; set; }//card       

        
        
        public string Website { get; set; }

        
        
        public string VatNumber { get; set; }//card

        
        
        public string LocalName { get; set; }//card

        
        
        public bool InActive { get; set; }//card

        
        
        public string PaymentTermId { get; set; }//card

        
        
        public string ReceivablesAccountingCard { get; set; }

        
        
        public string PayablesAccountingCard { get; set; }

        
        
        public string Notes { get; set; }//card

        
        
        public string Code { get; set; }//card

        
        
        public string InvoiceCurrencyId { get; set; }

        
        
        public string VatTypeId { get; set; }

        
        
        public string ComputedLocalName { get; set; }

        
        
        public DateTime? CreateDate { get; set; }

        
        
        public DateTime? UpdateDate { get; set; }

        
        
        public string CreatedByUserId { get; set; }

        
        
        public string UpdatedByUserId { get; set; }

        //card
        
        public string PartnerTypeId { get; set; }

        //card
        
        public string CardPMId { get; set; }

        
        
        public string ExistedContactId { get; set; }

        
        
        public bool FieldsChanged { get; set; }

        
        
        public string CityName { get; set; }

        
        
        public string CountryId { get; set; }

        
        
        public string CountryCode { get; set; }

        
        
        public string CountryName { get; set; }

        
        public string SearchFields { get; set; }

        
        
        public bool IsExternal { get; set; }

        
        
        public string PrimaryContactId { get; set; }

        
        
        public bool IsFirstContactToAdd { get; set; }

        
        public bool EnableConsolidationInvoices { get; set; }

        
        
        public string IRSPlace { get; set; }

        
        
        public string IRSNumber { get; set; }

        
        public string PrimaryContactName { get; set; }
        
        public string PrimaryContactEmail { get; set; }
        
        public string PrimaryContactPhone { get; set; }

       
        
        public CardPM Card { get; set; }

       

        
        
        public string ExternalAccountingBusinessArea { get; set; }

        
        
        public string PaymentMethodCode { get; set; }

        
        
        public string ExternalId2 { get; set; }

        
        
        public string SATForeignRFC { get; set; }

        
        public string MetodoPagoCode { get; set; }

        
        public string UsoCFDICode { get; set; }

        
        public string GLAccountId { get; set; }
        public string CreatedByPartner { get; set; }

        
        public bool AccountingVATSplit { get; set; }

        
        public string UploadingUniqueKey { get; set; }

        
        public string GLAccountNumber { get; set; }
        
        public string BillToId { get; set; }
    }
}