using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityPOCOs;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Def.EntityQueryServicesExt;
using Logitude.Customs.BL.Messaging.Customs.SignQueueBL;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class CustomsSettingQueryService
    {
        //static Dictionary<int,CustomsSettingPM> _CustomsSettingCache = new Dictionary<int,CustomsSettingPM>();
        public static string GetUnfDBConnectionInfo(int tenant)
        {
            CustomsSettingPM customsSettingPM = CustomsSettingQueryService.GetSettingByTenant(tenant);

            //var customsSettingQueryService = new CustomsSettingQueryService(tenant);
            //customsSettingPM = customsSettingQueryService.GetSettingByTenantN(tenant);
            if (customsSettingPM == null)
            {
                throw new Exception("GetSettingByTenant(tenant) ==null");
            }
            return customsSettingPM.UnfConnectionString;
        }

        public static LogitudeCustomsSettingsM GetLogitudeCustomsSettingsM(int tenant)
        {
            CustomsSettingPM customsSettingPM = CustomsSettingQueryService.GetSettingByTenant(tenant);

            //var customsSettingQueryService = new CustomsSettingQueryService(tenant);
            //customsSettingPM = customsSettingQueryService.GetSettingByTenantN(tenant);
            if (customsSettingPM == null)
            {
                throw new Exception("GetSettingByTenant(tenant) ==null");
            }
            return new LogitudeCustomsSettingsM()
            {
                UnfConnectionString = customsSettingPM.UnfConnectionString,
                OnPremiseFillingService = customsSettingPM.OnPremiseFillingService,
                IsConnectedToUniFreight = customsSettingPM.IsConnectedToUniFreight,

            };
        }
        public static CustomsSettingPM GetSettingByTenant(int tenant)
        {
            CustomsSettingPM settingPM = null;


            string entityKeyString = "CustomsSettingQueryService:GetSettingByTenant" + "_" + tenant.ToString();
            settingPM = CacheManager.GetOrInsertNewObject<CustomsSettingPM>(entityKeyString, () =>
            {
                var qs = new CustomsSettingQueryService(tenant);
                var settingPoco = qs.repository.GetSettingByTenant(tenant);
                settingPM = qs.GetEntityPM(settingPoco);
                //   settingPM.IsConnectedToUniFreight = false;
                return settingPM;
            });
            //settingPM.IsConnectedToUniFreight = false;
            return settingPM ?? new CustomsSettingPM() { Tenant = tenant };

            if (tenant < 0) //dummy tenant place holder can not be less than zero
            {
                return new CustomsSettingPM() { Tenant = tenant };
            }

            settingPM = CacheManager.CacheWrapper.Get(entityKeyString) as CustomsSettingPM;


            if (settingPM == null)
            {


                var qs = new CustomsSettingQueryService(tenant);
                var settingPoco = qs.repository.GetSettingByTenant(tenant);
                settingPM = qs.GetEntityPM(settingPoco);

                if (settingPM != null)
                {
                    CacheManager.CacheWrapper.Insert(entityKeyString, settingPM);
                }
                else
                {
                    CacheManager.CacheWrapper.Insert(entityKeyString, new NullCache());
                }
            }

            return settingPM;
        }



        public List<CustomsSettingPM> GetAll()
        {
            var allPocos = repository.GetRealAll().ToList();
            var allPMs = allPocos.Select(rec => GetEntityPM(rec)).ToList();
            return allPMs;
        }



        public List<CustomsSetting> GetAll(int tenant)
        {
            return repository.GetAll(tenant).ToList();
        }
        public bool IsCourierTenant(int tenant)
        {
            var pm = GetSettingByTenantN(tenant, fromCache: true);
            return (pm.CompanyType == "B");


        }
        public CustomsSettingPM GetSettingByTenantN(int tenant, bool fromCache = true)
        {
            //CustomsSetting setting = repository.GetSettingByTenant(tenant);
            //CustomsSettingPM settingPM = new CustomsSettingPM()
            //{
            //    Id = setting.Id,
            //    IsConnectedToUniFreight = setting.IsConnectedToUniFreight,
            //};
            string entityKeyString = "GetSettingByTenantN," + tenant.ToString();

            var pm = CacheManager.GetOrInsertNewObject<CustomsSettingPM>(
                entityKeyString,
                () =>
                {
                    var qs = new CustomsSettingQueryService(tenant);
                    var settingPoco = qs.repository.GetSettingByTenant(tenant);
                    return GetEntityPM(settingPoco);
                },
                fromCache);




            return pm;
        }

        public CustomsSettingPM GetTenantByCustomsAgentId(string customsAgentId)
        {
            string entityKeyString = "GetTenantByCustomsAgentId," + customsAgentId;

            var pm = CacheManager.GetOrInsertNewObject<CustomsSettingPM>(
                entityKeyString,
                () =>
                {
                    var settingPoco = this.repository.GetTenantByCustomsAgentId(customsAgentId);
                    return GetEntityPM(settingPoco);
                });

            return pm;
        }

        public bool IsHSMSign_IsOn(int tenant)
        {
            var customsEnvironmentSettingQueryService = new CustomsEnvironmentSettingQueryService(tenant);
            var environmentSettingPM = customsEnvironmentSettingQueryService.GetEnvironmentSettingPM();
            if (environmentSettingPM==null)
            {
                return false;
            }
            var setting = this.GetSettingByTenantN(tenant);

            var signQueueHSMService = new SignQueueHSMService();
            bool hasValidHsm = signQueueHSMService.GetHSMAllCertificates(tenant, false).Any(i => i.IsOk == true);

            bool fromEnvSetting =
                !string.IsNullOrEmpty(environmentSettingPM.HSMActiveCertUrl) &&
                !string.IsNullOrEmpty(environmentSettingPM.HSMSignServiceUrl) &&
                !string.IsNullOrEmpty(environmentSettingPM.HSMToken) &&
                !string.IsNullOrEmpty(environmentSettingPM.HSMSignProcess)

                ;
            bool fromTenantSetting =
                !string.IsNullOrEmpty(setting.HSMCompanyId) &&
                !string.IsNullOrEmpty(setting.HSMToken);
            return fromEnvSetting && fromTenantSetting && hasValidHsm;
        }

        public CustomsSettingPM GetSingleByTenant(int tenant)
        {
            var poco = repository.GetSettingByTenant(tenant);


            return GetEntityPM(poco);

        }


        internal CustomsSettingPM GetSettingPMByCustomsAgentId(string CustomsAgentId)
        {
            string entityKeyString = "GetSettingPMByCustomsAgentId," + CustomsAgentId;
            var pm = CacheManager.GetOrInsertNewObject<CustomsSettingPM>(entityKeyString, () =>
            {
                var poco = repository.GetRealAll().FirstOrDefault(rec => rec.CustomsAgentId == CustomsAgentId);
                return GetEntityPM(poco);
            });
            return pm;
        }


        public List<TenantM> GetTenantDetailsMessagesPMs(
            Func<int, string> getDcaFilterByEnvironment,
            Func<int, bool> IsSuppressDca
            )
        {
            List<TenantM> tenantMs = new List<TenantM>();
            var poco = repository.GetRealAll().ToList();
            foreach (var item in poco)
            {
                TenantM tenantM = new TenantM();
                tenantM.CustomsAgentId = item.CustomsAgentId;
                tenantM.HaveFeature = item.IsMessagesPending;
                tenantM.TenantId = item.Tenant;
                tenantM.IIGServiceAddress = item.IIGServiceAddress;
                tenantM.QtyFeedbackInPendingMessage = Convert.ToInt32(item.QtyFeedbackInPendingMessage);
                tenantM.DCAPartnerVault = item.DCAPartnerVault;
                tenantM.DcaFilterByEnvironment ="";
                //if (item.IsMessagesPending.GetValueOrDefault() &&
                //    !String.IsNullOrWhiteSpace(item.IIGServiceAddress) &&
                //    !String.IsNullOrWhiteSpace(item.DCAPartnerVault))
                {
                    string myDcaFilterByEnvironment = getDcaFilterByEnvironment(item.Tenant);
                    tenantM.DcaFilterByEnvironment = myDcaFilterByEnvironment;
                }
                tenantM.SuppressDCA_FileFree = IsSuppressDca(item.Tenant);
                tenantMs.Add(tenantM);
            }
            //var allPMs = poco.Select(rec => GetEntityPM(rec)).ToList();

            return tenantMs;

        }


        public DateTime? GetLastRunningDCAWS(int tenant)
        {
            var poco = repository.GetSettingByTenant(tenant);

            return poco.LastRunningDCAWS;

        }



    }


    public class TenantM
    {
        public string CustomsAgentId { get; internal set; }
        public bool? HaveFeature { get; internal set; }
        public int TenantId { get; internal set; }
        public int QtyFeedbackInPendingMessage { get; internal set; }
        public string IIGServiceAddress { get; internal set; }
        public string LastActionLog { get; set; }
        public string DCADownloadFolder { get; internal set; }
        public string DCAPartnerVault { get; internal set; }
        
        public string DcaFilterByEnvironment { get; internal set; }

        
        public bool SuppressDCA_FileFree { get; internal set; }
    }

#if false
    public class CustomsSettingQService
    {
        public string GetUnfDBConnectionInfo(int tenant)
        {
            CustomsSettingPM customsSettingPM = CustomsSettingQueryService.GetSettingByTenant(tenant);

            //var customsSettingQueryService = new CustomsSettingQueryService(tenant);
            //customsSettingPM = customsSettingQueryService.GetSettingByTenantN(tenant);
            if (customsSettingPM == null)
            {
                throw new Exception("GetSettingByTenant(tenant) ==null");
            }
            return customsSettingPM.UnfConnectionString;
        }   
    }
#endif

    public class DICustomsSettingQueryService : IDICustomsSettingQueryService
    {

        public bool IsCourierTenant(int tenant)
        {
            var customsSettingQueryService = new CustomsSettingQueryService(tenant);
            return customsSettingQueryService.IsCourierTenant(tenant);
        }
    }

}
