
using Logitude.CargoTracking.BL.CargoTrackingServices.HelperClasses;
using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 

namespace Logitude.CargoTracking.BL.CargoTrackingServices.Services
{
    public class CargoTrackingMainService
    {
        public static long timeOut = 10000000000000000;
        public string LastUpdate;
        public RecordUpdated recordUpdated = new RecordUpdated();
        public List<CargoTable> FillCargoTableList()
        {
            List<CargoTable> CargoTableLists = new List<CargoTable>();

            CargoTableLists.Add(new CargoTable()
            {
                TableName = "Port",
                FieldsDBName = "Id,Tenant,Code,CountryId,EnglishName,AutomaticLastUpdateDate",
                CT_FieldsDBName = "Id,Code,EnglishName,CountryId",
                KeyName = "Id",
                ConditionKey = "Id",
                DBTableName = "Ports",
                CT_TableName = "CargoTrackingPorts",
                Main_CT_TableName = "CargoTrackingPorts",
                Pre_TableName = "Pre_CargoTrackingPorts",
                ConditionsNumber = 1,
            });

            CargoTableLists.Add(new CargoTable()
            {
                TableName = "Card",
                FieldsDBName = "Id,Tenant,Code,LocalName,EnglishName,AutomaticLastUpdateDate",
                KeyName = "Id",
                ConditionKey = "Id",
                CT_FieldsDBName = "Id,Code,EnglishName,LocalName",
                DBTableName = "Cards",
                CT_TableName = "CargoTrackingCards",
                Main_CT_TableName = "CargoTrackingCards",
                Pre_TableName = "Pre_CargoTrackingCards",
                ConditionsNumber = 1,
            });

            CargoTableLists.Add(new CargoTable()
            {
                TableName = "TransportModes",
                FieldsDBName = "Id,Name,SearchFields,AutomaticLastUpdateDate",
                KeyName = "Id",
                ConditionKey = "Id",
                CT_FieldsDBName = "Id,Name,SearchFields",
                DBTableName = "TransportModes",
                CT_TableName = "CargoTrackingTransportModes",
                Main_CT_TableName = "CargoTrackingTransportModes",
                Pre_TableName = "Pre_CargoTrackingTransportModes",
                ConditionsNumber = 1,

            });

            CargoTableLists.Add(new CargoTable()
            {
                TableName = "Countries",
                FieldsDBName = "Id,LocalName,Code,EnglishName,AutomaticLastUpdateDate",
                KeyName = "Id",
                ConditionKey = "Id",
                CT_FieldsDBName = "Id,LocalName,Code,EnglishName",
                DBTableName = "Countries",
                CT_TableName = "CargoTrackingCountries",
                Main_CT_TableName = "CargoTrackingCountries",
                Pre_TableName = "Pre_CargoTrackingCountries",
                ConditionsNumber = 1,

            });

            CargoTableLists.Add(new CargoTable()
            {
                TableName = "Shipment",
                FieldsDBName = "Id,Tenant,CustomerId,TransportModeId,MasterShipmentDataId,House,ShipmentNumber,FromPortId,ToPortId,ShipperId,ConsigneeId,GrossWeight,Volume,CustomConnectToShipment,AutomaticLastUpdateDate,ShipmentPickUpIndex,FirstPickupETA,ShipmentLevelCode,CustomsClearanceDate,CustomFileId,CreateDateTime,SecurityKey,ConsigneeName,ShipperName,CustomerReference1,CustomerReference2",
                KeyName = "Id",
                ConditionKey = "EntityId",
                DBTableName = "Shipments",
                CT_TableName = "CargoTrackingShipments",
                Main_CT_TableName = "CargoTrackingShipments",
                CT_FieldsDBName = "Tenant,IsMainRecord,CustomerId,TransportModeId,Master,House,ShipmentNumber,FromPortId,ToPortId,ShipperId,ConsigneeId,GrossWeight,Volume,PickupDone,PickupDate,PickupEstimationDate,FromWarehouseDone,FromWarehouseNotes,DepartureDate,DepartureEstimationDate,ClearanceDone,ClearanceDate,FromWarehouseEstimationDate,FromWarehouseDate,DepartureDone,ArrivalDone,ToWarehouseDate,ToWarehouseEstimationDate,DeliveredEstimationDate,DeliveredDone,DeliveredDate,ArrivalEstimationDate,ToWarehouseDone,ArrivalDate,EntityId,EntityType,ForwardingShipmentHeaderId,CustomsShipmentHeaderId,CurrentMilestoneCode,CurrentMilestoneDate,ClearanceDone,ClearanceDate,ToWarehouseNotes,CustomsPaymentDone,CustomsPaymentDate,CreateDate,SecurityKey,ConsigneeName,ShipperName,CustomerReference",
                Condition1 = " ((ShipmentLevelCode ='D' or ShipmentLevelCode ='H') and CustomFileId is not null)",
                Condition2 = " ((ShipmentLevelCode !='D' and ShipmentLevelCode !='H') or CustomFileId is null)",
                Pre_TableName = "Pre_CargoTrackingShipments",
                ConditionsNumber = 2,
            });


            CargoTableLists.Add(new CargoTable()
            {
                TableName = "Shipments",
                FieldsDBName = "Id,Tenant,ShipmentNumber,SecurityKey,SearchFields,CreateDateTime,CustomerReference1,CustomerReference2,AutomaticLastUpdateDate",
                KeyName = "Id",
                ConditionKey = "SecurityKey",
                CT_FieldsDBName = "Tenant,SecurityKey,SearchFields,ShipmentDate",
                DBTableName = "Shipments",
                CT_TableName = "CargoTrackingShipmentSearches",
                Main_CT_TableName = "CargoTrackingShipmentSearches",
                Pre_TableName = "Pre_CargoTrackingShipmentSearches",
                ConditionsNumber = 1,

            });

  

            return CargoTableLists;

        }


