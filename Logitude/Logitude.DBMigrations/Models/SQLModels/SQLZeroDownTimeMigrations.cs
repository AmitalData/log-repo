using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
            SqlConnection connection = new SqlConnection(ToolConfigurations.MainConnectionString);
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

                ExitZeroDownTimeMigrations(exception.Message);
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
                "[DBMigrationsLastDefaultValue] = " + updateNumber.ToString() + " WHERE [DBMigrationsLastDefaultValue] = " + (updateNumber - 1).ToString() + ";";

            SqlConnection sqlConnection = new SqlConnection(ToolConfigurations.GetConnectionString(databaseType));

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

                    Thread.Sleep(1000);

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
                }
                catch (Exception exception)
                {
                    sqlConnection.Close();
                    ExitZeroDownTimeMigrations(exception.Message + "\nError Details: " + exception.ToString());
                }
            }
        }
        
        protected override void UpdateDBMigrationsSetDefaultValue(string dbMigrationsSetDefaultValueId, string property, string value)
        {
            string queryString = "UPDATE [dbo].[DBMigrationsSetDefaultValues] SET [" + property + "] = '" + value + "' WHERE [Id] = '" + dbMigrationsSetDefaultValueId + "';";

            SqlConnection sqlConnection = new SqlConnection(ToolConfigurations.MainConnectionString);

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
                ExitZeroDownTimeMigrations(exception.Message);
            }
        }

        protected override void AddNotNullCheckConstraint(DBMigrationsSetDefaultValue dbMigrationsSetDefaultValue)
        {
            string databaseType = dbMigrationsSetDefaultValue.DatabaseType;
            string schemaName = dbMigrationsSetDefaultValue.SchemaName;
            string tableName = dbMigrationsSetDefaultValue.TableName;
            string columnName = dbMigrationsSetDefaultValue.ColumnName;
            string checkConstraintName = "CK_" + columnName + "_NotNull";

            string queryString = "ALTER TABLE [" + schemaName + "].[" + tableName + "] WITH NOCHECK ADD CONSTRAINT [" + checkConstraintName + "] CHECK([" + columnName + "] IS NOT NULL);";

            SqlConnection sqlConnection = new SqlConnection(ToolConfigurations.GetConnectionString(databaseType));

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
                ExitZeroDownTimeMigrations(exception.Message);
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

        protected override List<DBMigrationsDataScript> GetDBMigrationsDataScripts(bool preScripts)
        {
            string queryString = "SELECT * FROM [dbo].[DBMigrationsDataScripts] WHERE [Status] <> 'Done' AND [IsPreSxml] = " + (preScripts ? "1" : "0") + " ORDER BY [ScriptExecutionNumber]";

            List<DBMigrationsDataScript> dbMigrationsDataScripts = new List<DBMigrationsDataScript>();

            SqlDataReader reader = null;
            SqlConnection connection = new SqlConnection(ToolConfigurations.MainConnectionString);
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
                        ScriptHistoryAction = reader["ScriptHistoryAction"].ToString()
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

                ExitZeroDownTimeMigrations(exception.Message);
            }

            return dbMigrationsDataScripts;
        }

        protected override void UpdateDBMigrationsDataScript(string dbMigrationsDataScriptId, string property, string value)
        {
            string queryString = "UPDATE [dbo].[DBMigrationsDataScripts] SET [" + property + "] = '" + value + "' WHERE [Id] = '" + dbMigrationsDataScriptId + "';";

            SqlConnection sqlConnection = new SqlConnection(ToolConfigurations.MainConnectionString);

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
                ExitZeroDownTimeMigrations(exception.Message);
            }
        }

        protected override void ExecuteScriptAsBatches(DBMigrationsDataScript dbMigrationsDataScript)
        {
            int affectedRows = 1;
            int scriptExecutionNumber = dbMigrationsDataScript.ScriptExecutionNumber;
            string dbMigrationsDataScriptId = dbMigrationsDataScript.Id;
            string databaseType = dbMigrationsDataScript.DatabaseType;
            string queryString = dbMigrationsDataScript.SxmlScript;
            queryString = Regex.Replace(queryString, "[$]Top[$]", "TOP(1000)", RegexOptions.IgnoreCase);
            queryString = Regex.Replace(queryString, "[$]LastCounter[$]", ("[DBMigrationsLastScript] = " + scriptExecutionNumber.ToString()), RegexOptions.IgnoreCase);
            queryString = Regex.Replace(queryString, "[$]BatchWhere[$]", ("[DBMigrationsLastScript] = " + (scriptExecutionNumber - 1).ToString()), RegexOptions.IgnoreCase);

            SqlConnection sqlConnection = new SqlConnection(ToolConfigurations.GetConnectionString(databaseType));

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

                    Thread.Sleep(1000);

                    if (affectedRows >= 1000)
                    {
                        int elapsedTime = (int)(batchEndTime - batchStartTime).TotalMilliseconds;
                        UpdateDBMigrationsDataScript(dbMigrationsDataScriptId, "LastBatchElapsedTime", elapsedTime.ToString());
                    }
                }
                catch (Exception exception)
                {
                    sqlConnection.Close();
                    ExitZeroDownTimeMigrations(exception.Message + "\nError Details: " + exception.ToString());
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
                    "VALUES('" + sxmlFileName + "', '" + startDate.ToString() + "', '" + scriptBody.Replace("'", "''") + "', " + elapsedTime.ToString() + ", '" + hashValue + "', " + version.ToString() + ");\n";
            }
            else
            {
                queryString = "UPDATE [dbo].[DBScriptsHistory] SET [ExecutionDate] = '" + startDate.ToString() + "', [ScriptBody] = '" + scriptBody.Replace("'", "''") +
                    "', [ElapsedTimeInMs] = " + elapsedTime.ToString() + ", [HashValue] = '" + hashValue + "', [Version] = " + version.ToString() + " WHERE [SxmlFileName] = '" + sxmlFileName + "';\n";
            }

            SqlConnection sqlConnection = new SqlConnection(ToolConfigurations.GetConnectionString(databaseType));

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
                ExitZeroDownTimeMigrations(exception.Message);
            }
        }
    }
}