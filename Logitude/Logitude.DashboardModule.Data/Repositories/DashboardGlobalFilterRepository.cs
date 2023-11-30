 
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
   public partial class DashboardGlobalFilterRepository:IRepository<DashboardGlobalFilter>
   {
        
		public List<DashboardGlobalFilter> GetMulti(EntityKeyFields entityKeys)
        {
            DashboardKeys myEntityKeys = entityKeys as DashboardKeys;
            return (from a in context.DashboardGlobalFilters where a.DashboardId == myEntityKeys.Id select a).ToList();
        }

        public List<DashboardGlobalFilter> GetDashboardFiltersByDashboardId(string dashboardId, int tenant)
        {
            return (from a in context.DashboardGlobalFilters where a.Tenant == tenant && a.DashboardId == dashboardId select a).ToList();
        }
    }
}
   