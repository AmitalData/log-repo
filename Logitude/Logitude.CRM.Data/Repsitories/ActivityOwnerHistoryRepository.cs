 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.CRM.Data.Repsitories
{
   public partial class ActivityOwnerHistoryRepository:IRepository<ActivityOwnerHistory>
   {
        
		public List<ActivityOwnerHistory> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public List<ActivityOwnerHistory> GetActivityOwnerHistorOwnerIdActivityId(string activityId, string ownerId, int tenant)
        {
            return (from a in context.ActivityOwnerHistories
                    where a.ActivityId == activityId && a.OwnerId == ownerId && a.Tenant == tenant && a.NeedSynchronization
                    select a).ToList();
        }

   }

}
   