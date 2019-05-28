using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Simplog.Server.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
    [CustomValidation(typeof(Validators.ClassLevelValidator), "ValidateClass")]
    public class TenantPM
    {
        [Key]
        public int Id { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string AddressId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string LocalAddressId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CompanyAddress { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Company { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Signature { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string IATA { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string VatNumber { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string SearchFields { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string PaymentTermId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string PaymentTermName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool IsHybrid { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool IsQuoteSubjectEdited { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string AgentId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string AgentName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string DefaultQuestionnaireId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CASSCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CurrencyId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string AccountingCurrencyCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string AccountingCurrencyName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ProfitCurrencyId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ProfitCurrencyCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? ProfitCurrencyRate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string FreightCurrencyId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string FreightCurrencyCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string OtherChargesCurrencyId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string OtherChargesCurrencyCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string QuoteSaleCurrencyId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string QuoteSaleCurrencyCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string VolumeUnitCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string DimensionsUnitCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string GrossWeightUnitCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ChargeableWeightUnitCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string WeightMeasurementUnitCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ExportFreightPrepaidCollectId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ImportFreightPrepaidCollectId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ExportOtherPrepaidCollectId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ImportOtherPrepaidCollectId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string MasterExportFreightPrepaidCollectId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string MasterImportFreightPrepaidCollectId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string MasterExportOtherPrepaidCollectId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string MasterImportOtherPrepaidCollectId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? STDVatPercentage { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? TimeZoneOffset { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Language { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string DateTimeFormat { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Email { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Website { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Format { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Direction { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public int DayLightOffset { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? DayLightStartDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? DayLightEndDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string PasswordPolicyCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string PasswordStrength { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool IsDataBackupBuilt { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CountryCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CountryName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string PackageCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string TemporalPackageCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? TemporalStartDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? TemporalEndDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CurrencyCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string InvoiceSection1 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string InvoiceSection2 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string BankDetails { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool IsSharedLogisticsActivated { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool IsWebAccessActivated { get; set; }
        
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool IsMobileActivated { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool SharedLogisticsMessageLink { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string LocalCustomsCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string VatUniqueTypeCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string VatMandatoryTypeCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string VatUniqueCountryId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string VatMandatoryCountryId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool VatMandatoryForPotentialCustomers { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool IsCustomerTelRequired { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool IsCustomerFaxRequired { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool IsPickDelAdrsRequired { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool IsCustomerAddress1Required { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool HasPrimaryContact { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool AllowEAWBMoreThanTenPackages { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string RegulatedAgentNumber { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool RegulatedAgentRegimeActivated { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CustomerId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CustomerName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CustomerMobile { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CustomerPhone { get; set; }

        public bool IsDocumentsArchive { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool IsCustomerTenantShare { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool CustomerTenantShareImportFile { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool CustomerTenantShareExportFile { get; set; }


        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool AllowAgentInCustomersLOV { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool IsPotentialTelRequired { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool IsPotentialFaxRequired { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string VatFormatTypeCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string VatFormatCountryId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool IsNumeric { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public int? VatSize { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string LogBoxAdminUserId { get; set; }

        public bool CreateTenantFromSignUp { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool IsCorrespondenceRightToLeftEnabled { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool IsNotesRightToLeftEnabled { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? AccountingActivationDate { get; set; }

        public bool AccountingActivated { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CurrencySign { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string PrivateLabelId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool IsInternalTicketByDefault { get; set; }

        public bool ProrateMasterReceivables { get; set; }

        public bool ExportQuotationsToIntegratedSystem { get; set; }


        //[CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        //public string DropBoxAccessToken { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string SCACCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string FMCNumber { get; set; }

        public bool TenantVATManagement { get; set; }

        public bool DocumentShareAsDefault { get; set; }
        public string StorageEncryptionKey { get; set; }
        public string LayoutDirection { get; set; }


        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string TemperatureUnitCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string NumberFormatCode { get; set; }


        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string DefaultSLAId { get; set; }
        public string StockTypeCode { get; set; }

        public bool AutoArchiveOnInvoice { get; set; }
        public string EcommerceSupportEmail { get; set; }

        public string CBSA { get; set; }
        public string CAAT { get; set; }


    }
}
