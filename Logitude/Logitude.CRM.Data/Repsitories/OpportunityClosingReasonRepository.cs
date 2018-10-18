 
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
   public partial class OpportunityClosingReasonRepository:IRepository<OpportunityClosingReason>
   {        
		public List<OpportunityClosingReason> GetMulti(EntityKeyFields entityKeys)
        {            
			throw new NotImplementedException();
        }

        public OpportunityClosingReason GetOpportunityClosingReasonByCode(string code, int tenant)
        {
            return (from a in context.OpportunityClosingReasons where a.Code == code && a.Tenant == tenant select a).FirstOrDefault();
        }

        public IQueryable<OpportunityClosingReason> GetOpportunityClosingReasonsByTenant(int tenant)
        {
            IQueryable<OpportunityClosingReason> stages = from a in context.OpportunityClosingReasons
                                       where a.Tenant == tenant
                                       select a;
            return stages;
        }
   }
}
   