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
    public partial class TicketTypeUpdateService
    {
        protected override void OnCreating(TicketTypePM entityPM, EntityPM entityParentPM)
        {
            entityPM.Id = IdCounter.GetNumber("TicketType", entityPM.Tenant);
        }


        protected override void OnUpdating(EntityPMs.TicketTypePM entityPM)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Update)
            {

            }
        }

        protected override void OnUpdating(EntityPMs.TicketTypePM entityPM, TicketType entityPOCO)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Update)
            {

            }
        }

        protected override void Trace(TicketTypePM entityPM, TicketType entityPOCO, string changesXml)
        {
            ICommonDataContext commonContext = CommonDataContext.GetContext(entityPM.Tenant);
            ContactRepository contactRep = new ContactRepository(commonContext);
            Contact contact = contactRep.GetSingleContactByEmail(AuthenticationUtil.GetAuthenticatedUser(), entityPM.Tenant);

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Update)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "UPTT",
                    UserId = contact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = "TicketType",
                    Notes = changesXml
                });

                if (entityPM.Inactive && !entityPOCO.Inactive)
                {
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "IATT",
                        UserId = contact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = "TicketType",
                        Notes = changesXml
                    });
                }

                else if (!entityPM.Inactive && entityPOCO.Inactive)
                {
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "ACTT",
                        UserId = contact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = "TicketType",
                        Notes = changesXml
                    });
                }
            }

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "CRTT",
                    UserId = contact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = "TicketType",
                    Notes = changesXml
                });
            }

        }

    }

}
