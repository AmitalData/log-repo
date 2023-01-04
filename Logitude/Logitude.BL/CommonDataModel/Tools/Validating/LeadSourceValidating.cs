using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Logitude.BL.CommonDataModel.Tools.Validating
{
    public class LeadSourceValidating
    {
        public static void Validate(LeadSourcePM entityPM)
        {
            LeadSourceRepository entityRepository = new LeadSourceRepository(entityPM.Tenant);

            LeadSource leadSource = entityRepository.GetSingleLeadSourceByCode(entityPM.Code, entityPM.Tenant);

            if (leadSource != null)
            {
                if (leadSource.Id != entityPM.Id)
                {
                    throw new ApplicationException("Lead source with same code already exists");
                }
            }
        }
    }
}