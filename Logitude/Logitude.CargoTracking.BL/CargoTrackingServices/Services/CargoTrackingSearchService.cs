 
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
                string SearchField = null;
                for (int i = 0; i < 2; i++)
                {
                    SearchField = null;
                    switch (i)
                    {
 
                        case 0:
                            {
                                if (!TableRow["CustomerReference1"].Equals(null) && TableRow["CustomerReference1"].GetType().Name != "DBNull")
                                { SearchField = (string)TableRow["CustomerReference1"]; }

                                break;
                            }
                        case 1:
                            {
                                if (!TableRow["CustomerReference2"].Equals(null) && TableRow["CustomerReference2"].GetType().Name != "DBNull")
                                { SearchField = (string)TableRow["CustomerReference2"]; }
                                break;
                            }

                    }
                    if (SearchField!=null)
                    {
                        DataRow TableRow1 = dataTable.NewRow();
                        TableRow1.ItemArray = TableRow.ItemArray.Clone() as object[];
                        TableRow1.SetField("SearchFields", SearchField);
                        TableRow1.SetField("Id", TableRow["Id"] + "_" + (i+1));
                        dataTable.Rows.Add(TableRow1);
                    }
                    
                }
                if (!TableRow["ShipmentNumber"].Equals(null) && TableRow["ShipmentNumber"].GetType().Name != "DBNull")
                { TableRow.SetField("SearchFields", TableRow["ShipmentNumber"]); }

            }


        }

    }

}
