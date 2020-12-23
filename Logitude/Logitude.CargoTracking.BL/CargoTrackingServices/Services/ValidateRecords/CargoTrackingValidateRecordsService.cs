 
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

        public static bool ValidateRecords(string TableName, SqlDataReader reader)
        {
            bool  IsValid = true;
            if (TableName == "CargoTrackingShipments" || TableName == "CargoTrackingShipmentSearches")
            {
                IsValid= IsRecordFieldsValid("ShipmentLevelCode", "C", reader); // C Equal Consol not custom
                if(IsValid)
                IsValid = IsRecordFieldsValid("IsCancelled", true, reader); // C Equal Consol not custom

            }

            return IsValid;
        }



        private static bool IsRecordFieldsValid<T>(string CoulmnName, T FieldValue, SqlDataReader reader)
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
