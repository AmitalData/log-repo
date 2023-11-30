using AmitalCustomsWindowsService.Tester;
using AmitalCustomsWindowsService.Utils;
using CustomsWorkerRole.BL;
using Logitude.BL.Helpers;
using Logitude.BL.Resolvers;
using Logitude.Customs.BL.EntityQueryServiceExt;
using Logitude.Customs.BL.Validators;
using Logitude.Customs.Def.EntityQueryServicesExt;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.TreeFilterQuery;
using Logitude.Server.Tools.Utils;
using Microsoft.Practices.Unity;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Unifreight.Data.AmitalModel;
using WebFreight.Web.CustomModel;
using WebFreight.Web.Security;

namespace AmitalCustomsWindowsService
{
    //TEST !!
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        //static void Main1()
        //{
        //    ServiceBase[] ServicesToRun;
        //    ServicesToRun = new ServiceBase[]
        //    {
        //        new MyWinService()
        //    };
        //    ServiceBase.Run(ServicesToRun);
        //}

        [STAThread]
        static void Main()
        {
            
            bool test = false;
            if (test)
            {
                //int i=CustomsWorkerRole.Utils.GenUtil.GetQueueTimeOutInMin();
                //CustomsWorkerRole.Test.clsTester.CheckCustomsContext();
                (new Oracle2SQL())
                    //.CreateCustomsContext();
                    //.GetReNameLongTable(root: @"C:\log2004\Logitude\");
                    .GetReNameSchemaCustoms(root: @"C:\log2004\Logitude\");

                //.GetReNameLongColumns(root: @"C:\log2004\Logitude\");
                //.ChangeToBit();
                //(new CustomsWorkerRole.Test.clsTester()).CheckCustomContext();

            }

            //ThreadPool.SetMinThreads(400, 400);
            ServiceBase[] ServicesToRun;

            // More than one user Service may run within the same process. To add
            // another service to this process, change the following line to
            // create a second service object. For example,
            //
            //   ServicesToRun = new ServiceBase[] {new Service1(), new MySecondUserService()};
            //


            //var aa = Assembly.GetExecutingAssembly().GetName().Version.ToString();

            //GatewayService.TestXmlDF_MSG10000_ImportDeclaration(@"D:\Source\2012\UnifreightIIG\UnifreightIIG.ServerTester\UnifreightIIG.ServerTester\IIGProxys\ImportDeclaration\SaveDF_MSG2750_2754_ImportDeclarationRequest-309925709-7788.xml");
            

            Debug.WriteLine("AmitalCustomsWindowsService !!!...");
            Logger.LogMe("AmitalCustomsWindowsService", false);
            Logger.LogMe("Environment.UserInteractive" + Environment.UserInteractive.ToString(), false);

            //TestSystemTable();
            //ThreadStartStatic();

            if (System.Environment.CommandLine.EndsWith("TesterForm", StringComparison.OrdinalIgnoreCase))
            {
                Logger.LogMe("Debugger", false);
                Program.ThreadStartStaticIsMustB4UsingTheDB();
                System.Windows.Forms.Application.Run(new AmitalCustomsWindowsService.Tester.TesterForm());
                return;
            }
            if (System.Environment.CommandLine.ToUpper().EndsWith("TST"))
            {
                Logger.LogMe("Debugger", false);

                ServiceBase d = GetMyService();
                //CustomExportService.BaseAddress = System.Configuration.ConfigurationSettings.AppSettings["baseAddress"].ToString();
                (d as IServiceStartMe).StartMe();

                System.Windows.Forms.MessageBox.Show("debug mode");
                //System.Windows.Forms.MessageBox.Show("Click to end service");
                System.Windows.Forms.Application.Run();


            }
            else
            {
                Logger.LogMe("Runtime", false);
                ServicesToRun = new ServiceBase[] { GetMyService() /*new MyWinService()*/ };
                ServiceBase.Run(ServicesToRun);
            }

        }

        private static ServiceBase GetMyService()
        {
            if (!String.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["LoadTestWService"]))
            {
                return new LoadTestWService();
            }
            
            return new AmitalCustomTolerantWindowsService();

            //<add key="TolerantWindowsService" value="1" />
            //<add key="RestartThreadsEveryInMin" value="90" />


            if (!String.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["Suppress_TolerantWindowsService"]))
            //if (String.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["TolerantWindowsService"]))
            {
                return (new MyWinService() as ServiceBase);
            }
            else
            {
                return new AmitalCustomTolerantWindowsService();

            }
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
                Logger.LogMe(prodInfo, false);

                Action<bool, bool, bool> BuildObjectTablesZipFilesDataAction = WebFreight.Web.MetaDataUpdate.TenantsUpdateClass.BuildObjectTablesZipFilesData;
                CustomsWorkerRole.CustomsWorkerEntryPoint.StartStatic(false, BuildObjectTablesZipFilesDataAction, prodInfo, SecurityUtility.CheckContactFeature);

                InjectionUtil.Init(null, null, null, () => (new ByteCompressorUtil()) as IByteCompressorUtil, null,null,null, null , () => (new TreeFilterQueryService()) as ITreeFilterQueryService);
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
                    
                var serverMonitorControlService = new ServerMonitorControlService();
                serverMonitorControlService.StopProccessIfNotExist();
                
                _ThreadStartStaticLoaded = true;
                
            }
            catch (Exception e)
            {

                Logger.LogMe(e.ToString(), true);
                if (Environment.UserInteractive)
                {
                    Debug.Fail("StartStatic");
                }
                Logger.LogMe(e.ToString(), true);
                _ThreadStartStaticLoaded = false;
            }
        }
    }
}
