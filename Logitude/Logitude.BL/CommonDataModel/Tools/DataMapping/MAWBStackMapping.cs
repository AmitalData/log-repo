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
    public class MAWBStackMapping
    {
        public static void MapEntity(MAWBStackPM entityPM, MAWBStack poco, bool isNewState)
        {
            poco.Number = entityPM.Number;
            poco.AirlineId = entityPM.AirlineId;
            poco.Tenant = entityPM.Tenant;
            poco.InsertionDate = entityPM.InsertionDate;
            poco.Notes = entityPM.Notes;
            poco.AssignedToId = entityPM.AssignedToId;
            poco.IsUsed = entityPM.IsUsed;
           
        }
    }
}
