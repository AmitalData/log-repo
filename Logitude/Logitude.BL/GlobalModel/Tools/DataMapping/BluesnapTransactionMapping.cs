using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Logitude.BL.GlobalModel.EntityPMs;

namespace Logitude.BL.GlobalModel.Tools.DataMapping
{
    public class BluesnapTransactionMapping
    {
        public static void MapEntity(BluesnapTransactionPM entityPM, BluesnapTransaction poco, bool isNewState)
        {
            if (isNewState)
            {
                poco.Id = entityPM.Id;

            }
            poco.TransactionDate = entityPM.TransactionDate;
            poco.Tenant = entityPM.Tenant;
            poco.LogitudeAmital = entityPM.LogitudeAmital;
            poco.DocumentId = entityPM.DocumentId;
            poco.CreateDate = entityPM.CreateDate;

        }
    }
}
