
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
    public class CargoTrackingMainService
    {
        public static long timeOut = 10000000000000000;

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
                ConditionsNumber = 1,
            });

            CargoTableLists.Add(new CargoTable()
            {
                TableName = "Shipment",
                FieldsDBName = "Id,Tenant,CustomerId,TransportModeId,MasterShipmentDataId,House,ShipmentNumber,FromPortId,ToPortId,ShipperId,ConsigneeId,GrossWeight,Volume,CustomConnectToShipment,AutomaticLastUpdateDate,ShipmentPickUpIndex,FirstPickupETA,ShipmentLevelCode,CustomsClearanceDate,CustomFileId,CreateDateTime",
                KeyName = "Id",
                ConditionKey = "EntityId",
                DBTableName = "Shipments",
                CT_TableName = "CargoTrackingShipments",
                CT_FieldsDBName = "Tenant,CustomerId,TransportModeId,Master,House,ShipmentNumber,FromPortId,ToPortId,ShipperId,ConsigneeId,GrossWeight,Volume,PickupDone,PickupDate,EntityId,EntityType,ForwardingShipmentHeaderId,CustomsShipmentHeaderId,CurrentMilestoneCode,CurrentMilestoneDate,ClearanceDone,ClearanceDate",
                Condition1 = " where ((AutomaticLastUpdateDate > ( select LastUpdateDate from WaterMarks where TableName =  'CargoTrackingShipments')) and ((ShipmentLevelCode ='D' or ShipmentLevelCode ='H') and CustomFileId is not null))",
                Condition2 = " where ((AutomaticLastUpdateDate > ( select LastUpdateDate from WaterMarks where TableName =  'CargoTrackingShipments')) and (((ShipmentLevelCode !='D' and ShipmentLevelCode !='H') or CustomFileId is null)))",
                ConditionsNumber = 2,
            });


            CargoTableLists.Add(new CargoTable()
            {
                TableName = "Shipments",
                FieldsDBName = "Id,Tenant,ShipmentNumber,SearchFields,CreateDateTime,CustomerReference1,CustomerReference2,AutomaticLastUpdateDate",
                KeyName = "Id",
                ConditionKey = "ShipmentId",
                CT_FieldsDBName = "Tenant,ShipmentId,SearchFields,ShipmentDate",
                DBTableName = "Shipments",
                CT_TableName = "CargoTrackingShipmentSearches",
                ConditionsNumber = 1,

            });
        
         

            return CargoTableLists;

        }


        public int UpdateCTDataBase(CargoArgs buildCargoArgs, int NumberOfBulkPerTime, bool? IsUpdateAfterFinished = null, CargoTrackingArguments CargoTrackingArguments = null)
        {
            int NumberRecordUpdated = 0;
            bool IsUpadteWaterMark = false;
            if (buildCargoArgs.Table.Condition1 == null)
            {
                IsUpadteWaterMark = true;
                NumberRecordUpdated += UpdateCTService(buildCargoArgs, NumberOfBulkPerTime, IsUpdateAfterFinished, CargoTrackingArguments,null, IsUpadteWaterMark);
            }
            if (buildCargoArgs.Table.Condition1!=null)
            {
                if (buildCargoArgs.Table.Condition2 == null)
                {
                    IsUpadteWaterMark = true;
                }
                NumberRecordUpdated += UpdateCTService(buildCargoArgs, NumberOfBulkPerTime, IsUpdateAfterFinished, CargoTrackingArguments, buildCargoArgs.Table.Condition1, IsUpadteWaterMark);
            }
            if (buildCargoArgs.Table.Condition2 != null)
            {
                if (buildCargoArgs.Table.Condition3 == null)
                {
                    IsUpadteWaterMark = true;
                }
                NumberRecordUpdated += UpdateCTService(buildCargoArgs, NumberOfBulkPerTime, IsUpdateAfterFinished, CargoTrackingArguments, buildCargoArgs.Table.Condition2, IsUpadteWaterMark);
            }
            if (buildCargoArgs.Table.Condition3 != null)
            {
                IsUpadteWaterMark = true;
                NumberRecordUpdated += UpdateCTService(buildCargoArgs, NumberOfBulkPerTime, IsUpdateAfterFinished, CargoTrackingArguments, buildCargoArgs.Table.Condition3, IsUpadteWaterMark);
            }
            return NumberRecordUpdated;
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
                            if (column.ColumnName!="IDE")
                            {
                                dataTable.Columns.Add(column);

                            }
                        }
                    }

                    while (true)
                    {

                        while (reader.Read())
                        {

                            dataTable = FillDataTableValues(listCols, reader, dataTable, buildCargoArgs.Table.CT_TableName);

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

                        automaticLastUpdateDate = UpdateBulkValues(buildCargoArgs, dataTable, table, automaticLastUpdateDate, IsUpdateAfterFinished,   CargoTrackingArguments);

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
                //if (IsUpdateAfterFinished == null)
                //{
                //    UpdateCTDataBase(buildCargoArgs, NumberOfBulkPerTime, true,CargoTrackingArguments);
                //}
                if (IsUpadteWaterMark)
                {
                    UpdateWaterMarkAfterFinishCheck(table, automaticLastUpdateDate, buildCargoArgs);

                }
            }




            return NumberOfCoulmnsUpdated;

        }

        private SqlDataReader GetSqlDataReader(CargoArgs buildCargoArgs, CargoTable table, SqlConnection sourceConnection, CargoTrackingArguments CargoTrackingArguments = null, string Condition=null)
        {
            string fieldName = !string.IsNullOrEmpty(buildCargoArgs.Table.FieldsDBName) ? buildCargoArgs.Table.FieldsDBName : "*";
            string condition = GetUpdateDataBaseCondition(buildCargoArgs, CargoTrackingArguments, Condition);

            SqlCommand commandSourceData = new SqlCommand(
                           "SELECT " + fieldName + " " +
                           "FROM dbo." + table.DBTableName + condition, sourceConnection);
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
                                CargoTrackingTableLogicService.SetTableLogic(dr, buildCargoArgs.Table.CT_TableName);
                                //if (IsUpdateAfterFinished == null || IsUpdateAfterFinished == false)
                                //{
                                //    CargoTrackingTableLogicAfterUpdating.SetTableLogic(dr, buildCargoArgs.Table.CT_TableName);
                                //}
                            }

                            //if (IsUpdateAfterFinished == true)
                            //{

                            //    foreach (DataRow dr in dataTable.Rows)
                            //    {
                            //        CargoTrackingUpdatedAfterFinishMapping.UpdatedAfterFinishMapping(dr, buildCargoArgs.Table.CT_TableName);
                            //    }
                            //}



                            bulkCopy.EnableStreaming = true;
                            bulkCopy.BatchSize = 100000;
                            bulkCopy.WriteToServer(dataTable);

                        }

                        finally
                        {
                            if (  CargoTrackingArguments ==null )
                            {
                                automaticLastUpdateDate = GetAutomaticLastUpdateDate(automaticLastUpdateDate, dataTable);

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

        private DateTime? GetAutomaticLastUpdateDate(DateTime? automaticLastUpdateDate, DataTable dataTable)
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

            return automaticLastUpdateDate;
        }

        private void UpdateWaterMarkAfterFinishCheck(CargoTable table, DateTime? automaticLastUpdateDate, CargoArgs buildCargoArgs)
        {
            if (table != null && table.DBTableName != "WaterMarks")
            {
               
                var lastUpdateDate = string.Empty;
                if (automaticLastUpdateDate != null) lastUpdateDate = automaticLastUpdateDate.Value.ToString("MM/dd/yyyy hh:mm:ss.fff tt");
                else lastUpdateDate = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt");
                UpdateWaterMarksTable(table, lastUpdateDate, buildCargoArgs.SourceConnectionString);
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
                    CargoTrackingCustomMappingService.MappingDB_CTDB(dt, sbc, DB_columns, Table.CT_TableName);
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

        public void CheckAndUpdateWaterMark(string dbSourceConnection)
        {

            List<CargoTable> CargoTableLists = FillCargoTableList();
            foreach (CargoTable table in CargoTableLists)
            {
                if (table.DBTableName != "WaterMarks")
                {
                    using (SqlConnection SourceConnection =
                         new SqlConnection(dbSourceConnection))
                    {
                        SourceConnection.Open();

                        SqlCommand commandSourceData = new SqlCommand(
                       "SELECT  TableName" +
                       " FROM dbo.WaterMarks WHERE TableName = '" + table.CT_TableName + "'", SourceConnection);

                        SqlDataReader reader = commandSourceData.ExecuteReader();
                        if (!reader.HasRows)
                        {
                            string lastUpdateDate = GetAutomaticLastUpdateDate(table.DBTableName, dbSourceConnection);
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
            string cmd = "update  WaterMarks set LastUpdateDate = '" + date + "' where tableName = '" + table.CT_TableName + "'";
            ExecuteSql(cmd, connectionString);

        }

        public void AddWaterMarksRecord(CargoTable table, string date, string connectionString)
        {
            string cmd = "insert into WaterMarks  values('" + table.CT_TableName + "' , '" + date + "')";
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
                    string cmd = "delete " + deleteRowsArgs.TableName + " where " + deleteRowsArgs.KeyName + " in " + ("(" + deletedRows.ToString() + ")").Replace(",)", ")");
                    ExecuteSql(cmd, deleteRowsArgs.ConnectionString);
                    rowsCount = 0;
                    deletedRows.Clear();
                }
            }

            return !string.IsNullOrEmpty(allDeletedRows.ToString()) ? ("(" + allDeletedRows.ToString() + ")").Replace(",)", ")") : null;

        }

        public string BuildConnectionString(string catalog, string userName, string password, string server)
        {
            string result = "Data Source=" + server + ";Initial Catalog=" + catalog + ";Integrated Security=False;Persist Security Info=True;User ID=" + userName + ";Password= " + password + ";MultipleActiveResultSets=True;Connect Timeout=60";
            return result;
        }

        private string GetUpdateDataBaseCondition(CargoArgs buildCargoArgs, CargoTrackingArguments CargoTrackingArguments = null, string Condition=null)
        {
            string condition = CargoTrackingTableCondition.Condition(buildCargoArgs.Table.CT_TableName, CargoTrackingArguments, Condition);
            //string condition = " where AutomaticLastUpdateDate > ( select LastUpdateDate from WaterMarks where TableName = " + "'" + buildCargoArgs.Table.CT_TableName + "')";
            return condition;
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
    }

    public class CargoTrackingArguments
    {
        public DateTime? FromDate;
        public DateTime? ToDate;
        public int? Tenant;

    }

}
