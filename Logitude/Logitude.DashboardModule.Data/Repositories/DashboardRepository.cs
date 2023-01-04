 
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
   public partial class DashboardRepository:IRepository<Dashboard>
   {
        
		public List<Dashboard> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public IQueryable<Dashboard> GetAllByIds(string dashboardsIds, int tenant)
        {
            return (from a in context.Dashboards
                    where a.Tenant == tenant
                    && dashboardsIds.Contains(a.Id)
                    select a);
        }

        public IQueryable<Dashboard> GetAllByIdsList(List<string> dashboardsIds)
        {
            return (from a in context.Dashboards
                    where dashboardsIds.Contains(a.Id)
                    select a);
        }
    }
}
   