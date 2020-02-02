using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class AirlineList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string Code { get; set; }
        public string EnglishName { get; set; }
        public string VatNumber { get; set; }
        public string LocalName { get; set; }
        public string Prefix { get; set; }
        public string AWBAccount { get; set; }
        public bool AddedManually { get; set; }
        public bool InActive { get; set; }
        public string ReceivablesAccountingCard { get; set; }
        public string PayablesAccountingCard { get; set; }
        public string Remark { get; set; }
        public string InvoiceCurrencyId { get; set; }
        public string VatTypeId { get; set; }       
        public bool CheckDigit { get; set; }
        public bool LimitedLength { get; set; }
        public string Notes { get; set; }
        public string PaymentTermEnglishName { get; set; }
        public string PaymentTermId { get; set; }
        public string Website { get; set; }
        public string SearchFields { get; set; }
        public string TTY { get; set; }
        public bool ChampNeedsRegistration { get; set; }
        public bool IsChampRegistered { get; set; }
        public string AccountNumber { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string CreatedByUserId { get; set; }
        public string UpdatedByUserId { get; set; }
        public bool EnableConsolidationInvoices { get; set; }        
        public string CityName { get; set; }
        public string CountryId { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string GLSHKPIMA { get; set; }
        public bool IsGLSHKRegistered { get; set; }
        public bool GLSHKNeedsRegistration { get; set; }
        public bool ChampFWB { get; set; }
        public bool ChampFHL { get; set; }
        public bool ChampFSU { get; set; }
        public bool ChampFSRFSA { get; set; }
        public bool ChampFVRFVA { get; set; }
        public bool ChampFFRFFA { get; set; }
        public bool GLSHKFWB { get; set; }
        public bool GLSHKFHL { get; set; }
        public bool GLSHKFSU { get; set; }
        public bool GLSHKFSRFSA { get; set; }
        public bool GLSHKFVRFVA { get; set; }
        public bool GLSHKFFRFFA { get; set; }
        public bool IsAllowedInAirlinesRestriction { get; set; }
        public string RegistrationNotes { get; set; }
        public bool ChampRegistrationRequested { get; set; }
        public bool GLSHKRegistrationRequested { get; set; }
        public bool HasAdaptations { get; set; }
        public string ICAO { get; set; }
        public string RegistrationUpdatedBy { get; set; }
        public bool IsManagingProduct { get; set; }
        public bool IsProductMandatory { get; set; }
        public bool IsDescriptionOfGoodsFromList { get; set; }
        public int? ScheduleDays { get; set; }
        public bool NoAvailabilityInFVAMessages { get; set; }
        public bool IsDeclined { get; set; }
        public string DeclineNotes { get; set; }
        public string ExternalAccountingBusinessArea { get; set; }
        public string PaymentMethodCode { get; set; }
        public string ExternalId2 { get; set; }
        public string SATForeignRFC { get; set; }
        public string MetodoPagoCode { get; set; }
        public string UsoCFDICode { get; set; }

        public string PrimaryContactName { get; set; }
        public string PrimaryContactEmail { get; set; }
        public string PrimaryContactPhone { get; set; }
        public string StateName { get; set; }
    }
}