        public RecordUpdated UpdateCTDataBase(CargoArgs buildCargoArgs, int NumberOfBulkPerTime, bool? IsUpdateAfterFinished = null, CargoTrackingArguments CargoTrackingArguments = null)
        {
            //CargoTrackingArguments = new CargoTrackingArguments(); // Just For Devlopment
            //CargoTrackingArguments.FromDate = new DateTime(2011, 1, 1);
            //CargoTrackingArguments.ToDate = new DateTime(2020, 7, 7);


            int NumberRecordUpdated = 0;

            if (CargoTrackingArguments!=null)
            {
                DropTable(buildCargoArgs.Table.Pre_TableName, buildCargoArgs.DestinationConnectionString);
                CheckPreTablesIsExist(buildCargoArgs.Table.Pre_TableName, buildCargoArgs.DestinationConnectionString);
                buildCargoArgs.Table.CT_TableName = buildCargoArgs.Table.Pre_TableName;
            }

            recordUpdated.IsFromBuild = GetIsIncrementalRunning(buildCargoArgs.SourceConnectionString);

            if (!recordUpdated.IsFromBuild || CargoTrackingArguments!=null) {
                
                bool IsUpadteWaterMark = false;
                if (buildCargoArgs.Table.Condition1 == null)
                {
                    buildCargoArgs.Table.CurrentCondition = 0;
                    IsUpadteWaterMark = true;
                    NumberRecordUpdated += UpdateCTService(buildCargoArgs, NumberOfBulkPerTime, IsUpdateAfterFinished, CargoTrackingArguments, null, IsUpadteWaterMark);
                }
                if (buildCargoArgs.Table.Condition1 != null)
                {
                    buildCargoArgs.Table.CurrentCondition = 1;
                    if (buildCargoArgs.Table.Condition2 == null)
                    {
                        IsUpadteWaterMark = true;
                    }
                    NumberRecordUpdated += UpdateCTService(buildCargoArgs, NumberOfBulkPerTime, IsUpdateAfterFinished, CargoTrackingArguments, buildCargoArgs.Table.Condition1, IsUpadteWaterMark);
                }
                if (buildCargoArgs.Table.Condition2 != null)
                {
                    buildCargoArgs.Table.CurrentCondition = 2;
                    if (buildCargoArgs.Table.Condition3 == null)
                    {
                        IsUpadteWaterMark = true;
                    }
                    NumberRecordUpdated += UpdateCTService(buildCargoArgs, NumberOfBulkPerTime, IsUpdateAfterFinished, CargoTrackingArguments, buildCargoArgs.Table.Condition2, IsUpadteWaterMark);
                }
                if (buildCargoArgs.Table.Condition3 != null)
                {
                    buildCargoArgs.Table.CurrentCondition = 3;
                    IsUpadteWaterMark = true;
                    NumberRecordUpdated += UpdateCTService(buildCargoArgs, NumberOfBulkPerTime, IsUpdateAfterFinished, CargoTrackingArguments, buildCargoArgs.Table.Condition3, IsUpadteWaterMark);
                }
            }
            recordUpdated.NumberOfRecordUpdated = NumberRecordUpdated;
            

            return recordUpdated;
        }
        private  int UpdateCTService(CargoArgs buildCargoArgs, int NumberOfBulkPerTime, bool? IsUpdateAfterFinished = null, CargoTrackingArguments CargoTrackingArguments = null,string Condition =null, bool IsUpadteWaterMark=false)
        {

            CargoTable table = buildCargoArgs.Table;
            DataTable dataTable = null;
            DateTime? automaticLastUpdateDate = null;
            int NumberOfCoulmnsUpdated = 0;
            int MaxRecoredTakeEachTime = NumberOfBulkPerTime;
            int NumberRecoredTake = 0;
            using (SqlConnection sourceConnection =
                                  new SqlConnection(buildCargoArgs.SourceConnectionString))
            {
                sourceConnection.Open();
                SqlDataReader reader = GetSqlDataReader(buildCargoArgs, table, sourceConnection, CargoTrackingArguments, Condition);
                DataTable dtSchema = reader.GetSchemaTable();
                List<DataColumn> listCols = new List<DataColumn>();
                dataTable = new DataTable();

                if (reader.HasRows)
                {
                    if (dtSchema != null)
                    {
                        foreach (DataRow drow in dtSchema.Rows)
                        {
                            DataColumn column = GetCoulmnFromDataRow(drow);
                            listCols.Add(column);
                            if (column.ColumnName != "IDE")
                            {
                                dataTable.Columns.Add(column);

                            }
                        }
                    }
                    while (true)
                    {

                        while (reader.Read())
                        {

                            dataTable = FillDataTableValues(listCols, reader, dataTable, buildCargoArgs.Table.Main_CT_TableName);

                            NumberRecoredTake++;

                            if (NumberRecoredTake == MaxRecoredTakeEachTime)
                            {
                                break;
                            }

                        }

                        var columns = dataTable.Rows
                                         .Cast<DataRow>()
                                         .Select(r => (string)r[table.KeyName].ToString())
                                         .ToList();

                        NumberOfCoulmnsUpdated += columns.Count;

                        table = PrepareTableParameters(buildCargoArgs, table, columns);

                        automaticLastUpdateDate = UpdateBulkValues(buildCargoArgs, dataTable, table, automaticLastUpdateDate, IsUpdateAfterFinished, CargoTrackingArguments);

                        if (NumberRecoredTake != MaxRecoredTakeEachTime)
                        {
                            reader.Close();
                            break;
                        }
                        dataTable.Rows.Clear();
                        NumberRecoredTake = 0;
                    }

                }
                else
                {
                 
                    reader.Close();
                }
                if (CargoTrackingArguments != null)
                {
                    Rename_Pre_Tables(buildCargoArgs, Condition);
                }
                if (IsUpadteWaterMark)
                {
                    UpdateWaterMarkAfterFinishCheck(table, automaticLastUpdateDate, buildCargoArgs);

                }

            }
            return NumberOfCoulmnsUpdated;

        }
        public void Rename_Pre_Tables(CargoArgs buildCargoArgs , string Condition)
        {
            if (buildCargoArgs.Table.ConditionsNumber > 1)
            {
                switch (buildCargoArgs.Table.ConditionsNumber)
                {

                    case 2:
                        {
                            if (buildCargoArgs.Table.Condition2 == Condition)
                            {
                                StartRenameCargoTables(buildCargoArgs);
                            }
                            break;
                        }
                    case 3:
                        {
                            if (buildCargoArgs.Table.Condition3 == Condition)
                            {
                                StartRenameCargoTables(buildCargoArgs);
                            }
                            break;
                        }

                }

            }
            else
            {
                StartRenameCargoTables(buildCargoArgs);
            }
         

        }

