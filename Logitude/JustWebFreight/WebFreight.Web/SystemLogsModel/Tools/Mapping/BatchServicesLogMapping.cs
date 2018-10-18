using Logitude.SystemLogs.POCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFreight.Web.SystemLogsModel.EntityPMs;

namespace WebFreight.Web.SystemLogsModel.Tools.Mapping
{
    public class ErrorLogMapping
    {

        public static void MapEntity(ErrorLogPM entityPM, ErrorLog poco, bool isNewEntity)
        {

            if (isNewEntity)
            {
                poco.Id = entityPM.Id;
                poco.Tenant = entityPM.Tenant;
            }

            poco.IP = entityPM.IP;
            poco.ClientDate = entityPM.ClientDate;
            poco.Exception = entityPM.Exception;
            poco.LogDate = entityPM.LogDate;
            poco.SearchFields = entityPM.SearchFields;
            poco.Tier = entityPM.Tier;
            poco.UserName = entityPM.UserName;
        }
    }
}