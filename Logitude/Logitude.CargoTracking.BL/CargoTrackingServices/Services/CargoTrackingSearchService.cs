 
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

        public static void SearchService(DataRow TableRow, DataTable dataTable, string TableName)
        {
            if (TableName == "CargoTrackingShipmentSearches")
            {
                 AddCustomerRefrences(TableRow, dataTable, "CustomerReference1");
                 AddCustomerRefrences(TableRow, dataTable, "CustomerReference2");
                 AddNewRecord(TableRow, dataTable, "Master");
                 AddNewRecord(TableRow, dataTable, "House");
                if (!TableRow["ShipmentNumber"].Equals(null) && TableRow["ShipmentNumber"].GetType().Name != "DBNull")
                { TableRow.SetField("SearchFields", TableRow["ShipmentNumber"]); }

            }


        }


        private static void AddCustomerRefrences(DataRow TableRow, DataTable dataTable,string CoulmnNmae)
        {
            string SearchField = null;
            if (!TableRow[CoulmnNmae].Equals(null) && TableRow[CoulmnNmae].GetType().Name != "DBNull")
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
            if (!TableRow[CoulmnNmae].Equals(null) && TableRow[CoulmnNmae].GetType().Name != "DBNull")
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
