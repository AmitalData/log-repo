using System;
using System.Collections.Generic;

namespace Logitude.FullAccounting.Test.Models
{
    public class CardPM
    {
        public string Id { get; set; }

        public string EnglishName { get; set; }

        public string Code { get; set; }

        public string PartnerCode { get; set; }

        public string BankName { get; set; }
        public string BankAddress { get; set; }
        public string Swift { get; set; }
        public string AccountNumber { get; set; }
        public string IBANNumber { get; set; }
        public int Tenant { get; set; }
        public string GLAccountDisplayNumber { get; set; }
        public double? CreditLimitAmount { get; set; }
        public string Phone { get; set; }
        public string VatNumber { get; set; }

        public string LocalName { get; set; }
        public string ComputedLocalName { get; set; }

        public bool InActive { get; set; }

        public string PaymentTermId { get; set; }

        public string PartnerTypeId { get; set; }

        public string ReceivablesAccountingCard { get; set; }

        public string PayablesAccountingCard { get; set; }

        public string Notes { get; set; }

        public DateTime? CreateDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public string CreatedByUserId { get; set; }

        public string UpdatedByUserId { get; set; }

        public string SalesmanUserId { get; set; }
        public string AccountManagerUserId { get; set; }
        public string SalesmanBusinessUnitId { get; set; }

        public string PartnerTypeName { get; set; }
        public string MainAddressId { get; set; }
        public string BillingAddressId { get; set; }
        public string Website { get; set; }
        public string InvoiceCurrencyId { get; set; }
        public string VatTypeId { get; set; }

        public string Prefix { get; set; }

        public string CityName { get; set; }
        public string ImageDetailId { get; set; }
        public bool DisconectFromContact { get; set; }
        public bool InternetAccess { get; set; }
        public string PrimaryContactId { get; set; }
        public bool IsCustomer { get; set; }
        public bool EnableConsolidationInvoices { get; set; }
        public string IRSPlace { get; set; }
        public string IRSNumber { get; set; }
        public string StateName { get; set; }
        public string RankId { get; set; }
        public string IndustryId { get; set; }


        public string CountryId { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public string SearchFields { get; set; }
        public bool FieldsChanged { get; set; }
        public string ContactId { get; set; }

        public string SATForeignRFC { get; set; }


        public DateTime? InvitationDate { get; set; }
        public int? SharedLogisticsInvitationStatusCode { get; set; }
        public string SharedLogisticsInvitationStatusName { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public string CollectorId { get; set; }
        public string ClassifierId { get; set; }
        public string CollectorName { get; set; }
        public string ClassifierName { get; set; }
        public bool IsActiveForMobile { get; set; }

        public string SupportNotes { get; set; }

        public string GLAccountId { get; set; }

        public string ExternalAccountingBusinessArea { get; set; }

        public string SATPaymentMethodCode { get; set; }


        public string ExternalId2 { get; set; }

        public string MetodoPagoCode { get; set; }


        public string UsoCFDICode { get; set; }
        public bool IsInternationalPartner { get; set; }
        public bool IsAutonomy { get; set; }

        public string CustomerStatusCode { get; set; }



        public string CalculatedLocalName { get; set; }

        public string CalculatedEnglishName { get; set; }

        public bool IsDisconnectedFromGLAccount { get; set; }
        public string CreatedByPartner { get; set; }
        public int? StorageFreeDays { get; set; }

        public bool AccountingVATSplit { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }


        public string BillToId { get; set; }
        public string ICAO { get; set; }

        public string EmailForSendingSingArinvoice { get; set; }

        public string BankCodeId { get; set; }
        public string BankBranch { get; set; }

    }
}
