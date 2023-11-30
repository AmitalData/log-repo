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
    public class CustomerSizeMapping
    {
        public static void MapEntity(CustomerSizePM entityPM, CustomerSize poco, bool isNewEntity)
        {
            poco.Name = entityPM.Name;
            poco.Id = entityPM.Id;
            poco.Tenant = entityPM.Tenant;
            poco.SearchFields = entityPM.Order + "," + entityPM.Name;
            poco.OrderNumber = entityPM.Order;
            poco.InActive = entityPM.InActive;
            poco.Code = entityPM.Code;
        }
    }
}