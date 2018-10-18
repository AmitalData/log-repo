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
   public partial class OpportunityAdditionalServiceRepository:IRepository<OpportunityAdditionalService>
   {        
		public List<OpportunityAdditionalService> GetMulti(EntityKeyFields entityKeys)
        {
            OpportunityKeys keys = entityKeys as OpportunityKeys;
            return (from a in context.OpportunityAdditionalServices where a.OpportunityId == keys.Id select a).ToList();
        }

        public List<OpportunityAdditionalService> GetOpportunityAdditionalServicesByOpportunityId(string opportunityId, int tenant)
        {
            return (from a in context.OpportunityAdditionalServices
                    where a.OpportunityId == opportunityId && a.Tenant == tenant
                    select a).ToList();
        }
   }
}
   