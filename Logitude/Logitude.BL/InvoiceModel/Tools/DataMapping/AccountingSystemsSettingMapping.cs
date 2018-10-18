using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Logitude.BL.InvoiceModel.EntityPMs;

namespace Logitude.BL.InvoiceModel.Tools.DataMapping
{
    public class AccountingSystemsSettingMapping
    {
        public static void MapEntity(AccountingSystemsSettingPM entityPM, AccountingSystemsSetting entity, bool isNewState)
        {
            if (isNewState)
            {
                entity.Id = entityPM.Id;
                entity.Tenant = entityPM.Tenant;
             
            }

            entity.GetExternalCodeInterval = entityPM.GetExternalCodeInterval;
            entity.UpdateOnNextRequest = entityPM.UpdateOnNextRequest;
        }

    }
}