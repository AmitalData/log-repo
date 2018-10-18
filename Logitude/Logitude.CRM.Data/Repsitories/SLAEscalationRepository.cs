 
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
   public partial class SLAEscalationRepository:IRepository<SLAEscalation>
   {
        
		public List<SLAEscalation> GetMulti(EntityKeyFields entityKeys)
        {
            SLAHeaderKeys myEntityKeys = entityKeys as SLAHeaderKeys;
            return (from a in context.SLAEscalations where a.SLAHeaderId == myEntityKeys.Id select a).ToList();
        }

        public SLAEscalation GetSingleByTenant(int tenant)
        {
            return (from a in context.SLAEscalations
                    where a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public List<SLAEscalation> GetSLAEscalationsBySLAHeaderId(string id, int tenant)
        {
            return (from a in context.SLAEscalations
                    where a.SLAHeaderId == id && a.Tenant == tenant
                    select a).ToList();
        }

   }

}
   