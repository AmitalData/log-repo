using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.Tools.DataMapping
{
    public partial class ShipmentMapping    {
        public static void MapShipmentUnassignedField(ShipmentUnassignedFieldPM itemPM, ShipmentUnassignedField itemPoco, bool isNewEntity)
        {
            if (isNewEntity)
            {
                itemPoco.Id = itemPM.Id;
                itemPoco.Tenant = itemPM.Tenant;
                itemPoco.ShipmentId = itemPM.ShipmentId;
            }

            itemPoco.ReceivedCode = itemPM.ReceivedCode;
            itemPoco.ReceivedData = itemPM.ReceivedData;
            itemPoco.ReplacedDataId = itemPM.ReplacedDataId;
            itemPoco.FieldName = itemPM.FieldName;
            itemPoco.ObjectTableId = itemPM.ObjectTableId;
            itemPoco.ComputingPartnrCode = itemPM.ComputingPartnrCode;
        }
    }
}
