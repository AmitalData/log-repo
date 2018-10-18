 
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
   public partial class OpportunityProductLocationRepository:IRepository<OpportunityProductLocation>
   {        
		public List<OpportunityProductLocation> GetMulti(EntityKeyFields entityKeys)
        {
            OpportunityProductKeys keys = entityKeys as OpportunityProductKeys;
            return (from a in context.OpportunityProductLocations where a.OpportunityId == keys.OpportunityId && a.OpportunityProductTypeCode == keys.OpportunityProductTypeCode select a).ToList();
        }

        public List<OpportunityProductLocation> GetLocationsByOpportunityId(string opportunityId, int tenant)
        {
            return (from a in context.OpportunityProductLocations where a.OpportunityId == opportunityId && a.Tenant == tenant select a).ToList();
        }
   }

}
   