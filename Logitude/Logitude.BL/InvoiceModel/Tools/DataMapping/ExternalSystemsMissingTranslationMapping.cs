using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Logitude.BL.InvoiceModel.EntityPMs;

namespace Logitude.BL.InvoiceModel.Tools.DataMapping
{
    public class ExternalSystemsMissingTranslationMapping
    {

        public static void MapEntity(ExternalSystemsMissingTranslationPM entityPM, ExternalSystemsMissingTranslation entity, bool isNewState)
        {
            if (isNewState)
            {
                entity.Id = entityPM.Id;
                entity.Tenant = entityPM.Tenant;

            }

            entity.LogitudeTable = entityPM.LogitudeTable;
            entity.LogitudeTable = entityPM.LogitudeTable;
            entity.LogitudeId = entityPM.LogitudeId;
            entity.Split2 = entityPM.Split2;
            entity.Split1 = entityPM.Split1;
            entity.IsResolved = entityPM.IsResolved;
        }


    }
}