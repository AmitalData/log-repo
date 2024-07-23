using System;
using Simplog.Server.Infrastructure.Helpers;
using System.Web;
using System.Threading;
using System.Web.Routing;
using System.Web.Http;
using Microsoft.ServiceBus.Messaging;
using Simplog.Server.Infrastructure.Azure;
using WebFreight.Web.TopicQueues;
using Simplog.Server.Infrastructure;
using Logitude.SystemLogs;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Logitude.Server.Tools;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.Server.Tools.Helpers;
using Logitude.Customs.BL.EntityQueryServices;
using System.Globalization;
using WebFreight.Web.CustomModel;
using WebFreight.Web.AccountingModel;
using WebFreight.Web.Security;
using Logitude.BL.Interfaces;
using WebFreight.Web.Validators;
using Logitude.BL.Helpers;
using Autofac;
using System.Reflection;
using Autofac.Integration.WebApi;
//using WebFreight.Web.Azure.TopicQueues;
using Microsoft.AspNet.SignalR;
using Stimulsoft.Base;
using Simplog.Server.Infrastructure.LogitudeCacheManager;
using Logitude.Server.Tools.Utils;
using Logitude.Customs.BL.Messaging.Amital;
using Simplog.Server.Infrastructure.DataContracts;
using System.Timers;
using Logitude.SystemLogs.Repositories;
using Logitude.SystemLogs.POCOs;
using Logitude.Server.Tools.Counters;
using WebFreight.Web.Helpers;
using Logitude.BL.Resolvers;
using Logitude.Server.Tools.Resolvers;
using Logitude.Customs.BL.Validators;
using System.Web.Hosting;
using WebFreight.Web.Helpers.APIHelpers;
using Logitude.Customs.BL.PatchDistribution;
using Logitude.Customs.BL.PatchDistribution.Patches;
using Simplog.Server.Infrastructure.Interfaces;
using Microsoft.Practices.Unity;
using Logitude.Server.Tools.Interfaces;
using Logitude.Server.Tools.TreeFilterQuery;
using System.IO;
using Microsoft.Azure.Management.ResourceManager;

