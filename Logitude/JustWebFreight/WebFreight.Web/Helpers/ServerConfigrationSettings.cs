using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers
{
    public static class ServerConfigrationSettings
    {
        public static Setting GetSettings()
        {
            SettingRepository settingRepository = new SettingRepository();
            Setting setting;
            string id = "1";
            string key = "Setting" + id;
            if (HttpContext.Current != null)
            {
                if (CacheManager.CacheWrapper.Get(key) == null)
                {
                    setting = settingRepository.GetSingleSetting(id);

                    CacheManager.CacheWrapper.Insert(id, setting, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);

                }
                else
                {
                    setting = (Setting)CacheManager.CacheWrapper.Get(key);
                  
                }

            }
            else
            {
                setting = settingRepository.GetSingleSetting(id);
            }

            return setting;
        }
    }
}