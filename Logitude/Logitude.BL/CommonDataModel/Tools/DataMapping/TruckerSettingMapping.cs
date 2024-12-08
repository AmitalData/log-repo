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
    public class TruckerSettingMapping
    {
        public static void MapEntity(TruckerSettingPM itemPM, TruckerSetting itemPoco, bool isNewEntity)
        {
            if (isNewEntity)
            {
                itemPoco.Id = itemPM.Id;
                itemPoco.Tenant = itemPM.Tenant;
            }
            itemPoco.TruckerId = itemPM.TruckerId;
            itemPoco.AddressId = itemPM.AddressId;
            itemPoco.FromAddressCityId = itemPM.FromAddressCityId;
            itemPoco.ToAddressCityId = itemPM.ToAddressCityId;
            itemPoco.Responsibility = itemPM.Responsibility;
        }
    }
}
