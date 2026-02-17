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
    public class CustomerForwarderByProductMapping
    {
        public static void MapEntity(CustomerForwarderByProductPM entityPM, CustomerForwarderByProduct poco, bool isNewEntity)
        {
            poco.CustomerId = entityPM.CustomerId;
            poco.ProductTypeCode = entityPM.ProductTypeCode;
            poco.ForwarderId = entityPM.ForwarderId;
            poco.Tenant = entityPM.Tenant;
        }
    }
}