        private void StartRenameCargoTables(CargoArgs buildCargoArgs)
        {
            string cmd = ChaneNameScript(buildCargoArgs.Table.Main_CT_TableName, buildCargoArgs.Table.Main_CT_TableName + "_SW") + "\n";
            cmd += ChaneNameScript(buildCargoArgs.Table.Pre_TableName, buildCargoArgs.Table.Main_CT_TableName) + "\n";
            cmd += ChaneNameScript(buildCargoArgs.Table.Main_CT_TableName + "_SW", buildCargoArgs.Table.Pre_TableName) + "\n";
            ExecuteSql(cmd, buildCargoArgs.DestinationConnectionString);
        }
        private string  ChaneNameScript(string Old, string New)
        {

            string cmd = "EXEC sp_rename '" + Old + "', '" + New  + "' \n ";
            cmd += " exec sp_rename 'PK_"+ Old + "', 'PK_"+ New + "', 'object' \n ";
            if (Old== "CargoTrackingShipments" || New== "CargoTrackingShipments")
            {
                cmd += " exec sp_rename 'FK_"+ Old + "_CargoTrackingHeaderEntityTypes_EntityType', 'FK_"+ New + "_CargoTrackingHeaderEntityTypes_EntityType', 'object' \n ";
                cmd += " exec sp_rename 'FK_"+ Old + "_CargoTrackingMilestones_CurrentMilestoneCode', 'FK_"+ New + "_CargoTrackingMilestones_CurrentMilestoneCode', 'object' \n ";
                cmd += " exec sp_rename 'UQ_"+ Old + "_EntityType_EntityId_Tenant', 'UQ_"+ New + "_EntityType_EntityId_Tenant', 'object' \n ";

            }
            return cmd;
        }

