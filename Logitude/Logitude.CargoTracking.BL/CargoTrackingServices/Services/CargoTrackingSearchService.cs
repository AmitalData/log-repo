
using Logitude.CargoTracking.BL.CargoTrackingServices.HelperClasses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CargoTracking.BL.CargoTrackingServices.Services
{
    public class CargoTrackingSearchService
    {

        public static void SearchService(DataRow TableRow, BulkDataPreperation bulkDataPreperation, string TableName)
        {
            if (TableName == "CargoTrackingShipmentSearches" || TableName == "CargoTrackingShipments")
            {
                if (bulkDataPreperation.dataTable2 == null)
                {
                    bulkDataPreperation.dataTable2 = bulkDataPreperation.dataTable.Clone();
                }

                AddNewRecord(TableRow, bulkDataPreperation.dataTable2, "ShipmentNumber");
                AddCustomerRefrences(TableRow, bulkDataPreperation.dataTable2, "CustomerReference1");
                AddCustomerRefrences(TableRow, bulkDataPreperation.dataTable2, "CustomerReference2");
                AddNewRecord(TableRow, bulkDataPreperation.dataTable2, "Master");
                AddNewRecord(TableRow, bulkDataPreperation.dataTable2, "House");
                AddNewRecord(TableRow, bulkDataPreperation.dataTable2, "ForwarderShipmentNumber");
                AddNewRecord(TableRow, bulkDataPreperation.dataTable2, "CustomFileNumber");
                AddNewRecord(TableRow, bulkDataPreperation.dataTable2, "CustomsDeclarationNumber");
                AddNewRecord(TableRow, bulkDataPreperation.dataTable2, "ShipperName");
                AddNewRecord(TableRow, bulkDataPreperation.dataTable2, "ConsigneeName");
                //AddNewRecord(TableRow, bulkDataPreperation.dataTable2, "ContainersNumbers");


            }


        }


        private static void AddCustomerRefrences(DataRow TableRow, DataTable dataTable,string CoulmnNmae)
        {
            string SearchField = null;
            var Value = TableRow[CoulmnNmae];
            if (!Value.Equals(null) && Value.GetType().Name != "DBNull" && !string.IsNullOrEmpty((string)Value) && !string.IsNullOrWhiteSpace((string)Value))
            { SearchField = (string)TableRow[CoulmnNmae]; 

            string[] SearchArr = SearchField.Split(',');
            for (int i = 0; i < SearchArr.Length; i++)
            {
  
                    DataRow TableRow1 = dataTable.NewRow();
                    TableRow1.ItemArray = TableRow.ItemArray.Clone() as object[];
                    TableRow1.SetField("SearchFields", SearchArr[i]);
                    dataTable.Rows.Add(TableRow1);
         

            }
           } 
        }



        private static void AddNewRecord(DataRow TableRow, DataTable dataTable, string CoulmnNmae)
        {
            string SearchField = null;
            var Value = TableRow[CoulmnNmae];
            if (!Value.Equals(null) && Value.GetType().Name != "DBNull" && !string.IsNullOrEmpty((string)Value) && !string.IsNullOrWhiteSpace((string)Value))
            {
                    SearchField = (string)TableRow[CoulmnNmae];
                    DataRow TableRow1 = dataTable.NewRow();
                    TableRow1.ItemArray = TableRow.ItemArray.Clone() as object[];
                    TableRow1.SetField("SearchFields", SearchField);
                    dataTable.Rows.Add(TableRow1);
 
            }
        }

 
    }

}
