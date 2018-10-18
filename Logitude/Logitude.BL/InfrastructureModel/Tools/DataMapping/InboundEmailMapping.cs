using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.Tools.DataMapping
{
    public class InboundEmailMapping
    {
        public static void MapEntity(InboundEmailPM entityPM, InboundEmail entityPOCO, bool isNewState)
        {
            if (isNewState)
            {
                entityPOCO.Tenant = entityPM.Tenant;
            }
            entityPOCO.ObjectTableId = entityPM.ObjectTableId;
            entityPOCO.EntityId = entityPM.EntityId;
            entityPOCO.CreateDate = entityPM.CreateDate;
            entityPOCO.UpdateDate = entityPM.UpdateDate;
            entityPOCO.Uniquekey = entityPM.Uniquekey;
            entityPOCO.CreatedByContactId = entityPM.CreatedByContactId;
            entityPOCO.IsRejected = entityPM.IsRejected;
            entityPOCO.AnalyzeQueueId = entityPM.AnalyzeQueueId;
        }

        public static void MapInboundEmailLineEntity(InboundEmailLinePM entityPM, InboundEmailLine entityPOCO, bool isNewState)
        {
            if (isNewState)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
            }
         
            entityPOCO.InboundEmailId = entityPM.InboundEmailId;
            entityPOCO.CommunicationLogId = entityPM.CommunicationLogId;
            entityPOCO.Subject = entityPM.Subject;
            entityPOCO.Sender = entityPM.Sender;
            entityPOCO.Body = entityPM.Body;
            entityPOCO.FullBody = entityPM.FullBody;
            entityPOCO.HTMLFullBody = entityPM.HTMLFullBody;
            entityPOCO.Recepient = entityPM.Recepient;
            entityPOCO.CreateDate = entityPM.CreateDate;
            entityPOCO.CCs = entityPM.CCs;
            entityPOCO.Bcc = entityPM.Bcc;
            entityPOCO.InternalUsers = entityPM.InternalUsers;
            entityPOCO.Direction = entityPM.Direction;
            entityPOCO.EntityLineId = entityPM.EntityLineId;

        }
    }
}
