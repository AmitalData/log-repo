using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Infrastructure.BL.EntityUpdateServices
{
    public partial class PriceStepUpdateService
    {
        protected override void OnCreating(PriceStepPM entityPM, EntityPM entityParentPM)
        {
            
        }

        protected override void OnUpdating(PriceStepPM entityPM)
        {
           
        }
        protected override void Trace(PriceStepPM entityPM, PriceStep entityPOCO, string changesXml)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Update)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "UPEV",
                    UserId = entityPM.UpdatedByUserId,
                    EntityId = entityPM.Id,
                    ObjectTableName = "PriceSteps"
                });
            }

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "CREV",
                    UserId = entityPM.CreatedByUserId,
                    EntityId = entityPM.Id,
                    ObjectTableName = "PriceSteps"
                });
            }

        }
    }
}
