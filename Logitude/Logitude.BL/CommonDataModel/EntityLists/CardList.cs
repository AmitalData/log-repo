using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class CardList
    {
        [Key]
        public string Id { get; set; }
        public string EnglishName { get; set; }
        public string Code { get; set; }
        public int Tenant { get; set; }
        public string VatNumber { get; set; }
        public string LocalName { get; set; }
        public bool InActive { get; set; }
        public string PaymentTermName { get; set; }
        public string PaymentTermEnglishName { get; set; }
        public string PaymentTermLocalName { get; set; }
        public string PartnerTypeName { get; set; }
        public string ReceivablesAccountingCard { get; set; }
        public string PayablesAccountingCard { get; set; }
        public string Notes { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string CreatedByUserId { get; set; }
        public string UpdatedByUserId { get; set; }
        public string PartnerTypeId { get; set; }
        public string PaymentTermId { get; set; }      
        public string WebSite { get; set; }
        public string InvoiceCurrencyId { get; set; }
        public string VatTypeId { get; set; }
        public string Prefix { get; set; }
        public string ICAO { get; set; }
        public string SearchFields { get; set; }
        public string BankName { get; set; }
        public string BankAddress { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Swift { get; set; }
        public string AccountNumber { get; set; }
        public string IBANNumber { get; set; }
        public string PrimaryContactId { get; set; }
        public bool IsActiveForMobile { get; set; }
        public string GLAccountDisplayNumber { get; set; }

        // by islam
        public bool InUse { get; set; }
        public string RecentlyAdded { get; set; }
        public DateTime? InvitationDate { get; set; }
        public int? SharedLogisticsInvitationStatusCode { get; set; }
        public string SharedLogisticsInvitationStatusName { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public string AirlineAccountNumber { get; set; }


        public string ContactId { get; set; }
        public string MainAddressId { get; set; }        
        public string PickAddressId { get; set; }
        public string BillingAddressId { get; set; }
        public string CustomerStatusCode { get; set; }
        public string CustomerStatusName { get; set; }
        public bool IsCustomer { get; set; }
        public bool EnableConsolidationInvoices { get; set; }
        public string CityName { get; set; }
        public string CountryId { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string SalesmanUserId { get; set; }
        public string SalesmanBusinessUnitId { get; set; }
        public string SalesmanUserEnglishName { get; set; }
        public string AccountManagerUserName { get; set; }
        public string AccountManagerUserId { get; set; }
        public string CASSCode { get; set; }
        public string IATACode { get; set; }
        public string RegulatedAgentCode { get; set; }
        public string KnownConsignor { get; set; }
        public DateTime? KCExpirationDate { get; set; }
        public string IRSPlace { get; set; }
        public string IRSNumber { get; set; }
        public string SupportNotes { get; set; }
        public string GLAccountId { get; set; }
        public string GLAccountCurrency { get; set; }
        public bool IsCreditLimitEnabled { get; set; }
        public double? CreditLimitAmount { get; set; }
        public double? CreditLimitOpenBalance { get; set; }
        public int? CreditLimitWarningPercentage { get; set; }
        public bool BlockNewInvoiceCreation { get; set; }
        public bool BlockNewShipmentCreation { get; set; }
        public string ExternalAccountingBusinessArea { get; set; }
        public string SATPaymentMethodCode { get; set; }
        public string ExternalId2 { get; set; }

        public string SATForeignRFC { get; set; }
        public string MetodoPagoCode { get; set; }
        public string UsoCFDICode { get; set; }
        public string RegimenFiscalCode { get; set; }
        public string FirmCode { get; set; }        
        public string StateName { get; set; }
        public bool IsInternationalPartner { get; set; }
        public bool IsAutonomy { get; set; }
        public string CalculatedLocalName { get; set; }
        public string CalculatedEnglishName { get; set; }
        public string CreatedByPartner { get; set; }
        public decimal OpenShipments { get; set; }
        public string BusinessPhone { get; set; }


        public string CollectorId { get; set; }


        public int? StorageFreeDays { get; set; }
        public string RankId { get; set; }
        public string IndustryId { get; set; }
        public bool AccountingVATSplit { get; set; }

        public string WarehouseTypeCode { get; set; }
        public bool ChargeStorage { get; set; }
        public string ChargeStorageCurrencyId { get; set; }
        public string AirWeightMeasurementCode { get; set; }
        public string OceanWeightMeasurementCode { get; set; }
        public string InlandWeightMeasurementCode { get; set; }
        public string AirWeightRoundingCode { get; set; }
        public string OceanWeightRoundingCode { get; set; }
        public string InlandWeightRoundingCode { get; set; }

        public int SearchWeight { get; set; }

        public DateTime? RecordDate { get; set; }
        public string BillToId { get; set; }
        public double? AccountingPartnerCreditLimit { get; set; }


    }
}