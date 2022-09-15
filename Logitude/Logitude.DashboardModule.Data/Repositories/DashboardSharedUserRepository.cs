 
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
   public partial class DashboardSharedUserRepository:IRepository<DashboardSharedUser>
   {

        public List<DashboardSharedUser> GetMulti(EntityKeyFields entityKeys)
        {
            DashboardKeys myEntityKeys = entityKeys as DashboardKeys;
            return (from a in context.DashboardSharedUsers where a.DashboardId == myEntityKeys.Id select a).ToList();
        }

        public List<DashboardSharedUser> GetDashboardSharedUsersByDashboardId(string dashboardId, int tenant)
        {
            return (from a in context.DashboardSharedUsers where a.Tenant == tenant && a.DashboardId == dashboardId select a).ToList();
        }

        public IQueryable<DashboardSharedUser> GetDashboardSharedUsersByDashboardsIds(IQueryable<string> dashboardsIds, int tenant)
        {
            return (from a in context.DashboardSharedUsers where a.Tenant == tenant && dashboardsIds.Contains(a.DashboardId) select a);
        }
    }
}
   