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
    public class CargoTenantMilestoneDefinitionMapping
    {
        public static void MapEntity(CargoTenantMilestoneDefinitionPM entityPM, CargoTenantMilestoneDefinition poco, bool isNewEntity)
        {
            poco.Id = entityPM.Id;
            poco.Tenant = entityPM.Tenant;
            poco.Code = entityPM.Code;
            poco.IsCustomerView = entityPM.IsCustomerView;
        }
    }
}
