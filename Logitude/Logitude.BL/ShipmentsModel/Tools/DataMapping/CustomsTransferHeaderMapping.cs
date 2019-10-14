using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.Tools.DataMapping
{
    public class CustomsTransferHeaderMapping
    {
        public static void MapEntity(CustomsTransferHeaderPM entityPM, CustomsTransferHeader entity, bool isNewState)
        {
            if (isNewState)
            {
                entity.Id = entityPM.Id;
                entity.Tenant = entityPM.Tenant;
                entity.CustomsTransferTypeCode = entityPM.CustomsTransferTypeCode;
            }

            entity.TransferNumber = entityPM.TransferNumber;
            entity.TransferDate = entityPM.TransferDate;
            entity.FileName = entityPM.FileName;
            entity.CreatedByUserId = entityPM.CreatedByUserId;
            entity.Notes = entityPM.Notes;
            entity.SearchFields = entityPM.SearchFields;
        }

        public static void MapLine(CustomsTransferLinePM entityPM, CustomsTransferLine entity, bool isNewState)
        {
            if (isNewState)
            {
                entity.Id = entityPM.Id;
                entity.Tenant = entityPM.Tenant;
                entity.CustomsTransferHeaderId = entityPM.CustomsTransferHeaderId;
            }

            entity.ShipmentId = entityPM.ShipmentId;
            entity.ShipmentNumber = entityPM.ShipmentNumber;
            entity.SearchFields = entityPM.SearchFields;
        }
    }
}
