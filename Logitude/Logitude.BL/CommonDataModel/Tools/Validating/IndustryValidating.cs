using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Logitude.BL.CommonDataModel.Tools.Validating
{
    public class IndustryValidating
    {
        public static void Validate(EntityPMs.IndustryPM entityPM)
        {
            IndustryRepository entityRepository = new IndustryRepository(entityPM.Tenant);

            Industry industry = entityRepository.GetSingleIndustryByCode(entityPM.Code, entityPM.Tenant);

            if (industry != null)
            {
                if (industry.Id != entityPM.Id)
                {
                    throw new ApplicationException("Industry with same code already exists");
                }                
            }
        }
    }
}