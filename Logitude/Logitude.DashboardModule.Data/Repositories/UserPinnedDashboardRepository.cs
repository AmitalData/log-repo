 
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
   public partial class UserPinnedDashboardRepository:IRepository<UserPinnedDashboard>
   {        
		public List<UserPinnedDashboard> GetMulti(EntityKeyFields entityKeys)
        {            
			throw new NotImplementedException();
        }

        public UserPinnedDashboard GetPinnedDashboardsByUserId(string userId, int tenant)
        {
            return (from a in context.UserPinnedDashboards where a.Tenant == tenant && a.UserId == userId select a).FirstOrDefault();
        }
    }
}
   