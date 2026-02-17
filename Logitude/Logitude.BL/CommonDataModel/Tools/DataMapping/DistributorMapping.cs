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
    public class DistributorMapping
    {
        public static void MapEntity(DistributorPM entityPM, Distributor poco, bool isNewState)
        {
            if (isNewState)
            {
                poco.Code = entityPM.Code;
            }
            poco.EnglishName = entityPM.EnglishName;
            poco.LocalName = entityPM.LocalName;
            poco.SearchFields = entityPM.Code + "," + entityPM.EnglishName + "," + entityPM.LocalName;
        }
    }
}