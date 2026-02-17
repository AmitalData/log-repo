using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.Tools.DataMapping
{
    public class CardContactProductMapping
    {
        public static void MapEntity(CardContactProductPM entityPM, CardContactProduct poco, bool isNewEntity)
        {
            if (isNewEntity)
            {
                poco.Id = entityPM.Id;
                poco.CardContactId = entityPM.CardContactId;
                poco.ProductTypeCode = entityPM.ProductTypeCode;
                poco.Tenant = entityPM.Tenant;
            }            
        }
    }
}
