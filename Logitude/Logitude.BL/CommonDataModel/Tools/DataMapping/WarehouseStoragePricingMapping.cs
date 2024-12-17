using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.Tools.DataMapping
{
    public class WarehouseStoragePricingMapping
    {
        public static void MapEntity(WarehouseStoragePricingPM entityPM, WarehouseStoragePricing poco, bool isNewEntity)
        {
            if (isNewEntity)
            {
                poco.Tenant = entityPM.Tenant;
                poco.WarehouseId = entityPM.WarehouseId;
            }
            
            poco.StepFrom = entityPM.StepFrom;
            poco.StepTo = entityPM.StepTo;
            poco.Days = entityPM.Days;
            poco.SalePrice = entityPM.SalePrice;
            poco.LineNumber = entityPM.LineNumber;
        }
    }
}
