using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.Tools.DataMapping
{
    public class AutomationHistoryMapping
    {

        public static void MapEntity(AutomationHistoryPM entityPM, AutomationHistory entityPOCO, bool isNewState)
        {
            if (isNewState)
            {
                entityPOCO.Version = entityPM.Version;
                entityPOCO.Tenant = entityPM.Tenant;
                entityPOCO.AutomationsId = entityPM.AutomationsId;
            }

            entityPOCO.CreateDate = entityPM.CreateDate;
            entityPOCO.AutomationXML = entityPM.AutomationXML;

        }

    }
}
