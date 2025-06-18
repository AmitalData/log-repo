using AmitalCloud.Infrastructure.Domain.EntityClasses;
using AmitalCloud.Infrastructure.Domain.EntityPMs;

namespace AmitalCloud.Infrastructure.Data.DataMapping
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