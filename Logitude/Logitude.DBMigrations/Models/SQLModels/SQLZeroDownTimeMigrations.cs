using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Logitude.DBMigrations.Models
{
    public class SQLZeroDownTimeMigrations : ZeroDownTimeMigrations
    {
        protected override List<DBMigrationsSetDefaultValue> GetDBMigrationsSetDefaultValues()
        {
            string queryString = "SELECT * FROM [dbo].[DBMigrationsSetDefaultValues] WHERE [Status] <> 'Done' ORDER BY [UpdateNumber]";

            List<DBMigrationsSetDefaultValue> dbMigrationsSetDefaultValues = new List<DBMigrationsSetDefaultValue>();

            SqlDataReader reader = null;
            string connectionString = GetZeroDownTimeConnectionString(ToolConfigurations.MainConnectionString);
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(queryString, connection);

            try
            {
                connection.Open();
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    DBMigrationsSetDefaultValue dbMigrationsSetDefaultValue = new DBMigrationsSetDefaultValue
                    {
                        Id = reader["Id"].ToString(),
                        DatabaseType = reader["DatabaseType"].ToString(),
                        SchemaName = reader["SchemaName"].ToString(),
                        TableName = reader["TableName"].ToString(),
                        ColumnName = reader["ColumnName"].ToString(),
                        DefaultValue = reader["DefaultValue"].ToString(),
                        UpdateNumber = Convert.ToInt32(reader["UpdateNumber"].ToString()),
                        DoneRecordsCount = Convert.ToInt32(reader["DoneRecordsCount"].ToString())
                    };
                    dbMigrationsSetDefaultValues.Add(dbMigrationsSetDefaultValue);
                }

                reader.Close();
                connection.Close();
            }
            catch (Exception exception)
            {
                if (reader != null)
                {
                    reader.Close();
                }
                connection.Close();

                ExitTool("Error: " + exception.Message);
            }

            return dbMigrationsSetDefaultValues;
        }

        protected override void SetDefaultValueAsBatches(DBMigrationsSetDefaultValue dbMigrationsSetDefaultValue)
        {
            int affectedRows = 1;
            int updateNumber = dbMigrationsSetDefaultValue.UpdateNumber;
            int doneRecordsCount = dbMigrationsSetDefaultValue.DoneRecordsCount;
            string dbMigrationsSetDefaultValueId = dbMigrationsSetDefaultValue.Id;
            string databaseType = dbMigrationsSetDefaultValue.DatabaseType;
            string schemaName = dbMigrationsSetDefaultValue.SchemaName;
            string tableName = dbMigrationsSetDefaultValue.TableName;
            string columnName = dbMigrationsSetDefaultValue.ColumnName;
            string defaultValue = FormatDefaultValue(dbMigrationsSetDefaultValue.DefaultValue);

            string queryString = "UPDATE TOP(1000) [" + schemaName + "].[" + tableName + "] SET [" + columnName + "] = " + defaultValue + ", " +
                "[DBMigrationsLastDefaultValue] = " + updateNumber.ToString() + " WHERE [DBMigrationsLastDefaultValue] = " + (updateNumber - 1).ToString() +
                " OR [DBMigrationsLastDefaultValue] IS NULL;";

            string connectionString = GetZeroDownTimeConnectionString(ToolConfigurations.GetConnectionString(databaseType));
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            while (affectedRows > 0)
            {
                try
                {
                    sqlConnection.Open();
                    SqlCommand sqlCommand = new SqlCommand();
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = queryString;
                    sqlCommand.CommandTimeout = 3600;
                    DateTime batchStartTime = DateTime.Now;
                    affectedRows = sqlCommand.ExecuteNonQuery();
                    DateTime batchEndTime = DateTime.Now;
                    sqlConnection.Close();

                    if(affectedRows > 0)
                    {
                        doneRecordsCount += affectedRows;
                        UpdateDBMigrationsSetDefaultValue(dbMigrationsSetDefaultValueId, "DoneRecordsCount", doneRecordsCount.ToString());
                        if (affectedRows >= 1000)
                        {
                            int elapsedTime = (int)(batchEndTime - batchStartTime).TotalMilliseconds;
                            UpdateDBMigrationsSetDefaultValue(dbMigrationsSetDefaultValueId, "LastBatchElapsedTime", elapsedTime.ToString());
                        }
                    }

                    Thread.Sleep(500);
                }
                catch (Exception exception)
                {
                    sqlConnection.Close();
                    ExitTool("Error: " + exception.Message);
                }
            }
        }
        
        protected override void UpdateDBMigrationsSetDefaultValue(string dbMigrationsSetDefaultValueId, string property, string value)
        {
            string queryString = "UPDATE [dbo].[DBMigrationsSetDefaultValues] SET [" + property + "] = '" + value + "' WHERE [Id] = '" + dbMigrationsSetDefaultValueId + "';";

            string connectionString = GetZeroDownTimeConnectionString(ToolConfigurations.MainConnectionString);
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            try
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = queryString;
                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
            catch (Exception exception)
            {
                sqlConnection.Close();
                ExitTool("Error: " + exception.Message);
            }
        }

        protected override void AddNotNullCheckConstraint(DBMigrationsSetDefaultValue dbMigrationsSetDefaultValue)
        {
            string databaseType = dbMigrationsSetDefaultValue.DatabaseType;
            string schemaName = dbMigrationsSetDefaultValue.SchemaName;
            string tableName = dbMigrationsSetDefaultValue.TableName;
            string columnName = dbMigrationsSetDefaultValue.ColumnName;
            string checkConstraintName = "CK_NotNull_" + tableName + "_" + columnName;

            string queryString = "ALTER TABLE [" + schemaName + "].[" + tableName + "] WITH NOCHECK ADD CONSTRAINT [" + checkConstraintName + "] CHECK([" + columnName + "] IS NOT NULL);";

            string connectionString = GetZeroDownTimeConnectionString(ToolConfigurations.GetConnectionString(databaseType));
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            try
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = queryString;
                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
            catch (Exception exception)
            {
                sqlConnection.Close();
                ExitTool("Error: " + exception.Message);
            }
        }

        protected override string FormatDefaultValue(string defaultValue)
        {
            if(defaultValue.ToLower() == "CurrentDate".ToLower())
            {
                return "GETDATE()";
            }

            return defaultValue;
        }

        protected override List<DBMigrationsDataScript> GetDBMigrationsDataScripts(bool preScripts, List<string> statuses)
        {
            string statusesQueryString = string.Join(",", statuses.Select(s => "'" + s + "'").ToArray());
            string queryString = "SELECT * FROM [dbo].[DBMigrationsDataScripts] WHERE [Status] IN (" + statusesQueryString + ") AND [IsPreSxml] = " + (preScripts ? "1" : "0") + " ORDER BY [ScriptExecutionNumber]";
            
            List<DBMigrationsDataScript> dbMigrationsDataScripts = new List<DBMigrationsDataScript>();

            SqlDataReader reader = null;
            string connectionString = GetZeroDownTimeConnectionString(ToolConfigurations.MainConnectionString);
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(queryString, connection);

            try
            {
                connection.Open();
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    DBMigrationsDataScript dbMigrationsDataScript = new DBMigrationsDataScript
                    {
                        Id = reader["Id"].ToString(),
                        DatabaseType = reader["DatabaseType"].ToString(),
                        SxmlFileName = reader["SxmlFileName"].ToString(),
                        SxmlScript = reader["SxmlScript"].ToString(),
                        ScriptExecutionNumber = Convert.ToInt32(reader["ScriptExecutionNumber"].ToString()),
                        ScriptVersion = Convert.ToInt32(reader["ScriptVersion"].ToString()),
                        ScriptHashValue = reader["ScriptHashValue"].ToString(),
                        ScriptHistoryAction = reader["ScriptHistoryAction"].ToString(),
                        TargetTableName = reader["TargetTableName"].ToString(),
                        BatchSize = String.IsNullOrEmpty(reader["BatchSize"].ToString()) ? 1000 : Convert.ToInt32(reader["BatchSize"].ToString())
                    };
                    dbMigrationsDataScripts.Add(dbMigrationsDataScript);
                }

                reader.Close();
                connection.Close();
            }
            catch (Exception exception)
            {
                if (reader != null)
                {
                    reader.Close();
                }
                connection.Close();

                ExitTool("Error: " + exception.Message);
            }

            return dbMigrationsDataScripts;
        }

        protected override void UpdateDBMigrationsDataScript(string dbMigrationsDataScriptId, string property, string value)
        {
            string queryString = "UPDATE [dbo].[DBMigrationsDataScripts] SET [" + property + "] = '" + value + "' WHERE [Id] = '" + dbMigrationsDataScriptId + "';";

            string connectionString = GetZeroDownTimeConnectionString(ToolConfigurations.MainConnectionString);
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            try
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = queryString;
                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
            catch (Exception exception)
            {
                sqlConnection.Close();
                ExitTool("Error: " + exception.Message);
            }
        }
        
        protected override void ExecuteScriptAsBatches(DBMigrationsDataScript dbMigrationsDataScript)
        {
            int exceptionSleep = 30000;
            int retryNumber = 0;
            int batchNumberCounter = 1;
            int affectedRows = 1;
            int scriptExecutionNumber = dbMigrationsDataScript.ScriptExecutionNumber;
            int batchSize = dbMigrationsDataScript.BatchSize;
            string dbMigrationsDataScriptId = dbMigrationsDataScript.Id;
            string databaseType = dbMigrationsDataScript.DatabaseType;
            string targetTableName = dbMigrationsDataScript.TargetTableName;
            string queryString = dbMigrationsDataScript.SxmlScript;
            //string csvFileName = dbMigrationsDataScript.SxmlFileName.Replace(".sxml", String.Empty) + "_" + DateTime.Now.Ticks.ToString();

            queryString = Regex.Replace(queryString, "[$]LastCounterWhere[$]", targetTableName + ".[Id] IN (SELECT Id from @IdsTable)", RegexOptions.IgnoreCase);
            queryString = "DECLARE @IdsTable TABLE (Id VARCHAR(MAX));\n" +
                          "INSERT INTO @IdsTable SELECT TOP(" + batchSize.ToString() + ") [Id] FROM [" + targetTableName + "] WHERE [DBMigrationsLastScript] <= " + (scriptExecutionNumber - 1).ToString() +
                          " OR [DBMigrationsLastScript] IS NULL;\n" +
                          queryString + "\n" +
                          "UPDATE [" + targetTableName + "] SET [DBMigrationsLastScript] = " + scriptExecutionNumber.ToString() + " WHERE [Id] IN (SELECT Id FROM @IdsTable);\n" +
                          "DELETE FROM @IdsTable;";

            string connectionString = GetZeroDownTimeConnectionString(ToolConfigurations.GetConnectionString(databaseType));
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            while (affectedRows > 0)
            {
                try
                {
                    sqlConnection.Open();
                    SqlCommand sqlCommand = new SqlCommand();
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = queryString;
                    sqlCommand.CommandTimeout = ToolConfigurations.AOTScriptsExecutionTimeOut;
                    DateTime batchStartTime = DateTime.Now;
                    affectedRows = sqlCommand.ExecuteNonQuery();
                    DateTime batchEndTime = DateTime.Now;
                    sqlConnection.Close();

                    int elapsedTime = (int)(batchEndTime - batchStartTime).TotalMilliseconds;

                    if (affectedRows >= batchSize)
                    {
                        UpdateDBMigrationsDataScript(dbMigrationsDataScriptId, "LastBatchElapsedTime", elapsedTime.ToString());
                    }

                    //string data = batchNumberCounter.ToString() + "," + batchStartTime.ToString() + "," + batchEndTime.ToString() + "," + elapsedTime.ToString() + "," + affectedRows.ToString() + "\n";
                    //AppendToCSVFile(csvFileName, data);

                    batchNumberCounter++;
                    retryNumber = 0;
                    Thread.Sleep(500);
                }
                catch (SqlException exception)
                {
                    sqlConnection.Close();

                    if (exception.Number == -2)
                    {
                        retryNumber++;
                        if (retryNumber <= 10)
                        {
                            Thread.Sleep(exceptionSleep * retryNumber);
                        }
                        else
                        {
                            SendEmailsForDataScriptTimeout(dbMigrationsDataScript);
                            ExitTool("Error: " + exception.Message);
                        }
                    }
                    else
                    {
                        ExitTool("Error: " + exception.Message);
                    }
                }
            }
        }

        protected override void InsertIntoDBScriptsHistory(DBMigrationsDataScript dbMigrationsDataScript)
        {
            string queryString;
            string databaseType = dbMigrationsDataScript.DatabaseType;
            string sxmlFileName = dbMigrationsDataScript.SxmlFileName;
            string scriptBody = dbMigrationsDataScript.SxmlScript;
            string hashValue = dbMigrationsDataScript.ScriptHashValue;
            int version = dbMigrationsDataScript.ScriptVersion;
            string scriptHistoryAction = dbMigrationsDataScript.ScriptHistoryAction;
            DateTime startDate = dbMigrationsDataScript.StartDate.Value;
            DateTime endDate = dbMigrationsDataScript.EndDate.Value;
            int elapsedTime = (int)(endDate - startDate).TotalMilliseconds;

            if (scriptHistoryAction == "Insert")
            {
                queryString = "INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])" +
                    "VALUES('" + sxmlFileName + "', '" + startDate.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + scriptBody.Replace("'", "''") + "', " + elapsedTime.ToString() + ", '" + hashValue + "', " + version.ToString() + ");\n";
            }
            else
            {
                queryString = "UPDATE [dbo].[DBScriptsHistory] SET [ExecutionDate] = '" + startDate.ToString("yyyy-MM-dd HH:mm:ss") + "', [ScriptBody] = '" + scriptBody.Replace("'", "''") +
                    "', [ElapsedTimeInMs] = " + elapsedTime.ToString() + ", [HashValue] = '" + hashValue + "', [Version] = " + version.ToString() + " WHERE [SxmlFileName] = '" + sxmlFileName + "';\n";
            }

            string connectionString = GetZeroDownTimeConnectionString(ToolConfigurations.GetConnectionString(databaseType));
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            try
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = queryString;
                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
            catch (Exception exception)
            {
                sqlConnection.Close();
                ExitTool("Error: " + exception.Message);
            }
        }

        protected void AppendToCSVFile(string csvFileName, string data)
        {
            string reportsDirectoryPath = @"C:\Users\AbedMalakh\Desktop\ZeroDownTimeExecuteScriptsReports";
            if (Directory.Exists(reportsDirectoryPath))
            {
                string csvFilePath = reportsDirectoryPath + @"\" + csvFileName + ".csv";
                if (!File.Exists(csvFilePath))
                {
                    string dataHeader = "Batch Number,Start Time,End Time,Elapsed Time(ms),Affected Rows\n";
                    File.WriteAllText(csvFilePath, dataHeader);
                }

                File.AppendAllText(csvFilePath, data);
            }
        }

        protected override void CreateDBMigrationsLastDefaultValueColumn(string databaseType, string tableName)
        {
            string indexOnlineOption = ToolConfigurations.AOTCreateIndexWithOnline ? " WITH (ONLINE = ON)" : null;

            string queryString = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + tableName + "' AND COLUMN_NAME = 'DBMigrationsLastDefaultValue')\n" +
                                 "BEGIN\n" +
                                 "ALTER TABLE [" + tableName + "] ADD [DBMigrationsLastDefaultValue] INT NULL;\n" +
                                 "CREATE NONCLUSTERED INDEX [IX_" + tableName + "_DBMigrationsLastDefaultValue] ON [" + tableName + "]([DBMigrationsLastDefaultValue])" + indexOnlineOption + ";\n" +
                                 "END";

            string connectionString = GetZeroDownTimeConnectionString(ToolConfigurations.GetConnectionString(databaseType));
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            try
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = queryString;
                sqlCommand.CommandTimeout = 7200;
                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
            catch (Exception exception)
            {
                sqlConnection.Close();
                ExitTool("Error: " + exception.Message);
            }
        }

        protected override void CreateDBMigrationsLastScriptColumn(string databaseType, string tableName)
        {
            string indexOnlineOption = ToolConfigurations.AOTCreateIndexWithOnline ? " WITH (ONLINE = ON)" : null;

            string queryString = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + tableName + "' AND COLUMN_NAME = 'DBMigrationsLastScript')\n" +
                                 "BEGIN\n" +
                                 "ALTER TABLE [" + tableName + "] ADD [DBMigrationsLastScript] INT NULL;\n" +
                                 "CREATE NONCLUSTERED INDEX [IX_" + tableName + "_DBMigrationsLastScript] ON [" + tableName + "]([DBMigrationsLastScript])" + indexOnlineOption + ";\n" +
                                 "END";

            string connectionString = GetZeroDownTimeConnectionString(ToolConfigurations.GetConnectionString(databaseType));
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            try
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = queryString;
                sqlCommand.CommandTimeout = 7200;
                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
            catch (Exception exception)
            {
                sqlConnection.Close();
                ExitTool("Error: " + exception.Message);
            }
        }

        protected override void CreateResetLastScriptTrigger(string databaseType, string tableName)
        {
            string queryString = "IF NOT EXISTS (SELECT * FROM sys.objects WHERE [type] = 'TR' and [name] = 'TR_ResetLastScript_" + tableName + "')\n" +
                                 "BEGIN\n" +
                                 "EXEC('CREATE TRIGGER [TR_ResetLastScript_" + tableName + "]\n" +
                                 "ON [" + tableName + "]\n" +
                                 "FOR UPDATE AS BEGIN\n" +
                                 "SET NOCOUNT ON;\n" +
                                 "IF TRIGGER_NESTLEVEL() > 1 RETURN;\n" +
                                 "IF (SELECT program_name FROM sys.dm_exec_sessions WHERE session_id = (SELECT @@SPID)) <> ''ZeroDownTimeDBMigrationsTool''\n" +
                                 "BEGIN\n" +
                                 "UPDATE [" + tableName + "] SET [DBMigrationsLastScript] = NULL WHERE Id IN (SELECT DISTINCT Id FROM Inserted);\n" +
                                 "END\n" +
                                 "END');\n" +
                                 "END";

            string connectionString = GetZeroDownTimeConnectionString(ToolConfigurations.GetConnectionString(databaseType));
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            try
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = queryString;
                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
            catch (Exception exception)
            {
                sqlConnection.Close();
                ExitTool("Error: " + exception.Message);
            }
        }

        protected string GetZeroDownTimeConnectionString(string connectionString)
        {
            string zeroDownTimeConnectionString = (connectionString.EndsWith(";") ? connectionString : connectionString + ";") + "Application Name=ZeroDownTimeDBMigrationsTool;";
            return zeroDownTimeConnectionString;
        }
    }
}