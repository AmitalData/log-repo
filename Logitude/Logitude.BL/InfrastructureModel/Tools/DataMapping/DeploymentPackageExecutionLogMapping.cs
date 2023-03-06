
using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.Tools.DataMapping
{
    public class DeploymentPackageExecutionLogMapping
    {
        public static void MapEntity(DeploymentPackageExecutionLogPM entityPM, DeploymentPackageExecutionLog entityPOCO, bool isNewState)
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
            entityPOCO.RequestXML = entityPM.RequestXML;
            entityPOCO.RetryNumber = entityPM.RetryNumber;
            entityPOCO.DoneDate = entityPM.DoneDate;
            entityPOCO.Logs = entityPM.Logs;
            entityPOCO.ExceptionMessage = entityPM.ExceptionMessage;
            entityPOCO.ExecutedByServerName = entityPM.ExecutedByServerName;

      


        }
    }
}
