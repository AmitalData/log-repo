 
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
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.CRM.Data.Repsitories
{
   public partial class OpportunityProductRepository:IRepository<OpportunityProduct>
   {
        
		public List<OpportunityProduct> GetMulti(EntityKeyFields entityKeys)
        {
            OpportunityKeys keys = entityKeys as OpportunityKeys;
            return (from a in context.OpportunityProducts where a.OpportunityId == keys.Id select a).ToList();
        }

        public List<OpportunityProduct> GetProductsByOpportunityId(string opportunityId, int tenant)
        {
            return (from a in context.OpportunityProducts.Include("OpportunityProductType").Include("PrepaidCollect") where a.OpportunityId == opportunityId && a.Tenant == tenant select a).ToList();
        }

   }

}
   