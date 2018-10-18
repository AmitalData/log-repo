using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.Tools.DataMapping
{
    public class CreditLimitSettingMpapping
    {
        public static void MapEntity(CreditLimitSettingPM entityPM, CreditLimitSetting entityPOCO, bool isNewEntity)
        {
            if (isNewEntity)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
            }

            entityPOCO.InvoiceCreationBlock = entityPM.InvoiceCreationBlock;
            entityPOCO.InvoiceCreationWarning = entityPM.InvoiceCreationWarning;
            entityPOCO.IsCreditLimitEnabled = entityPM.IsCreditLimitEnabled;
            entityPOCO.ShipmentCreationBlock = entityPM.ShipmentCreationBlock;
        }
    }
}
