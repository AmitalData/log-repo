using Logitude.DashboardModule.BL.EntityPMs;
using Logitude.Server.Tools.Counters;

namespace Logitude.DashboardModule.BL.EntityUpdateServices
{
    public partial class DashboardsUserSettingUpdateService
    {
        protected override void OnCreating(DashboardsUserSettingPM entityPM, Server.Tools.EntityPM entityParentPM)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPM.Id = IdCounter.GetNumber("DashboardsUserSetting", entityPM.Tenant);               
            }
        }

        protected override void OnUpdating(DashboardsUserSettingPM entityPM)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Update)
            {
            }
        }
    }
}
