using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CRM.BL.EntityUpdateServices
{
    public partial class TicketSeverityUpdateService
    {
        protected override void OnCreating(TicketSeverityPM entityPM, EntityPM entityParentPM)
        {
            entityPM.Id = IdCounter.GetNumber("TicketSeverity", entityPM.Tenant);
        }

        protected override void OnUpdating(EntityPMs.TicketSeverityPM entityPM)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Update)
            {

            }
        }

        protected override void OnUpdating(EntityPMs.TicketSeverityPM entityPM, TicketSeverity entityPOCO)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Update)
            {

            }
        }

        protected override void Trace(TicketSeverityPM entityPM, TicketSeverity entityPOCO, string changesXml)
        {
            ICommonDataContext commonContext = CommonDataContext.GetContext(entityPM.Tenant);
            ContactRepository contactRep = new ContactRepository(commonContext);
            Contact contact = contactRep.GetSingleContactByEmail(AuthenticationUtil.GetAuthenticatedUser(), entityPM.Tenant);

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Update)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "UPTE",
                    UserId = contact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = "TicketSeverity",
                    Notes = changesXml
                });

                if (entityPM.Inactive && !entityPOCO.Inactive)
                {
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "IATE",
                        UserId = contact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = "TicketSeverity",
                        Notes = changesXml
                    });
                }

                else if (!entityPM.Inactive && entityPOCO.Inactive)
                {
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "ACTE",
                        UserId = contact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = "TicketSeverity",
                        Notes = changesXml
                    });
                }
            }

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "CRTE",
                    UserId = contact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = "TicketSeverity",
                    Notes = changesXml
                });
            }

        }

    }
}
