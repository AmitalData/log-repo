using Logitude.Customs.Data.Repsitories;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Linq;
using System.Web.Caching;

namespace CustomsWorkerRole
{
    public abstract class CustomsWorkerEntryPoint : WorkerEntryPointDoneLog
    {
        public override void StartMe()
        {
            if (CacheManager.CacheWrapper != null) return;
            CustomsWorkerEntryPoint.StartStatic();
        }
        public static void StartStatic(bool suppressCache = false, Action<bool, bool> BuildObjectTablesZipFilesDataAction=null,string prodInfo = null,
            Action<string, string, int, string> checkContactFeature= null
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

            
            ThreadedRoleEntryPoint.StartStatic(BuildObjectTablesZipFilesDataAction, prodInfo);
            try
            {
                var repo = new CustomsSettingRepository(1);
                WorkerRoleServiceLocator.HaveCourierTenant = repo.GetRealAll().Any(r => r.CompanyType == "B");
            }
            catch (Exception)
            {

                //throw;
            }
           
            InjectionUtil.Init(null, null, checkContactFeature, () => (new ByteCompressorUtil()) as IByteCompressorUtil, null,null,null);
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

     
    }



}