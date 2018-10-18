using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Social.BL.EntityPMs;
using Logitude.Social.Data.EntityPOCOs;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Social.BL.EntityUpdateServices
{
    public partial class FollowEntityUpdateService
    {
        protected override void OnCreating(FollowEntityPM entityPM, Server.Tools.EntityPM entityParentPM)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPM.Id = IdCounter.GetNumber("FollowEntity", entityPM.Tenant);
                entityPM.CreateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                
            }
        }

        protected override void Trace(FollowEntityPM entityPM, FollowEntity entityPOCO, string changesXml)
        {
            if (!string.IsNullOrEmpty(entityPM.ObjectTableId))
            {
                ICommonDataContext commonContext = CommonDataContext.GetContext(entityPM.Tenant);
                ContactRepository contactRep = new ContactRepository(commonContext);
                Contact contact = contactRep.GetSingleContactByEmail(AuthenticationUtil.GetAuthenticatedUser(), entityPM.Tenant);

                ObjectTable table = ObjectTableRepository.GetSingleObjectTableById(entityPM.ObjectTableId, 0);

                if (table != null)
                {
                    if (table.Name == "Opportunity")
                    {
                        EventTracer.CreateTraceEvent(new EventTracerArgs()
                        {
                            Tenant = entityPM.Tenant,
                            EventTypeCode = "OPFO",
                            UserId = contact.Id,
                            EntityId = entityPM.EntityId,
                            ObjectTableName = "Opportunity",
                            Notes = changesXml
                        });
                    }
                }
            }
        }
    }
}