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
    public class VesselMapping
    {
        public static void MapEntity(VesselPM entityPM, Vessel poco, bool isNewState)
        {
            if (isNewState)
            {
                poco.Tenant = entityPM.Tenant;
            }

            poco.AddedManually = entityPM.AddedManually;
            poco.Code = entityPM.Code;
            poco.EnglishName = entityPM.EnglishName;
            poco.InActive = entityPM.InActive;
            poco.LocalName = entityPM.LocalName;
            poco.Notes = entityPM.Notes;            
            poco.SearchFields = entityPM.Code + "," + entityPM.EnglishName + "," + entityPM.LocalName;
            poco.IMOCode = entityPM.IMOCode;
            poco.CountryId = entityPM.CountryId;
        }
    }
}
