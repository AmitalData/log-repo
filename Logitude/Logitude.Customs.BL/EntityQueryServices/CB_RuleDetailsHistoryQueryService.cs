using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools;
using System.Collections.Generic;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class CB_RuleDetailsHistoryQueryService : EntityQueryService<CB_RuleDetailsHistory, CB_RuleDetailsHistoryKeys, CB_RuleDetailsHistoryPM, CB_RulePM, CB_RuleKeys>
    {
        public List<CB_RuleDetailsHistoryList> GetCustomsBookRulesData(int customsItemId)
        {
            return this.repository.GetCustomsBookRulesData(customsItemId);
        }

    }

}
