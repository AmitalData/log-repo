
using Logitude.CargoTracking.BL.CargoTrackingServices.HelperClasses;
using Logitude.CargoTracking.BL.CargoTrackingServices.Services.CustomMapping;
using Logitude.CargoTracking.BL.CargoTrackingServices.Services.MainService;
using Logitude.CargoTracking.BL.CargoTrackingServices.Services.QueueServices;
using Logitude.CargoTracking.BL.CargoTrackingServices.Services.SearchService;
using Logitude.CargoTracking.BL.CargoTrackingServices.Services.ServicesHelper;
using Logitude.CargoTracking.BL.CargoTrackingServices.Services.TableLogic;
using Logitude.CargoTracking.BL.CargoTrackingServices.Services.TableStructure.Helper;
using Logitude.CargoTracking.BL.CargoTrackingServices.Services.ValidateRecords;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using Logitude.CargoTracking.BL.CargoTrackingServices.Services.TableConditions;

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
        const int ShipmentTable_GetShipmentOrders = 3;
        const int GeneralTable_WithoutCustomCondition = 0;
        ShipmentMilestonesSyncService syncService = new ShipmentMilestonesSyncService();
        CargoReferencesSyncQueueService cargoReferencesSyncQueueService = new CargoReferencesSyncQueueService();
        CargoDisconnectQueueService cargoDisconnectQueueService = new CargoDisconnectQueueService();


        public RecordUpdated UpdateCargoTrackingDataBase(CargoTrackingUpdateDataBaseArgs cargoTrackingDataBaseArgs)
        {

            SetTablesStructureHelper(cargoTrackingDataBaseArgs);
            SetBuildProcessData(cargoTrackingDataBaseArgs);
            RecordUpdated = new RecordUpdated();
            RecordUpdated.IsFromBuild = ServiceHelper.GetIsIncrementalRunning(cargoTrackingDataBaseArgs.BuildCargoArgs.SourceConnectionString);
            RecordUpdated = UpdateCargoTracking(cargoTrackingDataBaseArgs, RecordUpdated);
            return RecordUpdated;
        }

        private void UpdateCargoTracking(BulkDataPreperation bulkDataPreperation, List<DataColumn> dataColumnListCols
                       )
        {
            if (bulkDataPreperation.SqlDataReader.HasRows)
            {
                MapDataTableColumn(bulkDataPreperation, dataColumnListCols);
                UpdateCargoTrackingBulk(bulkDataPreperation, dataColumnListCols);
                UpdateLastRemainMainRecordsIfExist(bulkDataPreperation);
                UpdateLastRemainInnerRecordsIfExist(bulkDataPreperation);
                if (KeysForRecoredsNotValidated.Count > 0)
                {
                    RemoveNotValidLines(bulkDataPreperation.CargoTrackingUpdateDataBaseArgs);
                }
                bulkDataPreperation.SqlDataReader.Close();
            }
            else
            {

                bulkDataPreperation.SqlDataReader.Close();
            }
        }

        private void RemoveNotValidLines(CargoTrackingUpdateDataBaseArgs cargoTrackingDataBaseArgs)
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
            string deleteNotValidRecordsCommand = "";
            deleteNotValidRecordsCommand = "delete from " + cargoTrackingDataBaseArgs.BuildCargoArgs.Table.CargoTracking_TableName + " where " + cargoTrackingDataBaseArgs.BuildCargoArgs.Table.ConditionKey + " in " + ("(" + deletedRows.ToString() + ")").Replace(",)", ")");
            ServiceHelper.ExecuteSql(deleteNotValidRecordsCommand, cargoTrackingDataBaseArgs.BuildCargoArgs.DestinationConnectionString);
            if (cargoTrackingDataBaseArgs.BuildCargoArgs.Table.CargoTracking_InnerTableName != null)
            {
                deleteNotValidRecordsCommand = "delete from " + cargoTrackingDataBaseArgs.BuildCargoArgs.Table.CargoTracking_InnerTableName + " where " + cargoTrackingDataBaseArgs.BuildCargoArgs.Table.InnerConditionKey + " in " + ("(" + deletedRows.ToString() + ")").Replace(",)", ")");
                ServiceHelper.ExecuteSql(deleteNotValidRecordsCommand, cargoTrackingDataBaseArgs.BuildCargoArgs.DestinationConnectionString);
            }
            KeysForRecoredsNotValidated = new List<string>();

        }


        private void MapDataTableColumn(BulkDataPreperation bulkDataPreperation, List<DataColumn> dataColumnListCols)
        {
            if (bulkDataPreperation.CargoTrackingUpdateDataBaseArgs.DataTableSchema != null)
            {
                foreach (DataRow drow in bulkDataPreperation.CargoTrackingUpdateDataBaseArgs.DataTableSchema.Rows)
                {
                    DataColumn column = GetCoulmnFromDataRow(drow);
                    column.AllowDBNull = true;
                    dataColumnListCols.Add(column);

                    bulkDataPreperation.MainDataTable.Columns.Add(column);
                }

                AddDummyCoulmnsToDatatTable(bulkDataPreperation);
            }
        }

        private void UpdateCargoTrackingBulk(BulkDataPreperation bulkDataPreperation, List<DataColumn> dataColumnListCols)
        {
            while (bulkDataPreperation.SqlDataReader.Read())
            {
                bulkDataPreperation.MainDataTable = FillDataTableValues(dataColumnListCols, bulkDataPreperation, bulkDataPreperation.CargoTrackingUpdateDataBaseArgs.BuildCargoArgs.Table.Main_CargoTracking_TableName);

                bulkDataPreperation.NumberRecoredTake++;

                if (bulkDataPreperation.NumberRecoredTake == bulkDataPreperation.MaxRecoredTakeEachTime)
                {
                    UpdateCargoTrackingMainBulk(bulkDataPreperation, bulkDataPreperation.CargoTrackingUpdateDataBaseArgs);

                }
                if (bulkDataPreperation.InnerDataTable != null && bulkDataPreperation.InnerDataTable.Rows.Count >= bulkDataPreperation.MaxRecoredTakeEachTime)
                {
                    UpdateCargoTrackingInnerBulk(bulkDataPreperation, bulkDataPreperation.CargoTrackingUpdateDataBaseArgs);
                }
                if (KeysForRecoredsNotValidated.Count > 0)
                {
                    RemoveNotValidLines(bulkDataPreperation.CargoTrackingUpdateDataBaseArgs);

                }

            }
        }

        private void UpdateLastRemainMainRecordsIfExist(BulkDataPreperation bulkDataPreperation)
        {
            if (bulkDataPreperation.MainDataTable.Rows.Count > 0)
            {
                var Lastcolumns = bulkDataPreperation.MainDataTable.Rows
                                                     .Cast<DataRow>()
                                                     .Select(r => (string)r[bulkDataPreperation.CargoTrackingTable.KeyName].ToString())
                                                     .ToList();
                bulkDataPreperation.SelectedDataTable = bulkDataPreperation.MainDataTable;
                bulkDataPreperation = BulkUpdateTable(bulkDataPreperation, Lastcolumns);

            }
        }

        private void UpdateLastRemainInnerRecordsIfExist(BulkDataPreperation bulkDataPreperation)
        {
            if (bulkDataPreperation.InnerDataTable != null && bulkDataPreperation.InnerDataTable.Rows.Count > 0)
            {
                var Lastcolumns = bulkDataPreperation.InnerDataTable.Rows
                                                     .Cast<DataRow>()
                                                     .Select(r => (string)r[bulkDataPreperation.CargoTrackingTable.InnerKeyName].ToString())
                                                     .ToList();
                bulkDataPreperation.SelectedDataTable = bulkDataPreperation.InnerDataTable;
                bulkDataPreperation = BulkUpdateTable(bulkDataPreperation, Lastcolumns, true);
            }
        }

        private void AfterFinishUpdateCargoTracking(BulkDataPreperation bulkDataPreperation, bool isUpadteWaterMark)

        {

            if (bulkDataPreperation.CargoTrackingUpdateDataBaseArgs.CargoTrackingArguments != null)
            {
                BuildAllIndexesWithConstraient(bulkDataPreperation.CargoTrackingUpdateDataBaseArgs.BuildCargoArgs);
                Rename_Pre_Tables(bulkDataPreperation.CargoTrackingUpdateDataBaseArgs.BuildCargoArgs);
            }
            if (isUpadteWaterMark)
            {
                ServiceHelper.UpdateWaterMarkAfterFinishCheck(bulkDataPreperation.CargoTrackingTable, bulkDataPreperation.AutomaticLastUpdateDate, bulkDataPreperation.CargoTrackingUpdateDataBaseArgs.BuildCargoArgs);

            }


        }

        private static void CreatePreOldShipmentsTable(BulkDataPreperation bulkDataPreperation)
        {
            var tableName = bulkDataPreperation.CargoTrackingUpdateDataBaseArgs.BuildCargoArgs.Table.Main_CargoTracking_TableName;
            if (tableName == "CargoTrackingShipments")
            {
                ShipmentOldValuesService oldValuesService = new ShipmentOldValuesService();
                oldValuesService.CreatePreOldDataDBTable(bulkDataPreperation.CargoTrackingUpdateDataBaseArgs.BuildCargoArgs);
            }
        }
        private static void AddDefaultEntities(CargoTrackingArgs buildCargoArgs)
        {
            var tableName = buildCargoArgs.Table.Main_CargoTracking_TableName;
            if (tableName == "CargoTrackingCards")
            {
                AddDefaultShipper(buildCargoArgs);
                AddDefaultConsignee(buildCargoArgs);
            }
        }

        private static void AddDefaultConsignee(CargoTrackingArgs buildCargoArgs)
        {
            var sql = string.Concat(
                     $"IF NOT EXISTS (SELECT * from [dbo].[CargoTrackingCards] where id='DF-CONSIGNE') {Environment.NewLine} BEGIN  {Environment.NewLine} " +
                     $"INSERT INTO [dbo].[CargoTrackingCards] ([Id] ,[Code] ,[EnglishName] ,[LocalName] ,[Tenant]) ",
                     $"VALUES ('DF-CONSIGNE', 'no-consignee', 'No Consignee', N'No Consignee', 0)" +
                     $"END");
            ServiceHelper.ExecuteSql(sql, buildCargoArgs.DestinationConnectionString);
        }

        private static void AddDefaultShipper(CargoTrackingArgs buildCargoArgs)
        {
            var sql = string.Concat(
                                     $"IF NOT EXISTS (SELECT * from [dbo].[CargoTrackingCards] where id='DF-SHIPPER') {Environment.NewLine} BEGIN  {Environment.NewLine} " +
                                     $"INSERT INTO [dbo].[CargoTrackingCards] ([Id] ,[Code] ,[EnglishName] ,[LocalName] ,[Tenant]) ",
                                     $"VALUES ('DF-SHIPPER', 'no-shipper', 'No Shipper', N'No Shipper', 0) {Environment.NewLine}" +
                                     $"END");
            ServiceHelper.ExecuteSql(sql, buildCargoArgs.DestinationConnectionString);
        }

        private RecordUpdated UpdateCargoTrackingDatabase(UpdateCargoTrackingRecords updateCargoTrackingRecords, bool isUpadteWaterMark = false)
        {
            RecordUpdated recordUpdated = new RecordUpdated();
            KeysForRecoredsNotValidated = new List<string>();
            var cargoTrackingDataBaseArgs = updateCargoTrackingRecords.CargoTrackingUpdateDataBaseArgs;

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
                bulkDataPreperation.Milestones = updateCargoTrackingRecords.Milestones;
                bulkDataPreperation.AllTenantIds = updateCargoTrackingRecords.AllTenantIds;
                bulkDataPreperation.RecordUpdated = updateCargoTrackingRecords.RecordUpdated;
                bulkDataPreperation.MilestonesNotPermitted = updateCargoTrackingRecords.MilestonesNotPermitted;

                //CreatePreOldShipmentsTable(bulkDataPreperation);

                UpdateCargoTracking(bulkDataPreperation, dataColumnListCols);
                AfterFinishUpdateCargoTracking(bulkDataPreperation, isUpadteWaterMark);
            
                //SwapPreOldShipmentsWithOldShipmentsTable(bulkDataPreperation);

                sourceConnection.Close();
            }
            recordUpdated.NumberOfRecordUpdated = bulkDataPreperation.NumberOfMainCoulmnsUpdated;
            recordUpdated.NumberOfRecordUpdated2 = bulkDataPreperation.NumberOfInnerCoulmnsUpdated;
            if (bulkDataPreperation.AutomaticLastUpdateDate > cargoTrackingDataBaseArgs.MaxDate || cargoTrackingDataBaseArgs.MaxDate == null)
                cargoTrackingDataBaseArgs.MaxDate = bulkDataPreperation.AutomaticLastUpdateDate;
            return recordUpdated;
        }

        private static void SwapPreOldShipmentsWithOldShipmentsTable(BulkDataPreperation bulkDataPreperation)
        {
            if (bulkDataPreperation.NumberOfMainCoulmnsUpdated > 0 && bulkDataPreperation.CargoTrackingTable.CurrentCondition != ShipmentTable_GetAllNonCustomShipmentsThatContainForwardingShipments)
            {
                ShipmentOldValuesService oldValuesService = new ShipmentOldValuesService();
                oldValuesService.CreateOldCargoShipmentsFromPreOldCargoShipments(bulkDataPreperation.CargoTrackingUpdateDataBaseArgs.BuildCargoArgs);
            }
        }

        private void AddDummyCoulmnsToDatatTable(BulkDataPreperation bulkDataPreperation)
        {
            if (!string.IsNullOrEmpty(bulkDataPreperation.CargoTrackingTable.FieldsDummyName))
            {
                foreach (string ColumnName in bulkDataPreperation.CargoTrackingTable.FieldsDummyName.Split(','))
                {
                    DataColumn dataColumn = new DataColumn(ColumnName);
                    dataColumn.AllowDBNull = true;
                    bulkDataPreperation.MainDataTable.Columns.Add(dataColumn);
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

            DataTable dataTable = bulkDataPreperation.InnerDataTable.Clone();
            foreach (DataRow drtableOld in bulkDataPreperation.InnerDataTable.Rows)
            {
                dataTable.ImportRow(drtableOld);

            }
            bulkDataPreperation.InnerDataTable.Rows.Clear();
            bulkDataPreperation.CargoTrackingUpdateDataBaseArgs = cargoTrackingDataBaseArgs;
            bulkDataPreperation.SelectedDataTable = dataTable;
            BulkUpdateTable(bulkDataPreperation, columns, true);

        }



        private void UpdateCargoTrackingMainBulk(BulkDataPreperation bulkDataPreperation,
                                                 CargoTrackingUpdateDataBaseArgs cargoTrackingDataBaseArgs)
        {
            var columns = bulkDataPreperation.MainDataTable.Rows
                     .Cast<DataRow>()
                     .Select(r => (string)r[bulkDataPreperation.CargoTrackingTable.KeyName].ToString())
                     .ToList();
            DataTable dataTable = bulkDataPreperation.MainDataTable.Clone();
            foreach (DataRow drtableOld in bulkDataPreperation.MainDataTable.Rows)
            {
                dataTable.ImportRow(drtableOld);

            }
            bulkDataPreperation.MainDataTable.Rows.Clear();
            bulkDataPreperation.NumberRecoredTake = 0;
            bulkDataPreperation.CargoTrackingUpdateDataBaseArgs = cargoTrackingDataBaseArgs;
            bulkDataPreperation.SelectedDataTable = dataTable;

            BulkUpdateTable(bulkDataPreperation, columns);
        }

        private List<string> GetForwardingShipmentsIds(DataTable dataTable)
        {
            var ids = dataTable.Rows
                     .Cast<DataRow>().Where(r => r["EntityType"].ToString() == "O" && r["ForwardingShipmentHeaderId"] != null)
                     .Select(r => r["ForwardingShipmentHeaderId"].ToString())
                     .ToList();
            ids.AddRange(
                dataTable.Rows
                     .Cast<DataRow>().Where(r => r["EntityType"].ToString() == "F")
                     .Select(r => r["EntityId"].ToString())
                     .ToList()
                );
            ids.AddRange(
                dataTable.Rows
                     .Cast<DataRow>().Where(r => r["EntityType"].ToString() == "C" && r["ForwardingShipmentHeaderId"] != null)
                     .Select(r => r["ForwardingShipmentHeaderId"].ToString())
                     .ToList()
                );
            return ids;
        }

        private BulkDataPreperation InitializeBulkDataPreperation(CargoTrackingUpdateDataBaseArgs cargoTrackingDataBaseArgs)
        {
            BulkDataPreperation bulkDataPreperation = new BulkDataPreperation()
            {
                CargoTrackingTable = cargoTrackingDataBaseArgs.BuildCargoArgs.Table,
                MaxRecoredTakeEachTime = cargoTrackingDataBaseArgs.NumberOfBulkPerTime,
                CargoTrackingUpdateDataBaseArgs = cargoTrackingDataBaseArgs,
                MainDataTable = null,
                SqlDataReader = null,
                NumberOfMainCoulmnsUpdated = 0,
                NumberOfInnerCoulmnsUpdated = 0,
                NumberRecoredTake = 0,
                AutomaticLastUpdateDate = cargoTrackingDataBaseArgs.MaxDate
            };

            return bulkDataPreperation;

        }

        private BulkDataPreperation BulkUpdateTable(BulkDataPreperation bulkDataPreperation,
                                                           List<string> columns, bool
                                                           isInnerCargoTracking = false)
        {
            if (!isInnerCargoTracking)
                bulkDataPreperation.NumberOfMainCoulmnsUpdated += columns.Count;
            else
                bulkDataPreperation.NumberOfInnerCoulmnsUpdated += columns.Count;

            bulkDataPreperation.CargoTrackingTable = PrepareTableParameters(bulkDataPreperation, columns, isInnerCargoTracking);
            BulkUpdateValues(bulkDataPreperation, isInnerCargoTracking);
            var tableName = bulkDataPreperation.CargoTrackingUpdateDataBaseArgs.BuildCargoArgs.Table.Main_CargoTracking_TableName;
            if (tableName == "CargoTrackingShipments" && !bulkDataPreperation.CargoTrackingUpdateDataBaseArgs.IsUpdateFromBuild && !isInnerCargoTracking)
            {
                bulkDataPreperation.CargoTrackingUpdateDataBaseArgs.ForwardingShipmentsIds = GetForwardingShipmentsIds(bulkDataPreperation.SelectedDataTable);
                syncService.IncremantalSyncShipmentMilstones(bulkDataPreperation);
                cargoReferencesSyncQueueService.InsertToQueue(bulkDataPreperation);
                cargoDisconnectQueueService.InsertToQueue(bulkDataPreperation);
            }
            return bulkDataPreperation;
        }
        public void Rename_Pre_Tables(CargoTrackingArgs buildCargoArgs)
        {
            if (buildCargoArgs.Table.ConditionsNumber > 1)
            {
                if (buildCargoArgs.Table.CurrentCondition == 3)
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
                string innerTableChangeNameScript = buildCargoArgs.InnerTableStructureHelper.GetTableStructureChangeNameScript(buildCargoArgs.Table.Main_CargoTracking_InnerTableName, buildCargoArgs.Table.Main_CargoTracking_InnerTableName + "_SW") + "\n";
                innerTableChangeNameScript += buildCargoArgs.InnerTableStructureHelper.GetTableStructureChangeNameScript(buildCargoArgs.Table.Pre_InnerTableName, buildCargoArgs.Table.Main_CargoTracking_InnerTableName) + "\n";
                innerTableChangeNameScript += buildCargoArgs.InnerTableStructureHelper.GetTableStructureChangeNameScript(buildCargoArgs.Table.Main_CargoTracking_InnerTableName + "_SW", buildCargoArgs.Table.Pre_InnerTableName) + "\n";
                ServiceHelper.ExecuteSql(innerTableChangeNameScript, buildCargoArgs.DestinationConnectionString);
            }
        }

        private SqlDataReader GetSqlDataReader(CargoTrackingUpdateDataBaseArgs cargoTrackingDataBaseArgs,
                                               CargoTrackingTable table,
                                               SqlConnection sourceConnection)
        {
            string tableConditionCommand = "";
            if (cargoTrackingDataBaseArgs.BuildCargoArgs.Table.Main_CargoTracking_TableName == "CargoTrackingShipments"
                && cargoTrackingDataBaseArgs.BuildCargoArgs.Table.CurrentCondition == ShipmentTable_GetAllCustomsShipmentsThatContainForwardingShipments)
                tableConditionCommand = ShipmentTableCondtions.GetAllCustomsShipmentsThatContainForwardingShipments(cargoTrackingDataBaseArgs, table, LastUpdate);

            else if (cargoTrackingDataBaseArgs.BuildCargoArgs.Table.Main_CargoTracking_TableName == "CargoTrackingShipments"
                && cargoTrackingDataBaseArgs.BuildCargoArgs.Table.CurrentCondition == ShipmentTable_GetAllNonCustomShipmentsThatContainForwardingShipments)
                tableConditionCommand = ShipmentTableCondtions.GetAllNonCustomShipmentsThatContainForwardingShipments(cargoTrackingDataBaseArgs, table, LastUpdate);

            else if (cargoTrackingDataBaseArgs.BuildCargoArgs.Table.Main_CargoTracking_TableName == "CargoTrackingShipments"
                && cargoTrackingDataBaseArgs.BuildCargoArgs.Table.CurrentCondition == ShipmentTable_GetShipmentOrders)
                tableConditionCommand = ShipmentTableCondtions.GetShipmentOrders(cargoTrackingDataBaseArgs, table, LastUpdate);

            else
                tableConditionCommand = AddDefaultWhereCondition(cargoTrackingDataBaseArgs, table);

            SqlCommand commandSourceData = new SqlCommand(tableConditionCommand, sourceConnection);
            //commandSourceData.Transaction = sourceConnection.BeginTransaction(IsolationLevel.Snapshot);
            commandSourceData.CommandTimeout = (int)ServiceHelper.TimeOut;
            SqlDataReader reader = commandSourceData.ExecuteReader(CommandBehavior.CloseConnection);

            return reader;

        }

        private string AddDefaultWhereCondition(CargoTrackingUpdateDataBaseArgs cargoTrackingDataBaseArgs,
                                               CargoTrackingTable table)
        {
            string fieldName = !string.IsNullOrEmpty(cargoTrackingDataBaseArgs.BuildCargoArgs.Table.FieldsDBName) ? cargoTrackingDataBaseArgs.BuildCargoArgs.Table.FieldsDBName : "*";
            string condition = GetUpdateDataBaseCondition(cargoTrackingDataBaseArgs.BuildCargoArgs, cargoTrackingDataBaseArgs.CargoTrackingArguments);
            string tableConditionCommand = "SELECT " + fieldName + " " +
                    "FROM dbo." + table.DBTableName + condition;

            return tableConditionCommand;
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
            bool isRecoredValid = CargoTrackingValidateRecordsService.ValidateRecords(tableName, bulkDataPreperation.SqlDataReader);
            if (isRecoredValid)
            {
                AddValidRecoredToDataTable(listCols, bulkDataPreperation, tableName);
            }
            else
            {
                AddNotValidRecoredToRecoredsNotValidatedList(listCols, bulkDataPreperation);
            }
            return bulkDataPreperation.MainDataTable;
        }

        private void AddValidRecoredToDataTable(List<DataColumn> listCols, BulkDataPreperation bulkDataPreperation, string tableName)
        {
            DataRow dataRow = bulkDataPreperation.MainDataTable.NewRow();
            for (int i = 0; i < listCols.Count; i++)
            {
                dataRow[((DataColumn)listCols[i])] = bulkDataPreperation.SqlDataReader[i];
            }
            CargoTrackingSearchService.CreateSearchReferencesForShipment(dataRow, bulkDataPreperation, tableName);
            bulkDataPreperation.MainDataTable.Rows.Add(dataRow);
        }

        private void AddNotValidRecoredToRecoredsNotValidatedList(List<DataColumn> listCols, BulkDataPreperation bulkDataPreperation)
        {
            DataRow dataRow = bulkDataPreperation.MainDataTable.NewRow();
            for (int i = 0; i < listCols.Count; i++)
            {
                dataRow[((DataColumn)listCols[i])] = bulkDataPreperation.SqlDataReader[i];
            }
            KeysForRecoredsNotValidated.Add((string)dataRow[bulkDataPreperation.CargoTrackingTable.KeyName]);
        }


        private CargoTrackingTable PrepareTableParameters(BulkDataPreperation bulkDataPreperation,
                                                          List<string> columns,
                                                          bool isFromInnerCargoTrackingTable = false)
        {
            bulkDataPreperation.CargoTrackingTable.UpdatedCount = columns != null ? columns.Count() : 0;

            if (isFromInnerCargoTrackingTable)
            {
                CargoDeleteRowsArgs deleteRowsArgs = new CargoDeleteRowsArgs() { TableName = bulkDataPreperation.CargoTrackingTable.CargoTracking_InnerTableName, KeyName = bulkDataPreperation.CargoTrackingTable.InnerConditionKey, IdsList = columns, ConnectionString = bulkDataPreperation.CargoTrackingUpdateDataBaseArgs.BuildCargoArgs.DestinationConnectionString, ReturnDeleteIdsAsString = true };
                bulkDataPreperation.CargoTrackingTable.InnerRefreshIds = columns.ToString();
                if (bulkDataPreperation.CargoTrackingUpdateDataBaseArgs.CargoTrackingArguments == null)
                    bulkDataPreperation.CargoTrackingTable.InnerRefreshIds = DeleteRowsFromCargoTables(deleteRowsArgs);
            }
            else
            {
                CargoDeleteRowsArgs deleteRowsArgs = new CargoDeleteRowsArgs() { TableName = bulkDataPreperation.CargoTrackingTable.CargoTracking_TableName, KeyName = bulkDataPreperation.CargoTrackingTable.ConditionKey, IdsList = columns, ConnectionString = bulkDataPreperation.CargoTrackingUpdateDataBaseArgs.BuildCargoArgs.DestinationConnectionString, ReturnDeleteIdsAsString = true };
                bulkDataPreperation.CargoTrackingTable.RefreshIds = columns.ToString();
                if (bulkDataPreperation.CargoTrackingUpdateDataBaseArgs.CargoTrackingArguments == null)
                    bulkDataPreperation.CargoTrackingTable.RefreshIds = DeleteRowsFromCargoTables(deleteRowsArgs);
            }
            return bulkDataPreperation.CargoTrackingTable;
        }



        private DateTime? BulkUpdateValues(BulkDataPreperation bulkDataPreperation, bool isFromInnerCargoTrackingTable = false)
        {
            if (!string.IsNullOrEmpty(bulkDataPreperation.CargoTrackingTable.RefreshIds) ||
                !string.IsNullOrEmpty(bulkDataPreperation.CargoTrackingTable.InnerRefreshIds))
            {
                using (SqlConnection destinationConnection =
                                    new SqlConnection(bulkDataPreperation.CargoTrackingUpdateDataBaseArgs.BuildCargoArgs.DestinationConnectionString))
                {


                    UpdateCargoTrackingBulk(destinationConnection, bulkDataPreperation, isFromInnerCargoTrackingTable);

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
                catch (Exception exception)
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
            string ErrorsLog = "Table Name: " + bulkDataPreperation.CargoTrackingTable.Main_CargoTracking_TableName +
                                       Environment.NewLine + "Erros: " + exception.Message +
                                       Environment.NewLine + "Stack Trace: " + exception.StackTrace +
                                       Environment.NewLine;


            if (RecordUpdated.ErrorLogs == null)
                RecordUpdated.ErrorLogs = ErrorsLog;

            if (!RecordUpdated.ErrorLogs.Contains(ErrorsLog))
                RecordUpdated.ErrorLogs += ErrorsLog;

        }

        private void SetAutomaticLastUpdateDateAfterWriteData(BulkDataPreperation bulkDataPreperation)
        {
            var maxDate = ServiceHelper.GetAutomaticLastUpdateDate(bulkDataPreperation.SelectedDataTable, bulkDataPreperation.CargoTrackingTable.IsClosedTable);
            if (bulkDataPreperation.AutomaticLastUpdateDate == null)
                bulkDataPreperation.AutomaticLastUpdateDate = maxDate;
            else if (maxDate > bulkDataPreperation.AutomaticLastUpdateDate)
                bulkDataPreperation.AutomaticLastUpdateDate = maxDate;
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

            try
            {
                //bulkCopy.ColumnMappings.Clear();

                string[] targetColNames = bulkDataPreperation.CargoTrackingTable.CargoTracking_FieldsDBName.Split(',');
                var firstRow = bulkDataPreperation.SelectedDataTable.Rows[0];

                //foreach (string colName in targetColNames)
                //{
                //    var value = firstRow[colName];
                //    Console.WriteLine(colName + ": (" + value+ ") ,Type: " + value.GetType().Name);
                //    if (value.Equals("") && value.GetType().Name == "DateTime")
                //        value = DateTime.MinValue;
                //}
                //foreach (DataRow row in bulkDataPreperation.SelectedDataTable.Rows)
                //{
                //    foreach (string colName in targetColNames)
                //    {
                //        //var value = row[colName];
                //        ////Console.WriteLine(colName + ": (" + value + ") ,Type: " + value.GetType().Name);
                //        //if (value.Equals("") && colName.Contains("Date"))
                //        //{
                //        //    row[colName] = DateTime.Now;
                //        //}
                //    }
                //}
                //foreach (DataRow row in bulkDataPreperation.SelectedDataTable.Rows)
                //{
                //    foreach (string colName in targetColNames)
                //    {
                //        var value = row[colName];
                //        Console.WriteLine(colName + ": (" + value + ") ,Type: " + value.GetType().Name);

                //    }
                //}
                bulkCopy.WriteToServer(bulkDataPreperation.SelectedDataTable);

            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("Received an invalid column length from the bcp client for colid"))
                    ThrowColumnWidthValidation(bulkCopy, ex);
                throw ex;
            }
            catch (InvalidOperationException ex)
            {
                throw ex;
            }
            bulkCopy.Close();
        }

        private static bool IsFieldNullOrEmpty(DataRow tableRow, string coulmnName)
        {
            if (tableRow[coulmnName].Equals(null) || tableRow[coulmnName].Equals("") || tableRow[coulmnName].GetType().Name == "DBNull")
                return true;
            return false;
        }

        private static void ThrowColumnWidthValidation(SqlBulkCopy bulkCopy, SqlException ex)
        {
            string pattern = @"\d+";
            Match match = Regex.Match(ex.Message.ToString(), pattern);
            var index = Convert.ToInt32(match.Value) - 1;

            FieldInfo fi = typeof(SqlBulkCopy).GetField("_sortedColumnMappings", BindingFlags.NonPublic | BindingFlags.Instance);
            var sortedColumns = fi.GetValue(bulkCopy);
            var items = (Object[])sortedColumns.GetType().GetField("_items", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(sortedColumns);

            FieldInfo itemdata = items[index].GetType().GetField("_metadata", BindingFlags.NonPublic | BindingFlags.Instance);
            var metadata = itemdata.GetValue(items[index]);

            var column = metadata.GetType().GetField("column", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).GetValue(metadata);
            var length = metadata.GetType().GetField("length", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).GetValue(metadata);
            var message = (String.Format("Data copy failed: Column {0} contains data with a length greater than: {1}", column, length));
            throw new ApplicationException(message);
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
                    var args = new SetTableLogicArgs()
                    {
                        TableRow = dr,
                        TableName = bulkDataPreperation.CargoTrackingUpdateDataBaseArgs.BuildCargoArgs.Table.Main_CargoTracking_TableName,
                        ConditionNumber = bulkDataPreperation.CargoTrackingUpdateDataBaseArgs.BuildCargoArgs.Table.CurrentCondition,
                        Milestones = bulkDataPreperation.Milestones,
                        NotPermittedMilestones = bulkDataPreperation.MilestonesNotPermitted
                    };
                    CargoTrackingTableLogicService.SetTableLogic(args);
                }
                else
                {
                    var args = new SetTableLogicArgs()
                    {
                        TableRow = dr,
                        TableName = bulkDataPreperation.CargoTrackingUpdateDataBaseArgs.BuildCargoArgs.Table.Main_CargoTracking_InnerTableName,
                        ConditionNumber = bulkDataPreperation.CargoTrackingUpdateDataBaseArgs.BuildCargoArgs.Table.CurrentCondition,
                        Milestones = bulkDataPreperation.Milestones,
                        NotPermittedMilestones = bulkDataPreperation.MilestonesNotPermitted
                    };
                    CargoTrackingTableLogicService.SetTableLogic(args);
                }


            }

        }


        public void AutoMapColumns(SqlBulkCopy sqlBulkCopy, BulkDataPreperation bulkDataPreperation, bool isFromInnerCargoTracking = false)
        {
            List<string> mappingMatching = new List<string>();
            string[] dataBase_Columns = bulkDataPreperation.CargoTrackingTable.FieldsDBName.Split(',');
            string[] cargoTrackingDB_Columns = bulkDataPreperation.CargoTrackingTable.CargoTracking_FieldsDBName.Split(',');
            if (isFromInnerCargoTracking)
                cargoTrackingDB_Columns = bulkDataPreperation.CargoTrackingTable.InnerCargoTracking_FieldsDBName.Split(',');
            string compareDB = MapMatchingColumnsName(cargoTrackingDB_Columns, dataBase_Columns, mappingMatching);
            for (int i = 0; i < cargoTrackingDB_Columns.Length; i++)
            {
                var CompareList = compareDB.Split(',').ToList();
                if (!CompareList.Contains(cargoTrackingDB_Columns[i]))
                {
                    bulkDataPreperation.CoulmnForCusstomMapping = cargoTrackingDB_Columns[i];
                    DataRowCustomMap(bulkDataPreperation, sqlBulkCopy, isFromInnerCargoTracking);
                }
            }
            AddMappingMatchingToBulkColumMap(sqlBulkCopy, mappingMatching);
        }
        private void DataRowCustomMap(BulkDataPreperation bulkDataPreperation,
                                       SqlBulkCopy sqlBulkCopy,
                                       bool isFromInnerCargoTracking)
        {
            if (isFromInnerCargoTracking)
            {
                CargoTrackingCustomMappingService.MapCargoTrackingToDataBase(bulkDataPreperation, sqlBulkCopy, bulkDataPreperation.CargoTrackingTable.Main_CargoTracking_InnerTableName);
            }
            else
            {
                CargoTrackingCustomMappingService.MapCargoTrackingToDataBase(bulkDataPreperation, sqlBulkCopy, bulkDataPreperation.CargoTrackingTable.Main_CargoTracking_TableName);
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
        private string MapMatchingColumnsName(string[] CargoTrackingDB_Columnss,
                                              string[] DataBase_Columnss,
                                              List<string> MappingMatching)
        {
            StringBuilder compareDB = new StringBuilder();
            foreach (string CTDB_columns in CargoTrackingDB_Columnss)
            {
                foreach (string DB_columns in DataBase_Columnss)
                {
                    if (CTDB_columns.Equals(DB_columns))
                    {
                        MappingMatching.Add(DB_columns + "," + CTDB_columns);
                        compareDB.Append(CTDB_columns + ",");
                    }
                }

            }

            return compareDB.ToString();
        }

        private string GetUpdateDataBaseCondition(CargoTrackingArgs buildCargoArgs, CargoTrackingArguments cargoTrackingArguments = null)
        {
            GetLastUpdateIfArgumentsProvided(buildCargoArgs, cargoTrackingArguments);
            BuildWhereConditionArgs buildWhereConditionArgs = new BuildWhereConditionArgs()
            {
                TableName = buildCargoArgs.Table.Main_CargoTracking_TableName,
                LastUpdate = LastUpdate,
                CargoTrackingArguments = cargoTrackingArguments,
            };
            string condition = CargoTrackingTableBuildWhereCondition.BuildWhereCondition(buildWhereConditionArgs, buildCargoArgs.Table.IsClosedTable);
            return condition;
        }

        private void GetLastUpdateIfArgumentsProvided(CargoTrackingArgs buildCargoArgs, CargoTrackingArguments cargoTrackingArguments)
        {
            if (cargoTrackingArguments == null)
            {
                LastUpdate = ServiceHelper.GetTableLastUpdate(buildCargoArgs.Table.Main_CargoTracking_TableName, buildCargoArgs.DestinationConnectionString);
            }

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
            TableStructureHelper mainTableStructureHelper = cargoTrackingDataBaseArgs.BuildCargoArgs.Table.MainTableStructureHelper;
            TableStructureHelper innerTableStructureHelper = null;
            cargoTrackingDataBaseArgs.BuildCargoArgs.MainTableStructureHelper = mainTableStructureHelper;
            if (!string.IsNullOrEmpty(cargoTrackingDataBaseArgs.BuildCargoArgs.Table.Pre_InnerTableName))
            {
                innerTableStructureHelper = cargoTrackingDataBaseArgs.BuildCargoArgs.Table.InnerTableStructureHelper;
                cargoTrackingDataBaseArgs.BuildCargoArgs.InnerTableStructureHelper = innerTableStructureHelper;
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
                    BuildShipments(updateCargoTrackingRecords);
                }
                _recordUpdated.NumberOfRecordUpdated = updateCargoTrackingRecords.NumberRecordUpdated;
                _recordUpdated.NumberOfRecordUpdated2 = updateCargoTrackingRecords.NumberRecordUpdated2;
            }

            return _recordUpdated;
        }
        private bool CheckWhichMethodUse()
        {
            XmlDocument _document = new XmlDocument();
            _document.Load("./CargoTrackingConfig.xml");

            var node = _document.GetElementsByTagName("ApplyNewMethod");
            foreach (XmlNode item in node)
            {
                return item.InnerText.ToLower() == "true";
            }
            return false;

        }
        private void BuildShipments(UpdateCargoTrackingRecords updateCargoTrackingRecords)
        {
            var cargoTrackingShipmentsService = new CargoTrackingShipmentsService();
            updateCargoTrackingRecords.Milestones = cargoTrackingShipmentsService.GetMilestones(updateCargoTrackingRecords.CargoTrackingUpdateDataBaseArgs.BuildCargoArgs.DestinationConnectionString);
            updateCargoTrackingRecords.AllTenantIds = ServiceHelper.GetAllTenants(updateCargoTrackingRecords.CargoTrackingUpdateDataBaseArgs.BuildCargoArgs.SourceConnectionString); ;
            updateCargoTrackingRecords.MilestonesNotPermitted = cargoTrackingShipmentsService.GetAllNotPermittedMilestones(updateCargoTrackingRecords.CargoTrackingUpdateDataBaseArgs.BuildCargoArgs.SourceConnectionString);

            if (false)
            {
                BuildShipmentsNew(updateCargoTrackingRecords);
            }
            else
            {
                var LastUpdate = ServiceHelper.GetTableLastUpdate(updateCargoTrackingRecords.CargoTrackingUpdateDataBaseArgs.BuildCargoArgs.Table.Main_CargoTracking_TableName, updateCargoTrackingRecords.CargoTrackingUpdateDataBaseArgs.BuildCargoArgs.DestinationConnectionString);
                updateCargoTrackingRecords.CargoTrackingUpdateDataBaseArgs.ShipmentsWaterMark = LastUpdate;
                UpdateCargoTrackingCondition(updateCargoTrackingRecords, ShipmentTable_GetAllCustomsShipmentsThatContainForwardingShipments);
                UpdateCargoTrackingCondition(updateCargoTrackingRecords, ShipmentTable_GetAllNonCustomShipmentsThatContainForwardingShipments, true);
                UpdateCargoTrackingCondition(updateCargoTrackingRecords, ShipmentTable_GetShipmentOrders, true);
                if (updateCargoTrackingRecords.NumberRecordUpdated > 0)
                {
                    SyncShipments(updateCargoTrackingRecords);
                }
            }



        }

        private void SyncShipments(UpdateCargoTrackingRecords updateCargoTrackingRecords)
        {
            if (updateCargoTrackingRecords.CargoTrackingUpdateDataBaseArgs.IsUpdateFromBuild)
            {
                syncService.BuildSyncShipmentMilstones(updateCargoTrackingRecords);
                return;
            }
        }

        private void BuildShipmentsNew(UpdateCargoTrackingRecords updateCargoTrackingRecords)
        {
            var cargoTrackingShipmentsService = new CargoTrackingShipmentsService();
            var result = cargoTrackingShipmentsService.Update(updateCargoTrackingRecords);
            updateCargoTrackingRecords.NumberRecordUpdated += result.RecordsNumber;
            if (updateCargoTrackingRecords.CargoTrackingUpdateDataBaseArgs.CargoTrackingArguments != null)
            {
                updateCargoTrackingRecords.CargoTrackingUpdateDataBaseArgs.BuildCargoArgs.Table.CurrentCondition = ShipmentTable_GetAllNonCustomShipmentsThatContainForwardingShipments;
                BuildAllIndexesWithConstraient(updateCargoTrackingRecords.CargoTrackingUpdateDataBaseArgs.BuildCargoArgs);
                StartRenameCargoTables(updateCargoTrackingRecords.CargoTrackingUpdateDataBaseArgs.BuildCargoArgs);
            }
            if (result.RecordsNumber > 0)
            {
                ServiceHelper.UpdateWaterMarkAfterFinishCheck(updateCargoTrackingRecords.CargoTrackingUpdateDataBaseArgs.BuildCargoArgs.Table, result.LastUpdateDate, updateCargoTrackingRecords.CargoTrackingUpdateDataBaseArgs.BuildCargoArgs);

            }
        }

        private void UpdateCargoTrackingCondition(UpdateCargoTrackingRecords updateCargoTrackingRecords, int CurrentCondition, bool IsUpadteWaterMark = false)
        {
            updateCargoTrackingRecords.CargoTrackingUpdateDataBaseArgs.BuildCargoArgs.Table.CurrentCondition = CurrentCondition;
            updateCargoTrackingRecords.IsUpadteWaterMark = IsUpadteWaterMark;
            updateCargoTrackingRecords.RecordUpdated = UpdateCargoTrackingDatabase(updateCargoTrackingRecords, updateCargoTrackingRecords.IsUpadteWaterMark);
            updateCargoTrackingRecords.NumberRecordUpdated += updateCargoTrackingRecords.RecordUpdated.NumberOfRecordUpdated;
            updateCargoTrackingRecords.NumberRecordUpdated2 += updateCargoTrackingRecords.RecordUpdated.NumberOfRecordUpdated2;
        }
    }
}
