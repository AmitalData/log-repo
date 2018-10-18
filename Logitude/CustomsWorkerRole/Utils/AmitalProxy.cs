using Logitude.Customs.BL.EntityQueryServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomsWorkerRole.Utils
{
    public static class AmitalProxy
    {
        public static bool IsConnectedToUniFreight()
        {
            
            var pm = CustomsSettingQueryService.GetSettingByTenant(1);
            if (pm != null && pm.IsConnectedToUniFreight) return true;
            return false;
        }

        public static bool Have_UnfConnectionString()
        {

            var pm = CustomsSettingQueryService.GetSettingByTenant(1);
            if (pm != null && !String.IsNullOrWhiteSpace(pm.UnfConnectionString)) return true;
            return false;
        }
    }
}
