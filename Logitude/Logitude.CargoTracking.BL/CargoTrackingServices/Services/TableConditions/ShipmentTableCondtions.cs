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



        public static string GetFirstConditions(string fieldName ,CargoTrackingUpdateDataBaseArgs cargoTrackingDataBaseArgs, CargoTable table, string LastUpdate)
        {
            string cmd = "";

            fieldName = " C." + fieldName.Replace(",", " ,C.");
            cmd = "SELECT " + fieldName + ", Min(P.Id) as ForwardingIdForCustom,com.ContainersNumbers as ContainersNumbers,com.FinalDeliveryETA as FinalDeliveryETA,com.FinalDeliveryATA as FinalDeliveryATA ,com.FirstPickupATD as FirstPickupATD, Mas.MainCarriageATD as MainCarriageATD,Mas.Master as Master ,  Mas.MainCarriageETD  as MainCarriageETD , Mas.MainCarriageATA  as MainCarriageATA , Mas.MainCarriageETA  as MainCarriageETA " + " FROM dbo." + table.DBTableName + " P JOIN dbo." + table.DBTableName + " C ON P.CustomFileId = C.Id  Left Outer JOIN dbo.ShipmentComputedFields com on com.Id = C.Id  Left outer JOIN dbo.ShipmentMasterDatas Mas on Mas.Id = C.Id ";

            if (cargoTrackingDataBaseArgs.CargoTrackingArguments == null)
            {
                LastUpdate = ServiceHelper.GetTableLastUpdate(cargoTrackingDataBaseArgs.buildCargoArgs.Table.CT_TableName, cargoTrackingDataBaseArgs.buildCargoArgs.DestinationConnectionString);
                cmd += " where (C.AutomaticLastUpdateDate > '" + LastUpdate + "')";
            }
            else
            {
                if (cargoTrackingDataBaseArgs.CargoTrackingArguments.Tenant != null)
                {
                    cmd += " where C.Tenant=" + cargoTrackingDataBaseArgs.CargoTrackingArguments.Tenant + " and C.CreateDateTime >= '" + cargoTrackingDataBaseArgs.CargoTrackingArguments.FromDate.Value.Date + "' and C.CreateDateTime <= '" + cargoTrackingDataBaseArgs.CargoTrackingArguments.ToDate.Value.Date + "'";

                }
                else
                {
                    cmd += " where C.CreateDateTime >= '" + cargoTrackingDataBaseArgs.CargoTrackingArguments.FromDate.Value.Date + "' and C.CreateDateTime <= '" + cargoTrackingDataBaseArgs.CargoTrackingArguments.ToDate.Value.Date + "'";
                }
            }

            cmd += " group by " + fieldName + ",com.ContainersNumbers,com.FinalDeliveryETA,com.FinalDeliveryATA,com.FirstPickupATD,Mas.MainCarriageATD,Mas.Master,Mas.MainCarriageETD,Mas.MainCarriageATA,Mas.MainCarriageETA ";


            return cmd;
        }




        public static string GetSecoundConditions(string fieldName, CargoTrackingUpdateDataBaseArgs cargoTrackingDataBaseArgs, CargoTable table, string LastUpdate)
        {
            string cmd = "";

            fieldName = " P." + fieldName.Replace(",", " ,P.");
            cmd = "Select " + fieldName + ",com.ContainersNumbers as ContainersNumbers, com.FinalDeliveryETA as FinalDeliveryETA,com.FinalDeliveryATA as FinalDeliveryATA ,com.FirstPickupATD as FirstPickupATD, Mas.MainCarriageATD as MainCarriageATD, Mas.Master as Master ,  Mas.MainCarriageETD  as MainCarriageETD , Mas.MainCarriageATA  as MainCarriageATA , Mas.MainCarriageETA  as MainCarriageETA FROM dbo. " + table.DBTableName + " P Left Outer JOIN dbo.ShipmentComputedFields com on com.Id = P.Id  Left outer JOIN dbo.ShipmentMasterDatas Mas on Mas.Id = P.Id " + " Where P.Id not in (Select C.Id From  dbo." + table.DBTableName + " SH JOIN dbo." + table.DBTableName + " C ON SH.CustomFileId = C.Id) ";


            if (cargoTrackingDataBaseArgs.CargoTrackingArguments == null)
            {
                LastUpdate = ServiceHelper.GetTableLastUpdate(cargoTrackingDataBaseArgs.buildCargoArgs.Table.CT_TableName, cargoTrackingDataBaseArgs.buildCargoArgs.DestinationConnectionString);
                cmd += " and (P.AutomaticLastUpdateDate > '" + LastUpdate + "')";
            }
            else
            {
                if (cargoTrackingDataBaseArgs.CargoTrackingArguments.Tenant != null)
                {
                    cmd += " and P.Tenant=" + cargoTrackingDataBaseArgs.CargoTrackingArguments.Tenant + " and P.CreateDateTime >= '" + cargoTrackingDataBaseArgs.CargoTrackingArguments.FromDate.Value.Date + "' and P.CreateDateTime <= '" + cargoTrackingDataBaseArgs.CargoTrackingArguments.ToDate.Value.Date + "'";

                }
                else
                {
                    cmd += " and P.CreateDateTime >= '" + cargoTrackingDataBaseArgs.CargoTrackingArguments.FromDate.Value.Date + "' and P.CreateDateTime <= '" + cargoTrackingDataBaseArgs.CargoTrackingArguments.ToDate.Value.Date + "'";
                }
            }

            cmd += " group by " + fieldName + ",com.ContainersNumbers,com.FinalDeliveryETA,com.FinalDeliveryATA,com.FirstPickupATD,Mas.MainCarriageATD,Mas.MainCarriageETD,Mas.Master,Mas.MainCarriageATA,Mas.MainCarriageETA ";

            return cmd;
        }

    }
}
