
using Logitude.CargoTracking.BL.CargoTrackingServices.HelperClasses;
using Logitude.CargoTracking.BL.CargoTrackingServices.Services.CustomMapping;
using Logitude.CargoTracking.BL.CargoTrackingServices.Services.SearchService;
using Logitude.CargoTracking.BL.CargoTrackingServices.Services.ServicesHelper;
using Logitude.CargoTracking.BL.CargoTrackingServices.Services.TableLogic;
using Logitude.CargoTracking.BL.CargoTrackingServices.Services.TableStructure;
using Logitude.CargoTracking.BL.CargoTrackingServices.Services.TableStructure.Helper;
using Logitude.CargoTracking.BL.CargoTrackingServices.Services.ValidateRecords;
using Logitude.CargoTracking.Data;
using Logitude.CargoTracking.Data.EntityListQueryServices;
using Logitude.CargoTracking.Data.EntityLists;
using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
 

namespace Logitude.CargoTracking.BL.CargoTrackingServices.Services
{
    public class CargoTrackingMainService
    {
     
        public string LastUpdate;
        public RecordUpdated RecordUpdated = new RecordUpdated();
        private List<string> KeysForRecoredsNotValidated;
        const int CargoTrackingTable_SingleCondition = 1;
        const int CargoTrackingTable_MultiConditions = 2;
        const int ShipmentTable_GetAllCustomsShipmentsThatContainForwardingShipments = 1;
        const int ShipmentTable_GetAllNonCustomShipmentsThatContainForwardingShipments = 2;
        const int GeneralTable_WithoutCustomCondition = 0;


        public RecordUpdated UpdateCargoTrackingDataBase(CargoTrackingUpdateDataBaseArgs cargoTrackingDataBaseArgs)
        {

            SetTablesStructureHelper(cargoTrackingDataBaseArgs);
            SetBuildProcessData(cargoTrackingDataBaseArgs);
            RecordUpdated = new RecordUpdated();
            RecordUpdated.IsFromBuild = ServiceHelper.GetIsIncrementalRunning(cargoTrackingDataBaseArgs.BuildCargoArgs.SourceConnectionString);
            RecordUpdated = UpdateCargoTracking(cargoTrackingDataBaseArgs, RecordUpdated);
            return RecordUpdated;
        }

        private void UpdateCargoTracking(BulkDataPreperation bulkDataPreperation, 
                        CargoTrackingUpdateDataBaseArgs cargoTrackingDataBaseArgs,
                        List<DataColumn> dataColumnListCols)
        {
            if (bulkDataPreperation.SqlDataReader.HasRows)
            {
                MapDataTableColumn(bulkDataPreperation, cargoTrackingDataBaseArgs.DataTableSchema, dataColumnListCols);
                UpdateCargoTrackingBulk(bulkDataPreperation, cargoTrackingDataBaseArgs, dataColumnListCols);
                UpdateLastRemainMainRecordsIfExist(bulkDataPreperation, cargoTrackingDataBaseArgs);
                UpdateLastRemainInnerRecordsIfExist(bulkDataPreperation, cargoTrackingDataBaseArgs);
                if (KeysForRecoredsNotValidated.Count > 0)
                {
                    RemoveNotValidateLines(cargoTrackingDataBaseArgs);
                }
                bulkDataPreperation.SqlDataReader.Close();
            }
            else
            {

                bulkDataPreperation.SqlDataReader.Close();
            }
        }
 
        private void RemoveNotValidateLines(CargoTrackingUpdateDataBaseArgs cargoTrackingDataBaseArgs)
        {
            if (cargoTrackingDataBaseArgs.CargoTrackingArguments == null)
            {
                StringBuilder deletedRows = new StringBuilder();
                foreach (string id in KeysForRecoredsNotValidated)
                {
                    deletedRows.Append("'" + id + "'" + ",");
                }
                ExecuteDeleteNotValidateCommand(cargoTrackingDataBaseArgs, deletedRows);
            }
        }


        private void ExecuteDeleteNotValidateCommand(CargoTrackingUpdateDataBaseArgs cargoTrackingDataBaseArgs, StringBuilder deletedRows)
        {
            string DeleteNotValidRecordsCommand = "";
            DeleteNotValidRecordsCommand = "delete from " + cargoTrackingDataBaseArgs.BuildCargoArgs.Table.CargoTracking_TableName + " where " + cargoTrackingDataBaseArgs.BuildCargoArgs.Table.ConditionKey + " in " + ("(" + deletedRows.ToString() + ")").Replace(",)", ")");
            ServiceHelper.ExecuteSql(DeleteNotValidRecordsCommand, cargoTrackingDataBaseArgs.BuildCargoArgs.DestinationConnectionString);
            if (cargoTrackingDataBaseArgs.BuildCargoArgs.Table.CargoTracking_InnerTableName != null)
            {
                DeleteNotValidRecordsCommand = "delete from " + cargoTrackingDataBaseArgs.BuildCargoArgs.Table.CargoTracking_InnerTableName + " where " + cargoTrackingDataBaseArgs.BuildCargoArgs.Table.InnerConditionKey + " in " + ("(" + deletedRows.ToString() + ")").Replace(",)", ")");
                ServiceHelper.ExecuteSql(DeleteNotValidRecordsCommand, cargoTrackingDataBaseArgs.BuildCargoArgs.DestinationConnectionString);
            }
            KeysForRecoredsNotValidated = new List<string>();

        }


