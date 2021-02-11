 
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CargoTracking.BL.CargoTrackingServices.Services.ValidateRecords
{
    public class CargoTrackingValidateRecordsService
    {

        public static bool ValidateRecords(string tableName, SqlDataReader reader)
        {
            bool  isValid = true;
            if (tableName == "CargoTrackingShipments" || tableName == "CargoTrackingShipmentSearches")
            {
                isValid= IsRecordFieldsValid("ShipmentLevelCode", "C", reader); // C Equal Consol not custom
                if(isValid)
                isValid = IsRecordFieldsValid("IsCancelled", true, reader); // C Equal Consol not custom

            }

            return isValid;
        }



        private static bool IsRecordFieldsValid<T>(string coulmnName, T fieldValue, SqlDataReader reader)
        {

            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (reader.GetName(i) == coulmnName && reader.GetValue(i).Equals(fieldValue))
                {
                    return false;
                }
            }

            return true;
        }



    }

}
