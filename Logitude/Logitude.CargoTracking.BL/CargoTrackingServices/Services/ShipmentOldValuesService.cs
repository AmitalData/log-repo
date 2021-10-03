using Logitude.CargoTracking.BL.CargoTrackingServices.HelperClasses;
using Logitude.CargoTracking.BL.CargoTrackingServices.Services.ServicesHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CargoTracking.BL.CargoTrackingServices.Services
{
    public class ShipmentOldValuesService
    {
        public void CreatePreOldDataDBTable(CargoTrackingArgs cargoArgs)
        {
            var sql = BuildScriptForCreatePreOldCargoShipment();
            ExcuteSqlScript(cargoArgs, sql);
        }
        public void CreateOldCargoShipmentsFromPreOldCargoShipments(CargoTrackingArgs cargoArgs)
        {
            var sql = BuildScriptForCreatePreOldCargoShipment();
            ExcuteSqlScript(cargoArgs, sql);
        }
        private string BuildScriptForCreatePreOldCargoShipment()
        {
            var sql = string.Concat(
                        $"IF OBJECT_ID(N'dbo.PreOldCargoShipments', N'U') IS NOT NULL  drop table PreOldCargoShipments; " ,
                        $"select  * into PreOldCargoShipments from CargoTrackingShipments");
            return sql;
        }
        private string BuildScriptForCreateOldCargoShipmentsFromPreOldCargoShipments()
        {
            var sql =
                       $"IF OBJECT_ID(N'dbo.OldCargoShipments', N'U') IS NOT NULL drop table OldCargoShipments;" + Environment.NewLine +
                       $"IF OBJECT_ID(N'dbo.PreOldCargoShipments', N'U') IS NOT NULL " + Environment.NewLine +
                       $"BEGIN" + Environment.NewLine +
                       $"   select  * into OldCargoShipments from PreOldCargoShipments;" + Environment.NewLine +
                       $"   drop table PreOldCargoShipments;" + Environment.NewLine +
                       $"END";
            return sql;
        }
        private void ExcuteSqlScript(CargoTrackingArgs cargoArgs, string sql)
        {
            ServiceHelper.ExecuteSql(sql, cargoArgs.DestinationConnectionString);
        }

    }
}