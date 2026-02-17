using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityPOCOs;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return  new LogitudeCustomsSettingsM(){
                 UnfConnectionString =customsSettingPM.UnfConnectionString,
                 OnPremiseFillingService = customsSettingPM.OnPremiseFillingService,
                 IsConnectedToUniFreight = customsSettingPM.IsConnectedToUniFreight,
                 
            };
        }
        public static CustomsSettingPM GetSettingByTenant(int tenant)
        {
            CustomsSettingPM settingPM = null;
            
            
            string entityKeyString = "CustomsSettingQueryService:GetSettingByTenant" + "_" + tenant.ToString() ;
            settingPM =CacheManager.GetOrInsertNewObject<CustomsSettingPM>(entityKeyString, () =>
            {
                var qs = new CustomsSettingQueryService(tenant);
                var settingPoco = qs.repository.GetSettingByTenant(tenant);
                settingPM = qs.GetEntityPM(settingPoco);
             //   settingPM.IsConnectedToUniFreight = false;
                return settingPM ;
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
            var allPocos= repository.GetRealAll().ToList();
            var allPMs = allPocos.Select(rec => GetEntityPM(rec)).ToList();
            return allPMs;
        }

           
          
        public List<CustomsSetting> GetAll(int tenant)
        {
            return repository.GetAll(tenant).ToList();
        }
        public CustomsSettingPM GetSettingByTenantN(int tenant, bool fromCache = true)
        {
            //CustomsSetting setting = repository.GetSettingByTenant(tenant);
            //CustomsSettingPM settingPM = new CustomsSettingPM()
            //{
            //    Id = setting.Id,
            //    IsConnectedToUniFreight = setting.IsConnectedToUniFreight,
            //};
            string entityKeyString = "GetSettingByTenantN," + tenant.ToString() ;

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

}
