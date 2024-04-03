using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.Tools.Validating
{
    public class PortGroupValidating
    {
        public static void Validate(PortGroupPM entityPM)
        {
            PortGroupRepository entityRepository = new PortGroupRepository(entityPM.Tenant);
            PortGroup portGroup = entityRepository.GetSinglePortGroupByCode(entityPM.Code, entityPM.Tenant);

            if (portGroup != null)
            {
                if (portGroup.Id != entityPM.Id)
                {
                    throw new ApplicationException("Port group with same code already exists");
                }
            }
        }
    }
}
