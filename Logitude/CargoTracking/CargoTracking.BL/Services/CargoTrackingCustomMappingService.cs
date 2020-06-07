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
    public class CargoTrackingCustomMappingService
    {

        public static  void MappingDB_CTDB(DataTable dataTable, CargoTable Table)
        {
            if (Table.CT_TableName== "CargoTrackingPorts")
            {
                dataTable.Columns.Add("EnglishName");
            }
        }


    }
 
}
