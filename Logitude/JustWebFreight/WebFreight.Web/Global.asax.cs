using System;
using Simplog.Server.Infrastructure.Helpers;
using System.Web;
using System.Threading;
using System.Web.Routing;
using System.Web.Http;
using Microsoft.ServiceBus.Messaging;
using Simplog.Server.Infrastructure.Azure;
using WebFreight.Web.TopicQueues;
using Microsoft.WindowsAzure.ServiceRuntime;
using Simplog.Server.Infrastructure;
using Logitude.SystemLogs;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Microsoft.Practices.Unity;
using Logitude.Server.Tools.StorageService;
using Logitude.Server.Tools;
using System.Linq;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.Server.Tools.Helpers;
using System.Diagnostics;
using Logitude.Customs.BL.EntityQueryServices;
using System.Globalization;
using System.Web.Mvc;
using WebFreight.Web.CustomModel;
using WebFreight.Web.AccountingModel;
using WebFreight.Web.CRMModel;
using WebFreight.Web.SocialModel;
using WebFreight.Web.BookingModel;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Security;
using System.Net.Http;
using Logitude.Accounting.Def.EntityUpdateServicesExt;
using Logitude.BL.Interfaces;
using WebFreight.Web.Validators;
using Logitude.BL.Helpers;
using Autofac;
using System.Reflection;
using Autofac.Integration.WebApi;
using WebFreight.Web.Azure.TopicQueues;
using Microsoft.AspNet.SignalR;
using Stimulsoft.Base;
using Simplog.Server.Infrastructure.LogitudeCacheManager;
using Logitude.Server.Tools.Utils;
using Logitude.Customs.BL.Messaging.Amital;
using Logitude.Server.Tools.Models;
using Simplog.Server.Infrastructure.DataContracts;
using System.Timers;
using Logitude.SystemLogs.Repositories;
using Logitude.SystemLogs.POCOs;
using Logitude.Server.Tools.Counters;
using WebFreight.Web.Helpers;
using Logitude.BL.Resolvers;
using Logitude.Server.Tools.Resolvers;

