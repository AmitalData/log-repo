using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Interfaces;
using Logitude.Server.Tools.Resolvers;
using Logitude.Server.Tools.TreeFilterQuery;
using Logitude.Server.Tools.Utils;
using Logitude.SystemLogs;
using Microsoft.AspNet.SignalR;
using Microsoft.ServiceBus.Messaging;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Azure;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure.LogitudeCacheManager;
using Logitude.BL.Helpers;
using Logitude.BL.Interfaces;
using Logitude.BL.Resolvers;
using Logitude.BL.Security;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.Messaging.Amital;
using Logitude.Customs.BL.Validators;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.CustomsMessaging.MessagingServices;
using Stimulsoft.Base;
using WebFreight.Web;
//using WebFreight.Web.AccountingModel;
//using WebFreight.Web.CustomModel;
//using WebFreight.Web.Helpers;
//using WebFreight.Web.Helpers.APIHelpers;
//using WebFreight.Web.MetaDataUpdate;
//using WebFreight.Web.TopicQueues;
//using WebFreight.Web.Validators;


namespace AmitalCloud.WEB.API
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


                LogitudeAppSettings.StartDateTime = DateTime.Now;
                LogitudeAppSettings.IsRecycled = true;
                LogitudeAppSettings.WarmingIsFinished = false;
                NetCommonHelper.Logger.DevLog.Instance.SetProcessName("WebSite", true);
                if (string.IsNullOrEmpty(LogitudeSettings.DeploymentStage))
                {
                    string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
                    LogitudeSettings.DatabaseManagementSystem = dbms;
                    LogitudeSettings.DebugKey = System.Configuration.ConfigurationManager.AppSettings.Get("DebugKey");
                    FillAppSettings();
                    LogitudeSettings_AmitalInit();

                }
                ContainerAccessor.InitContainer();
                ContainerAccessor.RegisterTypeFactory<IRulesValidator, RulesValidator>("RulesValidator", new RulesValidator());
                ContainerAccessor.RegisterTypeFactory<IQuoteTemplateReportHelper, QuoteTemplateReportHelper>("QuoteTemplateReportHelper", new QuoteTemplateReportHelper());
                ContainerAccessor.RegisterTypeFactory<IAddManualTraceEventsHelper, AddManualTraceEventsHelper>("AddManualTraceEventsHelper", new AddManualTraceEventsHelper());
                LoggedContactResolver.RegisterLoggedContactUtil();
                DateTimeUtilResolver.RegisterDateTimeUtil();
                TranslateTextsClassUtilResolver.RegisterTranslateTextsClassUtil();
                IdCounterUtilResolver.RegisterIdCounterUtil();
                MessagingServiceFactoryHelper.InitContainer();
                AccountingRegistrations.Register();
                CustomsRegistrations.Register();
                CacheManager.CacheWrapper = new CacheWrapper(HttpContext.Current.Cache);
                if (LogitudeSettings.DeploymentStage == "Test2" || LogitudeSettings.DeploymentStage == "logboxwe1" || LogitudeSettings.DeploymentStage == "logboxpre")
                {
                    LogitudeCacheManager.ServerCache = new RedisCache();
                }
                else
                {
                    LogitudeCacheManager.ServerCache = new LocalHttpCache();
                }

                InfraRegistrationHelper.Register();
                RouteTable.Routes.MapHttpRoute(
              name: "DefaultGetApi",
              routeTemplate: "api/{controller}/{id}",
              defaults: new { id = RouteParameter.Optional, action = "Get" },
              constraints: new { id = @"\d+", httpMethod = new HttpMethodConstraint("Get") }
          );

                RouteTable.Routes.MapHttpRoute(
                    name: "DefaultApi",
                    routeTemplate: "api/{controller}/{id}",
                    defaults: new { id = RouteParameter.Optional },
                    constraints: new { id = @"\d+" }
                );

                RouteTable.Routes.MapHttpRoute(
                    name: "ActionApi",
                    routeTemplate: "api/{controller}/{action}/{id}",
                    defaults: new { id = RouteParameter.Optional, action = RouteParameter.Optional }
                );


                RouteTable.Routes.MapHttpRoute("Route1", "api/{controller}/getsinglepmwithoutcomposition/{id}/{tenant}", new { controller = "Shipments", action = "GetSingleShipmentPMWithoutComposition" });
                RouteTable.Routes.MapHttpRoute("Route2", "api/{controller}/getsinglepm/{id}/{tenant}", new { controller = "Shipments", action = "GetSingleShipmentPM" });
                RouteTable.Routes.MapHttpRoute("Route3", "api/{controller}/getsinglepmbykey/{securitykey}/{id}/{tenant}", new { controller = "Shipments", action = "GetSingleShipmentPMByKey" });
                RouteTable.Routes.MapHttpRoute("Route4", "api/{controller}/getsinglepmbykeyandtenant/{securitykey}/{tenant}", new { controller = "Shipments", action = "GetSingleShipmentPMByKeyAndTenant" });
                RouteTable.Routes.MapHttpRoute("HybridTest", "api/WcfApi/Hybrid/Test/{action}", new { controller = "HybridTest", action = RouteParameter.Optional });
                RouteTable.Routes.Ignore("{resource}.axd/{*pathInfo}");
                var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
                json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;
                GlobalConfiguration.Configuration.Formatters.XmlFormatter.UseXmlSerializer = true;
                GlobalConfiguration.Configuration.Filters.Add(new ApiExceptionFilter());
                GlobalHost.Configuration.ConnectionTimeout = TimeSpan.FromSeconds(110);
                GlobalHost.Configuration.DisconnectTimeout = TimeSpan.FromSeconds(15);//30
                GlobalHost.Configuration.KeepAlive = TimeSpan.FromSeconds(5);
                StiLicense.Key = "6vJhGtLLLz2GNviWmUTrhSqnOItdDwjBylQzQcAOiHk5LQfMb0Dr1Ze4z6YRXSb7imTiay6/HzKYGUzkd/h3FMt5R7" +
    "uunoM5lX8Vs2voVkSeT6Wv6WI6Jcy4xOeAjjPkTBhC+ivrrxidMQjLaebItqFcnJWqKXBUgoJa0WfmH3soi0IbfEmI" +
    "fQ3ZmMq5BHsjsKoHSdnbzDUPWMXieYRTJZL6tsBC6QRy2ALPnYwg88ZJDGAWgAqMhZ+M0BVM17B3YJN9mu1MfAblN7" +
    "rG1eWrSrR5B53af4aeWs0RmqVNatfenGL8sufvTgOiyEuQmC9J7sHOT6VoQpWOlZthrc7JOl4zbw+qduZHZrpLuK+1" +
    "O3AB8EeDCQ6EgM8TcUesQBZZrUA4ZUFpxsCdvL0n4DQiB1tIof1TGHXCtZ62S1kAfU4XJzEGM/g3MYbKridAK5ckyc" +
    "0xwsK2y46rm9W3EV0m49Na0pcJe+2ZScc6BP1o3tDS9ddHbfkt7hFZpUNTqOxn9BOP0YVoQul+dPckYle4PS4mzXVp" +
    "tMrKV4En69rnW/z658axW0kQ2GxorKwW0IAR";

                if (LogitudeSettings.IsCostomsDeploy &&
                    LogitudeSettings.DatabaseManagementSystem.Equals("oracle", StringComparison.OrdinalIgnoreCase) &&
                    LogitudeSettings.QueueServiceMode.Equals("db", StringComparison.OrdinalIgnoreCase)
                    )
                {
                    return;
                }
                TopicDescription dataCacheTopic;
                try
                {
                    if (!StorageAcountDetails.NameSpaceManager.TopicExists(StorageAcountDetails.DataCacheTopicName))
                    {
                        dataCacheTopic = StorageAcountDetails.NameSpaceManager.CreateTopic(StorageAcountDetails.DataCacheTopicName);
                    }
                    else
                    {
                        dataCacheTopic = StorageAcountDetails.NameSpaceManager.GetTopic(StorageAcountDetails.DataCacheTopicName);
                    }
                    if (!IsDebug())
                    {
                        string subscribtionName = Environment.MachineName;
                        var commandLineArgs = Environment.GetCommandLineArgs();
                        if (commandLineArgs.Length > 1)
                        {
                            subscribtionName += ("_" + commandLineArgs[2]);
                        }
                        SubscriptionDescription myAgentSubscription;
                        if (!StorageAcountDetails.NameSpaceManager.SubscriptionExists(dataCacheTopic.Path, subscribtionName))
                        {
                            myAgentSubscription = StorageAcountDetails.NameSpaceManager.CreateSubscription(dataCacheTopic.Path, subscribtionName);
                        }

                        CacheMessageHandler cacheMessageHandler = new CacheMessageHandler();
                        Thread cacheThread = new Thread(cacheMessageHandler.HandleTopicMessages);
                        cacheThread.Start();
                    }
                    this.StartSignalRTopicThread();
                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex, DateTime.Now, 0, "", "WebRole", "Global.asax : Application_Start Method", null);

                }
                AppDomain.CurrentDomain.FirstChanceException += (mySender, eventArgs) =>
                {
                };

            }

        private void FillAppSettings()
        {
            SettingRepository settingRepository = new SettingRepository();
            Setting setting = settingRepository.GetSingleSetting("1");
            LogitudeSettings.Id = setting.Id;
            LogitudeSettings.ChampEnv = setting.ChampEnv;
            LogitudeSettings.ChampURL = setting.ChampURL;
            LogitudeSettings.ChampTestAPIURL = setting.ChampTestAPIURL;
            LogitudeSettings.ChampTestAPIPassword = setting.ChampTestAPIPassword;
            LogitudeSettings.ChampProdAPIURL = setting.ChampProdAPIURL;
            LogitudeSettings.ChampProdAPIPassword = setting.ChampProdAPIPassword;
            LogitudeSettings.CustomerCareIP = setting.CustomerCareIP;
            LogitudeSettings.DeploymentStage = setting.DeploymentStage;
            LogitudeSettings.IsLogEnabled = setting.IsLogEnabled;
            LogitudeSettings.LogitudeURL = setting.LogitudeURL;
            LogitudeSettings.TotangoServiceId = setting.TotangoServiceId;
            LogitudeSettings.UsingAzure = setting.UsingAzure;
            LogitudeSettings.StorageAccountKey = setting.StorageAccountKey;
            LogitudeSettings.StorageAccountName = setting.StorageAccountName;
            LogitudeSettings.StorageType = setting.StorageType;
            LogitudeSettings.LogitudeCRMTenantNumber = setting.LogitudeCRMTenantNumber;
            LogitudeSettings.AutoSignupEmail = setting.AutoSignupEmail;
            LogitudeSettings.AutoSignupPassword = setting.AutoSignupPassword;
            LogitudeSettings.ForceHttps = setting.ForceHttps;
            LogitudeSettings.CheckConnectionURL = setting.CheckConnectionURL;
            LogitudeSettings.AndroidSharedAppMinimumVersion = setting.AndroidSharedAppMinimumVersion;
            LogitudeSettings.IOSSharedAppMinimumVersion = setting.IOSSharedAppMinimumVersion;
            LogitudeSettings.WorkEnvironment = setting.WorkEnvironment;
            LogitudeSettings.LogoCode = setting.LogoCode;
            LogitudeSettings.EnableHybridQueue = setting.EnableHybridQueue;
            LogitudeSettings.EmailAlertSignature = setting.EmailAlertSignature;
            LogitudeSettings.IOSAppLink = setting.IOSAppLink;
            LogitudeSettings.AndroidAppLink = setting.AndroidAppLink;
            LogitudeSettings.AndroidPodAppMinimumVersion = setting.AndroidPodAppMinimumVersion;
            LogitudeSettings.IOSPodAppMinimumVersion = setting.IOSPodAppMinimumVersion;
            LogitudeSettings.MinimumOutlookVersion = setting.MinimumOutlookVersion;
            LogitudeSettings.ABMProductId = setting.ABMProductId;
            LogitudeSettings.AzureFolderName = setting.AzureFolderName;
            LogitudeSettings.SignAppVersion = setting.SignAppVersion;
            LogitudeSettings.ReportsRunUsingWR = setting.ReportsRunUsingWR;
            LogitudeSettings.SMSServiceUserId = setting.SMSServiceUserId;
            LogitudeSettings.SMSServiceAuthToken = setting.SMSServiceAuthToken;
            LogitudeSettings.SMSServicePhoneNumber = setting.SMSServicePhoneNumber;
            LogitudeSettings.GLSHKEnv = setting.GLSHKEnv;
            LogitudeSettings.GLSHKURL = setting.GLSHKURL;
            LogitudeSettings.NotificationHubName = setting.NotificationHubName;
            LogitudeSettings.NotificationHubConnectionString = setting.NotificationHubConnectionString;
            LogitudeSettings.DomainName = setting.DomainName;
            LogitudeSettings.ProductName = setting.ProductName;
            LogitudeSettings.QueueServiceMode = setting.QueueServiceMode;
            LogitudeSettings.StorageServiceMode = setting.StorageServiceMode;
            LogitudeSettings.DropboxAppKey = setting.DropboxAppKey;
            LogitudeSettings.DropboxAppSecret = setting.DropboxAppSecret;
            LogitudeSettings.OceanInsightsToken = setting.OceanInsightsToken;
            LogitudeSettings.CPUIntensiveWebServicesURL = setting.CPUIntensiveWebServicesURL;
            LogitudeSettings.AmitalCloudEnvironmentURL = setting.AmitalCloudEnvironmentURL;
            LogitudeSettings.AmitalCloudLogitudeTenantPrimaryKey = setting.AmitalCloudLogitudeTenantPrimaryKey;
            LogitudeSettings.OITenantNumber = setting.OITenantNumber;
            LogitudeSettings.AzurePrincipalSecretKey = setting.AzurePrincipalSecretKey;
            LogitudeSettings.DNSZone = setting.DNSZone;
            LogitudeSettings.DNSIPAddress = setting.DNSIPAddress;
            LogitudeSettings.WorkflowStorageAccountName = setting.WorkflowStorageAccountName;
            LogitudeSettings.WorkflowStorageAccountKey = setting.WorkflowStorageAccountKey;
            LogitudeSettings.System2RedirectFraction = setting.System2RedirectFraction;
            LogitudeSettings.WindWardSettings = setting.WindWardSettings;
            LogitudeSettings.LogitudeIISURL = setting.LogitudeIISURL;
            LogitudeSettings.TempStorageConnection = setting.TempStorageConnection;
        }

        private static void LogitudeSettings_AmitalInit()
        {
            Func<IAmitalRestrictOwnerService> createAmitalRestrictOwnerModelService = null;
            if (LogitudeSettings.IsCostomsDeploy)
            {

                bool useAppData = true;
                if (useAppData)
                {
                    AppDataUtil.Init(HostingEnvironment.ApplicationPhysicalPath);
                    var appDataUtil = new AppDataUtil();
                    LogitudeSettings.ProductInfo = appDataUtil.GetProdInfo();

                }
                else
                {
                    var assemblyUtil = new Logitude.Server.Tools.Helpers.AssemblyUtil();
                    LogitudeSettings.ProductInfo = assemblyUtil.GetProductInfo(typeof(Global).Assembly);
                }
                LogitudeSettings.GetUnfDBConnectionInfoFromTenantInject = CustomsSettingQueryService.GetUnfDBConnectionInfo;// this project no need but in FilingManager is must 
                LogitudeSettings.GetLogitudeCustomsSettingsMInject = CustomsSettingQueryService.GetLogitudeCustomsSettingsM;
                LogitudeSettings.GetLogitudeCustomsSettingsMInject = CustomsSettingQueryService.GetLogitudeCustomsSettingsM;
                createAmitalRestrictOwnerModelService = () =>
                {
                    var amitalRestrictOwnerService = new AmitalRestrictOwnerService();
                    return amitalRestrictOwnerService;
                };
            }
            LogitudeSettings.HandleDbExceptionInject = ExceptionHandler.HandleDbException;
            LogitudeSettings.HandleBuildObjectTablesZipFilesData_Inject =  TenantsUpdateClass.BuildObjectTablesZipFilesData;
            Func<int> getTenantFromToken = () =>
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                return authToken.Tenant;

            };
            InjectionUtil.Init(createAmitalRestrictOwnerModelService, getTenantFromToken, SecurityUtility.CheckContactFeature,
                () => (new ByteCompressorUtil()) as IByteCompressorUtil,
                new IISManager(),
                () => (new HtmlEditorHelper()) as IHtmlEditorHelper,
                () => (new EntityUpdateReflectorService()) as IEntityUpdateReflectorService,
                () => (new EntityGetReflectorService()) as IEntityGetReflectorService,
                () => (new TreeFilterQueryService()) as ITreeFilterQueryService
                );
            ProxyUtil.SecurityUtilityCheckFeature = SecurityUtility.CheckFeature;
            InjectionUtil.GetRequiredFieldErrorsForCourierDeclarationIsValid =
                (string courierMasterId, int tenant) =>
                {
                    var courierMasterRequiredErrors = CustomsRequiredFieldsValidator.GetCourierMasterRequiredFieldErrorsForCourierDeclaration(courierMasterId, tenant);
                    if (courierMasterRequiredErrors != null)
                    {
                        return courierMasterRequiredErrors.RequiredFields.Count == 0;

                    }
                    else
                    {
                        return true;
                    }
                };
            LogitudeSettings.GetUserNameInject = AuthenticationUtil.ResolveUserIdentityName;
        }



        private void StartSignalRTopicThread()
        {
            TopicDescription signalRTopic;
            if (!StorageAcountDetails.NameSpaceManager.TopicExists(StorageAcountDetails.SignalRHubTopicName))
            {
                signalRTopic = StorageAcountDetails.NameSpaceManager.CreateTopic(StorageAcountDetails.SignalRHubTopicName);
            }
            else
            {
                signalRTopic = StorageAcountDetails.NameSpaceManager.GetTopic(StorageAcountDetails.SignalRHubTopicName);
            }
            SubscriptionDescription myAgentSubscription;
            string subscribtionName = Environment.MachineName; //roleId[roleId.Length - 1];
            if (!StorageAcountDetails.NameSpaceManager.SubscriptionExists(signalRTopic.Path, subscribtionName))
            {
                myAgentSubscription = StorageAcountDetails.NameSpaceManager.CreateSubscription(signalRTopic.Path, subscribtionName);
            }
        }


        private bool IsDebug()
        {
#if DEBUG
            return true;
#else
                return false;
#endif
        }


    }
}
