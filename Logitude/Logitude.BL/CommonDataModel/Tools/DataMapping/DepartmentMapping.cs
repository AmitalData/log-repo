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
    public class DepartmentMapping
    {
        public static void MapEntity(DepartmentPM entityPM, Department poco, bool isNewEntity)
        {
            poco.EnglishName = entityPM.EnglishName;
            poco.InActive = entityPM.InActive;
            poco.LocalName = entityPM.LocalName;
            poco.Notes = entityPM.Notes;
            poco.Tenant = entityPM.Tenant;
            poco.SearchFields = entityPM.EnglishName + "," + entityPM.LocalName;
            poco.Code = entityPM.Code;
        }
    }
}