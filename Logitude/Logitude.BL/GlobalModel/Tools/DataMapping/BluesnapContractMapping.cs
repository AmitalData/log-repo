using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Logitude.BL.GlobalModel.EntityPMs;

namespace Logitude.BL.GlobalModel.Tools.DataMapping
{
    public class BluesnapContractMapping
    {
        public static void MapEntity(BluesnapContractPM entityPM, BluesnapContract poco, bool isNewState)
        {

            poco.Code = entityPM.Code;
            poco.Name = entityPM.Name;
            poco.ContractId = entityPM.ContractId;
            poco.SearchFields = entityPM.Code + "," + entityPM.Name;
            poco.InActive = entityPM.InActive;
            poco.BluesnapContractTypeCode = entityPM.BluesnapContractTypeCode;

        }
    }
}
