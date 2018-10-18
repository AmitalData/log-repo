using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Simplog.Global.Data.GlobalModel;
using Simplog.Server.Infrastructure;
using Devart.Data.Oracle;

namespace Logitude.Server.Tools.Counters
{
    public class TenantCounter
    {
        public static int GetNumber()
        {
            int number = 0;

            string dbConnectionInfo = "";
            if (LogitudeSettings.DatabaseManagementSystem == "oracle")
            {
                dbConnectionInfo = ConfigurationManager.ConnectionStrings["Oracle_Globalstr"].ConnectionString;

            }
            else
            {
                dbConnectionInfo = ConfigurationManager.ConnectionStrings["Globalstr"].ConnectionString;
            }

            string strConnString = GetConnection(dbConnectionInfo);

            if (LogitudeSettings.DatabaseManagementSystem == "oracle")
            {
                using (OracleConnection cn = new OracleConnection(strConnString))
                {
                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = cn;
                    cmd.CommandText = 
                        //LogitudeDBSchema.LOGITUDE_GLOBAL.ToString() + "." + "GetNextGlobalTenantId";
                        DbContextBaseUtil.GetStoredProcedureName("GetNextGlobalTenantId", LogitudeDBSchema.LOGITUDE_GLOBAL,
                        cmd.Connection.ConnectionString);
                    cmd.CommandType = CommandType.StoredProcedure;


                    OracleParameter lastNumberPar = new OracleParameter("v_pLastNumber", OracleDbType.Integer);

                    lastNumberPar.Direction = ParameterDirection.Output;

                    cmd.Parameters.Add(lastNumberPar);

                    cn.Open();
                    cmd.ExecuteNonQuery();
                    cn.Close();
                    number = (int)cmd.Parameters["v_pLastNumber"].Value;
                }

                return number;

            }
            else
            {
                using (SqlConnection cn = new SqlConnection(strConnString))
                {
                    SqlCommand cmd = new SqlCommand("GetNextGlobalTenantId", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter lastNumberPar = new SqlParameter("@pLastNumber", SqlDbType.Int);

                    lastNumberPar.Direction = ParameterDirection.Output;

                    cmd.Parameters.Add(lastNumberPar);

                    cn.Open();
                    cmd.ExecuteNonQuery();
                    cn.Close();
                    number = (int)cmd.Parameters["@pLastNumber"].Value;

                }
            }
            return number;
        }


        public static string GetConnection(string dbConnectioninfo)
        {

            DbConnection connection = DatabaseInitializer.GetConnection(dbConnectioninfo);
            GlobalContext context = new GlobalContext(connection);

            return context.Database.Connection.ConnectionString;// entityBuilder.ConnectionString;
        }
        //public static string GetConnection(string dbConnectioninfo)
        //{

        //    string dbConnectionInfo = dbConnectioninfo;

        //    // Specify the provider name, server and database.
        //    string providerName = "System.Data.SqlClient";
        //    string serverName = ".";
        //    string databaseName = "";


        //    string[] information = dbConnectionInfo.Split(',');
        //    string dbName = information[0];
        //    string userName = information[1];
        //    string pass = information[2];
        //    string servername = information[3];
        //    SqlConnectionStringBuilder sqlBuilder =
        //        new SqlConnectionStringBuilder();

        //    // Set the properties for the data source.
        //    sqlBuilder.DataSource = servername;
        //    sqlBuilder.InitialCatalog = dbName;
        //    sqlBuilder.IntegratedSecurity = false;
        //    //sqlBuilder.PersistSecurityInfo = true;
        //    sqlBuilder.Password = pass;
        //    sqlBuilder.UserID = userName;
        //    sqlBuilder.MultipleActiveResultSets = true;
        //    // Build the SqlConnection connection string.
        //    string providerString = sqlBuilder.ToString();

        //    // Initialize the EntityConnectionStringBuilder.
        //    //EntityConnectionStringBuilder entityBuilder =
        //    //    new EntityConnectionStringBuilder();

        //    //Set the provider name.
        //    //entityBuilder.Provider = providerName;

        //    // Set the provider-specific connection string.
        //    //entityBuilder.ConnectionString = providerString;
        //    //entityBuilder.Metadata = string.Format(@"res://*/{0}.csdl|res://*/{0}.ssdl|res://*/{0}.msl",
        //    //    "CommonDataModel.CommonDataModel");

        //    // EntityConnection connection = new EntityConnection(entityBuilder.ConnectionString);//(entityBuilder.ConnectionString);


        //    return providerString;// entityBuilder.ConnectionString;
        //}

    }
}