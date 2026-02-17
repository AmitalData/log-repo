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
    public class BusinessUnitMapping
    {
        public static void MapEntity(BusinessUnitPM entityPM, BusinessUnit Poco, bool isNewEntity)
        {
            if (isNewEntity)
            {
                Poco.Id = entityPM.Id;
                Poco.Tenant = entityPM.Tenant;
            }

            Poco.Name = entityPM.Name;
            Poco.ParentId = entityPM.ParentId;
            Poco.InActive = entityPM.InActive;

            BuildSearchField(entityPM, Poco);
        }

        private static void BuildSearchField(BusinessUnitPM entityPM, BusinessUnit entityPoco)
        {
            string mySearchFields = "";

            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Name);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.ParentName);

            entityPM.SearchFields = mySearchFields;
            entityPoco.SearchFields = mySearchFields;
        }
    }
}