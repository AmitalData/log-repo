using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Logitude.BL.ShipmentsModel.EntityPMs;

namespace Logitude.BL.ShipmentsModel.Tools.DataMapping
{
    public partial class ShipmentMapping
    {
        public static void MapAWBOCI(AWBOCIPM itemPM, AWBOCI itemPoco, bool isNewEntity)
        {
            if (isNewEntity)
            {
                itemPoco.Tenant = itemPM.Tenant;
                itemPoco.ShipmentId = itemPM.ShipmentId;
            }

            itemPoco.CountryId = itemPM.CountryId;
            itemPoco.AWBInformationCode = itemPM.AWBInformationCode;
            itemPoco.AWBCustomsInformationCode = itemPM.AWBCustomsInformationCode;
            itemPoco.SupplementaryCustomsInfo = itemPM.SupplementaryCustomsInfo;
        }

    }
}