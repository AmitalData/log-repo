using Logitude.BL.InfrastructureModel.EntityPMs;
 
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Logitude.BL.InfrastructureModel.Tools.DataMapping
{
    public class QueueMessageMoreDetailsMapping
    {
        public static void MapEntity(QueueMessageMoreDetailsPM entityPM, QueueMessageMoreDetails entityPOCO)
        { 
            entityPOCO.MessageBody = entityPM.MessageBody;
            entityPOCO.CompleteDateTime = entityPM.CompleteDateTime;
            entityPOCO.CreateDateTime = entityPM.CreateDateTime;
            entityPOCO.Field1 = entityPM.Field1;
            entityPOCO.Field2 = entityPM.Field2;
            entityPOCO.Field3 = entityPM.Field3;
            entityPOCO.NextRunDateTime = entityPM.NextRunDateTime;
            entityPOCO.ProcessingDateTime = entityPM.ProcessingDateTime;
            entityPOCO.QueueDefinitionCode = entityPM.QueueDefinitionCode;
            entityPOCO.RetryNumber = entityPM.RetryNumber;
            entityPOCO.Status = entityPM.Status;
         
        }
    }
}