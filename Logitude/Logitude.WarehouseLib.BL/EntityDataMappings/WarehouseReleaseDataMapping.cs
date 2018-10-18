
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools;
using Logitude.WarehouseLib.Data.EntityPOCOs;
using Logitude.WarehouseLib.BL.EntityPMs;
using Logitude.WarehouseLib.Data;
using Simplog.Server.Infrastructure;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.WarehouseLib.Data.Repositories;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Logitude.WarehouseLib.BL.EntityDataMappings
{
   
   public partial class WarehouseReleaseDataMapping: IMapping<WarehouseReleasePM, WarehouseRelease>
   {

        public void CustomPMToPOCO(WarehouseReleasePM entityPM, WarehouseRelease entityPOCO)
        {
            AddPOCOPropertyName(POCOPropertyNames.Id);

            AddPOCOPropertyName(POCOPropertyNames.Tenant);
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {

                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
            }

            BuildSearchFields(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
            entityPM.ReleaseDate = entityPM.ActualReleaseDate != null ? entityPM.ActualReleaseDate : entityPM.ExpectedReleaseDate;
        }

        public void CustomPOCOToPM(WarehouseReleasePM entityPM, WarehouseRelease entityPOCO)
        {
            string x = "";
            if (!string.IsNullOrEmpty(entityPOCO.CustomerRef1) && !string.IsNullOrEmpty(entityPOCO.CustomerRef2)) x = ",";
            entityPM.References = entityPOCO.CustomerRef1 + x + entityPOCO.CustomerRef2;

            CardQuery cardQuery = new CardQuery(entityPM.Tenant);
            CardPM cardPM = cardQuery.GetSinglePM(entityPM.CustomerId, entityPM.Tenant);
            if (cardPM != null) entityPM.CustomerName = cardPM.EnglishName;

            WarehouseQuery warehouseQuery = new WarehouseQuery(entityPM.Tenant);
            WarehousePM warehousePM = warehouseQuery.GetSinglePM(entityPM.WarehouseId, entityPM.Tenant);
            if (warehousePM != null) entityPM.WarehouseName = warehousePM.EnglishName;

            string shipmentLevelName = "";
            string shipmentTypeName = "";
            entityPM.ShipmentNumberWithType = entityPM.ShipmentNumber;
            if (!string.IsNullOrEmpty(entityPM.ShipmentLevelCode))
            {
                ShipmentLevelQuery ShipmentLevelQuery = new ShipmentLevelQuery(entityPM.Tenant);
                ShipmentLevelPM ShipmentLevelPM = ShipmentLevelQuery.GetSingleShipmentLevelPM(entityPM.ShipmentLevelCode);
                if (ShipmentLevelPM != null) shipmentLevelName = ShipmentLevelPM.Name;
            }

            if (!string.IsNullOrEmpty(entityPM.ShipmentTypeId))
            {
                ShipmentTypeQuery shipmentTypeQuery = new ShipmentTypeQuery(entityPM.Tenant);
                ShipmentTypePM shipmentTypePM = shipmentTypeQuery.GetSinglePM(entityPM.ShipmentTypeId, entityPM.Tenant);
                if (shipmentTypePM != null) shipmentTypeName = shipmentTypePM.Name;
            }
            string space = !string.IsNullOrEmpty(shipmentLevelName) && !string.IsNullOrEmpty(shipmentTypeName) ? " " : "";
            if (!string.IsNullOrEmpty(shipmentLevelName) || !string.IsNullOrEmpty(shipmentTypeName))
            {
                entityPM.ShipmentNumberWithType += (" (" + (shipmentTypeName  + space + shipmentLevelName) + ")");
            }

            WarehouseReleaseStatusRepository warehouseReleaseStatusRepository = new WarehouseReleaseStatusRepository(entityPM.Tenant);
            WarehouseReleaseStatus warehouseReleaseStatus = warehouseReleaseStatusRepository.GetSingle(entityPM.StatusCode);
            if (warehouseReleaseStatus != null) entityPM.StatusName = warehouseReleaseStatus.Name;

            entityPM.ReleaseDate = entityPOCO.ActualReleaseDate != null ? entityPOCO.ActualReleaseDate : entityPOCO.ExpectedReleaseDate;
        }

        private void BuildSearchFields(WarehouseReleasePM entityPM, WarehouseRelease entityPOCO,  bool isNewEntity)
        {
            string mySearchFields = "";
            string releaseReference = entityPM.CustomerRef1 + "," + entityPM.CustomerRef2;
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.ReleaseBy);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Notes);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.ReleaseNumber);
            MethodHelper.AddToSearchFields(ref mySearchFields, releaseReference);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.MasterNumber);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.ShipmentNumber);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.SpecialInstruction);

            if (!string.IsNullOrEmpty(entityPM.CustomerId))
            {
                Card myCard = CardRepository.GetSingleCard(entityPM.CustomerId, entityPM.Tenant, true);
                if (myCard != null)
                {
                    MethodHelper.AddToSearchFields(ref mySearchFields, myCard.EnglishName);
                }
            }

            if (!string.IsNullOrEmpty(entityPM.StatusCode))
            {
                WarehouseReleaseStatusRepository myWarehouseReleaseStatusRepository = new WarehouseReleaseStatusRepository(entityPM.Tenant);
                WarehouseReleaseStatus myStatus = myWarehouseReleaseStatusRepository.GetSingle(entityPM.StatusCode);
                if (myStatus != null)
                {
                    MethodHelper.AddToSearchFields(ref mySearchFields, myStatus.Name);
                }
            }

            if (!string.IsNullOrEmpty(entityPM.WarehouseId))
            {
                Card myCard = CardRepository.GetSingleCard(entityPM.WarehouseId, entityPM.Tenant, true);
                if (myCard != null)
                {
                    MethodHelper.AddToSearchFields(ref mySearchFields, myCard.EnglishName);
                }
            }

            entityPM.SearchFields = mySearchFields;
        }
    }

}
   