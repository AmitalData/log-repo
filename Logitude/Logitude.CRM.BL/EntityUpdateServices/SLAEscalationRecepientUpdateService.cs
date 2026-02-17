using Logitude.CRM.BL.EntityPMs;
using Logitude.Server.Tools.Counters;
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
    public partial class SLAEscalationRecepientUpdateService
    {
        protected override void OnCreating(SLAEscalationRecepientPM entityPM, SLAEscalationPM entityParentPM)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPM.Id = IdCounter.GetNumber("SLAEscalationRecepient", entityPM.Tenant);
                entityPM.SLAEscalationId = entityParentPM.Id;
            }
        }

        protected override void OnUpdating(SLAEscalationRecepientPM entityPM)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Update)
            {

            }
        }
    }
}
