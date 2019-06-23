using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.Data.EntityPOCOs;
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

namespace Logitude.CRM.BL.EntityUpdateServices
{
    public partial class OccasionTypeUpdateService
    {
        protected override void OnCreating(OccasionTypePM entityPM, EntityPM entityParentPM)
        {
            entityPM.Id = IdCounter.GetNumber("OccasionType", entityPM.Tenant);
        }

        protected override void OnUpdating(EntityPMs.OccasionTypePM entityPM)
        {
            DateTime myDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);

            ICommonDataContext commonContext = CommonDataContext.GetContext(entityPM.Tenant);
            ContactRepository contactRepository = new ContactRepository(entityPM.Tenant);
            Contact loggedContact = contactRepository.GetSingleContactByEmail(AuthenticationUtil.ResolveLoggingUserId(entityPM.Tenant), entityPM.Tenant);
            if (loggedContact != null)
            {
                entityPM.UpdatedByUserId = loggedContact.Id;
            }

            entityPM.UpdateDate = myDate;

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPM.CreateDate = myDate;
                if (loggedContact != null && entityPM.CreatedByUserId == null)
                {
                    entityPM.CreatedByUserId = loggedContact.Id;
                }
            }

        }

        protected override void OnUpdating(EntityPMs.OccasionTypePM entityPM, OccasionType entityPOCO)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Update)
            {

            }
        }

        protected override void Trace(OccasionTypePM entityPM, OccasionType entityPOCO, string changesXml)
        {
            ICommonDataContext commonContext = CommonDataContext.GetContext(entityPM.Tenant);
            ContactRepository contactRep = new ContactRepository(commonContext);
            Contact contact = contactRep.GetSingleContactByEmail(AuthenticationUtil.GetAuthenticatedUser(), entityPM.Tenant);

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Update)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "UPEV",
                    UserId = contact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = "OccasionType",
                    Notes = changesXml
                });
            }

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "CREV",
                    UserId = contact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = "OccasionType",
                    Notes = changesXml
                });
            }

        }
    }
}