        private void MapDataTableColumn(BulkDataPreperation bulkDataPreperation,
                 DataTable dataTableSchema,
                 List<DataColumn> dataColumnListCols)
        {
            if (dataTableSchema != null)
            {
                foreach (DataRow drow in dataTableSchema.Rows)
                {
                    DataColumn column = GetCoulmnFromDataRow(drow);
                    dataColumnListCols.Add(column);
                    bulkDataPreperation.MainDataTable.Columns.Add(column);
                }

                AddDummyCoulmnsToDatatTable(bulkDataPreperation);
            }
        }

        private void UpdateCargoTrackingBulk(BulkDataPreperation bulkDataPreperation,
                        CargoTrackingUpdateDataBaseArgs cargoTrackingDataBaseArgs,
                        List<DataColumn> dataColumnListCols)
        {
            while (bulkDataPreperation.SqlDataReader.Read())
            {
                bulkDataPreperation.MainDataTable = FillDataTableValues(dataColumnListCols, bulkDataPreperation, cargoTrackingDataBaseArgs.BuildCargoArgs.Table.Main_CargoTracking_TableName);

                bulkDataPreperation.NumberRecoredTake++;

                if (bulkDataPreperation.NumberRecoredTake == bulkDataPreperation.MaxRecoredTakeEachTime)
                {
                    UpdateCargoTrackingMainBulk(bulkDataPreperation, cargoTrackingDataBaseArgs);
                }
                if (bulkDataPreperation.InnerDataTable != null && bulkDataPreperation.InnerDataTable.Rows.Count >= bulkDataPreperation.MaxRecoredTakeEachTime)
                {
                    UpdateCargoTrackingInnerBulk(bulkDataPreperation, cargoTrackingDataBaseArgs);
                }
                if (KeysForRecoredsNotValidated.Count > 0)
                {
                    RemoveNotValidateLines(cargoTrackingDataBaseArgs);

                }

            }
        }

        private void UpdateLastRemainMainRecordsIfExist(BulkDataPreperation bulkDataPreperation,
                        CargoTrackingUpdateDataBaseArgs cargoTrackingDataBaseArgs)
        {
            if (bulkDataPreperation.MainDataTable.Rows.Count > 0)
            {
                var Lastcolumns = bulkDataPreperation.MainDataTable.Rows
                                                     .Cast<DataRow>()
                                                     .Select(r => (string)r[bulkDataPreperation.CargoTrackingTable.KeyName].ToString())
                                                     .ToList();
                bulkDataPreperation.CargoTrackingUpdateDataBaseArgs = cargoTrackingDataBaseArgs;
                bulkDataPreperation.SelectedDataTable = bulkDataPreperation.MainDataTable;
                bulkDataPreperation = UpdateBulkPreperations(bulkDataPreperation, Lastcolumns);
            }
        }

        private void UpdateLastRemainInnerRecordsIfExist(BulkDataPreperation bulkDataPreperation,
                      CargoTrackingUpdateDataBaseArgs cargoTrackingDataBaseArgs)
        {
            if (bulkDataPreperation.InnerDataTable != null && bulkDataPreperation.InnerDataTable.Rows.Count > 0)
            {
                var Lastcolumns = bulkDataPreperation.InnerDataTable.Rows
                                                     .Cast<DataRow>()
                                                     .Select(r => (string)r[bulkDataPreperation.CargoTrackingTable.InnerKeyName].ToString())
                                                     .ToList();
                bulkDataPreperation.CargoTrackingUpdateDataBaseArgs = cargoTrackingDataBaseArgs;
                bulkDataPreperation.SelectedDataTable = bulkDataPreperation.InnerDataTable;
                bulkDataPreperation = UpdateBulkPreperations(bulkDataPreperation, Lastcolumns, true);
            }
        }

