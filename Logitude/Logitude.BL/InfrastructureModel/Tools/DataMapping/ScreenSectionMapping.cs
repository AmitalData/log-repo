using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Logitude.BL.InfrastructureModel.Tools.DataMapping
{
    public class ScreenSectionMapping
    {
        public static void MapEntity(ScreenSectionPM screenSectionPM, ScreenSection screenSection, bool isNewState)
        {

            if (isNewState)
            {
                screenSection.Tenant = screenSectionPM.Tenant;
                screenSection.Id = screenSectionPM.Id;
                screenSection.ScreenCode = (screenSectionPM.ObjectTableName + screenSectionPM.Tenant.ToString() + screenSectionPM.Name.Trim().ToUpper());

            }

            screenSection.Name = screenSectionPM.Name;
            screenSection.CreateByUserId = screenSectionPM.CreateByUserId;

        }
    }
}