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
    public class GlobalZoneMapping
    {
        public static void MapEntity(GlobalZonePM entityPM, GlobalZone poco, bool isNewState)
        {
            if (isNewState)
            {
                poco.Code = entityPM.Code;
                poco.Tenant = entityPM.Tenant;
            }

            poco.EnglishName = entityPM.EnglishName;
            poco.InActive = entityPM.InActive;
            poco.LocalName = entityPM.LocalName;
            poco.Notes = entityPM.Notes;            
            poco.SearchFields = entityPM.Code + "," + entityPM.EnglishName + "," + entityPM.LocalName;
        }
    }
}