        private void AfterFinishUpdateCargoTracking(BulkDataPreperation bulkDataPreperation,
                        CargoTrackingUpdateDataBaseArgs cargoTrackingDataBaseArgs,
                        bool isUpadteWaterMark)
        {
            if (cargoTrackingDataBaseArgs.CargoTrackingArguments != null)
            {
                BuildAllIndexesWithConstraient(cargoTrackingDataBaseArgs.BuildCargoArgs);
                Rename_Pre_Tables(cargoTrackingDataBaseArgs.BuildCargoArgs);
            }
            if (isUpadteWaterMark)
            {
                ServiceHelper.UpdateWaterMarkAfterFinishCheck(bulkDataPreperation.CargoTrackingTable, bulkDataPreperation.AutomaticLastUpdateDate, cargoTrackingDataBaseArgs.BuildCargoArgs);

            }
        }
        private RecordUpdated UpdateCargoTrackingDatabase(CargoTrackingUpdateDataBaseArgs cargoTrackingDataBaseArgs, 
                                                         bool isUpadteWaterMark = false)
        {
            RecordUpdated _RecordUpdated = new RecordUpdated();
            KeysForRecoredsNotValidated = new List<string>();
            BulkDataPreperation bulkDataPreperation = InitializeBulkDataPreperation(cargoTrackingDataBaseArgs);
            using (SqlConnection sourceConnection =
                                  new SqlConnection(cargoTrackingDataBaseArgs.BuildCargoArgs.SourceConnectionString))
            {
                sourceConnection.Open();
                bulkDataPreperation.SqlDataReader = GetSqlDataReader(cargoTrackingDataBaseArgs, bulkDataPreperation.CargoTrackingTable, sourceConnection);
                DataTable DataTableSchema = bulkDataPreperation.SqlDataReader.GetSchemaTable();
                cargoTrackingDataBaseArgs.DataTableSchema = DataTableSchema;
                List<DataColumn> dataColumnListCols = new List<DataColumn>();
                bulkDataPreperation.MainDataTable = new DataTable();
                UpdateCargoTracking(bulkDataPreperation, cargoTrackingDataBaseArgs,dataColumnListCols);
                AfterFinishUpdateCargoTracking(bulkDataPreperation, cargoTrackingDataBaseArgs, isUpadteWaterMark);
                sourceConnection.Close();
            }
            _RecordUpdated.NumberOfRecordUpdated = bulkDataPreperation.NumberOfMainCoulmnsUpdated;
            _RecordUpdated.NumberOfRecordUpdated2 = bulkDataPreperation.NumberOfInnerCoulmnsUpdated;
            return _RecordUpdated;
        }

        private void AddDummyCoulmnsToDatatTable(BulkDataPreperation bulkDataPreperation)
        {
            if (!string.IsNullOrEmpty(bulkDataPreperation.CargoTrackingTable.FieldsDummyName))
            {
                foreach (string ColumnName in bulkDataPreperation.CargoTrackingTable.FieldsDummyName.Split(','))
                {
                    bulkDataPreperation.MainDataTable.Columns.Add(ColumnName);
                }
            }
       
        }

        private void BuildAllIndexesWithConstraient(CargoTrackingArgs buildCargoArgs)
        {
            if (buildCargoArgs.Table.CargoTracking_TableName == "Pre_CargoTrackingShipments" && buildCargoArgs.Table.CurrentCondition == 2)
            {
                string mainTableStructureRelationsCommand = buildCargoArgs.MainTableStructureHelper.GetTableStructureRelations(buildCargoArgs.Table.CargoTracking_TableName);
                ServiceHelper.ExecuteSql(mainTableStructureRelationsCommand, buildCargoArgs.DestinationConnectionString);
                string mainTableStructureConstraintsCommand = buildCargoArgs.MainTableStructureHelper.GetTableStructureUniqueConstraints(buildCargoArgs.Table.CargoTracking_TableName);
                ServiceHelper.ExecuteSql(mainTableStructureConstraintsCommand, buildCargoArgs.DestinationConnectionString);
                string mainTableStructureIndexsCommand = buildCargoArgs.MainTableStructureHelper.GetTableStructureReBuildIndexs(buildCargoArgs.Table.CargoTracking_TableName);
                ServiceHelper.ExecuteSql(mainTableStructureIndexsCommand, buildCargoArgs.DestinationConnectionString);
            }
            if (buildCargoArgs.Table.CargoTracking_InnerTableName == "Pre_CargoTrackingShipmentSearches" && buildCargoArgs.Table.CurrentCondition == 2)
            {
                string innerTableStructureIndexsCommand = buildCargoArgs.InnerTableStructureHelper.GetTableStructureReBuildIndexs(buildCargoArgs.Table.CargoTracking_InnerTableName);
                ServiceHelper.ExecuteSql(innerTableStructureIndexsCommand, buildCargoArgs.DestinationConnectionString);
            }

        }

        private void UpdateCargoTrackingInnerBulk(BulkDataPreperation bulkDataPreperation, 
                                                  CargoTrackingUpdateDataBaseArgs cargoTrackingDataBaseArgs)
        {
            var columns = bulkDataPreperation.InnerDataTable.Rows
                     .Cast<DataRow>()
                     .Select(r => (string)r[bulkDataPreperation.CargoTrackingTable.InnerKeyName].ToString())
                     .ToList();

            DataTable ThreadDataTable = bulkDataPreperation.InnerDataTable.Clone();
            foreach (DataRow drtableOld in bulkDataPreperation.InnerDataTable.Rows)
            {
                ThreadDataTable.ImportRow(drtableOld);

            }
            bulkDataPreperation.InnerDataTable.Rows.Clear();
            bulkDataPreperation.CargoTrackingUpdateDataBaseArgs = cargoTrackingDataBaseArgs;
            bulkDataPreperation.SelectedDataTable = ThreadDataTable;
            UpdateBulkPreperations(bulkDataPreperation, columns, true);

        }



