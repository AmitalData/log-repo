
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

                AddShipmentNumberReference(TableRow, bulkDataPreperation.dataTable2);
                AddSplittedData(TableRow, bulkDataPreperation.dataTable2, "CustomerReference1");
                AddSplittedData(TableRow, bulkDataPreperation.dataTable2, "CustomerReference2");
                AddNewRecord(TableRow, bulkDataPreperation.dataTable2, "Master");
                AddNewRecord(TableRow, bulkDataPreperation.dataTable2, "House");
                AddNewRecord(TableRow, bulkDataPreperation.dataTable2, "ForwarderShipmentNumber");
                AddNewRecord(TableRow, bulkDataPreperation.dataTable2, "CustomFileNumber");
                AddNewRecord(TableRow, bulkDataPreperation.dataTable2, "CustomsDeclarationNumber");
                AddNewRecord(TableRow, bulkDataPreperation.dataTable2, "ShipperName");
                AddNewRecord(TableRow, bulkDataPreperation.dataTable2, "ConsigneeName");
                AddSplittedData(TableRow, bulkDataPreperation.dataTable2, "ContainersNumbers");


            }


        }
        private static void AddShipmentNumberReference(DataRow TableRow, DataTable dataTable)
        {
            string CoulmnName = "ShipmentNumber";
            AddNewRecord(TableRow, dataTable, CoulmnName);
            var Value = TableRow[CoulmnName];
            string SearchField = (string)Value;
            if (!string.IsNullOrEmpty(SearchField) && SearchField.Contains("/"))
            {
                string SearchArr = SearchField.Split('/')[1];
                AddNewReference(TableRow, dataTable, SearchArr);
            }
            
        }

        private static void AddSplittedData(DataRow TableRow, DataTable dataTable,string CoulmnName)
        { 
            if (!IsNullOrEmpty(TableRow, CoulmnName))
            {
                var Value = TableRow[CoulmnName];
                string SearchField = (string)Value;
                string[] SearchArr = SearchField.Split(',');
                for (int i = 0; i < SearchArr.Length; i++)
                {
                    AddNewReference(TableRow, dataTable, SearchArr[i]);
                }
            } 
        }



        private static void AddNewRecord(DataRow TableRow, DataTable dataTable, string CoulmnName)
        {
            if (!IsNullOrEmpty(TableRow, CoulmnName))
            {
                    var Value = TableRow[CoulmnName];
                    string SearchField = (string)Value;
                    AddNewReference(TableRow, dataTable, SearchField);
            }
        }


        private static void AddNewReference(DataRow TableRow, DataTable dataTable, string SearchField)
        {
            DataRow TableRow1 = dataTable.NewRow();
            TableRow1.ItemArray = TableRow.ItemArray.Clone() as object[];
            TableRow1.SetField("SearchFields", SearchField.Trim());
            if (!IsNullOrEmpty(TableRow1, "SearchFields"))
                dataTable.Rows.Add(TableRow1);
        }
        private static bool IsNullOrEmpty(DataRow TableRow,  string CoulmnName)
        {
            bool IsNull = false;
            var Value = TableRow[CoulmnName];
            if (Value.Equals(null) || Value.GetType().Name == "DBNull" || string.IsNullOrEmpty((string)Value) ||  string.IsNullOrWhiteSpace((string)Value))
            {
                IsNull = true;
            }
            return IsNull;
        }
 
    }

}
