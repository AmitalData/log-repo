using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Logitude.BL.InfrastructureModel.Tools.DataMapping
{
    public class TenantSettingMapping
    {
        public static void MapEntity(TenantSettingPM tenantSettingPM, TenantSetting tenantSetting, bool isNewState)
        {
            tenantSetting.SettingCode = tenantSettingPM.SettingCode;
            tenantSetting.ObjectTableId = tenantSettingPM.ObjectTableId;
            tenantSetting.Tenant = tenantSettingPM.Tenant;
            tenantSetting.SettingValue = tenantSettingPM.SettingValue;
            tenantSetting.Prefix = tenantSettingPM.Prefix;
            tenantSetting.Size = tenantSettingPM.Size;
            tenantSetting.DontIncludeDirects = tenantSettingPM.DontIncludeDirects;
            tenantSetting.IsDocumentFilingByEmailEnabled = tenantSettingPM.IsDocumentFilingByEmailEnabled;
        }
    }
}