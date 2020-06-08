using CargoTracking.CargoTracking.BL.HelperClasses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTracking.CargoTracking.BL.Services
{
    public class CargoTrackingLineByLineMappingService
    {
        public static List<object> RecordValues;
        public static List<string> CoulmnsName;
        public static List<object> Full_CoulmnValues;
        public static List<string> Full_CoulmnNames;
        public static string TableName;
        public static List<object> MappingDB_CTDB()
        {
            if (TableName== "CargoTrackingPorts")
            {
                if (GetValueOfCoulmn("Code") !=null && GetValueOfCoulmn("Code").Equals("AAA"))
                {
                    SetValueOfCoulmn("EnglishName", "Memo");
                }

            }

            if (TableName == "CargoTrackingShipments")
            {
                SetValueOfCoulmn("Master", GetValueOfCoulmn("MasterShipmentDataId"));
                SetValueOfCoulmn("PickupDate", GetValueOfCoulmn("FirstPickupETA"));
                CompareNullabelFirstPickupETADateTime();
            }

            return RecordValues;
        }

        private static void CompareNullabelFirstPickupETADateTime()
        {
            var FirstPickupETA = GetValueOfCoulmn("FirstPickupETA").GetType();
            if (FirstPickupETA.FullName == "System.DBNull")
            {
                SetValueOfCoulmn("PickupDone", false);
            }
            else
            {
                DateTime? lastPostDate = (DateTime?)(GetValueOfCoulmn("FirstPickupETA"));
                DateTime? todayDate = DateTime.Today.Date;
                if (FirstPickupETA != null && lastPostDate.Value.Date < todayDate.Value.Date)
                {
                    SetValueOfCoulmn("PickupDone", true);
                }
                else
                {
                    SetValueOfCoulmn("PickupDone", false);

                }
            }

        }
        private static object GetValueOfCoulmn(string ColumName)
        {
            object TargetValue = null;
            for (int i = 0; i < Full_CoulmnNames.Count(); i++)
            {
                if (Full_CoulmnNames[i] == ColumName)
                {
                    TargetValue = Full_CoulmnValues[i];
                }

            }

            return TargetValue;
        }
        private static void SetValueOfCoulmn(string ColumName, object Recordvalue)
        {
            for (int i = 0; i < CoulmnsName.Count(); i++)
            {
                if (CoulmnsName[i] == ColumName)
                {
                  RecordValues[i]= Recordvalue;
                }

            }

        }

    }

}
