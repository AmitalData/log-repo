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



        public static string GetFirstConditions(string fieldName ,CargoTrackingUpdateDataBaseArgs cargoTrackingDataBaseArgs, CargoTrackingTable table, string LastUpdate)
        {
            fieldName = " C." + fieldName.Replace(",", " ,C.");
            string cmd = "SELECT " + fieldName + ", Min(P.Id) as ForwardingIdForCustom,com.ContainersNumbers as ContainersNumbers,com.FinalDeliveryETA as FinalDeliveryETA,com.FinalDeliveryATA as FinalDeliveryATA ,com.FirstPickupATD as FirstPickupATD, Mas.MainCarriageATD as MainCarriageATD,Mas.Master as Master ,  Mas.MainCarriageETD  as MainCarriageETD , Mas.MainCarriageATA  as MainCarriageATA , Mas.MainCarriageETA  as MainCarriageETA " + " FROM dbo." + table.DBTableName + " P JOIN dbo." + table.DBTableName + " C ON P.CustomFileId = C.Id  Left Outer JOIN dbo.ShipmentComputedFields com on com.Id = C.Id  Left outer JOIN dbo.ShipmentMasterDatas Mas on Mas.Id = C.Id ";
            if (cargoTrackingDataBaseArgs.CargoTrackingArguments == null)
            {
                LastUpdate = ServiceHelper.GetTableLastUpdate(cargoTrackingDataBaseArgs.BuildCargoArgs.Table.CargoTracking_TableName, cargoTrackingDataBaseArgs.BuildCargoArgs.DestinationConnectionString);
                cmd += " where (C.AutomaticLastUpdateDate > '" + LastUpdate + "')";
            }
            else
            {
                if (cargoTrackingDataBaseArgs.CargoTrackingArguments.Tenant != null)
                    cmd += " where C.Tenant=" + cargoTrackingDataBaseArgs.CargoTrackingArguments.Tenant + " and DATEADD(dd, DATEDIFF(dd, 0, P.CreateDateTime ), 0) >= '" + cargoTrackingDataBaseArgs.CargoTrackingArguments.FromDate.Value.Date + "' and DATEADD(dd, DATEDIFF(dd, 0, P.CreateDateTime ), 0) <= '" + cargoTrackingDataBaseArgs.CargoTrackingArguments.ToDate.Value.Date + "'";
                else
                    cmd += " where DATEADD(dd, DATEDIFF(dd, 0, C.CreateDateTime ), 0) >= '" + cargoTrackingDataBaseArgs.CargoTrackingArguments.FromDate.Value.Date + "' and DATEADD(dd, DATEDIFF(dd, 0, C.CreateDateTime ), 0) <= '" + cargoTrackingDataBaseArgs.CargoTrackingArguments.ToDate.Value.Date + "'";
            }

            cmd += " group by " + fieldName + ",com.ContainersNumbers,com.FinalDeliveryETA,com.FinalDeliveryATA,com.FirstPickupATD,Mas.MainCarriageATD,Mas.Master,Mas.MainCarriageETD,Mas.MainCarriageATA,Mas.MainCarriageETA ";


            return cmd;
        }




        public static string GetSecoundConditions(string fieldName, CargoTrackingUpdateDataBaseArgs cargoTrackingDataBaseArgs, CargoTrackingTable table, string LastUpdate)
        {
            fieldName = " P." + fieldName.Replace(",", " ,P.");
            string cmd = "Select " + fieldName + ",com.ContainersNumbers as ContainersNumbers, com.FinalDeliveryETA as FinalDeliveryETA,com.FinalDeliveryATA as FinalDeliveryATA ,com.FirstPickupATD as FirstPickupATD, Mas.MainCarriageATD as MainCarriageATD, Mas.Master as Master ,  Mas.MainCarriageETD  as MainCarriageETD , Mas.MainCarriageATA  as MainCarriageATA , Mas.MainCarriageETA  as MainCarriageETA FROM dbo. " + table.DBTableName + " P Left Outer JOIN dbo.ShipmentComputedFields com on com.Id = P.Id  Left outer JOIN dbo.ShipmentMasterDatas Mas on Mas.Id = P.Id " + " Where P.Id not in (Select C.Id From  dbo." + table.DBTableName + " SH JOIN dbo." + table.DBTableName + " C ON SH.CustomFileId = C.Id) ";


            if (cargoTrackingDataBaseArgs.CargoTrackingArguments == null)
            {
                LastUpdate = ServiceHelper.GetTableLastUpdate(cargoTrackingDataBaseArgs.BuildCargoArgs.Table.CargoTracking_TableName, cargoTrackingDataBaseArgs.BuildCargoArgs.DestinationConnectionString);
                cmd += " and (P.AutomaticLastUpdateDate > '" + LastUpdate + "')";
            }
            else
            {
                if (cargoTrackingDataBaseArgs.CargoTrackingArguments.Tenant != null)
                    cmd += " and P.Tenant=" + cargoTrackingDataBaseArgs.CargoTrackingArguments.Tenant + " and DATEADD(dd, DATEDIFF(dd, 0, P.CreateDateTime ), 0) >= '" + cargoTrackingDataBaseArgs.CargoTrackingArguments.FromDate.Value.Date + "' and DATEADD(dd, DATEDIFF(dd, 0, P.CreateDateTime ), 0) <= '" + cargoTrackingDataBaseArgs.CargoTrackingArguments.ToDate.Value.Date + "'";
                else
                    cmd += " and DATEADD(dd, DATEDIFF(dd, 0, P.CreateDateTime ), 0) >= '" + cargoTrackingDataBaseArgs.CargoTrackingArguments.FromDate.Value.Date + "' and DATEADD(dd, DATEDIFF(dd, 0, P.CreateDateTime ), 0) <= '" + cargoTrackingDataBaseArgs.CargoTrackingArguments.ToDate.Value.Date + "'";
            }

            cmd += " group by " + fieldName + ",com.ContainersNumbers,com.FinalDeliveryETA,com.FinalDeliveryATA,com.FirstPickupATD,Mas.MainCarriageATD,Mas.MainCarriageETD,Mas.Master,Mas.MainCarriageATA,Mas.MainCarriageETA ";

            return cmd;
        }

    }
}
