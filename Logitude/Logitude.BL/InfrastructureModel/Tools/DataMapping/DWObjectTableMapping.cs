using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.Tools.DataMapping
{
    public class DWObjectTableMapping
    {
        public static void MapEntity(DWObjectTablePM entityPM, DWObjectTable entityPOCO, bool isNewState)
        {
            if (isNewState)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
            }

            entityPOCO.Name = entityPM.Name;
            entityPOCO.Code = entityPM.Code;
            entityPOCO.TypeCode = entityPM.TypeCode;
            entityPOCO.DefaultFilterBy = entityPM.DefaultFilterBy;

        }
    }
}
