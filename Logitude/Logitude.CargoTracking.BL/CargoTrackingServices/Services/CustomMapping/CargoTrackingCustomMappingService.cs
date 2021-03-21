
using Logitude.CargoTracking.BL.CargoTrackingServices.HelperClasses;
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

        public static void MapCargoTrackingToDataBase(BulkDataPreperation bulkDataPreperation, SqlBulkCopy sqlBulkCopy, string tableName)
        {
             AddCustomColumn(bulkDataPreperation, sqlBulkCopy, tableName);
        }
        private static void AddCustomColumn(BulkDataPreperation bulkDataPreperation, SqlBulkCopy sqlBulkCopy,string tableName)
        {
            if (tableName == "CargoTrackingShipmentSearches" ||
                tableName == "CargoTrackingShipments")
            {
                if (bulkDataPreperation.CoulmnForCusstomMapping != "Id")
                {
                    AutoCoulmnMap(bulkDataPreperation.SelectedDataTable, sqlBulkCopy, bulkDataPreperation.CoulmnForCusstomMapping);
                }

            }
            else
            {
                AutoCoulmnMap(bulkDataPreperation.SelectedDataTable, sqlBulkCopy, bulkDataPreperation.CoulmnForCusstomMapping);
            }
           

        }


        private static void AutoCoulmnMap(DataTable dataTable, SqlBulkCopy sqlBulkCopy, string columnName)
        {
            if (dataTable.Columns.IndexOf(columnName) ==-1)
            {
                DataColumn dataColumn = new DataColumn(columnName);
                dataColumn.AllowDBNull = true;
                dataTable.Columns.Add(dataColumn);
            }
            sqlBulkCopy.ColumnMappings.Add(columnName, columnName);
        }


    }

}
