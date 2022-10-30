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
                screenSection.Id = screenSectionPM.Id;
                screenSection.ScreenCode = screenSectionPM.ScreenCode;
                screenSection.Tenant = screenSectionPM.Tenant;
            }


            screenSection.Number = screenSectionPM.Number;
            screenSection.NumberOfRows = screenSectionPM.NumberOfRows;
            screenSection.Name = screenSectionPM.Name;
            screenSection.CreatedByUserId = screenSectionPM.CreatedByUserId;
            screenSection.Inactive = screenSectionPM.Inactive;
            screenSection.Type = screenSectionPM.Type;
            screenSection.RelatedScreenCode = screenSectionPM.RelatedScreenCode;

        }
    }
}