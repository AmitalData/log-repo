
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

        public static void MappingDB_CTDB(BulkDataPreperation bulkDataPreperation, SqlBulkCopy SqlBulkCopy, string TableName)
        {
             AddCustomColumn(bulkDataPreperation, SqlBulkCopy,TableName);
        }
        private static void AddCustomColumn(BulkDataPreperation bulkDataPreperation, SqlBulkCopy SqlBulkCopy,string TableName)
        {
            if (TableName == "CargoTrackingShipmentSearches" || 
                TableName == "CargoTrackingShipments")
            {
                if (bulkDataPreperation.CoulmnForCusstomMapping != "Id")
                {
                    AutoCoulmnMap(bulkDataPreperation.SelectedDataTable, SqlBulkCopy, bulkDataPreperation.CoulmnForCusstomMapping);
                }

            }
            else
            {
                AutoCoulmnMap(bulkDataPreperation.SelectedDataTable, SqlBulkCopy, bulkDataPreperation.CoulmnForCusstomMapping);
            }
           

        }


        private static void AutoCoulmnMap(DataTable dataTable, SqlBulkCopy sbc, string ColumnName)
        {
            if (dataTable.Columns.IndexOf(ColumnName)==-1)
            {
                dataTable.Columns.Add(ColumnName);
            }
            sbc.ColumnMappings.Add(ColumnName, ColumnName);
        }


    }

}
