 
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CargoTracking.BL.CargoTrackingServices.Services
{
    public class CargoTrackingCustomMappingService
    {

        public static void MappingDB_CTDB(DataTable dataTable, SqlBulkCopy sbc, string CoulmnName, string TableName)
        {
             AddCustomColumn(dataTable, sbc, CoulmnName, TableName);
        }
        private static void AddCustomColumn(DataTable dataTable, SqlBulkCopy sbc, string ColumnName,string TableName)
        {
            if (TableName == "CargoTrackingShipmentSearches" && ColumnName == "Id")
            {

            }
            else
            {
                if (!dataTable.Columns.Contains(ColumnName))
                {
                    dataTable.Columns.Add(ColumnName);
                }
                sbc.ColumnMappings.Add(ColumnName, ColumnName);
            }
           

        }


    }

}
