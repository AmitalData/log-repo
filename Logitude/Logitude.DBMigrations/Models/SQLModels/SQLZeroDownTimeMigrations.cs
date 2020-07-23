using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Logitude.DBMigrations.Models
{
    public class SQLZeroDownTimeMigrations : ZeroDownTimeMigrations
    {
        protected override List<ZeroDownTimeDefaultValueMigration> GetDefaultValueMigrations()
        {
            string queryString = "SELECT * FROM [dbo].[DBMigrationsSetDefaultValues] WHERE [Status] <> 'Done'";

            List<ZeroDownTimeDefaultValueMigration> defaultValueMigrations = new List<ZeroDownTimeDefaultValueMigration>();

            SqlDataReader reader = null;
            SqlConnection connection = new SqlConnection(ToolConfigurations.MainConnectionString);
            SqlCommand command = new SqlCommand(queryString, connection);

            try
            {
                connection.Open();
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ZeroDownTimeDefaultValueMigration defaultValueMigration = new ZeroDownTimeDefaultValueMigration
                    {
                        Id = reader["Id"].ToString(),
                        DatabaseType = reader["DatabaseType"].ToString(),
                        SchemaName = reader["SchemaName"].ToString(),
                        TableName = reader["TableName"].ToString(),
                        ColumnName = reader["ColumnName"].ToString(),
                        DefaultValue = reader["DefaultValue"].ToString(),
                        UpdateNumber = Convert.ToInt32(reader["UpdateNumber"].ToString())
                    };
                    defaultValueMigrations.Add(defaultValueMigration);
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

            return defaultValueMigrations.OrderBy(d => d.UpdateNumber).ToList();
        }

        protected override void SetDefaultValues(ZeroDownTimeDefaultValueMigration defaultValueMigration)
        {
            string queryString = "UPDATE TOP(1000) [" + defaultValueMigration.SchemaName + "].[" + defaultValueMigration.TableName + "] SET [" + defaultValueMigration.ColumnName + "] = " + FormatDefaultValue(defaultValueMigration.DefaultValue) + ", " +
                "[DBMigrationsLastDefaultValue] = " + defaultValueMigration.UpdateNumber + " WHERE [DBMigrationsLastDefaultValue] = " + (defaultValueMigration.UpdateNumber - 1) + ";";

            SqlConnection sqlConnection = new SqlConnection(ToolConfigurations.GetConnectionString(defaultValueMigration.DatabaseType));
            
            try
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = queryString;
                DateTime batchStartTime = DateTime.Now;
                int updatedRows = sqlCommand.ExecuteNonQuery();
                DateTime batchEndTime = DateTime.Now;
                sqlConnection.Close();

                Thread.Sleep(1000);

                if (updatedRows != 0)
                {
                    int elapsedTime = batchEndTime.Subtract(batchStartTime).Milliseconds;
                    defaultValueMigration.DoneRecordsCount += updatedRows;
                    UpdateDefaultValueMigration(defaultValueMigration.Id, "DoneRecordsCount", defaultValueMigration.DoneRecordsCount.ToString());
                    if(updatedRows >= 1000)
                    {
                        UpdateDefaultValueMigration(defaultValueMigration.Id, "LastBatchElapsedTime", elapsedTime.ToString());
                    }
                    SetDefaultValues(defaultValueMigration);
                }
            }
            catch (Exception exception)
            {
                sqlConnection.Close();
                ExitZeroDownTimeMigrations(exception.Message);
            }
        }

        protected override void UpdateDefaultValueMigration(string defaultValueMigrationId, string property, string value)
        {
            string queryString = "UPDATE [dbo].[DBMigrationsSetDefaultValues] SET [" + property + "] = '" + value + "' WHERE [Id] = '" + defaultValueMigrationId + "';";

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

        protected override void UnsetColumnNullable(ZeroDownTimeDefaultValueMigration defaultValueMigration)
        {
            string queryString = "DECLARE @columnType VARCHAR(500); " +
                                 "DECLARE @sql VARCHAR(1000); " +
                                 "SELECT @columnType = " +
                                 "CASE WHEN CHARACTER_MAXIMUM_LENGTH > 0 " +
                                 "THEN DATA_TYPE + '(' + CAST(CHARACTER_MAXIMUM_LENGTH AS VARCHAR(100)) + ')' " +
                                 "WHEN CHARACTER_MAXIMUM_LENGTH = -1 AND DATA_TYPE <> 'xml' " +
                                 "THEN DATA_TYPE + '(MAX)' " +
                                 "WHEN DATA_TYPE IN ('numeric', 'decimal') " +
                                 "THEN DATA_TYPE + '(' + CAST(NUMERIC_PRECISION AS VARCHAR(100)) + ',' + CAST(NUMERIC_SCALE AS VARCHAR(100)) + ')' " +
                                 "ELSE DATA_TYPE " +
                                 "END " +
                                 "FROM INFORMATION_SCHEMA.COLUMNS " +
                                 "WHERE TABLE_SCHEMA = '" + defaultValueMigration.SchemaName + "' AND TABLE_NAME = '" + defaultValueMigration.TableName + "' AND COLUMN_NAME = '" + defaultValueMigration.ColumnName + "'" +
                                 "SET @sql = 'ALTER TABLE ' + '[" + defaultValueMigration.SchemaName + "]' + '.' + '[" + defaultValueMigration.TableName + "]' + ' ALTER COLUMN ' + '[" + defaultValueMigration.ColumnName + "]' + ' ' + @columnType + ' NOT NULL;'" +
                                 "EXEC(@sql);";

            SqlConnection sqlConnection = new SqlConnection(ToolConfigurations.GetConnectionString(defaultValueMigration.DatabaseType));

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
        
        protected override void AddColumnDefaultValue(ZeroDownTimeDefaultValueMigration defaultValueMigration)
        {
            string queryString = "ALTER TABLE [" + defaultValueMigration.SchemaName + "].[" + defaultValueMigration.TableName + "] ADD DEFAULT " +
                                 FormatDefaultValue(defaultValueMigration.DefaultValue) + " FOR [" + defaultValueMigration.ColumnName + "];";

            SqlConnection sqlConnection = new SqlConnection(ToolConfigurations.GetConnectionString(defaultValueMigration.DatabaseType));

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
    }
}