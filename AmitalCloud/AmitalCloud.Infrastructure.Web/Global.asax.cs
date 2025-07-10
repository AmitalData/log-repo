using AmitalCloud.Infrastructure.Application.EntityQueryServices;
using AmitalCloud.Infrastructure.Application.Helpers;
using AmitalCloud.Infrastructure.Data.Helpers;
using AmitalCloud.Infrastructure.Data.Repositories;
//using AmitalCloud.Infrastructure.Data.Helpers;
//using AmitalCloud.Infrastructure.Data.Repositories;
//using AmitalCloud.Infrastructure.Data.Security;
using AmitalCloud.Infrastructure.Domain.DataContracts;
using AmitalCloud.Infrastructure.Domain.EntityPMs;
using AmitalCloud.Infrastructure.Domain.Helpers;
using AmitalCloud.Infrastructure.Model.EntityClasses;
using AmitalCloud.Shipment.WebAPI;
using System;
using System.Web;
using System.Web.Http;
using AmitalCloud.Infrastructure.Domain.Interfaces;
using AmitalCloud.Infrastructure.Web.Helpers.TreeFilterQuery;
using AmitalCloud.Infrastructure.Data.Queries;

namespace AmitalCloud.Infrastructure.WebAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            if (string.IsNullOrEmpty(AmitalCloudSettings.DeploymentStage))
            {
                string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
                AmitalCloudSettings.DatabaseManagementSystem = dbms;
                AmitalCloudSettings.DebugKey = System.Configuration.ConfigurationManager.AppSettings.Get("DebugKey");
                FillAppSettings();
                LogitudeSettings_AmitalInit();
            }

            CacheManager.CacheWrapper = new CacheWrapper(HttpContext.Current.Cache);

            LoggedContactResolver.RegisterLoggedContactUtil();

            GlobalConfiguration.Configure(WebApiConfig.Register);
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            if (HttpContext.Current?.Request?.Headers != null && !string.IsNullOrEmpty(HttpContext.Current.Request.Headers["Token"]))
            {
                string token = HttpContext.Current.Request.Headers["Token"];

                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                if (authToken != null)
                {
                    HttpContext.Current.Items.Add("authToken", authToken);
                    HttpContext.Current.Items.Add("Tenant", authToken.Tenant);

                    if (!authToken.APIToken && !authToken.InActive)
                    {
                        HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(new System.Security.Principal.GenericIdentity(authToken.Email), new string[0]);
                    }
                }
            }
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            // because of the preflight request (CORS)
            if (Request.HttpMethod == "OPTIONS")
            {
                Response.StatusCode = 200;
                Response.End();
            }

            string clientmode = System.Configuration.ConfigurationManager.AppSettings.Get("clientMode");
            if (clientmode == "angular" && SettingUtil.DeploymentStage.IsDBStage(SettingUtil.DeploymentStage.Development))
            {
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "http://localhost:4200");
                HttpContext.Current.Response.AddHeader("Access-Control-Expose-Headers", "http://localhost:4200");
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Credentials", "true");
            }

            //These headers are handling the "pre-flight" OPTIONS call sent by the browser
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE");
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Accept, token,ServerTime,TwoFactorkey");
            HttpContext.Current.Response.AddHeader("Access-Control-Max-Age", "1728000");
            HttpContext.Current.Response.AddHeader("Strict-Transport-Security", "max-age=31536000 ; includeSubDomains");

            var systemUrl = AmitalCloudSecurityUtility.getLoggedDomain();
            if (!string.IsNullOrEmpty(systemUrl) && systemUrl.ToLower().Contains("staging") && !SettingUtil.DeploymentStage.IsDBStage(SettingUtil.DeploymentStage.LogboxAndAccountingProduction))
            {
                HttpContext.Current.Items.Add("workerrolename", "staging");
                return;
            }

            if (!string.IsNullOrEmpty(HttpContext.Current.Request.CurrentExecutionFilePath) && HttpContext.Current.Request.CurrentExecutionFilePath.ToLower().Contains("/wcfapi/"))
            {
                if (!HttpContext.Current.Items.Contains("workerrolename"))
                    HttpContext.Current.Items.Add("workerrolename", "production");
            }
            else
            {
                if (HttpContext.Current.Request.Headers["workerrolename"] != null)
                {
                    if (!HttpContext.Current.Items.Contains("workerrolename"))
                        HttpContext.Current.Items.Add("workerrolename", HttpContext.Current.Request.Headers["workerrolename"]);
                    else
                        HttpContext.Current.Items["workerrolename"] = HttpContext.Current.Request.Headers["workerrolename"];
                }
            }
        }

        private void FillAppSettings()
        {
            SettingQueryService settingQueryService = new SettingQueryService(0);
            SettingPM setting = settingQueryService.GetSingle(SettingQuery.GetDefaultSettingId(), false, true);
            AmitalCloudSettings.Id = setting.Id;
            AmitalCloudSettings.ChampEnv = setting.ChampEnv;
            AmitalCloudSettings.ChampURL = setting.ChampURL;
            AmitalCloudSettings.ChampTestAPIURL = setting.ChampTestAPIURL;
            AmitalCloudSettings.ChampTestAPIPassword = setting.ChampTestAPIPassword;
            AmitalCloudSettings.ChampProdAPIURL = setting.ChampProdAPIURL;
            AmitalCloudSettings.ChampProdAPIPassword = setting.ChampProdAPIPassword;
            AmitalCloudSettings.CustomerCareIP = setting.CustomerCareIP;
            AmitalCloudSettings.DeploymentStage = setting.DeploymentStage;
            AmitalCloudSettings.IsLogEnabled = setting.IsLogEnabled;
            AmitalCloudSettings.AmitalURL = setting.LogitudeURL;
            AmitalCloudSettings.TotangoServiceId = setting.TotangoServiceId;
            AmitalCloudSettings.UsingAzure = setting.UsingAzure;
            AmitalCloudSettings.StorageAccountKey = setting.StorageAccountKey;
            AmitalCloudSettings.StorageAccountName = setting.StorageAccountName;
            AmitalCloudSettings.StorageType = setting.StorageType;
            AmitalCloudSettings.AmitalCRMTenantNumber = setting.LogitudeCRMTenantNumber;
            AmitalCloudSettings.AutoSignupEmail = setting.AutoSignupEmail;
            AmitalCloudSettings.AutoSignupPassword = setting.AutoSignupPassword;
            AmitalCloudSettings.ForceHttps = setting.ForceHttps;
            AmitalCloudSettings.CheckConnectionURL = setting.CheckConnectionURL;
            AmitalCloudSettings.AndroidSharedAppMinimumVersion = setting.AndroidSharedAppMinimumVersion;
            AmitalCloudSettings.IOSSharedAppMinimumVersion = setting.IOSSharedAppMinimumVersion;
            AmitalCloudSettings.WorkEnvironment = setting.WorkEnvironment;
            AmitalCloudSettings.LogoCode = setting.LogoCode;
            AmitalCloudSettings.EnableHybridQueue = setting.EnableHybridQueue;
            AmitalCloudSettings.EmailAlertSignature = setting.EmailAlertSignature;
            AmitalCloudSettings.IOSAppLink = setting.IOSAppLink;
            AmitalCloudSettings.AndroidAppLink = setting.AndroidAppLink;
            AmitalCloudSettings.AndroidPodAppMinimumVersion = setting.AndroidPodAppMinimumVersion;
            AmitalCloudSettings.IOSPodAppMinimumVersion = setting.IOSPodAppMinimumVersion;
            AmitalCloudSettings.MinimumOutlookVersion = setting.MinimumOutlookVersion;
            AmitalCloudSettings.ABMProductId = setting.ABMProductId;
            AmitalCloudSettings.AzureFolderName = setting.AzureFolderName;
            AmitalCloudSettings.SignAppVersion = setting.SignAppVersion;
            AmitalCloudSettings.ReportsRunUsingWR = setting.ReportsRunUsingWR;
            AmitalCloudSettings.SMSServiceUserId = setting.SMSServiceUserId;
            AmitalCloudSettings.SMSServiceAuthToken = setting.SMSServiceAuthToken;
            AmitalCloudSettings.SMSServicePhoneNumber = setting.SMSServicePhoneNumber;
            AmitalCloudSettings.GLSHKEnv = setting.GLSHKEnv;
            AmitalCloudSettings.GLSHKURL = setting.GLSHKURL;
            AmitalCloudSettings.NotificationHubName = setting.NotificationHubName;
            AmitalCloudSettings.NotificationHubConnectionString = setting.NotificationHubConnectionString;
            AmitalCloudSettings.DomainName = setting.DomainName;
            AmitalCloudSettings.ProductName = setting.ProductName;
            AmitalCloudSettings.QueueServiceMode = setting.QueueServiceMode;
            AmitalCloudSettings.StorageServiceMode = setting.StorageServiceMode;
            AmitalCloudSettings.DropboxAppKey = setting.DropboxAppKey;
            AmitalCloudSettings.DropboxAppSecret = setting.DropboxAppSecret;
            AmitalCloudSettings.OceanInsightsToken = setting.OceanInsightsToken;
            AmitalCloudSettings.CPUIntensiveWebServicesURL = setting.CPUIntensiveWebServicesURL;
            AmitalCloudSettings.AmitalCloudEnvironmentURL = setting.AmitalCloudEnvironmentURL;
            AmitalCloudSettings.AmitalCloudAmitalTenantPrimaryKey = setting.AmitalCloudLogitudeTenantPrimaryKey;
            AmitalCloudSettings.OITenantNumber = setting.OITenantNumber;
            AmitalCloudSettings.AzurePrincipalSecretKey = setting.AzurePrincipalSecretKey;
            AmitalCloudSettings.DNSZone = setting.DNSZone;
            AmitalCloudSettings.DNSIPAddress = setting.DNSIPAddress;
            AmitalCloudSettings.WorkflowStorageAccountName = setting.WorkflowStorageAccountName;
            AmitalCloudSettings.WorkflowStorageAccountKey = setting.WorkflowStorageAccountKey;
            AmitalCloudSettings.System2RedirectFraction = setting.System2RedirectFraction;
            AmitalCloudSettings.WindWardSettings = setting.WindWardSettings;
            AmitalCloudSettings.AmitalIISURL = setting.LogitudeIISURL;
            AmitalCloudSettings.TempStorageConnection = setting.TempStorageConnection;
        }

        private static void LogitudeSettings_AmitalInit()
        {
            Func<IAmitalRestrictOwnerService> createAmitalRestrictOwnerModelService = null;

            Func<int> getTenantFromToken = () =>
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                return authToken?.Tenant ?? 0;
            };

            InjectionUtil.Init(createAmitalRestrictOwnerModelService, getTenantFromToken, AmitalCloudSecurityUtility.CheckContactFeature,
                () => (new ByteCompressorUtil()) as IByteCompressorUtil,
                () => (new TreeFilterQueryService()) as ITreeFilterQueryService);
        }
    }
}
