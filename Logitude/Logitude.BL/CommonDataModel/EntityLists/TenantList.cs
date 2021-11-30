using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class TenantList
    {
        [Key]
        public int Id { get; set; }
        public string AddressId { get; set; }
        public string CompanyAddress { get; set; }
        public string Company { get; set; }
        public string Signature { get; set; }
        public string IATA { get; set; }
        public string VatNumber { get; set; }
        public string SearchFields { get; set; }
        public bool IsHybrid { get; set; }
        public string PaymentTermId { get; set; }
        public string PaymentTermName { get; set; }
        public string AgentId { get; set; }
        public string AgentName { get; set; }
        public string CASSCode { get; set; }
        public string DefaultQuestionnaireId { get; set; }
        public string CurrencyId { get; set; }
        public string AccountingCurrencyCode { get; set; }
        public string ProfitCurrencyId { get; set; }
        public string ProfitCurrencyCode { get; set; }
        public string FreightCurrencyId { get; set; }
        public string FreightCurrencyCode { get; set; }
        public string OtherChargesCurrencyId { get; set; }
        public string OtherChargesCurrencyCode { get; set; }
        public string QuoteSaleCurrencyId { get; set; }
        public string QuoteSaleCurrencyCode { get; set; }
        public string VolumeUnitCode { get; set; }
        public string DimensionsUnitCode { get; set; }
        public string GrossWeightUnitCode { get; set; }
        public string ChargeableWeightUnitCode { get; set; }
        public string WeightMeasurementUnitCode { get; set; }
        public string ExportFreightPrepaidCollectId { get; set; }
        public string ImportFreightPrepaidCollectId { get; set; }
        public string ExportOtherPrepaidCollectId { get; set; }
        public string ImportOtherPrepaidCollectId { get; set; }
        public string MasterExportFreightPrepaidCollectId { get; set; }
        public string MasterImportFreightPrepaidCollectId { get; set; }
        public string MasterExportOtherPrepaidCollectId { get; set; }
        public string MasterImportOtherPrepaidCollectId { get; set; }
        public double? STDVatPercentage { get; set; }
        public double? TimeZoneOffset { get; set; }
        public string Language { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string Format { get; set; }
        public string Direction { get; set; }
        public int DayLightOffset { get; set; }
        public DateTime? DayLightStartDate { get; set; }
        public DateTime? DayLightEndDate { get; set; }
        public string PasswordPolicyCode { get; set; }
        public string PasswordStrength { get; set; }
        public bool IsDataBackupBuilt { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string PackageCode { get; set; }
        public string CurrencyCode { get; set; }
        public string InvoiceSection1 { get; set; }
        public string InvoiceSection2 { get; set; }
        public string BankDetails { get; set; }
        public bool IsSharedLogisticsActivated { get; set; }
        public bool IsMobileActivated { get; set; }
        public bool IsWebAccessActivated { get; set; }
        public bool SharedLogisticsMessageLink { get; set; }
        public string LocalCustomsCode { get; set; }
        public string VatUniqueTypeCode { get; set; }
        public string VatMandatoryTypeCode { get; set; }
        public string VatUniqueCountryId { get; set; }
        public string VatMandatoryCountryId { get; set; }
        public bool VatMandatoryForPotentialCustomers { get; set; }
        public bool IsQuoteSubjectEdited { get; set; }
        public bool AllowEAWBMoreThanTenPackages { get; set; }
        public string RegulatedAgentNumber { get; set; }
        public bool RegulatedAgentRegimeActivated { get; set; }
        public bool IsDocumentsArchive { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public bool CustomerTenantShareCustomsFile { get; set; }
        public bool CustomerTenantShareImportFile { get; set; }
        public bool CustomerTenantShareExportFile { get; set; }
        public bool AllowAgentInCustomersLOV { get; set; }
        public bool IsCorrespondenceRightToLeftEnabled { get; set; }
        public bool IsNotesRightToLeftEnabled { get; set; }
        public DateTime? AccountingActivationDate { get; set; }
        public bool AccountingActivated { get; set; }
        public bool IsInternalTicketByDefault { get; set; }
        public bool ProrateMasterReceivables { get; set; }
        public bool IsIncrementalBuildRunning { get; set; }

        public string SCACCode { get; set; }
        public bool ExportQuotationsToIntegratedSystem { get; set; }
        //public string DropBoxAccessToken { get; set; }
        public bool TenantVATManagement { get; set; }
        public string StorageEncryptionKey { get; set; }
        public string TemperatureUnitCode { get; set; }
        public string DefaultSLAId { get; set; }
        public string NumberFormatCode { get; set; }

        public string CBSA { get; set; }
        public string CAAT { get; set; }
        public bool IsTestTenant { get; set; }
        public string CheckDigitControlAlgorithmCode { get; set; }
        public bool HideFCLAllIn { get; set; }
        public bool AllowCustomersInAgentsLOV { get; set; }
        public string VatUniquePartnerTypeCode { get; set; }
        public bool SharedLogisMasterMessageLink { get; set; }
        public bool IsQuotesRequestActivatedInShared { get; set; }
        public bool IsCargoTrackWebAccessActivated { get; set; }
        public int? AutomaticallyCloseDays { get; set; }


    }
}