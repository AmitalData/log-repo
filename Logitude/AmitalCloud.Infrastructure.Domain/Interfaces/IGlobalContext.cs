using AmitalCloud.Infrastructure.Domain.EntityPOCOs;
using AmitalCloud.Infrastructure.Domain.Interfaces;
using System.Data.Entity;
using System.Linq;

namespace AmitalCloud.Infrastructure.Data.Context
{
    public interface IGlobalContext :IContext
    {
        IDbSet<GlobalContact> GlobalContacts { get; }
        IDbSet<GlobalTenant> GlobalTenants { get; }
        IDbSet<GlobalDB> GlobalDBs { get; }
        IDbSet<ConvertProgramInfo> ConvertProgramInfoes {get; }
        IDbSet<PerformanceLog> PerformanceLogs { get; }
        IDbSet<LogitudeLead> LogitudeLeads { get; }
        IDbSet<TenantManagement> TenantManagements { get; }
        IDbSet<AnalyzeQueue> AnalyzeQueues { get; }
        IDbSet<AnalyzeQueueStatus> AnalyzeQueueStatus { get; }
        IDbSet<RecurringPeriod> RecurringPeriods { get; }
        IDbSet<PaymentChannel> PaymentChannels { get; }
        IDbSet<PaymentMethod> PaymentMethods { get; }
        IDbSet<ContactPassword> ContactPasswords { get; }
        IDbSet<PasswordResetRequest> PasswordResetRequests { get; }
        IDbSet<Setting> Settings { get; }
        IDbSet<PaymentCurrency> PaymentCurrencies { get; }
        IDbSet<AutoSignupEmail> AutoSignupEmails { get; }
        IDbSet<BluesnapContract> BluesnapContracts { get; }
        IDbSet<BluesnapTransaction> BluesnapTransactions { get; }
        IDbSet<AWBMessagesCCSType> AWBMessagesCCSTypes { get; }
        IDbSet<ContactMobileDevice> ContactMobileDevices { get; }
        IDbSet<MobileNotificationLog> MobileNotificationLogs { get; }
        IDbSet<SystemMetadataLastUpdate> SystemMetadataLastUpdates { get; }
        IDbSet<MonitorServiceLastUpdate> MonitorServiceLastUpdates { get; }
        IDbSet<HelpResource> HelpResources { get; }
        IDbSet<BatchServicesDefinition> BatchServicesDefinitions { get; }
        IDbSet<ChangePasswordLog> ChangePasswordLogs { get; }
        IDbSet<TenantType> TenantTypes { get; }
        IDbSet<ApiCredintials> ApiCredintials { get; }
        IDbSet<TenantManagementLicense> TenantManagementLicenses { get; }
        IDbSet<TenantAddOn> TenantAddOns { get; }
        IDbSet<OneTimePassword> OneTimePasswords { get; }
        IDbSet<BatchServicesDefinitionMods> BatchServicesDefinitionMods { get; }
        IDbSet<TenantManagmentPrivateLabels> TenantManagmentPrivateLabels { get; }
        IDbSet<AgentSharedLogisticsKey> AgentSharedLogisticsKeys { get; }
        IDbSet<SessionPolicy> SessionPolicies { get; }
        IDbSet<CaptchaKey> CaptchaKeys { get; }
        IDbSet<InvalidEmailResetPassword> InvalidEmailResetPasswords { get; }
        IDbSet<WebhookKeys> WebhookKeys { get; }
        IDbSet<BluesnapContractType> BluesnapContractTypes { get; }
        string GetCurrentConnection();
        void DetectChanges();
    }
}