namespace WebFreight.Web
{
    public class Global : System.Web.HttpApplication
    {
        System.Timers.Timer aTimer = new System.Timers.Timer();
        protected void Application_Start(object sender, EventArgs e)
        {
            LogitudeAppSettings.StartDateTime = DateTime.Now;
            //if ((DateTime.Now - LogitudeAppSettings.EndDateTime).TotalMinutes <= 5)
            //{
            LogitudeAppSettings.IsRecycled = true;
            LogitudeAppSettings.WarmingIsFinished = false;
            //} 
            NLog.LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NLog.config"));

            if (string.IsNullOrEmpty(LogitudeSettings.DeploymentStage))
            {
                string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
                LogitudeSettings.DatabaseManagementSystem = dbms;
                LogitudeSettings.DebugKey = System.Configuration.ConfigurationManager.AppSettings.Get("DebugKey");
                FillAppSettings();
                //Thread settingsThread = new Thread(HandleSettingsChanges);
                //settingsThread.Start();


                //SessionContextConfiguration conf = new SessionContextConfiguration();


                ////SettingRepository settingRepository = new SettingRepository();
                ////Setting setting = settingRepository.GetSingleSetting("1");
                ////LogitudeSettings.Id = setting.Id;
                ////LogitudeSettings.ChampEnv = setting.ChampEnv;
                ////LogitudeSettings.ChampURL = setting.ChampURL;
                ////LogitudeSettings.CustomerCareIP = setting.CustomerCareIP;
                ////LogitudeSettings.DeploymentStage = setting.DeploymentStage;
                ////LogitudeSettings.IsLogEnabled = setting.IsLogEnabled;
                ////LogitudeSettings.LogitudeURL = setting.LogitudeURL;
                ////LogitudeSettings.TotangoServiceId = setting.TotangoServiceId;
                ////LogitudeSettings.UsingAzure = setting.UsingAzure;
                ////LogitudeSettings.StorageAccountKey = setting.StorageAccountKey;
                ////LogitudeSettings.StorageAccountName = setting.StorageAccountName;
                ////LogitudeSettings.StorageType = setting.StorageType;
                ////LogitudeSettings.LogitudeCRMTenantNumber = setting.LogitudeCRMTenantNumber;
                ////LogitudeSettings.AutoSignupEmail = setting.AutoSignupEmail;
                ////LogitudeSettings.AutoSignupPassword = setting.AutoSignupPassword;
                ////LogitudeSettings.ForceHttps = setting.ForceHttps;
                ////LogitudeSettings.CheckConnectionURL = setting.CheckConnectionURL;
                ////LogitudeSettings.AndroidSharedAppMinimumVersion = setting.AndroidSharedAppMinimumVersion;
                ////LogitudeSettings.IOSSharedAppMinimumVersion = setting.IOSSharedAppMinimumVersion;
                ////LogitudeSettings.WorkEnvironment = setting.WorkEnvironment;
                ////LogitudeSettings.LogoCode = setting.LogoCode;
                ////LogitudeSettings.EnableHybridQueue = setting.EnableHybridQueue;
                ////LogitudeSettings.EmailAlertSignature = setting.EmailAlertSignature;
                ////LogitudeSettings.IOSAppLink = setting.IOSAppLink;
                ////LogitudeSettings.AndroidAppLink = setting.AndroidAppLink;
                ////LogitudeSettings.AndroidPodAppMinimumVersion = setting.AndroidPodAppMinimumVersion;
                ////LogitudeSettings.IOSPodAppMinimumVersion = setting.IOSPodAppMinimumVersion;
                ////LogitudeSettings.MinimumOutlookVersion = setting.MinimumOutlookVersion;
                ////LogitudeSettings.ABMProductId = setting.ABMProductId;
                ////LogitudeSettings.AzureFolderName = setting.AzureFolderName;
                ////LogitudeSettings.SignAppVersion = setting.SignAppVersion;
                ////LogitudeSettings.ReportsRunUsingWR = setting.ReportsRunUsingWR;
                ////LogitudeSettings.SMSServiceUserId = setting.SMSServiceUserId;
                ////LogitudeSettings.SMSServiceAuthToken = setting.SMSServiceAuthToken;
                ////LogitudeSettings.SMSServicePhoneNumber = setting.SMSServicePhoneNumber;

                //LogitudeSettings.IsCostomsDeploy = Logitude.Customs.BL.Utils.CustomsSettingUtil.ForceDownloadXapFromIIS();
                LogitudeSettings_AmitalInit();
                ////LogitudeSettings.GLSHKEnv = setting.GLSHKEnv;
                ////LogitudeSettings.GLSHKURL = setting.GLSHKURL;
                ////LogitudeSettings.NotificationHubName = setting.NotificationHubName;
                ////LogitudeSettings.NotificationHubConnectionString = setting.NotificationHubConnectionString;
                ////LogitudeSettings.DomainName = setting.DomainName;
                ////LogitudeSettings.ProductName = setting.ProductName;
                ////LogitudeSettings.QueueServiceMode = setting.QueueServiceMode;
                ////LogitudeSettings.StorageServiceMode = setting.StorageServiceMode;
                ////LogitudeSettings.DropboxAppKey = setting.DropboxAppKey;
                ////LogitudeSettings.DropboxAppSecret = setting.DropboxAppSecret;

                //aTimer.Elapsed += new ElapsedEventHandler(OnSettingsCheckTimedEvent);
                //aTimer.Interval = 60000;
                //aTimer.Enabled = true;

            }

            //string storageServiceMode = System.Configuration.ConfigurationManager.AppSettings.Get("StorageServiceMode");
            //string queueServiceMode = System.Configuration.ConfigurationManager.AppSettings.Get("QueueServiceMode");
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


            //AreaRegistration.RegisterAllAreas();

            // A route that enables RPC requests
            //RouteTable.Routes.MapHttpRoute(
            //    name: "RpcApi",
            //    routeTemplate: "rpc/{controller}/{action}",
            //    defaults: new { action = "Get" }
            //);


            //        RouteTable.Routes.MapHttpRoute(
            //name: "DefaultApi",
            //routeTemplate: "api/{controller}/{id}",
            //defaults: new { id = System.Web.Http.RouteParameter.Optional }
            //);

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
            // RouteTable.Routes.MapHttpRoute("HybridTest", "api/WcfApi/Hybrid/Test/{action}", new { controller = "HybridTest", action = RouteParameter.Optional });
            RouteTable.Routes.MapHttpRoute("HybridModel", "api/WebAPI/HybridModel/{controller}/{action}");

            RouteTable.Routes.Ignore("{resource}.axd/{*pathInfo}");

            //GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();

            var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;


            GlobalConfiguration.Configuration.Formatters.XmlFormatter.UseXmlSerializer = true;
            GlobalConfiguration.Configuration.Filters.Add(new ApiExceptionFilter());
            //GlobalConfiguration.Configuration.Formatters.Add(GlobalConfiguration.Configuration.Formatters.XmlFormatter);
            //var builder = new ContainerBuilder();
            //var config = GlobalConfiguration.Configuration;
            //builder.RegisterType<BranchesController>();
            ////builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            //var container = builder.Build();
            //config.DependencyResolver = new AutofacWebApiDependencyResolver(container);


            //GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Serialize;

            //GlobalConfiguration.Configuration.Formatters.Remove(GlobalConfiguration.Configuration.Formatters.XmlFormatter);

            // Make long polling connections wait a maximum of 110 seconds for a
            // response. When that time expires, trigger a timeout command and
            // make the client reconnect.
            GlobalHost.Configuration.ConnectionTimeout = TimeSpan.FromSeconds(110);

            // Wait a maximum of 30 seconds after a transport connection is lost
            // before raising the Disconnected event to terminate the SignalR connection.
            GlobalHost.Configuration.DisconnectTimeout = TimeSpan.FromSeconds(15);//30

            // For transports other than long polling, send a keepalive packet every
            // 10 seconds. 
            // This value must be no more than 1/3 of the DisconnectTimeout value.
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
                if (!isDebug())
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
                //eventArgs.Exception.sou
                //FirstChanceExceptionEventArgsLogger.LogException(eventArgs);

                //Debug.WriteLine(eventArgs.Exception.ToString());
            };

        }

