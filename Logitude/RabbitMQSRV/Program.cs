using Logitude.BL.Helpers;
using Logitude.BL.Resolvers;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.Validators;
using Logitude.Customs.Data.Repsitories;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.CustomsMessaging.Testers;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Utils;
using Logitude.SystemLogs;
using RabbitMQSRV.Testers;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using System.Windows.Forms;
using WebFreight.Web.CustomModel;
using WebFreight.Web.Security;

namespace RabbitMQSRV
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ThreadStartStaticIsMustB4UsingTheDB();
            //(new PooledPublish()).Test();
            //EcomTesterService.Test();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
        static bool _ThreadStartStaticLoaded = false;
        public static void ThreadStartStaticIsMustB4UsingTheDB()
        {
            if (_ThreadStartStaticLoaded) return;
            string prodInfo = "";

            try
            {
                var assemblyUtil = new Logitude.Server.Tools.Helpers.AssemblyUtil();
                prodInfo = assemblyUtil.GetProductInfo(typeof(Program).Assembly);
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug(prodInfo);

                Action<bool, bool> BuildObjectTablesZipFilesDataAction = WebFreight.Web.MetaDataUpdate.TenantsUpdateClass.BuildObjectTablesZipFilesData;
                /*CustomsWorkerRole.*/CustomsWorkerEntryPoint.StartStatic(false, BuildObjectTablesZipFilesDataAction, prodInfo, SecurityUtility.CheckContactFeature);

                InjectionUtil.Init(null, null, null, () => (new ByteCompressorUtil()) as IByteCompressorUtil, null, null, null);
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

                Simplog.Server.Infrastructure.LogitudeSettings.HandleLogMe?.Invoke("StartStatic", false, "", DateTime.MaxValue);//problem in the amial windows service debug mode after merge

                CustomsRegistrations.Register();
                InfraRegistrationHelper.Register();
                LoggedContactResolver.RegisterLoggedContactUtil();
                _ThreadStartStaticLoaded = true;

            }
            catch (Exception e)
            {

                NetCommonHelper.Logger.DevLog.Instance.WriteFatal(e);
                if (Environment.UserInteractive)
                {
                    Debug.Fail("StartStatic");
                }
              
                _ThreadStartStaticLoaded = false;
            }
        }

    }

    public abstract class WorkerEntryPoint
    {
        public string ThreadName;
        public bool DebugMode;
        public object DebugObject;
        public int? Tenant;
        private static void EnsureHttpRuntime()
        {
            try
            {
                if (null == _httpRuntime)
                {
                    try
                    {
                        //Monitor.Enter(typeof(State));
                        if (null == _httpRuntime)
                        {
                            // Create an Http Content to give us access to the cache.
                            _httpRuntime = new HttpRuntime();

                        }
                    }
                    finally
                    {
                        //Monitor.Exit(typeof(State));
                    }

                }
            }
            catch (Exception e)
            {
                //Logger.LogMe(e.ToString(), true);
            }
        }
        public static Cache Cache
        {
            get
            {
                try
                {
                    WorkerEntryPoint.EnsureHttpRuntime();
                    return HttpRuntime.Cache;
                }
                catch (Exception e)
                {
                    //Logger.LogMe(e.ToString(), true);
                }
                return null;

            }



        }

        public abstract void StartMe();

        public virtual bool OnStart()
        {
            //StartMe();
            if (LogitudeSettings.IsCostomsDeploy)
            {
                var he = new CultureInfo("he-IL");// '("en-US") '    "he-IL")
                he.DateTimeFormat.DateSeparator = ".";
                he.DateTimeFormat.ShortDatePattern = "dd-MM-yy";// ' "yyyy/MM/dd" '  ' "DD/MM/YYYY"
                System.Threading.Thread.CurrentThread.CurrentCulture = he;


            }
            ThreadName = this.GetType().Name;
            return (true);
        }

        /// <summary>
        /// This method prevents unhandled exceptions from being thrown
        /// from the worker thread.
        /// </summary>
        public void ProtectedRun()
        {
            if (LogitudeSettings.DeploymentStage != "Dev")
            {
                try
                {

                    Run();
                }
                catch (SystemException e)
                {
                    ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole", "General :  Run()1 Method", null);
                    //throw e;

                }
                catch (Exception e)
                {
                    ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole", "General :  Run()2 Method", null);
                    //throw e;
                }
            }

            else
            {
                Run();
            }
        }
        public abstract void WorkOnce();

        public virtual void Run()
        {
        }

        public virtual void OnStop()
        {
        }

        public static HttpRuntime _httpRuntime { get; set; }
    }
    public abstract class CustomsWorkerEntryPoint : WorkerEntryPointDoneLog
    {
        public override void StartMe()
        {
            if (CacheManager.CacheWrapper != null) return;
            CustomsWorkerEntryPoint.StartStatic();
        }
        public static void StartStatic(bool suppressCache = false, Action<bool, bool> BuildObjectTablesZipFilesDataAction = null, string prodInfo = null,
            Action<string, string, int, string> checkContactFeature = null
            )
        {
            if (suppressCache)
            {
                CacheManager.CacheWrapper = new NoCache4uWrapper();
            }
            else
            {
                CacheManager.CacheWrapper = new CacheWrapper(//HttpContext.Current.Cache
            Cache
            );
            }


            ThreadedRoleEntryPointStartStatic(BuildObjectTablesZipFilesDataAction, prodInfo);
            try
            {
                var repo = new CustomsSettingRepository(1);
                WorkerRoleServiceLocator.HaveCourierTenant = repo.GetRealAll().Any(r => r.CompanyType == "B");
            }
            catch (Exception)
            {

                //throw;
            }

            InjectionUtil.Init(null, null, checkContactFeature, () => (new ByteCompressorUtil()) as IByteCompressorUtil, null, null, null);
            //ProxyUtil.SecurityUtilityCheckFeature = SecurityUtility.CheckFeature;

            //string storageServiceMode = ConfigurationManager.AppSettings.Get("StorageServiceMode");
            //ContainerAccessor.InitContainer(storageServiceMode);

            MessagingServiceFactoryHelper.InitContainer();
            //if (!ContainerAccessor.Container.IsRegistered<IMessagingServiceInterfaceType>("2715"))
            //{
            //    ExceptionHandler.HandleException(null, DateTime.Now, 0, "", "WorkerRole", "CustomsMessagingSheetWR: ProcessMessage():!ContainerAccessor.Container.IsRegistered :analyzeClass=2715", null);
            //    //message.DeadLetter();
            //    ///return;
            //}
        }

        public static void ThreadedRoleEntryPointStartStatic(Action<bool, bool> BuildObjectTablesZipFilesDataAction = null, string ProductInfo = null)
        {
            if (string.IsNullOrEmpty(LogitudeSettings.DeploymentStage))
            {
                string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
                LogitudeSettings.DatabaseManagementSystem = dbms;
                LogitudeSettings.DebugKey = System.Configuration.ConfigurationManager.AppSettings.Get("DebugKey");
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
                LogitudeSettings.IOSSharedAppMinimumVersion = setting.IOSSharedAppMinimumVersion;
                LogitudeSettings.WorkEnvironment = setting.WorkEnvironment;
                LogitudeSettings.LogoCode = setting.LogoCode;
                LogitudeSettings.EnableHybridQueue = setting.EnableHybridQueue;

                //LogitudeSettings.IsCostomsDeploy = Logitude.Customs.BL.Utils.CustomsSettingUtil.ForceDownloadXapFromIIS();
                //LogitudeSettings.GetUnfDBConnectionInfoFromTenantInject = CustomsSettingQueryService.GetUnfDBConnectionInfo;
                LogitudeSettings.GetLogitudeCustomsSettingsMInject = CustomsSettingQueryService.GetLogitudeCustomsSettingsM;

                LogitudeSettings.HandleDbExceptionInject = ExceptionHandler.HandleDbException;
                LogitudeSettings.HandleBuildObjectTablesZipFilesData_Inject = BuildObjectTablesZipFilesDataAction;
                LogitudeSettings.GetUserNameInject = AuthenticationUtil.ResolveUserIdentityName;

                LogitudeSettings.StorageServiceMode = setting.StorageServiceMode;
                LogitudeSettings.QueueServiceMode = setting.QueueServiceMode;
                LogitudeSettings.ABMProductId = setting.ABMProductId;
                LogitudeSettings.AzureFolderName = setting.AzureFolderName;
                LogitudeSettings.CPUIntensiveWebServicesURL = setting.CPUIntensiveWebServicesURL;


            }

            //  CommunicationWorkerRole.ThreadedRoleEntryPoint.SetWorkerRoleName();


            if (LogitudeSettings.IsCostomsDeploy)
            {
                LogitudeSettings.ProductInfo = ProductInfo;

                var he = new CultureInfo("he-IL");// '("en-US") '    "he-IL")
                he.DateTimeFormat.DateSeparator = ".";
                he.DateTimeFormat.ShortDatePattern = "dd-MM-yy";// ' "yyyy/MM/dd" '  ' "DD/MM/YYYY"
                System.Threading.Thread.CurrentThread.CurrentCulture = he;

                LogitudeSettings.HandleLogMe = new Action<string, bool, string, DateTime>((mess, err, suffix, stopLogAt) =>
                {
                    if (DateTime.Now > stopLogAt) return;
                    if (err)
                    {
                        NetCommonHelper.Logger.DevLog.Instance.WriteError(mess+suffix);
                    }
                    else
                    {
                        NetCommonHelper.Logger.DevLog.Instance.WriteDebug(mess+ suffix);
                    }
                   
                });

                LogitudeSettings.RunWorkerRoleAutomaticBreakPoint = false;

                LogitudeSettings.WorkerRoleName = LogitudeSettings.WorkerRoleName ?? "production";
            }
            // string storageServiceMode = System.Configuration.ConfigurationManager.AppSettings.Get("StorageServiceMode");
            //string queueServiceMode = System.Configuration.ConfigurationManager.AppSettings.Get("QueueServiceMode");
            ContainerAccessor.InitContainer();



        }
    }


}
