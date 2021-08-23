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

            var shipmentComputedFields =
                 "com.ContainersNumbers as ContainersNumbers," +
                " com.FinalDeliveryETA as FinalDeliveryETA,com.FinalDeliveryATA as FinalDeliveryATA, " +
                "com.FinalDeliveryETD as FinalDeliveryETD,com.FinalDeliveryATD as FinalDeliveryATD" +
                ",com.FirstPickupATD as FirstPickupATD";

            var shipmentMasterFields =
                 "Mas.MainCarriageATD as MainCarriageATD, Mas.Master as Master " +
                ", Mas.MainCarriageETD  as MainCarriageETD , Mas.MainCarriageATA  as MainCarriageATA " +
                ", Mas.MainCarriageETA  as MainCarriageETA ";

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
             "min(AdditionalData.GatepassDocumentsReady) as GatepassDocumentsReady ";

            var groupSelect = "Min(P.Id) as ForwardingIdForCustom";

            var selectScript = $"SELECT {shipmentFields}, {shipmentComputedFields}, {shipmentMasterFields}, {groupSelect} , {forwardingShipmentFields} , {shipmentAdditionalDataFields}";



            var fromScript = $"FROM dbo.{table.DBTableName} P ";

            var joinScript = $"JOIN dbo.{table.DBTableName} C                   ON P.CustomFileId = C.Id " +
                             $"LEFT OUTER JOIN dbo.ShipmentComputedFields com ON com.Id = C.Id " +
                             $"LEFT OUTER JOIN dbo.ShipmentMasterDatas Mas    ON Mas.Id = C.MasterShipmentDataId "+
                             $"LEFT OUTER JOIN dbo.ShipmentMasterDatas ForwardingMaster    ON ForwardingMaster.Id = P.MasterShipmentDataId "+
                             $"LEFT OUTER JOIN dbo.ShipmentComputedFields ForwardingComputed    ON ForwardingComputed.Id = P.Id " +
                             $"LEFT OUTER JOIN dbo.ShipmentAdditionalCloudDatas AdditionalData    ON AdditionalData.Id = P.Id ";


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




            var groupByScript =
                $" GROUP BY {shipmentFields}" +
                @",com.ContainersNumbers,
                    com.FinalDeliveryETA,
                    com.FinalDeliveryATA,
                    com.FirstPickupATD,
                    com.FinalDeliveryETD,
                    com.FinalDeliveryATD,
                    Mas.Master,
                    Mas.MainCarriageATD,
                    Mas.MainCarriageETD,
                    Mas.MainCarriageATA,
                    Mas.MainCarriageETA";


            string sqlQuery = string.Join(Environment.NewLine, selectScript , fromScript , joinScript , whereScript , groupByScript);


            return sqlQuery;
        }




        public static string GetAllNonCustomShipmentsThatContainForwardingShipments(CargoTrackingUpdateDataBaseArgs cargoTrackingDataBaseArgs,
            CargoTrackingTable table, string LastUpdate)
        {

            // Select

            string shipmentFields = !string.IsNullOrEmpty(cargoTrackingDataBaseArgs.BuildCargoArgs.Table.FieldsDBName) ? cargoTrackingDataBaseArgs.BuildCargoArgs.Table.FieldsDBName : "*";

            shipmentFields = " P." + shipmentFields.Replace(",", " ,P.");

            var shipmentComputedFields =
                 "com.ContainersNumbers as ContainersNumbers," +
                " com.FinalDeliveryETA as FinalDeliveryETA,com.FinalDeliveryATA as FinalDeliveryATA, " +
                "com.FinalDeliveryETD as FinalDeliveryETD,com.FinalDeliveryATD as FinalDeliveryATD" +
                ",com.FirstPickupATD as FirstPickupATD";

            var shipmentMasterFields =
                 "Mas.MainCarriageATD as MainCarriageATD, Mas.Master as Master " +
                ",  Mas.MainCarriageETD  as MainCarriageETD , Mas.MainCarriageATA  as MainCarriageATA " +
                ", Mas.MainCarriageETA  as MainCarriageETA";

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
             "min(AdditionalData.GatepassDocumentsReady) as GatepassDocumentsReady ";


            var selectScript = $"Select {shipmentFields} , {shipmentComputedFields} , {shipmentMasterFields} , {forwardingShipmentFields} , {shipmentAdditionalDataFields} ";




            // From

            var fromScript = $"FROM dbo.{table.DBTableName} P ";


            var joinScript = @"LEFT OUTER JOIN dbo.ShipmentComputedFields com ON com.Id = P.Id 
                            LEFT OUTER JOIN dbo.ShipmentMasterDatas Mas    ON Mas.Id = P.MasterShipmentDataId
                            LEFT OUTER JOIN dbo.ShipmentAdditionalCloudDatas AdditionalData    ON AdditionalData.Id = P.Id ";



            // Where
            List<string> whereConditions = new List<string>();

            var notCustomShipment = $" P.Id NOT IN (SELECT C.Id FROM  dbo.{table.DBTableName } SH " +
                                                    $"JOIN  dbo.{table.DBTableName}  C  ON SH.CustomFileId = C.Id) ";
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
            var groupByScript =
                $" GROUP BY {shipmentFields}" +
                @",com.ContainersNumbers,
                    com.FinalDeliveryETA,
                    com.FinalDeliveryATA,
                    com.FirstPickupATD,
                    com.FinalDeliveryETD,
                    com.FinalDeliveryATD,
                    Mas.Master,
                    Mas.MainCarriageATD,
                    Mas.MainCarriageETD,
                    Mas.MainCarriageATA,
                    Mas.MainCarriageETA";


            string sqlQuery = selectScript + fromScript + joinScript + whereScript + groupByScript;

            return sqlQuery;
        }

    }
}
