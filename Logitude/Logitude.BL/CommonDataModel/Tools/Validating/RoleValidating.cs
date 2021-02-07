using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Logitude.BL.CommonDataModel.Tools.Validating
{
    public class RoleValidating
    {
        public static void Validate(RolePM entityPM, bool isNew, RoleRepository entityRepository)
        {
            if (isNew && entityPM.IsCustomRole)
            {
                RoleQuery roleQuery = new RoleQuery(entityPM.Tenant);
                List<RolePM> userRoles = roleQuery.GetRolesByUser(entityPM.UserId, entityPM.Tenant).Where(d => d.Exists).ToList();
                if (userRoles.Where(d => d.Id == entityPM.ParentRoleId).Any())
                {
                    throw new Exception("Can't add both a parent and a child roles");
                }
            }

            if (!string.IsNullOrEmpty(entityPM.Name))
            {
                bool exist = false;
                IQueryable<Role> roles = entityRepository.GetRoles(entityPM.Tenant);

                if (isNew)
                {
                    exist = (from a in roles
                             where a.Name == entityPM.Name
                             select a).Any();
                }

                else
                {
                    exist = (from a in roles
                             where a.Name == entityPM.Name
                             && a.Id != entityPM.Id
                             select a).Any();
                }

                if(exist)
                {
                    string msg = TranslateTextsClass.Translate("General.M.EntityAlreadyExists", entityPM.Tenant);
                    msg = msg.Replace("%Entity", "Role");
                    throw new Exception(msg);
                }
            }
        }
    }
}