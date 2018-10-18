using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Security;

namespace Logitude.BL.InfrastructureModel.Tools.DataMapping
{
    public class TextCodeMapping
    {
        public static void MapEntity(TextCodePM textCodePM, TextCode textCode, bool isNewState)
        {
            if (textCodePM.LocalDefaultText != textCode.LocalDefaultText)
            {
                ContactPM loggedContact = new ContactQuery(textCodePM.Tenant).GetContactByEmailOnly(SecurityUtility.GetAuthenticatedUser(), textCodePM.Tenant);

                textCodePM.IsSpellChecked = true;
                textCodePM.SpellCheckDate = TenantServerConfigration.GetCurrentDateTime(textCodePM.Tenant);
                textCodePM.SpellCheckedByUserId = loggedContact.Id;
            }

            textCode.Code = textCodePM.Code;
            textCode.DefaultText = textCodePM.DefaultText;
            textCode.DefaultTextPlural = textCodePM.DefaultTextPlural;
            textCode.ObjectTableId = textCodePM.ObjectTableId;
            textCode.Tenant = textCodePM.Tenant;
            textCode.TextCodeTypeCode = textCodePM.TextCodeTypeCode;
            textCode.InActive = textCodePM.InActive;
            textCode.LocalDefaultText = textCodePM.LocalDefaultText;
            textCode.IsSpellChecked = textCodePM.IsSpellChecked;
            textCode.SpellCheckDate = textCodePM.SpellCheckDate;
            textCode.SpellCheckedByUserId = textCodePM.SpellCheckedByUserId;

        }
    }
}