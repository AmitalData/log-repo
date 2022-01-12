using DW_Editor_Tool.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseData.Service
{
    class FillDWObjectFieldsService
    {
       // Change Object Fields Stirng/List + Object Table Name + Connection String + any needed DIM and Lookup table values
        string objectFieldsList = "";
        string objectTableName = "Container";
        public static string connectionString = "Data Source=.;Initial Catalog=Logbox_Main;Integrated Security=False;Persist Security Info=True;User ID=sa;Password= Saas256;MultipleActiveResultSets=True;Connect Timeout=60";
        internal void BuildContainerObjectFields()
        {
            List<DWObjectFieldViewModel> fieldsList = new List<DWObjectFieldViewModel>();
            DWObjectTableViewModel tableViewModel = new DWObjectTableViewModel();

            BuilDWObjectFields(tableViewModel, fieldsList);
        }

        private static void BuilDWObjectFields(DWObjectTableViewModel tableViewModel, List<DWObjectFieldViewModel> fieldsList)
        {
            string connectionString = "Data Source=.;Initial Catalog=Logbox_Main;Integrated Security=False;Persist Security Info=True;User ID=sa;Password= Saas256;MultipleActiveResultSets=True;Connect Timeout=60";
            string objectTableName = "Container";
            var objectTableLists = GetObjectFields(connectionString);

            var fieldListString = "ActualEmptyPickupDate,EmptyPickupLocation,EmptyPickupLocationPortId,EstimatedEmptyPickupDate,PreCarriageLocation,PreCarriageLocationPortId,POLLocation," +
"POLLocationPortId,EstimatedPOLArrival,ActualPOLArrival,EstimatedPOLLoaded,ActualPOLLoaded,EstimatedPOLVesselDeparture,ActualPOLVesselDeparture,Transshipment1Location,Transshipment1LocationPortId,EstimatedTrans1VesselArrival," +
"ActualTransshipment1VesselArrival,EstimatedTransshipment1Discharge,ActualTransshipment1Discharge,EstimatedTransshipment1Loaded,ActualTransshipment1Loaded,EstimatedTrans1VesselDeparture," +
"ActualTrans1VesselDeparture,Transshipment2Location,Transshipment2LocationPortId,EstimatedTrans2VesselArrival,ActualTransshipment2VesselArrival,ActualTransshipment2VesselArrival,EstimatedTransshipment2Discharge," +
"ActualTransshipment2Discharge,EstimatedTransshipment2Loaded,ActualTransshipment2Loaded,EstimatedTrans2VesselDeparture,ActualTrans2VesselDeparture,Transshipment3Location,Transshipment3LocationPortId,EstimatedTrans3VesselArrival," +
"ActualTransshipment3VesselArrival,EstimatedTransshipment3Discharge,ActualTransshipment3Discharge,EstimatedTransshipment3Loaded,ActualTransshipment3Loaded,EstimatedTrans3VesselDeparture,ActualTrans3VesselDeparture," +
"Transshipment4Location,Transshipment4LocationPortId,EstimatedTrans4VesselArrival,ActualTransshipment4VesselArrival,EstimatedTransshipment4Discharge,ActualTransshipment4Discharge,EstimatedTransshipment4Loaded," +
"ActualTransshipment4Loaded,EstimatedTrans4VesselDeparture,ActualTrans4VesselDeparture,PODLocation,PODLocationPortId,EstimatedPODVesselArrival,EstimatedPODDischarge,ActualPODVesselArrival," +
"ActualPODDischarge,EstimatedPODDeparture,ActualPODDeparture,AvailabilityLocation,EmptyReturnLocationPortId,EmptyReturnLocation,EstimatedEmptyReturn,ActualEmptyReturn";

            string[] fieldss = fieldListString.Split(',');

            foreach (string field in fieldss)
            {
                var item = (from rowfield in objectTableLists.AsEnumerable()
                            where rowfield.Field<string>("FieldName") == field
                            select rowfield).FirstOrDefault();

                if (item != null)
                {
                    DWObjectFieldViewModel fieldViewModel = new DWObjectFieldViewModel(tableViewModel, false);
                    fieldViewModel.Id = item["Id"].ToString();
                    fieldViewModel.Name = item["DisplayName"].ToString();
                    fieldViewModel.Code = "[" + item["DisplayName"].ToString() + "]";
                    fieldViewModel.DataTypeCode = GetDataType(item["DataTypeCode"].ToString());
                    fieldViewModel.DimensionTableCode = GeDimensionTableCode(item["LookUpName"].ToString(), item["DataTypeCode"].ToString());
                    fieldViewModel.IsRequired = (bool)item["IsRequiered"];
                    fieldViewModel.MinLength = (int)item["MinLength"];
                    fieldViewModel.MaxLength = (int)item["MaxLength"];
                    fieldViewModel.OriginalObjectFieldCode = objectTableName + "." + item["FieldName"].ToString();
                    fieldsList.Add(fieldViewModel);
                }
                else
                { // print to check
                    Console.WriteLine(field + " Not Found!");
                }
            }

        }

        private static string GeDimensionTableCode(string tableName, string dataType)
        {

            if (dataType == "DateTime") return "DIM_Dates";

            if (string.IsNullOrEmpty(tableName)) return null;
            string DimensionTableCode;
            switch (tableName)
            {
                case "Port":
                    DimensionTableCode = "DIM_Ports";
                    break;
                case "Card":
                    DimensionTableCode = "DIM_Partners";
                    break;
                case "User":
                    DimensionTableCode = "DIM_Users";
                    break;
                default:
                    DimensionTableCode = null;
                    break;
            }
            return DimensionTableCode;
        }

        private static string GetDataType(string dataType)
        {
            string type;
            switch (dataType)
            {
                case "LookUp":
                    type = "Dimension";
                    break;
                case "DateTime":
                    type = "Dimension";
                    break;
                default:
                    type = dataType;
                    break;
            }
            return type;
        }

        private static DataTable GetObjectFields(string connectionString)
        {
            string objectTableName = "Container";

            DataTable objectFieldsTable = new DataTable();
            using (SqlConnection sourceConnection = new SqlConnection(connectionString))
            {
                sourceConnection.Open();
                SqlCommand commandSourceData = new SqlCommand("select objectTables.Id as Id,ObjectFields.FieldName as FieldName, objectTables.Name as LookUpName,TextCodes.DefaultText as DisplayName, DataTypeCode , MaxLength, MinLength, IsRequiered  from ObjectFields " +
                    "left JOIN objectTables on ObjectFields.LookupTableId = objectTables.id" +
                    " left JOIN TextCodes on ObjectFields.FullNameTextCodeId = TextCodes.id " +
                    "where ObjectFields.ObjectTableId = (select id from ObjectTables where name = " +
                    "'" + objectTableName + "')" + " and ObjectFields.FieldName  in" +
                    " ('ActualEmptyPickupDate', 'EmptyPickupLocation', 'EmptyPickupLocationPortId', 'EstimatedEmptyPickupDate', 'PreCarriageLocation', 'PreCarriageLocationPortId', 'POLLocation', 'POLLocationPortId', 'EstimatedPOLArrival', 'ActualPOLArrival', 'EstimatedPOLLoaded', 'ActualPOLLoaded', 'EstimatedPOLVesselDeparture', 'ActualPOLVesselDeparture', 'Transshipment1Location', 'Transshipment1LocationPortId', 'EstimatedTrans1VesselArrival', 'ActualTransshipment1VesselArrival', 'EstimatedTransshipment1Discharge', 'ActualTransshipment1Discharge', 'EstimatedTransshipment1Loaded', 'ActualTransshipment1Loaded', 'EstimatedTrans1VesselDeparture', 'ActualTrans1VesselDeparture', 'Transshipment2Location', 'Transshipment2LocationPortId', 'EstimatedTrans2VesselArrival', 'ActualTransshipment2VesselArrival', 'ActualTransshipment2VesselArrival', 'EstimatedTransshipment2Discharge', 'ActualTransshipment2Discharge', 'EstimatedTransshipment2Loaded', 'ActualTransshipment2Loaded', 'EstimatedTrans2VesselDeparture', 'ActualTrans2VesselDeparture', 'Transshipment3Location', 'Transshipment3LocationPortId', 'EstimatedTrans3VesselArrival', 'ActualTransshipment3VesselArrival', 'EstimatedTransshipment3Discharge', 'ActualTransshipment3Discharge', 'EstimatedTransshipment3Loaded', 'ActualTransshipment3Loaded', 'EstimatedTrans3VesselDeparture', 'ActualTrans3VesselDeparture', 'Transshipment4Location', 'Transshipment4LocationPortId', 'EstimatedTrans4VesselArrival', 'ActualTransshipment4VesselArrival', 'EstimatedTransshipment4Discharge', 'ActualTransshipment4Discharge', 'EstimatedTransshipment4Loaded', 'ActualTransshipment4Loaded', 'EstimatedTrans4VesselDeparture', 'ActualTrans4VesselDeparture', 'PODLocation', 'PODLocationPortId', 'EstimatedPODVesselArrival', 'EstimatedPODDischarge', 'ActualPODVesselArrival', 'ActualPODDischarge', 'EstimatedPODDeparture', 'ActualPODDeparture', 'AvailabilityLocation', 'EmptyReturnLocationPortId', 'EmptyReturnLocation', 'EstimatedEmptyReturn', 'ActualEmptyReturn')", sourceConnection);

                SqlDataReader reader = commandSourceData.ExecuteReader();
                Console.WriteLine("added succsigully");
                objectFieldsTable.Load(reader);
                reader.Close();
            }

            return objectFieldsTable;
        }
    }
}
