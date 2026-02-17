using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.Tools.DataMapping
{
    public class ChargesExternalAccountsByProductMapping
    {
        public static void MapEntity(ChargesExternalAccountsByProductPM entityPM, ChargesExternalAccountsByProduct poco, bool isNewState)
        {
            if (isNewState)
            {
                poco.Id = entityPM.Id;
                poco.Tenant = entityPM.Tenant;
                poco.ChargesTypeId = entityPM.ChargesTypeId;
                poco.ProductTypeCode = entityPM.ProductTypeCode;
            }

            poco.PayablesGLAccount = entityPM.PayablesGLAccount;
            poco.PayablesCostCenter = entityPM.PayablesCostCenter;
            poco.ReceivablesGLAccount = entityPM.ReceivablesGLAccount;
            poco.ReceivablesCostCenter = entityPM.ReceivablesCostCenter;
            poco.UpdateDate = entityPM.UpdateDate;
            poco.UpdatedByUserId = entityPM.UpdatedByUserId;
        }
    }
}
