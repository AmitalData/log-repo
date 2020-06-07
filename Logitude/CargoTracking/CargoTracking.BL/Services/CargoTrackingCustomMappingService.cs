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

        public static void MappingDB_CTDB(DataTable dataTable, SqlBulkCopy sbc, string CoulmnName)
        {
             AddCustomColumn(dataTable, sbc, CoulmnName);
        }
        private static void AddCustomColumn(DataTable dataTable, SqlBulkCopy sbc, string ColumnName)
        {
            dataTable.Columns.Add(ColumnName);
            sbc.ColumnMappings.Add(ColumnName, ColumnName);
        }
 

    }

}
