 
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
   public partial class ClaimsRelatedEntsReasonsExpRepository:IRepository<ClaimsRelatedEntsReasonsExp>
   {
        
		public List<ClaimsRelatedEntsReasonsExp> GetMulti(EntityKeyFields entityKeys)
        {
            ClaimsRelatedEntitiesReasonKeys claimsRelatedEntitiesReasonKeys = entityKeys as ClaimsRelatedEntitiesReasonKeys;

            return (from a in context.ClaimsRelatedEntsReasonsExps
                    where a.ClaimId == claimsRelatedEntitiesReasonKeys.ClaimId 
                    && a.CounterKey == claimsRelatedEntitiesReasonKeys.CounterKey
                    && a.ReasonLineNo == claimsRelatedEntitiesReasonKeys.LineNo
                    select a).ToList();
        }

   }

}
   