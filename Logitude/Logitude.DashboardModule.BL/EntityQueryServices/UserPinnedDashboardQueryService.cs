using Logitude.DashboardModule.BL.EntityPMs;
using Logitude.DashboardModule.Data.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.DashboardModule.BL.EntityQueryServices
{
    public partial class UserPinnedDashboardQueryService
    {
        public UserPinnedDashboardPM GetPinnedDashboardsByUserId(string userId, int tenant)
        {
            UserPinnedDashboard userPinnedDashboard = repository.GetPinnedDashboardsByUserId(userId, tenant);
            if (userPinnedDashboard == null) return null;

            UserPinnedDashboardPM result = new UserPinnedDashboardPM();
            mapping.CustomPOCOToPM(result, userPinnedDashboard);
            mapping.POCOToPM(result, userPinnedDashboard);

            return result;
        }
    }
}