        private void UpdateCargoTrackingMainBulk(BulkDataPreperation bulkDataPreperation, 
                                                 CargoTrackingUpdateDataBaseArgs cargoTrackingDataBaseArgs)
        {
            var columns = bulkDataPreperation.MainDataTable.Rows
                     .Cast<DataRow>()
                     .Select(r => (string)r[bulkDataPreperation.CargoTrackingTable.KeyName].ToString())
                     .ToList();
            var ThreadStardBulding = bulkDataPreperation.NumberRecoredTake;
            DataTable ThreadDataTable = bulkDataPreperation.MainDataTable.Clone();
            foreach (DataRow drtableOld in bulkDataPreperation.MainDataTable.Rows)
            {
                ThreadDataTable.ImportRow(drtableOld);

            }
            bulkDataPreperation.MainDataTable.Rows.Clear();
            bulkDataPreperation.NumberRecoredTake = 0;
            bulkDataPreperation.CargoTrackingUpdateDataBaseArgs = cargoTrackingDataBaseArgs;
            bulkDataPreperation.SelectedDataTable = ThreadDataTable;
            UpdateBulkPreperations(bulkDataPreperation,columns);

        }

        private BulkDataPreperation InitializeBulkDataPreperation(CargoTrackingUpdateDataBaseArgs cargoTrackingDataBaseArgs)
        {
            DateTime? AutomaticDate = null;
            if (cargoTrackingDataBaseArgs.CargoTrackingArguments != null)
            {
                AutomaticDate = DateTime.Now;
            }
           
            BulkDataPreperation bulkDataPreperation = new BulkDataPreperation()
            {
                CargoTrackingTable = cargoTrackingDataBaseArgs.BuildCargoArgs.Table,
                MaxRecoredTakeEachTime = cargoTrackingDataBaseArgs.NumberOfBulkPerTime,
                MainDataTable = null,
                AutomaticLastUpdateDate = AutomaticDate,
                SqlDataReader = null,
                NumberOfMainCoulmnsUpdated = 0,
                NumberOfInnerCoulmnsUpdated =0,
                NumberRecoredTake = 0,
            };

            return bulkDataPreperation;

        }

        private BulkDataPreperation UpdateBulkPreperations(BulkDataPreperation bulkDataPreperation, 
                                                           List<string> columns ,bool 
                                                           isInnerCargoTracking=false)
        {
            if (!isInnerCargoTracking)
               bulkDataPreperation.NumberOfMainCoulmnsUpdated += columns.Count;
            else
               bulkDataPreperation.NumberOfInnerCoulmnsUpdated += columns.Count;

            bulkDataPreperation.CargoTrackingTable = PrepareTableParameters(bulkDataPreperation, columns, isInnerCargoTracking);
            bulkDataPreperation.AutomaticLastUpdateDate = UpdateBulkValues(bulkDataPreperation, isInnerCargoTracking);

            return bulkDataPreperation;
        }
        public void Rename_Pre_Tables(CargoTrackingArgs buildCargoArgs)
        {
            if (buildCargoArgs.Table.ConditionsNumber > 1)
            {
                if (buildCargoArgs.Table.CurrentCondition == 2)
                    StartRenameCargoTables(buildCargoArgs);
            }
            else
                StartRenameCargoTables(buildCargoArgs);

        }

        private void StartRenameCargoTables(CargoTrackingArgs buildCargoArgs)
        {
            string minTableChangeNameScript = buildCargoArgs.MainTableStructureHelper.GetTableStructureChangeNameScript(buildCargoArgs.Table.Main_CargoTracking_TableName, buildCargoArgs.Table.Main_CargoTracking_TableName + "_SW") + "\n";
            minTableChangeNameScript += buildCargoArgs.MainTableStructureHelper.GetTableStructureChangeNameScript(buildCargoArgs.Table.Pre_TableName, buildCargoArgs.Table.Main_CargoTracking_TableName) + "\n";
            minTableChangeNameScript += buildCargoArgs.MainTableStructureHelper.GetTableStructureChangeNameScript(buildCargoArgs.Table.Main_CargoTracking_TableName + "_SW", buildCargoArgs.Table.Pre_TableName) + "\n";
            ServiceHelper.ExecuteSql(minTableChangeNameScript, buildCargoArgs.DestinationConnectionString);
            if (buildCargoArgs.Table.Main_CargoTracking_InnerTableName != null)
            {
                string innerTableChangeNameScript  = buildCargoArgs.InnerTableStructureHelper.GetTableStructureChangeNameScript(buildCargoArgs.Table.Main_CargoTracking_InnerTableName, buildCargoArgs.Table.Main_CargoTracking_InnerTableName + "_SW") + "\n";
                innerTableChangeNameScript += buildCargoArgs.InnerTableStructureHelper.GetTableStructureChangeNameScript(buildCargoArgs.Table.Pre_InnerTableName, buildCargoArgs.Table.Main_CargoTracking_InnerTableName) + "\n";
                innerTableChangeNameScript += buildCargoArgs.InnerTableStructureHelper.GetTableStructureChangeNameScript(buildCargoArgs.Table.Main_CargoTracking_InnerTableName + "_SW", buildCargoArgs.Table.Pre_InnerTableName) + "\n";
                ServiceHelper.ExecuteSql(innerTableChangeNameScript, buildCargoArgs.DestinationConnectionString);
            }
        }