        private SqlDataReader GetSqlDataReader(CargoArgs buildCargoArgs, CargoTable table, SqlConnection sourceConnection, CargoTrackingArguments CargoTrackingArguments = null, string Condition=null)
        {
            string cmd;
            string condition;
            string fieldName = !string.IsNullOrEmpty(buildCargoArgs.Table.FieldsDBName) ? buildCargoArgs.Table.FieldsDBName : "*";
           
            if (buildCargoArgs.Table.Main_CT_TableName == "CargoTrackingShipments" && buildCargoArgs.Table.CurrentCondition==1)
            {
                fieldName= " C." + fieldName.Replace(",", " ,C.");
                cmd = "SELECT "+ fieldName+ ", P.Id as ForwardingIdForCustom " + " FROM dbo." + table.DBTableName + " P JOIN dbo." + table.DBTableName+ " C ON P.CustomFileId = C.Id";
                if (CargoTrackingArguments == null)
                {
                    LastUpdate = GetTableLastUpdate(buildCargoArgs.Table.CT_TableName, buildCargoArgs.DestinationConnectionString);
                    cmd += " where (C.AutomaticLastUpdateDate > '" + LastUpdate + "')";
                }
                else
                {
                    cmd += " where C.CreateDateTime >= '" + CargoTrackingArguments.FromDate + "' and C.CreateDateTime <= '" + CargoTrackingArguments.ToDate + "'";

                }
            }
            else if (buildCargoArgs.Table.Main_CT_TableName == "CargoTrackingShipments" && buildCargoArgs.Table.CurrentCondition == 2)
            {
                cmd = "Select "+ fieldName  + " FROM dbo. " + table.DBTableName + " Where Id not in (Select C.Id From  dbo." + table.DBTableName + " P JOIN dbo." + table.DBTableName + " C ON P.CustomFileId = C.Id)";

                if (CargoTrackingArguments == null)
                {
                    LastUpdate = GetTableLastUpdate(buildCargoArgs.Table.CT_TableName, buildCargoArgs.DestinationConnectionString);
                    cmd += " and (AutomaticLastUpdateDate > '" + LastUpdate + "')";
                }
                else
                {
                    cmd += " and CreateDateTime >= '" + CargoTrackingArguments.FromDate + "' and CreateDateTime <= '" + CargoTrackingArguments.ToDate + "'";

                }

            }
            else
            {
                  condition = GetUpdateDataBaseCondition(buildCargoArgs, CargoTrackingArguments, Condition);
                  cmd = "SELECT " + fieldName + " " +
                          "FROM dbo." + table.DBTableName + condition;
            }


            SqlCommand commandSourceData = new SqlCommand(cmd, sourceConnection);

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

        private DataTable FillDataTableValues(List<DataColumn> listCols, SqlDataReader reader, DataTable dataTable, string TableName)
        {
            bool IsRoWValid = CargoTrackingBlockRecordsService.BlockRecords(TableName, reader);
            if (IsRoWValid)
            {
                DataRow dataRow = dataTable.NewRow();
                for (int i = 0; i < listCols.Count; i++)
                {
                    dataRow[((DataColumn)listCols[i])] = reader[i];
                }
                CargoTrackingSearchService.SearchService(dataRow, dataTable, TableName);
                dataTable.Rows.Add(dataRow);
            }


            return dataTable;
        }

        private CargoTable PrepareTableParameters(CargoArgs buildCargoArgs, CargoTable table, List<string> columns)
        {
            table.UpdatedCount = columns != null ? columns.Count() : 0;
            table.RefreshIds = DeleteRowsFromCargoTables(new CargoDeleteRowsArgs() { TableName = table.CT_TableName, KeyName = table.ConditionKey, IdsList = columns, ConnectionString = buildCargoArgs.DestinationConnectionString, ReturnDeleteIdsAsString = true });

            return table;
        }

        private DateTime? UpdateBulkValues(CargoArgs buildCargoArgs, DataTable dataTable, CargoTable table, DateTime? automaticLastUpdateDate, bool? IsUpdateAfterFinished, CargoTrackingArguments CargoTrackingArguments=null)
        {
            if (!string.IsNullOrEmpty(table.RefreshIds))
            {

                using (SqlConnection destinationConnection =
                                    new SqlConnection(buildCargoArgs.DestinationConnectionString))
                {
                    destinationConnection.Open();

                    using (SqlBulkCopy bulkCopy =
                               new SqlBulkCopy(buildCargoArgs.DestinationConnectionString, SqlBulkCopyOptions.KeepIdentity))
                    {
                        bulkCopy.DestinationTableName =
                            "dbo." + table.CT_TableName;

                        bulkCopy.BulkCopyTimeout = (int)timeOut;

                        try
                        {
                            AutoMapColumns(bulkCopy, dataTable, table);


                            foreach (DataRow dr in dataTable.Rows)
                            {
                                CargoTrackingTableLogicService.SetTableLogic(dr, buildCargoArgs.Table.Main_CT_TableName, buildCargoArgs.Table.CurrentCondition);

                            }
 
                            bulkCopy.EnableStreaming = true;
                            bulkCopy.BatchSize = 100000;
                            bulkCopy.WriteToServer(dataTable);

                        }

                        finally
                        {
                            if (  CargoTrackingArguments ==null )
                            {
                                automaticLastUpdateDate = GetAutomaticLastUpdateDate(automaticLastUpdateDate, dataTable, table.IsClosedTable);

                            }
                            else
                            {
                                automaticLastUpdateDate = null;
                            }
                        }
                    }

                }

            }
            return automaticLastUpdateDate;
        }

        private DateTime? GetAutomaticLastUpdateDate(DateTime? automaticLastUpdateDate, DataTable dataTable,bool IsClosed)
        {

            if (!IsClosed)
            {
                var MaxUpdate = (DateTime)dataTable.Rows
                                                  .Cast<DataRow>()
                                                  .Max(d => d["AutomaticLastUpdateDate"]);

                if (MaxUpdate > automaticLastUpdateDate || automaticLastUpdateDate == null)
                {
                    automaticLastUpdateDate = (DateTime)dataTable.Rows
                   .Cast<DataRow>()
                   .Max(d => d["AutomaticLastUpdateDate"]);
                }
            }
           

            return automaticLastUpdateDate;
        }

        private void UpdateWaterMarkAfterFinishCheck(CargoTable table, DateTime? automaticLastUpdateDate, CargoArgs buildCargoArgs)
        {
            if (table != null && table.DBTableName != "CargoTrackingWatermarks")
            {
               
                var lastUpdateDate = string.Empty;
                if (automaticLastUpdateDate != null) lastUpdateDate = automaticLastUpdateDate.Value.ToString("MM/dd/yyyy hh:mm:ss.fff tt");
                else lastUpdateDate = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt");
                UpdateWaterMarksTable(table, lastUpdateDate, buildCargoArgs.DestinationConnectionString);
                table.IsUpdated = true;

            }
        }

        public void AutoMapColumns(SqlBulkCopy sbc, DataTable dt, CargoTable Table)
        {
            List<string> MappingMatching = new List<string>();
            List<string> MappingDeference = new List<string>();

            string[] DB_Cols = Table.FieldsDBName.Split(',');
            string[] CTDB_Cols = Table.CT_FieldsDBName.Split(',');
            string CompareDB = "";
            foreach (string CTDB_columns in CTDB_Cols)
            {
                foreach (string DB_columns in DB_Cols)
                {
                    if (CTDB_columns.Equals(DB_columns))
                    {
                        MappingMatching.Add(DB_columns + "," + CTDB_columns);
                        CompareDB += CTDB_columns + ",";
                    }
                }

            }
            foreach (string DB_columns in CTDB_Cols)
            {
                if (!CompareDB.Contains(DB_columns))
                {
                    CargoTrackingCustomMappingService.MappingDB_CTDB(dt, sbc, DB_columns, Table.Main_CT_TableName);
                }
            }

            foreach (string columns in MappingMatching)
            {
                string[] Cols = columns.Split(',');
                string col1 = Cols[0];
                string col2 = Cols[1];
                sbc.ColumnMappings.Add(col1, col2);
            }
        }

        public void CheckAndUpdateWaterMark(string dbSourceConnection, string dbDestenationConnection)
        {

            List<CargoTable> CargoTableLists = FillCargoTableList();
            foreach (CargoTable table in CargoTableLists)
            {
                if (table.DBTableName != "CargoTrackingWatermarks")
                {
                    using (SqlConnection SourceConnection =
                         new SqlConnection(dbSourceConnection))
                    {
                        SourceConnection.Open();

                        SqlCommand commandSourceData = new SqlCommand(
                       "SELECT  TableName" +
                       " FROM dbo.CargoTrackingWatermarks WHERE TableName = '" + table.CT_TableName + "'", SourceConnection);

                        SqlDataReader reader = commandSourceData.ExecuteReader();
                        if (!reader.HasRows)
                        {
                            string lastUpdateDate = GetAutomaticLastUpdateDate(table.DBTableName, dbDestenationConnection);
                            if (string.IsNullOrEmpty(lastUpdateDate)) lastUpdateDate = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt");
                            AddWaterMarksRecord(table, lastUpdateDate, dbSourceConnection);

                        }
                        else
                        {
                            SourceConnection.Close();

                        }
                    }
                }
            }

        }

        public void UpdateWaterMarksTable(CargoTable table, string date, string connectionString)
        {
            var TodayDate = TenantServerConfigration.GetCurrentDateTime(0);
            string cmd = "update  CargoTrackingWatermarks set LastUpdateDate = '" + date + "',LastRun = '"+ TodayDate + "' where tableName = '" + table.CT_TableName + "'";
            ExecuteSql(cmd, connectionString);

        }

        public void AddWaterMarksRecord(CargoTable table, string date, string connectionString)
        {
            string cmd = "insert into CargoTrackingWatermarks  values('" + table.CT_TableName + "' , NULL,NULL)";
            ExecuteSql(cmd, connectionString);
        }
        public void UpdateIsIncrementalRunning(int IsRunning, string connectionString)
        {
            string cmd = "Update Tenants set IsIncrementalBuildRunning = " + IsRunning +" Where Id = 0";
            ExecuteSql(cmd, connectionString);
        }
        public void ExecuteSql(string sqlString, string connectionString)
        {

            if (!string.IsNullOrEmpty(sqlString))
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(sqlString, cn);
                    sqlCommand.CommandTimeout = (int)timeOut;
                    cn.Open();
                    sqlCommand.ExecuteNonQuery();
                    cn.Close();
                }
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
                    string cmd = "delete from " + deleteRowsArgs.TableName + " where " + deleteRowsArgs.KeyName + " in " + ("(" + deletedRows.ToString() + ")").Replace(",)", ")");
                    ExecuteSql(cmd, deleteRowsArgs.ConnectionString);
                    rowsCount = 0;
                    deletedRows.Clear();
                }
            }

