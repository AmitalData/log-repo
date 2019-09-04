using Logitude.Server.Tools.Counters;
using Logitude.TariffModule.BL.EntityPMs;
using Logitude.TariffModule.Data.EntityPOCOs;
using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.TariffModule.BL.EntityUpdateServices
{
    public partial class TariffSurchargesUpdateUpdateService
    {
        protected override void OnCreating(TariffSurchargesUpdatePM entityPM, Server.Tools.EntityPM entityParentPM)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPM.CreateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                entityPM.Id = IdCounter.GetNumber("TariffSurchargesUpdate", entityPM.Tenant);
            }
        }

        protected override void OnUpdating(TariffSurchargesUpdatePM entityPM, TariffSurchargesUpdate entityPOCO)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Update)
            {

            }
        }
    }
}
