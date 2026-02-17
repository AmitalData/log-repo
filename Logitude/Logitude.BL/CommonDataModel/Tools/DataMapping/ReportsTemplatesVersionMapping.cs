
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.Tools.DataMapping
{
    public class ReportsTemplatesVersionMapping
    {
        public static void MapEntity(ReportsTemplatesVersionPM entityPM, ReportsTemplatesVersion entityPOCO, bool isNewState)
        {
            if (isNewState)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
            }

            entityPOCO.UpdateDate = entityPM.UpdateDate;
            entityPOCO.CreateDate = entityPM.CreateDate;
            entityPOCO.UpdatedByUserId = entityPM.UpdatedByUserId;
            entityPOCO.CreatedByUserId = entityPM.CreatedByUserId;

            entityPOCO.ReportDocumentId = entityPM.ReportDocumentId;
            entityPOCO.Version = entityPM.Version;
            entityPOCO.ReportId = entityPM.ReportId;
            entityPOCO.TemplateId = entityPM.TemplateId;
            entityPOCO.IsRestored = entityPM.IsRestored;

        }
    }
}
