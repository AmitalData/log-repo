using Logitude.DashboardModule.BL.EntityPMs;
using Logitude.DashboardModule.Data.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.DashboardModule.BL.EntityQueryServices
{
    public partial class DashboardsUserSettingQueryService
    {
        public DashboardsUserSettingPM GetDashboardsUserSettingsByUserId(string userId, int tenant)
        {
            DashboardsUserSetting DashboardsUserSetting = repository.GetPinnedDashboardsByUserId(userId, tenant);
            if (DashboardsUserSetting == null) return null;

            DashboardsUserSettingPM result = new DashboardsUserSettingPM();
            mapping.CustomPOCOToPM(result, DashboardsUserSetting);
            mapping.POCOToPM(result, DashboardsUserSetting);

            return result;
        }
    }
}
