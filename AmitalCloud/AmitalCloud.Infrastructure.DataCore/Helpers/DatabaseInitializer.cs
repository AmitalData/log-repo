
using Devart.Data.Oracle;
using System;
using System.Data.Common;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;

namespace AmitalCloud.Infrastructure.Data.Helpers
{
    public class DatabaseInitializer
    {
        [ThreadStatic] public static bool RunOnSeconderyDB = false;
        public static DbConnection GetConnection(string dbConnectionInfo, string SeconderyDBConnectionInfo)//SeconderyDBConnectionInfo = null
        {
            if (RunOnSeconderyDB == true && !string.IsNullOrEmpty(SeconderyDBConnectionInfo))
            {
                return GetConnection(SeconderyDBConnectionInfo, null, null);
            }
            else
            {
                return GetConnection(dbConnectionInfo, null, null);
            }
        }
        public static string GetConnectionString(string dbConnectionInfo, string SeconderyDBConnectionInfo)//SeconderyDBConnectionInfo = null
        {
            if (RunOnSeconderyDB == true && !string.IsNullOrEmpty(SeconderyDBConnectionInfo))
            {
                return GetConnectionString(SeconderyDBConnectionInfo, null, null);
            }
            else
            {
                return GetConnectionString(dbConnectionInfo, null, null);
            }
        }
        public static string GetConnectionString(string dbConnectionInfo, int? connectionLifetime = null, bool? suppressPool = null)
        {
            if (AmitalCloudSettings.DatabaseManagementSystem == "oracle")
            {
                return GetOracleConnectionString(dbConnectionInfo, connectionLifetime, suppressPool);
            }
            else
            {
                return GetSQLServerConnectionString(dbConnectionInfo, connectionLifetime);

            }
        }
        public static DbConnection GetConnection(string dbConnectionInfo, int? connectionLifetime = null, bool? suppressPool = null)
        {
            if (AmitalCloudSettings.DatabaseManagementSystem == "oracle")
            {
                return GeOracleDbConn(dbConnectionInfo, connectionLifetime, suppressPool);
            }
            else
            {
                return GetSQLServerConnecttion(dbConnectionInfo, connectionLifetime);
            }
        }

        private static DbConnection GeOracleDbConn(string dbConnectionInfo, int? connectionLifetime, bool? suppressPool)
        {
            string connString = GetOracleConnectionString(dbConnectionInfo, connectionLifetime, suppressPool);
            DbConnection con = new Devart.Data.Oracle.OracleConnection(connString);
            return con;
        }

        private static string GetOracleConnectionString(string dbConnectionInfo, int? connectionLifetime, bool? suppressPool)
        {
            string connString = dbConnectionInfo;
            bool bLifeTime = false;
            if (bLifeTime)
            {
                var main_ocsb = new OracleConnectionStringBuilder(connString);
                connString = LifeTime(connectionLifetime, main_ocsb);
            }
            bool suppressPool_FeatureIsON = true;
            if (suppressPool_FeatureIsON)
            {
                if (suppressPool.GetValueOrDefault())
                {
                    var main_ocsb = new OracleConnectionStringBuilder(connString);
                    main_ocsb.Pooling = false;
                    connString = main_ocsb.ConnectionString;
                }
            }
            return connString;
        }
        private static string LifeTime(int? connectionLifetime, OracleConnectionStringBuilder main_ocsb)
        {
            var connString = main_ocsb.ConnectionString;
            int iConnectionLifetime = (int)TimeSpan.FromHours(2).TotalSeconds;
            if (connectionLifetime.HasValue && connectionLifetime.Value > 0)
            {
                iConnectionLifetime = connectionLifetime.Value;
            }
            main_ocsb.ConnectionLifetime = iConnectionLifetime;//Sec
            connString = main_ocsb.ConnectionString;
            return connString;
        }
        private static DbConnection GetSQLServerConnecttion(string dbConnectionInfo, int? connectionLifetime = null)
        {
            string providerString = GetSQLServerConnectionString(dbConnectionInfo, connectionLifetime);
            return new SqlConnection(providerString);
        }

        private static string GetSQLServerConnectionString(string dbConnectionInfo, int? connectionLifetime = null)
        {
            string[] information = dbConnectionInfo.Split(',');
            string databaseName = information[0];
            string userName = information[1];
            string pass = information[2];
            string servername = information[3].Replace(":", ",");
            string applicationIntent = information.Length > 4 ? information[4] : "";
            SqlConnectionStringBuilder sqlBuilder =
                new SqlConnectionStringBuilder();
            sqlBuilder.DataSource = servername;
            sqlBuilder.InitialCatalog = databaseName;
            sqlBuilder.PersistSecurityInfo = true;
            sqlBuilder.IntegratedSecurity = false;
            sqlBuilder.Password = pass;
            sqlBuilder.UserID = userName;
            sqlBuilder.MultipleActiveResultSets = true;
            if (!string.IsNullOrEmpty(applicationIntent))
            {
                sqlBuilder.ApplicationIntent = applicationIntent == "ReadOnly" ? ApplicationIntent.ReadOnly : ApplicationIntent.ReadWrite;
            }
            if (connectionLifetime.HasValue && connectionLifetime.Value > 0)
            {
                sqlBuilder.ConnectTimeout = connectionLifetime.Value;
            }
            else
            {
                sqlBuilder.ConnectTimeout = 100; // old value was 60 . the 100 added by Rabaia with mohammad in order to fix Time Out Problem caused by Lock on get next counter "the update is temporary"
            }
            sqlBuilder.MaxPoolSize = 200;
            string providerString = sqlBuilder.ToString();
            EntityConnectionStringBuilder entityBuilder =
                new EntityConnectionStringBuilder();
            return providerString;
        }
    }
}
