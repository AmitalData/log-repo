using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Logitude.BL.InfrastructureModel.Tools.DataMapping
{
    public class EmailAlertSettingMapping
    {
        public static void MappingEmailAlertSetting(EmailAlertSettingPM itemPM, EmailAlertSetting itemPoco, bool isNewEntity)
        {
            if (isNewEntity)
            {
                itemPoco.Id = itemPM.Id;
                itemPoco.Tenant = itemPM.Tenant;
            }

            itemPoco.Code = itemPM.Code;
            itemPoco.Description = itemPM.Description;
            itemPoco.InActive = itemPM.InActive;
            itemPoco.SettingLevelCode = itemPM.SettingLevelCode;
            itemPoco.To = itemPM.To;
        }
    }
}