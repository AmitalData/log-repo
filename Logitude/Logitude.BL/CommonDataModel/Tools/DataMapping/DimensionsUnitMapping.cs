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
    public class DimensionsUnitMapping
    {
        public static void MapEntity(DimensionsUnitPM entityPM, DimensionsUnit poco, bool isNewEntity)
        {
            poco.Code = entityPM.Code;
            poco.Name = entityPM.Name; 
            poco.SearchFields = entityPM.Code + "," + entityPM.Name;
            
        }
    }
}