 
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
   public partial class DeficitDecisionRepository:IRepository<DeficitDecision>
   {
        
		public List<DeficitDecision> GetMulti(EntityKeyFields entityKeys)
        {
            DeficitKeys keys = entityKeys as DeficitKeys;
            return (from a in context.DeficitDecisions
                    where a.DeficitId == keys.Id
                    select a).ToList();
        }

        //public List<DeficitDecision> GetDeficitDecisionByTapagId(string tapagId, int tenant)
        //{

        //    if (string.IsNullOrEmpty(tapagId)) return null;
        //    return
        //          (
        //          from rec in context.DeficitDecisions
        //          where rec.TapagId == tapagId && rec.Tenant == tenant
        //          select rec
        //          )
        //          .ToList();

        //}
    }

}
   