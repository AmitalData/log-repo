using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class ShippingLineList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string Code { get; set; }
        public string EnglishName { get; set; }
        public string VatNumber { get; set; }
        public string LocalName { get; set; }
        public bool AddedManually { get; set; }
        public string Remark { get; set; }
        public string OurCreditNumber { get; set; }
        public string ShippingAgentEnglishName { get; set; }
        public bool InActive { get; set; }
        public string ReceivablesAccountingCard { get; set; }
        public string PayablesAccountingCard { get; set; }
        public string InvoiceCurrencyId { get; set; }
        public string Notes { get; set; }
        public string Website { get; set; }
        public string PaymentTermEnglishName { get; set; }
        public string PaymentTermId { get; set; }
        public string SCACCode { get; set; }
        public string SearchFields { get; set; }
        public string VatTypeId { get; set; }
        public bool EnableConsolidationInvoices { get; set; }
        public string CityName { get; set; }
        public string CountryId { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string ExternalAccountingBusinessArea { get; set; }
        public string PaymentMethodCode { get; set; }
        public string ExternalId2 { get; set; }
        public string SATForeignRFC { get; set; }
        public string MetodoPagoCode { get; set; }
        public string UsoCFDICode { get; set; }
        public bool IsINTTRARegistered { get; set; }
        public string INTTRARegistrationNotes { get; set; }

        public string PrimaryContactName { get; set; }
        public string PrimaryContactEmail { get; set; }
        public string PrimaryContactPhone { get; set; }

        public string CBSA { get; set; }
        public string CAAT { get; set; }
    }
}