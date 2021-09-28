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
        public void CreateOldDataDBTable(CargoTrackingArgs cargoArgs)
        {
            var sql = BuildScriptForCreateShipmentOldDataValues();
            ExcuteSqlScript(cargoArgs, sql);
        }
       
        private string BuildScriptForCreateShipmentOldDataValues()
        {
            var sql = string.Concat(
                        $"IF OBJECT_ID(N'dbo.PreOldCargoShipments', N'U') IS NOT NULL  drop table PreOldCargoShipments;select  * into PreOldCargoShipments from CargoTrackingShipments");
            return sql;
        }
       
        private void ExcuteSqlScript(CargoTrackingArgs cargoArgs, string sql)
        {
            ServiceHelper.ExecuteSql(sql, cargoArgs.DestinationConnectionString);
        }

    }
}