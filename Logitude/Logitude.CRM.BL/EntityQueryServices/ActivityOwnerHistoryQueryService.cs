using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.Data.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CRM.BL.EntityQueryServices
{
    public partial class ActivityOwnerHistoryQueryService
    {
        public List<ActivityOwnerHistoryPM> GetActivityOwnerHistoryByActivityId(string activityId, string ownerId, int tenant)
        {
            List<ActivityOwnerHistoryPM> result = new List<ActivityOwnerHistoryPM>();
            List<ActivityOwnerHistory> activityHistories = repository.GetActivityOwnerHistorOwnerIdActivityId(activityId, ownerId, tenant);

            foreach (ActivityOwnerHistory entity in activityHistories)
            {

                EntityPM = new ActivityOwnerHistoryPM();
                mapping.CustomPOCOToPM(EntityPM, entity);
                mapping.POCOToPM(EntityPM, entity);

                result.Add(EntityPM);
            }

            return result;
        }
    }
}
