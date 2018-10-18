using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.Tools.DataMapping
{
    public class InboundEmailLineMapping
    {
        public static void MapEntity(InboundEmailLinePM entityPM, InboundEmailLine entityPOCO, bool isNewState)
        {
            if (isNewState)
            {
                entityPOCO.Tenant = entityPM.Tenant;
            }
            entityPOCO.InboundEmailId = entityPM.InboundEmailId;
            entityPOCO.CommunicationLogId = entityPM.CommunicationLogId;
            entityPOCO.Subject = entityPM.Subject;
            entityPOCO.Sender = entityPM.Sender;
            entityPOCO.Body = entityPM.Body;
            entityPOCO.Recepient = entityPM.Recepient;
            entityPOCO.CreateDate = entityPM.CreateDate;
            entityPOCO.CCs = entityPM.CCs;
            entityPOCO.Bcc = entityPM.Bcc;
            entityPOCO.Direction = entityPM.Direction;
            entityPOCO.EntityLineId = entityPM.EntityLineId;
            entityPOCO.FullBody = entityPM.FullBody;
            entityPOCO.HTMLFullBody = entityPM.HTMLFullBody;
            entityPOCO.InternalUsers = entityPM.InternalUsers;
        }
    }
}
