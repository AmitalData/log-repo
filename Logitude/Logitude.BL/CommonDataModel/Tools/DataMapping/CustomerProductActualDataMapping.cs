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
    public class CustomerProductActualDataMapping
    {
        public static void MapEntity(CustomerProductActualDataPM entityPM, CustomerProductActualData poco, bool isNewEntity)
        {
            poco.CustomerId = entityPM.CustomerId;
            poco.ProductTypeCode = entityPM.ProductTypeCode;
            poco.Month = entityPM.Month;
            poco.Year = entityPM.Year;
            poco.Tenant = entityPM.Tenant;
            poco.TEU = entityPM.TEU;
            poco.NumberOfShipments = entityPM.NumberOfShipments;
            poco.ChargeableWeight = entityPM.ChargeableWeight;
            poco.Revenue = entityPM.Revenue;
        }
    }
}