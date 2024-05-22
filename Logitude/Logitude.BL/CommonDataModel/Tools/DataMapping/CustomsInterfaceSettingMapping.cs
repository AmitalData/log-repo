using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.Tools.DataMapping
{
    public class CustomsInterfaceSettingMapping
    {
        public static void MapEntity(CustomsInterfaceSettingPM entityPM, CustomsInterfaceSetting poco, bool isNewEntity)
        {
            poco.Tenant = entityPM.Tenant;
            poco.LocalCustomsInterfaceCode = entityPM.LocalCustomsInterfaceCode;
            poco.ImportToUSAInterfaceCode = entityPM.ImportToUSAInterfaceCode;
            poco.ExportFromUSAInterfaceCode = entityPM.ExportFromUSAInterfaceCode;
            poco.LocalCompanyId = entityPM.LocalCompanyId;
            poco.LocalUserId = entityPM.LocalUserId;
            poco.LocalPassword = entityPM.LocalPassword;
            poco.ActivateCustomsManagInShipment = entityPM.ActivateCustomsManagInShipment;
            poco.ArtemusInSettingsId = entityPM.ArtemusInSettingsId;
            poco.ArtemusOutSettingsId = entityPM.ArtemusOutSettingsId;
            poco.AMCAirStartDate = entityPM.AMCAirStartDate;
            poco.AMCOceanStartDate = entityPM.AMCOceanStartDate;
        }
    }
}