        private bool isDebug()
        {
#if DEBUG
            return true;
#else
                return false;
#endif
        }

        private void HandleSettingsChanges()
        {
            while (true)
            {
                try
                {
                    FillAppSettings();
                    Thread.Sleep(2000);
                }

                catch { }
            }
        }

        private static void LogitudeSettings_AmitalInit()//itzik:CleanCode when is possible -should convert 2 ContainerAccessor
        {
            //LogitudeSettings.IsCostomsDeploy = Logitude.Customs.BL.Utils.CustomsSettingUtil.ForceDownloadXapFromIIS();
            Func<IAmitalRestrictOwnerService> createAmitalRestrictOwnerModelService = null;

            if (LogitudeSettings.IsCostomsDeploy)
            {
                /// itzik : can use/convert to    !!!ContainerAccessor !!!! // ContainerAccessor.Container.RegisterType<ICustomsDocumentQueryServiceExt, CustomsDocumentQueryServiceExt>("CustomsDocumentQueryServiceExt", new InjectionFactory(c => new CustomsDocumentQueryServiceExt()));
                /// 

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
                // this project no need but in FilingManager is must 
                LogitudeSettings.GetUnfDBConnectionInfoFromTenantInject = CustomsSettingQueryService.GetUnfDBConnectionInfo;// this project no need but in FilingManager is must 
                LogitudeSettings.GetLogitudeCustomsSettingsMInject = CustomsSettingQueryService.GetLogitudeCustomsSettingsM;


                LogitudeSettings.GetLogitudeCustomsSettingsMInject = CustomsSettingQueryService.GetLogitudeCustomsSettingsM;

                createAmitalRestrictOwnerModelService = () =>
                {
                    var amitalRestrictOwnerService = new AmitalRestrictOwnerService();
                    return amitalRestrictOwnerService;
                };
            }


            //logging
            Logger.OverrideExecutablePath = HttpContext.Current.Server.MapPath("App_Data");
            LogitudeSettings.HandleLogMe = new Action<string, bool, string, DateTime>((mess, err, suffix, stopLogAt) =>
            {
                if (DateTime.Now > stopLogAt) return;
                Logger.LogMe(mess, err, suffix);
            });




            LogitudeSettings.HandleDbExceptionInject = ExceptionHandler.HandleDbException;
            LogitudeSettings.HandleBuildObjectTablesZipFilesData_Inject = MetaDataUpdate.TenantsUpdateClass.BuildObjectTablesZipFilesData;

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

        private static void ProductInfoSetting()
        {
            try
            {

                bool useAppData = false;
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
                    var myP19R03_0000_PatchDist = new P19R03_0001_PatchDist();
                    myP19R03_0000_PatchDist.Enshure_SeedDbMigrateTable();

                    if (!LogitudeSettings.IsCostomsDeploy)
                    {
                        return;
                    }

                    bool supressAlertProductMessage = !String.IsNullOrWhiteSpace(System.Configuration.ConfigurationManager.AppSettings.Get("supressAlertProductMessage"));
                    if (!supressAlertProductMessage)
                    {
                        var assemblyVersion = assemblyUtil.GetVersion(LogitudeSettings.ProductInfo);
                        var patchDistributionMatch = new PatchDistributionMatch();
                        var patchDistributionMatchModel = patchDistributionMatch.GetPatchDistributionMatchModel(assemblyVersion);
                        if (patchDistributionMatchModel.MyAssemblyDBMigrationModel == null)
                        {
                            return;
                        }
                        if (patchDistributionMatchModel.MajorVersionMatch == PatchDistributionMatch.MajorVersionMatchEnum.OldDB ||
                            patchDistributionMatchModel.MajorVersionMatch == PatchDistributionMatch.MajorVersionMatchEnum.OldSource)
                        {

                            LogitudeSettings.ProductMessage = patchDistributionMatchModel.Message;
                            return;
                        }
                        if (patchDistributionMatchModel.MajorVersionMatch == PatchDistributionMatch.MajorVersionMatchEnum.OK_DBAndAssemblyREqual)
                        {
                            var _PatchDistributionManager = new PatchDistributionManager();
                            var patchDistributionList = _PatchDistributionManager.GetPatchDistribution_MinorNotClosed(patchDistributionMatchModel.LastClosed_DBMigration.MajorVersion, patchDistributionMatchModel.LastClosed_DBMigration.MinorVersion);
                            if (patchDistributionList.Count == 0)
                            {
                                return;
                            }
                            LogitudeSettings.ProductMessage = @"הגרסה המיגורית תקינה 
אולם לא בוצעו עדכונים מינורים  ";


                        }
                    }

                }
            }
            catch (Exception e)
            {

                Logger.LogMe("ProductInfoSetting:" + e.ToString(), false);
            }
            finally
            {
                Logger.LogMe(LogitudeSettings.ProductMessage, false, "ProductMessage");
            }

        }


