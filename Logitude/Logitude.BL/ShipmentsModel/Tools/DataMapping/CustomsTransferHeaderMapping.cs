using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
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
            entity.ShipmentNumber = entityPM.ShipmentNumber;

            string mySearchFields = "";
            
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.TransferNumber);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.FileName);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.ShipmentNumber);

            entityPM.SearchFields = mySearchFields;
            entity.SearchFields = mySearchFields;
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
        }
    }
}
