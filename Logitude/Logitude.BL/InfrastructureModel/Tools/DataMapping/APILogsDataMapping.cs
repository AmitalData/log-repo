using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.Tools.DataMapping
{
    public class APILogsDataMapping
    {
        public static void MapEntity(APILogsDataPM entityPM, APILogsData entityPOCO, bool isNewState)
        {
            if (isNewState)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant; 
            }

            entityPOCO.DiagnosticLog = entityPM.DiagnosticLog;
            entityPOCO.RequestData = entityPM.RequestData;
            entityPOCO.ResponseData = entityPM.ResponseData;
            entityPOCO.ExceptionsMessage = entityPM.ExceptionsMessage;  
 
        }
    }
}
