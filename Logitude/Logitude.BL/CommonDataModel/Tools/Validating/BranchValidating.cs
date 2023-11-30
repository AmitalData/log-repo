using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Logitude.BL.CommonDataModel.Tools.Validating
{
    public class BranchValidating
    {

        public static void Validate(BranchPM entityPM, BranchRepository entityRepository)
        {
            //if (string.IsNullOrEmpty(entityPM.CounterCode))
            //{
            //    throw new Exception("Counter Code is Required");
            //}

            //Branch branch = entityRepository.GetBranchByCounterCode(entityPM.CounterCode, entityPM.Tenant);

            //if (branch != null && branch.Id != entityPM.Id)
            //{
            //    throw new Exception("Counter Code is already used in " + branch.EnglishName + " branch ");
            //}
        }
    }
}