        private SqlDataReader GetSqlDataReader(CargoTrackingUpdateDataBaseArgs cargoTrackingDataBaseArgs, 
                                               CargoTrackingTable table, 
                                               SqlConnection sourceConnection)
        {
            string TableConditionCommand = "";
            string condition;
            if (cargoTrackingDataBaseArgs.BuildCargoArgs.Table.Main_CargoTracking_TableName == "CargoTrackingShipments" && cargoTrackingDataBaseArgs.BuildCargoArgs.Table.CurrentCondition == 1)
                TableConditionCommand = ShipmentTableCondtions.GetAllCustomsShipmentsThatContainForwardingShipments(cargoTrackingDataBaseArgs, table, LastUpdate);
            else if (cargoTrackingDataBaseArgs.BuildCargoArgs.Table.Main_CargoTracking_TableName == "CargoTrackingShipments" && cargoTrackingDataBaseArgs.BuildCargoArgs.Table.CurrentCondition == 2)
                TableConditionCommand = ShipmentTableCondtions.GetAllNonCustomShipmentsThatContainForwardingShipments(cargoTrackingDataBaseArgs, table, LastUpdate);
            else
            {
                string fieldName = !string.IsNullOrEmpty(cargoTrackingDataBaseArgs.BuildCargoArgs.Table.FieldsDBName) ? cargoTrackingDataBaseArgs.BuildCargoArgs.Table.FieldsDBName : "*";
                condition = GetUpdateDataBaseCondition(cargoTrackingDataBaseArgs.BuildCargoArgs, cargoTrackingDataBaseArgs.CargoTrackingArguments);
                TableConditionCommand = "SELECT " + fieldName + " " +
                        "FROM dbo." + table.DBTableName + condition;
            }

            SqlCommand commandSourceData = new SqlCommand(TableConditionCommand, sourceConnection);
            commandSourceData.CommandTimeout = (int)ServiceHelper.TimeOut;
            SqlDataReader reader = commandSourceData.ExecuteReader(CommandBehavior.CloseConnection);

            return reader;

        }

        private DataColumn GetCoulmnFromDataRow(DataRow drow)
        {
            string columnName = System.Convert.ToString(drow["ColumnName"]);
            DataColumn column = new DataColumn(columnName, (Type)(drow["DataType"]));
            column.Unique = (bool)drow["IsUnique"];
            column.AllowDBNull = (bool)drow["AllowDBNull"];
            column.AutoIncrement = (bool)drow["IsAutoIncrement"];

            return column;
        }

        private DataTable FillDataTableValues(List<DataColumn> listCols, BulkDataPreperation bulkDataPreperation, string tableName)
        {
            bool isRoWValid = CargoTrackingValidateRecordsService.ValidateRecords(tableName, bulkDataPreperation.SqlDataReader);
            if (isRoWValid)
            {
                DataRow dataRow = bulkDataPreperation.MainDataTable.NewRow();
                for (int i = 0; i < listCols.Count; i++)
                {
                    dataRow[((DataColumn)listCols[i])] = bulkDataPreperation.SqlDataReader[i];
                }
                CargoTrackingSearchService.SearchService(dataRow, bulkDataPreperation, tableName);
                bulkDataPreperation.MainDataTable.Rows.Add(dataRow);
            }
            else
            {
                DataRow dataRow = bulkDataPreperation.MainDataTable.NewRow();
                for (int i = 0; i < listCols.Count; i++)
                {
                    dataRow[((DataColumn)listCols[i])] = bulkDataPreperation.SqlDataReader[i];
                }
                KeysForRecoredsNotValidated.Add((string)dataRow[bulkDataPreperation.CargoTrackingTable.KeyName]);
            }
            return bulkDataPreperation.MainDataTable;
        }

        private CargoTrackingTable PrepareTableParameters(BulkDataPreperation bulkDataPreperation, 
                                                          List<string> columns,
                                                          bool isFromInnerCargoTrackingTable=false)
        {
            bulkDataPreperation.CargoTrackingTable.UpdatedCount = columns != null ? columns.Count() : 0;

            if (isFromInnerCargoTrackingTable)
            {
                CargoDeleteRowsArgs DeleteRowsArgs = new CargoDeleteRowsArgs() { TableName = bulkDataPreperation.CargoTrackingTable.CargoTracking_InnerTableName, KeyName = bulkDataPreperation.CargoTrackingTable.InnerConditionKey, IdsList = columns, ConnectionString = bulkDataPreperation.CargoTrackingUpdateDataBaseArgs.BuildCargoArgs.DestinationConnectionString, ReturnDeleteIdsAsString = true };
                bulkDataPreperation.CargoTrackingTable.InnerRefreshIds = columns.ToString();
                if (bulkDataPreperation.CargoTrackingUpdateDataBaseArgs.CargoTrackingArguments == null)
                    bulkDataPreperation.CargoTrackingTable.InnerRefreshIds = DeleteRowsFromCargoTables(DeleteRowsArgs);
            }
            else
            {
                CargoDeleteRowsArgs DeleteRowsArgs = new CargoDeleteRowsArgs() { TableName = bulkDataPreperation.CargoTrackingTable.CargoTracking_TableName, KeyName = bulkDataPreperation.CargoTrackingTable.ConditionKey, IdsList = columns, ConnectionString = bulkDataPreperation.CargoTrackingUpdateDataBaseArgs.BuildCargoArgs.DestinationConnectionString, ReturnDeleteIdsAsString = true };
                bulkDataPreperation.CargoTrackingTable.RefreshIds = columns.ToString();
                if (bulkDataPreperation.CargoTrackingUpdateDataBaseArgs.CargoTrackingArguments == null)
                    bulkDataPreperation.CargoTrackingTable.RefreshIds = DeleteRowsFromCargoTables(DeleteRowsArgs);
            }
            return bulkDataPreperation.CargoTrackingTable;
        }

      

