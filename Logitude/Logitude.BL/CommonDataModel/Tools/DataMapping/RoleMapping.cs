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
    public class RoleMapping
    {
        public static void MapEntity(RolePM entityPM, Role entityPOCO, bool isNewState)
        {
            if (isNewState)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
            }

            entityPOCO.Name = entityPM.Name;
            entityPOCO.Code = entityPM.Code;
            entityPOCO.Description = entityPM.Description;
            entityPOCO.RoleTypeCode = entityPM.RoleTypeCode;
            entityPOCO.ParentRoleId = entityPM.ParentRoleId;
            entityPOCO.IsCustomRole = entityPM.IsCustomRole;
            entityPM.SearchFields = entityPM.Name;
            entityPOCO.SearchFields = entityPM.Name;
        }
    }
}