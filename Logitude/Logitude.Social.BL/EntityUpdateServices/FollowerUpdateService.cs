using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Social.BL.EntityUpdateServices
{
    public partial class FollowerUpdateService
    {
        protected override void OnCreating(EntityPMs.FollowerPM entityPM, Server.Tools.EntityPM entityParentPM)
        {

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPM.CreateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            }
            base.OnCreating(entityPM, entityParentPM);
        }
    }
}
