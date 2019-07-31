using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.Data.EntityPOCOs;
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
    public partial class BIReportFolderUpdateService
    {
        protected override void OnCreating(BIReportFolderPM entityPM, Server.Tools.EntityPM entityParentPM)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPM.Id = IdCounter.GetNumber("BIReportFolder", entityPM.Tenant);
                entityPM.CreateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            }
        }

        protected override void OnUpdating(BIReportFolderPM entityPM)
        {
            entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Update)
            {
                entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            }
        }
        protected override void Trace(BIReportFolderPM entityPM, BIReportFolder entityPOCO, string changesXml)
        {
            ICommonDataContext commonContext = CommonDataContext.GetContext(entityPM.Tenant);

            string loggedContactId = null;            
            ContactRepository contactRep = new ContactRepository(commonContext);
            Contact contact = contactRep.GetSingleContactByEmail(AuthenticationUtil.GetAuthenticatedUser(), entityPM.Tenant);
            if (contact != null)
            {
                loggedContactId = contact.Id;
            }


            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Update)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "UPEV",
                    UserId = loggedContactId,
                    EntityId = entityPM.Id,
                    ObjectTableName = "BIReportFolder",
                    Notes = changesXml
                });
            }

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "CREV",
                    UserId = loggedContactId,
                    EntityId = entityPM.Id,
                    ObjectTableName = "BIReportFolder",
                    Notes = changesXml
                });
            }
        }

    }
}
