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
    public class CustomerCustomsAgentByProductMapping
    {
        public static void MapEntity(CustomerCustomsAgentByProductPM entityPM, CustomerCustomsAgentByProduct poco, bool isNewEntity)
        {
            poco.CustomerId = entityPM.CustomerId;
            poco.ProductTypeCode = entityPM.ProductTypeCode;
            poco.CustomsAgentId = entityPM.CustomsAgentId;
            poco.Tenant = entityPM.Tenant;
        }
    }
}
