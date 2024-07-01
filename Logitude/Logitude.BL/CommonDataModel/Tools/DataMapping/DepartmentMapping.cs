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
            poco.Code = entityPM.Code;
            poco.DirectionId = entityPM.DirectionId;

            BuildSearchField(entityPM, poco);
        }

        private static void BuildSearchField(DepartmentPM entityPM, Department entityPoco)
        {
            string mySearchFields = "";

            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.EnglishName);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.LocalName);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Code);

            entityPM.SearchFields = mySearchFields;
            entityPoco.SearchFields = mySearchFields;
        }
    }
}