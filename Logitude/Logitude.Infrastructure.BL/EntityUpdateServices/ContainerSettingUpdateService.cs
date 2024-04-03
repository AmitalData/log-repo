using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.Data;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Infrastructure.BL.EntityUpdateServices
{
    public partial class ContainerSettingUpdateService
    {
        protected override void OnCreating(ContainerSettingPM entityPM, EntityPM entityParentPM)
        {
            if (InfrastructureContext.GetContext(Tenant).ContainerSettings.Any(x => x.Tenant == entityPM.Tenant))
            {
                throw new ApplicationException("Duplicate record on this tenant");
            }
        }

        protected override void OnUpdating(ContainerSettingPM entityPM)
        {
            if (InfrastructureContext.GetContext(Tenant).ContainerSettings.Any(x => x.Tenant == entityPM.Tenant && x.Id != entityPM.Id))
            {
                throw new ApplicationException("Duplicate record on this tenant");
            }
        }
    }
}
