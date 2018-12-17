using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.ShipmentsModel.EntityPMs;

namespace Logitude.BL.ShipmentsModel.Tools.DataMapping
{
    public partial class ShipmentMapping
    {
        public static void MapEntity(MessagingStockPM entityPM, MessagingStock entityPOCO, bool isNewEntity)
        {
            if (isNewEntity)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.TenantNumber = entityPM.TenantNumber;
                entityPOCO.CreateDate = entityPM.CreateDate;
                entityPOCO.CreatedByUserId = entityPM.CreatedByUserId;                
            }

            entityPOCO.StartDate = entityPM.StartDate;
            entityPOCO.EndDate = entityPM.EndDate;
            entityPOCO.Amount = entityPM.Amount;
            entityPOCO.Remaining = entityPM.Remaining;
            entityPOCO.UpdateDate = entityPM.UpdateDate;
            entityPOCO.UpdatedByUserId = entityPM.UpdatedByUserId;
            entityPOCO.IsCancelled = entityPM.IsCancelled;
            entityPOCO.Notes = entityPM.Notes;
            entityPOCO.TotalPrice = entityPM.TotalPrice;
            entityPOCO.StockType = entityPM.StockType;

            BuildSearchFields(entityPM, entityPOCO);
        }

        private static void BuildSearchFields(MessagingStockPM entityPM, MessagingStock entityPOCO)
        {
            string mySearchFields = "";

            if (!string.IsNullOrEmpty(entityPM.Status))
            {
                mySearchFields = string.IsNullOrEmpty(mySearchFields) ? entityPM.Status : mySearchFields + "," + entityPM.Status;
            }

            if (!string.IsNullOrEmpty(entityPM.Notes))
            {
                mySearchFields = string.IsNullOrEmpty(mySearchFields) ? entityPM.Notes : mySearchFields + "," + entityPM.Notes;
            }

            entityPM.SearchFields = mySearchFields;
            entityPOCO.SearchFields = mySearchFields;
        }
    }
}