        private DateTime? UpdateBulkValues(BulkDataPreperation bulkDataPreperation, bool isFromInnerCargoTrackingTable=false)
        {
            if (!string.IsNullOrEmpty(bulkDataPreperation.CargoTrackingTable.RefreshIds) || 
                !string.IsNullOrEmpty(bulkDataPreperation.CargoTrackingTable.InnerRefreshIds))
            {
                using (SqlConnection destinationConnection =
                                    new SqlConnection(bulkDataPreperation.CargoTrackingUpdateDataBaseArgs.BuildCargoArgs.DestinationConnectionString))
                {
                 

                    UpdateCargoTrackingBulk(destinationConnection,bulkDataPreperation, isFromInnerCargoTrackingTable);

                }

            }
            return bulkDataPreperation.AutomaticLastUpdateDate;
        }

        private void UpdateCargoTrackingBulk(SqlConnection destinationConnection,
                        BulkDataPreperation bulkDataPreperation,
                        bool isFromInnerCargoTrackingTable = false)
        {
            destinationConnection.Open();
            using (SqlBulkCopy bulkCopy =
                                 new SqlBulkCopy(bulkDataPreperation.CargoTrackingUpdateDataBaseArgs.BuildCargoArgs.DestinationConnectionString, SqlBulkCopyOptions.KeepIdentity))
            {
                try
                {
                    MapCargoTrackingDate(bulkCopy, bulkDataPreperation, isFromInnerCargoTrackingTable);
                    SqlBulkCopyWriteData(bulkCopy, bulkDataPreperation, isFromInnerCargoTrackingTable);
                }
                catch(Exception exception)
                {
                    SetErrorLog(exception, bulkDataPreperation);
                }

                finally
                {
                    SetAutomaticLastUpdateDateAfterWriteData(bulkDataPreperation);
                    destinationConnection.Close();
                }
            }

        }


        private void SetErrorLog(Exception exception, BulkDataPreperation bulkDataPreperation)
        {
            string ErrorsLog = "Table Name: " + bulkDataPreperation.CargoTrackingTable.CargoTracking_TableName +
                                       Environment.NewLine + "Erros: " + exception.Message +
                                       Environment.NewLine + "Stack Trace: " + exception.StackTrace +
                                       Environment.NewLine;
       
            if (!RecordUpdated.ErrorLogs.Contains(ErrorsLog))
                RecordUpdated.ErrorLogs += ErrorsLog;
        }

        private void SetAutomaticLastUpdateDateAfterWriteData(BulkDataPreperation bulkDataPreperation)
        {
            if (bulkDataPreperation.CargoTrackingUpdateDataBaseArgs.CargoTrackingArguments == null)
            {
                bulkDataPreperation.AutomaticLastUpdateDate = ServiceHelper.GetAutomaticLastUpdateDate(bulkDataPreperation.AutomaticLastUpdateDate, bulkDataPreperation.SelectedDataTable, bulkDataPreperation.CargoTrackingTable.IsClosedTable);
            }
            else
            {
                bulkDataPreperation.AutomaticLastUpdateDate = null;
            }

        }


        private void SqlBulkCopyWriteData(SqlBulkCopy bulkCopy,
                        BulkDataPreperation bulkDataPreperation,
                        bool isFromInnerCargoTrackingTable = false)
        {
            if (isFromInnerCargoTrackingTable)
                bulkCopy.DestinationTableName = "dbo." + bulkDataPreperation.CargoTrackingTable.CargoTracking_InnerTableName;
            else
                bulkCopy.DestinationTableName = "dbo." + bulkDataPreperation.CargoTrackingTable.CargoTracking_TableName;

            bulkCopy.BulkCopyTimeout = (int)ServiceHelper.TimeOut;
            bulkCopy.EnableStreaming = true;
            bulkCopy.BatchSize = 100000;
            bulkCopy.WriteToServer(bulkDataPreperation.SelectedDataTable);
            bulkCopy.Close();
        }
        private void MapCargoTrackingDate(SqlBulkCopy bulkCopy,
                        BulkDataPreperation bulkDataPreperation, 
                        bool isFromInnerCargoTrackingTable = false)

        {
            AutoMapColumns(bulkCopy, bulkDataPreperation, isFromInnerCargoTrackingTable);


            foreach (DataRow dr in bulkDataPreperation.SelectedDataTable.Rows)
            {
                if (!isFromInnerCargoTrackingTable)
                {
                     CargoTrackingTableLogicService.SetTableLogic(dr, bulkDataPreperation.CargoTrackingUpdateDataBaseArgs.BuildCargoArgs.Table.Main_CargoTracking_TableName, bulkDataPreperation.CargoTrackingUpdateDataBaseArgs.BuildCargoArgs.Table.CurrentCondition);
                }
                else
                {
                    CargoTrackingTableLogicService.SetTableLogic(dr, bulkDataPreperation.CargoTrackingUpdateDataBaseArgs.BuildCargoArgs.Table.Main_CargoTracking_InnerTableName, bulkDataPreperation.CargoTrackingUpdateDataBaseArgs.BuildCargoArgs.Table.CurrentCondition);
                }


            }

        }


