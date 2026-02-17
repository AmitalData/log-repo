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
    public class CustomerCompetitorProductMapping
    {
        public static void MapEntity(CustomerCompetitorProductPM entityPM, CustomerCompetitorProduct entityPoco, bool isNewState)
        {
            if (isNewState)
            {
                entityPoco.Tenant = entityPM.Tenant;
                entityPoco.CustomerId = entityPM.CustomerId;
                entityPoco.CompetitorId = entityPM.CompetitorId;
                entityPoco.ProductTypeCode = entityPM.ProductTypeCode;
            }
        }
    }
}