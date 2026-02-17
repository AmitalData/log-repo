using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.Tools.DataMapping
{
    public  class AutomationMapping
    {

        public static void MapEntity(AutomationPM entityPM, Automation entityPOCO, bool isNewState)
        {
            if (isNewState)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
            }

            entityPOCO.Name = entityPM.Name;
            entityPOCO.Description = entityPM.Description;
            entityPOCO.ResultCode = entityPM.ResultCode;
            entityPOCO.CreateDate = entityPM.CreateDate;
            entityPOCO.CreatedByUserId = entityPM.CreatedByUserId;
            entityPOCO.Inactive = entityPM.Inactive;
            entityPOCO.ObjectTableId = entityPM.ObjectTableId;
            entityPOCO.Type = entityPM.Type;
            entityPOCO.UpdateDate = entityPM.UpdateDate;
            entityPOCO.UpdatedByUserId = entityPM.UpdatedByUserId;
            entityPOCO.Version = entityPM.Version;
            entityPOCO.AutomationXML = entityPM.AutomationXML;
            entityPOCO.DocumentTypeId = entityPM.DocumentTypeId;  
            entityPOCO.TemplateId = entityPM.TemplateId;  
            entityPOCO.From = entityPM.From;
            entityPOCO.FromEmail = entityPM.FromEmail;
            entityPOCO.Order = entityPM.Order;
            entityPOCO.Code = entityPM.Code;


        }
    }
}
