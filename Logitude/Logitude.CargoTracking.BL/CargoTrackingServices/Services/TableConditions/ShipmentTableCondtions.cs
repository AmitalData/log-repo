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
            string shipmentFields = !string.IsNullOrEmpty(cargoTrackingDataBaseArgs.BuildCargoArgs.Table.FieldsDBName) ? cargoTrackingDataBaseArgs.BuildCargoArgs.Table.FieldsDBName : "*";
            shipmentFields = " C." + shipmentFields.Replace(",", " ,C.");
            string updatedShipmentFields = shipmentFields.Replace("C.ConsigneeName", 
                $@"(case 
                        when C.DirectionId = 'E' AND C.ConsigneeId IS NOT NULL AND ConsigneeCard.LocalName IS NOT NULL then ConsigneeCard.LocalName
                        when C.DirectionId = 'E' AND C.ConsigneeId IS NOT NULL then ConsigneeCard.EnglishName 
                        when C.DirectionId = 'E' AND C.ConsigneeId IS NULL then C.ConsigneeName end) 
                as ConsigneeName");

            updatedShipmentFields = updatedShipmentFields.Replace("C.ShipperName", 
                $@"(case 
                        when (C.DirectionId = 'I' OR C.ShipmentLevelCode = 'A') AND C.ShipperId IS NOT NULL AND ShipperCard.LocalName IS NOT NULL then ShipperCard.LocalName 
                        when (C.DirectionId = 'I' OR C.ShipmentLevelCode = 'A') AND C.ShipperId IS NOT NULL then ShipperCard.EnglishName 
                        when (C.DirectionId = 'I' OR C.ShipmentLevelCode = 'A') AND C.ShipperId IS NULL then C.ShipperName end) 
                    as ShipperName");

            var shipmentComputedFields =
                 "com.ContainersNumbers as ContainersNumbers," +
                " com.FinalDeliveryETA as FinalDeliveryETA,com.FinalDeliveryATA as FinalDeliveryATA, " +
                "com.FinalDeliveryETD as FinalDeliveryETD,com.FinalDeliveryATD as FinalDeliveryATD" +
                ",com.FirstPickupATD as FirstPickupATD";

            var shipmentMasterFields =$@"
                  Mas.Master as Master , 
                (
                  case 
                  when Mas.MainCarriageATD is not null or Mas.MainCarriageETD is not null  then Mas.MainCarriageATD
                  when Mas.Transshipment1ATD is not null or Mas.Transshipment1ETD is not null then Mas.Transshipment1ATD
                  when Mas.Transshipment2ATD is not null or Mas.Transshipment2ETD is not null then Mas.Transshipment2ATD
                  when Mas.Transshipment3ATD is not null or Mas.Transshipment3ETD is not null then Mas.Transshipment3ATD
                  ELSE Mas.MainCarriageATD
                  END
                ) as MainCarriageATD,
                (
                  case 
                  when Mas.MainCarriageETD is not null then Mas.MainCarriageETD
                  when Mas.Transshipment1ETD is not null then Mas.Transshipment1ETD
                  when Mas.Transshipment2ETD is not null then Mas.Transshipment2ETD
                  when Mas.Transshipment3ETD is not null then Mas.Transshipment3ETD
                  ELSE  Mas.MainCarriageETD
                  END
                ) as MainCarriageETD,                

                (
                  case 
                  when Mas.Transshipment3ATA is not null or Mas.Transshipment3ETA is not null  then Mas.Transshipment3ATA
                  when Mas.Transshipment2ATA is not null or Mas.Transshipment3ETA is not null then Mas.Transshipment2ATA
                  when Mas.Transshipment1ATA is not null or Mas.Transshipment3ETA is not null then Mas.Transshipment1ATA
                  ELSE  Mas.MainCarriageATA
                  END
                  ) as MainCarriageATA,
                  (
                  case 
                  when Mas.Transshipment3ETA is not null then Mas.Transshipment3ETA
                  when Mas.Transshipment2ETA is not null then Mas.Transshipment2ETA
                  when Mas.Transshipment1ETA is not null then Mas.Transshipment1ETA
                  ELSE  Mas.MainCarriageETA
                  END
                 ) as MainCarriageETA

                , Mas.ImportManifest as ImportManifest ";

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

            var groupSelect = "Min(P.Id) as ForwardingIdForCustom";

            var selectScript = $"SELECT {updatedShipmentFields}, {shipmentComputedFields}, {shipmentMasterFields}, {groupSelect} , {forwardingShipmentFields} , {shipmentAdditionalDataFields},{shipmentOrderFields}, {carrierCardFields}";




            var fromScript = $"FROM dbo.{table.DBTableName} C ";

            var joinScript = $"left  OUTER JOIN dbo.{table.DBTableName} P                   ON P.CustomFileId = C.Id " +
                             $"LEFT OUTER JOIN dbo.ShipmentComputedFields com ON com.Id = C.Id " +
                             $"LEFT OUTER JOIN dbo.ShipmentMasterDatas Mas    ON Mas.Id = C.MasterShipmentDataId "+
                             $"LEFT OUTER JOIN dbo.ShipmentMasterDatas ForwardingMaster    ON ForwardingMaster.Id = C.MasterShipmentDataId " +
                             $"LEFT OUTER JOIN dbo.ShipmentComputedFields ForwardingComputed    ON ForwardingComputed.Id = C.Id " +
                             $"LEFT OUTER JOIN dbo.ShipmentAdditionalCloudDatas AdditionalData    ON AdditionalData.Id = C.Id " +
                             $"LEFT OUTER JOIN dbo.ShipmentPickUpDeliveries ShipmentDeliveries    ON ShipmentDeliveries.ShipmentId = C.Id " +
                             $"LEFT OUTER JOIN dbo.Cards CarrierCard    ON CarrierCard.Id = ShipmentDeliveries.CarrierId " +
                             $"LEFT OUTER JOIN dbo.Cards ConsigneeCard    ON ConsigneeCard.Id = C.ConsigneeId " +
                             $"LEFT OUTER JOIN dbo.ShipmentOrders SHO    ON P.Id = SHO.ShipmentId " +
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
                string datePeriodCondition = $" {createDateWithoutTime} >= '{cargoTrackingDataBaseArgs.CargoTrackingArguments.FromDate.Value.Date.ToString("MM/dd/yyyy hh:mm:ss.fff tt")}' and" +
                                             $" {createDateWithoutTime} <= '{cargoTrackingDataBaseArgs.CargoTrackingArguments.ToDate.Value.Date.ToString("MM/dd/yyyy hh:mm:ss.fff tt")}'";
                whereConditions.Add(datePeriodCondition);
            }

            var whereScript = " WHERE C.ShipmentLevelCode = 'A' AND " + string.Join(" AND ", whereConditions);



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
                    Mas.MainCarriageETA,
                    Mas.Transshipment3ATA,
					Mas.Transshipment2ATA,
					Mas.Transshipment1ATA,
					Mas.Transshipment3ETA,
					Mas.Transshipment2ETA,
					Mas.Transshipment1ETA,
                    Mas.Transshipment3ATD,
					Mas.Transshipment2ATD,
					Mas.Transshipment1ATD,
					Mas.Transshipment3ETD,
					Mas.Transshipment2ETD,
					Mas.Transshipment1ETD";


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

            var shipmentMasterFields =$@"
                  Mas.Master as Master ,
                (
                  case 
                  when Mas.MainCarriageATD is not null or Mas.MainCarriageETD is not null  then Mas.MainCarriageATD
                  when Mas.Transshipment1ATD is not null or Mas.Transshipment1ETD is not null then Mas.Transshipment1ATD
                  when Mas.Transshipment2ATD is not null or Mas.Transshipment2ETD is not null then Mas.Transshipment2ATD
                  when Mas.Transshipment3ATD is not null or Mas.Transshipment3ETD is not null then Mas.Transshipment3ATD
                  ELSE Mas.MainCarriageATD
                  END
                ) as MainCarriageATD,
                (
                  case 
                  when Mas.MainCarriageETD is not null then Mas.MainCarriageETD
                  when Mas.Transshipment1ETD is not null then Mas.Transshipment1ETD
                  when Mas.Transshipment2ETD is not null then Mas.Transshipment2ETD
                  when Mas.Transshipment3ETD is not null then Mas.Transshipment3ETD
                  ELSE  Mas.MainCarriageETD
                  END
                ) as MainCarriageETD,
                
                
                (
                  case 
                  when Mas.Transshipment3ATA is not null or Mas.Transshipment3ETA is not null  then Mas.Transshipment3ATA
                  when Mas.Transshipment2ATA is not null or Mas.Transshipment2ETA is not null then Mas.Transshipment2ATA
                  when Mas.Transshipment1ATA is not null or Mas.Transshipment1ETA is not null then Mas.Transshipment1ATA
                  ELSE  Mas.MainCarriageATA
                  END
                  ) as MainCarriageATA,
                 (
                  case 
                  when Mas.Transshipment3ETA is not null then Mas.Transshipment3ETA
                  when Mas.Transshipment2ETA is not null then Mas.Transshipment2ETA
                  when Mas.Transshipment1ETA is not null then Mas.Transshipment1ETA
                  ELSE  Mas.MainCarriageETA
                  END
                 ) as MainCarriageETA
                , Mas.ImportManifest as ImportManifest ";

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
                string datePeriodCondition = $" {createDateWithoutTime} >= '{cargoTrackingDataBaseArgs.CargoTrackingArguments.FromDate.Value.Date.ToString("MM/dd/yyyy hh:mm:ss.fff tt")}' and" +
                                             $" {createDateWithoutTime} <= '{cargoTrackingDataBaseArgs.CargoTrackingArguments.ToDate.Value.Date.ToString("MM/dd/yyyy hh:mm:ss.fff tt")}'";
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
                    Mas.MainCarriageETA,
                    Mas.Transshipment3ATA,
					Mas.Transshipment2ATA,
					Mas.Transshipment1ATA,
					Mas.Transshipment3ETA,
					Mas.Transshipment2ETA,
					Mas.Transshipment1ETA,
                    Mas.Transshipment3ATD,
					Mas.Transshipment2ATD,
					Mas.Transshipment1ATD,
					Mas.Transshipment3ETD,
					Mas.Transshipment2ETD,
					Mas.Transshipment1ETD";


            string sqlQuery = " SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED " + selectScript + fromScript + joinScript + whereScript + groupByScript;

            return sqlQuery;
        }

        public static string GetShipmentOrders(CargoTrackingUpdateDataBaseArgs cargoTrackingDataBaseArgs,
           CargoTrackingTable table, string LastUpdate)
        {
            var shipmentTableStructure = new CargoTrackingShipmentTableStructure();
            string shipmentOrderColumns = shipmentTableStructure.GetShipmentOrderFields();
            var shipmentOrderFields = string.Join(",", shipmentOrderColumns);
            shipmentOrderFields = " SHO." + shipmentOrderFields.Replace(",", " ,SHO.");
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
                "NULL as FromWarehouseEstimationDate," +
                "NULL as ContainersNumbers," +
                "NULL as IsOperationalClosed," +
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
                "SHO.Quantity as PackagesQuantity," +

                " '' as CustomFileNumber," +
                " '' as ForwarderShipmentNumber," +
                " '' as ForwardingShipmentLevelCode," +
                " '' as CustomsDeclarationNumber," +
                $@" (
                    case 
                    when DirectionId = 'I' and ShipperCard.EnglishName is not null then ShipperCard.EnglishName 
                    when DirectionId = 'I' and ShipperCard.EnglishName is null then CasualSupplierName  
                    end
                    )as ShipperName," +
                " '' as MasterShipmentDataId," +
                " '' as FromPortId," +
                " '' as ToPortId," +
                " '' as CustomConnectToShipment," +
                " '' as CustomFileId," +
                $@" (
                    case 
                    when DirectionId = 'E' and ConsigneeCard.EnglishName is not null then ConsigneeCard.EnglishName 
                    when DirectionId = 'E' and ConsigneeCard.EnglishName is null then CasualImporterName  
                    end
                    ) as ConsigneeName," +
                " '' as CustomerReference1," +
                " '' as CustomerReference2," +
                " '' as WarehouseLegRemarks," +
                " '' as GrossWeightUnitCode," +
                " '' as ShipmentTypeId," +
                " '' as ExceptionDescription," +

                "'O' as EntityType," +
                "SHO.CreateDate as CreateDateTime," +
                "(case when SHO.AutomaticLastUpdateDate is null then SHO.UpdateDate when SHO.AutomaticLastUpdateDate is not null then SHO.AutomaticLastUpdateDate end) as AutomaticLastUpdateDate," +
                "PickupActualDateTime as PickupDate," +
                "PickupEstimatedDateTime as PickupEstimationDate," +
                "OnHandNumber as FromWarehouseNotes," +
                "OnHandDate as FromWarehouseDate";


             var selectScript = $"SELECT {shipmentOrderFields} , {cargoTrackingShipmentDefaultFields} ";

            var fromScript = $@"FROM dbo.ShipmentOrders SHO 
                                left join Cards ConsigneeCard on SHO.ConsigneeId = ConsigneeCard.id
                                left join Cards ShipperCard on SHO.ShipperId = ShipperCard.id
                                ";

            List<string> whereConditions = new List<string>();

            if (cargoTrackingDataBaseArgs.CargoTrackingArguments == null)
            {
                LastUpdate = ServiceHelper.GetTableLastUpdate(cargoTrackingDataBaseArgs.BuildCargoArgs.Table.Main_CargoTracking_TableName, cargoTrackingDataBaseArgs.BuildCargoArgs.DestinationConnectionString);

                string lastUpdateCondition = $" (SHO.AutomaticLastUpdateDate > '{LastUpdate}')";
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
                string datePeriodCondition = $" {createDateWithoutTime} >= '{cargoTrackingDataBaseArgs.CargoTrackingArguments.FromDate.Value.Date.ToString("MM/dd/yyyy hh:mm:ss.fff tt")}' and" +
                                             $" {createDateWithoutTime} <= '{cargoTrackingDataBaseArgs.CargoTrackingArguments.ToDate.Value.Date.ToString("MM/dd/yyyy hh:mm:ss.fff tt")}'";
                whereConditions.Add(datePeriodCondition);
            }

            var whereScript = " WHERE " + string.Join(" AND ", whereConditions);

            string sqlQuery = string.Join(Environment.NewLine, " SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED ", selectScript, fromScript, whereScript);

            return sqlQuery;
        }

    }
}
