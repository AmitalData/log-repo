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
        public static void MapShipmentCustomsTransmission(ShipmentCustomsTransmissionPM itemPM, ShipmentCustomsTransmission itemPoco, bool isNewEntity)
        {
            if (isNewEntity)
            {
                itemPoco.Tenant = itemPM.Tenant;
                itemPoco.ShipmentId = itemPM.ShipmentId;
            }
            itemPoco.LastSendDate = itemPM.LastSendDate;
            itemPoco.SentByUserId = itemPM.SentByUserId;
            itemPoco.Error = itemPM.Error;
            itemPoco.Status = itemPM.Status;
            itemPoco.MessageCode = itemPM.MessageCode;
            itemPoco.CommunicationLogId = itemPM.CommunicationLogId;
        }
    }
}
