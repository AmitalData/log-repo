using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Logitude.BL.InfrastructureModel.Tools.DataMapping
{
    public class SharedLogisticsUpdateMapping
    {
        public static void MapEntity(SharedLogisticsUpdatePM sharedLogisticsUpdatePM, SharedLogisticsUpdate sharedLogisticsUpdate, bool isNewState)
        {
            sharedLogisticsUpdate.EntityId = sharedLogisticsUpdatePM.EntityId;
            sharedLogisticsUpdate.DocumentId = sharedLogisticsUpdatePM.DocumentId;
            sharedLogisticsUpdate.HandledByUserId = sharedLogisticsUpdatePM.HandledByUserId;
            sharedLogisticsUpdate.HandledDate = sharedLogisticsUpdatePM.HandledDate;
            sharedLogisticsUpdate.ObjectTableId = sharedLogisticsUpdatePM.ObjectTableId;
            sharedLogisticsUpdate.Read = sharedLogisticsUpdatePM.Read;
            sharedLogisticsUpdate.ReceivedDate = sharedLogisticsUpdatePM.ReceivedDate;
            sharedLogisticsUpdate.ReceivedFrom = sharedLogisticsUpdatePM.ReceivedFrom;
            sharedLogisticsUpdate.Status = sharedLogisticsUpdatePM.Status;
            sharedLogisticsUpdate.Subject = sharedLogisticsUpdatePM.Subject;
        }
    }
}