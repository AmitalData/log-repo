using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.TariffModule.BL.EntityUpdateServices
{
    public partial class TariffVersionUpdateService
    {
        protected override void OnCreating(EntityPMs.TariffVersionPM entityPM, EntityPMs.TariffPM entityParentPM)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                if (entityParentPM != null)
                {
                    entityParentPM.LastVersion = entityPM.Version;
                }
            }
        }
    }
}
