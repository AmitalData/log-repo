using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Global.Data.GlobalModel.EntityPOCOs
{
    public class Setting
    {
        [Key]
        public string Id { get; set; }
        public string LogitudeURL { get; set; }
        public string ChampURL { get; set; }
        public string ChampTestAPIURL { get; set; }
        public string ChampTestAPIPassword { get; set; }
        public string ChampProdAPIURL { get; set; }
        public string ChampProdAPIPassword { get; set; }
        public string DeploymentStage { get; set; }
        public string ChampEnv { get; set; }
        public string CustomerCareIP { get; set; }
        public string TotangoServiceId { get; set; }
        public bool UsingAzure { get; set; }
        public string StorageAccountName { get; set; }
        public string StorageAccountKey { get; set; }
        public bool IsLogEnabled { get; set; }
        public string StorageType { get; set; }//azure,azureemulater,
        public int LogitudeCRMTenantNumber { get; set; }
        public double IOSSharedAppMinimumVersion { get; set; }
        public double AndroidSharedAppMinimumVersion { get; set; }
        public string AutoSignupEmail { get; set; }
        public string AutoSignupPassword { get; set; }
        public bool ForceHttps { get; set; }
        public string CheckConnectionURL { get; set; }//a url to call checkconnectionwebservice.asmx;
        public string WorkEnvironment { get; set; } //customs,main....
        public string LogoCode { get; set; }
        public bool EnableHybridQueue { get; set; }
        public string GLSHKURL { get; set; }
        public string GLSHKEnv { get; set; }
        public string NotificationHubName { get; set; }
        public string NotificationHubConnectionString { get; set; }
        public string CustomerTenantsURL { get; set; }
        public string ForwarderTenantsURL { get; set; }
        public string DomainName { get; set; }
        public string ProductName { get; set; }
        public string EmailAlertSignature { get; set; }
        public string QueueServiceMode { get; set; }
        public string StorageServiceMode { get; set; }
        public bool IsUpgradingChamp { get; set; }
        public string HtmlVersion { get; set; }
        public string AndroidAppLink { get; set; }
        public string IOSAppLink { get; set; }
        public double AndroidPodAppMinimumVersion { get; set; }
        public double IOSPodAppMinimumVersion { get; set; }
        public string MinimumOutlookVersion { get; set; }
        public bool SameUserLoginEnabled { get; set; }
        public string DropboxAppKey { get; set; }
        public string DropboxAppSecret { get; set; }
        public string ABMProductId { get; set; }
        public string LayoutDirection { get; set; } // rtl or ltr
        public string AzureFolderName { get; set; }
        public string SignAppVersion { get; set; }
        public string DocumentFilingEmailDomain { get; set; }
        public int System2RedirectFraction { get; set; }
        public bool ReportsRunUsingWR { get; set; }
        public string SMSServiceUserId { get; set; }
        public string SMSServiceAuthToken { get; set; }
        public string SMSServicePhoneNumber { get; set; }
        public string INTTRAProdFTPHost { get; set; }
        public string INTTRATestFTPHost { get; set; }
        public string OceanInsightsToken { get; set; }
        public int EmailSendingQuota { get; set; }
        public string ReleaseNotesURL { get; set; }
        public string CPUIntensiveWebServicesURL { get; set; }
        public int QBOOAuthDefault { get; set; }
        public string QBOClientID { get; set; }
        public string QBOClientSecret { get; set; }
        public string TMPersonalAccessToken { get; set; }

        public string LogitudeDemoTenants { get; set; }
        public DateTime? TMPersonalAccessExpirationDate { get; set; }
        public int OITenantNumber { get; set; }
        public string AmitalCloudEnvironmentURL { get; set; }

        public string AmitalCloudLogitudeTenantPrimaryKey { get; set; }
         public string PrivateKey { get; set; }
        public string AmitalTaxesUrl { get; set; }
        public string TaxesRediractUrl { get; set; }
        public string MamanServiceUrl { get; set; }
        public string AmitalApiAddress { get; set; }
        public string AmitalApiXFunctionsKey { get; set; }
    
         public string ReleaseDateString { get; set; }

        public string AzurePrincipalSecretKey { get; set; }
        public string DNSIPAddress { get; set; }
        public string DNSZone { get; set; }
        public string QboBaseUrl { get; set; }
        public string QboEnvironment { get; set; }
        
        public string WorkflowStorageAccountName { get; set; }
        public string WorkflowStorageAccountKey { get; set; }
		public string WindWardSettings { get; set; }
		public string LogitudeIISURL { get; set; }
		public string ExportUrl { get; set; }
        public string TempStorageConnection { get; set; }
    }
 }
