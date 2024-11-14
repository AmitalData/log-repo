using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools;
using RtfPipe;
using System.Collections.Generic;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class CB_RuleDetailsHistoryQueryService : EntityQueryService<CB_RuleDetailsHistory, CB_RuleDetailsHistoryKeys, CB_RuleDetailsHistoryPM, CB_RulePM, CB_RuleKeys>
    {
        public List<CB_RuleDetailsHistoryList> GetCustomsBookRulesData(int customsItemId)
        {
            List < CB_RuleDetailsHistoryList > CB_RuleDetailsHistoryList =   this.repository.GetCustomsBookRulesData(customsItemId);
            foreach (var rule in CB_RuleDetailsHistoryList)
            {
                 if (!string.IsNullOrEmpty(rule.RulesRTF))
                {
                    rule.RulesRTF = ConvertRtfToHtml(rule.RulesRTF); 
                }
            }
            return CB_RuleDetailsHistoryList;
        }

        public string ConvertRtfToHtml(string rtf)
        {
            string html = Rtf.ToHtml(rtf);

            return html;
        }
    }

}
