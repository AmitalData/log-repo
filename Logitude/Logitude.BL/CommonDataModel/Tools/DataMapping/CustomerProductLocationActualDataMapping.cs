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
    public class CustomerProductLocationActualDataMapping
    {
        public static void MapEntity(CustomerProductLocationActualDataPM entityPM, CustomerProductLocationActualData poco, bool isNewEntity)
        {
            poco.CustomerId = entityPM.CustomerId;
            poco.ProductTypeCode = entityPM.ProductTypeCode;
            poco.Month = entityPM.Month;
            poco.Year = entityPM.Year;
            poco.CountryId = entityPM.CountryId;
            poco.Tenant = entityPM.Tenant;
            poco.TEU = entityPM.TEU;
            poco.NumberOfShipments = entityPM.NumberOfShipments;
            poco.ChargeableWeight = entityPM.ChargeableWeight;
            poco.Revenue = entityPM.Revenue;
        }
    }
}