using Logitude.TariffModule.BL.EntityPMs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.TariffModule.BL.EntityUpdateServices
{
    public partial class TariffVersionUpdateService
    {
        protected override void OnCreating(TariffVersionPM entityPM, TariffPM entityParentPM)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                if (entityParentPM != null)
                {
                    entityParentPM.LastVersion = entityPM.Version;
                }
            }
        }

        protected override void UpdateComposition(TariffVersionPM entityPM)
        {
            TariffLineUpdateService tariffLineUpdateService = new TariffLineUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            tariffLineUpdateService.UpdateMulti(entityPM.TariffLines, entityPM.DeletedTariffLines, entityPM, false);
        }
    }
}
