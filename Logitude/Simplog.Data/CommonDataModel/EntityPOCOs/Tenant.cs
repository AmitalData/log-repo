using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ServiceModel.DomainServices;
using System.ServiceModel.DomainServices.Server;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class Tenant
    {
        [Key]
        public int Id { get; set; }
        public string Company { get; set; }
        public string CurrencyId { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string AddressId { get; set; }
        public string LocalAddressId { get; set; }
        public string Format { get; set; }
        public string Language { get; set; }
        public string Direction { get; set; }
        public string IATA { get; set; }
        public string Signature { get; set; }
        public string CASSCode { get; set; }
        public string DimensionsUnitCode { get; set; }
        public string VolumeUnitCode { get; set; }
        public string GrossWeightUnitCode { get; set; }
        [ForeignKey("WeightUnits")]
        public virtual WeightUnit WeightUnit { get; set; }
        public string ChargeableWeightUnitCode { get; set; }
        public string ExportFreightPrepaidCollectId { get; set; }
        public string ExportOtherPrepaidCollectId { get; set; }
        public string ImportFreightPrepaidCollectId { get; set; }
        public string ImportOtherPrepaidCollectId { get; set; }
        public string MasterExportFreightPrepaidCollectId { get; set; }
        public string MasterExportOtherPrepaidCollectId { get; set; }
        public string MasterImportFreightPrepaidCollectId { get; set; }
        public string MasterImportOtherPrepaidCollectId { get; set; }
        public bool IsHybrid { get; set; }
        public bool IsIncrementalBuildRunning { get; set; }
        public bool IsQuoteSubjectEdited { get; set; }
        public string DefaultQuestionnaireId { get; set; }
        public string FreightCurrencyId { get; set; }
        public string OtherChargesCurrencyId { get; set; }
        public string VatNumber { get; set; }
        public double? TimeZoneOffset { get; set; }
        public int DayLightOffset { get; set; }
        public DateTime? DayLightStartDate { get; set; }
        public DateTime? DayLightEndDate { get; set; }
        public string QuoteSaleCurrencyId { get; set; }
        public string PasswordPolicyCode { get; set; }
        public string PaymentTermId { get; set; }
        public string ProfitCurrencyId { get; set; }
        public string AgentId { get; set; }
        public string SearchFields { get; set; }
        public bool IsDataBackupBuilt { get; set; }
        public string WeightMeasurementUnitCode { get; set; }
        public string DateTimeFormat { get; set; }
        public string LocalCustomsCode { get; set; }
        public string VatUniqueTypeCode { get; set; }
        public string VatUniqueCountryId { get; set; }
        public string VatMandatoryCountryId { get; set; }
        public string VatMandatoryTypeCode { get; set; }
        public bool VatMandatoryForPotentialCustomers { get; set; }
        public bool AllowAgentInCustomersLOV { get; set; }
        public string VatUniquePartnerTypeCode { get; set; }

        public virtual VatUniqueType VatUniqueType { get; set; }
        public virtual Country VatUniqueCountry { get; set; }
        public virtual VatMandatoryType VatMandatoryType { get; set; }
        public virtual Country VatMandatoryCountry { get; set; }
        public virtual PasswordPolicy PasswordPolicy { get; set; }
        public virtual Card AgentCard { get; set; }
        public virtual PaymentTerm PaymentTerm { get; set; }
        public virtual Currency QuoteSaleCurrency { get; set; }
        public virtual PrepaidCollect ExportFreightPrepaidCollect { get; set; }
        public virtual PrepaidCollect ExportOtherPrepaidCollect { get; set; }
        public virtual PrepaidCollect ImportFreightPrepaidCollect { get; set; }
        public virtual PrepaidCollect ImportOtherPrepaidCollect { get; set; }
        public virtual PrepaidCollect MasterExportFreightPrepaidCollect { get; set; }
        public virtual PrepaidCollect MasterExportOtherPrepaidCollect { get; set; }
        public virtual PrepaidCollect MasterImportFreightPrepaidCollect { get; set; }
        public virtual PrepaidCollect MasterImportOtherPrepaidCollect { get; set; }
        public virtual Currency FreightCurrency { get; set; }
        public virtual Currency OtherChargesCurrency { get; set; }
        public virtual VolumeUnit VolumeUnit { get; set; }
        public virtual WeightUnit GrossWeightUnit { get; set; }
        public virtual WeightUnit ChargeableWeightUnit { get; set; }
        public virtual DimensionsUnit DimensionsUnit { get; set; }
        public virtual Currency ProfitCurrency { get; set; }
        public virtual VatUniquePartnerType VatUniquePartnerType { get; set; }

        public string InvoiceSection1 { get; set; }
        public string InvoiceSection2 { get; set; }
        public string BankDetails { get; set; }
        public bool IsSharedLogisticsActivated { get; set; }
        public bool IsWebAccessActivated { get; set; }

        public bool IsMobileActivated { get; set; }
        public bool SharedLogisticsMessageLink { get; set; }
        public bool IsCustomerTelRequired { get; set; }
        public bool IsCustomerFaxRequired { get; set; }
        public bool IsPickDelAdrsRequired { get; set; }
        public bool IsCustomerAddress1Required { get; set; }
        public bool HasPrimaryContact { get; set; }
        public bool AllowEAWBMoreThanTenPackages { get; set; }
        public bool SharedLogisMasterMessageLink { get; set; }

        public bool ExportQuotationsToIntegratedSystem { get; set; }

        public virtual AccountingSetting AccountingSetting { get; set; }

        [Include]
        [Association("TenantCurrency", "CurrencyId", "Id", IsForeignKey = true)]
        public virtual Currency Currency { get; set; }

        [ExternalReference]
        [Association("TenantAddress", "AddressId", "Id", IsForeignKey = true)]
        public virtual Address Address { get; set; }

        [ExternalReference]
        [Association("TenantLocalAddress", "LocalAddressId", "Id", IsForeignKey = true)]
        public virtual Address LocalAddress { get; set; }

        public string RegulatedAgentNumber { get; set; }
        public bool RegulatedAgentRegimeActivated { get; set; }
        public string CustomerId { get; set; }
        public virtual Card CustomerCard { get; set; }
        public bool IsCustomerTenantShare { get; set; }
        public bool CustomerTenantShareExportFile { get; set; }


        public bool IsPotentialTelRequired { get; set; }
        public bool IsPotentialFaxRequired { get; set; }
        public string VatFormatTypeCode { get; set; }
        public string VatFormatCountryId { get; set; }
        public bool IsNumeric { get; set; }
        public int? VatSize { get; set; }

        public virtual VatFormatType VatFormatType { get; set; }
        public virtual Country VatFormatCountry { get; set; }


        public bool IsCorrespondenceRightToLeftEnabled { get; set; }
        public bool IsNotesRightToLeftEnabled { get; set; }

        public DateTime? AccountingActivationDate { get; set; }
        public bool AccountingActivated { get; set; }

        public bool IsInternalTicketByDefault { get; set; }
        public bool IsFullTextSearchEnabled { get; set; }
        public bool ProrateMasterReceivables { get; set; }
        public string SCACCode { get; set; }
        public string FMCNumber { get; set; }
        public bool TenantVATManagement { get; set; }

        public string StorageEncryptionKey { get; set; }
        public string LayoutDirection { get; set; }

        [ForeignKey("TemperatureUnitCode")]
        public virtual TemperatureUnit TemperatureUnit { get; set; }
        public string TemperatureUnitCode { get; set; }

        public string DefaultSLAId { get; set; }
        public int TenantEmailSendingQuota { get; set; }

        public string NumberFormatCode { get; set; }

        [ForeignKey("NumberFormatCode")]
        public virtual NumberFormat NumberFormat { get; set; }

        public string EcommerceSupportEmail { get; set; }

        public string CBSA { get; set; }
        public string CAAT { get; set; }
        public bool IsTestTenant { get; set; }

        [ForeignKey("CheckDigitControlAlgorithmCode")]
        public virtual CheckDigitControlAlgorithm CheckDigitControlAlgorithm { get; set; }
        public string CheckDigitControlAlgorithmCode { get; set; }

        public bool ApplyVATForAllPartners { get; set; }
        public LogBoxTenantSetting LogBoxTenantSetting { get; set; }
        public bool HideFCLAllIn { get; set; }
        public bool AllowCustomersInAgentsLOV { get; set; }
        public DateTime? AutomaticLastUpdateDate { get; set; }
        public bool DisplayDocumentsAndEvents { get; set; }
        public string TransferQuotationsToUnifreightTrigger { get; set; }

        public double? AirRatio { get; set; }
        public double? LCLRatio { get; set; }
        public double? FCLRatio { get; set; }
        public double? LTLRatio { get; set; }
        public double? FTLRatio { get; set; }
    }
}