using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.TimeManagement.BL.EntityPMs;
using Logitude.TimeManagement.Data.EntityPOCOs;
using Logitude.TimeManagement.Data.Repositories;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.TimeManagement.BL.EntityUpdateServices
{
    public partial class TMBudgetUpdateService
    {

        protected override void OnCreating(TMBudgetPM entityPM, Server.Tools.EntityPM entityParentPM)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPM.Id = IdCounter.GetNumber("TMBudget", entityPM.Tenant);
            }
        }

        protected override void OnUpdating(TMBudgetPM entityPM)
        {

        }

        protected override void Trace(TMBudgetPM entityPM, TMBudget entityPOCO, string changesXml)
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
                    ObjectTableName = "TMBudget",
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
                    ObjectTableName = "TMBudget",
                    Notes = changesXml
                });
            }
        }
    }
}