            return !string.IsNullOrEmpty(allDeletedRows.ToString()) ? ("(" + allDeletedRows.ToString() + ")").Replace(",)", ")") : null;

        }

        public void DropTable(string TableName, string ConnectionString)
        {
            string cmd = "If exists (select * from sysobjects where name='" + TableName + "' and xtype='U') " +
                              " BEGIN " +
                              " Drop Table "+ TableName +
                              " END ";

             
            ExecuteSql(cmd, ConnectionString);
        }

        public void CheckPreTablesIsExist(string TableName, string ConnectionString)
        {
            string SQL = null;
            switch (TableName)
            {
                case "Pre_CargoTrackingPorts":
                    {
                        SQL = CreateTable_Pre_Ports(TableName);
                        break;
                    }
                case "Pre_CargoTrackingCards":
                    {
                        SQL = CreateTable_Pre_Cards(TableName);
                        break;
                    }
                case "Pre_CargoTrackingTransportModes":
                    {
                        SQL = CreateTable_Pre_TransportModes(TableName);
                        break;
                    }
                case "Pre_CargoTrackingCountries":
                    {
                        SQL = CreateTable_Pre_Countries(TableName);
                        break;
                    }
                case "Pre_CargoTrackingShipments":
                    {
                        SQL = CreateTable_Pre_Shipments(TableName) ;
                        SQL += CreateIndexAndRelations_Pre_Shipments(TableName);
                        break;
                    }
                case "Pre_CargoTrackingShipmentSearches":
                    {
                        SQL  = CreateTable_Pre_ShipmentSearchs(TableName) ;
                        SQL += CreateIndex_Pre_ShipmentSearchs(TableName);
                        break;
                    }
            }
            ExecuteSql(SQL, ConnectionString);
        }

 
        private string CreateIndex_Pre_ShipmentSearchs(string TableName)
        {
            string cmd = "CREATE NONCLUSTERED INDEX [IX_"+ TableName + "_Tenant_ShipmentDate_SearchFields] ON [dbo].["+ TableName + "]([Tenant],[ShipmentDate],[SearchFields]) End";

            return cmd;

        }

        private string CreateIndexAndRelations_Pre_Shipments(string TableName)
        {
            string cmd = "ALTER TABLE [dbo].["+ TableName + "] ADD CONSTRAINT [UQ_"+TableName+"_EntityType_EntityId_Tenant] UNIQUE([EntityType],[EntityId],[Tenant])\n";
            cmd += "ALTER TABLE [dbo].["+ TableName + "] ADD CONSTRAINT [FK_"+ TableName + "_CargoTrackingHeaderEntityTypes_EntityType] FOREIGN KEY([EntityType]) REFERENCES [dbo].[CargoTrackingHeaderEntityTypes]([Code])\n";
            cmd += "CREATE NONCLUSTERED INDEX [IX_" + TableName + "_EntityType] ON [dbo].[" + TableName + "]([EntityType])\n";
            cmd += "ALTER TABLE [dbo].["+ TableName + "] ADD CONSTRAINT [FK_"+ TableName + "_CargoTrackingMilestones_CurrentMilestoneCode] FOREIGN KEY([CurrentMilestoneCode]) REFERENCES [dbo].[CargoTrackingMilestones]([Code])\n";
            cmd += "CREATE NONCLUSTERED INDEX [IX_"+ TableName + "_CurrentMilestoneCode] ON [dbo].["+ TableName + "]([CurrentMilestoneCode]) End \n";

            return cmd;

        }

        private string CreateTable_Pre_Ports(string TableName)
        {
            string cmd = "If not exists (select * from sysobjects where name='"+TableName+"' and xtype='U')" +
                            "BEGIN " +
                            "CREATE TABLE [dbo].[" + TableName + "](" +
                            "[Id] VARCHAR(15) NOT NULL," +
                            "[Code] VARCHAR(3) NOT NULL," +
                            "[EnglishName] VARCHAR(40) NULL," +
                            "[CountryId] VARCHAR(15) NOT NULL," +
                            "CONSTRAINT[PK_"+ TableName + "] PRIMARY KEY([Id])" +
                            ")" +
                            " End";

            return cmd;

        }

        private string CreateTable_Pre_Cards(string TableName)
        {
            string cmd = "If not exists (select * from sysobjects where name='" + TableName + "' and xtype='U')" +
                            "BEGIN " +
                            "CREATE TABLE[dbo].["+ TableName + "]("+
                            "[Id] VARCHAR(15) NOT NULL,"+
                            "[Code] VARCHAR(15) NOT NULL,"+
                            "[EnglishName] VARCHAR(70) NULL,"+
                            "[LocalName] NVARCHAR(100) NULL,"+
                            "CONSTRAINT[PK_"+ TableName + "] PRIMARY KEY([Id])"+
                            ")" +
                            " End";

            return cmd;

        }

        private string CreateTable_Pre_Countries(string TableName)
        {
            string cmd = "If not exists (select * from sysobjects where name='" + TableName + "' and xtype='U')" +
                            "BEGIN " +
                            "CREATE TABLE[dbo].["+ TableName + "]("+
                            "[Id] VARCHAR(15) NOT NULL,"+
                            "[LocalName] NVARCHAR(120) NULL,"+
                            "[Code] CHAR(2) NOT NULL,"+
                            "[EnglishName] VARCHAR(120) NOT NULL,"+
                            "CONSTRAINT[PK_"+ TableName + "] PRIMARY KEY([Id])"+
                            ")"+
                            " End";

            return cmd;

        }

        private string CreateTable_Pre_TransportModes(string TableName)
        {
            string cmd = "If not exists (select * from sysobjects where name='" + TableName + "' and xtype='U')" +
                            "BEGIN " +
                            "CREATE TABLE[dbo].["+ TableName + "]("+
                            "[Id] CHAR(1) NOT NULL,"+
                            "[SearchFields] NVARCHAR(1000) NULL,"+
                            "[Name] VARCHAR(10) NOT NULL,"+
                            "CONSTRAINT[PK_"+ TableName + "] PRIMARY KEY([Id])"+
                            ")"+
                            " End";

            return cmd;

        }

        private string CreateTable_Pre_ShipmentSearchs(string TableName)
        {
            string cmd = "If not exists (select * from sysobjects where name='" + TableName + "' and xtype='U')" +
                            "BEGIN " +
                            "CREATE TABLE[dbo].["+ TableName + "]("+
                            "[Tenant] INT NOT NULL,"+
                            "[SearchFields] NVARCHAR(100) NULL,"+
                            "[SecurityKey] VARCHAR(40)   NULL," +
                            "[ShipmentDate] DATETIME NOT NULL,"+
                            "[Id] INT IDENTITY(1,1) NOT NULL,"+
                            "CONSTRAINT[PK_"+ TableName + "] PRIMARY KEY([Id])"+
                            ")" +
                            " \n";

            return cmd;

        }

        private string CreateTable_Pre_Shipments(string TableName)
        {
            string cmd = "If not exists (select * from sysobjects where name='" + TableName + "' and xtype='U')" +
                            "BEGIN " +
                            "CREATE TABLE[dbo].["+ TableName + "]("+
                            "[Id] INT IDENTITY(1,1) NOT NULL,"+
                            "[Tenant] INT NOT NULL,"+
                            "[EntityId] VARCHAR(15) NULL,"+
                            "[ForwardingShipmentHeaderId] VARCHAR(15) NULL,"+
                            "[CustomsShipmentHeaderId] VARCHAR(15) NULL,"+
                            "[EntityType] VARCHAR(1) NULL,"+
                            "[CurrentMilestoneCode] VARCHAR(2) NULL,"+
                            "[CurrentMilestoneDate] DATETIME NULL,"+
                            "[CustomerId] VARCHAR(15) NULL,"+
                            "[TransportModeId] VARCHAR(15) NULL,"+
                            "[Master] VARCHAR(20) NULL,"+
                            "[House] VARCHAR(20) NULL,"+
                            "[ShipmentNumber] VARCHAR(20) NULL,"+
                            "[FromPortId] VARCHAR(15) NULL,"+
                            "[ToPortId] VARCHAR(15) NULL,"+
                            "[ShipperId] VARCHAR(15) NULL,"+
                            "[ConsigneeId] VARCHAR(15) NULL,"+
                            "[GrossWeight] FLOAT NULL,"+
                            "[Volume] FLOAT NULL,"+
                            "[PickupDone] BIT NULL,"+
                            "[FromWarehouseDone] BIT NULL," +
                            "[ClearanceDone] BIT NULL," +
                            "[DepartureDone] BIT NULL," +
                            "[ArrivalDone] BIT NULL," +
                            "[ToWarehouseDate] DATETIME NULL," +
                            "[DeliveredEstimationDate] DATETIME NULL," +
                            "[ToWarehouseDone] BIT NULL," +
                            "[CustomsPaymentDone] BIT NULL," +
                            "[ClearanceDone] BIT NULL," +
                            "[DeliveredDone] BIT NULL," +
                            "[PickupDate] DATETIME NULL," +
                            "[DeliveredDate] DATETIME NULL," +
                            "[ArrivalDate] DATETIME NULL," +
                            "[ArrivalEstimationDate] DATETIME NULL," +
                            "[PickupEstimationDate] DATETIME NULL," +
                            "[FromWarehouseEstimationDate] DATETIME NULL," +
                            "[ToWarehouseEstimationDate] DATETIME NULL," +
                            "[FromWarehouseDate] DATETIME NULL," +
                            "[ClearanceDate] DATETIME NULL," +
                            "[DepartureDate] DATETIME NULL," +
                            "[DepartureEstimationDate] DATETIME NULL," +
                            "[CreateDate] DATETIME NOT NULL," +
                            "[CustomsPaymentDate] DATETIME NOT NULL," +
                            "[ClearanceDate] DATETIME NOT NULL," +
                            "[SecurityKey] VARCHAR(40) NULL," +
                            "[ConsigneeName] VARCHAR(70) NULL,"+
                            "[ShipperName] VARCHAR(70) NULL,"+
                            "[FromWarehouseNotes] NVARCHAR(32) NULL," +
                            "[ToWarehouseNotes] NVARCHAR(32) NULL," +
                            "[CustomerReference] VARCHAR(101) NULL," +
                            "CONSTRAINT[PK_"+ TableName + "] PRIMARY KEY([Id])"+
                            ")"+
                            "\n";

            return cmd;

        }


        public string BuildConnectionString(string catalog, string userName, string password, string server)
        {
            string result = "Data Source=" + server + ";Initial Catalog=" + catalog + ";Integrated Security=False;Persist Security Info=True;User ID=" + userName + ";Password= " + password + ";MultipleActiveResultSets=True;Connect Timeout=60";
            return result;
        }
      
        private string GetUpdateDataBaseCondition(CargoArgs buildCargoArgs, CargoTrackingArguments CargoTrackingArguments = null, string Condition=null)
        {
            //string LastUpdate = null;
            if (CargoTrackingArguments == null)
            {
                  LastUpdate = GetTableLastUpdate(buildCargoArgs.Table.CT_TableName, buildCargoArgs.DestinationConnectionString);
            }
            string condition = CargoTrackingTableCondition.Condition(buildCargoArgs.Table.Main_CT_TableName, LastUpdate, CargoTrackingArguments, Condition);
            return condition;
        }


        public string GetTableLastUpdate(string tableName, string connectionString)
        {

 
            string result = null;

            SqlConnection con = new SqlConnection(connectionString);

            SqlCommand com = new SqlCommand(
               "Select top 1 LastUpdateDate from CargoTrackingWatermarks where TableName = '" + tableName + "';", con);
            try
            {
                com.CommandTimeout = (int)timeOut;
                con.Open();
                using (SqlDataReader reader = com.ExecuteReader())
                {
                    reader.Read();
                    DateTime? datetime = null;
                    var value = reader["LastUpdateDate"];
                    if (value != null)
                    {
                        if (!string.IsNullOrEmpty(value.ToString()))
                        {
                            datetime = (DateTime?)(value);
                            if (datetime != null) result = datetime.Value.ToString("MM/dd/yyyy hh:mm:ss.fff tt");
                        }

                    }
                }
            }
            finally
            {
                con.Close();
            }
            return result;
        }

        public string GetAutomaticLastUpdateDate(string tableName, string connectionString)
        {
            string result = null;

            SqlConnection con = new SqlConnection(connectionString);

            SqlCommand com = new SqlCommand(
               "select MIN(AutomaticLastUpdateDate) -1 AutomaticLastUpdateDate " +
               "FROM dbo." + tableName + " ;", con);

            try
            {
                com.CommandTimeout = (int)timeOut;
                con.Open();
                using (SqlDataReader reader = com.ExecuteReader())
                {
                    reader.Read();
                    DateTime? datetime = null;
                    var value = reader["AutomaticLastUpdateDate"];
                    if (value != null)
                    {
                        if (!string.IsNullOrEmpty(value.ToString()))
                        {
                            datetime = (DateTime?)(value);
                            if (datetime != null) result = datetime.Value.ToString("MM/dd/yyyy hh:mm:ss.fff tt");
                        }

                    }
                }
            }
            finally
            {
                con.Close();
            }
            return result;
        }

        public bool GetIsIncrementalRunning(string connectionString)
        {
            bool Result = false;

            SqlConnection con = new SqlConnection(connectionString);

            SqlCommand com = new SqlCommand(
               "Select  IsIncrementalBuildRunning FROM dbo.Tenants Where Id = 0", con);
            try
            {
                com.CommandTimeout = (int)timeOut;
                con.Open();
                using (SqlDataReader reader = com.ExecuteReader())
                {
                    reader.Read();
                    bool IsRunning = false;
                    var value = reader["IsIncrementalBuildRunning"];
                    if (value != null)
                    { 
                            IsRunning = (bool)(value);
                            if (IsRunning != null) Result = IsRunning ;
      

                    }
                }
            }
            finally
            {
                con.Close();
            }
            return Result;
        }
    }

    public class CargoTrackingArguments
    {
        public DateTime? FromDate;
        public DateTime? ToDate;
        public int? Tenant;

    }

    public class RecordUpdated
    {
        public int NumberOfRecordUpdated {get;set;}
        public bool IsFromBuild { get; set; }
    }

}
