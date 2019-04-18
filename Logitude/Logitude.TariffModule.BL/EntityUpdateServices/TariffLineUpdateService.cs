using Logitude.Server.Tools.Counters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.TariffModule.BL.EntityUpdateServices
{
    public partial class TariffLineUpdateService
    {
        protected override void OnCreating(EntityPMs.TariffLinePM entityPM, EntityPMs.TariffPM entityParentPM)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPM.Id = IdCounter.GetNumber("TariffLine", entityPM.Tenant);
            }

            entityPM.TariffId = entityParentPM.Id;
        }
    }
}
