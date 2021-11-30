using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;

namespace Simplog.Global.Data.GlobalModel.Mapping
{
    public class TenantManagementMap : EntityTypeConfiguration<TenantManagement>
    {
        public TenantManagementMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(t => t.Name).IsRequired().HasMaxLength(100).IsUnicode(true);
            this.Property(t => t.PackageCode).HasMaxLength(5).IsUnicode(false);
            this.Property(t => t.PackageName).HasMaxLength(40).IsUnicode(false);
            this.Property(t => t.SearchFields).HasMaxLength(1000).IsUnicode(true);
            this.Property(t => t.InternalNotes).HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.BluesnapAccount).HasMaxLength(25).IsUnicode(true);
            this.Property(t => t.MainContract).HasMaxLength(25).IsUnicode(true);
            this.Property(t => t.RecurringPeriodCode).HasMaxLength(2).IsUnicode(true);
            this.Property(t => t.TemporalPackageCode).HasMaxLength(5).IsUnicode(true);
            this.Property(t => t.PaymentMethodCode).HasMaxLength(2).IsUnicode(false);
            this.Property(t => t.PaymentChannelCode).HasMaxLength(2).IsUnicode(false);
            this.Property(t => t.Notes).HasMaxLength(1000).IsUnicode(true);
            this.Property(t => t.PaymentCurrencyCode).HasMaxLength(3).IsUnicode(false);
            this.Property(t => t.DistributorCode).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.BluesnapContractId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.BluesnapCRMContractId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.BluesnapEAWBContractId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.BluesnapEAWBSContractId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.BluesnapInttraStockContractId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.BluesnapOneTimeContract).HasMaxLength(40).IsUnicode(false);            
            this.Property(t => t.AWBMessagesCCSTypeCode).HasMaxLength(10).IsUnicode(false);
            this.Property(t => t.PIMA).HasMaxLength(25).IsUnicode(false);
            this.Property(t => t.TenantTypeCode).HasMaxLength(3).IsUnicode(false);
            this.Property(t => t.TenantConnectedToAirlineCode).HasMaxLength(2).IsUnicode(false);
            this.Property(t => t.SignupRequestRecipients).HasMaxLength(500).IsUnicode(false);
            this.Property(t => t.LoginPageNotes).HasMaxLength(1000).IsUnicode(true);
            this.Property(t => t.SupportEmail).HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.TTY).HasMaxLength(33).IsUnicode(false);
            this.Property(t => t.Technology).HasMaxLength(5).IsUnicode(false);
            this.Property(t => t.OldTTY).HasMaxLength(33).IsUnicode(false);
            this.Property(t => t.RequestedAirlines).HasMaxLength(1000).IsUnicode(false);
            this.Property(t => t.RegisteredAirlines).HasMaxLength(1000).IsUnicode(false);
            this.Property(t => t.PendingAirlines).HasMaxLength(1000).IsUnicode(false);
            this.Property(t => t.ContactEmail).HasMaxLength(70).IsUnicode(false);
            this.Property(t => t.CustomerURL).HasMaxLength(250).IsUnicode(false);
            this.Property(t => t.StockTypeCode).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.PackageCodeSearchField).HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.BluesnapContractQTY).IsOptional();
            this.Property(t => t.BluesnapCRMContractQTY).IsOptional();
            this.Property(t => t.BluesnapEAWBContractQTY).IsOptional();
            this.Property(t => t.BluesnapEAWBSContractQTY).IsOptional();
            this.Property(t => t.BluesnapOneTimeContractQTY).IsOptional();
            this.Property(t => t.BluesnapInttraStockContractQTY).IsOptional();
            this.Property(t => t.SupportDomain).HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.CountryName).HasMaxLength(120).IsUnicode(false); 

            this.ToTable("TenantManagements");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.PackageCode).HasColumnName("PackageCode");
            this.Property(t => t.PackageName).HasColumnName("PackageName");
            this.Property(t => t.TrialStartDate).HasColumnName("TrialStartDate");
            this.Property(t => t.TrialEndDate).HasColumnName("TrialEndDate");
            this.Property(t => t.FirstPaymentDate).HasColumnName("FirstPaymentDate");
            this.Property(t => t.PaidUntilDate).HasColumnName("PaidUntilDate");
            this.Property(t => t.IsTrial).HasColumnName("IsTrial");
            this.Property(t => t.NumberOfUsers).HasColumnName("NumberOfUsers");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
            this.Property(t => t.BluesnapContractQTY).HasColumnName("BluesnapContractQTY");
            this.Property(t => t.BluesnapCRMContractQTY).HasColumnName("BluesnapCRMContractQTY");
            this.Property(t => t.BluesnapEAWBContractQTY).HasColumnName("BluesnapEAWBContractQTY");
            this.Property(t => t.BluesnapEAWBSContractQTY).HasColumnName("BluesnapEAWBSContractQTY");
            this.Property(t => t.BluesnapOneTimeContractQTY).HasColumnName("BluesnapOneTimeContractQTY");
            this.Property(t => t.BluesnapInttraStockContractQTY).HasColumnName("BluesnapInttraStockContractQTY");            
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            this.Property(t => t.SuspendDate).HasColumnName("SuspendDate");
            this.Property(t => t.InternalNotes).HasColumnName("InternalNotes");
            this.Property(t => t.BluesnapAccount).HasColumnName("BluesnapAccount");
            this.Property(t => t.MainContract).HasColumnName("MainContract");
            this.Property(t => t.TemporalStartDate).HasColumnName("TemporalStartDate");
            this.Property(t => t.TemporalEndDate).HasColumnName("TemporalEndDate");
            this.Property(t => t.RecurringPeriodCode).HasColumnName("RecurringPeriodCode");
            this.Property(t => t.TemporalPackageCode).HasColumnName("TemporalPackageCode");
            this.Property(t => t.IsRecurring).HasColumnName("IsRecurring");
            this.Property(t => t.PaymentFailure).HasColumnName("PaymentFailure");
            this.Property(t => t.PaymentMethodCode).HasColumnName("PaymentMethodCode");
            this.Property(t => t.PaymentChannelCode).HasColumnName("PaymentChannelCode");
            this.Property(t => t.LicensePrice).HasColumnName("LicensePrice");
            this.Property(t => t.Notes).HasColumnName("Notes");
            this.Property(t => t.LastLoginDateTime).HasColumnName("LastLoginDateTime");
            this.Property(t => t.PaymentCurrencyCode).HasColumnName("PaymentCurrencyCode");
            this.Property(t => t.IsAWBStockPrepaid).HasColumnName("IsAWBStockPrepaid");
            this.Property(t => t.ManageLicencesPerUser).HasColumnName("ManageLicencesPerUser");
            this.Property(t => t.DistributorCode).HasColumnName("DistributorCode");
            this.Property(t => t.IsSystemSupportEnabled).HasColumnName("IsSystemSupportEnabled");
            this.Property(t => t.IsDistributorSupportEnabled).HasColumnName("IsDistributorSupportEnabled");
            this.Property(t => t.IsCargonautEnabled).HasColumnName("IsCargonautEnabled");
            this.Property(t => t.LastFWBCargonautSentDate).HasColumnName("LastFWBCargonautSentDate");
            this.Property(t => t.LastFHLCargonautSentDate).HasColumnName("LastFHLCargonautSentDate");
            this.Property(t => t.IsDEXXConnectionEnabled).HasColumnName("IsDEXXConnectionEnabled");
            this.Property(t => t.BluesnapContractId).HasColumnName("BluesnapContractId");
            this.Property(t => t.BluesnapCRMContractId).HasColumnName("BluesnapCRMContractId");
            this.Property(t => t.BluesnapEAWBContractId).HasColumnName("BluesnapEAWBContractId");
            this.Property(t => t.BluesnapEAWBSContractId).HasColumnName("BluesnapEAWBSContractId");
            this.Property(t => t.BluesnapInttraStockContractId).HasColumnName("BluesnapInttraStockContractId");            
            this.Property(t => t.BluesnapOneTimeContract).HasColumnName("BluesnapOneTimeContract");
            this.Property(t => t.AWBMessagesCCSTypeCode).HasColumnName("AWBMessagesCCSTypeCode");
            this.Property(t => t.PIMA).HasColumnName("PIMA");
            this.Property(t => t.IsEAWBOnlyDemo).HasColumnName("IsEAWBOnlyDemo");
            this.Property(t => t.IsRestrictedByAirline).HasColumnName("IsRestrictedByAirline");
            this.Property(t => t.LastFFRSentDate).HasColumnName("LastFFRSentDate");
            this.Property(t => t.ManagesRegisteredAgent).HasColumnName("ManagesRegisteredAgent");
            this.Property(t => t.ActivityLastDate).HasColumnName("ActivityLastDate");
            this.Property(t => t.ActivityTotalLastWeek).HasColumnName("ActivityTotalLastWeek");
            this.Property(t => t.ActivityTotalLastMonth).HasColumnName("ActivityTotalLastMonth");
            this.Property(t => t.OpportunityLastDate).HasColumnName("OpportunityLastDate");
            this.Property(t => t.OpportunityTotalLastWeek).HasColumnName("OpportunityTotalLastWeek");
            this.Property(t => t.OpportunityTotalLastMonth).HasColumnName("OpportunityTotalLastMonth");
            this.Property(t => t.FSULastReceivedDate).HasColumnName("FSULastReceivedDate");
            this.Property(t => t.FSALastReceivedDate).HasColumnName("FSALastReceivedDate");
            this.Property(t => t.FSRLastSentDate).HasColumnName("FSRLastSentDate");
            this.Property(t => t.BillingByLogitude).HasColumnName("BillingByLogitude");
            this.Property(t => t.ResellerCommission).HasColumnName("ResellerCommission");
            this.Property(t => t.TenantTypeCode).HasColumnName("TenantTypeCode");
            this.Property(t => t.TenantConnectedToAirlineCode).HasColumnName("TenantConnectedToAirlineCode");
            this.Property(t => t.SignupRequestRecipients).HasColumnName("SignupRequestRecipients");
            this.Property(t => t.LoginPageNotes).HasColumnName("LoginPageNotes");
            this.Property(t => t.SupportEmail).HasColumnName("SupportEmail");
            this.Property(t => t.SupportActivated).HasColumnName("SupportActivated");
            this.Property(t => t.TTY).HasColumnName("TTY");
            this.Property(t => t.IsMultiPackage).HasColumnName("IsMultiPackage");
            this.Property(t => t.Technology).HasColumnName("Technology");
            this.Property(t => t.ShardLogisticLastDate).HasColumnName("ShardLogisticLastDate");
            this.Property(t => t.ShardLogisticTotalLastWeek).HasColumnName("ShardLogisticTotalLastWeek");
            this.Property(t => t.ShardLogisticTotalLastMonth).HasColumnName("ShardLogisticTotalLastMonth");
            this.Property(t => t.MobileLastDate).HasColumnName("MobileLastDate");
            this.Property(t => t.MobileTotalLastWeek).HasColumnName("MobileTotalLastWeek");
            this.Property(t => t.MobileTotalLastMonth).HasColumnName("MobileTotalLastMonth");
            this.Property(t => t.OldTTY).HasColumnName("OldTTY");
            this.Property(t => t.RequestedAirlines).HasColumnName("RequestedAirlines");
            this.Property(t => t.RegisteredAirlines).HasColumnName("RegisteredAirlines");
            this.Property(t => t.PendingAirlines).HasColumnName("PendingAirlines");
            this.Property(t => t.EnableBranding).HasColumnName("EnableBranding");
            this.Property(t => t.DeclarationMessage).HasColumnName("DeclarationMessage");
            this.Property(t => t.ActivatedforDeclarationApprove).HasColumnName("ActivatedforDeclarationApprove");
            this.Property(t => t.CustomerURL).HasColumnName("CustomerURL");
            this.Property(t => t.HideSharedlogistics).HasColumnName("HideSharedlogistics");
            this.Property(t => t.ContactEmail).HasColumnName("ContactEmail");
            this.Property(t => t.SilverlightEndDate).HasColumnName("SilverlightEndDate");
            this.Property(t => t.IsParentTenant).HasColumnName("IsParentTenant");
            this.Property(t => t.ParentTenantId).HasColumnName("ParentTenantId");
            this.Property(t => t.StockTypeCode).HasColumnName("StockTypeCode");
            this.Property(t => t.IsINTTRAStockPrepaid).HasColumnName("IsINTTRAStockPrepaid");
            this.Property(t => t.PackageCodeSearchField).HasColumnName("PackageCodeSearchField");
            this.Property(t => t.IsINTTRAOnlyDemo).HasColumnName("IsINTTRAOnlyDemo");
            this.Property(t => t.MainAdditionalPackageApplied).HasColumnName("MainAdditionalPackageApplied");
            this.Property(t => t.TotalPrice).HasColumnName("TotalPrice");
            this.Property(t => t.SupportDomain).HasColumnName("SupportDomain");
            this.Property(t => t.TotalNumberOfUsers).HasColumnName("TotalNumberOfUsers");
            this.Property(t => t.TotalFreeUsers).HasColumnName("TotalFreeUsers");
            this.Property(t => t.AveragePrice).HasColumnName("AveragePrice");
            this.Property(t => t.TotalPaymentamount).HasColumnName("TotalPaymentamount");
            this.Property(t => t.CountryName).HasColumnName("CountryName");
            this.Property(t => t.NoPaymentForChildTenants).HasColumnName("NoPaymentForChildTenants");
            this.Property(t => t.ScheduledTasksLimitPerReport).HasColumnName("ScheduledTasksLimitPerReport");
            this.Property(t => t.WhatsAppMessagingPhoneNumber).HasColumnName("WhatsAppMessagingPhoneNumber");

            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms == "oracle")
            {
                this.Property(t => t.AgentSharedLogisticsStatisticsLastDate).HasColumnName("AgentSharedLogisticsLastDate");
                this.Property(t => t.AgentSharedLogisticsStatisticsLastWeek).HasColumnName("AgentSharedLogisticsLastWeek");
                this.Property(t => t.AgentSharedLogisticsStatisticsLastMonth).HasColumnName("AgentSharedLogisticsLastMonth");
            }
            else
            {
                this.Property(t => t.AgentSharedLogisticsStatisticsLastDate).HasColumnName("AgentSharedLogisticsStatisticsLastDate");
                this.Property(t => t.AgentSharedLogisticsStatisticsLastWeek).HasColumnName("AgentSharedLogisticsStatisticsLastWeek");
                this.Property(t => t.AgentSharedLogisticsStatisticsLastMonth).HasColumnName("AgentSharedLogisticsStatisticsLastMonth");

            }


            this.HasRequired(t => t.GlobalTenant).WithOptional(t => t.TenantManagement);
            this.HasOptional(t => t.PaymentChannel).WithMany().HasForeignKey(d => d.PaymentChannelCode);
            this.HasOptional(t => t.PaymentMethod).WithMany().HasForeignKey(d => d.PaymentMethodCode);
            this.HasOptional(t => t.RecurringPeriod).WithMany().HasForeignKey(d => d.RecurringPeriodCode);
            this.HasOptional(t => t.PaymentCurrency).WithMany().HasForeignKey(d => d.PaymentCurrencyCode);
            this.HasOptional(t => t.BluesnapContract).WithMany().HasForeignKey(d => d.BluesnapContractId);
            this.HasOptional(t => t.BluesnapContractCRM).WithMany().HasForeignKey(d => d.BluesnapCRMContractId);
            this.HasOptional(t => t.BluesnapContractEAWB).WithMany().HasForeignKey(d => d.BluesnapEAWBContractId);
            this.HasOptional(t => t.BluesnapContractEAWBS).WithMany().HasForeignKey(d => d.BluesnapEAWBSContractId);
            this.HasOptional(t => t.BluesnapInttraStockContract).WithMany().HasForeignKey(d => d.BluesnapInttraStockContractId);            
            this.HasOptional(t => t.AWBMessagesCCSType).WithMany().HasForeignKey(d => d.AWBMessagesCCSTypeCode);
            this.HasOptional(t => t.TenantType).WithMany().HasForeignKey(d => d.TenantTypeCode);

        }
    }
}
