using Logitude.CargoTracking.BL.CargoTrackingServices.HelperClasses;
using Logitude.CargoTracking.BL.CargoTrackingServices.Services.ServicesHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CargoTracking.BL.CargoTrackingServices.Services.TableStructure
{
    public static class ShipmentTableCondtions
    {

        public static string GetAllCustomsShipmentsThatContainForwardingShipments(CargoTrackingUpdateDataBaseArgs cargoTrackingDataBaseArgs, 
            CargoTrackingTable table, string LastUpdate)
        {
            string shipmentFields = !string.IsNullOrEmpty(cargoTrackingDataBaseArgs.BuildCargoArgs.Table.FieldsDBName) ?  cargoTrackingDataBaseArgs.BuildCargoArgs.Table.FieldsDBName : "*";
            shipmentFields = " C." + shipmentFields.Replace(",", " ,C.");
            string updatedShipmentFields = shipmentFields.Replace("C.ConsigneeName", "(case when C.DirectionId = 'E' AND C.ConsigneeId IS NOT NULL then ConsigneeCard.EnglishName when" +
                " C.DirectionId = 'E' AND C.ConsigneeId IS NULL then C.ConsigneeName end) as ConsigneeName");

            updatedShipmentFields = updatedShipmentFields.Replace("C.ShipperName", "(case when (C.DirectionId = 'I' OR C.ShipmentLevelCode = 'A') AND C.ShipperId IS NOT NULL then ShipperCard.EnglishName when" +
                " (C.DirectionId = 'I' OR C.ShipmentLevelCode = 'A') AND C.ShipperId IS NULL then C.ShipperName end) as ShipperName");

            var shipmentComputedFields =
                 "com.ContainersNumbers as ContainersNumbers," +
                " com.FinalDeliveryETA as FinalDeliveryETA,com.FinalDeliveryATA as FinalDeliveryATA, " +
                "com.FinalDeliveryETD as FinalDeliveryETD,com.FinalDeliveryATD as FinalDeliveryATD" +
                ",com.FirstPickupATD as FirstPickupATD";

            var shipmentMasterFields =
                 "Mas.MainCarriageATD as MainCarriageATD, Mas.Master as Master " +
                ", Mas.MainCarriageETD  as MainCarriageETD , Mas.MainCarriageATA  as MainCarriageATA " +
                ", Mas.MainCarriageETA  as MainCarriageETA " +
                ", Mas.ImportManifest as ImportManifest ";

            var forwardingShipmentFields =
                "min(P.ShipmentNumber) as ForwardingShipmentNumber , " +
                " min(P.CustomerReference1) as ForwardingCustomerReference1, " +
                " min(P.CustomerReference2) as ForwardingCustomerReference2, " +
                " min(ForwardingComputed.ContainersNumbers) as ForwardingContainersNumbers, " +
                " min(P.House) as ForwardingHouse, " +
                " min(ForwardingMaster.Master) as ForwardingMaster, " +
                " min(P.CustomFileNumber) as ForwardingCustomFileNumber, " +
                " min(P.CustomsDeclarationNumber) as ForwardingCustomsDeclarationNumber, " +
                " min(P.ShipperName) as ForwardingShipperName, " +
                " min(P.ConsigneeName) as ForwardingConsigneeName, " +
                " min(P.ShipmentLevelCode) as ForwardingShipmentLevelCode ";


            var shipmentAdditionalDataFields =
             "min(AdditionalData.GoodsClassification) as GoodsClassification, " +
             "min(AdditionalData.DocumentInspection) as DocumentInspection , " +
             "AdditionalData.IsPaymentRequired as IsPaymentRequired , " +
             "min(AdditionalData.PaymentDateTime) as PaymentDateTime , " +
             "min(AdditionalData.PaymentRequestDateTime) as PaymentRequestDateTime , " +
             "NULL OrderShipmentNumber , " +
             "min(AdditionalData.GatepassDocumentsReady) as GatepassDocumentsReady ";


            var carrierCardFields =
            "min(CarrierCard.EnglishName) as CarrierEnglishName, " +
            "min(CarrierCard.LocalName) as CarrierLocalName ";

            var groupSelect = "Min(P.Id) as ForwardingIdForCustom";

            var selectScript = $"SELECT {updatedShipmentFields}, {shipmentComputedFields}, {shipmentMasterFields}, {groupSelect} , {forwardingShipmentFields} , {shipmentAdditionalDataFields}, {carrierCardFields}";



            var fromScript = $"FROM dbo.{table.DBTableName} P ";

            var joinScript = $"RIGHT OUTER JOIN dbo.{table.DBTableName} C                   ON P.CustomFileId = C.Id " +
                             $"LEFT OUTER JOIN dbo.ShipmentComputedFields com ON com.Id = C.Id " +
                             $"LEFT OUTER JOIN dbo.ShipmentMasterDatas Mas    ON Mas.Id = C.MasterShipmentDataId "+
                             $"LEFT OUTER JOIN dbo.ShipmentMasterDatas ForwardingMaster    ON ForwardingMaster.Id = P.MasterShipmentDataId "+
                             $"LEFT OUTER JOIN dbo.ShipmentComputedFields ForwardingComputed    ON ForwardingComputed.Id = P.Id " +
                             $"LEFT OUTER JOIN dbo.ShipmentAdditionalCloudDatas AdditionalData    ON AdditionalData.Id = P.Id " +
                             $"LEFT OUTER JOIN dbo.ShipmentPickUpDeliveries ShipmentDeliveries    ON ShipmentDeliveries.ShipmentId = P.Id "+
                             $"LEFT OUTER JOIN dbo.Cards CarrierCard    ON CarrierCard.Id = ShipmentDeliveries.CarrierId " +
                             $"LEFT OUTER JOIN dbo.Cards ConsigneeCard    ON ConsigneeCard.Id = C.ConsigneeId " +
                             $"LEFT OUTER JOIN dbo.Cards ShipperCard    ON ShipperCard.Id = C.ShipperId";


            List<string> whereConditions = new List<string>();



            if (cargoTrackingDataBaseArgs.CargoTrackingArguments == null)
            {
                LastUpdate = ServiceHelper.GetTableLastUpdate(cargoTrackingDataBaseArgs.BuildCargoArgs.Table.Main_CargoTracking_TableName, cargoTrackingDataBaseArgs.BuildCargoArgs.DestinationConnectionString);

                string lastUpdateCondition = $" (C.AutomaticLastUpdateDate > '{LastUpdate}')";
                whereConditions.Add(lastUpdateCondition);
            }
            else
            {
                if (cargoTrackingDataBaseArgs.CargoTrackingArguments.Tenant != null)
                {
                    string tenantCondition = $" C.Tenant = {cargoTrackingDataBaseArgs.CargoTrackingArguments.Tenant}";
                    whereConditions.Add(tenantCondition);
                }


                var createDateWithoutTime = "DATEADD(dd, DATEDIFF(dd, 0, C.CreateDateTime ), 0)";
                string datePeriodCondition = $" {createDateWithoutTime} >= '{cargoTrackingDataBaseArgs.CargoTrackingArguments.FromDate.Value.Date}' and" +
                                             $" {createDateWithoutTime} <= '{cargoTrackingDataBaseArgs.CargoTrackingArguments.ToDate.Value.Date}'";
                whereConditions.Add(datePeriodCondition);
            }

            var whereScript = " WHERE " + string.Join(" AND ", whereConditions);



            shipmentFields += ",AdditionalData.IsPaymentRequired";
            var groupByScript =
                $" GROUP BY {shipmentFields}" +
                @", ConsigneeCard.EnglishName,
                    ShipperCard.EnglishName,
                    com.ContainersNumbers,
                    com.FinalDeliveryETA,
                    com.FinalDeliveryATA,
                    com.FirstPickupATD,
                    com.FinalDeliveryETD,
                    com.FinalDeliveryATD,
                    Mas.Master,
                    Mas.MainCarriageATD,
                    Mas.MainCarriageETD,
                    Mas.MainCarriageATA,
                    Mas.ImportManifest,
                    Mas.MainCarriageETA";


            string sqlQuery = string.Join(Environment.NewLine, " SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED ", selectScript, fromScript, joinScript, whereScript, groupByScript);


            return sqlQuery;
        }

        public static string GetAllNonCustomShipmentsThatContainForwardingShipments(CargoTrackingUpdateDataBaseArgs cargoTrackingDataBaseArgs,
            CargoTrackingTable table, string LastUpdate)
        {

            // Select

            string shipmentFields = !string.IsNullOrEmpty(cargoTrackingDataBaseArgs.BuildCargoArgs.Table.FieldsDBName) ? cargoTrackingDataBaseArgs.BuildCargoArgs.Table.FieldsDBName : "*";

            shipmentFields = " P." + shipmentFields.Replace(",", " ,P.");
            string updatedShipmentFields = shipmentFields.Replace("P.ConsigneeName", "(case when P.DirectionId = 'E' AND P.ConsigneeId IS NOT NULL then ConsigneeCard.EnglishName when" +
                " P.DirectionId = 'E' AND P.ConsigneeId IS NULL then P.ConsigneeName end) as ConsigneeName");

            updatedShipmentFields = updatedShipmentFields.Replace("P.ShipperName", "(case when (P.DirectionId = 'I' OR P.ShipmentLevelCode = 'A') AND P.ShipperId IS NOT NULL then ShipperCard.EnglishName when" +
                " (P.DirectionId = 'I' OR P.ShipmentLevelCode = 'A') AND P.ShipperId IS NULL then P.ShipperName end) as ShipperName");

            var shipmentComputedFields =
                 "com.ContainersNumbers as ContainersNumbers," +
                " com.FinalDeliveryETA as FinalDeliveryETA,com.FinalDeliveryATA as FinalDeliveryATA, " +
                "com.FinalDeliveryETD as FinalDeliveryETD,com.FinalDeliveryATD as FinalDeliveryATD" +
                ",com.FirstPickupATD as FirstPickupATD";

            var shipmentMasterFields =
                 "Mas.MainCarriageATD as MainCarriageATD, Mas.Master as Master " +
                ",  Mas.MainCarriageETD  as MainCarriageETD , Mas.MainCarriageATA  as MainCarriageATA " +
                ", Mas.MainCarriageETA  as MainCarriageETA"+
                ", Mas.ImportManifest as ImportManifest ";

            var forwardingShipmentFields =
              "min(P.ShipmentNumber) as ForwardingShipmentNumber , " +
              " min(P.CustomerReference1) as ForwardingCustomerReference1, " +
              " min(P.CustomerReference2) as ForwardingCustomerReference2, " +
              " min(com.ContainersNumbers) as ForwardingContainersNumbers, " +
              " min(P.House) as ForwardingHouse, " +
              " min(Mas.Master) as ForwardingMaster, " +
              " min(P.CustomFileNumber) as ForwardingCustomFileNumber, " +
              " min(P.CustomsDeclarationNumber) as ForwardingCustomsDeclarationNumber, " +
              " min(P.ShipperName) as ForwardingShipperName, " +
              " min(P.ConsigneeName) as ForwardingConsigneeName, " +
              " min(P.ShipmentLevelCode) as ForwardingShipmentLevelCode ";

            var shipmentAdditionalDataFields =
             "min(AdditionalData.GoodsClassification) as GoodsClassification, " +
             "min(AdditionalData.DocumentInspection) as DocumentInspection , " +
             "AdditionalData.IsPaymentRequired as IsPaymentRequired , " +
             "min(AdditionalData.PaymentDateTime) as PaymentDateTime , " +
             "min(AdditionalData.PaymentRequestDateTime) as PaymentRequestDateTime , " +
             "min(AdditionalData.GatepassDocumentsReady) as GatepassDocumentsReady ";

            var shipmentOrderFields =
             "min(SHO.Master) as OrderMaster, " +
             "min(SHO.House) as OrderHouse, " +
             "min(SHO.CasualImporterName) as OrderShipperName, " +
             "min(SHO.OrderNumber) as OrderShipmentNumber, " +
             "min(SHO.PoNumber) as OrderPoNumber, " +
             "min(SHO.CustomerReferences) as OrderCustomerReference ";

            var carrierCardFields =
            "min(CarrierCard.EnglishName) as CarrierEnglishName, " +
            "min(CarrierCard.LocalName) as CarrierLocalName ";

            var selectScript = $"Select {updatedShipmentFields} , {shipmentComputedFields} , {shipmentMasterFields} , {forwardingShipmentFields} , {shipmentAdditionalDataFields} , {shipmentOrderFields}, {carrierCardFields} ";




            // From

            var fromScript = $"FROM dbo.{table.DBTableName} P ";


            var joinScript = @"LEFT OUTER JOIN dbo.ShipmentComputedFields com ON com.Id = P.Id 
                            LEFT OUTER JOIN dbo.ShipmentOrders SHO    ON SHO.ShipmentId = P.Id
                            LEFT OUTER JOIN dbo.ShipmentMasterDatas Mas    ON Mas.Id = P.MasterShipmentDataId
                            LEFT OUTER JOIN dbo.ShipmentAdditionalCloudDatas AdditionalData    ON AdditionalData.Id = P.Id
                            LEFT OUTER JOIN dbo.ShipmentPickUpDeliveries ShipmentDeliveries    ON ShipmentDeliveries.ShipmentId = P.Id
							LEFT OUTER JOIN dbo.Cards CarrierCard    ON CarrierCard.Id = ShipmentDeliveries.CarrierId
							LEFT OUTER JOIN dbo.Cards ConsigneeCard    ON ConsigneeCard.Id = P.ConsigneeId
							LEFT OUTER JOIN dbo.Cards ShipperCard    ON ShipperCard.Id = P.ShipperId";



            // Where
            List<string> whereConditions = new List<string>();

            var notCustomShipment = $" P.ShipmentLevelCode <> 'A'";//P.Id NOT IN (SELECT C.Id FROM  dbo.{table.DBTableName } SH " +
                                                   // $"JOIN  dbo.{table.DBTableName}  C  ON SH.CustomFileId = C.Id) ";
            whereConditions.Add(notCustomShipment);


            if (cargoTrackingDataBaseArgs.CargoTrackingArguments == null)
            {
                LastUpdate = ServiceHelper.GetTableLastUpdate(cargoTrackingDataBaseArgs.BuildCargoArgs.Table.Main_CargoTracking_TableName,cargoTrackingDataBaseArgs.BuildCargoArgs.DestinationConnectionString);

                string lastUpdateCondition = $" (P.AutomaticLastUpdateDate > '{LastUpdate}')";
                whereConditions.Add(lastUpdateCondition);
            }
            else
            {

                if (cargoTrackingDataBaseArgs.CargoTrackingArguments.Tenant != null)
                {
                    string tenantCondition = $" P.Tenant = {cargoTrackingDataBaseArgs.CargoTrackingArguments.Tenant}";
                    whereConditions.Add(tenantCondition);
                }


                var createDateWithoutTime = "DATEADD(dd, DATEDIFF(dd, 0, P.CreateDateTime ), 0)";
                string datePeriodCondition = $" {createDateWithoutTime} >= '{cargoTrackingDataBaseArgs.CargoTrackingArguments.FromDate.Value.Date}' and" +
                                             $" {createDateWithoutTime} <= '{cargoTrackingDataBaseArgs.CargoTrackingArguments.ToDate.Value.Date}'";
                whereConditions.Add(datePeriodCondition );
            }

            var whereScript = " WHERE " +  string.Join(" AND ", whereConditions);


            // Group By
            shipmentFields += ",AdditionalData.IsPaymentRequired";
            var groupByScript =
                $" GROUP BY {shipmentFields}" +
                @", ConsigneeCard.EnglishName,
                    ShipperCard.EnglishName,
                    com.ContainersNumbers,
                    com.FinalDeliveryETA,
                    com.FinalDeliveryATA,
                    com.FirstPickupATD,
                    com.FinalDeliveryETD,
                    com.FinalDeliveryATD,
                    Mas.Master,
                    Mas.MainCarriageATD,
                    Mas.MainCarriageETD,
                    Mas.MainCarriageATA,
                    Mas.ImportManifest,
                    Mas.MainCarriageETA";


            string sqlQuery = " SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED " + selectScript + fromScript + joinScript + whereScript + groupByScript;

            return sqlQuery;
        }

        public static string GetShipmentOrders(CargoTrackingUpdateDataBaseArgs cargoTrackingDataBaseArgs,
           CargoTrackingTable table, string LastUpdate)
        {
            var shipmentTableStructure = new CargoTrackingShipmentTableStructure();
            string shipmentOrderColumns = shipmentTableStructure.GetShipmentOrderFields();
            var shipmentOrderFields = string.Join(",", shipmentOrderColumns);
            
            var cargoTrackingShipmentDefaultFields =
                "NULL as FirstPickupETD," +
                "NULL as WarehouseLegActualEntryDate," +
                "NULL as WarehouseLegExpectedEntryDate," +
                "NULL as DeclarationDate," +
                "NULL as CustomsClearanceDate," +
                "NULL as FirstPickupETA," +
                "NULL as AssginedToCustomsAgentDate," +
                "NULL as AssignedToTruckerDate," +
                "NULL as ExceptionDate," +
                "NULL as ContainersNumbers," +
                "SHO.OrderNumber as ShipmentNumber," +

                "0 as CreateDone," +
                "0 as PickupDone," +
                "0 as DepartureDone," +
                "0 as ArrivalDone," +
                "0 as ToWarehouseDone," +
                "0 as CustomsPaymentDone," +
                "0 as ClearanceDone," +
                "0 as DeliveredDone," +
                "0 as FromWarehouseDone," +
                "0 as AssignedTruckerDone," +
                "0 as AssignedCustomsAgentDone," +
                "0 as DeliveryDone," +
                "0 as DocumentInspectionDone," +
                "0 as GoodsClassificationDone," +
                "0 as PaymentRequiredDone," +
                "0 as PaymentReceivedDone," +
                "0 as GatepassArrivedDone," +
                "0 as ShipmentPickUpIndex," +
                "0 as PackagesQuantity," +

                " '' as CustomFileNumber," +
                " '' as ForwarderShipmentNumber," +
                " '' as ForwardingShipmentLevelCode," +
                " '' as CustomsDeclarationNumber," +
                " CasualImporterName as ShipperName," +
                " '' as MasterShipmentDataId," +
                " '' as FromPortId," +
                " '' as ToPortId," +
                " '' as CustomConnectToShipment," +
                " '' as CustomFileId," +
                " '' as ConsigneeName," +
                " '' as CustomerReference1," +
                " '' as CustomerReference2," +
                " '' as WarehouseLegRemarks," +
                " '' as GrossWeightUnitCode," +
                " '' as ShipmentTypeId," +
                " '' as ExceptionDescription," +

                "'O' as EntityType," +
                "CreateDate as CreateDateTime," +
                "UpdateDate as AutomaticLastUpdateDate," +
                "PickupActualDateTime as PickupDate," +
                "PickupEstimatedDateTime as PickupEstimationDate";

            var selectScript = $"SELECT {shipmentOrderFields} , {cargoTrackingShipmentDefaultFields} ";

            var fromScript = $"FROM dbo.ShipmentOrders SHO ";

            List<string> whereConditions = new List<string>();

            if (cargoTrackingDataBaseArgs.CargoTrackingArguments == null)
            {
                LastUpdate = ServiceHelper.GetTableLastUpdate(cargoTrackingDataBaseArgs.BuildCargoArgs.Table.Main_CargoTracking_TableName, cargoTrackingDataBaseArgs.BuildCargoArgs.DestinationConnectionString);

                string lastUpdateCondition = $" (SHO.UpdateDate > '{LastUpdate}')";
                whereConditions.Add(lastUpdateCondition);
            }
            else
            {
                if (cargoTrackingDataBaseArgs.CargoTrackingArguments.Tenant != null)
                {
                    string tenantCondition = $" SHO.Tenant = {cargoTrackingDataBaseArgs.CargoTrackingArguments.Tenant}";
                    whereConditions.Add(tenantCondition);
                }


                var createDateWithoutTime = "DATEADD(dd, DATEDIFF(dd, 0, SHO.CreateDate ), 0)";
                string datePeriodCondition = $" {createDateWithoutTime} >= '{cargoTrackingDataBaseArgs.CargoTrackingArguments.FromDate.Value.Date}' and" +
                                             $" {createDateWithoutTime} <= '{cargoTrackingDataBaseArgs.CargoTrackingArguments.ToDate.Value.Date}'";
                whereConditions.Add(datePeriodCondition);
            }

            var whereScript = " WHERE " + string.Join(" AND ", whereConditions);

            string sqlQuery = string.Join(Environment.NewLine, " SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED ", selectScript, fromScript, whereScript);

            return sqlQuery;
        }

    }
}
