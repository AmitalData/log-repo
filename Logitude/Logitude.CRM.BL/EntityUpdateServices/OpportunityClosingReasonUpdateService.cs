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
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;

namespace Logitude.CRM.BL.EntityUpdateServices
{
    public partial class OpportunityClosingReasonUpdateService
    {
        protected override void OnCreating(OpportunityClosingReasonPM entityPM, EntityPM entityParentPM)
        {
            entityPM.Id = IdCounter.GetNumber("OpportunityClosingReason", entityPM.Tenant);
            entityPM.AddedManually = true;
            entityPM.IsClosedLost = true;
        }

        protected override void Trace(OpportunityClosingReasonPM entityPM, OpportunityClosingReason entityPOCO, string changesXml)
        {
            ContactRepository contactRep = new ContactRepository(entityPM.Tenant);
            var resolveLoggingUserId = AuthenticationUtil.ResolveUserIdentityName(entityPM.Tenant);
            Contact contact = contactRep.GetSingleContactByEmail(resolveLoggingUserId, entityPM.Tenant);

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "CRCL",
                    UserId = contact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = "OpportunityClosingReason",
                });
            }

            else
            {
                string notes = "";
                if (entityPM.InActive && !entityPOCO.InActive)
                {
                    notes = "Closing Reason Inactivated";
                }

                else if (!entityPM.InActive && entityPOCO.InActive)
                {
                    notes = "Closing Reason Activated";
                }

                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "UPCL",
                    UserId = contact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = "OpportunityClosingReason",
                    Notes = notes,
                });
            }
        }
    }
}
