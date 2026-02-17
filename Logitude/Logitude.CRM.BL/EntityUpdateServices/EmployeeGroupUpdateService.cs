using Logitude.BL.Helpers;
using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CRM.BL.EntityUpdateServices
{
    public partial class EmployeeGroupUpdateService
    {
        protected override void OnCreating(EmployeeGroupPM entityPM, EntityPM entityParentPM)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPM.Id = IdCounter.GetNumber("EmployeeGroup", entityPM.Tenant);

                DateTime myDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                entityPM.CreateDate = myDate;
                entityPM.UpdateDate = myDate;
            }

        }

        protected override void OnUpdating(EmployeeGroupPM entityPM)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Update)
            {
                
            }

            TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "User");
            TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "EmployeeGroup");
        }

        protected override void UpdateComposition(EmployeeGroupPM entityPM)
        {
            EmployeeGroupLineUpdateService linesUpdateService = new EmployeeGroupLineUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            linesUpdateService.UpdateMulti(entityPM.EmployeeGroupLines, entityPM.DeletedEmployeeGroupLines, entityPM, false);
        }

        protected override void Trace(EmployeeGroupPM entityPM, EmployeeGroup entityPOCO, string changesXml)
        {
            ContactRepository contactRep = new ContactRepository(entityPM.Tenant);
            var resolveLoggingUserId = AuthenticationUtil.ResolveUserIdentityName(entityPM.Tenant);
            Contact contact = contactRep.GetSingleContactByEmail(resolveLoggingUserId, entityPM.Tenant);

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "CREV",
                    UserId = contact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = "EmployeeGroup",
                });
            }

            else
            {
                string notes = "";
                if (entityPM.Inactive && !entityPOCO.Inactive)
                {
                    notes = "Employee Group Inactivated";
                }

                else if (!entityPM.Inactive && entityPOCO.Inactive)
                {
                    notes = "Employee Group Activated";
                }

                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "UPEV",
                    UserId = contact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = "EmployeeGroup",
                    Notes = notes
                });
            }
        }
    }
}
