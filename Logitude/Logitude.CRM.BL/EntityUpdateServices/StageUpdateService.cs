using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.CRM.BL.EntityPMs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.CRM.Data.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Logitude.CRM.BL.EntityUpdateServices
{
    public partial class StageUpdateService
    {
        protected override void OnCreating(StagePM entityPM, EntityPM entityParentPM)
        {
            entityPM.Id = IdCounter.GetNumber("Stage", entityPM.Tenant);
            entityPM.IsSelectable = true;
        }

        protected override void Trace(StagePM entityPM, Stage entityPOCO, string changesXml)
        {
            ContactRepository contactRep = new ContactRepository(entityPM.Tenant);
            var resolveLoggingUserId = AuthenticationUtil.ResolveUserIdentityName(entityPM.Tenant);
            Contact contact = contactRep.GetSingleContactByEmail(resolveLoggingUserId, entityPM.Tenant);

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "CRSG",
                    UserId = contact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = "Stage",
                });
            }

            else
            {
                string notes = "";
                if (entityPM.InActive && !entityPOCO.InActive)
                {
                    notes = "Stage Inactivated";
                }

                else if (!entityPM.InActive && entityPOCO.InActive)
                {
                    notes = "Stage Activated";
                }

                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "UPSG",
                    UserId = contact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = "Stage",
                    Notes = notes,
                });
            }
        }
    }
}
