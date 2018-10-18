 
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
   public partial class DepositConditionRepository:IRepository<DepositCondition>
   {
        
		public List<DepositCondition> GetMulti(EntityKeyFields entityKeys)
        {

            DepositKeys depositKeys = entityKeys as DepositKeys;

            return (from a in context.DepositConditions
                    where a.DepositId == depositKeys.Id 
                    select a).ToList();
        }

   }

}
   