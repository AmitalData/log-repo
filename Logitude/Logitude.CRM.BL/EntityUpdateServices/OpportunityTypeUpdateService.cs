using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.CRM.BL.EntityPMs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Logitude.CRM.BL.EntityUpdateServices
{
    public partial class OpportunityTypeUpdateService
    {
        protected override void OnCreating(OpportunityTypePM entityPM, EntityPM entityParentPM)
        {
            entityPM.Id = IdCounter.GetNumber("OpportunityType", entityPM.Tenant);
        }

        protected override void Trace(OpportunityTypePM entityPM, OpportunityType entityPOCO, string changesXml)
        {
            ContactRepository contactRep = new ContactRepository(entityPM.Tenant);
            var resolveLoggingUserId = AuthenticationUtil.ResolveUserIdentityName(entityPM.Tenant);
            Contact contact = contactRep.GetSingleContactByEmail(resolveLoggingUserId, entityPM.Tenant);

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "CROT",
                    UserId = contact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = "OpportunityType",
                });
            }

            else
            {
                string notes = "";
                if (entityPM.InActive && !entityPOCO.InActive)
                {
                    notes = "Opportunity Type Inactivated";
                }

                else if (!entityPM.InActive && entityPOCO.InActive)
                {
                    notes = "Opportunity Type Activated";
                }

                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "UPOT",
                    UserId = contact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = "OpportunityType",
                    Notes = notes,
                });
            }
        }
    }
}