namespace WebFreight.Web
{
    public class Global : System.Web.HttpApplication
    {
        System.Timers.Timer aTimer = new System.Timers.Timer();
        protected void Application_Start(object sender, EventArgs e)
        {
			AppDomain.CurrentDomain.FirstChanceException += (mySender, eventArgs) =>
			{
				//eventArgs.Exception.sou
				FirstChanceExceptionEventArgsLogger.LogException(eventArgs);

				//Debug.WriteLine(eventArgs.Exception.ToString());
			};

			if (string.IsNullOrEmpty(LogitudeSettings.DeploymentStage))
            {
                string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
                LogitudeSettings.DatabaseManagementSystem = dbms;
                LogitudeSettings.DebugKey = System.Configuration.ConfigurationManager.AppSettings.Get("DebugKey");
                FillAppSettings();
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
                Func<IAmitalRestrictOwnerService> createAmitalRestrictOwnerModelService = null;
                
                if (LogitudeSettings.IsCostomsDeploy)
                {
                    /// itzik : can use/convert to    !!!ContainerAccessor !!!! // ContainerAccessor.Container.RegisterType<ICustomsDocumentQueryServiceExt, CustomsDocumentQueryServiceExt>("CustomsDocumentQueryServiceExt", new InjectionFactory(c => new CustomsDocumentQueryServiceExt()));
                    var assemblyUtil = new Logitude.Server.Tools.Helpers.AssemblyUtil();
                    LogitudeSettings.ProductInfo = assemblyUtil.GetProductInfo(typeof(Global).Assembly);

                    // this project no need but in FilingManager is must 
                    LogitudeSettings.GetUnfDBConnectionInfoFromTenantInject = CustomsSettingQueryService.GetUnfDBConnectionInfo;// this project no need but in FilingManager is must 
                    LogitudeSettings.GetLogitudeCustomsSettingsMInject = CustomsSettingQueryService.GetLogitudeCustomsSettingsM;

                    Logger.OverrideExecutablePath = HttpContext.Current.Server.MapPath("App_Data");
                    LogitudeSettings.HandleLogMe = new Action<string, bool, string,DateTime>((mess, err, suffix, stopLogAt) =>
                    {
                        if (DateTime.Now > stopLogAt) return;
                        Logger.LogMe(mess, err, suffix);
                        });
                    LogitudeSettings.GetLogitudeCustomsSettingsMInject = CustomsSettingQueryService.GetLogitudeCustomsSettingsM;

                    createAmitalRestrictOwnerModelService = () =>
                      {
                          var amitalRestrictOwnerService = new AmitalRestrictOwnerService();
                          return amitalRestrictOwnerService;
                      };
                }
                
                LogitudeSettings.HandleDbExceptionInject = ExceptionHandler.HandleDbException;
                LogitudeSettings.HandleBuildObjectTablesZipFilesData_Inject = MetaDataUpdate.TenantsUpdateClass.BuildObjectTablesZipFilesData;

                Func<int> getTenantFromToken = () =>
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    return authToken.Tenant;
                    
                };
                InjectionUtil.Init(createAmitalRestrictOwnerModelService, getTenantFromToken, SecurityUtility.CheckContactFeature, () => (new ByteCompressorUtil()) as IByteCompressorUtil, new IISManager());
                ProxyUtil.SecurityUtilityCheckFeature = SecurityUtility.CheckFeature;



                LogitudeSettings.GetUserNameInject = AuthenticationUtil.ResolveUserIdentityName;
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

            LoggedContactResolver.RegisterLoggedContactUtil();
            DateTimeUtilResolver.RegisterDateTimeUtil();
            TranslateTextsClassUtilResolver.RegisterTranslateTextsClassUtil();
            IdCounterUtilResolver.RegisterIdCounterUtil();

            MessagingServiceFactoryHelper.InitContainer();


            AccountingRegistrations.Register();
            CustomsRegistrations.Register();
            
            CacheManager.CacheWrapper = new CacheWrapper(HttpContext.Current.Cache);
            if (LogitudeSettings.DeploymentStage == "Simplog" || LogitudeSettings.DeploymentStage == "amitalstorage" || LogitudeSettings.DeploymentStage == "Dev" || LogitudeSettings.DeploymentStage == "Test2" || LogitudeSettings.DeploymentStage == "logboxwe1")
            {
                LogitudeCacheManager.ServerCache = new RedisCache();
            }
            else
            {
                LogitudeCacheManager.ServerCache = new LocalHttpCache();
            }

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

            RouteTable.Routes.Ignore("{resource}.axd/{*pathInfo}");

            //GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();

            var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
			json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;


            GlobalConfiguration.Configuration.Formatters.XmlFormatter.UseXmlSerializer = true;
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
                //if (!RoleEnvironment.IsAvailable)//is azure env
                //{
                //    return;
                //}
                SubscriptionDescription myAgentSubscription;
                //string[] roleId = RoleEnvironment.CurrentRoleInstance.Id.Split('_');
                string subscribtionName = Environment.MachineName; //roleId[roleId.Length - 1];
                if (!StorageAcountDetails.NameSpaceManager.SubscriptionExists(dataCacheTopic.Path, subscribtionName))
                {
                    myAgentSubscription = StorageAcountDetails.NameSpaceManager.CreateSubscription(dataCacheTopic.Path, subscribtionName);
                }
                //  string ssss = RoleEnvironment.CurrentRoleInstance.Id;


                CacheMessageHandler cacheMessageHandler = new CacheMessageHandler();
                Thread cacheThread = new Thread(cacheMessageHandler.HandleTopicMessages);
                cacheThread.Start();

                this.StartSignalRTopicThread();

            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, "", "WebRole", "Global.asax : Application_Start Method", null);

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

            SignalRHubMessageHandler signalRMessageHandler = new SignalRHubMessageHandler();
            Thread signalRThread = new Thread(signalRMessageHandler.HandleTopicMessages);
            signalRThread.Start();
            //  string ssss = RoleEnvironment.CurrentRoleInstance.Id;
        }



        protected void Application_End(object sender, EventArgs e)
        {
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

            //   if (HttpContext.Current.Request.HttpMethod == "OPTIONS")
            //    {
            //These headers are handling the "pre-flight" OPTIONS call sent by the browser
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE");
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Accept, token,ServerTime,TwoFactorkey");
            HttpContext.Current.Response.AddHeader("Access-Control-Max-Age", "1728000");
            HttpContext.Current.Response.AddHeader("Strict-Transport-Security", "max-age=31536000 ; includeSubDomains");

            //     HttpContext.Current.Response.End();
            //  }

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
                                if(authToken.ClientType == "Web")
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
                            HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(new System.Security.Principal.GenericIdentity(authToken.Email), new string[0]);


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

		private string GetContactPasswordFromCache(string email)
		{
			ContactPasswordRepository contactPasswordRep = new ContactPasswordRepository();

			string cahce_key = "ContactPassword_" + email;
			if (CacheManager.CacheWrapper != null)
			{
				if (CacheManager.CacheWrapper.Get(cahce_key) == null)
				{
					ContactPassword contactPassword = contactPasswordRep.GetSingleContactPassword(email);
					if (CacheManager.CacheWrapper.Get(cahce_key) == null && contactPassword != null)
					{
						CacheManager.CacheWrapper.Insert(cahce_key, contactPassword.Password, null, DateTime.UtcNow.AddMinutes(5), TimeSpan.Zero);
						
					}

					return contactPassword.Password;
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
