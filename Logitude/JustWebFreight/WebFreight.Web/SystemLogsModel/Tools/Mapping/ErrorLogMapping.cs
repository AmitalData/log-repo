using Logitude.SystemLogs.POCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFreight.Web.SystemLogsModel.EntityPMs;

namespace WebFreight.Web.SystemLogsModel.Tools.Mapping
{
    public class BatchServicesLogMapping
    {

        public static void MapEntity(BatchServicesLogPM entityPM, BatchServicesLog poco, bool isNewEntity)
        {

            if (isNewEntity)
            {
                poco.CreateDate = entityPM.CreateDate;

            }

            poco.BatchServiceCode = entityPM.BatchServiceCode;
            poco.CPU = entityPM.CPU;
            poco.CreateDate = entityPM.CreateDate;
            poco.LastActivity = entityPM.LastActivity;
            poco.NumberOfDoneItems = entityPM.NumberOfDoneItems;
        }
    }
}