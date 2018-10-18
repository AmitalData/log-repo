 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Customs.Data.Repsitories
{
   public partial class ClaimRepository:IRepository<Claim>
   {
        
		public List<Claim> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public void GetWeeklyStatistic(int tenant, out int totalClaimedOpened)
        {
            totalClaimedOpened = this.context.Claims.Where(r => r.Tenant == tenant).Count();
        }
    }

}
   