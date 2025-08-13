
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.Tools.DataMapping
{
    public class ReportsTemplateMapping
    {
        public static void MapEntity(ReportsTemplatePM entityPM, ReportsTemplate entityPOCO, bool isNewState)
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

            entityPOCO.Description = entityPM.Description;
            entityPOCO.InActive = entityPM.InActive;
            entityPOCO.IsSystem = entityPM.IsSystem;
            entityPOCO.ReportId = entityPM.ReportId;
            entityPOCO.TemplateType = entityPM.TemplateType;

            entityPOCO.CC = entityPM.CC;
            entityPOCO.Subject = entityPM.Subject;
            entityPOCO.ReplyTo = entityPM.ReplyTo;
            entityPOCO.From = entityPM.From;

            entityPOCO.ObjectTableId = entityPM.ObjectTableId;
            entityPOCO.EntityId = entityPM.EntityId;
            entityPOCO.IsCopiedAtSignup = entityPM.IsCopiedAtSignup;
            entityPOCO.OriginalTemplateId = entityPM.OriginalTemplateId;


        }
    }
}
