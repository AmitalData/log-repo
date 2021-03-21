using System;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Logitude.BL.GlobalModel.EntityPMs
{
    [CustomValidation(typeof(Validators.ClassLevelValidator), "ValidateClass")]
    public class TenantManagementPM
    {
        [Key]
        public int Id { get; set; }
         
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Name { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? LastLoginDateTime { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string PackageCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string PackageName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? TrialStartDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? TrialEndDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? FirstPaymentDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? PaidUntilDate { get; set; }

        public string SearchFields { get; set; }
        public bool IsAWBStockPrepaid { get; set; }
        public bool IsActive { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]        
        public string TimeZone { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string GlobalDBId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string TTY { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public int? FreeUsers { get; set; }
        public bool IsRecurring { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string RecurringPeriodCode { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool PaymentFailure { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? SuspendDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string InternalNotes { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string BluesnapAccount { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string MainContract { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string TemporalPackageCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string TemporalPackageName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? TemporalStartDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? TemporalEndDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string PaymentMethodCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string PaymentChannelCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? LicensePrice { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Notes { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CountryName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? LastFWBSentDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? LastFHLSentDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? StatisticsUpdateDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? ShipmentLastDate { get; set; }

        public int ShipmentTotalLastWeek { get; set; }
        public int ShipmentTotalLastMonth { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? QuoteLastDate { get; set; }
        public int QuoteTotalLastWeek { get; set; }
        public int QuoteTotalLastMonth { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? ARInvoiceLastDate { get; set; }
        public int ARInvoiceTotalLastWeek { get; set; }
        public int ARInvoiceTotalLastMonth { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? APInvoiceLastDate { get; set; }
        public int APInvoiceTotalLastWeek { get; set; }
        public int APInvoiceTotalLastMonth { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? CustomerLastDate { get; set; }
        public int CustomerTotalLastWeek { get; set; }
        public int CustomerTotalLastMonth { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? MobileLastDate { get; set; }
        public int MobileTotalLastWeek { get; set; }
        public int MobileTotalLastMonth { get; set; }
        
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? ShardLogisticLastDate { get; set; }
        public int ShardLogisticTotalLastWeek { get; set; }
        public int ShardLogisticTotalLastMonth { get; set; }
        
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string PaymentCurrencyCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string DistributorCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string BluesnapContractId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string BluesnapCRMContractId { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string BluesnapEAWBContractId { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string BluesnapEAWBSContractId { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string BluesnapOneTimeContract { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string BluesnapInttraStockContractId { get; set; }




        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string AWBMessagesCCSTypeCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string PIMA { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? ActivityLastDate { get; set; }
        public int ActivityTotalLastWeek { get; set; }
        public int ActivityTotalLastMonth { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? OpportunityLastDate { get; set; }
        public int OpportunityTotalLastWeek { get; set; }
        public int OpportunityTotalLastMonth { get; set; }

        public bool ManageLicencesPerUser { get; set; }
        public bool IsSystemSupportEnabled { get; set; }
        public bool IsDistributorSupportEnabled { get; set; }
        public bool DoBlocking { get; set; }
        public int TrailDaysLeft { get; set; }
        public int PaidDaysLeft { get; set; }
        public bool IsTrial { get; set; }
        public int? NumberOfUsers { get; set; }
        public bool IsCargonautEnabled { get; set; }
        public int BluesnapContractQTY { get; set; }
        public int BluesnapCRMContractQTY { get; set; }
        public int BluesnapEAWBContractQTY { get; set; }
        public int BluesnapEAWBSContractQTY { get; set; }
        public int BluesnapOneTimeContractQTY { get; set; }
        public int BluesnapInttraStockContractQTY { get; set; }
        public bool IsDEXXConnectionEnabled { get; set; }
        public DateTime? LastFWBCargonautSentDate { get; set; }
        public DateTime? LastFHLCargonautSentDate { get; set; }
        public int SuspendDaysLeft { get; set; }
        public bool IsEAWBOnlyDemo { get; set; }
        public bool IsRestrictedByAirline { get; set; }
        public DateTime? LastFFRSentDate { get; set; }
        public bool ManagesRegisteredAgent { get; set; }
        public DateTime? FSULastReceivedDate { get; set; }
        public DateTime? FSALastReceivedDate { get; set; }
        public DateTime? FSRLastSentDate { get; set; }
        public DateTime? SilverlightEndDate { get; set; }
        public bool BillingByLogitude { get; set; }
        public int? ResellerCommission { get; set; }
        public string TenantTypeCode { get; set; }
        public string TenantConnectedToAirlineCode { get; set; }
        public string SignupRequestRecipients { get; set; }
        public string LoginPageNotes { get; set; }
        public string SupportEmail { get; set; }
        public bool SupportActivated { get; set; }
        public bool IsMultiPackage { get; set; }
        public string Technology { get; set; }
        public string RequestedAirlines { get; set; }
        public string RegisteredAirlines { get; set; }
        public string PendingAirlines { get; set; }

        public bool EnableBranding { get; set; }
        public string CustomerURL { get; set; }
        public bool HideSharedlogistics { get; set; }
        public string ContactEmail { get; set; }
        public string UpdateByUserId { get; set; }

        public bool IsParentTenant { get; set; }
        public int? ParentTenantId { get; set; }



        public DateTime? AgentSharedLogisticsStatisticsLastDate { get; set; }
        public int AgentSharedLogisticsStatisticsLastWeek { get; set; }
        public int AgentSharedLogisticsStatisticsLastMonth { get; set; }
        public bool ChangeHeaderColor { get; set; }
        public bool DocumentShareAsDefault { get; set; }
        public string StockTypeCode { get; set; }
        public bool IsINTTRAStockPrepaid { get; set; }
        public bool IsINTTRAOnlyDemo { get; set; }
        public bool AutoArchiveOnInvoice { get; set; }
        
        public string PackageCodeSearchField { get; set; }
        public string MainColor { get; set; }
        public string SecondaryColor { get; set; }
        
        public string BackgroundId { get; set; }
        public string ComapnylogoId { get; set; }
        public string InvertedLogoId { get; set; }
        public string BrowserIconId { get; set; }
        public string ShipmentHeaderImageId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string PrivateLabelId { get; set; }

        private List<string> packagesCodes_PK;
        public List<string> PackagesCodes_PK
        {
            get
            {
                if (packagesCodes_PK == null)
                {
                    packagesCodes_PK = new List<string>();
                }

                return packagesCodes_PK;
            }

            set
            {
                packagesCodes_PK = value;
            }
        }

        private List<string> packagesCodes_BS;
        public List<string> PackagesCodes_BS
        {
            get
            {
                if (packagesCodes_BS == null)
                {
                    packagesCodes_BS = new List<string>();
                }

                return packagesCodes_BS;
            }

            set
            {
                packagesCodes_BS = value;
            }
        }

        [Include]
        [Association("GlobalTenantTenantManagement", "Id", "Id")]
        public virtual GlobalTenant GlobalTenant { get; set; }

        private List<TenantManagementLicensePM> tenantManagementLicenses;
        [Include]
        [Association("TenantManagementLicensePMTenantManagementPM", "Id", "Tenant")]
        [Composition]
        public virtual List<TenantManagementLicensePM> TenantManagementLicenses
        {
            get
            {
                if (tenantManagementLicenses == null)
                {
                    tenantManagementLicenses = new List<TenantManagementLicensePM>();
                }

                return tenantManagementLicenses;
            }

            set
            {
                tenantManagementLicenses = value;
            }
        }

        private List<TenantAddOnPM> addOns;
        [Include]
        [Association("TenantAddOnPMTenantManagementPM", "Id", "Tenant")]
        [Composition]
        public virtual List<TenantAddOnPM> AddOns
        {
            get
            {
                if (addOns == null)
                {
                    addOns = new List<TenantAddOnPM>();
                }

                return addOns;
            }

            set
            {
                addOns = value;
            }
        }

        public bool IsTestTenant { get; set; }

        public bool MainAdditionalPackageApplied { get; set; }
        public double? TotalPrice { get; set; }
        public string MainColorOpacity { get; set; }
        public string SupportDomain { get; set; }
        public string SecondaryColorOpacity { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public int? TotalNumberOfUsers { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public int? TotalFreeUsers { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? AveragePrice { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? TotalPaymentamount { get; set; }
        public bool NoPaymentForChildTenants { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? LastEbookingSentDate { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? LastSISentDate { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public int NumberOfBookingSentLastWeek { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public int NumberOfSISentLastWeek { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? LastContainerStatusReceived { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? LastTariffUpdateDate { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? LastTariffUsageDate { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public int LastWeekCreatedTariffs { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public int LastMonthCreatedTariffs { get; set; }

        public int ScheduledTasksLimitPerReport { get; set; }
    }
}
