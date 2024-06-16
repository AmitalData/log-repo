using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools;
using System.Collections.Generic;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class CB_RequirementComputedDataQueryService : EntityQueryService<CB_RequirementComputedData, CB_RequirementComputedDataKeys, CB_RequirementComputedDataPM, object, CB_RequirementComputedDataKeys>
    {
        public List<CB_RequirementComputedDataList> GetCustomsBookRegularityRequirementData(int customsItemId)
        {
            return this.repository.GetCustomsBookRegularityRequirementData(customsItemId);
        }
    }

}
