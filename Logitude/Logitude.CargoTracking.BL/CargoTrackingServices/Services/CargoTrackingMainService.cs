
using Logitude.CargoTracking.BL.CargoTrackingServices.HelperClasses;
using Logitude.CargoTracking.BL.CargoTrackingServices.Services.ServicesHelper;
using Logitude.CargoTracking.BL.CargoTrackingServices.Services.TableStructure;
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
        public RecordUpdated recordUpdated = new RecordUpdated();
        public int MainThreadNumbers = 3;
        int ThreadsNumber = 0;
        int ThreadsCompleatedWork = 0;
        private object threadLock = new object();
        private List<string> KeysForRecoredsNotValidated;

        //private static readonly Semaphore WorkLimiter = new Semaphore(100, 100);

        public RecordUpdated UpdateCargoTrackingDataBase(CargoTrackingUpdateDataBaseArgs cargoTrackingDataBaseArgs)
        {


            recordUpdated = new RecordUpdated();
            if (cargoTrackingDataBaseArgs.CargoTrackingArguments != null)
            {
                ServiceHelper.DropTable(cargoTrackingDataBaseArgs.buildCargoArgs);
                ServiceHelper.CreateCargoTrackingTable(cargoTrackingDataBaseArgs.buildCargoArgs);
                cargoTrackingDataBaseArgs.buildCargoArgs.Table.CT_TableName = cargoTrackingDataBaseArgs.buildCargoArgs.Table.Pre_TableName;
                cargoTrackingDataBaseArgs.buildCargoArgs.Table.CT2_TableName = cargoTrackingDataBaseArgs.buildCargoArgs.Table.Pre2_TableName;
               // this.MainThreadNumbers = (int)cargoTrackingDataBaseArgs.CargoTrackingArguments.ThreadNumber;
            }

            recordUpdated.IsFromBuild = ServiceHelper.GetIsIncrementalRunning(cargoTrackingDataBaseArgs.buildCargoArgs.SourceConnectionString);
            recordUpdated = TableCheckConditions(cargoTrackingDataBaseArgs, recordUpdated);
 
            return recordUpdated;
        }

        private void RemoveNotValidateLines(CargoTrackingUpdateDataBaseArgs cargoTrackingDataBaseArgs)
        {
            if (cargoTrackingDataBaseArgs.CargoTrackingArguments == null)
            {
                StringBuilder deletedRows = new StringBuilder();
                string cmd = "";
                foreach (string id in KeysForRecoredsNotValidated)
                {
                    deletedRows.Append("'" + id + "'" + ",");
                }

                if (cargoTrackingDataBaseArgs.buildCargoArgs.Table.CT2_TableName == null)
                {
                    cmd = "delete from " + cargoTrackingDataBaseArgs.buildCargoArgs.Table.CT_TableName + " where " + cargoTrackingDataBaseArgs.buildCargoArgs.Table.ConditionKey + " in " + ("(" + deletedRows.ToString() + ")").Replace(",)", ")");
                    ServiceHelper.ExecuteSql(cmd, cargoTrackingDataBaseArgs.buildCargoArgs.DestinationConnectionString);
                    KeysForRecoredsNotValidated = new List<string>();
                }
                else if (cargoTrackingDataBaseArgs.buildCargoArgs.Table.CT2_TableName != null)
                {
                    cmd = "delete from " + cargoTrackingDataBaseArgs.buildCargoArgs.Table.CT_TableName + " where " + cargoTrackingDataBaseArgs.buildCargoArgs.Table.ConditionKey + " in " + ("(" + deletedRows.ToString() + ")").Replace(",)", ")");
                    ServiceHelper.ExecuteSql(cmd, cargoTrackingDataBaseArgs.buildCargoArgs.DestinationConnectionString);
                    cmd = "delete from " + cargoTrackingDataBaseArgs.buildCargoArgs.Table.CT2_TableName + " where " + cargoTrackingDataBaseArgs.buildCargoArgs.Table.ConditionKey2 + " in " + ("(" + deletedRows.ToString() + ")").Replace(",)", ")");
                    ServiceHelper.ExecuteSql(cmd, cargoTrackingDataBaseArgs.buildCargoArgs.DestinationConnectionString);
                    KeysForRecoredsNotValidated = new List<string>();
                }

            }
          


        }
        private RecordUpdated UpdateCargoTrackingService(CargoTrackingUpdateDataBaseArgs cargoTrackingDataBaseArgs, string Condition =null, bool IsUpadteWaterMark=false)
        {
            ThreadsNumber = 0;
            ThreadsCompleatedWork = 0;
           

            RecordUpdated _RecordUpdated = new RecordUpdated();
            KeysForRecoredsNotValidated = new List<string>();
            BulkDataPreperation bulkDataPreperation = InitializeBulkDataPreperation(cargoTrackingDataBaseArgs);
            //if (bulkDataPreperation.cargoTable.Main_CT_TableName == "CargoTrackingShipments" && cargoTrackingDataBaseArgs.CargoTrackingArguments != null)
            //{
            //    ThreadPool.SetMinThreads(1, 1);
            //    ThreadPool.SetMaxThreads(MainThreadNumbers, MainThreadNumbers);
            //}
            bool IsLastRecord = false;
            using (SqlConnection sourceConnection =
                                  new SqlConnection(cargoTrackingDataBaseArgs.buildCargoArgs.SourceConnectionString))
            {
                sourceConnection.Open();
                bulkDataPreperation.sqlDataReader = GetSqlDataReader(cargoTrackingDataBaseArgs, bulkDataPreperation.cargoTable, sourceConnection, Condition);
                DataTable dtSchema = bulkDataPreperation.sqlDataReader.GetSchemaTable();
                List<DataColumn> listCols = new List<DataColumn>();
                bulkDataPreperation.dataTable = new DataTable();

                if (bulkDataPreperation.sqlDataReader.HasRows)
                {
                    if (dtSchema != null)
                    {
                        foreach (DataRow drow in dtSchema.Rows)
                        {
                            DataColumn column = GetCoulmnFromDataRow(drow);
                            listCols.Add(column);
                            bulkDataPreperation.dataTable.Columns.Add(column);
                        }
                    }

                    while (bulkDataPreperation.sqlDataReader.Read())
                    {
                        bulkDataPreperation.dataTable = FillDataTableValues(listCols, bulkDataPreperation, cargoTrackingDataBaseArgs.buildCargoArgs.Table.Main_CT_TableName);

                        bulkDataPreperation.NumberRecoredTake++;

                        if (bulkDataPreperation.NumberRecoredTake == bulkDataPreperation.MaxRecoredTakeEachTime)
                        {
                            RunBulkThreads(bulkDataPreperation, cargoTrackingDataBaseArgs);
                        }
                        if (bulkDataPreperation.dataTable2 != null && bulkDataPreperation.dataTable2.Rows.Count >= bulkDataPreperation.MaxRecoredTakeEachTime)
                        {
                            RunBulkThreads2(bulkDataPreperation, cargoTrackingDataBaseArgs);
                        }
                        if (KeysForRecoredsNotValidated.Count>0)
                        {
                            RemoveNotValidateLines(cargoTrackingDataBaseArgs);

                        }

                        if (ThreadsNumber != 0 && ThreadsNumber % MainThreadNumbers == 0)
                        {
                            while (ThreadsCompleatedWork < ThreadsNumber)
                            {

                            }
                        }
                    }
                    while (ThreadsCompleatedWork < ThreadsNumber)
                    {

                    }
                    if (bulkDataPreperation.dataTable.Rows.Count > 0)
                    {
                        var Lastcolumns = bulkDataPreperation.dataTable.Rows
                                                             .Cast<DataRow>()
                                                             .Select(r => (string)r[bulkDataPreperation.cargoTable.KeyName].ToString())
                                                             .ToList();

                        bulkDataPreperation = UpdateBulkPreperations(bulkDataPreperation, cargoTrackingDataBaseArgs, bulkDataPreperation.dataTable, Lastcolumns);
                    }

                    if (bulkDataPreperation.dataTable2 != null && bulkDataPreperation.dataTable2.Rows.Count > 0)
                    {
                        var Lastcolumns = bulkDataPreperation.dataTable2.Rows
                                                             .Cast<DataRow>()
                                                             .Select(r => (string)r[bulkDataPreperation.cargoTable.KeyName2].ToString())
                                                             .ToList();

                        bulkDataPreperation = UpdateBulkPreperations(bulkDataPreperation, cargoTrackingDataBaseArgs, bulkDataPreperation.dataTable2, Lastcolumns, true);
                    }
                    if (KeysForRecoredsNotValidated.Count > 0)
                    {
                        RemoveNotValidateLines(cargoTrackingDataBaseArgs);
                    }
                    bulkDataPreperation.sqlDataReader.Close();



                }
                else
                {

                    bulkDataPreperation.sqlDataReader.Close();
                }
                if (cargoTrackingDataBaseArgs.CargoTrackingArguments != null)
                {
                    Rename_Pre_Tables(cargoTrackingDataBaseArgs.buildCargoArgs, Condition);
                }
                if (IsUpadteWaterMark)
                {
                    ServiceHelper.UpdateWaterMarkAfterFinishCheck(bulkDataPreperation.cargoTable, bulkDataPreperation.automaticLastUpdateDate, cargoTrackingDataBaseArgs.buildCargoArgs);

                }
                sourceConnection.Close();
            }
             _RecordUpdated.NumberOfRecordUpdated = bulkDataPreperation.NumberOfCoulmnsUpdated;
            _RecordUpdated.NumberOfRecordUpdated2 = bulkDataPreperation.NumberOfCoulmnsUpdated2;

            return _RecordUpdated;

        }


        private void BuildAllIndexesWithConstraient(CargoArgs buildCargoArgs)
        {
            if (buildCargoArgs.Table.CT_TableName == "Pre_CargoTrackingShipments" && buildCargoArgs.Table.CurrentCondition == 2)
            {
                string cmd2 = ShipmentTableStrucrue.CreateConstraientWithRelations_Pre_Shipments(buildCargoArgs.Table.CT_TableName);
                ServiceHelper.ExecuteSql(cmd2, buildCargoArgs.DestinationConnectionString);
                cmd2 = ShipmentTableStrucrue.ReBuildIndexes_Pre_Shipments(buildCargoArgs.Table.CT_TableName);
                ServiceHelper.ExecuteSql(cmd2, buildCargoArgs.DestinationConnectionString);
            }


            if (buildCargoArgs.Table.CT2_TableName == "Pre_CargoTrackingShipmentSearches" && buildCargoArgs.Table.CurrentCondition == 2)
            {
                string cmd = ShipmentSearcheTableStrucrue.ReBuildIndexes_Pre_ShipmentSearchs(buildCargoArgs.Table.CT2_TableName);
                ServiceHelper.ExecuteSql(cmd, buildCargoArgs.DestinationConnectionString);
            }


        }

        private void RunBulkThreads2(BulkDataPreperation bulkDataPreperation , CargoTrackingUpdateDataBaseArgs cargoTrackingDataBaseArgs)
        {
 
                var columns = bulkDataPreperation.dataTable2.Rows
                         .Cast<DataRow>()
                         .Select(r => (string)r[bulkDataPreperation.cargoTable.KeyName2].ToString())
                         .ToList();
               
                DataTable ThreadDataTable = bulkDataPreperation.dataTable2.Clone();
                foreach (DataRow drtableOld in bulkDataPreperation.dataTable2.Rows)
                {
                    ThreadDataTable.ImportRow(drtableOld);

                }
                bulkDataPreperation.dataTable2.Rows.Clear();
            //if (bulkDataPreperation.cargoTable.Main_CT_TableName == "CargoTrackingShipments" && cargoTrackingDataBaseArgs.CargoTrackingArguments != null)
            //{
            //     WorkLimiter.WaitOne();
            //     ThreadPool.QueueUserWorkItem(o => BuildThreadPool(bulkDataPreperation, cargoTrackingDataBaseArgs, ThreadDataTable, columns, true));
            //}
            //else
            //{
                BuildThreadPool(bulkDataPreperation, cargoTrackingDataBaseArgs, ThreadDataTable, columns, true);
            //}
                
                

        }

   

        private void RunBulkThreads(BulkDataPreperation bulkDataPreperation, CargoTrackingUpdateDataBaseArgs cargoTrackingDataBaseArgs)
        {

            
                var columns = bulkDataPreperation.dataTable.Rows
                         .Cast<DataRow>()
                         .Select(r => (string)r[bulkDataPreperation.cargoTable.KeyName].ToString())
                         .ToList();
                var ThreadStardBulding = bulkDataPreperation.NumberRecoredTake;
                DataTable ThreadDataTable = bulkDataPreperation.dataTable.Clone();
                foreach (DataRow drtableOld in bulkDataPreperation.dataTable.Rows)
                {
                    ThreadDataTable.ImportRow(drtableOld);

                }
                bulkDataPreperation.dataTable.Rows.Clear();
                bulkDataPreperation.NumberRecoredTake = 0;

            //if(bulkDataPreperation.cargoTable.Main_CT_TableName == "CargoTrackingShipments" && cargoTrackingDataBaseArgs.CargoTrackingArguments != null)
            //{
            //  WorkLimiter.WaitOne();
            //  ThreadPool.QueueUserWorkItem(o => BuildThreadPool(bulkDataPreperation, cargoTrackingDataBaseArgs, ThreadDataTable, columns));
            //}
            //else
            //{
              BuildThreadPool(bulkDataPreperation, cargoTrackingDataBaseArgs, ThreadDataTable, columns);
            //}
            

        }
 
        private void BuildThreadPool (BulkDataPreperation bulkDataPreperation, CargoTrackingUpdateDataBaseArgs cargoTrackingDataBaseArgs, DataTable ThreadDataTable, List<string> columns, bool IsCT2 = false)
        {
            try
            {
                //ThreadsNumber += 1;
                Interlocked.Increment(ref ThreadsNumber);
                bulkDataPreperation = UpdateBulkPreperations(bulkDataPreperation, cargoTrackingDataBaseArgs, ThreadDataTable, columns, IsCT2);
            }
            finally
            {
                //if (bulkDataPreperation.cargoTable.Main_CT_TableName == "CargoTrackingShipments" && cargoTrackingDataBaseArgs.CargoTrackingArguments != null)
                //    WorkLimiter.Release();

                    Interlocked.Increment(ref ThreadsCompleatedWork);
                //ThreadsCompleatedWork += 1;
            }
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
                cargoTable = cargoTrackingDataBaseArgs.buildCargoArgs.Table,
                MaxRecoredTakeEachTime = cargoTrackingDataBaseArgs.NumberOfBulkPerTime,
                dataTable = null,
                automaticLastUpdateDate = AutomaticDate,
                sqlDataReader = null,
                NumberOfCoulmnsUpdated = 0,
                NumberOfCoulmnsUpdated2 =0,
                NumberRecoredTake = 0,
            };

            return bulkDataPreperation;


        }

        private BulkDataPreperation UpdateBulkPreperations(BulkDataPreperation bulkDataPreperation, CargoTrackingUpdateDataBaseArgs cargoTrackingDataBaseArgs, DataTable dataTable ,List<string> columns ,bool IsCT2=false)
        {
       
            if (!IsCT2)
               bulkDataPreperation.NumberOfCoulmnsUpdated += columns.Count;
            else
               bulkDataPreperation.NumberOfCoulmnsUpdated2 += columns.Count;

            bulkDataPreperation.cargoTable = PrepareTableParameters(cargoTrackingDataBaseArgs, bulkDataPreperation.cargoTable, columns, IsCT2);
 
            bulkDataPreperation.automaticLastUpdateDate = UpdateBulkValues(cargoTrackingDataBaseArgs, dataTable, bulkDataPreperation.cargoTable, bulkDataPreperation.automaticLastUpdateDate, IsCT2);

          

            return bulkDataPreperation;
        }
        public void Rename_Pre_Tables(CargoArgs buildCargoArgs , string Condition)
        {
         
                BuildAllIndexesWithConstraient(buildCargoArgs);

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
            string cmd = ShipmentTableStrucrue.ChaneNameScript(buildCargoArgs.Table.Main_CT_TableName, buildCargoArgs.Table.Main_CT_TableName + "_SW") + "\n";
            cmd += ShipmentTableStrucrue.ChaneNameScript(buildCargoArgs.Table.Pre_TableName, buildCargoArgs.Table.Main_CT_TableName) + "\n";
            cmd += ShipmentTableStrucrue.ChaneNameScript(buildCargoArgs.Table.Main_CT_TableName + "_SW", buildCargoArgs.Table.Pre_TableName) + "\n";
            ServiceHelper.ExecuteSql(cmd, buildCargoArgs.DestinationConnectionString);
            if (buildCargoArgs.Table.Main_CT2_TableName != null)
            {
                cmd  = ShipmentSearcheTableStrucrue.ChaneNameScript(buildCargoArgs.Table.Main_CT2_TableName, buildCargoArgs.Table.Main_CT2_TableName + "_SW") + "\n";
                cmd += ShipmentSearcheTableStrucrue.ChaneNameScript(buildCargoArgs.Table.Pre2_TableName, buildCargoArgs.Table.Main_CT2_TableName) + "\n";
                cmd += ShipmentSearcheTableStrucrue.ChaneNameScript(buildCargoArgs.Table.Main_CT2_TableName + "_SW", buildCargoArgs.Table.Pre2_TableName) + "\n";
                ServiceHelper.ExecuteSql(cmd, buildCargoArgs.DestinationConnectionString);
            }
        }
    
        private SqlDataReader GetSqlDataReader(CargoTrackingUpdateDataBaseArgs cargoTrackingDataBaseArgs, CargoTable table, SqlConnection sourceConnection , string Condition=null)
        {
            string cmd="";
            string condition;
            string fieldName = !string.IsNullOrEmpty(cargoTrackingDataBaseArgs.buildCargoArgs.Table.FieldsDBName) ? cargoTrackingDataBaseArgs.buildCargoArgs.Table.FieldsDBName : "*";
           
            if (cargoTrackingDataBaseArgs.buildCargoArgs.Table.Main_CT_TableName == "CargoTrackingShipments" && cargoTrackingDataBaseArgs.buildCargoArgs.Table.CurrentCondition==1)
            {
                cmd = ShipmentTableCondtions.GetFirstConditions(fieldName, cargoTrackingDataBaseArgs, table,LastUpdate);
            }
            else if (cargoTrackingDataBaseArgs.buildCargoArgs.Table.Main_CT_TableName == "CargoTrackingShipments" && cargoTrackingDataBaseArgs.buildCargoArgs.Table.CurrentCondition == 2)
            {
                cmd= ShipmentTableCondtions.GetSecoundConditions(fieldName, cargoTrackingDataBaseArgs, table, LastUpdate);
            }

            else
            {
                  condition = GetUpdateDataBaseCondition(cargoTrackingDataBaseArgs.buildCargoArgs, cargoTrackingDataBaseArgs.CargoTrackingArguments, Condition);
                  cmd = "SELECT " + fieldName + " " +
                          "FROM dbo." + table.DBTableName + condition;
            }


            SqlCommand commandSourceData = new SqlCommand(cmd, sourceConnection);
            commandSourceData.CommandTimeout = (int)ServiceHelper.timeOut;
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

        private DataTable FillDataTableValues(List<DataColumn> listCols, BulkDataPreperation bulkDataPreperation, string TableName)
        {
            bool IsRoWValid = CargoTrackingValidateRecordsService.ValidateRecords(TableName, bulkDataPreperation.sqlDataReader);
            if (IsRoWValid)
            {
                DataRow dataRow = bulkDataPreperation.dataTable.NewRow();
                for (int i = 0; i < listCols.Count; i++)
                {
                    dataRow[((DataColumn)listCols[i])] = bulkDataPreperation.sqlDataReader[i];
                }
               
                  
                CargoTrackingSearchService.SearchService(dataRow, bulkDataPreperation, TableName);
                bulkDataPreperation.dataTable.Rows.Add(dataRow);
            }
            else
            {
                DataRow dataRow = bulkDataPreperation.dataTable.NewRow();
                for (int i = 0; i < listCols.Count; i++)
                {
                    dataRow[((DataColumn)listCols[i])] = bulkDataPreperation.sqlDataReader[i];
                }
                KeysForRecoredsNotValidated.Add((string)dataRow[bulkDataPreperation.cargoTable.KeyName]);
            }


            return bulkDataPreperation.dataTable;
        }

        private CargoTable PrepareTableParameters(CargoTrackingUpdateDataBaseArgs cargoTrackingDataBaseArgs, CargoTable table, List<string> columns,bool IsCT2=false)
        {
            table.UpdatedCount = columns != null ? columns.Count() : 0;

            if (IsCT2)
            {
                CargoDeleteRowsArgs DeleteRowsArgs = new CargoDeleteRowsArgs() { TableName = table.CT2_TableName, KeyName = table.ConditionKey2, IdsList = columns, ConnectionString = cargoTrackingDataBaseArgs.buildCargoArgs.DestinationConnectionString, ReturnDeleteIdsAsString = true };

                table.RefreshIds2 = columns.ToString();
                if (cargoTrackingDataBaseArgs.CargoTrackingArguments == null)
                {
                    table.RefreshIds2 = DeleteRowsFromCargoTables(DeleteRowsArgs);
                }

            }
            else
            {
                CargoDeleteRowsArgs DeleteRowsArgs = new CargoDeleteRowsArgs() { TableName = table.CT_TableName, KeyName = table.ConditionKey, IdsList = columns, ConnectionString = cargoTrackingDataBaseArgs.buildCargoArgs.DestinationConnectionString, ReturnDeleteIdsAsString = true };
                table.RefreshIds = columns.ToString();
                if (cargoTrackingDataBaseArgs.CargoTrackingArguments == null)
                {
                    table.RefreshIds2 = DeleteRowsFromCargoTables(DeleteRowsArgs);
                }

            }
            return table;
        }

      

        private DateTime? UpdateBulkValues(CargoTrackingUpdateDataBaseArgs cargoTrackingDataBaseArgs, DataTable dataTable, CargoTable table, DateTime? automaticLastUpdateDate ,bool IsCT2=false)
        {
            if (!string.IsNullOrEmpty(table.RefreshIds) || !string.IsNullOrEmpty(table.RefreshIds2))
            {

                using (SqlConnection destinationConnection =
                                    new SqlConnection(cargoTrackingDataBaseArgs.buildCargoArgs.DestinationConnectionString))
                {
                    destinationConnection.Open();

                    using (SqlBulkCopy bulkCopy =
                               new SqlBulkCopy(cargoTrackingDataBaseArgs.buildCargoArgs.DestinationConnectionString, SqlBulkCopyOptions.KeepIdentity))
                    {
                     
                      

                        try
                        {
                            AutoMapColumns(bulkCopy, dataTable, table, IsCT2);


                            foreach (DataRow dr in dataTable.Rows)
                            {
                                if (!IsCT2)
                                {
                                    CargoTrackingTableLogicService.SetTableLogic(dr, cargoTrackingDataBaseArgs.buildCargoArgs.Table.Main_CT_TableName, cargoTrackingDataBaseArgs.buildCargoArgs.Table.CurrentCondition);
                                }
                                else
                                {
                                    CargoTrackingTableLogicService.SetTableLogic(dr, cargoTrackingDataBaseArgs.buildCargoArgs.Table.Main_CT2_TableName, cargoTrackingDataBaseArgs.buildCargoArgs.Table.CurrentCondition);
                                }
                                

                            }
                          

                            lock (threadLock)
                            {
                                if (IsCT2)
                                    bulkCopy.DestinationTableName = "dbo." + table.CT2_TableName;
                                else
                                    bulkCopy.DestinationTableName = "dbo." + table.CT_TableName;

                                bulkCopy.BulkCopyTimeout = (int)ServiceHelper.timeOut;
                                bulkCopy.EnableStreaming = true;
                                bulkCopy.BatchSize = 100000;
                                bulkCopy.WriteToServer(dataTable);
                                bulkCopy.Close();

                            }


                        }

                        finally
                        {
                            if (cargoTrackingDataBaseArgs.CargoTrackingArguments == null)
                            {
                                automaticLastUpdateDate = ServiceHelper.GetAutomaticLastUpdateDate(automaticLastUpdateDate, dataTable, table.IsClosedTable);

                        }
                            else
                        {
                            automaticLastUpdateDate = null;
                        }
                            destinationConnection.Close();
                    }
                    }

                }

            }
            return automaticLastUpdateDate;
        }

    

        public void AutoMapColumns(SqlBulkCopy sbc, DataTable dt, CargoTable Table , bool IsCT2=false)
        {
            List<string> MappingMatching = new List<string>();
            List<string> MappingDeferCT2_FieldsDBNameence = new List<string>();

            string[] DB_Cols = Table.FieldsDBName.Split(',');
            string[] CTDB_Cols = Table.CT_FieldsDBName.Split(',');
            if (IsCT2)
            {
                   CTDB_Cols = Table.CT2_FieldsDBName.Split(',');

            }
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
                    if (IsCT2)
                    {
                        CargoTrackingCustomMappingService.MappingDB_CTDB(dt, sbc, DB_columns, Table.Main_CT2_TableName);
                    }
                    else
                    {
                        CargoTrackingCustomMappingService.MappingDB_CTDB(dt, sbc, DB_columns, Table.Main_CT_TableName);
                    }
                    
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
 

        private string GetUpdateDataBaseCondition(CargoArgs buildCargoArgs, CargoTrackingArguments CargoTrackingArguments = null, string Condition = null)
        {
            //string LastUpdate = null;
            if (CargoTrackingArguments == null)
            {
                LastUpdate = ServiceHelper.GetTableLastUpdate(buildCargoArgs.Table.CT_TableName, buildCargoArgs.DestinationConnectionString);
            }
            BuildWhereConditionArgs buildWhereConditionArgs = new BuildWhereConditionArgs()
            {
                TableName = buildCargoArgs.Table.Main_CT_TableName,
                LastUpdate = LastUpdate,
                CargoTrackingArguments = CargoTrackingArguments,
                Condition = Condition,
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
                    string cmd = "delete from " + deleteRowsArgs.TableName + " where " + deleteRowsArgs.KeyName + " in " + ("(" + deletedRows.ToString() + ")").Replace(",)", ")");
                    ServiceHelper.ExecuteSql(cmd, deleteRowsArgs.ConnectionString);
                    rowsCount = 0;
                    deletedRows.Clear();
                }
            }

            return !string.IsNullOrEmpty(allDeletedRows.ToString()) ? ("(" + allDeletedRows.ToString() + ")").Replace(",)", ")") : null;

        }

    

    public RecordUpdated TableCheckConditions(CargoTrackingUpdateDataBaseArgs cargoTrackingDataBaseArgs, RecordUpdated _recordUpdated)
        {
            int NumberRecordUpdated = 0;
            int NumberRecordUpdated2 = 0;

            if (!_recordUpdated.IsFromBuild || cargoTrackingDataBaseArgs.CargoTrackingArguments != null)
            {

                bool IsUpadteWaterMark = false;
                if (cargoTrackingDataBaseArgs.buildCargoArgs.Table.Condition1 == null)
                {
                    cargoTrackingDataBaseArgs.buildCargoArgs.Table.CurrentCondition = 0;
                    IsUpadteWaterMark = true;
                    _recordUpdated = UpdateCargoTrackingService(cargoTrackingDataBaseArgs, null, IsUpadteWaterMark);
                    NumberRecordUpdated += _recordUpdated.NumberOfRecordUpdated;
                    NumberRecordUpdated2 += _recordUpdated.NumberOfRecordUpdated2;
                }
                if (cargoTrackingDataBaseArgs.buildCargoArgs.Table.Condition1 != null)
                {
                    cargoTrackingDataBaseArgs.buildCargoArgs.Table.CurrentCondition = 1;
                    if (cargoTrackingDataBaseArgs.buildCargoArgs.Table.Condition2 == null)
                    {
                        IsUpadteWaterMark = true;
                    }
                    _recordUpdated = UpdateCargoTrackingService(cargoTrackingDataBaseArgs, cargoTrackingDataBaseArgs.buildCargoArgs.Table.Condition1, IsUpadteWaterMark);
                    NumberRecordUpdated += _recordUpdated.NumberOfRecordUpdated;
                    NumberRecordUpdated2 += _recordUpdated.NumberOfRecordUpdated2;
                }
                if (cargoTrackingDataBaseArgs.buildCargoArgs.Table.Condition2 != null)
                {
                    cargoTrackingDataBaseArgs.buildCargoArgs.Table.CurrentCondition = 2;
                    if (cargoTrackingDataBaseArgs.buildCargoArgs.Table.Condition3 == null)
                    {
                        IsUpadteWaterMark = true;
                    }
                    _recordUpdated = UpdateCargoTrackingService(cargoTrackingDataBaseArgs, cargoTrackingDataBaseArgs.buildCargoArgs.Table.Condition2, IsUpadteWaterMark);
                    NumberRecordUpdated += _recordUpdated.NumberOfRecordUpdated;
                    NumberRecordUpdated2 += _recordUpdated.NumberOfRecordUpdated2;

                }
                if (cargoTrackingDataBaseArgs.buildCargoArgs.Table.Condition3 != null)
                {
                    cargoTrackingDataBaseArgs.buildCargoArgs.Table.CurrentCondition = 3;
                    IsUpadteWaterMark = true;
                    _recordUpdated = UpdateCargoTrackingService(cargoTrackingDataBaseArgs, cargoTrackingDataBaseArgs.buildCargoArgs.Table.Condition3, IsUpadteWaterMark);
                    NumberRecordUpdated += _recordUpdated.NumberOfRecordUpdated;
                    NumberRecordUpdated2 += _recordUpdated.NumberOfRecordUpdated2;
                }
            }
            _recordUpdated.NumberOfRecordUpdated = NumberRecordUpdated;
            _recordUpdated.NumberOfRecordUpdated2 = NumberRecordUpdated2;
            return _recordUpdated;
        }





    }

 
  
 

}
