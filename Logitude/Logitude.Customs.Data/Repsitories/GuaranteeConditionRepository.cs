 
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
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.Customs.Data.Repsitories
{
   public partial class GuaranteeConditionRepository:IRepository<GuaranteeCondition>
   {
        
		public List<GuaranteeCondition> GetMulti(EntityKeyFields entityKeys)
        {
            
            GuaranteeKeys parentKeys = entityKeys as GuaranteeKeys;

            return (from a in context.GuaranteeConditions
                    where a.GuaranteeId == parentKeys.Id
                        select a).ToList();
        }

        //public List<GuaranteeCondition> GetGuaranteeConditionsForGuarantee(string guaranteeId, int tenant)
        //{
        //    return (from a in context.GuaranteeConditions.Include("ReturnCondition")
        //            where a.GuaranteeId == guaranteeId && a.Tenant == tenant
        //                select a).ToList();
        //}
   }

}
   