        private void OnSettingsCheckTimedEvent(object source, ElapsedEventArgs e)
        {
            FillAppSettings();
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
            //if (!RoleEnvironment.IsAvailable)//is azure env
            //{
            //    return;
            //}
            SubscriptionDescription myAgentSubscription;
            //string[] roleId = RoleEnvironment.CurrentRoleInstance.Id.Split('_');
            string subscribtionName = Environment.MachineName; //roleId[roleId.Length - 1];
            if (!StorageAcountDetails.NameSpaceManager.SubscriptionExists(signalRTopic.Path, subscribtionName))
            {
                myAgentSubscription = StorageAcountDetails.NameSpaceManager.CreateSubscription(signalRTopic.Path, subscribtionName);
            }

            //SignalRHubMessageHandler signalRMessageHandler = new SignalRHubMessageHandler();
            //Thread signalRThread = new Thread(signalRMessageHandler.HandleTopicMessages);
            //signalRThread.Start();
            //  string ssss = RoleEnvironment.CurrentRoleInstance.Id;
        }



        protected void Application_End(object sender, EventArgs e)
        {
            LogitudeAppSettings.EndDateTime = DateTime.Now;
            aTimer.Enabled = false;
            ContainerAccessor.CleanUp();
        }


        //RouteTable.Routes.MapHttpRoute(
        //    name: "Default",
        //    url: "{controller}/{action}/{id}",
        //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
        //);