        public void AutoMapColumns(SqlBulkCopy SqlBulkCopy, BulkDataPreperation bulkDataPreperation, bool isFromInnerCargoTracking=false)
        {
            List<string> mappingMatching = new List<string>();
            string[] dataBase_Columns = bulkDataPreperation.CargoTrackingTable.FieldsDBName.Split(',');
            string[] cargoTrackingDB_Columns = bulkDataPreperation.CargoTrackingTable.CargoTracking_FieldsDBName.Split(',');
            if (isFromInnerCargoTracking)
                cargoTrackingDB_Columns = bulkDataPreperation.CargoTrackingTable.InnerCargoTracking_FieldsDBName.Split(',');
            string CompareDB = MapMatchingColumnsName(cargoTrackingDB_Columns, dataBase_Columns, mappingMatching);
            for (int i =0; i< cargoTrackingDB_Columns.Length;i++)
            {
                var CompareList = CompareDB.Split(',').ToList();
                if (!CompareList.Contains(cargoTrackingDB_Columns[i]))
                {
                    bulkDataPreperation.CoulmnForCusstomMapping = cargoTrackingDB_Columns[i];
                    DataRowCustomMap(bulkDataPreperation, SqlBulkCopy, isFromInnerCargoTracking);
                }
            }
            AddMappingMatchingToBulkColumMap(SqlBulkCopy, mappingMatching);
        }
        private void DataRowCustomMap(BulkDataPreperation bulkDataPreperation,
                                       SqlBulkCopy SqlBulkCopy,
                                       bool isFromInnerCargoTracking)
        {
            if (isFromInnerCargoTracking)
            {
                CargoTrackingCustomMappingService.MapCargoTrackingToDataBase(bulkDataPreperation, SqlBulkCopy, bulkDataPreperation.CargoTrackingTable.Main_CargoTracking_InnerTableName);
            }
            else
            {
                CargoTrackingCustomMappingService.MapCargoTrackingToDataBase(bulkDataPreperation, SqlBulkCopy, bulkDataPreperation.CargoTrackingTable.Main_CargoTracking_TableName);
            }

        }

        private void AddMappingMatchingToBulkColumMap(SqlBulkCopy SqlBulkCopy, List<string> MappingMatching)
        {
            foreach (string columns in MappingMatching)
            {
                string[] Cols = columns.Split(',');
                string col1 = Cols[0];
                string col2 = Cols[1];

                SqlBulkCopy.ColumnMappings.Add(col1, col2);
            }
        }
        private string MapMatchingColumnsName(string[] CargoTrackingDB_Columnss , 
                                              string[] DataBase_Columnss, 
                                              List<string> MappingMatching)
        {
          StringBuilder CompareDB = new StringBuilder();
            foreach (string CTDB_columns in CargoTrackingDB_Columnss)
            {
                foreach (string DB_columns in DataBase_Columnss)
                {
                    if (CTDB_columns.Equals(DB_columns))
                    {
                        MappingMatching.Add(DB_columns + "," + CTDB_columns);
                        CompareDB.Append(CTDB_columns + ",");
                    }
                }

            }

            return CompareDB.ToString();
        }

        private string GetUpdateDataBaseCondition(CargoTrackingArgs buildCargoArgs, CargoTrackingArguments cargoTrackingArguments = null)
        {
            if (cargoTrackingArguments == null)
            {
                LastUpdate = ServiceHelper.GetTableLastUpdate(buildCargoArgs.Table.CargoTracking_TableName, buildCargoArgs.DestinationConnectionString);
            }
            BuildWhereConditionArgs buildWhereConditionArgs = new BuildWhereConditionArgs()
            {
                TableName = buildCargoArgs.Table.Main_CargoTracking_TableName,
                LastUpdate = LastUpdate,
                CargoTrackingArguments = cargoTrackingArguments,
            };
            string condition = CargoTrackingTableBuildWhereCondition.BuildWhereCondition(buildWhereConditionArgs, buildCargoArgs.Table.IsClosedTable);
            return condition;
        }


        public string DeleteRowsFromCargoTables(CargoDeleteRowsArgs deleteRowsArgs)
        {
            int rowsCount = 0;
            StringBuilder allDeletedRows = new StringBuilder();
            StringBuilder deletedRows = new StringBuilder();
            foreach (string id in deleteRowsArgs.IdsList)
            {
                rowsCount += 1;
                deletedRows.Append("'" + id + "'" + ",");
                if (rowsCount == 1000 || (deleteRowsArgs.IdsList.IndexOf(id) == deleteRowsArgs.IdsList.IndexOf(deleteRowsArgs.IdsList.Last())))
                {
                    if (deleteRowsArgs.ReturnDeleteIdsAsString) allDeletedRows.Append(deletedRows.ToString());
                    string DeleteRecordsScript = "delete from " + deleteRowsArgs.TableName + " where " + deleteRowsArgs.KeyName + " in " + ("(" + deletedRows.ToString() + ")").Replace(",)", ")");
                    ServiceHelper.ExecuteSql(DeleteRecordsScript, deleteRowsArgs.ConnectionString);
                    rowsCount = 0;
                    deletedRows.Clear();
                }
            }

            return !string.IsNullOrEmpty(allDeletedRows.ToString()) ? ("(" + allDeletedRows.ToString() + ")").Replace(",)", ")") : null;

        }

