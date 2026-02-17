using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class CourierPendingReasonUpdateService
    {
        protected override void OnCreating(CourierPendingReasonPM entityPM, EntityPM entityParentPM)
        {
            //entityPM.Tenant = entityParentPM.Tenant;
        }

        protected override void OnUpdating(CourierPendingReasonPM entityPM, CourierPendingReason entityPOCO)
        {
            base.OnUpdating(entityPM, entityPOCO);
        }
    }
}
