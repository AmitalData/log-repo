using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.Tools.DataMapping
{
    public class AutomationResultEmailRecipientMapping
    {


        public static void MapEntity(AutomationResultEmailRecipientPM entityPM, AutomationResultEmailRecipient entityPOCO, bool isNewState)
        {
            if (isNewState)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
            }


            entityPOCO.AutomationsId = entityPM.AutomationsId;
            entityPOCO.RecipientType = entityPM.RecipientType;
            entityPOCO.RecipientValue = entityPM.RecipientValue;



        }

    }
}
