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
    public class PaymentTermMapping
    {
        public static void MapEntity(PaymentTermPM entityPM, PaymentTerm poco, bool isNewState)
        {
            ContactPM loggedContact = new ContactQuery(entityPM.Tenant).GetContactByEmailOnly(SecurityUtility.GetAuthenticatedUser(), entityPM.Tenant);

            if (isNewState)
            {
                poco.Tenant = entityPM.Tenant;
                poco.AddedManually = entityPM.AddedManually;
            }
           
            poco.Days = entityPM.Days;
            poco.EnglishName = entityPM.EnglishName;
            poco.DisplayInLOV = entityPM.DisplayInLOV;
            poco.InActive = entityPM.InActive;
            poco.LocalName = entityPM.LocalName;
            poco.Description = entityPM.Description;
            poco.LocalDescription = entityPM.LocalDescription;
            poco.SearchFields = entityPM.EnglishName + "," + entityPM.LocalName;
            poco.IsManuallySet = entityPM.IsManuallySet;
            poco.ExternalId = entityPM.ExternalId;
            poco.CurrentMonth = entityPM.CurrentMonth;
            poco.FromDateTypeCode = entityPM.FromDateTypeCode;
            poco.Code = entityPM.Code;
        }
    }
}