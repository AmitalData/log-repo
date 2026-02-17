using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.Tools.DataMapping
{
    public class CardExternalAccountsByProductMapping
    {
        public static void MapEntity(CardExternalAccountsByProductPM entityPM, CardExternalAccountsByProduct poco, bool isNewState)
        {
            if (isNewState)
            {
                poco.Id = entityPM.Id;
                poco.Tenant = entityPM.Tenant;
                poco.CardId = entityPM.CardId;
                poco.ProductTypeCode = entityPM.ProductTypeCode;
            }

            poco.GLAccount = entityPM.GLAccount;
            poco.CostCenter = entityPM.CostCenter;
            poco.UpdateDate = entityPM.UpdateDate;
            poco.UpdatedByUserId = entityPM.UpdatedByUserId;
        }
    }
}
