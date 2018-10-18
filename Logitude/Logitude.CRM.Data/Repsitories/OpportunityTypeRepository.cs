 
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
   public partial class OpportunityTypeRepository:IRepository<OpportunityType>
   {
        
		public List<OpportunityType> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public OpportunityType GetOpportunityTypeByCode(string code, int tenant)
        {
            return (from a in context.OpportunityTypes where a.Code == code && a.Tenant == tenant select a).FirstOrDefault();
        }

   }

}
   