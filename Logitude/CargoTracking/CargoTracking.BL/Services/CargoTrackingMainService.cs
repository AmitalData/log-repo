using CargoTracking.CargoTracking.BL.HelperClasses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CargoTracking.CargoTracking.BL.Services
{
    public class CargoTrackingMainService
    {
        public static long timeOut = 10000000000000000;
     

        public int UpdateCTDataBase(CargoArgs buildCargoArgs)
        {
            CargoTable table = buildCargoArgs.Table;
            string fieldName = !string.IsNullOrEmpty(buildCargoArgs.Table.FieldsDBName) ? buildCargoArgs.Table.FieldsDBName : "*";
            string condition = GetUpdateDataBaseCondition(buildCargoArgs);
            DataTable dataTable = null;
            DateTime? automaticLastUpdateDate = null;
            int NumberOfCoulmnsUpdated = 0;
            using (SqlConnection sourceConnection =
                       new SqlConnection(buildCargoArgs.SourceConnectionString))
            {
                sourceConnection.Open();
                bool CompleatedUpdate = false;
                int TakeNumber = 1000;
                int SkipNumber = 0;
                while (!CompleatedUpdate)
                {


                    SqlCommand commandSourceData = new SqlCommand(
                   "SELECT TOP " + TakeNumber + " " + fieldName +
                   " FROM ( SELECT *, ROW_NUMBER() OVER (ORDER BY Id) AS ROW_NUM  FROM dbo." + table.DBTableName + condition + ") x WHERE ROW_NUM>" + SkipNumber + "", sourceConnection);


                    SqlDataReader reader = commandSourceData.ExecuteReader();
                 
                    if (reader.HasRows)
                    {
                        dataTable = new DataTable();
                        dataTable.Load(reader);

                        var columns = dataTable.Rows
                                         .Cast<DataRow>()
                                         .Select(r => (string)r[table.KeyName].ToString())
                                         .ToList();

                        
                    

                        if (columns.Count < 1000)
                        {
                            CompleatedUpdate = true;
                        }
                        SkipNumber += TakeNumber;
                        NumberOfCoulmnsUpdated += columns.Count;


                        table.UpdatedCount = columns != null ? columns.Count() : 0;
                        table.RefreshIds = DeleteRowsFromCargoTables(new CargoDeleteRowsArgs() { TableName = table.CT_TableName, KeyName = table.KeyName, IdsList = columns, ConnectionString = buildCargoArgs.DestinationConnectionString, ReturnDeleteIdsAsString = true });

                        if (!string.IsNullOrEmpty(table.RefreshIds))
                        {

                            using (SqlConnection destinationConnection =
                                       new SqlConnection(buildCargoArgs.DestinationConnectionString))
                            {
                                destinationConnection.Open();

                                using (SqlBulkCopy bulkCopy =
                                           new SqlBulkCopy(destinationConnection))
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
                                        }

                                        bulkCopy.EnableStreaming = true;
                                        bulkCopy.BatchSize = 100000;
                                        bulkCopy.WriteToServer(dataTable);

                                    }

                                    finally
                                    {
                                        reader.Close();

                                        var MaxUpdate = (DateTime)dataTable.Rows
                                       .Cast<DataRow>()
                                       .Max(d => d["AutomaticLastUpdateDate"]);

                                        if (MaxUpdate > automaticLastUpdateDate || automaticLastUpdateDate==null)
                                        {
                                            automaticLastUpdateDate = (DateTime)dataTable.Rows
                                           .Cast<DataRow>()
                                           .Max(d => d["AutomaticLastUpdateDate"]);
                                        }
                                       

                                    }
                                }

                            }


                        }




                    }

                    else
                    {
                        CompleatedUpdate = true;
                        reader.Close();
                    }
                }
                if (table!=null  && dataTable  !=null&& table.DBTableName != "WaterMarks")
                {
                   
                    var lastUpdateDate = string.Empty;
                    if (automaticLastUpdateDate != null) lastUpdateDate = automaticLastUpdateDate.Value.ToString("MM/dd/yyyy hh:mm:ss.fff tt");
                    else lastUpdateDate = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt");
                    UpdateWaterMarksTable(table, lastUpdateDate, buildCargoArgs.SourceConnectionString);
                    table.IsUpdated = true;

                }
            }

            return NumberOfCoulmnsUpdated;

        }


        public int UpdateLineByLine(CargoArgs buildCargoArgs)
        {
            CargoTable table = buildCargoArgs.Table;
            string fieldName = !string.IsNullOrEmpty(buildCargoArgs.Table.FieldsDBName) ? buildCargoArgs.Table.FieldsDBName : "*";
            string condition = GetUpdateDataBaseCondition(buildCargoArgs);
            DateTime? automaticLastUpdateDate = null;
            int NumberOfCoulmnsUpdated = 0;
            using (SqlConnection sourceConnection =
                       new SqlConnection(buildCargoArgs.SourceConnectionString))
            {
                sourceConnection.Open();
        
              

                    SqlCommand commandSourceData = new SqlCommand(
                   "SELECT "+ fieldName +" "+
                   "FROM dbo." + table.DBTableName + condition , sourceConnection);



                    SqlDataReader reader = commandSourceData.ExecuteReader();
                    List<string> RecordsIds = new List<string>();
                    var RowWithCoulmn = new Dictionary<string, object>();
               
                while (reader.Read())
                    {
                        object[] tempRow = new object[reader.FieldCount];

                        for (int i = 0; i < reader.FieldCount; i++)
                        {

                        string g = reader.GetName(i);

                        if (reader.GetName(i) == table.KeyName /*&& reader.GetValue(i).Equals("1-1")*/) {
                              RecordsIds.Add((string)reader.GetValue(i));
                        }
                        if (reader.GetName(i) == "AutomaticLastUpdateDate")
                        {
                         
                             DateTime? MaxUpdate = null;
                             var Auto = reader.GetValue(i).GetType();
                            if (Auto.FullName == "System.DBNull")
                            {
                                MaxUpdate = null;
                            }
                            else
                            {
                                MaxUpdate = (DateTime?)reader.GetValue(i);


                                if ( automaticLastUpdateDate == null || MaxUpdate > automaticLastUpdateDate )
                                {
                                    automaticLastUpdateDate = (DateTime?)MaxUpdate;
                                }
 
                            }

                        }
                        RowWithCoulmn.Add(reader.GetName(i), reader[i]);
                           tempRow[i] = reader[i];
                       }

                    table.RefreshIds = DeleteRowsFromCargoTables(new CargoDeleteRowsArgs() { TableName = table.CT_TableName, KeyName = table.KeyName, IdsList = RecordsIds, ConnectionString = buildCargoArgs.DestinationConnectionString, ReturnDeleteIdsAsString = true });
                    MappingAndUpdateLineByLine(buildCargoArgs, RowWithCoulmn);
                    RowWithCoulmn.Clear();
                    RecordsIds.Clear();
                    NumberOfCoulmnsUpdated++;

                }

                reader.Close();

                if (table != null && table.DBTableName != "WaterMarks")
                {

                    var lastUpdateDate = string.Empty;
                    if (automaticLastUpdateDate != null) lastUpdateDate = automaticLastUpdateDate.Value.ToString("MM/dd/yyyy hh:mm:ss.fff tt");
                    else lastUpdateDate = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt");
                    UpdateWaterMarksTable(table, lastUpdateDate, buildCargoArgs.SourceConnectionString);
                    table.IsUpdated = true;

                }


            }

            return NumberOfCoulmnsUpdated;

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
                        MappingMatching.Add(DB_columns + ","+ CTDB_columns);
                        CompareDB += CTDB_columns + ",";
                    }
                }
              
            }
            foreach (string DB_columns in CTDB_Cols)
            {
                if (!CompareDB.Contains(DB_columns))
                {
                    CargoTrackingCustomMappingService.MappingDB_CTDB(dt, sbc, DB_columns);
                }
            }

            foreach (string columns in  MappingMatching)
            {
                string [] Cols = columns.Split(',');
                string col1 = Cols[0];
                string col2 = Cols[1];
                sbc.ColumnMappings.Add(col1, col2);
            }
        }
        private void MappingAndUpdateLineByLine(CargoArgs buildCargoArgs, Dictionary<string, object> Record)
        {
            List<string> CoulmnName = Record.Keys.ToList();
            List<object> CoumnValues = Record.Values.ToList();

            List<string> Final_CoulmnNames = new List<string>();
            List<string> Final_CoulmnNames_Par = new List<string>();
            List<object> Final_CoulmnValues = new List<object>();
            List<string> Full_CoulmnNames = new List<string>();
            List<object> Full_CoulmnValues = new List<object>();

            string[] CTDB_Cols = buildCargoArgs.Table.CT_FieldsDBName.Split(',');
            for (int i =0; i < CoulmnName.Count(); i++)
            {
                if (CTDB_Cols.Contains(CoulmnName[i]))
                {
                    Final_CoulmnNames.Add(CoulmnName[i]);
                    Final_CoulmnNames_Par.Add("@"+CoulmnName[i]);
                    Final_CoulmnValues.Add( CoumnValues[i] );
                }
                Full_CoulmnNames.Add(CoulmnName[i]);
                Full_CoulmnValues.Add(CoumnValues[i]);
            }
            foreach (string CTDB_columns in CTDB_Cols)
            {

                if (!Final_CoulmnNames.Contains(CTDB_columns))
                {
                    Final_CoulmnNames.Add(CTDB_columns);
                    Final_CoulmnNames_Par.Add("@" + CTDB_columns);
                    Final_CoulmnValues.Add(null);
                    Full_CoulmnNames.Add(CTDB_columns);
                    Full_CoulmnValues.Add(null);
                }
                
            }

            CargoTrackingLineByLineMappingService.CoulmnsName = Final_CoulmnNames;
            CargoTrackingLineByLineMappingService.RecordValues = Final_CoulmnValues;
            CargoTrackingLineByLineMappingService.Full_CoulmnNames = Full_CoulmnNames;
            CargoTrackingLineByLineMappingService.Full_CoulmnValues = Full_CoulmnValues;
            CargoTrackingLineByLineMappingService.TableName = buildCargoArgs.Table.CT_TableName;

            Final_CoulmnValues = CargoTrackingLineByLineMappingService.MappingDB_CTDB();

            string Excecute_CoulmnNames = String.Join(",", Final_CoulmnNames);
            string Excecute_CoulmnNames_Par = String.Join(",", Final_CoulmnNames_Par);
            object Excecute_CoumnValues = String.Join(",", Final_CoulmnValues);

            AddValuesToCargoTracking(Excecute_CoulmnNames, Excecute_CoulmnNames_Par, Final_CoulmnValues, buildCargoArgs.Table.CT_TableName, buildCargoArgs.DestinationConnectionString);
          
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
                            string lastUpdateDate =  GetAutomaticLastUpdateDate(table.DBTableName, dbSourceConnection);
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
        public List<CargoTable> FillCargoTableList()
        {
            List<CargoTable> CargoTableLists = new List<CargoTable>();
            CargoTableLists.Add(new CargoTable() { TableName = "Port", FieldsDBName = "Id,Code,CountryId,EnglishName,AutomaticLastUpdateDate", CT_FieldsDBName = "Id,Code,EnglishName,CountryId", KeyName = "Id", DBTableName = "Ports", CT_TableName = "CargoTrackingPorts" });
            CargoTableLists.Add(new CargoTable() { TableName = "Card", FieldsDBName = "Id,Code,LocalName,EnglishName,AutomaticLastUpdateDate", KeyName = "Id", CT_FieldsDBName = "Id,Code,EnglishName,LocalName", DBTableName = "Cards", CT_TableName = "CargoTrackingCards" });
            CargoTableLists.Add(new CargoTable()
            {
                TableName = "Shipment",
                FieldsDBName = "Id,Tenant,CustomerId,TransportModeId,MasterShipmentDataId,House,ShipmentNumber,FromPortId,ToPortId,ShipperId,ConsigneeId,GrossWeight,Volume,CustomConnectToShipment,AutomaticLastUpdateDate,ShipmentPickUpIndex,FirstPickupETA",
                KeyName = "Id",
                DBTableName = "Shipments",
                CT_TableName = "CargoTrackingShipments",
                CT_FieldsDBName = "Id,Tenant,CustomerId,TransportModeId,Master,House,ShipmentNumber,FromPortId,ToPortId,ShipperId,ConsigneeId,GrossWeight,Volume,PickupDone,PickupDate"
            });

            return CargoTableLists;

        }
        public void UpdateWaterMarksTable(CargoTable table, string date, string connectionString)
        {
            string cmd = "update  WaterMarks set LastUpdateDate = '" + date + "' where tableName = '" + table.CT_TableName + "'";
            ExecuteSql(cmd, connectionString);

        }
        public void AddWaterMarksRecord(CargoTable table, string date, string connectionString)
        {
            string cmd =  "insert into WaterMarks  values('" + table.CT_TableName + "' , '" + date + "')" ;
            ExecuteSql(cmd, connectionString);
        }
        private void AddValuesToCargoTracking(string Excecute_CoulmnNames, string Excecute_CoulmnNames_Par, List<object> Excecute_CoumnValues_List, string TableName ,string ConnectionString)
        {
            using (SqlConnection cn = new SqlConnection(ConnectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(@"insert into " + TableName + " (" + Excecute_CoulmnNames + ")   VALUES(" + Excecute_CoulmnNames_Par + ")", cn);
                    cmd.CommandTimeout = (int)timeOut;
                    cn.Open();

                    List<string> Excecute_CoulmnNames_Par_List = Excecute_CoulmnNames_Par.Split(',').ToList();

                    for (int i = 0; i < Excecute_CoulmnNames_Par_List.Count; i++)
                    {
                        if (Excecute_CoumnValues_List[i] == null)
                        {
                            cmd.Parameters.AddWithValue(Excecute_CoulmnNames_Par_List[i], DBNull.Value);

                        }

                        else
                        {
                            cmd.Parameters.AddWithValue(Excecute_CoulmnNames_Par_List[i], Excecute_CoumnValues_List[i]);

                        }
                    }

                    cmd.ExecuteNonQuery();

                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message+"\n"+ e.StackTrace);

                }
                cn.Close();
            }
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
        private  string GetUpdateDataBaseCondition(CargoArgs buildCargoArgs)
        {
 
            string condition = " where AutomaticLastUpdateDate > ( select LastUpdateDate from WaterMarks where TableName = " + "'" + buildCargoArgs.Table.CT_TableName + "')";
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
 
}
