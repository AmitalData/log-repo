
//using Microsoft.WindowsAzure.ServiceRuntime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Logitude.SystemLogs;
using CustomsWorkerRole.DCA;
using Microsoft.ServiceBus.Messaging;
using System.IO;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Azure;
using Logitude.Server.Tools;
using Logitude.CustomsMessaging.MessagingServices;
using Microsoft.Practices.Unity;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.Messaging.Customs;
using System.Diagnostics;
using Logitude.CustomsMessaging.Dca;



namespace CustomsWorkerRole
{
    public //public for E:\users\itzik\documents\visual studio 2012\Projects\CustomsWorkerRoleTester\CustomsWorkerRoleTester
        class DownloadDcaMessageSheetWR : CustomsWorkerEntryPoint
    {


        private readonly int _SeedDefaultTenant;
        private bool _OnStartDone;
        private List<Logitude.Customs.Def.EntityPMs.CustomsSettingPM> _AllCustomsSetting;

        public DownloadDcaMessageSheetWR()
        {

            _SeedDefaultTenant = 1;
            _SeedDefaultTenant = 0;
        }
        public override void Run()
        {

            while (!WorkerRoleServiceLocator.PleaseShutDown)
            {

                if (!General.IsUpdating())
                {

                    
                    try
                    {
                        WorkOnce();
                        Thread.Sleep(TimeSpan.FromSeconds(2)); 
                    }
                    catch (Exception e)
                    {
                        ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole", "DownloadDcaMessageSheetWR : Run() Method", null);
                        Thread.Sleep(TimeSpan.FromMinutes(1)); ///+1 MIN
                    }
                }


                //Thread.Sleep(TimeSpan.FromMinutes(1));
                

                
            }

        }

      
   
        public override bool OnStart()
        {
            if (_OnStartDone) return true;
            _OnStartDone = true;
            DoneItemsInRange = new Dictionary<DateTime, int>();
            MessagingServiceFactoryHelper.InitContainer();
            if (!ContainerAccessor.Container.IsRegistered<IMessagingServiceInterfaceType>("190"))
            {
                ExceptionHandler.HandleException(null, DateTime.Now, 0, "", "WorkerRole", "CustomsMessagingSheetWR: ProcessMessage():!ContainerAccessor.Container.IsRegistered :analyzeClass=2715", null);
                //message.DeadLetter();
                ///return;
            }

            


            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;


            //DiagnosticMonitor.Start("DiagnosticsConnectionString");

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.
            //RoleEnvironment.Changing += RoleEnvironmentChanging;

            return base.OnStart();
        }

        //private void RoleEnvironmentChanging(object sender, RoleEnvironmentChangingEventArgs e)
        //{
        //    // If a configuration setting is changing
        //    if (e.Changes.Any(change => change is RoleEnvironmentConfigurationSettingChange))
        //    {
        //        // Set e.Cancel to true to restart this role instance
        //        e.Cancel = true;
        //    }
        //}
        static DateTime _LastActiveAt;
        static DateTime _LastReadAllCustomsSetting;
        
        public override void WorkOnce()
        {
            OnStart();
            if (DateTime.Now.Subtract(_LastActiveAt) < TimeSpan.FromSeconds(10))
            {
                Thread.Sleep(TimeSpan.FromSeconds(2));
                Debug.WriteLine("do not disturb the DCAServer Wait 10 sec ");
                return;
            }
            _LastActiveAt = DateTime.Now;
            if (DateTime.Now.Subtract(_LastReadAllCustomsSetting) > TimeSpan.FromMinutes(20))//cache 20 min
            {
                //CustomsWorkerRole.Utils.GenUtil.CollectGC();
                _LastReadAllCustomsSetting = DateTime.Now;
                ///_AllCustomsSetting.Clear();
                _AllCustomsSetting = null;
            }
            if (_AllCustomsSetting == null)
            {
                var customsSettingQueryService = new CustomsSettingQueryService(_SeedDefaultTenant);
                _AllCustomsSetting = customsSettingQueryService.GetAll();
            }



            var debugIIGMessageId = "";
            var debugTenant = this.Tenant;
            if (this.DebugObject != null)
            {
                debugIIGMessageId = this.DebugObject.ToString();

            }

            var costomSettingDCAList = _AllCustomsSetting.Where(env => !String.IsNullOrEmpty(env.DCAServiceAddress));
            if (debugTenant.HasValue && debugTenant.GetValueOrDefault() >= 0)
            {
                costomSettingDCAList = costomSettingDCAList.Where(rec => rec.Tenant == debugTenant.GetValueOrDefault());
            }
            var sw = Stopwatch.StartNew();
            //var suppressTest = false;
            foreach (var costomSetting in costomSettingDCAList)
            {
                try
                {
                    LastActivity = DateTime.UtcNow;
                    var myDcaService = new DcaDownloadTenantService(costomSetting);

                    myDcaService.SetLastActivity = () =>
                    {
                        this.LastActivity = DateTime.UtcNow;
                    };
                    myDcaService.LogDoneItemInMemoryAction = this.LogDoneItemInMemory;
                    myDcaService.DownloadAll(debugIIGMessageId);
                }
                catch (Exception e)
                {

                    ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole", "DownloadDcaMessageSheetWR :DownloadAll" + costomSetting.DCAPartnerVault, null);
                    Thread.Sleep(TimeSpan.FromMinutes(1));
                }

            }
            SleepTil1Min(sw);

        }

        private static void SleepTil1Min(Stopwatch sw)
        {
            var ts = sw.Elapsed;
            sw.Stop();
            if (ts < TimeSpan.FromMinutes(1))
            {
                //Thread.Sleep(TimeSpan.FromMinutes(1).Subtract(ts));
                Thread.Sleep(TimeSpan.FromSeconds(5));
            }
        }
    }
}
