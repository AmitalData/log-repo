using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simplog.Global.Data.GlobalModel.EntityPOCOs
{
    public class TenantManagement
    {
        public TenantManagement()
        {
            IsDistributorSupportEnabled = true;
            IsSystemSupportEnabled = true;
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string PackageCode { get; set; }
        public string PackageName { get; set; }
        public DateTime? TrialStartDate { get; set; }
        public DateTime? TrialEndDate { get; set; }
        public DateTime? FirstPaymentDate { get; set; }      
        public DateTime? PaidUntilDate { get; set; }
        public bool IsTrial { get; set; }
        public int? NumberOfUsers { get; set; }
        public string SearchFields { get; set; }
        public int? FreeUsers { get; set; }
        public bool IsRecurring { get; set; }
        public string RecurringPeriodCode { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }        
        public string BluesnapAccount { get; set; }
        public string MainContract { get; set; }
        public string TemporalPackageCode { get; set; }
        public DateTime? TemporalStartDate { get; set; }
        public DateTime? TemporalEndDate { get; set; }
        public string PaymentMethodCode { get; set; }
        public string PaymentChannelCode { get; set; }
        public double? LicensePrice { get; set; }
        public double? TotalPrice { get; set; }
        public string Notes { get; set; }
        public string TTY { get; set; }
        public DateTime? LastFWBSentDate { get; set; }
        public DateTime? LastFHLSentDate { get; set; }
        public DateTime? StatisticsUpdateDate { get; set; }
        public DateTime? ShipmentLastDate { get; set; }
        public int ShipmentTotalLastWeek { get; set; }
        public int ShipmentTotalLastMonth { get; set; }
        public DateTime? QuoteLastDate { get; set; }
        public int QuoteTotalLastWeek { get; set; }
        public int QuoteTotalLastMonth { get; set; }
        public DateTime? ARInvoiceLastDate { get; set; }
        public int ARInvoiceTotalLastWeek { get; set; }
        public int ARInvoiceTotalLastMonth { get; set; }
        public DateTime? APInvoiceLastDate { get; set; }
        public int APInvoiceTotalLastWeek { get; set; }
        public int APInvoiceTotalLastMonth { get; set; }
        public DateTime? CustomerLastDate { get; set; }
        public int CustomerTotalLastWeek { get; set; }
        public int CustomerTotalLastMonth { get; set; }
        public string CountryName { get; set; }
        public DateTime? LastLoginDateTime { get; set; }
        public string PaymentCurrencyCode { get; set; }        
        public bool IsSystemSupportEnabled { get; set; }
        public bool IsDistributorSupportEnabled { get; set; }
        public string DistributorCode { get; set; }
        public bool IsAWBStockPrepaid { get; set; }
        public bool ManageLicencesPerUser { get; set; }
        public bool IsCargonautEnabled { get; set; }
        public bool IsDEXXConnectionEnabled { get; set; }
        public DateTime? LastFWBCargonautSentDate { get; set; }
        public DateTime? LastFHLCargonautSentDate { get; set; }
        public bool PaymentFailure { get; set; }
        public DateTime? SuspendDate  { get; set; }
        public string InternalNotes { get; set; }
        public string BluesnapContractId { get; set; }
        public string BluesnapCRMContractId { get; set; }
        public string BluesnapEAWBContractId { get; set; }
        public string BluesnapEAWBSContractId { get; set; }
        public string BluesnapOneTimeContract { get; set; }
        public string BluesnapInttraStockContractId { get; set; }


        public int BluesnapContractQTY { get; set; }
        public int BluesnapCRMContractQTY { get; set; }
        public int BluesnapEAWBContractQTY { get; set; }
        public int BluesnapEAWBSContractQTY { get; set; }
        public int BluesnapOneTimeContractQTY { get; set; }
        public int BluesnapInttraStockContractQTY { get; set; }
        

        public string AWBMessagesCCSTypeCode { get; set; }
        public string PIMA { get; set; }
        public bool IsEAWBOnlyDemo { get; set; }
        public bool IsRestrictedByAirline { get; set; }
        public DateTime? LastFFRSentDate { get; set; }
        public bool ManagesRegisteredAgent  { get; set; }
        public DateTime? ActivityLastDate { get; set; }
        public int ActivityTotalLastWeek { get; set; }
        public int ActivityTotalLastMonth { get; set; }
        public DateTime? OpportunityLastDate { get; set; }
        public int OpportunityTotalLastWeek { get; set; }
        public int OpportunityTotalLastMonth { get; set; }
        public DateTime? FSULastReceivedDate { get; set; }
        public DateTime? FSALastReceivedDate { get; set; }
        public DateTime? FSRLastSentDate { get; set; }
        public bool BillingByLogitude { get; set; }
        public int? ResellerCommission { get; set; }
        public string TenantTypeCode { get; set; }
        public string TenantConnectedToAirlineCode { get; set; }
        public string SignupRequestRecipients { get; set; }
        public string LoginPageNotes { get; set; }
        public string SupportEmail { get; set; }
        public bool SupportActivated  { get; set; }
        public bool IsMultiPackage { get; set; }
        public string OldTTY { get; set; }
        public string RequestedAirlines { get; set; }
        public string RegisteredAirlines { get; set; }
        public string PendingAirlines { get; set; }
        public bool EnableBranding { get; set; }
        public string CustomerURL { get; set; }
        public bool HideSharedlogistics { get; set; }
        public string ContactEmail { get; set; }        
        public string Technology { get; set; }
        public virtual GlobalTenant GlobalTenant { get; set; }
        public DateTime? ShardLogisticLastDate { get; set; }
        public int ShardLogisticTotalLastWeek { get; set; }
        public int ShardLogisticTotalLastMonth { get; set; }
        public DateTime? MobileLastDate { get; set; }
        public int MobileTotalLastWeek { get; set; }
        public int MobileTotalLastMonth { get; set; }
        public DateTime? SilverlightEndDate { get; set; }

        public bool IsParentTenant { get; set; }
        public int? ParentTenantId { get; set; }
        public bool ChangeHeaderColor { get; set; }

        public DateTime? AgentSharedLogisticsStatisticsLastDate { get; set; }
        public int AgentSharedLogisticsStatisticsLastWeek { get; set; }
        public int AgentSharedLogisticsStatisticsLastMonth { get; set; }


        [ForeignKey("RecurringPeriodCode")]
        public virtual RecurringPeriod RecurringPeriod { get; set; }

        [ForeignKey("PaymentMethodCode")]
        public virtual PaymentMethod PaymentMethod { get; set; }

        [ForeignKey("PaymentChannelCode")]
        public virtual PaymentChannel PaymentChannel { get; set; }

        [ForeignKey("PaymentCurrencyCode")]
        public virtual PaymentCurrency PaymentCurrency { get; set; }

        [ForeignKey("BluesnapContractId")]
        public virtual BluesnapContract BluesnapContract { get; set; }

        [ForeignKey("BluesnapCRMContractId")]
        public virtual BluesnapContract BluesnapContractCRM { get; set; }

        [ForeignKey("BluesnapEAWBContractId")]
        public virtual BluesnapContract BluesnapContractEAWB { get; set; }

        [ForeignKey("BluesnapEAWBSContractId")]
        public virtual BluesnapContract BluesnapContractEAWBS { get; set; }

        [ForeignKey("BluesnapInttraStockContractId")]
        public virtual BluesnapContract BluesnapInttraStockContract { get; set; }
        

        [ForeignKey("AWBMessagesCCSTypeCode")]
        public virtual AWBMessagesCCSType AWBMessagesCCSType { get; set; }

        [ForeignKey("TenantTypeCode")]
        public virtual TenantType TenantType { get; set; }

        public string StockTypeCode { get; set; } //A - Agent Stock, C - Customer Stock

        public bool IsINTTRAStockPrepaid { get; set; }

        public string PackageCodeSearchField { get; set; }

        public bool IsINTTRAOnlyDemo { get; set; }

        public bool MainAdditionalPackageApplied { get; set; }
        public string SupportDomain { get; set; }
        public string MainColor { get; set; }
        public string SecondaryColor { get; set; }
        public int? TotalNumberOfUsers { get; set; }
        public int? TotalFreeUsers { get; set; }
        public double? AveragePrice { get; set; }
        public double? TotalPaymentamount { get; set; }
     
        public string BackgroundId { get; set; }
        public string ComapnylogoId { get; set; }
        public string InvertedLogoId { get; set; }
        public string BrowserIconId { get; set; }
        public string ShipmentHeaderImageId { get; set; }

        public bool NoPaymentForChildTenants { get; set; }

        public DateTime? LastEbookingSentDate { get; set; }
        public DateTime? LastSISentDate { get; set; }
        public int NumberOfBookingSentLastWeek { get; set; }
        public int NumberOfSISentLastWeek { get; set; }
        public DateTime? LastContainerStatusReceived { get; set; }
        public DateTime? LastTariffUpdateDate { get; set; }
        public DateTime? LastTariffUsageDate { get; set; }
        public int LastWeekCreatedTariffs { get; set; }
        public int LastMonthCreatedTariffs { get; set; }
        public int ScheduledTasksLimitPerReport { get; set; }
        public string AmitalApiToken { get; set; }
    }
}