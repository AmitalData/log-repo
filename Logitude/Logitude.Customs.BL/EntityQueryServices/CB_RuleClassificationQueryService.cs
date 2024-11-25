 
using Logitude.Server.Tools;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityLists;
using System.Collections.Generic;

namespace Logitude.Customs.BL.EntityQueryServices
{ 
   public partial class CB_RuleClassificationQueryService: EntityQueryService<CB_RuleClassification,CB_RuleClassificationKeys,CB_RuleClassificationPM,CB_RuleClassificationPM,CB_RuleClassificationKeys>
   {

        public List<CB_RuleClassification> GetCustomsBookRulesData(int customsItemId)
        {
            List<CB_RuleClassification> cb_RuleClassificationList = this.repository.GetRulesByCustomsItemId(customsItemId);
           
            return cb_RuleClassificationList;
        }

       
    }
   
}
	 