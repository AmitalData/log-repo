using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class CustomsEnvironmentSettingQueryService : EntityQueryService<CustomsEnvironmentSetting, CustomsEnvironmentSettingKeys, CustomsEnvironmentSettingPM, object, CustomsEnvironmentSettingKeys>
    {
        public CustomsEnvironmentSettingPM GetEnvironmentSettingPM()
        {
            string entityKeyString = "GetCustomsEnvironmentSettingPM" ;
            CustomsEnvironmentSettingPM settingPM = CacheManager.GetOrInsertNewObject<CustomsEnvironmentSettingPM>(entityKeyString, () =>
            {
                var poco = this.repository.GetAll().FirstOrDefault();
                if (poco is null)
                {
                    return null;
                }
                return this.GetEntityPM(poco);    
                 ;
            });

            return settingPM;




        }
    }
}
