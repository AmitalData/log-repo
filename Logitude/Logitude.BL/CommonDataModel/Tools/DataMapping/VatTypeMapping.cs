using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Security;

namespace Logitude.BL.CommonDataModel.Tools.DataMapping
{
    public class VatTypeMapping
    {
        public static void MapEntity(VatTypePM entityPM, VatType poco, bool isNewState)
        {
            ContactPM loggedContact = new ContactQuery(entityPM.Tenant).GetContactByEmailOnly(SecurityUtility.GetAuthenticatedUser(), entityPM.Tenant);

            if (isNewState)
            {
                poco.Tenant = entityPM.Tenant;
            }

            poco.AddedManually = entityPM.AddedManually;
            poco.Code = entityPM.Code;
            poco.EnglishName = entityPM.EnglishName;
            poco.InActive = entityPM.InActive;
            poco.LocalName = entityPM.LocalName;
            poco.SearchFields = entityPM.Code + "," + entityPM.EnglishName + "," + entityPM.LocalName;
            poco.Tenant = entityPM.Tenant;
            poco.Description = entityPM.Description;
            poco.LocalDescription = entityPM.LocalDescription;
            poco.ExternalVATCard = entityPM.ExternalVATCard;
            poco.ExternalTAXItemId = entityPM.ExternalTAXItemId;
            poco.IsMultiPercentage = entityPM.IsMultiPercentage;
            poco.RecognizedPercentage = entityPM.RecognizedPercentage;
        }
    }
}