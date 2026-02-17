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
    public class IncotermMapping
    {
        public static void MapEntity(IncotermPM entityPM, Incoterm poco, bool isNewEntity)
        {
            var resolveLoggingUserId = AuthenticationUtil.ResolveUserIdentityName(entityPM.Tenant);
            ContactPM loggedContact = new ContactQuery(entityPM.Tenant).GetContactByEmailOnly(resolveLoggingUserId, entityPM.Tenant);

            if (isNewEntity)
            {
                poco.Tenant = entityPM.Tenant;
            }
            poco.AddedManually = entityPM.AddedManually;
            poco.Code = entityPM.Code;
            poco.Freight = entityPM.Freight;
            poco.InActive = entityPM.InActive;
            poco.LocalName = entityPM.LocalName;
            poco.Name = entityPM.Name;
            poco.OtherCharges = entityPM.OtherCharges;
            poco.Notes = entityPM.Notes;
            poco.Tenant = entityPM.Tenant;
            poco.SearchFields = entityPM.Code + "," + entityPM.LocalName + "," + entityPM.Name;
        }
    }
}
