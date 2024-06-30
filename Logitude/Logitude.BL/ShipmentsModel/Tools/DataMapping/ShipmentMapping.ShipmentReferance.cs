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
        public static void MapShipmentReferance(ShipmentReferancePM itemPM, ShipmentReferance itemPoco, bool isNewEntity)
        {
            if (isNewEntity)
            {
                itemPoco.Tenant = itemPM.Tenant;
                itemPoco.ShipmentId = itemPM.ShipmentId;
            }
            itemPoco.PartnerId = itemPM.PartnerId;
            itemPoco.LineNumber = itemPM.LineNumber;
            itemPoco.ReferenceType = itemPM.ReferenceType;
        }
    }
}
