using Logitude.Infrastructure.BL.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Logitude.Infrastructure.BL.EntityQueryServices
{
    public partial class ContainerSettingQueryService
    {
        public ContainerSettingPM GetSingle(int tenant)
        {
            var entity = this.repository.GetAll(tenant).FirstOrDefault();
            if (entity == null) return null;

            ContainerSettingPM result = new ContainerSettingPM();
            mapping.CustomPOCOToPM(result, entity);
            mapping.POCOToPM(result, entity);
            return result;
        }
    }
}
