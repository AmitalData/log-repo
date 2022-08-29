 
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
   public partial class WidgetRepository:IRepository<Widget>
   {        
		public List<Widget> GetMulti(EntityKeyFields entityKeys)
        {
            DashboardKeys myEntityKeys = entityKeys as DashboardKeys;
            return (from a in context.Widgets where a.DashboardId == myEntityKeys.Id select a).ToList();
        }

        public List<Widget> GetWidgetsByDashboardId(string dashboardId, int tenant)
        {
            return (from a in context.Widgets where a.Tenant == tenant && a.DashboardId == dashboardId select a).ToList();
        }
    }
}
   