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
    public class AirlineAreaMapping
    {
        public static void MapEntity(AirlineAreaPM entityPM, AirlineArea poco, bool isNewState)
        {
            if (isNewState)
            {
                poco.Id = entityPM.Id;
                poco.Tenant = entityPM.Tenant;
                poco.CreateDate = entityPM.CreateDate;
                poco.CreatedByUserId = entityPM.CreatedByUserId;
                poco.AirlineId = entityPM.AirlineId;

            }

            poco.Description = entityPM.Description;
            poco.Name = entityPM.Name;
            poco.UpdatedByUserId = entityPM.UpdatedByUserId;
            poco.UpdateDate = entityPM.UpdateDate;
        }
    }
}
