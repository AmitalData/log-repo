using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Logitude.BL.CommonDataModel.Tools.Validating
{
    public class BranchValidating
    {

        public static void Validate(EntityPMs.BranchPM entityPM, BranchRepository entityRepository)
        {
            if (string.IsNullOrEmpty(entityPM.CounterCode))
            {
                throw new Exception("Counter Code is Required");
            }

            string branchName = entityRepository.GetBranchNameByCounterCode(entityPM.CounterCode, entityPM.Tenant);

            if (!string.IsNullOrEmpty(branchName))
            {
                throw new Exception("Counter Code is already used in " + branchName + " branch ");
            }
        }
    }
}