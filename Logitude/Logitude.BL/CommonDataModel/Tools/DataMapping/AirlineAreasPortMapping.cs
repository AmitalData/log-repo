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
    public class AirlineAreasPortMapping
    {
        public static void MapEntity(AirlineAreasPortPM entityPM, AirlineAreasPort poco, bool isNewState)
        {
            if (isNewState)
            {
                poco.Id = entityPM.Id;
                poco.Tenant = entityPM.Tenant;
                poco.AddedDate = entityPM.AddedDate;
                poco.AddedByUserId = entityPM.AddedByUserId;
               poco.AirlineAreaId = entityPM.AirlineAreaId;
                poco.PortId = entityPM.PortId;

            }
            
        }
    }
}
