using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.WarehouseLib.BL.EntityPMs;
using Logitude.WarehouseLib.Data.EntityPOCOs;
using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.WarehouseLib.BL.EntityUpdateServices
{
   

    public partial class WarehouseReleasePackageUpdateService : EntityUpdateService<WarehouseReleasePackage, WarehouseReleasePackagePM, WarehouseReleasePM>
    {

        protected override void OnCreating(WarehouseReleasePackagePM entityPM, WarehouseReleasePM entityParentPM)
        {
            if (entityParentPM != null)
            {
                entityPM.WarehouseReleaseId = entityParentPM.Id;
                entityPM.Id = IdCounter.GetNumber("WarehouseReleasePackage", entityPM.Tenant);
                entityPM.CreateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            }

            base.OnCreating(entityPM, entityParentPM);
        }


        protected override void OnUpdating(WarehouseReleasePackagePM entityPM)
        {
            entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Update)
            {
                entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            }
        }



    }

}