        private void SetTablesStructureHelper(CargoTrackingUpdateDataBaseArgs cargoTrackingDataBaseArgs)
        {
            TableStructureHelper MainTableStructureHelper = cargoTrackingDataBaseArgs.BuildCargoArgs.Table.MainTableStructureHelper;
            TableStructureHelper InnerTableStructureHelper = null;
            cargoTrackingDataBaseArgs.BuildCargoArgs.MainTableStructureHelper = MainTableStructureHelper;
            if (!string.IsNullOrEmpty(cargoTrackingDataBaseArgs.BuildCargoArgs.Table.Pre_InnerTableName))
            {
                InnerTableStructureHelper = cargoTrackingDataBaseArgs.BuildCargoArgs.Table.InnerTableStructureHelper;
                cargoTrackingDataBaseArgs.BuildCargoArgs.InnerTableStructureHelper = InnerTableStructureHelper;
            }

        }

        private void SetBuildProcessData(CargoTrackingUpdateDataBaseArgs cargoTrackingDataBaseArgs)
        {
            if (cargoTrackingDataBaseArgs.CargoTrackingArguments != null)
            {
                ServiceHelper.DropTable(cargoTrackingDataBaseArgs.BuildCargoArgs);
                ServiceHelper.CreateCargoTrackingTable(cargoTrackingDataBaseArgs.BuildCargoArgs);
                cargoTrackingDataBaseArgs.BuildCargoArgs.Table.CargoTracking_TableName = cargoTrackingDataBaseArgs.BuildCargoArgs.Table.Pre_TableName;
                cargoTrackingDataBaseArgs.BuildCargoArgs.Table.CargoTracking_InnerTableName = cargoTrackingDataBaseArgs.BuildCargoArgs.Table.Pre_InnerTableName;
                
            }
        }

        public RecordUpdated UpdateCargoTracking(CargoTrackingUpdateDataBaseArgs cargoTrackingDataBaseArgs, RecordUpdated _recordUpdated)
        {
            if (!_recordUpdated.IsFromBuild || cargoTrackingDataBaseArgs.CargoTrackingArguments != null)
            {
                UpdateCargoTrackingRecords updateCargoTrackingRecords = new UpdateCargoTrackingRecords()
                {
                    IsUpadteWaterMark = true,
                    NumberRecordUpdated = 0,
                    NumberRecordUpdated2 = 0,
                    RecordUpdated = _recordUpdated,
                    CargoTrackingUpdateDataBaseArgs = cargoTrackingDataBaseArgs,
                };
                if (cargoTrackingDataBaseArgs.BuildCargoArgs.Table.ConditionsNumber == CargoTrackingTable_SingleCondition)
                {
                    UpdateCargoTrackingCondition(updateCargoTrackingRecords, GeneralTable_WithoutCustomCondition, true);

                }
                if (cargoTrackingDataBaseArgs.BuildCargoArgs.Table.ConditionsNumber == CargoTrackingTable_MultiConditions)
                {
                    UpdateCargoTrackingCondition(updateCargoTrackingRecords, ShipmentTable_GetAllCustomsShipmentsThatContainForwardingShipments);
                    UpdateCargoTrackingCondition(updateCargoTrackingRecords, ShipmentTable_GetAllNonCustomShipmentsThatContainForwardingShipments, true);
                }
                _recordUpdated.NumberOfRecordUpdated = updateCargoTrackingRecords.NumberRecordUpdated;
                _recordUpdated.NumberOfRecordUpdated2 = updateCargoTrackingRecords.NumberRecordUpdated2;
            }
            
            return _recordUpdated;
        }


     
        private void UpdateCargoTrackingCondition(UpdateCargoTrackingRecords updateCargoTrackingRecords,int CurrentCondition, bool IsUpadteWaterMark = false)
        {
            updateCargoTrackingRecords.CargoTrackingUpdateDataBaseArgs.BuildCargoArgs.Table.CurrentCondition = CurrentCondition;
            updateCargoTrackingRecords.IsUpadteWaterMark = IsUpadteWaterMark;
            updateCargoTrackingRecords.RecordUpdated = UpdateCargoTrackingDatabase(updateCargoTrackingRecords.CargoTrackingUpdateDataBaseArgs, updateCargoTrackingRecords.IsUpadteWaterMark);
            updateCargoTrackingRecords.NumberRecordUpdated += updateCargoTrackingRecords.RecordUpdated.NumberOfRecordUpdated;
            updateCargoTrackingRecords.NumberRecordUpdated2 += updateCargoTrackingRecords.RecordUpdated.NumberOfRecordUpdated2;
        }


    }

 
  
 

}
