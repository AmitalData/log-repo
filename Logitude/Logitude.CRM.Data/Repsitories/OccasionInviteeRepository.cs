 
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
   public partial class OccasionInviteeRepository:IRepository<OccasionInvitee>
   {        
		public List<OccasionInvitee> GetMulti(EntityKeyFields entityKeys)
        {
            OccasionKeys myEntityKeys = entityKeys as OccasionKeys;
            return (from a in context.OccasionInvitees where a.OccasionId == myEntityKeys.Id select a).ToList();
        }

        public IQueryable<OccasionInvitee> GetOccasionInviteesByOccasion(string occasionId, int tenant)
        {
            return from a in context.OccasionInvitees
                   where a.OccasionId == occasionId && a.Tenant == tenant
                   select a;
        }
    }
}
   