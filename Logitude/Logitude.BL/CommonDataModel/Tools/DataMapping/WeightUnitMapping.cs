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
    public class WeightUnitMapping
    {
        public static void MapEntity(WeightUnitPM entityPM, WeightUnit poco, bool isNewEntity)
        {
            poco.Code = entityPM.Code;
            poco.Name = entityPM.Name;
            poco.PrintAs = entityPM.PrintAs;
            poco.SearchFields = entityPM.Code + "," + entityPM.Name;
            
        }
    }
}