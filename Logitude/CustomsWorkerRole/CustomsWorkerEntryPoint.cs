using Logitude.Customs.Data.Repsitories;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.TreeFilterQuery;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Caching;
using WebFreight.Web.GlobalModel;

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
                Dictionary<int, string> globalDBs = new Dictionary<int, string>();
                List<GlobalTenant> globalTenants = new GlobalDomainService().GetAllTenants();
                foreach (var item in globalTenants)
                {
                    globalDBs.Add(item.Id, item.GlobalDBId);
                }
                CacheManager.CacheWrapper = new CacheWrapper(//HttpContext.Current.Cache
            Cache,globalDBs
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
           
            InjectionUtil.Init(null, null, checkContactFeature, () => (new ByteCompressorUtil()) as IByteCompressorUtil, null,null,null, null, () => (new TreeFilterQueryService()) as ITreeFilterQueryService);

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