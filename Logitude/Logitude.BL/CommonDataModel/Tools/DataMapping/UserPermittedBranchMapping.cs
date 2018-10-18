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
    public class UserPermittedBranchMapping
    {
        public static void MapEntity(UserPermittedBranchPM userPermittedBranchPm, UserPermittedBranch userPermittedBranch, bool isNewState)
        {
            userPermittedBranch.UserId = userPermittedBranchPm.UserId;
            userPermittedBranch.BranchId = userPermittedBranchPm.BranchId;
            userPermittedBranch.Tenant = userPermittedBranchPm.Tenant;
            
        }
    }
}
