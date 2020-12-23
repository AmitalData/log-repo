
using Logitude.CargoTracking.BL.CargoTrackingServices.HelperClasses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Logitude.CargoTracking.BL.CargoTrackingServices.Services.SearchService
{

    public class CargoTrackingSearchService
    {
        public static List<string> PrivateRefrencesList = new List<string>(){ "ConsigneeName", "ShipperName" };

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
                ReferencecArgs ReferencecArgs = new ReferencecArgs()
                {
                    DataTable = dataTable,
                    CoulmnName = CoulmnName,
                    SearchField = SearchArr,
                    TableRow = tableRow,

                };
                AddNewReference(ReferencecArgs);
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
                    ReferencecArgs ReferencecArgs = new ReferencecArgs()
                    {
                        DataTable = dataTable,
                        CoulmnName = coulmnName,
                        SearchField = SearchArr[i],
                        TableRow = tableRow,

                    };
                    AddNewReference(ReferencecArgs);
                }
            } 
        }



        private static void AddNewRecord(DataRow tableRow, DataTable dataTable, string coulmnName)
        {
            if (!IsNullOrEmpty(tableRow, coulmnName))
            {
                var Value = tableRow[coulmnName];
                string SearchField = (string)Value;
                ReferencecArgs ReferencecArgs = new ReferencecArgs()
                {
                    DataTable = dataTable,
                    CoulmnName = coulmnName,
                    SearchField = SearchField,
                    TableRow = tableRow,

                };
                AddNewReference(ReferencecArgs);
            }
        }


        private static void AddNewReference(ReferencecArgs referencecArgs)
        {
            DataRow TableRow1 = referencecArgs.DataTable.NewRow();
            TableRow1.ItemArray = referencecArgs.TableRow.ItemArray.Clone() as object[];
            TableRow1.SetField("SearchFields", referencecArgs.SearchField.Trim());
            TableRow1.SetField("ReferenceType", GetReferenceTypeFromCoulmnName(referencecArgs.CoulmnName));
            SetIsPublicForCoulmn(TableRow1, referencecArgs.CoulmnName);
            if (!IsNullOrEmpty(TableRow1, "SearchFields"))
                referencecArgs.DataTable.Rows.Add(TableRow1);
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

        private static void SetIsPublicForCoulmn(DataRow TableRow, string coulmnName)
        {
            bool IsPublic = true;
            if (PrivateRefrencesList.Contains(coulmnName))
                IsPublic = false;
 
            TableRow.SetField("IsPublic", IsPublic);

        }

        private static string GetReferenceTypeFromCoulmnName(string CoulmnName)
        {
            string ReferenceType = Regex.Replace(CoulmnName, "([a-z])([A-Z])", "$1 $2");
            return ReferenceType;
        }

    }

}
