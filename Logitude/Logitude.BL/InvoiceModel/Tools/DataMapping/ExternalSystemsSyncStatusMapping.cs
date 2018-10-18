using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Logitude.BL.InvoiceModel.EntityPMs;

namespace Logitude.BL.InvoiceModel.Tools.DataMapping
{
    public class ExternalSystemsSyncStatusMapping
    {
        public static void MapEntity(ExternalSystemsSyncStatusPM entityPM, ExternalSystemsSyncStatus entity, bool isNewState)
        {
            if (isNewState)
            {
                entity.Id = entityPM.Id;
                entity.Tenant = entityPM.Tenant;

            }

            entity.Subject = entityPM.Subject;
            entity.Status = entityPM.Status;
            entity.StatusDate = entityPM.StatusDate;
            entity.ProgressDetails = entityPM.ProgressDetails;
          
        }

    }
}