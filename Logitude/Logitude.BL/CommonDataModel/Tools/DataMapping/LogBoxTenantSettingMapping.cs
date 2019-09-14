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
    public class LogBoxTenantSettingMapping
    {
        public static void MapEntity(LogBoxTenantSettingPM entityPM, LogBoxTenantSetting poco, bool isNewState)
        {

            if (isNewState)
            {
                poco.Id = entityPM.Id;
            }
            poco.IsDocumentsArchive = entityPM.IsDocumentsArchive;
            poco.CustomerTenantShareImportFile = entityPM.CustomerTenantShareImportFile;
            poco.LogBoxAdminUserId = entityPM.LogBoxAdminUserId;
            poco.DocumentShareAsDefault = entityPM.DocumentShareAsDefault;
            poco.StockTypeCode = entityPM.StockTypeCode;
            poco.AutoArchiveOnInvoice = entityPM.AutoArchiveOnInvoice;

        }
    }
}
