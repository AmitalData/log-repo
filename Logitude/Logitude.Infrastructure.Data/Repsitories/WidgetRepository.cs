 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Infrastructure.Data.Repsitories
{
   public partial class WidgetRepository:IRepository<Widget>
   {
        
		public List<Widget> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public List<Widget> GetWidgetsByDashboardId(string dashboardId, int tenant)
        {
            return (from a in context.Widgets where a.Tenant == tenant && a.DashboardId == dashboardId select a).ToList();
        }
    }

}
   