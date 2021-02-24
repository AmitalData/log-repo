using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Simplog.Data.CommonDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Logitude.BL.CommonDataModel.Tools.Validating
{
    public class UserValidating
    {
        public static void Validate(UserPM entityPM, bool isNew)
        {

            if (!entityPM.SignupRole)
            {
                RoleQuery roleQuery = new RoleQuery(entityPM.Tenant);
                List<RolePM> roles = new List<RolePM>();

                if (isNew)
                {
                    roles = entityPM.RolePMLists;
                }
                else
                {
                    roles = roleQuery.GetRolesByUser(entityPM.Id, entityPM.Tenant).ToList();
                }

                roles = roles.Where(d => d.Exists).ToList();
                List<string> parentRolesIds = roles.Where(d => !string.IsNullOrEmpty(d.ParentRoleId)).Select(a => a.ParentRoleId).ToList();

                if (roles.Where(d => parentRolesIds.Contains(d.Id)).Any())
                {
                    throw new Exception("Can't add both a parent and a child roles");
                }
            }
        }
    }
}