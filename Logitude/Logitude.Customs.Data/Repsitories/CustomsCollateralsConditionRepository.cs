 
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
   public partial class CustomsCollateralsConditionRepository:IRepository<CustomsCollateralsCondition>
   {
        
		public List<CustomsCollateralsCondition> GetMulti(EntityKeyFields entityKeys)
        {

            CustomsCollateralKeys keys = entityKeys as CustomsCollateralKeys;
            List<CustomsCollateralsCondition> conditions = (from a in context.CustomsCollateralsConditions
                                                      where a.CustomsCollateralId == keys.Id
                                                      select a).ToList();
            return conditions;
        }

   }

}
   