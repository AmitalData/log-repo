
using Devart.Data.Oracle;
using Devart.Data.Oracle.Entity;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Simplog.Server.Infrastructure
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


        public static DbConnection GetConnection(string dbConnectionInfo, int? connectionLifetime = null, bool? suppressPool = null)
        {


            // Specify the provider name, server and database.
            //string providerName = "System.Data.SqlClient";

            //
            //if (dbConnectionInfo.Contains("Global"))
            //{

            //    return GetSQLServerConnecttion(dbConnectionInfo);
            //}
            //else
            //{



            //#if ORACLE_DB
            //             Console.WriteLine("oracle data");

            //             var config = Devart.Data.Oracle.Entity.Configuration.OracleEntityProviderConfig.Instance;
            //                config.Workarounds.ColumnTypeCasingConventionCompatibility = true; //if 
            //                config.Workarounds.DisableQuoting = true;
            //                config.QueryOptions.UseCSharpNullComparisonBehavior = true;
            //                // Apply the IgnoreSchemaName workaround
            //                config.Workarounds.IgnoreSchemaName = true;
            //                config.CodeFirstOptions.TruncateLongDefaultNames = true;
            //                config.QueryOptions.CaseInsensitiveComparison = true;
            //                config.QueryOptions.CaseInsensitiveLike = true;
            //                Devart.Data.Oracle.Entity.OracleEntityProviderServices.HandleNullStringsAsEmptyStrings = false;  


            //              if (dbConnectionInfo.Contains("Global"))
            //            {

            //                DbConnection con = new Devart.Data.Oracle.OracleConnection("User Id=global;  Password=global;Direct=True;Data Source=localhost;port=1521;sid=xe");
            //                return con;
            //               // return GetSQLServerConnecttion("Oracle_Global,sa,Saas256,.");
            //            }
            //            else
            //            {

            //                DbConnection con = new Devart.Data.Oracle.OracleConnection("User Id=logitude7;  Password=logitude7;Direct=True;Data Source=localhost;port=1521;sid=xe");
            //                return con;
            //            }


            //#else
            //            Console.WriteLine("sql data");
            //            return GetSQLServerConnecttion(dbConnectionInfo);
            //#endif


            if (LogitudeSettings.DatabaseManagementSystem == "oracle")
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

            DbConnection con = new Devart.Data.Oracle.OracleConnection(connString);
            return con;
        }

        private static string LifeTime(int? connectionLifetime, OracleConnectionStringBuilder main_ocsb)
        {
            /*
            Connection Lifetime
            -or-
            Load Balance Timeout
            0
            When a connection is returned to the pool, its creation time is compared with the current time, and the connection is destroyed if that time span (in seconds) exceeds the value specified by Connection Lifetime. This is useful in clustered configurations to force load balancing between a running server and a server just brought online.

            A value of zero (0) causes pooled connections to have the maximum connection timeout.                 

                         */
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

        private static DbConnection GetSQLServerConnecttion(string dbConnectionInfo,int? connectionLifetime = null)
        {
            string[] information = dbConnectionInfo.Split(',');
            string databaseName = information[0];
            string userName = information[1];
            string pass = information[2];
            //< add name = "Globalstr" connectionString = "Logitude2-5_Global,sa,Saas256!,10.10.10.48,49172\ITZIK" />
            //< add name = "Globalstr" connectionString = "Logitude2-5_Global,sa,Saas256!,10.10.10.48:49172\ITZIK" />
            //< add name = "Globalstr" connectionString = "Logitude2-5_Global,sa,Saas256!,servername:port\Instance" />
            string servername = information[3].Replace(":",",");
            string applicationIntent = information.Length > 4 ? information[4] : "";
            // Initialize the connection string builder for the
            // underlying provider.
            SqlConnectionStringBuilder sqlBuilder =
                new SqlConnectionStringBuilder();

            // Set the properties for the data source.
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
            //sqlBuilder.ConnectTimeout = 240;
            // Build the SqlConnection connection string.
            string providerString = sqlBuilder.ToString();

            // Initialize the EntityConnectionStringBuilder.
            EntityConnectionStringBuilder entityBuilder =
                new EntityConnectionStringBuilder();

            DbConnection connection = new SqlConnection(providerString);
            //if (WebFreightEntryPoint.CheckConnectionStrategy)
            //{
            //    DateTime checkDate = DateTime.UtcNow;
            //    DateTime endCheckDate = WebFreightEntryPoint.CheckConnectionStartDate.AddMinutes(5);
            //    if (checkDate <= endCheckDate)
            //    {
            //        RetryConnectionClass.CheckConnection(connection);
            //    }
            //    else
            //    {
            //        WebFreightEntryPoint.CheckConnectionStrategy = false;
            //    }
            //}
            return connection;
        }

       


    }


}
