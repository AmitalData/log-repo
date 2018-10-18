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
    public class CustomerSalesmanByProductMapping
    {
        public static void MapEntity(CustomerSalesmanByProductPM entityPM, CustomerSalesmanByProduct poco, bool isNewEntity)
        {
            poco.CustomerId = entityPM.CustomerId;
            poco.ProductTypeCode = entityPM.ProductTypeCode;
            poco.SalesmanUserId = entityPM.SalesmanUserId;
            poco.Tenant = entityPM.Tenant;

       
        }
    }
}