        //protected void Session_Start(object sender, EventArgs e)
        //{

        //}
        bool _IAmDebuging_StopOpenNewThreads = false;
        protected void Application_BeginRequest(object sender, EventArgs e)
        {

            if (LogitudeSettings.IsCostomsDeploy)
            {
                var he = new CultureInfo("he-IL");// '("en-US") '    "he-IL")
                he.DateTimeFormat.DateSeparator = ".";
                he.DateTimeFormat.ShortDatePattern = "dd-MM-yy";// ' "yyyy/MM/dd" '  ' "DD/MM/YYYY"
                System.Threading.Thread.CurrentThread.CurrentCulture = he;


                if (_IAmDebuging_StopOpenNewThreads)
                {
                    HttpContext.Current.Response.End();
                }
            }



            //string token = HttpContext.Current.Request.Headers["Token"];
            //if (!string.IsNullOrEmpty(token))
            //{
            //    ICommonDataContext context = CommonDataContext.GetContext(0);
            //    AuthenticationTokenRepository tokenRep = new AuthenticationTokenRepository(context);
            //    AuthenticationToken authToken = tokenRep.GetSingleToken(token);
            //    if (authToken != null)
            //    {
            //        HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(new System.Security.Principal.GenericIdentity(authToken.Email), new string[0]);
            //    }
            //}

            //HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(new System.Security.Principal.GenericIdentity("Nikhil"), new string[0]);
            //if (HttpContext.Current != null)
            //{
            //     if (HttpContext.Current.User != null)
            //    {
            //        Thread.CurrentPrincipal = HttpContext.Current.User;
            //    }
            //    if (Thread.CurrentThread.Name == null)
            //    {
            //        Thread.CurrentThread.Name = "Mohammad";
            //    }
            //}
            string clientmode = System.Configuration.ConfigurationManager.AppSettings.Get("clientMode");
            if (clientmode == "angular" && LogitudeSettings.DeploymentStage == "Dev") //islam: please don't remark this !!!!!!!!!!!!!!
            {
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "http://localhost:4200");
                HttpContext.Current.Response.AddHeader("Access-Control-Expose-Headers", "http://localhost:4200");
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Credentials", "true");
            }
            if (LogitudeSettings.DeploymentStage.ToLower() == "test2")
            {
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "https://test.logitudeworld.com/");
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "http://test.logitudeworld.com/");

            }
            //   if (HttpContext.Current.Request.HttpMethod == "OPTIONS")
            //    {
            //These headers are handling the "pre-flight" OPTIONS call sent by the browser
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE");
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Accept, token,ServerTime,TwoFactorkey");
            HttpContext.Current.Response.AddHeader("Access-Control-Max-Age", "1728000");
            HttpContext.Current.Response.AddHeader("Strict-Transport-Security", "max-age=31536000 ; includeSubDomains");

            //     HttpContext.Current.Response.End();
            //  }


            var systemUrl = SecurityUtility.getLoggedDomain();
            if (!string.IsNullOrEmpty(systemUrl) && systemUrl.ToLower().Contains("staging") && LogitudeSettings.DeploymentStage != "amitalstorage")
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


        private void InSertFailedTokenLog(string token)
        {
            FailedTokenLogRepository failedTokenLogRepository = new FailedTokenLogRepository();
            FailedTokenLog failedTokenLog = new FailedTokenLog()
            {
                Id = IdCounter.GetNumber("FailedTokenLog", 0).ToString(),
                Browser = HttpContext.Current.Request.Browser.Type,
                IP = AuthenticationUtil.GetIP4Address(),
                GMTDateTime = DateTime.Now,
                Token = token,

            };

            string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
            if (!string.IsNullOrEmpty(currentIP)) failedTokenLog.Browser = failedTokenLog.Browser.ToUpper();

            failedTokenLogRepository.Add(failedTokenLog);
            failedTokenLogRepository.SubmitChanges();
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.CurrentExecutionFilePath) && HttpContext.Current.Request.CurrentExecutionFilePath.Contains("/WcfApi/"))
                {
                    HttpContext.Current.User = null;

                }

                string token = HttpContext.Current.Request.Headers["Token"];
                if (!string.IsNullOrEmpty(token))
                {

                    ICommonDataContext context = CommonDataContext.GetContext(0);
                    AuthenticationTokenRepository tokenRep = new AuthenticationTokenRepository(context);
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    if (authToken != null)
                    {
                        if (!authToken.APIToken)
                        {
                            if (!authToken.InActive)
                            {
                                if (authToken.ClientType == "Web")
                                {
                                    if (authToken.ExpirationDate != null)
                                    {
                                        DateTime nowDate = DateTime.Now;
                                        DateTime expirationDate = (DateTime)authToken.ExpirationDate;
                                        if (expirationDate < nowDate)
                                        {
                                            HttpContext.Current.Items.Add("Session", "SessionExpiration");
                                            return;
                                        }

                                    }


                                    if (GetContactPasswordFromCache(authToken.Email) == authToken.Password)
                                    {
                                        HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(new System.Security.Principal.GenericIdentity(authToken.Email), new string[0]);
                                    }
                                    else
                                    {
                                        ContactPasswordRepository contactPasswordRep = new ContactPasswordRepository();
                                        ContactPassword contactPassword = contactPasswordRep.GetSingleContactPassword(authToken.Email);
                                        if (contactPassword != null && contactPassword.Password == authToken.Password)
                                            HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(new System.Security.Principal.GenericIdentity(authToken.Email), new string[0]);

                                    }
                                }

                                else HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(new System.Security.Principal.GenericIdentity(authToken.Email), new string[0]);
                            }
                            else HttpContext.Current.User = null;

                        }
                        else
                        {
                            HandleAPIAuthenticationToken(authToken);
                        }

                    }
                    else
                    {

                        InSertFailedTokenLog(token);
                        HttpContext.Current.User = null;
                    }
                }

            }
            catch (Exception ex)
            {
                string authenticateduser = "";
                try
                {
                    authenticateduser = Security.SecurityUtility.GetAuthenticatedUser();
                }
                catch
                {
                    authenticateduser = "UnKnown";
                }

                string ip = "";
                if (HttpContext.Current != null && HttpContext.Current.Request != null)
                {
                    string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
                    if (string.IsNullOrEmpty(currentIP))
                    {
                        currentIP = HttpContext.Current.Request.UserHostAddress;
                    }
                    ip = currentIP;
                }

                ExceptionHandler.HandleException(ex, DateTime.Now, 0, "", authenticateduser, "", ip);

            }
        }

        private static void HandleAPIAuthenticationToken(AuthenticationToken authToken)
        {
            if (authToken.ExpirationDate != null && (DateTime)authToken.ExpirationDate < DateTime.Now)
            {
                HttpContext.Current.Items.Add("APICredintial", "APICredintialExpired");
                return;
            }
            HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(new System.Security.Principal.GenericIdentity(authToken.Email), new string[0]);
        }

        private static object _lock = new object();
        private string GetContactPasswordFromCache(string email)
        {
            ContactPasswordRepository contactPasswordRep = new ContactPasswordRepository();
            string cahce_key = "ContactPassword_" + email;
            if (CacheManager.CacheWrapper != null)
            {
                if (CacheManager.CacheWrapper.Get(cahce_key) == null)
                {
                    lock (_lock)
                    {
                        return GetContactPassword(email, contactPasswordRep, cahce_key);
                    }
                }
                else
                {
                    return (string)CacheManager.CacheWrapper.Get(cahce_key);
                }
            }
            else
            {
                ContactPassword contactPassword = contactPasswordRep.GetSingleContactPassword(email);
                return contactPassword.Password;
            }

        }

        private string GetContactPassword(string email, ContactPasswordRepository contactPasswordRep, string cahce_key)
        {
            if (CacheManager.CacheWrapper.Get(cahce_key) == null)
            {
                ContactPassword contactPassword = contactPasswordRep.GetSingleContactPassword(email);
                if (contactPassword != null)
                {
                    CacheManager.CacheWrapper.Insert(cahce_key, contactPassword.Password, null, DateTime.UtcNow.AddMinutes(5), TimeSpan.Zero);
                }

                return contactPassword != null ? contactPassword.Password : "";
            }
            return (string)CacheManager.CacheWrapper.Get(cahce_key);
        }



        protected void Application_Error(object sender, EventArgs e)
        {

        }

        //protected void Session_End(object sender, EventArgs e)
        //{

        //}

        //protected void Application_End(object sender, EventArgs e)
        //{

        //}

        //internal sealed class DomainServiceFactory : IDomainServiceFactory
        //{

        //    private IDomainServiceFactory _defaultFactory;

        //    public DomainServiceFactory(IDomainServiceFactory defaultFactory)
        //    {
        //        _defaultFactory = defaultFactory;
        //    }

        //    public DomainService CreateDomainService(Type domainServiceType, DomainServiceContext context)
        //    {
        //        if ((domainServiceType == typeof(WebFreightDomainService)) ||
        //            (domainServiceType == typeof(GeneralDomainService)) )
        //        {
        //            DomainServiceContext authServiceContext =
        //                new DomainServiceContext(context, DomainOperationType.Query);
        //            SimplogAuthinticationService authService =
        //                (SimplogAuthinticationService)_defaultFactory.CreateDomainService(typeof(SimplogAuthinticationService), authServiceContext);

        //            UserData currentMember = authService.GetUser();

        //            // Hack for Ajax demo
        //            //if (currentMember == null)
        //            //{
        //            //    currentMember = new  UserData()
        //            //    {
        //            //        Name = "nikhilk",
        //            //        DisplayName = "Nikhil Kothari",
        //            //        MemberID = 1
        //            //    };
        //            //    HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(new System.Security.Principal.GenericIdentity("Nikhil"), new string[0]);
        //            //}
        //            if (currentMember != null)
        //            {
        //                DomainService domainService = (DomainService)Activator.CreateInstance(domainServiceType, currentMember);
        //                domainService.Initialize(context);

        //                return domainService;
        //            }
        //        }

        //        return _defaultFactory.CreateDomainService(domainServiceType, context);
        //    }

        //    public void ReleaseDomainService(DomainService domainService)
        //    {
        //        if ((domainService is WebFreightDomainService) ||
        //            (domainService is GeneralDomainService) )
        //        {
        //            domainService.Dispose();
        //        }
        //        else
        //        {
        //            _defaultFactory.ReleaseDomainService(domainService);
        //        }
        //    }
        //}


    }




}
