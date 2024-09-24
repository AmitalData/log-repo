using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools;
using System.Collections.Generic;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class CB_RuleQueryService : EntityQueryService<CB_Rule, CB_RuleKeys, CB_RulePM, CB_CustomsItemPM, CB_CustomsItemKeys>
    {
        public List<CB_RuleList> GetCustomsBookRulesData(int customsItemId)
        {
            return this.repository.GetCustomsBookRulesData(customsItemId);
        }
    }

}
