using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Global.Data.GlobalModel.Mapping
{
    public class SettingMap : EntityTypeConfiguration<Setting>
    {
        public SettingMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(40).IsUnicode(false);
            this.Property(t => t.LogitudeURL).IsRequired().HasMaxLength(1000).IsUnicode(false);
            this.Property(t => t.ChampURL).IsRequired().HasMaxLength(1000).IsUnicode(false);
            this.Property(t => t.IOSSharedAppMinimumVersion).IsRequired();
            this.Property(t => t.AndroidSharedAppMinimumVersion).IsRequired();
            this.Property(t => t.DeploymentStage).IsRequired().HasMaxLength(20).IsUnicode(false);
            this.Property(t => t.UsingAzure).IsRequired();
            this.Property(t => t.IsLogEnabled).IsRequired();
            this.Property(t => t.ChampEnv).IsRequired().HasMaxLength(20).IsUnicode(false);
            this.Property(t => t.CustomerCareIP).IsRequired().HasMaxLength(250).IsUnicode(false);
            this.Property(t => t.TotangoServiceId).IsRequired().HasMaxLength(20).IsUnicode(false);
            this.Property(t => t.StorageAccountName).IsRequired().HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.StorageType).IsRequired().HasMaxLength(60).IsUnicode(false);
            this.Property(t => t.AutoSignupEmail).IsRequired().HasMaxLength(70).IsUnicode(false);
            this.Property(t => t.AutoSignupPassword).IsRequired().HasMaxLength(40).IsUnicode(false);
            this.Property(t => t.ForceHttps).IsRequired();
            this.Property(t => t.CheckConnectionURL).IsRequired().HasMaxLength(1000).IsUnicode(false);
            this.Property(t => t.WorkEnvironment).HasMaxLength(20).IsUnicode(false);
            this.Property(t => t.LogoCode).HasMaxLength(5).IsUnicode(false);
            this.Property(t => t.EnableHybridQueue).IsRequired();
            this.Property(t => t.GLSHKURL).IsRequired().HasMaxLength(1000).IsUnicode(false);
            this.Property(t => t.GLSHKEnv).IsRequired().HasMaxLength(20).IsUnicode(false);
            this.Property(t => t.NotificationHubName).HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.NotificationHubConnectionString).HasMaxLength(600).IsUnicode(false);
            this.Property(t => t.ForwarderTenantsURL).HasMaxLength(600).IsUnicode(false);
            this.Property(t => t.CustomerTenantsURL).HasMaxLength(600).IsUnicode(false);
            this.Property(t => t.DomainName).HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.ProductName).HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.EmailAlertSignature).HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.QueueServiceMode).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.StorageServiceMode).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.HtmlVersion).HasMaxLength(10).IsUnicode(false);
            this.Property(t => t.AndroidAppLink).HasMaxLength(1000).IsUnicode(false);
            this.Property(t => t.IOSAppLink).HasMaxLength(1000).IsUnicode(false);
            this.Property(t => t.MinimumOutlookVersion).HasMaxLength(10).IsUnicode(false);
            this.Property(t => t.DropboxAppKey).HasMaxLength(50).IsUnicode(false);
            this.Property(t => t.DropboxAppSecret).HasMaxLength(50).IsUnicode(false);
            this.Property(t => t.ABMProductId).HasMaxLength(25).IsUnicode(false);
            this.Property(t => t.LayoutDirection).HasMaxLength(3).IsUnicode(false);
            this.Property(t => t.AzureFolderName).HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.SignAppVersion).HasMaxLength(10).IsUnicode(false);
            this.Property(t => t.DocumentFilingEmailDomain).HasMaxLength(1000).IsUnicode(false);
            this.Property(t => t.SMSServiceUserId).HasMaxLength(40).IsUnicode(false);
            this.Property(t => t.SMSServiceAuthToken).HasMaxLength(40).IsUnicode(false);
            this.Property(t => t.SMSServicePhoneNumber).HasMaxLength(40).IsUnicode(false);
            this.Property(t => t.INTTRAProdFTPHost).HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.INTTRATestFTPHost).HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.OceanInsightsToken).HasMaxLength(200).IsUnicode(false);
            this.Property(t => t.ReleaseNotesURL).HasMaxLength(600).IsUnicode(false);
            this.Property(t => t.CPUIntensiveWebServicesURL).HasMaxLength(1000).IsUnicode(false);
            this.Property(t => t.ChampProdAPIURL).HasMaxLength(1000).IsUnicode(false);
            this.Property(t => t.ChampProdAPIPassword).HasMaxLength(40).IsUnicode(false);
            this.Property(t => t.ChampTestAPIURL).HasMaxLength(1000).IsUnicode(false);
            this.Property(t => t.ChampTestAPIPassword).HasMaxLength(40).IsUnicode(false);
            this.Property(t => t.QBOClientID).HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.QBOClientSecret).HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.StorageAccountKey).IsRequired().HasMaxLength(1000).IsUnicode(false);
            this.Property(t => t.TMPersonalAccessToken).HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.LogitudeDemoTenants).IsUnicode(false).HasColumnName("LogitudeDemoTenants");
            this.Property(t => t.AmitalCloudEnvironmentURL).HasMaxLength(1000).IsUnicode(false);
            this.Property(t => t.AmitalCloudLogitudeTenantPrimaryKey).HasMaxLength(50).IsUnicode(true);
            this.Property(t => t.ReleaseDateString).HasMaxLength(50).IsUnicode(false);
            this.Property(t => t.DNSZone).HasMaxLength(1000).IsUnicode(false);
            this.Property(t => t.DNSIPAddress).HasMaxLength(1000).IsUnicode(false);
            this.Property(t => t.AzurePrincipalSecretKey).HasMaxLength(1000).IsUnicode(false);
			this.Property(t => t.WindWardSettings).HasMaxLength(1000).IsUnicode(false);

			// Table & Column Mappings
			this.ToTable("Settings");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.LogitudeURL).HasColumnName("LogitudeURL");
            this.Property(t => t.ChampURL).HasColumnName("ChampURL");
            this.Property(t => t.DeploymentStage).HasColumnName("DeploymentStage");
            this.Property(t => t.ChampEnv).HasColumnName("ChampEnv");
            this.Property(t => t.CustomerCareIP).HasColumnName("CustomerCareIP");
            this.Property(t => t.TotangoServiceId).HasColumnName("TotangoServiceId");
            this.Property(t => t.UsingAzure).HasColumnName("UsingAzure");
            this.Property(t => t.StorageAccountName).HasColumnName("StorageAccountName");
            this.Property(t => t.StorageAccountKey).HasColumnName("StorageAccountKey");
            this.Property(t => t.IsLogEnabled).HasColumnName("IsLogEnabled");
            this.Property(t => t.StorageType).HasColumnName("StorageType");
            this.Property(t => t.LogitudeCRMTenantNumber).HasColumnName("LogitudeCRMTenantNumber");
            this.Property(t => t.System2RedirectFraction).HasColumnName("System2RedirectFraction");
            this.Property(t => t.AutoSignupEmail).HasColumnName("AutoSignupEmail");
            this.Property(t => t.AutoSignupPassword).HasColumnName("AutoSignupPassword");
            this.Property(t => t.ForceHttps).HasColumnName("ForceHttps");
            this.Property(t => t.CheckConnectionURL).HasColumnName("CheckConnectionURL");
            this.Property(t => t.WorkEnvironment).HasColumnName("WorkEnvironment");
            this.Property(t => t.IOSSharedAppMinimumVersion).HasColumnName("IOSSharedAppMinimumVersion");
            this.Property(t => t.AndroidSharedAppMinimumVersion).HasColumnName("AndroidSharedAppMinimumVersion");
            this.Property(t => t.DomainName).HasColumnName("DomainName");
            this.Property(t => t.ProductName).HasColumnName("ProductName");
            this.Property(t => t.LogoCode).HasColumnName("LogoCode");
            this.Property(t => t.EnableHybridQueue).HasColumnName("EnableHybridQueue");
            this.Property(t => t.GLSHKURL).HasColumnName("GLSHKURL");
            this.Property(t => t.GLSHKEnv).HasColumnName("GLSHKEnv");
            this.Property(t => t.NotificationHubName).HasColumnName("NotificationHubName");
            this.Property(t => t.HtmlVersion).HasColumnName("HtmlVersion");
            this.Property(t => t.IOSAppLink).HasColumnName("IOSAppLink");
            this.Property(t => t.AndroidAppLink).HasColumnName("AndroidAppLink");
            this.Property(t => t.AndroidPodAppMinimumVersion).HasColumnName("AndroidPodAppMinimumVersion");
            this.Property(t => t.IOSPodAppMinimumVersion).HasColumnName("IOSPodAppMinimumVersion");
            this.Property(t => t.SameUserLoginEnabled).HasColumnName("SameUserLoginEnabled");
            this.Property(t => t.MinimumOutlookVersion).HasColumnName("MinimumOutlookVersion");
            this.Property(t => t.DropboxAppSecret).HasColumnName("DropboxAppSecret");
            this.Property(t => t.DropboxAppKey).HasColumnName("DropboxAppKey");
            this.Property(t => t.ABMProductId).HasColumnName("ABMProductId");
            this.Property(t => t.AzureFolderName).HasColumnName("AzureFolderName");
            this.Property(t => t.SignAppVersion).HasColumnName("SignAppVersion");
            this.Property(t => t.DocumentFilingEmailDomain).HasColumnName("DocumentFilingEmailDomain");
            this.Property(t => t.ReportsRunUsingWR).HasColumnName("ReportsRunUsingWR");
            this.Property(t => t.CPUIntensiveWebServicesURL).HasColumnName("CPUIntensiveWebServicesURL");
            this.Property(t => t.ChampProdAPIURL).HasColumnName("ChampProdAPIURL");
            this.Property(t => t.ChampProdAPIPassword).HasColumnName("ChampProdAPIPassword");
            this.Property(t => t.ChampTestAPIPassword).HasColumnName("ChampTestAPIPassword");
            this.Property(t => t.ChampTestAPIURL).HasColumnName("ChampTestAPIURL");
            this.Property(t => t.QBOOAuthDefault).HasColumnName("QBOOAuthDefault");
            this.Property(t => t.QBOClientID).HasColumnName("QBOClientID");
            this.Property(t => t.QBOClientSecret).HasColumnName("QBOClientSecret");
            this.Property(t => t.TMPersonalAccessExpirationDate).HasColumnName("TMPersonalAccessExpirationDate");
            this.Property(t => t.OITenantNumber).HasColumnName("OITenantNumber");
            this.Property(t => t.AmitalCloudEnvironmentURL).HasColumnName("AmitalCloudEnvironmentURL");
            this.Property(t => t.AmitalCloudLogitudeTenantPrimaryKey).HasColumnName("AmitalCloudLogitudeTenantPrimaryKey");
            this.Property(t => t.ReleaseDateString).HasColumnName("ReleaseDateString");
            this.Property(t => t.DNSZone).HasColumnName("DNSZone");
            this.Property(t => t.DNSIPAddress).HasColumnName("DNSIPAddress");
            this.Property(t => t.AzurePrincipalSecretKey).HasColumnName("AzurePrincipalSecretKey");
            
            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms == "oracle")
            {
                this.Property(t => t.NotificationHubConnectionString).HasColumnName("NotificationHubConnString");
                this.Property(t => t.AmitalCloudLogitudeTenantPrimaryKey).HasColumnName("AmitalCloudLogitudeTenantPK");
            }
            else
            {
                this.Property(t => t.NotificationHubConnectionString).HasColumnName("NotificationHubConnectionString");
                this.Property(t => t.AmitalCloudLogitudeTenantPrimaryKey).HasColumnName("AmitalCloudLogitudeTenantPrimaryKey");
            }

            this.Property(t => t.ForwarderTenantsURL).HasColumnName("ForwarderTenantsURL");
            this.Property(t => t.CustomerTenantsURL).HasColumnName("CustomerTenantsURL");
            this.Property(t => t.EmailAlertSignature).HasColumnName("EmailAlertSignature");
            this.Property(t => t.QueueServiceMode).HasColumnName("QueueServiceMode");
            this.Property(t => t.StorageServiceMode).HasColumnName("StorageServiceMode");
            this.Property(t => t.IsUpgradingChamp).HasColumnName("IsUpgradingChamp");
            this.Property(t => t.LayoutDirection).HasColumnName("LayoutDirection");
            this.Property(t => t.SMSServiceUserId).HasColumnName("SMSServiceUserId");
            this.Property(t => t.SMSServiceAuthToken).HasColumnName("SMSServiceAuthToken");
            this.Property(t => t.SMSServicePhoneNumber).HasColumnName("SMSServicePhoneNumber");
            this.Property(t => t.INTTRAProdFTPHost).HasColumnName("INTTRAProdFTPHost");
            this.Property(t => t.INTTRATestFTPHost).HasColumnName("INTTRATestFTPHost");
            this.Property(t => t.OceanInsightsToken).HasColumnName("OceanInsightsToken");
            this.Property(t => t.EmailSendingQuota).HasColumnName("EmailSendingQuota");
            this.Property(t => t.ReleaseNotesURL).HasColumnName("ReleaseNotesURL");
            this.Property(t => t.TMPersonalAccessToken).HasColumnName("TMPersonalAccessToken");
			this.Property(t => t.WindWardSettings).HasColumnName("WindWardSettings");

		}
	}
}
