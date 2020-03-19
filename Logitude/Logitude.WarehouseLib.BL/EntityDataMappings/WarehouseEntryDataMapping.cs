
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
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.WarehouseLib.Data.Repositories;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.WarehouseLib.BL.Service;

namespace Logitude.WarehouseLib.BL.EntityDataMappings
{
   
   public partial class WarehouseEntryDataMapping: IMapping<WarehouseEntryPM, WarehouseEntry>
   {



        public void CustomPMToPOCO(WarehouseEntryPM entityPM, WarehouseEntry entityPOCO)
        {
            AddPOCOPropertyName(POCOPropertyNames.Id);

            AddPOCOPropertyName(POCOPropertyNames.Tenant);
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {

                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
            }
        
            BuildSearchFields(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
        }


        public void CustomPOCOToPM(WarehouseEntryPM entityPM, WarehouseEntry entityPOCO)
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

            entityPM.ShipmentNumberWithType = entityPM.ShipmentNumber;
            string shipmentLevelName = "";
            string shipmentTypeName = "";

            if (!string.IsNullOrEmpty(entityPM.ShipmentLevelCode))
            {
                ShipmentLevelQuery ShipmentLevelQuery = new ShipmentLevelQuery(entityPM.Tenant);
                ShipmentLevelPM ShipmentLevelPM = ShipmentLevelQuery.GetSingleShipmentLevelPM(entityPM.ShipmentLevelCode);
                if (ShipmentLevelPM != null) shipmentLevelName = ShipmentLevelPM.Name;
            }

   
            if (!string.IsNullOrEmpty(entityPM.ShipmentTypeId)) {
                ShipmentTypeQuery shipmentTypeQuery = new ShipmentTypeQuery(entityPM.Tenant);
                ShipmentTypePM shipmentTypePM = shipmentTypeQuery.GetSinglePM(entityPM.ShipmentTypeId, entityPM.Tenant);
                if (shipmentTypePM != null) shipmentTypeName = shipmentTypePM.Name;
            }

            string space = !string.IsNullOrEmpty(shipmentLevelName) && !string.IsNullOrEmpty(shipmentTypeName) ? " " : "";
            if (!string.IsNullOrEmpty(shipmentLevelName)|| !string.IsNullOrEmpty(shipmentTypeName))
            {
                entityPM.ShipmentNumberWithType += (" (" + (shipmentTypeName + space + shipmentLevelName) + ")");
            }

            WarehouseEntryStatusRepository warehouseEntryStatusRepository = new WarehouseEntryStatusRepository(entityPM.Tenant);
            WarehouseEntryStatus warehouseEntryStatus = warehouseEntryStatusRepository.GetSingle(entityPM.StatusCode);
            if (warehouseEntryStatus != null) entityPM.StatusName = warehouseEntryStatus.Name;

            #region Routing
            WarehouseEntryRoutingService warehouseEntryRoutingService = new WarehouseEntryRoutingService();
            WarehouseEntryRouting warehouseEntryRouting = warehouseEntryRoutingService.GetWarehouseEntryRouting(new WarehouseEntryRoutingArgs() { TransportModeId = entityPM.TransportModeId, DirectionId = entityPM.DirectionId, FromAddressId = entityPM.FromAddressId, ToAddressId = entityPM.ToAddressId, FromPortId = entityPM.FromPortId, ToPortId = entityPM.ToPortId, ToCountryId = entityPM.ToCountryId, FromCountryId = entityPM.FromCountryId, FromTypeCode = entityPM.FromTypeCode, ToTypeCode = entityPM.ToTypeCode, Tenant = warehousePM.Tenant });
            entityPM.Origin = warehouseEntryRouting.Origin;
            entityPM.Destination = warehouseEntryRouting.Destination;
            entityPM.Routing = warehouseEntryRouting.Routing;
            #endregion

            entityPM.LastStatusUpdateDate = entityPM.LastStatusUpdateDate;
            entityPM.MasterHouse = entityPM.MasterNumber + " " + entityPM.HouseNumber;
            entityPM.EntryReferencesAndDate = entityPM.EntryReference + " " + entityPM.ActualEntryDate;

        }



    


        private void BuildSearchFields(WarehouseEntryPM entityPM, WarehouseEntry entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";
            string releaseReference = entityPM.CustomerRef1 + "," + entityPM.CustomerRef2;
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.ReceivedBy);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Notes);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.EntryNumber);
            MethodHelper.AddToSearchFields(ref mySearchFields, releaseReference);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.MasterNumber);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.ShipmentNumber);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.SpecialInstruction);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.EntryReference);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.ShipperName);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.CustomerName);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.ConsigneeName);


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
   