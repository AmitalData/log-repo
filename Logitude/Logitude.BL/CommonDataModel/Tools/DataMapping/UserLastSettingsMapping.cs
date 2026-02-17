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
    public class UserLastSettingsMapping
    {
        public static void MapEntity(UserLastSettingsPM entityPM, UserLastSettings poco, bool isNewState)
        {
            poco.UserId = entityPM.UserId;
            poco.Tenant = entityPM.Tenant;
            poco.Id = entityPM.Id;
            poco.ControlNameSpace = entityPM.ControlNameSpace;
            poco.FilterName = entityPM.FilterName;
            poco.FilterValue = entityPM.FilterValue;
        }
    }
}