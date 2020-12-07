
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

        public static void SearchService(DataRow tableRow, BulkDataPreperation bulkDataPreperation, string tableName)
        {
            if (tableName == "CargoTrackingShipmentSearches" || tableName == "CargoTrackingShipments")
            {
                if (bulkDataPreperation.dataTable2 == null)
                {
                    bulkDataPreperation.dataTable2 = bulkDataPreperation.dataTable.Clone();
                }

                AddShipmentNumberReference(tableRow, bulkDataPreperation.dataTable2);
                AddSplittedData(tableRow, bulkDataPreperation.dataTable2, "CustomerReference1");
                AddSplittedData(tableRow, bulkDataPreperation.dataTable2, "CustomerReference2");
                AddNewRecord(tableRow, bulkDataPreperation.dataTable2, "Master");
                AddNewRecord(tableRow, bulkDataPreperation.dataTable2, "House");
                AddNewRecord(tableRow, bulkDataPreperation.dataTable2, "ForwarderShipmentNumber");
                AddNewRecord(tableRow, bulkDataPreperation.dataTable2, "CustomFileNumber");
                AddNewRecord(tableRow, bulkDataPreperation.dataTable2, "CustomsDeclarationNumber");
                AddNewRecord(tableRow, bulkDataPreperation.dataTable2, "ShipperName");
                AddNewRecord(tableRow, bulkDataPreperation.dataTable2, "ConsigneeName");
                AddSplittedData(tableRow, bulkDataPreperation.dataTable2, "ContainersNumbers");


            }


        }
        private static void AddShipmentNumberReference(DataRow tableRow, DataTable dataTable)
        {
            string CoulmnName = "ShipmentNumber";
            AddNewRecord(tableRow, dataTable, CoulmnName);
            var Value = tableRow[CoulmnName];
            string SearchField = (string)Value;
            if (!string.IsNullOrEmpty(SearchField) && SearchField.Contains("/"))
            {
                string SearchArr = SearchField.Split('/')[1];
                AddNewReference(tableRow, dataTable, SearchArr);
            }
            
        }

        private static void AddSplittedData(DataRow tableRow, DataTable dataTable,string coulmnName)
        { 
            if (!IsNullOrEmpty(tableRow, coulmnName))
            {
                var Value = tableRow[coulmnName];
                string SearchField = (string)Value;
                string[] SearchArr = SearchField.Split(',');
                for (int i = 0; i < SearchArr.Length; i++)
                {
                    AddNewReference(tableRow, dataTable, SearchArr[i]);
                }
            } 
        }



        private static void AddNewRecord(DataRow tableRow, DataTable dataTable, string coulmnName)
        {
            if (!IsNullOrEmpty(tableRow, coulmnName))
            {
                    var Value = tableRow[coulmnName];
                    string SearchField = (string)Value;
                    AddNewReference(tableRow, dataTable, SearchField);
            }
        }


        private static void AddNewReference(DataRow tableRow, DataTable dataTable, string searchField)
        {
            DataRow TableRow1 = dataTable.NewRow();
            TableRow1.ItemArray = tableRow.ItemArray.Clone() as object[];
            TableRow1.SetField("SearchFields", searchField.Trim());
            if (!IsNullOrEmpty(TableRow1, "SearchFields"))
                dataTable.Rows.Add(TableRow1);
        }
        private static bool IsNullOrEmpty(DataRow tableRow,  string coulmnName)
        {
            bool IsNull = false;
            var Value = tableRow[coulmnName];
            if (Value.Equals(null) || Value.GetType().Name == "DBNull" || string.IsNullOrEmpty((string)Value) ||  string.IsNullOrWhiteSpace((string)Value))
            {
                IsNull = true;
            }
            return IsNull;
        }
 
    }

}
