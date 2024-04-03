
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.Tools.DataMapping
{
    public class DocumentsExecutionLogMapping
    {
        public static void MapEntity(DocumentsExecutionLogPM entityPM, DocumentsExecutionLog entityPOCO, bool isNewState)
        {
            if (isNewState)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
            }
         
            entityPOCO.StartDate = entityPM.StartDate;
            entityPOCO.StatusCode = entityPM.StatusCode;
            entityPOCO.CreateDate = entityPM.CreateDate;
            entityPOCO.CreatedByUserId = entityPM.CreatedByUserId;
            entityPOCO.Subject = entityPM.Subject;
            entityPOCO.DocumentTypeTemplateId = entityPM.DocumentTypeTemplateId;
            entityPOCO.RequestXML = entityPM.RequestXML;
            entityPOCO.RetryNumber = entityPM.RetryNumber;
            entityPOCO.DoneDate = entityPM.DoneDate;
            entityPOCO.DocumentTypeId = entityPM.DocumentTypeId;
            entityPOCO.Logs = entityPM.Logs;
            entityPOCO.ExceptionMessage = entityPM.ExceptionMessage;
            entityPOCO.ExecutedByServerName = entityPM.ExecutedByServerName;

      


        }
    }
}
