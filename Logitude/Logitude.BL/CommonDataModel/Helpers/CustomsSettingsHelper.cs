using Logitude.Customs.Data.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Data;

namespace Logitude.BL.CommonDataModel.Helpers
{
    internal class CustomsSettingsHelper
    {
        public static CustomsSetting GetCache(int tenant) =>
            CacheManager.GetOrInsertNewObject<CustomsSetting>("customsStetting" + tenant, () =>
                CustomContext.GetContext(tenant).CustomsSettings.FirstOrDefault(r => r.Tenant == tenant));        
    }
}
