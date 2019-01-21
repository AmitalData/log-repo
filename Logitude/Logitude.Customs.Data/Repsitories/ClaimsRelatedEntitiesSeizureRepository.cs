 
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
   public partial class ClaimsRelatedEntitiesSeizureRepository:IRepository<ClaimsRelatedEntitiesSeizure>
   {
        
		public List<ClaimsRelatedEntitiesSeizure> GetMulti(EntityKeyFields entityKeys)
        {
            ClaimsRelatedEntityKeys claimsRelatedEntityKeys = entityKeys as ClaimsRelatedEntityKeys;

            return (from a in context.ClaimsRelatedEntitiesSeizures
                    where a.ClaimId == claimsRelatedEntityKeys.ClaimId && a.CounterKey == claimsRelatedEntityKeys.EntityCounterKey
                    select a).ToList();
        }

   }

}
   