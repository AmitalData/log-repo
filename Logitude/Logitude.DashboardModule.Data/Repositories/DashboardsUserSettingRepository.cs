 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.DashboardModule.Data.EntityPOCOs;
using Logitude.DashboardModule.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.DashboardModule.Data.Repositories
{
   public partial class DashboardsUserSettingRepository:IRepository<DashboardsUserSetting>
   {

        public List<DashboardsUserSetting> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public DashboardsUserSetting GetPinnedDashboardsByUserId(string userId, int tenant)
        {
            return (from a in context.DashboardsUserSettings where a.Tenant == tenant && a.UserId == userId select a).FirstOrDefault();
        }

    }

}
   