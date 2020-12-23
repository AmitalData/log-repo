 
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CargoTracking.BL.CargoTrackingServices.Services.CustomMapping
{
    public class CargoTrackingCustomMappingService
    {

        public static void MappingDB_CTDB(DataTable dataTable, SqlBulkCopy sbc, string CoulmnName, string TableName)
        {
             AddCustomColumn(dataTable, sbc, CoulmnName, TableName);
        }
        private static void AddCustomColumn(DataTable dataTable, SqlBulkCopy sbc, string ColumnName,string TableName)
        {
            if (TableName == "CargoTrackingShipmentSearches"  )
            {
                if (ColumnName != "Id")
                {
                    AutoCoulmnMap(dataTable, sbc, ColumnName);
                }
            }
            else if (TableName == "CargoTrackingShipments")
            {
                if (ColumnName != "Id")
                {
                    AutoCoulmnMap(dataTable, sbc, ColumnName);
                }
            }
            else
            {
                AutoCoulmnMap(dataTable, sbc, ColumnName);
            }
           

        }


        private static void AutoCoulmnMap(DataTable dataTable, SqlBulkCopy sbc, string ColumnName)
        {
            if (!dataTable.Columns.Contains(ColumnName))
            {
                dataTable.Columns.Add(ColumnName);
            }
            sbc.ColumnMappings.Add(ColumnName, ColumnName);
        }


    }

}
