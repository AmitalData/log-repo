
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

    public static class CargoTrackingSearchService
    {

        public static List<string> PrivateRefrencesList = new List<string>() { "ConsigneeName", "ShipperName" };
        public static List<string> PrivateShipmentTypes = new List<string> { "LCLD", "MYGO", "MYGI" };

        public static void CreateSearchReferencesForShipment(DataRow tableRow, BulkDataPreperation bulkDataPreperation, string tableName)
        {
            if (tableName == "CargoTrackingShipmentSearches" || tableName == "CargoTrackingShipments")
            {
                if (bulkDataPreperation.InnerDataTable == null)
                    bulkDataPreperation.InnerDataTable = bulkDataPreperation.MainDataTable.Clone();

                CreateShipmentRefences(tableRow, bulkDataPreperation);
            }


        }
        private static void CreateShipmentRefences(DataRow tableRow, BulkDataPreperation bulkDataPreperation)
        {
            bool isValidToCreateRefrences = IsShipmentValidToCreateRefrences(tableRow, bulkDataPreperation);
            if (isValidToCreateRefrences)
            {
                AddShipmentNumberReference(tableRow, bulkDataPreperation.InnerDataTable);
                AddSplittedData(new SplittedDataArguments
                    .Builder()
                    .TableRow(tableRow)
                    .DataTable(bulkDataPreperation.InnerDataTable)
                    .CoulmnName("CustomerReference1")
                    .Delimiter(',')
                    .Build());

                AddSplittedData(new SplittedDataArguments
                    .Builder()
                    .TableRow(tableRow)
                    .DataTable(bulkDataPreperation.InnerDataTable)
                    .CoulmnName("CustomerReference2")
                    .Delimiter(',')
                    .Build());

                AddSplittedData(new SplittedDataArguments
                    .Builder()
                    .TableRow(tableRow)
                    .DataTable(bulkDataPreperation.InnerDataTable)
                    .CoulmnName("ContainersNumbers")
                    .Delimiter(',')
                    .Build());

                AddSplittedData(new SplittedDataArguments
                    .Builder()
                    .TableRow(tableRow)
                    .DataTable(bulkDataPreperation.InnerDataTable)
                    .CoulmnName("House")
                    .Delimiter('-')
                    .Build());
                AddForwardingShipmentNumberReference(tableRow, bulkDataPreperation.InnerDataTable);

                AddNewRecord(tableRow, bulkDataPreperation.InnerDataTable, "Master");
                AddNewRecord(tableRow, bulkDataPreperation.InnerDataTable, "ForwarderShipmentNumber");
                AddNewRecord(tableRow, bulkDataPreperation.InnerDataTable, "CustomFileNumber");
                AddNewRecord(tableRow, bulkDataPreperation.InnerDataTable, "CustomsDeclarationNumber");
                AddNewRecord(tableRow, bulkDataPreperation.InnerDataTable, "ShipperName");
                AddNewRecord(tableRow, bulkDataPreperation.InnerDataTable, "ConsigneeName");
                AddNewRecord(tableRow, bulkDataPreperation.InnerDataTable, "ForwarderShipmentNumber");
                SaveTheWholeHouseReferenceinSearchTable(tableRow, bulkDataPreperation);
            }

        }

        private static void SaveTheWholeHouseReferenceinSearchTable(DataRow tableRow, BulkDataPreperation bulkDataPreperation)
        {
            if (tableRow["House"].ToString().Contains('-'))
            {
                AddNewRecord(tableRow, bulkDataPreperation.InnerDataTable, "House");
            }

        }

        private static void AddForwardingShipmentNumberReference(DataRow tableRow, DataTable innerDataTable)
        {
            if (tableRow["ShipmentLevelCode"].Equals("A"))
            {
                AddNewRecord(tableRow, innerDataTable, "ForwardingShipmentNumber");
            }
        }

        private static bool IsShipmentValidToCreateRefrences(DataRow tableRow, BulkDataPreperation bulkDataPreperation)
        {
            bool isShipmentValid = true;
            if (bulkDataPreperation.CargoTrackingUpdateDataBaseArgs.CargoTrackingArguments!=null)
            {
                DateTime shipmetnCreateDate = (DateTime)(tableRow["CreateDateTime"]);
                DateTime cargTrackingBuildToDate = (DateTime)bulkDataPreperation.CargoTrackingUpdateDataBaseArgs.CargoTrackingArguments.ToDate;
                DateTime cargTrackingBuildFromDate = (DateTime)bulkDataPreperation.CargoTrackingUpdateDataBaseArgs.CargoTrackingArguments.FromDate;
                int differenceMonthsBetweenBuildToAndFromDate = SetDifferenceMonthsBetweenBuildToAndFromDate(cargTrackingBuildToDate, cargTrackingBuildFromDate);
                if(differenceMonthsBetweenBuildToAndFromDate>=6)
                   cargTrackingBuildFromDate = cargTrackingBuildToDate.AddMonths(-differenceMonthsBetweenBuildToAndFromDate);
                if (shipmetnCreateDate.Date < cargTrackingBuildFromDate.Date ||
                    shipmetnCreateDate.Date > cargTrackingBuildToDate.Date)
                {
                    isShipmentValid = false;
                }
            }

            return isShipmentValid;
        }

        private static int SetDifferenceMonthsBetweenBuildToAndFromDate( DateTime cargTrackingBuildToDate , DateTime cargTrackingBuildFromDate)
        {
            int differenceMonthsBetweenBuildToAndFromDate = MonthDifference(cargTrackingBuildToDate, cargTrackingBuildFromDate);
            if (differenceMonthsBetweenBuildToAndFromDate >= 6)
                differenceMonthsBetweenBuildToAndFromDate = 6;

            return differenceMonthsBetweenBuildToAndFromDate;
        }

        private static int MonthDifference(this DateTime toDate, DateTime fromDate)
        {
            return (toDate.Month - fromDate.Month) + 12 * (toDate.Year - fromDate.Year);
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

        private static void AddSplittedData(SplittedDataArguments splittedDataArguments)
        {
            if (!IsNullOrEmpty(splittedDataArguments.TableRow, splittedDataArguments.CoulmnName))
            {
                var Value = splittedDataArguments.TableRow[splittedDataArguments.CoulmnName];
                string SearchField = (string)Value;
                string[] SearchArr = SearchField.Split(splittedDataArguments.Delimiter);
                for (int i = 0; i < SearchArr.Length; i++)
                {
                    ReferencecArgs ReferencecArgs = new ReferencecArgs()
                    {
                        DataTable = splittedDataArguments.DataTable,
                        CoulmnName = splittedDataArguments.CoulmnName,
                        SearchField = SearchArr[i],
                        TableRow = splittedDataArguments.TableRow,

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
        private static bool IsNullOrEmpty(DataRow tableRow, string coulmnName)
        {
            bool IsNull = false;
            var Value = tableRow[coulmnName];
            if (Value.Equals(null) || Value.GetType().Name == "DBNull" || string.IsNullOrEmpty((string)Value) || string.IsNullOrWhiteSpace((string)Value))
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
            if(coulmnName == "Master")
            {
                IsPublic = SetIsPublicForMasterColumn(TableRow);
            }

            if(coulmnName == "ContainersNumbers")
            {
                IsPublic = CheckContainerShipmentSearchPublicity(TableRow);
            }
            TableRow.SetField("IsPublic", IsPublic);
        }

        private static bool SetIsPublicForMasterColumn(DataRow TableRow)
        {
            if (TableRow["ShipmentLevelCode"].Equals("H"))
            {
                return false;
            }

            if (TableRow["ForwardingShipmentLevelCode"].Equals("H"))
            {
                return false;
            }

            return true;
        }

        private static bool CheckContainerShipmentSearchPublicity(DataRow searchRecord)
        {
            if (IsNullOrEmpty(searchRecord, "ShipmentTypeId"))
                return false;

            string shipmentTypeId = (string)searchRecord["ShipmentTypeId"];

            return !PrivateShipmentTypes.Contains(shipmentTypeId?.ToUpper());
        }

        private static string GetReferenceTypeFromCoulmnName(string CoulmnName)
        {
            string ReferenceType = Regex.Replace(CoulmnName, "([a-z])([A-Z])", "$1 $2");
            return ReferenceType;
        }


    }

}
