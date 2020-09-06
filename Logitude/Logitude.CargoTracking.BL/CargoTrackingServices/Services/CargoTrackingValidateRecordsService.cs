 
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CargoTracking.BL.CargoTrackingServices.Services
{
    public class CargoTrackingValidateRecordsService
    {

        public static bool ValidateRecords(string TableName, SqlDataReader reader)
        {
            bool  IsValid = true;
            if (TableName == "CargoTrackingShipments" || TableName == "CargoTrackingShipmentSearches")
            {
                IsValid= IsRecordFieldsEqualValue(TableName, "ShipmentLevelCode", "C", reader);

            }

            return IsValid;
        }



        private static bool IsRecordFieldsEqualValue<T>(string TableName, string CoulmnName, T FieldValue, SqlDataReader reader)
        {

            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (reader.GetName(i) == CoulmnName && reader.GetValue(i).Equals(FieldValue))
                {
                    return false;
                }
            }

            return true;
        }



    }

}
