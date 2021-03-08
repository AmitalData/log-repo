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
            string fielsdName = !string.IsNullOrEmpty(cargoTrackingDataBaseArgs.BuildCargoArgs.Table.FieldsDBName) ? 
                cargoTrackingDataBaseArgs.BuildCargoArgs.Table.FieldsDBName : "*";

            fielsdName = " C." + fielsdName.Replace(",", " ,C.");
            string getAllCustomsShipmentsThatContainForwardingShipmentsCommand 
                = "SELECT " + fielsdName + ", Min(P.Id) as ForwardingIdForCustom" +
                ",com.ContainersNumbers as ContainersNumbers,com.FinalDeliveryETA as FinalDeliveryETA," +
                "com.FinalDeliveryATA as FinalDeliveryATA ,com.FirstPickupATD as FirstPickupATD," +
                " Mas.MainCarriageATD as MainCarriageATD,Mas.Master as Master ,  Mas.MainCarriageETD  as MainCarriageETD" +
                " , Mas.MainCarriageATA  as MainCarriageATA , Mas.MainCarriageETA  as MainCarriageETA "
                + ", min(P.ShipmentNumber) as ForwardingShipmentNumber"
                
                + " FROM dbo." + table.DBTableName + " P JOIN dbo." + table.DBTableName + // P: forwarding shipment
                " C ON P.CustomFileId = C.Id  Left Outer JOIN dbo.ShipmentComputedFields com on com.Id = C.Id  " + // C: custom shipment
                "Left outer JOIN dbo.ShipmentMasterDatas Mas on Mas.Id = C.MasterShipmentDataId ";

            if (cargoTrackingDataBaseArgs.CargoTrackingArguments == null)
            {
                LastUpdate = ServiceHelper.GetTableLastUpdate(cargoTrackingDataBaseArgs.BuildCargoArgs.Table.Main_CargoTracking_TableName,
                    cargoTrackingDataBaseArgs.BuildCargoArgs.DestinationConnectionString);

                getAllCustomsShipmentsThatContainForwardingShipmentsCommand += " where (C.AutomaticLastUpdateDate > '" + LastUpdate + "')";
                    //+" or (Mas.AutomaticLastUpdateDate > '" + LastUpdate + "') or (com.AutomaticLastUpdateDate > '" + LastUpdate + "'))";
            }
            else
            {
                if (cargoTrackingDataBaseArgs.CargoTrackingArguments.Tenant != null)
                    getAllCustomsShipmentsThatContainForwardingShipmentsCommand += " where C.Tenant=" + 
                        cargoTrackingDataBaseArgs.CargoTrackingArguments.Tenant + " and DATEADD(dd, DATEDIFF(dd, 0, P.CreateDateTime ), 0) >= '"
                        + cargoTrackingDataBaseArgs.CargoTrackingArguments.FromDate.Value.Date + 
                        "' and DATEADD(dd, DATEDIFF(dd, 0, P.CreateDateTime ), 0) <= '" + 
                        cargoTrackingDataBaseArgs.CargoTrackingArguments.ToDate.Value.Date + "'";
                else
                    getAllCustomsShipmentsThatContainForwardingShipmentsCommand += " where DATEADD(dd, DATEDIFF(dd, 0, C.CreateDateTime ), 0) >= '" 
                        + cargoTrackingDataBaseArgs.CargoTrackingArguments.FromDate.Value.Date + 
                        "' and DATEADD(dd, DATEDIFF(dd, 0, C.CreateDateTime ), 0) <= '" + 
                        cargoTrackingDataBaseArgs.CargoTrackingArguments.ToDate.Value.Date + "'";
            }

            getAllCustomsShipmentsThatContainForwardingShipmentsCommand += " group by " + fielsdName + ",com.ContainersNumbers,com.FinalDeliveryETA," +
                "com.FinalDeliveryATA,com.FirstPickupATD,Mas.MainCarriageATD,Mas.Master,Mas.MainCarriageETD,Mas.MainCarriageATA,Mas.MainCarriageETA ";


            return getAllCustomsShipmentsThatContainForwardingShipmentsCommand;
        }




        public static string GetAllNonCustomShipmentsThatContainForwardingShipments(CargoTrackingUpdateDataBaseArgs cargoTrackingDataBaseArgs,
            CargoTrackingTable table, string LastUpdate)
        {
            string fieldsName = !string.IsNullOrEmpty(cargoTrackingDataBaseArgs.BuildCargoArgs.Table.FieldsDBName) ?
                cargoTrackingDataBaseArgs.BuildCargoArgs.Table.FieldsDBName : "*";

            fieldsName = " P." + fieldsName.Replace(",", " ,P.");
            string getAllNonCustomShipmentsThatContainForwardingShipmentsCommand = "Select " + fieldsName + ",com.ContainersNumbers as ContainersNumbers," +
                " com.FinalDeliveryETA as FinalDeliveryETA,com.FinalDeliveryATA as FinalDeliveryATA " +
                ",com.FirstPickupATD as FirstPickupATD, Mas.MainCarriageATD as MainCarriageATD, Mas.Master as Master " +
                ",  Mas.MainCarriageETD  as MainCarriageETD , Mas.MainCarriageATA  as MainCarriageATA " +
                ", Mas.MainCarriageETA  as MainCarriageETA,min(P.ShipmentNumber) as ForwardingShipmentNumber "

                + "FROM dbo. " + table.DBTableName +
                " P Left Outer JOIN dbo.ShipmentComputedFields com on com.Id = P.Id " +
                " Left outer JOIN dbo.ShipmentMasterDatas Mas on Mas.Id = P.MasterShipmentDataId " + 
                " Where P.Id not in (Select C.Id From  dbo." + table.DBTableName + " SH JOIN dbo." + table.DBTableName + " C ON SH.CustomFileId = C.Id) ";
            if (cargoTrackingDataBaseArgs.CargoTrackingArguments == null)
            {
                LastUpdate = ServiceHelper.GetTableLastUpdate(cargoTrackingDataBaseArgs.BuildCargoArgs.Table.Main_CargoTracking_TableName,
                    cargoTrackingDataBaseArgs.BuildCargoArgs.DestinationConnectionString);

                getAllNonCustomShipmentsThatContainForwardingShipmentsCommand += " and (P.AutomaticLastUpdateDate > '" + LastUpdate + "')";
                    //+" or (Mas.AutomaticLastUpdateDate > '" + LastUpdate + "') or (com.AutomaticLastUpdateDate > '" + LastUpdate + "'))";
            }
            else
            {
                if (cargoTrackingDataBaseArgs.CargoTrackingArguments.Tenant != null)
                    getAllNonCustomShipmentsThatContainForwardingShipmentsCommand += " and P.Tenant=" + 
                        cargoTrackingDataBaseArgs.CargoTrackingArguments.Tenant + " and DATEADD(dd, DATEDIFF(dd, 0, P.CreateDateTime ), 0) >= '" 
                        + cargoTrackingDataBaseArgs.CargoTrackingArguments.FromDate.Value.Date +
                        "' and DATEADD(dd, DATEDIFF(dd, 0, P.CreateDateTime ), 0) <= '" + 
                        cargoTrackingDataBaseArgs.CargoTrackingArguments.ToDate.Value.Date + "'";
                else
                    getAllNonCustomShipmentsThatContainForwardingShipmentsCommand += " and DATEADD(dd, DATEDIFF(dd, 0, P.CreateDateTime ), 0) >= '" 
                        + cargoTrackingDataBaseArgs.CargoTrackingArguments.FromDate.Value.Date + 
                        "' and DATEADD(dd, DATEDIFF(dd, 0, P.CreateDateTime ), 0) <= '" 
                        + cargoTrackingDataBaseArgs.CargoTrackingArguments.ToDate.Value.Date + "'";
            }

            getAllNonCustomShipmentsThatContainForwardingShipmentsCommand += " group by " + fieldsName + ",com.ContainersNumbers,com.FinalDeliveryETA," +
                "com.FinalDeliveryATA,com.FirstPickupATD,Mas.MainCarriageATD,Mas.MainCarriageETD,Mas.Master,Mas.MainCarriageATA,Mas.MainCarriageETA ";

            return getAllNonCustomShipmentsThatContainForwardingShipmentsCommand;
        }

    }
}
