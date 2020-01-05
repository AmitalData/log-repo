using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Security;
using Simplog.Data.Helpers;

namespace Logitude.BL.CommonDataModel.Tools.DataMapping
{
    public class CarrierAreaMapping
    {
        public static void MapEntity(CarrierAreaPM entityPM, CarrierArea poco, bool isNewState, string loggedContactId)
        {
            if (isNewState)
            {
                poco.Id = entityPM.Id;
                poco.Tenant = entityPM.Tenant;
                poco.CreateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                poco.CreatedByUserId = loggedContactId;
                poco.CarrierId = entityPM.CarrierId;
                poco.TransportModeCode = entityPM.TransportModeCode;
            }

            poco.Description = entityPM.Description;
            poco.Name = entityPM.Name;
            poco.UpdatedByUserId = loggedContactId;
            poco.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
        }
    }
}
