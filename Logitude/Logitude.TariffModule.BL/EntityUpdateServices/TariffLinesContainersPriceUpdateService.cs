using Logitude.Server.Tools.Counters;
using Logitude.TariffModule.BL.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.TariffModule.BL.EntityUpdateServices
{
    public partial class TariffLinesContainersPriceUpdateService
    {
        protected override void OnCreating(TariffLinesContainersPricePM entityPM, TariffLinePM entityParentPM)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPM.TariffId = entityParentPM.TariffId;
                entityPM.TariffLineId = entityParentPM.Id;
                entityPM.Tenant = entityParentPM.Tenant;
                entityPM.Id = IdCounter.GetNumber("TariffLinesContainersPrice", entityPM.Tenant);
            }
        }
    }
}
