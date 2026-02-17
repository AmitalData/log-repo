using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Logitude.BL.InfrastructureModel.Tools.DataMapping
{
    public class CounterMapping
    {
        public static void MapEntity(CounterPM counterPM, Counter counter, bool isNewState)
        {
            counter.Name = counterPM.Name;
            counter.ObjectTableId = counterPM.ObjectTableId;
            counter.Tenant = counterPM.Tenant;
            counter.Code = counterPM.Code;
            counter.ChangedByUserId = counterPM.ChangedByUserId;
            counter.ChangedDate = counterPM.ChangedDate;
        }
    }
}