using AmitalCloud.Infrastructure.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using UnifreightIIG.Common.Utils;

namespace AmitalCloud.Infrastructure.APITools.ExternalServices
{
    public static partial class FactoryServicePool<T>
         where T : class
    {
        private static List<StateServiceModel> _StateServiceModelList;
        [ThreadStatic]
        private static Random _Random;
        private static readonly object lockObject = new object();
        static FactoryServicePool()
        {
            _StateServiceModelList = new List<StateServiceModel>();
            _Random = new Random();
        }
        internal static void Use(ServiceTypeEnum serviceType, int tenant, Action<T> proxyMethodAction)
        {
            var curStateServiceModel = GetTheBestStateServiceModel(serviceType, tenant);
            UseService(curStateServiceModel.ExternalService, proxyMethodAction);
        }
        private static void UseService(ExternalServicePM externalService, Action<T> proxyMethodAction)
        {
            var factory = new ServiceWrapper<T>(externalService.ServiceAddressUrl, externalService.TimeoutInSec);
            LogMessagingUtil.Instance.AppendLine("Start " + typeof(T).Name + " =" + externalService.ServiceAddressUrl);
            var proxy = (IClientChannel)factory.GetChannel<T>();
            var success = false;
            var sw = Stopwatch.StartNew();
            try
            {
                using (proxy)
                {
                    proxyMethodAction((T)proxy);
                }
                success = true;
                LogMessagingUtil.Instance.AppendLine(typeof(T).Name + " Success:took:" + sw.Elapsed.ToString());
            }
            finally
            {
                if (!success)
                {
                    LogMessagingUtil.Instance.AppendLine(typeof(T).Name + " Failed");
                    proxy.Abort();
                }
                Feedback(externalService.ServiceAddressUrl, success);
            }
        }
        private static StateServiceModel GetTheBestStateServiceModel(ServiceTypeEnum serviceType, int tenant)
        {
            StateServiceModel theBestStateServiceModel = null;
            try
            {
                var allTenantService = _StateServiceModelList.Where(rec =>
                    rec.ExternalService.Tenant == tenant &&
                    rec.ExternalService.ServiceType == serviceType
                    ).ToList();
                if (allTenantService.Count < 1)
                {
                    allTenantService = AddFromDB(serviceType, tenant);
                }
                if (allTenantService.Count < 1)
                {
                    throw new Exception("No Defination for serviceType=" + serviceType.ToString() + ":Tenant=" + tenant);
                }
                int itemAt;
                var succList = allTenantService.Where(rec => rec.IsLastUsedSuccess).ToList();
                if (succList.Count == 0)
                {
                    LogMessagingUtil.Instance.AppendLine("No good Service  found  type:" + serviceType.ToString());
                    LogMessagingUtil.Instance.AppendLine("All Service  Are:" +
                        allTenantService.Select(rec => rec.ExternalService.ServiceAddressUrl).Aggregate(
                        (rec1, rec2) => { return rec1 + "," + rec2; }));
                    itemAt = GetRandom(allTenantService.Count);
                    theBestStateServiceModel = allTenantService.Skip(itemAt).Take(1).FirstOrDefault();
                    AddFromDB(serviceType, tenant);//LoadAllAgain !!
                }
                else
                {
                    if (String.IsNullOrWhiteSpace("The Best it to repeat work  with one All The time !!"))
                    {
                        theBestStateServiceModel = succList.FirstOrDefault();
                    }
                    else
                    {
                        itemAt = GetRandom(succList.Count);
                        theBestStateServiceModel = succList.Skip(itemAt).Take(1).FirstOrDefault();
                    }
                }
            }
            finally
            {
                if (theBestStateServiceModel == null)
                {
                    theBestStateServiceModel = _StateServiceModelList.First();
                }
            }
            return theBestStateServiceModel;
        }
        private static List<StateServiceModel> AddFromDB(ServiceTypeEnum serviceType, int tenant)
        {
            var queryService = new ExternalServicesRepository();
            var externalServiceS = queryService.GetAll(tenant, serviceType);
            if (externalServiceS.Count < 1)
            {
                throw new Exception("no ExternalServicesQueryService for tenant" + tenant.ToString() + "  serviceType:" + serviceType.ToString());
            }
            lock (lockObject)
            {
                _StateServiceModelList.RemoveAll(rec => rec.ExternalService.Tenant == tenant & rec.ExternalService.ServiceType == serviceType);
                foreach (var item in externalServiceS)
                {
                    _StateServiceModelList.Add(new StateServiceModel() { ExternalService = item });
                }
            }
            return _StateServiceModelList.Where(rec =>
                    rec.ExternalService.Tenant == tenant &&
                    rec.ExternalService.ServiceType == serviceType
                    ).ToList();
        }
        static int GetRandom(int max)
        {
            if (_Random == null)
            {
                var seed = (int)DateTime.Now.Ticks;
                _Random = new Random(seed);
            }
            var itemAt = _Random.Next(max);
            if (itemAt < 0)
            {
                itemAt = 0;
            }
            return itemAt;
        }
        internal static void ReloadMeWhenRelayServiceDBChange()
        {
        }
        private static void Feedback(string ServiceAddressUrl, bool success)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    NetCommonHelper.Logger.DevLog.Instance.WriteDebug(ServiceAddressUrl + (success ? ":success" : ":Failed"));
                    lock (lockObject)
                    {
                        var item = _StateServiceModelList.FirstOrDefault(rec => rec.ExternalService.ServiceAddressUrl == ServiceAddressUrl);
                        item.LastUsedAt = DateTime.Now;
                        item.IsLastUsedSuccess = success;
                    }
                }
                catch (Exception)
                {
                }
            });
        }
    }
}
