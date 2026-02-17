using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.Tools.DataMapping
{
    public class CardContactAdditionalServiceMapping
    {
        public static void MapEntity(CardContactAdditionalServicePM entityPM, CardContactAdditionalService entityPoco, bool isNewState)
        {
            if (isNewState)
            {
                entityPoco.Id = entityPM.Id;
                entityPoco.CardContactId = entityPM.CardContactId;
                entityPoco.AdditionalServiceId = entityPM.AdditionalServiceId;
                entityPoco.Tenant = entityPM.Tenant;
            }
        }
    }
}
