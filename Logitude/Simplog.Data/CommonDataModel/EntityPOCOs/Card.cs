using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class Card
    {
        [Key]
        public string Id { get; set; }
        public string BankName  { get; set; }
        public string BankAddress{ get; set; }
        public string Swift { get; set; }
        public string AccountNumber{ get; set; }
        public string IBANNumber { get; set; }
        public string EnglishName { get; set; }
        public int Tenant { get; set; }
        public string VatNumber { get; set; }
        public string LocalName { get; set; }
        public bool InActive { get; set; }
        public string ReceivablesAccountingCard { get; set; }
        public string PayablesAccountingCard { get; set; }
        public string ExternalId2 { get; set; }
        public string Notes { get; set; }        
        public string Code { get; set; }
        public string Website { get; set; }
        public string SearchFields { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public DateTime? InvitationDate { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool IsCustomer { get; set; }
        public bool EnableConsolidationInvoices { get; set; }
        public string CityName { get; set; }
        public bool IsActiveForMobile { get; set; }
        public string IRSPlace { get; set; }
        public string IRSNumber { get; set; }
        public string SupportNotes { get; set; }
        public string GLAccountId { get; set; }
        public string ExternalAccountingBusinessArea { get; set; }
        public string SATPaymentMethodCode  { get; set; }
        public string SATForeignRFC { get; set; }
        public string ZipCode { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Phone { get; set; }
        public string GLAccountDisplayNumber { get; set; }

        [ForeignKey("SalesmanUserId")]
        public virtual User SalesmanUser { get; set; }
        public string SalesmanUserId { get; set; }

        [ForeignKey("CountryId")]
        public virtual Country MainAddressCountry { get; set; }
        public string CountryId { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        
        public string StateName { get; set; }

        [ForeignKey("CreatedByUserId")]
        public virtual User CreatedByUser { get; set; }
        public string CreatedByUserId { get; set; }

        [ForeignKey("UpdatedByUserId")]
        public virtual User UpdatedByUser { get; set; }
        public string UpdatedByUserId { get; set; }

        [ForeignKey("PrimaryContactId")]
        public Contact PrimaryContact { get; set; }
        public string PrimaryContactId { get; set; }

        [ForeignKey("ImageDetailId")]
        public ImageDetail ImageDetail { get; set; }
        public string ImageDetailId { get; set; }

        [ForeignKey("InvoiceCurrencyId")]
        public virtual Currency InvoiceCurrency { get; set; }
        public string InvoiceCurrencyId { get; set; }

        [ForeignKey("VatTypeId")]
        public virtual VatType VatType { get; set; }
        public string VatTypeId { get; set; }

        [ForeignKey("PaymentTermId")]
        public virtual PaymentTerm PaymentTerm { get; set; }
        public string PaymentTermId { get; set; }

        [ForeignKey("PartnerTypeId")]
        public virtual PartnerType PartnerType { get; set; }
        public string PartnerTypeId { get; set; }
        public string PartnerTypeName { get; set; }

        [ForeignKey("SharedLogisticsInvitationStatusCode")]
        public virtual SharedLogisticsInvitationStatus SharedLogisticsInvitationStatus { get; set; }
        public int? SharedLogisticsInvitationStatusCode { get; set; }

        [ForeignKey("CollectorId")]
        public virtual User CollectorUser { get; set; }
        public string CollectorId { get; set; }

        [ForeignKey("ClassifierId")]
        public virtual User ClassifierUser { get; set; }
        public string ClassifierId { get; set; }

        public string MetodoPagoCode { get; set; }

        [ForeignKey("MetodoPagoCode")]
        public virtual MetodoPago MetodoPago { get; set; }

        public string UsoCFDICode { get; set; }

        [ForeignKey("UsoCFDICode")]
        public virtual UsoCFDI UsoCFDI { get; set; }  
        public string RegimenFiscalCode { get; set; }
        [ForeignKey("RegimenFiscalCode")]
        public virtual RegimenFiscal RegimenFiscal { get; set; }

        public virtual Trucker Trucker { get; set; }
        public virtual Airline Airline { get; set; }
        public virtual ShippingLine ShippingLine { get; set; }
        public virtual CustomAgent CustomAgent { get; set; }
        public virtual ShippingAgent ShippingAgent { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Warehouse Warehouse { get; set; }
        public virtual Agent Agent { get; set; }
        public virtual Vendor Vendor { get; set; }
        public virtual Participant Participant { get; set; }
        public virtual CustomsShipper CustomsShipper { get; set; }
        public virtual AccountingPartner AccountingPartner { get; set; }
        
        public bool IsInternationalPartner { get; set; }
        public bool IsAutonomy { get; set; }

        public string CreatedByPartner { get; set; }

        public int? StorageFreeDays { get; set; }
        public DateTime? AutomaticLastUpdateDate { get; set; }

        public bool AccountingVATSplit { get; set; }

        public string UploadingUniqueKey { get; set; }

        public string BillToId { get; set; }
    }
}
