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
    public class CommodityMapping
    {
        public static void MapEntity(CommodityPM entityPM, Commodity entityPoco, bool isNewState)
        {
            if (isNewState)
            {
                entityPoco.Id = entityPM.Id;
                entityPoco.Tenant = entityPM.Tenant;
            }

            entityPoco.Code = entityPM.Code;
            entityPoco.Name = entityPM.Name;

            entityPM.SearchFields = entityPM.Code + "," + entityPM.Name;
            entityPoco.SearchFields = entityPM.SearchFields;
            entityPoco.InActive = entityPM.InActive;
            entityPoco.AirlineId = entityPM.AirlineId;
        }
    }
}