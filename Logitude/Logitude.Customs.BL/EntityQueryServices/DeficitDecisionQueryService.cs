using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial  class DeficitDecisionQueryService: EntityQueryService<DeficitDecision, DeficitDecisionKeys, DeficitDecisionPM, DeficitPM, DeficitKeys>
    {
      
      //public List<DeficitDecisionPM> GetDeficitDecisionByTapagId(string tapagId, int tenant)
      //{
      //    List<DeficitDecisionPM> DeficitDecisionPMList = null;
      //    if (string.IsNullOrEmpty(tapagId)) return DeficitDecisionPMList;
      //    List<DeficitDecision> DeficitDecision = repository.GetDeficitDecisionByTapagId(tapagId, tenant);

      //    if (DeficitDecision != null)
      //    {
      //        DeficitDecisionPMList = new List<DeficitDecisionPM>();
      //        foreach (var DeficitDecisionItem in DeficitDecision)
      //        {
      //            DeficitDecisionPMList.Add(this.GetEntityPM(DeficitDecisionItem));
      //        }
      //    }
      //    return DeficitDecisionPMList;
      //}
        
    }
}
